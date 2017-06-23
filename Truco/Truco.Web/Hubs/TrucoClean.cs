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

            if (juego.ListaJugadores.Count() >= 2)
            {
                if (juego.ListaJugadores.Count() == 4)
                {
                    // Si el juego esta completo...
                    Clients.Caller.mostrarmensaje("El juego ya está completo!");
                }
                else
                {
                    if (juego.ListaJugadores.Count() == 3)
                    {
                        Jugador.Nombre = nombre;
                        Jugador.IdConexion = Context.ConnectionId;
                        Jugador.NombreInterno = $"user{juego.Equipo2.ListaJugadores.Count + 3}";
                        Jugador.Orden = juego.ID + 1;
                        Jugador.Turno = true;
                        juego.Equipo2.ListaJugadores.Add(Jugador);
                        juego.ListaJugadores.Add(Jugador);
                        //Clients.Others.mostrarnuevousuario(nombre);
                    }
                    else
                    {
                        Jugador.Nombre = nombre;
                        Jugador.IdConexion = Context.ConnectionId;
                        Jugador.NombreInterno = $"user{juego.Equipo1.ListaJugadores.Count + 2}";
                        Jugador.Orden = juego.ID + 1;
                        Jugador.Turno = true;
                        juego.Equipo1.ListaJugadores.Add(Jugador);
                        juego.ListaJugadores.Add(Jugador);
                        //Clients.Others.mostrarnuevousuario(nombre);
                    }
                    if (juego.Equipo2.ListaJugadores.Count() == 2)
                    {
                        Clients.All.mostrarpuntos("Ellos", 0);
                        Clients.All.mostrarpuntos("Nosotros", 0);
                        Repartir();
                    }
                }
            }
            else
            {
                // Sino ...
                if (juego.ListaJugadores.Count() == 1)
                {
                    Jugador.Nombre = nombre;
                    Jugador.IdConexion = Context.ConnectionId;
                    Jugador.NombreInterno = $"user{juego.Equipo2.ListaJugadores.Count() + 2}";
                    Jugador.Orden = juego.ID + 1;
                    Jugador.Turno = true;
                    juego.Equipo2.ListaJugadores.Add(Jugador);
                    juego.ListaJugadores.Add(Jugador);
                    //Clients.Others.mostrarnuevousuario(nombre);
                }
                else
                {
                    Jugador.Nombre = nombre;
                    Jugador.IdConexion = Context.ConnectionId;
                    Jugador.NombreInterno = $"user{juego.Equipo1.ListaJugadores.Count() + 1}";
                    Jugador.Orden = juego.ID + 1;
                    Jugador.Turno = true;
                    juego.Equipo1.ListaJugadores.Add(Jugador);
                    juego.ListaJugadores.Add(Jugador);
                    //Clients.Others.mostrarnuevousuario(nombre);
                }
            }

            if (juego.ListaJugadores.Count() == 4)
            {
                foreach (var item in juego.ListaJugadores)
                {
                    // Por cada jugador
                    Clients.All.mostrarNombre(item);
                }
            }

            // Si es el ultimo jugador...




        }
        private Jugador ObtenerJugador(string idConexion)
        {
            Jugador JugadorObtenido = null;
            foreach (var Jug1 in juego.Equipo1.ListaJugadores)
            {
                if (idConexion == Jug1.IdConexion)
                {
                    JugadorObtenido = Jug1;
                }
            }
            foreach (var Jug1 in juego.Equipo2.ListaJugadores)
            {
                if (idConexion == Jug1.IdConexion)
                {
                    JugadorObtenido = Jug1;
                }
            }
            return JugadorObtenido;
        }

        public void Repartir()
        {
            Clients.All.limpiarTablero();
            juego.PrepararMano(4);
            foreach (var item in juego.Equipo1.ListaJugadores)
            {
                Clients.Client(item.IdConexion).mostrarCartas(item.ListaCartas);
            }

            foreach (var item in juego.Equipo2.ListaJugadores)
            {
                Clients.Client(item.IdConexion).mostrarCartas(item.ListaCartas);
            }

            Clients.Client(juego.Equipo1.ListaJugadores[0].IdConexion).habilitarMovimientos();
            //Clients.Client(...).hideEnvidoEnvidoBotton();
            //Clients.Client(...).hideVale4Botton();
            //Clients.Client(...).hideReTrucoBotton();
            //Clients.Client(...).showEnvidoBotton();
            //Clients.Client(...).showTrucoBotton();
            //Clients.Client(...).showRealEnvidoBotton();
            //Clients.Client(...).showFaltaEnvidoBotton();

            //Clients.Client(...).desabilitarMovimientos();
            //Clients.Client(...).hideEnvidoOptions();
            //Clients.Client(...).hideTrucoBotton();
            //Clients.Client(...).hideReTrucoBotton();
            //Clients.Client(...).hideVale4Botton();              
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

        public void JugarCartaa(string codigoCarta)
        {
            //Clients.Client(jugador.IdConexion).deshabilitarMovimientos();
            // 1- Ejecutar el codigo seteando el numero de mano/ronda correspondiente.
            // 2- Habilitar los movimientos del siguiente jugador y deshabilitar el actual.
            // 3- Habilitar acciones correspondientes.


            var jugador = ObtenerJugador(Context.ConnectionId);
            var carta = jugador.ListaCartas.Where(x => x.Codigo == codigoCarta).Single();

            //foreach (var jugadorSel in juego.Equipo1.ListaJugadores)
            //{
            //    var mayorTurno = juego.Equipo1.ListaJugadores.Where(x => x.Turno).Max(x => x.Mano);
            //    var jugadorConTurno = juego.Equipo1.ListaJugadores.Where(x => x.Mano == mayorTurno).Single();
            //}

            var mayorTurno = 0;
            var idMay = "asd";
            Jugador ProximoJugador = null;
            CartasMesa CartaTirada = new CartasMesa();
            foreach (var item in juego.Equipo1.ListaJugadores)
            {
                if (item.Turno)
                {
                    if (item.Mano > mayorTurno)
                    {
                        mayorTurno = item.Mano;
                        idMay = item.IdConexion;
                        CartaTirada.Equipo = "Equipo1";
                        CartaTirada.IdJugador = item.IdConexion;
                        CartaTirada.Mano = juego.NumeroMano;
                    }
                }
            }
            foreach (var item in juego.Equipo2.ListaJugadores)
            {
                if (item.Turno)
                {
                    if (item.Mano > mayorTurno)
                    {
                        mayorTurno = item.Mano;
                        idMay = item.IdConexion;
                        CartaTirada.Equipo = "Equipo2";
                        CartaTirada.IdJugador = item.IdConexion;
                        CartaTirada.Mano = juego.NumeroMano;
                    }
                }
            }
            juego.CartasJugadas += 1;
            //seteo el turno del jugador que tiro la carta en false
            foreach (var item in juego.Equipo1.ListaJugadores)
            {
                if (idMay == item.IdConexion)
                {
                    item.Turno = false;
                }
            }
            foreach (var item in juego.Equipo2.ListaJugadores)
            {
                if (idMay == item.IdConexion)
                {
                    item.Turno = false;
                }
            }
                      
            if (juego.NumeroMano == 0)
            {
                juego.NumeroMano = 1;
            }
           
            CartaTirada.Carta = carta;
            juego.ListaCartasJugadas.Add(CartaTirada);//Se cargan las cartas que se van jugando en una lista
            Clients.All.mostrarCarta(carta, jugador.NombreInterno, juego.NumeroMano);
            Clients.Client(jugador.IdConexion).deshabilitarMovimientos(); //deshabilito el movimiento al jdr que acaba de tirar

            if (juego.CartasJugadas == 4) //quiere decir que se termino una mano.
            {
                juego.NumeroMano = juego.NumeroMano;
                juego.NumeroMano++;
                juego.CartasJugadas = 0; //reestablesco el valor a 0 nuevamente para la proxima mano.
                juego.Equipo1.ListaJugadores[0].Turno = true;
                juego.Equipo1.ListaJugadores[1].Turno = true;
                juego.Equipo2.ListaJugadores[0].Turno = true;
                juego.Equipo2.ListaJugadores[1].Turno = true;

                Cartas mayCarta = null;
                string idJugadorMayorCarta = "";
                bool BanderaPrimerCarta = false;
                foreach (var item in juego.ListaCartasJugadas) //Se saca la mayor carta de la mano.
                {
                    if (item.Mano == juego.NumeroMano - 1) //encontre las cartas que se jugaron en la mano que se jugo recien
                    {
                        if (BanderaPrimerCarta == false) //si es la primer carta de la mano que voy a comparar
                        {
                            BanderaPrimerCarta = true;
                            mayCarta = item.Carta;
                            idJugadorMayorCarta = item.IdJugador;
                        }
                        else
                        {
                            if (item.Carta.Valor > mayCarta.Valor)
                            {
                                mayCarta = item.Carta;
                                idJugadorMayorCarta = item.IdJugador;
                            }
                        }
                    }
                }
                foreach (var item in juego.Equipo1.ListaJugadores)
                {
                    if (item.IdConexion == idJugadorMayorCarta)
                    {
                        ProximoJugador = item;
                    }
                }
                foreach (var item in juego.Equipo2.ListaJugadores)
                {
                    if (item.IdConexion == idJugadorMayorCarta)
                    {
                        ProximoJugador = item;
                    }
                }
                Clients.Client(ProximoJugador.IdConexion).habilitarMovimientos(); //habilito movimiento al jugador que tuvo la carta mas alta de la mano.

            }
            /////////////////////////////////////////////////////////////////////////////////////////
            //LOS JUGADORES ESTAN MAL SENTADOS, CORREGIR!!! (deben sentarse uno enfrentado al otro)//
            /////////////////////////////////////////////////////////////////////////////////////////

            //habilito el movimiento al proximo jugador
            if (mayorTurno > 1) // Esto quiere decir que todavia queda alguien por tirar en la mano
            {
                foreach (var item in juego.Equipo1.ListaJugadores)
                {
                    if (item.Mano == mayorTurno - 1)
                    {
                        ProximoJugador = item;
                    }
                }
                foreach (var item in juego.Equipo2.ListaJugadores)
                {
                    if (item.Mano == mayorTurno - 1)
                    {
                        ProximoJugador = item;
                    }
                }
                Clients.Client(ProximoJugador.IdConexion).habilitarMovimientos();
            }
            // NOTA: Una vez que todos jugaron su primera carta, tengo que ver a quien volver a habilitar su movimiento para que siga con la mano 2 (depende quien gano la primera)
        } //MAURI, TRATE DE SEGUIR TU METODO PERO NO PUDE ENTENDER BIEN COMO HACER QUE SIGA FUNCIONANDO Y ESTOY TRATANDO DE HACER OTRO. TE AVISO DE ESTO PORQUE LE CAMBIE EL NOMBRE A TU METODO
        //ASI ARRANCABA EL MIO PARA PODER PROBARLO

        public void JugarCarta(string codigoCarta)
        {
            var turnoF = 0; // CANTIDAD DE JUGADORES CON TURNOS EN FALSE
            var nroRonda = juego.NumeroRonda;
            var nroMano = juego.NumeroMano;
            var jugador = ObtenerJugador(Context.ConnectionId);
            var carta = jugador.ListaCartas.Where(x => x.Codigo == codigoCarta).Single();
            //Jugador ProximoJugador = null;
            CartasMesa CartaTirada = new CartasMesa();

            if (turnoF != 4) //SI EL CONTADOR turnoF ES DISTINTO DE 4
            {
                if ((nroRonda == 0) && (nroMano == 0)) //SI LA RONDA ES LA PRIMERA Y LA MANO ES LA PRIMERA
                {
                    foreach (var jugadorSeleccionado in juego.ListaJugadores) //BUSCO EN LA LISTA QUE CREE DONDE ESTAN TODOS LOS JUGADORES SIN IMPORTAR SU EQUIPO
                    {
                        if (jugadorSeleccionado.IdConexion == Context.ConnectionId) //VEO SI EL JUGADOR QUE TIENE EL CONTEXT ES EL MISMO QUE ESTA SELECCIONADO EN LA LISTA EN ESE MOMENTO
                        {
                            Clients.Client(jugadorSeleccionado.IdConexion).habilitarMovimientos(); //LE HABILITO EL MOVIMIENTO
                            Clients.All.mostrarCarta(carta, jugadorSeleccionado.NombreInterno, juego.NumeroMano); //HAGO QUE JUEGUE LA CARTA
                            Clients.Client(jugadorSeleccionado.IdConexion).deshabilitarMovimientos(); //LE DESHABILITO EL MOVIMIENTO
                            turnoF++; //SUMO 1 A TURNOF
                            jugadorSeleccionado.Turno = false; // LE PONGO EN FALSE LA PROPIEDAD TURNO
                        }                        
                    }//ESTO SE DEBERIA REPETIR CON LOS 4 JUGADORES ANTES DE SALIR DEL FOREACH
                    turnoF = 0; //CUANDO SALE DEL FOREACH LE PONGO EN 0 LA PROPIEDAD TURNOF ASI EN EL SIGUENTE IF PUEDO SEGURI HACIENDO COTRAS COSAS
                    foreach (var jugadores in juego.ListaJugadores)//Y TAMBIEN LE PONGO EN TRUE DE VUELTA EL TURNO A TODOS LOS JUGADORES YA QUE JUGARON LA MANO
                    {
                        jugadores.Turno = true;
                    }
                }
            }
        }
    }
}