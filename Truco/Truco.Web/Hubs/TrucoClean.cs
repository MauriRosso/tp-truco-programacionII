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
            Clients.All.limpiarTablero();
            juego.PrepararMano(4);
            juego.ListaJugadores.OrderByDescending(x => x.Mano); //Cada vez que reparto, ordeno la lista en base a la mano.
            foreach (var item in juego.Equipo1.ListaJugadores)
            {
                Clients.Client(item.IdConexion).mostrarCartas(item.ListaCartas);
            }

            foreach (var item in juego.Equipo2.ListaJugadores)
            {
                Clients.Client(item.IdConexion).mostrarCartas(item.ListaCartas);
            }

            Clients.Client(juego.ListaJugadores[0].IdConexion).habilitarMovimientos(); //Habilito el mov. del primer jugador para comenzar a jugar

            //foreach (var item in juego.ListaJugadores)
            //{
            //    Clients.Client(item.IdConexion).showEnvidoBotton();
            //    Clients.Client(item.IdConexion).showTrucoBotton();
            //    Clients.Client(item.IdConexion).showRealEnvidoBotton();
            //    Clients.Client(item.IdConexion).showFaltaEnvidoBotton();
            //}

            //Clients.Client(item.IdConexion).hideEnvidoEnvidoBotton();
            //Clients.Client(item.IdConexion).hideVale4Botton();
            //Clients.Client(item.IdConexion).hideReTrucoBotton();

            //Clients.Client(item.IdConexion).hideEnvidoOptions();
            //Clients.Client(item.IdConexion).hideTrucoBotton();
            //Clients.Client(item.IdConexion).hideReTrucoBotton();
            //Clients.Client(item.IdConexion).hideVale4Botton();

            //Clients.Client(...).desabilitarMovimientos();
        }

        //public void cantar(string accion) 
        //{
        //    Clients.Others.mostrarmensaje("Jugador X canto ACCION");
        //    Clients.Caller.mostrarmensaje("Yo cante ACCION");

        //    Clients.Client(jugador.IdConexion).deshabilitarMovimientos();

        //    // Si el juego termino...
        //    Clients.Client(jugador.IdConexion).mostrarMensajeFinal(true); // GANADOR
        //    Clients.Client(jugador.IdConexion).mostrarMensajeFinal(false); // PERDEDOR
        //    Clients.All.deshabilitarMovimientos();

        //    // Sino
        //    Clients.All.limpiarpuntos();

        //    // Y mostrar puntos y repartir.


        //    switch (accion) 
        //    {   
        //        case "me voy al mazo":            
        //            break;
        //        case "envido":
        //            Clients.All.hidemazo();
        //            break;
        //        case "envidoenvido":
        //            Clients.All.hidemazo();
        //            break;
        //        case "faltaenvido":
        //            Clients.All.hidemazo();
        //            break;
        //        case "realenvido":
        //            Clients.All.hidemazo();
        //            break;
        //        case "truco":
        //            break;
        //        case "retruco":
        //            break;
        //        case "vale4":
        //            break;
        //    }
        //}

        //public void EjecutarAccion(string accion, bool confirmacion)
        //{
        //    // confirmacion == true => Acepto la acción.
        //    Clients.All.mostrarmensaje("Jugador X acepto/rechazo la ACCION");

        //    switch (accion)
        //    {
        //        case "Envido":
        //            Clients.All.showmazo();            
        //            Clients.Client(jugador.IdConexion).habilitarMovimientos();
        //            break;
        //        case "EnvidoEnvido":
        //            Clients.All.showmazo();
        //            Clients.Client(jugador.IdConexion).habilitarMovimientos();
        //            break;
        //        case "RealEnvido":
        //            Clients.All.showmazo();
        //            Clients.Client(jugador.IdConexion).habilitarMovimientos();
        //            break;
        //        case "FaltaEnvido":
        //            Clients.All.showmazo();
        //            Clients.Client(jugador.IdConexion).habilitarMovimientos();
        //            break;
        //        case "Truco":
        //            break;
        //        case "ReTruco":
        //            break;
        //        case "Vale4":
        //            break;
        //    }
        //}

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
                        break; //Para no seguir recorriendo la lista sin sentido, ya que encontre lo que buscaba.
                    }
                }
                Clients.Client(ProximoJugador.IdConexion).habilitarMovimientos(); //habilito movimiento al jugador que tuvo la carta mas alta de la mano.

                juego.NumeroMano++;
                juego.CartasJugadas = 0; //reestablesco el valor a 0 nuevamente para la proxima mano.
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
                        break;
                    }
                }
                Clients.Client(ProximoJugador.IdConexion).habilitarMovimientos();
            }             
        }  

        //public void JugarCarta(string codigoCarta)
        //{

        //    //LA LOGICA ES DESHABILITAR EL MOVIMIENTO DEL JUG. ACTUAL Y HABILITAR EL MOV. DEL PROXIMO JUGADOR.

        //    int turnoF = 0; // CANTIDAD DE JUGADORES CON TURNOS EN FALSE
        //    int nroMano = juego.NumeroMano;
        //    var nroRonda = juego.NumeroRonda;
        //    var jugador = ObtenerJugador(Context.ConnectionId);
        //    var carta = jugador.ListaCartas.Where(x => x.Codigo == codigoCarta).Single();
        //    //Jugador ProximoJugador = null;
        //    CartasMesa CartaTirada = new CartasMesa();

        //    if (turnoF != 4) //SI EL CONTADOR turnoF ES DISTINTO DE 4
        //    {
        //        if ((nroRonda == 0) && (nroMano == 1)) //SI LA RONDA ES LA PRIMERA Y LA MANO ES LA PRIMERA
        //        {
        //            //foreach (var jugadorSeleccionado in juego.ListaJugadores) //BUSCO EN LA LISTA QUE CREE DONDE ESTAN TODOS LOS JUGADORES SIN IMPORTAR SU EQUIPO
        //            //{
        //            //    if (jugadorSeleccionado.IdConexion == Context.ConnectionId) //VEO SI EL JUGADOR QUE TIENE EL CONTEXT ES EL MISMO QUE ESTA SELECCIONADO EN LA LISTA EN ESE MOMENTO
        //            //    {
        //            //        Clients.All.mostrarCarta(carta, jugadorSeleccionado.NombreInterno, juego.NumeroMano); //HAGO QUE JUEGUE LA CARTA
        //            //        Clients.Client(jugadorSeleccionado.IdConexion).deshabilitarMovimientos(); //LE DESHABILITO EL MOVIMIENTO
        //            //        Clients.Client(jugadorSeleccionado.IdConexion).habilitarMovimientos(); //LE HABILITO EL MOVIMIENTO AL PROXIMO
        //            //        turnoF++; //SUMO 1 A TURNOF
        //            //        jugadorSeleccionado.Turno = false; // LE PONGO EN FALSE LA PROPIEDAD TURNO
        //            //    }                        
        //            //}//ESTO SE DEBERIA REPETIR CON LOS 4 JUGADORES ANTES DE SALIR DEL FOREACH

        //            for (int i = 0; i < juego.ListaJugadores.Count(); i++)
        //            {
        //                if (juego.ListaJugadores[i].IdConexion == Context.ConnectionId) //VEO SI EL JUGADOR QUE TIENE EL CONTEXT ES EL MISMO QUE ESTA SELECCIONADO EN LA LISTA EN ESE MOMENTO
        //                {
        //                    Clients.All.mostrarCarta(carta, juego.ListaJugadores[i].NombreInterno, juego.NumeroMano); //HAGO QUE JUEGUE LA CARTA
        //                    Clients.Client(juego.ListaJugadores[i].IdConexion).deshabilitarMovimientos(); //LE DESHABILITO EL MOVIMIENTO
        //                    Clients.Client(juego.ListaJugadores[i + 1].IdConexion).habilitarMovimientos(); //LE HABILITO EL MOVIMIENTO AL PROXIMO
        //                    turnoF++; //SUMO 1 A TURNOF
        //                    juego.ListaJugadores[i].Turno = false; // LE PONGO EN FALSE LA PROPIEDAD TURNO
        //                }
        //            }//ESTO SE DEBERIA REPETIR CON LOS 4 JUGADORES ANTES DE SALIR DEL FOREACH
        //            turnoF = 0; //CUANDO SALE DEL FOREACH LE PONGO EN 0 LA PROPIEDAD TURNOF ASI EN EL SIGUENTE IF PUEDO SEGURI HACIENDO COTRAS COSAS
        //            foreach (var jugadores in juego.ListaJugadores)//Y TAMBIEN LE PONGO EN TRUE DE VUELTA EL TURNO A TODOS LOS JUGADORES YA QUE JUGARON LA MANO
        //            {
        //                jugadores.Turno = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        nroMano++;
        //    }
        //}
    }
}