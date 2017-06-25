using Microsoft.AspNet.SignalR;
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
                    Jugador jugadorConTurno = juego.ListaJugadores.Find(x => x.Turno);
                    Clients.AllExcept(jugadorConTurno.IdConexion).hideEnvidoOptions();
                    Clients.AllExcept(jugadorConTurno.IdConexion).hideTrucoBotton();
                    Clients.All.hideReTrucoBotton();
                    Clients.All.hideVale4Botton();
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

            foreach (var item in juego.ListaJugadores)
            {                       
                item.Turno = false;
            }
        }

        public Jugador ProximoJugador (string idConexion)
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

            Clients.Others.mostrarmensaje(jugador.Nombre + " cantó " + (accion == "envidoenvido" ? "doble envido" : accion));
            Clients.Caller.mostrarmensaje("Has cantado " + (accion == "envidoenvido" ? "doble envido" : accion));
            Clients.Client(jugador.IdConexion).deshabilitarMovimientos();
            
            //// Si el juego termino...

            //Clients.Client(jugador.IdConexion).mostrarMensajeFinal(true); // GANADOR
            //Clients.Client(jugador.IdConexion).mostrarMensajeFinal(false); // PERDEDOR
            //Clients.All.deshabilitarMovimientos();
            
            //// Sino
            
            //Clients.All.limpiarpuntos();
            
            //// Y mostrar puntos y repartir.
            
            switch (accion)
            {
                case "me voy al mazo":
                    break;

                case "envido":
                    Clients.All.hidemazo();
                    Clients.Client(proximoJugador.IdConexion).showEnvidoOptions();
                    break;

                case "envidoenvido":
                    Clients.All.hidemazo();
                    break;

                case "faltaenvido":
                    Clients.All.hidemazo();
                    break;

                case "realenvido":
                    Clients.All.hidemazo();
                    break;

                case "truco":
                    break;

                case "retruco":
                    break;

                case "vale4":
                    break;
            }
        }

        public void CantarPuntos(string accion, int puntos)
        {
            Clients.Client(Context.ConnectionId).hideSeccionesEnvido();
            juego.CuantosCantaronPuntos++;
            int posicion = -1;
            int equipo = 0;

            if (juego.Equipo1.ListaJugadores.Exists(jugador => jugador.IdConexion == Context.ConnectionId))
            {
                posicion = juego.Equipo1.ListaJugadores.FindIndex(jugador => jugador.IdConexion == Context.ConnectionId);
                equipo = 1;
            }
            else
            {
                posicion = juego.Equipo2.ListaJugadores.FindIndex(jugador => jugador.IdConexion == Context.ConnectionId);
                equipo = 2;
            }

            if (juego.ListaJugadores[posicion].PuntosEnvido != puntos)
            {
                if (equipo == 1)
                {
                    juego.Equipo1.ListaJugadores[posicion].PuntosEnvido = 0;
                }
                else
                {
                    juego.Equipo2.ListaJugadores[posicion].PuntosEnvido = 0;
                }
            }

            if (juego.CuantosCantaronPuntos == 4)
            {
                switch (accion)
                {
                    case "Envido":
                        juego.MetodoEnvido();
                        break;

                    case "EnvidoEnvido":
                        juego.MetodoDobleEnvido();
                        break;

                    case "RealEnvido":
                        juego.MetodoRealEnvido();
                        break;

                    case "FaltaEnvido":
                        juego.MetodoFaltaEnvido();
                        break;
                }
            }
        }

        public void EjecutarAccion(string accion, bool confirmacion)
        {
            // confirmacion == true => Acepto la acción.
            Jugador jugador = ObtenerJugador(Context.ConnectionId);
            Jugador proximoJugador = ProximoJugador(Context.ConnectionId);
            Clients.Others.mostrarmensaje(jugador.Nombre + (confirmacion ? " aceptó el " : " rechazó el ") + accion);
            Clients.Caller.mostrarmensaje("Has " + (confirmacion ? "aceptado " : "rechazado ") + "el " + (accion == "EnvidoEnvido" ? "Doble envido" : accion));

            switch (accion)
            {
                case "Envido":
                    Clients.Client(jugador.IdConexion).showmazo();
                    Clients.Client(jugador.IdConexion).habilitarMovimientos();
                    
                    if (confirmacion) //Contestó "sí"
                    {
                        Clients.All.showQuieroEnvido(accion);
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

                case "EnvidoEnvido":
                    Clients.Client(proximoJugador.IdConexion).showmazo();
                    Clients.Client(proximoJugador.IdConexion).habilitarMovimientos();

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
                    Clients.Client(proximoJugador.IdConexion).habilitarMovimientos();

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
                    Clients.Client(proximoJugador.IdConexion).habilitarMovimientos();

                    if (confirmacion) //Contestó "sí"
                    {
                        Clients.All.showFaltaEnvidoOptions(accion);
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
                    break;

                case "ReTruco":
                    break;

                case "Vale4":
                    break;
            }
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

                        if (!juego.EnvidoCantado && juego.NumeroMano == 1)
                        {
                            Clients.Client(ProximoJugador.IdConexion).showEnvidoBotton();
                            Clients.Client(ProximoJugador.IdConexion).showEnvidoEnvidoBotton();
                            Clients.Client(ProximoJugador.IdConexion).showRealEnvidoButton();
                            Clients.Client(ProximoJugador.IdConexion).showFaltaEnvidoButton();
                        }
                        
                        if (!juego.TrucoCantado)
                        {
                            Clients.Client(ProximoJugador.IdConexion).showTrucoBotton();
                            Clients.Client(ProximoJugador.IdConexion).showReTrucoBotton();
                            Clients.Client(ProximoJugador.IdConexion).showVale4Botton();
                        }

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

                        if (!juego.EnvidoCantado && juego.NumeroMano == 1)
                        {
                            Clients.Client(ProximoJugador.IdConexion).showEnvidoBotton();
                            Clients.Client(ProximoJugador.IdConexion).showEnvidoEnvidoBotton();
                            Clients.Client(ProximoJugador.IdConexion).showRealEnvidoButton();
                            Clients.Client(ProximoJugador.IdConexion).showFaltaEnvidoButton();
                        }
                        
                        if (!juego.TrucoCantado)
                        {
                            Clients.Client(ProximoJugador.IdConexion).showTrucoBotton();
                            Clients.Client(ProximoJugador.IdConexion).showReTrucoBotton();
                            Clients.Client(ProximoJugador.IdConexion).showVale4Botton();
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