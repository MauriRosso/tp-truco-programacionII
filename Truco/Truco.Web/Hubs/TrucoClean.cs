﻿using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Entidades;

namespace Truco.Web.Hubs
{
    [HubName("truco")]
    public class Truco : Hub
    {
        public static Partida juego = new Partida();

        public void AgregarJugador(string nombre)
        {
            Jugador Jugador = new Jugador();

            if (juego.CantidadJugadores == 4)
            {
                // Si el juego esta completo...
                Clients.Caller.mostrarmensaje("El juego ya está completo!");
            }
            if (juego.CantidadJugadores == 0 || juego.CantidadJugadores == 2) //juego.ListaJugadores.Count() >= 2
            {
                juego.CantidadJugadores += 1;
                Jugador.Nombre = nombre;
                Jugador.IdConexion = Context.ConnectionId;
                Jugador.NombreInterno = $"user{juego.ListaJugadores.Count() + 1}";
                Jugador.Orden = juego.ID + 1;
                Jugador.Turno = true;
                Jugador.Equipo = "Equipo1";
                juego.Equipo1.ListaJugadores.Add(Jugador);
                juego.ListaJugadores.Add(Jugador);
                Clients.Others.mostrarnuevousuario(nombre);
            }
            else
            {
                juego.CantidadJugadores += 1;
                Jugador.Nombre = nombre;
                Jugador.IdConexion = Context.ConnectionId;
                Jugador.NombreInterno = $"user{juego.ListaJugadores.Count() + 1}";
                Jugador.Orden = juego.ID + 1;
                Jugador.Turno = true;
                Jugador.Equipo = "Equipo2";
                juego.Equipo2.ListaJugadores.Add(Jugador);
                juego.ListaJugadores.Add(Jugador);
                Clients.Others.mostrarnuevousuario(nombre);
                if (juego.CantidadJugadores == 4)
                {
                    Clients.All.mostrarpuntos("Ellos", 0);
                    Clients.All.mostrarpuntos("Nosotros", 0);
                    juego.NumeroMano = 1;
                    juego.NumeroRonda = 0;
                    Repartir();
                    juego.ListaJugadores[0].Turno = true;
                    //Jugador jugadorConTurno = juego.ListaJugadores.Find(x => x.Turno);
                    //Clients.AllExcept(jugadorConTurno.IdConexion).hideEnvidoOptions();
                    //Clients.AllExcept(jugadorConTurno.IdConexion).hideTrucoBotton();
                    //Clients.All.hideReTrucoBotton();
                    //Clients.All.hideVale4Botton();
                }
            }

            foreach (var item in juego.ListaJugadores)
            {
                // Por cada jugador
                Clients.All.mostrarNombre(item);
            }

            // Si es el ultimo jugador...

        }

        private Jugador ObtenerJugador(string idConexion)
        {
            Jugador JugadorObtenido = null;
            foreach (var Jug in juego.ListaJugadores)
            {
                if (idConexion == Jug.IdConexion)
                {
                    JugadorObtenido = Jug;
                }
            }
            return JugadorObtenido;
        }

        public void Repartir()
        {
            Jugador jugador = ObtenerJugador(Context.ConnectionId);
            Clients.All.limpiarTablero();

            juego.RepartirCartas(4);
            juego.AsignarMano(juego.NumeroRonda);
            juego.CalcularPuntosEnvido();
            juego.MayorPuntos = 0;
            juego.ListaJugadores.OrderByDescending(x => x.Mano); //Cada vez que reparto, ordeno la lista en base a la mano.
            foreach (var item in juego.Equipo1.ListaJugadores)
            {
                Clients.Client(item.IdConexion).mostrarCartas(item.ListaCartas);
            }

            foreach (var item in juego.Equipo2.ListaJugadores)
            {
                Clients.Client(item.IdConexion).mostrarCartas(item.ListaCartas);
            }
            foreach (var item in juego.ListaJugadores)
            {
                if (item.Mano == 4)
                {
                    Clients.Client(item.IdConexion).habilitarMovimientos(); //Habilito el mov. del jug con mayor mano para empezar a jugar. 
                }
            }

            foreach (var item in juego.ListaJugadores)
            {
                item.Turno = false;
                item.CantoTruco = false;
            }

            foreach (var jugadorSeleccionado in juego.ListaJugadores)
            {
                Clients.Client(jugadorSeleccionado.IdConexion).showTrucoBotton();
                Clients.Client(jugadorSeleccionado.IdConexion).hideReTrucoBotton();
                Clients.Client(jugadorSeleccionado.IdConexion).hideVale4Botton();
                if (jugadorSeleccionado.Mano == 2 || jugadorSeleccionado.Mano == 1)
                {
                    Clients.Client(jugadorSeleccionado.IdConexion).showEnvidoBotton();
                    Clients.Client(jugadorSeleccionado.IdConexion).hideEnvidoEnvidoBotton();
                    Clients.Client(jugadorSeleccionado.IdConexion).showRealEnvidoBotton();
                    Clients.Client(jugadorSeleccionado.IdConexion).showFaltaEnvidoBotton();
                }
                else
                {
                    Clients.Client(jugadorSeleccionado.IdConexion).hideEnvidoOptions();
                }
            }
        }

        public Jugador AnteriorJugador(string idConexion)
        {
            var jugador = ObtenerJugador(idConexion);
            Jugador AnteriorJugador = null;

            for (int i = 0; i < juego.ListaJugadores.Count(); i++)
            {
                if (jugador.IdConexion == juego.ListaJugadores[i].IdConexion) //Encuentro al proximo jugador
                {
                    if (i - 1 < 0) // Garantizo que la lista sea circular, es decir que no se desborde.
                    {
                        AnteriorJugador = juego.ListaJugadores[3];
                    }
                    else
                    {
                        AnteriorJugador = juego.ListaJugadores[i - 1];
                    }
                    break;
                }
            }
            return AnteriorJugador;
        } //Metodo para buscar el proximo jugador de una lista, te lo saque del metodo 

        public Jugador ProximoJugador(string idConexion)
        {
            var jugador = ObtenerJugador(idConexion);
            Jugador ProximoJugador = null;

            for (int i = 0; i < juego.ListaJugadores.Count(); i++)
            {
                if (jugador.IdConexion == juego.ListaJugadores[i].IdConexion) //Encuentro al proximo jugador
                {
                    if (i + 1 > 3) // Garantizo que la lista sea circular, es decir que no se desborde.
                    {
                        ProximoJugador = juego.ListaJugadores[0];
                    }
                    else
                    {
                        ProximoJugador = juego.ListaJugadores[i + 1];
                    }
                    break;
                }
            }
            return ProximoJugador;
        } //Metodo para buscar el proximo jugador de una lista, te lo saque del metodo JugarCartas, creo que funciona no lo lleuge a probar

        public void cantar(string accion)
        {
            Jugador jugador = ObtenerJugador(Context.ConnectionId);
            Jugador proximoJugador = ProximoJugador(jugador.IdConexion);
            //Jugador jugadorCantoTruco = ObtenerJugador(Context.ConnectionId);
            jugador.Turno = true;
            proximoJugador.Turno = true;
            if (jugador.Turno)
            {
                Clients.Others.mostrarmensaje(jugador.Nombre + " cantó " + (accion == "envidoenvido" ? "doble envido" : accion));
                Clients.Caller.mostrarmensaje("Has cantado " + (accion == "envidoenvido" ? "doble envido" : accion));
                Clients.Client(jugador.IdConexion).deshabilitarMovimientos();

                if (juego.Equipo1.Puntos >= 30 || juego.Equipo2.Puntos >= 30)
                {
                    if (juego.Equipo1.Puntos >= 30)
                    {
                        Clients.Client(juego.Equipo1.ListaJugadores[0].IdConexion).mostrarMensajeFinal(true);
                        Clients.Client(juego.Equipo1.ListaJugadores[1].IdConexion).mostrarMensajeFinal(true);
                        Clients.Client(juego.Equipo2.ListaJugadores[0].IdConexion).mostrarMensajeFinal(false);
                        Clients.Client(juego.Equipo2.ListaJugadores[1].IdConexion).mostrarMensajeFinal(false);
                    }
                    else
                    {
                        Clients.Client(juego.Equipo2.ListaJugadores[0].IdConexion).mostrarMensajeFinal(true);
                        Clients.Client(juego.Equipo2.ListaJugadores[1].IdConexion).mostrarMensajeFinal(true);
                        Clients.Client(juego.Equipo1.ListaJugadores[0].IdConexion).mostrarMensajeFinal(false);
                        Clients.Client(juego.Equipo1.ListaJugadores[1].IdConexion).mostrarMensajeFinal(false);
                    }
                    Clients.All.deshabilitarMovimientos();
                }

                //// Sino

                //Clients.All.limpiarpuntos();

                //// Y mostrar puntos y repartir.
                switch (accion)
                {
                    case "me voy al mazo":
                        if (jugador.Equipo == "Equipo1")
                        {
                            juego.ListaCartasJugadas.Clear();
                            juego.ListaJugadores[0].ListaCartas.Clear();
                            juego.ListaJugadores[1].ListaCartas.Clear();
                            juego.ListaJugadores[2].ListaCartas.Clear();
                            juego.ListaJugadores[3].ListaCartas.Clear();
                            juego.Equipo1.ManoGanada = 0;
                            juego.NumeroMano = 1;
                            juego.CartasJugadas = 0;
                            juego.TerminoRonda = true;
                            juego.NumeroRonda += 1;
                            juego.Equipo2.Puntos++;
                        }
                        else
                        {
                            juego.ListaCartasJugadas.Clear();
                            juego.ListaJugadores[0].ListaCartas.Clear();
                            juego.ListaJugadores[1].ListaCartas.Clear();
                            juego.ListaJugadores[2].ListaCartas.Clear();
                            juego.ListaJugadores[3].ListaCartas.Clear();
                            juego.Equipo1.ManoGanada = 0;
                            juego.NumeroMano = 1;
                            juego.CartasJugadas = 0;
                            juego.TerminoRonda = true;
                            juego.NumeroRonda += 1;
                            juego.Equipo1.Puntos++;
                        }
                        Clients.All.limpiarpuntos();
                        foreach (var jugadorSeleccionado in juego.ListaJugadores)
                        {
                            
                            if (jugadorSeleccionado.Equipo == "Equipo1")
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo1.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo2.Puntos);
                            }
                            else
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo2.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo1.Puntos);
                            }
                        }

                        Repartir();
                        break;

                    case "envido":
                        Clients.All.hidemazo();
                        Clients.Client(jugador.IdConexion).hideEnvidoBotton();
                        Clients.Client(jugador.IdConexion).hideEnvidoOptions();
                        Clients.Client(proximoJugador.IdConexion).showEnvidoOptions();
                        Clients.All.hideTrucoBotton();
                        break;

                    case "envidoenvido":
                        Clients.All.hidemazo();
                        Clients.Client(jugador.IdConexion).hideEnvidoBotton();
                        Clients.Client(proximoJugador.IdConexion).showEnvidoEnvidoOptions();
                        Clients.Client(jugador.IdConexion).hideEnvidoEnvidoOptions();
                        Clients.All.hideTrucoBotton();
                        break;

                    case "faltaenvido":
                        Clients.All.hidemazo();
                        Clients.Client(jugador.IdConexion).hideEnvidoBotton();
                        Clients.Client(proximoJugador.IdConexion).showFaltaEnvidoOptions();
                        Clients.Client(jugador.IdConexion).hideFaltaEnvidoOptions();
                        Clients.All.hideTrucoBotton();
                        break;

                    case "realenvido":
                        Clients.All.hidemazo();
                        Clients.Client(jugador.IdConexion).hideEnvidoBotton();
                        Clients.Client(proximoJugador.IdConexion).showRealEnvidoOptions();
                        Clients.Client(jugador.IdConexion).hideRealEnvidoOptions();
                        Clients.All.hideTrucoBotton();
                        break;

                    case "truco":
                        Clients.Client(proximoJugador.IdConexion).showTrucoOptions();
                        Clients.Client(jugador.IdConexion).hideTrucoBotton();
                        Clients.Client(proximoJugador.IdConexion).hideTrucoBotton();
                        juego.TrucoCantado = true;
                        jugador.CantoTruco = true;
                        break;

                    case "retruco":
                        Clients.Client(proximoJugador.IdConexion).showReTrucoOptions();
                        Clients.Client(jugador.IdConexion).hideReTrucoBotton();
                        Clients.Client(proximoJugador.IdConexion).hideReTrucoBotton();
                        juego.TrucoCantado = true;
                        break;

                    case "vale4":
                        Clients.Client(proximoJugador.IdConexion).showVale4Options();
                        Clients.Client(jugador.IdConexion).hideVale4Botton();
                        juego.TrucoCantado = true;
                        break;
                }
            }
            else
            {
                if (accion == "envido")
                {
                    Clients.Client(jugador.IdConexion).showEnvidoBotton();
                }
                if (accion == "realenvido")
                {
                    Clients.Client(jugador.IdConexion).showRealEnvidoBotton();
                }
                if (accion == "faltaenvido")
                {
                    Clients.Client(jugador.IdConexion).showFaltaEnvidoBotton();
                }
                if (accion == "truco")
                {
                    Clients.Client(jugador.IdConexion).showTrucoBotton();
                }
            }
        }

        public void CantarPuntos(string accion, int puntos)
        {
            var jugador = ObtenerJugador(Context.ConnectionId);
            Clients.Client(jugador.IdConexion).hideSeccionesEnvido();
            juego.CuantosCantaronPuntos++;

            foreach (var jugadorSeleccionado in juego.ListaJugadores)
            {
                if (jugadorSeleccionado.IdConexion == jugador.IdConexion)
                {
                    if (jugadorSeleccionado.PuntosEnvido != puntos)
                    {
                        jugadorSeleccionado.PuntosEnvido = 0;
                    }
                    else
                    {
                        if (jugadorSeleccionado.PuntosEnvido > juego.MayorPuntos)
                        {
                            juego.MayorPuntos = jugadorSeleccionado.PuntosEnvido;
                        }
                    }
                }
            }

            if (juego.CuantosCantaronPuntos == 4)
            {
                string equipoGanador = "";

                switch (accion)
                {
                    case "Envido":
                        equipoGanador = juego.MetodoEnvido();
                        Clients.All.limpiarpuntos();
                        foreach (var jugadorSeleccionado in juego.ListaJugadores)
                        {
                            if (jugadorSeleccionado.Equipo == "Equipo1")
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo1.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo2.Puntos);
                            }
                            else
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo2.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo1.Puntos);
                            }
                        }
                        break;

                    case "EnvidoEnvido":
                        equipoGanador = juego.MetodoDobleEnvido();
                        break;

                    case "RealEnvido":
                        equipoGanador = juego.MetodoRealEnvido();
                        break;

                    case "FaltaEnvido":
                        equipoGanador = juego.MetodoFaltaEnvido();
                        Clients.All.limpiarpuntos();
                        foreach (var jugadorSeleccionado in juego.ListaJugadores)
                        {
                            if (jugadorSeleccionado.Equipo == "Equipo1")
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo1.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo2.Puntos);
                            }
                            else
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo2.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo1.Puntos);
                            }
                        }

                        if (juego.Equipo1.Puntos >= 30)
                        {
                         Clients.All.limpiarTablero();
                        }
                        if (juego.Equipo2.Puntos >= 30)
                        {
                            Clients.All.limpiarTablero();
                        }
                        break;
                }
                // muestro el equipo que gano el envido, real... ACA.
                Clients.All.mostrarmensaje("El " + equipoGanador + " ganó el " + accion + " con " + juego.MayorPuntos + " puntos");
                Clients.All.showTrucoBotton();
                juego.CuantosCantaronPuntos = 0;
                //Clients.Client(Context.ConnectionId).habilitarMovimientos();
            }
        }

        public void EjecutarAccion(string accion, bool confirmacion)
        {
            // confirmacion == true => Acepto la acción.
            Jugador jugador = ObtenerJugador(Context.ConnectionId);
            Jugador JugadorAnterior = AnteriorJugador(Context.ConnectionId);
            Jugador proximoJugador = ProximoJugador(Context.ConnectionId);
            Clients.Others.mostrarmensaje(jugador.Nombre + (confirmacion ? " aceptó el " : " rechazó el ") + accion);
            Clients.Caller.mostrarmensaje("Has " + (confirmacion ? "aceptado " : "rechazado ") + "el " + (accion == "EnvidoEnvido" ? "Doble envido" : accion));
            if (confirmacion)
            {
                foreach (var jugadores in juego.ListaJugadores)
                {
                    Clients.Client(jugadores.IdConexion).deshabilitarMovimientos();
                }
            }
            switch (accion)
            {
                case "Envido":
                    Clients.Client(jugador.IdConexion).showmazo();

                    if (confirmacion) //Contestó "sí"
                    {
                        Clients.All.showQuieroEnvido(accion);
                        Clients.All.hideEnvidoOptions();
                    }
                    else //Contestó "no"
                    {
                        if (jugador.Equipo == juego.Equipo1.Nombre)
                        {
                            juego.Equipo2.Puntos += 1;
                        }
                        else
                        {
                            juego.Equipo1.Puntos += 1;
                        }
                        Clients.All.showTrucoBotton();
                        Clients.All.hideEnvidoOptions();
                    }

                    break;

                case "EnvidoEnvido":
                    Clients.Client(proximoJugador.IdConexion).showmazo();

                    if (confirmacion) //Contestó "sí"
                    {
                        Clients.All.showEnvidoEnvidoOptions(accion);
                    }
                    else //Contestó "no"
                    {
                        if (jugador.Equipo == juego.Equipo1.Nombre)
                        {
                            juego.Equipo2.Puntos += 2;
                        }
                        else
                        {
                            juego.Equipo1.Puntos += 2;
                        }
                    }

                    break;

                case "RealEnvido":
                    Clients.Client(proximoJugador.IdConexion).showmazo();

                    if (confirmacion) //Contestó "sí"
                    {
                        Clients.All.showRealEnvidoOptions(accion);
                    }
                    else //Contestó "no"
                    {
                        if (jugador.Equipo == juego.Equipo1.Nombre)
                        {
                            juego.Equipo2.Puntos += 1;
                        }
                        else
                        {
                            juego.Equipo1.Puntos += 1;
                        }
                    }

                    break;

                case "FaltaEnvido":
                    Clients.Client(proximoJugador.IdConexion).showmazo();

                    if (confirmacion) //Contestó "sí"
                    {
                        Clients.All.hideFaltaEnvidoOptions();
                        Clients.All.showQuieroEnvido(accion);
                        //Clients.All.showFaltaEnvidoOptions(accion);
                     }
                    else //Contestó "no"
                    {
                        if (jugador.Equipo == juego.Equipo1.Nombre)
                        {
                            juego.Equipo2.Puntos += 1;
                        }
                        else
                        {
                            juego.Equipo1.Puntos += 1;
                        }
                    }

                    break;

                case "Truco":
                    Clients.Client(jugador.IdConexion).showmazo();
                    Clients.Client(jugador.IdConexion).hideTrucoOptions();

                    if (confirmacion)
                    {
                        juego.NivelTrucoCantado = 1;
                    }
                    else
                    {
                        string equipoQuePerdio = juego.ListaJugadores.Find(x => x.IdConexion == Context.ConnectionId).Equipo;
                        if (equipoQuePerdio == "Equipo1")
                        {
                            juego.Equipo2.Puntos++;
                        }
                        else
                        {
                            juego.Equipo1.Puntos++;

                        }
                    }

                    break;

                case "ReTruco":
                    Clients.Client(jugador.IdConexion).showmazo();
                    Clients.Client(jugador.IdConexion).hideReTrucoOptions();

                    if (confirmacion)
                    {
                        juego.NivelTrucoCantado = 2;
                    }
                    else
                    {
                        string equipoQuePerdio = juego.ListaJugadores.Find(x => x.IdConexion == Context.ConnectionId).Equipo;
                        if (equipoQuePerdio == "Equipo1")
                        {
                            juego.Equipo2.Puntos += 2;
                        }
                        else
                        {
                            juego.Equipo1.Puntos += 2;

                        }
                    }
                    Clients.Client(JugadorAnterior.IdConexion).habilitarMovimientos();
                    break;

                case "Vale4":
                    Clients.Client(jugador.IdConexion).showmazo();
                    Clients.Client(jugador.IdConexion).hideVale4Options();

                    if (confirmacion)
                    {
                        juego.NivelTrucoCantado = 3;
                    }
                    else
                    {
                        string equipoQuePerdio = juego.ListaJugadores.Find(x => x.IdConexion == Context.ConnectionId).Equipo;
                        if (equipoQuePerdio == "Equipo1")
                        {
                            juego.Equipo2.Puntos += 3;
                        }
                        else
                        {
                            juego.Equipo1.Puntos += 3;

                        }
                    }

                    break;
            }
            Clients.Client(JugadorAnterior.IdConexion).habilitarMovimientos();
        }

        public void JugarCarta(string codigoCarta) //ANDA DE 10!!!!! SIGAN ESTE METODO, FALTA CANTAR EL ENVIDO Y TRUCO Y DEMAS. LA RONDA AHORA SIGUE EL SENTIDO LOGICO.
        {
            // 1- Ejecutar el codigo seteando el numero de mano/ronda correspondiente.
            // 2- Habilitar los movimientos del siguiente jugador y deshabilitar el actual.
            // 3- Habilitar acciones correspondientes.

            var jugador = ObtenerJugador(Context.ConnectionId);
            var carta = jugador.ListaCartas.Where(x => x.Codigo == codigoCarta).Single();

            Jugador ProximoJugador = null;
            CartasMesa CartaTirada = new CartasMesa();

            foreach (var item in juego.ListaJugadores)
            {
                if (item.IdConexion == jugador.IdConexion) //Busco el jugador que hizo la llamada, en la lista.
                {
                    CartaTirada.Jugador = item;
                    CartaTirada.Mano = juego.NumeroMano;
                    CartaTirada.Carta = carta;
                    juego.ListaCartasJugadas.Add(CartaTirada);//Se cargan las cartas que se van jugando en una lista
                    item.Turno = false;
                    Clients.Client(Context.ConnectionId).hideEnvidoOptions();
                    Clients.Client(Context.ConnectionId).hideTrucoBotton();
                    Clients.Client(Context.ConnectionId).hideReTrucoBotton();
                    Clients.Client(Context.ConnectionId).hideVale4Botton();
                }
            }

            juego.CartasJugadas += 1;
            Clients.All.mostrarCarta(carta, jugador.NombreInterno, juego.NumeroMano);
            Clients.Client(jugador.IdConexion).deshabilitarMovimientos(); //deshabilito el movimiento al jdr que acaba de tirar


            if (juego.CartasJugadas == 4) //SE TERMINO UNA MANO.
            {
                Cartas mayCarta = null;
                string mayEquipoCarta = "";
                Jugador JugadorMayorCarta = null;
                bool banderaPrimerCarta = false;
                bool empateContinua = false;
                bool empateNoContinua = false;

                foreach (var item in juego.ListaCartasJugadas) //Se saca la mayor carta de la mano.
                {
                    if (item.Mano == juego.NumeroMano) //encontre las cartas que se jugaron en la mano que se jugo recien
                    {
                        if (banderaPrimerCarta == false) //si es la primer carta de la mano que voy a comparar
                        {
                            banderaPrimerCarta = true;
                            mayCarta = item.Carta;
                            mayEquipoCarta = item.Jugador.Equipo;
                            JugadorMayorCarta = item.Jugador;
                        }
                        else
                        {
                            if (item.Carta.Valor > mayCarta.Valor)
                            {
                                mayCarta = item.Carta;
                                mayEquipoCarta = item.Jugador.Equipo;
                                JugadorMayorCarta = item.Jugador;
                            }
                            else
                            {
                                if (item.Carta.Valor == mayCarta.Valor) //Si hay empate en las cartas mayores
                                {
                                    if (item.Jugador.Equipo != mayEquipoCarta)//Si las cartas empatadas son de distintos equipos
                                    {
                                        if (juego.NumeroMano == 1)
                                        {
                                            //Si este empate se da en la mano 1, el juego CONTINUA, se ve quien es mano para ver quien sigue, y en la mano siguiente se define.NOTA: Para darse esto, las cartas igualadas deben ser de DISTINTO equipo.
                                            empateContinua = true;
                                            if (item.Jugador.Mano > JugadorMayorCarta.Mano) //Se ve quien es mano
                                            {
                                                mayCarta = item.Carta;
                                                JugadorMayorCarta = item.Jugador;
                                            }
                                        }
                                        else //Si se da en la mano 2 o 3, el juego NO CONTINUA. Se ve que equipo gano la mano 1 para definir al ganador de la mano.
                                        {
                                            empateNoContinua = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item in juego.ListaJugadores)
                {
                    if (item.IdConexion == JugadorMayorCarta.IdConexion)
                    {
                        ProximoJugador = item;
                        item.Turno = true;
                        break; //Para no seguir recorriendo la lista sin sentido, ya que encontre lo que buscaba.
                    }
                }
                if (mayEquipoCarta == "Equipo1")
                {
                    juego.Equipo1.ManoGanada ++;
                }
                if (mayEquipoCarta == "Equipo2")
                {
                    juego.Equipo2.ManoGanada ++;
                }
                if (juego.NumeroMano >= 2)
                {
                    if (juego.Equipo1.ManoGanada == 2)
                    {
                        switch (juego.NivelTrucoCantado)
                        {
                            case 1: //Truco
                                juego.Equipo1.Puntos += 2;
                                break;
                            case 2: //Truco
                                juego.Equipo1.Puntos += 3;
                                break;
                            case 3: //Truco
                                juego.Equipo1.Puntos += 4;
                                break;
                            case 0: //no se canto truco
                                if (juego.TrucoCantado == false)
                                {
                                    juego.Equipo1.Puntos++;
                                }
                                break;
                        }
                        juego.TrucoCantado = false;
                        if (juego.Equipo1.Puntos >= 30)
                        {
                            Clients.All.limpiarTablero();
                            juego.JuegoTerminado = true;
                            Clients.Client(juego.ListaJugadores[0].IdConexion).mostrarMensajeFinal(true);
                            Clients.Client(juego.ListaJugadores[2].IdConexion).mostrarMensajeFinal(true);
                            Clients.Client(juego.ListaJugadores[1].IdConexion).mostrarMensajeFinal(false);
                            Clients.Client(juego.ListaJugadores[3].IdConexion).mostrarMensajeFinal(false);

                            Clients.All.deshabilitarMovimientos();
                        }
                        Clients.All.limpiarpuntos();
                        Clients.All.mostrarmensaje("El equipo 1 gano la ronda");
                        foreach (var jugadorSeleccionado in juego.ListaJugadores)
                        {
                            if (jugadorSeleccionado.Equipo == "Equipo1")
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo1.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo2.Puntos);
                            }
                            else
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo2.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo1.Puntos);
                            }
                        }
                        if (juego.JuegoTerminado == false)
                        {
                            juego.ListaCartasJugadas.Clear();
                            juego.ListaJugadores[0].ListaCartas.Clear();
                            juego.ListaJugadores[1].ListaCartas.Clear();
                            juego.ListaJugadores[2].ListaCartas.Clear();
                            juego.ListaJugadores[3].ListaCartas.Clear();
                            juego.Equipo1.ManoGanada = 0;
                            juego.NumeroMano = 1;
                            juego.CartasJugadas = 0;
                            juego.TerminoRonda = true;
                            juego.NumeroRonda += 1;
                            Repartir();
                        }
                    }
                    if (juego.Equipo2.ManoGanada == 2)
                    {
                        switch (juego.NivelTrucoCantado)
                        {
                            case 1: //Truco
                                juego.Equipo2.Puntos += 2;
                                break;
                            case 2: //Truco
                                juego.Equipo2.Puntos += 3;
                                break;
                            case 3: //Truco
                                juego.Equipo2.Puntos += 4;
                                break;
                            case 0: //no se canto truco
                                if (juego.TrucoCantado == false)
                                {
                                    juego.Equipo2.Puntos++;
                                }
                                break;
                        }
                        juego.TrucoCantado = false;
                        if (juego.Equipo2.Puntos >= 30)
                        {
                            Clients.All.limpiarTablero();
                            juego.JuegoTerminado = true;
                            Clients.Client(juego.ListaJugadores[0].IdConexion).mostrarMensajeFinal(false);
                            Clients.Client(juego.ListaJugadores[2].IdConexion).mostrarMensajeFinal(false);
                            Clients.Client(juego.ListaJugadores[1].IdConexion).mostrarMensajeFinal(true);
                            Clients.Client(juego.ListaJugadores[3].IdConexion).mostrarMensajeFinal(true);

                            Clients.All.deshabilitarMovimientos();
                        }
                        Clients.All.limpiarpuntos();
                        Clients.All.mostrarmensaje("El equipo 2 gano la ronda");
                        foreach (var jugadorSeleccionado in juego.ListaJugadores)
                        {
                            if (jugadorSeleccionado.Equipo == "Equipo1")
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo1.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo2.Puntos);
                            }
                            else
                            {
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Nosotros", juego.Equipo2.Puntos);
                                Clients.Client(jugadorSeleccionado.IdConexion).mostrarpuntos("Ellos", juego.Equipo1.Puntos);
                            }
                        }
                        if (juego.JuegoTerminado == false)
                        {
                            juego.ListaCartasJugadas.Clear();
                            juego.ListaJugadores[0].ListaCartas.Clear();
                            juego.ListaJugadores[1].ListaCartas.Clear();
                            juego.ListaJugadores[2].ListaCartas.Clear();
                            juego.ListaJugadores[3].ListaCartas.Clear();
                            juego.Equipo2.ManoGanada = 0;
                            juego.NumeroMano = 1;
                            juego.CartasJugadas = 0;
                            juego.TerminoRonda = true;
                            juego.NumeroRonda += 1;
                            Repartir();
                        }
                    }
                }
                if (juego.TerminoRonda == false)
                {
                    Clients.Client(ProximoJugador.IdConexion).habilitarMovimientos(); //habilito movimiento al jugador que tuvo la carta mas alta de la mano.
                    juego.NumeroMano++;
                    juego.CartasJugadas = 0; //reestablesco el valor a 0 nuevamente para la proxima mano.
                }
                juego.TerminoRonda = false;


            }
            else //Todavia queda alguien por jugar en la mano.
            {
                //habilito el movimiento al proximo jugador
                for (int i = 0; i < juego.ListaJugadores.Count(); i++)
                {
                    if (jugador.IdConexion == juego.ListaJugadores[i].IdConexion) //Encuentro al proximo jugador
                    {
                        if (i + 1 > 3) // Garantizo que la lista sea circular, es decir que no se desborde.
                        {
                            ProximoJugador = juego.ListaJugadores[0];
                        }
                        else
                        {
                            ProximoJugador = juego.ListaJugadores[i + 1];
                        }
                        juego.ListaJugadores[i].Turno = true;

                        break;
                    }
                }
                Clients.Client(ProximoJugador.IdConexion).habilitarMovimientos();
            }
        }
    }
}