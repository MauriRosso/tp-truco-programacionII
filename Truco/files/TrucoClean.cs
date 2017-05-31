using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Truco.Web.Entities;

namespace Truco.Web.Hubs
{
    [HubName("truco")]
    public class Truco : Hub
    {
        public static Juego juego = new Juego();

        public void AgregarJugador(string nombre)
        {

            // Si el juego esta completo...
            Clients.Caller.mostrarmensaje("El juego ya está completo!");

            // Sino ...
            Clients.Others.mostrarnuevousuario(nombre);

            // Por cada jugador
            Clients.All.mostrarnombre(jugador);

            // Si es el ultimo jugador...

            // Por cada jugador 
            Clients.Client(jugador.IdConexion).mostrarpuntos("Ellos", puntosEquipo2);
            Clients.Client(jugador.IdConexion).mostrarpuntos("Nosotros", puntosEquipo1);

            Repartir();
        }

        public void cantar(string accion) 
        {
            Clients.Others.mostrarmensaje("Jugador X canto ACCION");
            Clients.Caller.mostrarmensaje("Yo cante ACCION");
            
            Clients.Client(jugador.IdConexion).deshabilitarMovimientos();

            // Si el juego termino...
            Clients.Client(jugador.IdConexion).mostrarMensajeFinal(true); // GANADOR
            Clients.Client(jugador.IdConexion).mostrarMensajeFinal(false); // PERDEDOR
            Clients.All.deshabilitarMovimientos();

            // Sino
            Clients.All.limpiarpuntos();

            // Y mostrar puntos y repartir.


            switch (accion) 
            {   
                case "me voy al mazo":            
                    break;
                case "envido":
                    Clients.All.hidemazo();
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
      
        public void EjecutarAccion(string accion, bool confirmacion)
        {
            // confirmacion == true => Acepto la acción.
            Clients.All.mostrarmensaje("Jugador X acepto/rechazo la ACCION");
            
            switch (accion)
            {
                case "Envido":
                    Clients.All.showmazo();            
                    Clients.Client(jugador.IdConexion).habilitarMovimientos();
                    break;
                case "EnvidoEnvido":
                    Clients.All.showmazo();
                    Clients.Client(jugador.IdConexion).habilitarMovimientos();
                    break;
                case "RealEnvido":
                    Clients.All.showmazo();
                    Clients.Client(jugador.IdConexion).habilitarMovimientos();
                    break;
                case "FaltaEnvido":
                    Clients.All.showmazo();
                    Clients.Client(jugador.IdConexion).habilitarMovimientos();
                    break;
                case "Truco":
                    break;
                case "ReTruco":
                    break;
                case "Vale4":
                    break;
            }
        }

        public void JugarCarta(string codigoCarta, string accion)
        {                        
            Clients.All.mostrarCarta(carta, selector);
        }       

        public void Repartir()
        {
            Clients.All.limpiarTablero();

            Clients.Client(jugador.IdConexion).mostrarCartas(carta);

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
    }
}