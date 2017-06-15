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
        public static Equipo Equipo1 = new Equipo();
        public static Equipo Equipo2 = new Equipo();
        public void AgregarJugador(string nombre)
        {
            Jugador Jugador = new Jugador();

            if (Equipo1.ListaJugadores.Count() == 2)
            {
                if (Equipo2.ListaJugadores.Count() == 2)
                {
                    // Si el juego esta completo...
                    Clients.Caller.mostrarmensaje("El juego ya está completo!");
                }
                else
                {
                    if (Equipo2.ListaJugadores.Count() == 1)
                    {
                        Clients.All.mostrarpuntos("Ellos", 0);
                        Clients.All.mostrarpuntos("Nosotros", 0);
                    }
                        Jugador.Nombre = nombre;
                        Jugador.IdConexion = Context.ConnectionId;
                        Jugador.NombreInterno = $"user{Equipo2.ListaJugadores.Count + 3}";
                        Jugador.Orden = juego.ID + 1;
                        Equipo2.ListaJugadores.Add(Jugador);
                        Clients.Others.mostrarnuevousuario(nombre);
                }
            }
            else
            {
                // Sino ...
                Jugador.Nombre = nombre;
                Jugador.IdConexion = Context.ConnectionId;
                Jugador.NombreInterno = $"user{Equipo1.ListaJugadores.Count() + 1}";
                Jugador.Orden = juego.ID + 1;
                Equipo1.ListaJugadores.Add(Jugador);
                Clients.Others.mostrarnuevousuario(nombre);
            }


            foreach (var item in Equipo1.ListaJugadores)
            {
                // Por cada jugador
                Clients.All.mostrarNombre(item);
            }
            foreach (var item in Equipo2.ListaJugadores)
            {
                // Por cada jugador
                Clients.All.mostrarNombre(item);
            }


            // Si es el ultimo jugador...
            


            Repartir();
        }
        private Jugador ObtenerJugador(string idConexion)
        {
            Jugador JugadorObtenido = null;
            foreach (var Jug1 in Equipo1.ListaJugadores)
            {
                if (idConexion == Jug1.IdConexion)
                {
                    JugadorObtenido = Jug1;
                }
            }
            foreach (var Jug1 in Equipo2.ListaJugadores)
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

            Clients.Client(ObtenerJugador(Context.ConnectionId).IdConexion).mostrarCartas(ObtenerJugador(Context.ConnectionId).ListaCartas);

            /*
             * Propiedades de la Carta:
             * Codigo
             * Imagen
             * Codigo                        
             */

            //Clients.Client(jugador.IdConexion).habilitarMovimientos();
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

        //public void JugarCarta(string codigoCarta)
        //{
        //    Clients.All.mostrarCarta(carta, nombreInterno, cartaElegida);
        //}
    }
}