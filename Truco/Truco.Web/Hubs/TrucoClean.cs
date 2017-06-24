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

            //Clients.Client(item.IdConexion).hideEnvidoOptions();
            //Clients.Client(item.IdConexion).hideTrucoBotton();
            //Clients.Client(item.IdConexion).hideReTrucoBotton();
            //Clients.Client(item.IdConexion).hideVale4Botton();

            //Clients.Client(...).desabilitarMovimientos();
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
        }

        public void cantar(string accion)
        {
            var jugador = ObtenerJugador(Context.ConnectionId);

            foreach (var jugadorSeleccionado in juego.ListaJugadores) //busco el jugador que canta en la lista de jugadores
            {
                if (jugador.IdConexion == jugadorSeleccionado.IdConexion)
                {
                    jugadorSeleccionado.Turno = true; //voy a usar la propiedad Turno para hacer que los que tengan True unicamente puedan cantar (en el metodo Repartir setee a todos los jugadores con Turno = false)
                    if (jugadorSeleccionado.Turno) //me aseguro que tenga la propiedad true por las dudas
                    {
                        if (jugadorSeleccionado.Mano == 2 || jugadorSeleccionado.Mano == 1 && juego.NumeroMano == 1) //al fijarse si estos jugadores tienen Mano == 2 || Mano == 1 significa que son los ultimos 2 jugadores de la mano
                        {                                                                                            // numero 1, y los unicos que pueden cantar el envido
                            Clients.Others.mostrarmensaje("{0}, canto {1}", jugador.Nombre, accion);
                            Clients.Caller.mostrarmensaje("Yo cante {0}", accion);  //estas cosas venian con lo que nos dio el profe
                            Clients.Client(jugador.IdConexion).deshabilitarMovimientos();

                            //// Si el juego termino...
                            //Clients.Client(jugador.IdConexion).mostrarMensajeFinal(true); // GANADOR
                            //Clients.Client(jugador.IdConexion).mostrarMensajeFinal(false); // PERDEDOR
                            //Clients.All.deshabilitarMovimientos();

                            //// Sino
                            //Clients.All.limpiarpuntos();

                            // Y mostrar puntos y repartir.

                            //aca arranque de vuelta yo
                            var confirmacion = true; //Variable en la que voy a guardar si el jugador del equipo contrario acepta o no la accion
                            switch (accion)
                            {
                                case "me voy al mazo": //Aca habria que agregar una propiedad a jugador PosibilidadEnvido que arranque en true ya que si un jugador de un equipo canta el truco antes que el otro jugador tenga la posibildiad
                                    if (jugadorSeleccionado.Equipo == "Equipo 1")//de cantar el envido se deberian sumar 2 puntos en vez de 1
                                    {
                                        juego.Equipo2.Puntos++;
                                    }
                                    if (jugadorSeleccionado.Equipo == "Equipo 2")
                                    {
                                        juego.Equipo1.Puntos++;
                                    }
                                    break;
                                case "envido": 
                                    if (jugadorSeleccionado.Equipo == "Equipo 1") //me fijo si el que canto el envido esta en el equipo 1
                                    {                                       
                                        foreach (var jugadorEquipoContrario in juego.Equipo2.ListaJugadores)//busco los jugadores del equipo 2
                                        {
                                            var siguienteJugador = ProximoJugador(jugadorEquipoContrario.IdConexion);//elijo el proximo jugador para que decida si acepta o no, ya que si dejariamos que los 2 elijan si quieren o no se armaria un quilombo
                                           
                                                //jugadorEquipoContrario(Context.ConnectionId).hideTrucoBotton();//primero mostarle al jugadorEquipoContrario el boton QuieroEnvido o, habilitarle los demas botones de las opciones Envido por si quiere cantar otra cosa
                                                //jugadorEquipoContrario.showQuieroEnvido(); //aca no encuentro la funcion de js para poder agregar en al variable "confirmacion" el valor de si quiere o no el envido                                                                                    
                                        }
                                    }
                                    if (jugadorSeleccionado.Equipo == "Equipo 2")
                                    {

                                    }
                                    break;
                                case "envidoenvido":
                                    Clients.All.hidemazo();
                                    Clients.All.hideTrucoBotton();
                                    Clients.All.hideReTrucoBotton();
                                    Clients.All.hideVale4Botton();

                                    Clients.All.hideEnvidoEnvidoBotton();
                                    Clients.All.hideEnvidoBotton();
                                    Clients.Others.showRealEnvidoBotton();
                                    Clients.Others.showFaltaEnvidoBotton();
                                    Clients.Caller.hideEnvidoOptions();
                                    break;
                                case "faltaenvido":
                                    Clients.All.hidemazo();
                                    Clients.All.hideTrucoBotton();
                                    Clients.All.hideReTrucoBotton();
                                    Clients.All.hideVale4Botton();

                                    Clients.All.hideEnvidoOptions();
                                    break;
                                case "realenvido":
                                    Clients.All.hidemazo();
                                    Clients.All.hideTrucoBotton();
                                    Clients.All.hideReTrucoBotton();
                                    Clients.All.hideVale4Botton();

                                    Clients.All.hideEnvidoOptions();
                                    Clients.Others.showFaltaEnvidoBotton();
                                    break;
                                case "truco":
                                    Clients.All.hideEnvidoOptions();

                                    Clients.All.showmazo();
                                    Clients.Others.showReTrucoBotton();
                                    break;
                                case "retruco":
                                    Clients.All.showmazo();
                                    Clients.Others.showVale4Botton();
                                    break;
                                case "vale4":
                                    Clients.All.showmazo();
                                    Clients.All.hideTrucoBotton();
                                    Clients.All.hideReTrucoBotton();
                                    Clients.All.hideVale4Botton();
                                    break;
                            }


                        }
                    }
                    else
                    {
                        Clients.Caller.hideEnvidoOptions();
                        switch (accion)
                        {
                            case "me voy al mazo":
                                break;                           
                            case "truco":
                                Clients.All.showmazo();
                                Clients.All.hideTrucoBotton();
                                Clients.Others.showReTrucoBotton();
                                break;
                            case "retruco":
                                Clients.All.showmazo();
                                Clients.Others.showVale4Botton();
                                break;
                            case "vale4":
                                Clients.All.showmazo();
                                Clients.All.hideTrucoBotton();
                                Clients.All.hideReTrucoBotton();
                                Clients.All.hideVale4Botton();
                                break;
                        }
                    }               
                }                
            }           
        }

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
    }
}