using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Partida
    {
        public int ID { get;}
        public int CantidadJugadores { get; set; }
        public string Nombre { get; set; }

        //Donde me marca error es porque tengo que recorrer la lista de equipos y agregarle las propiedades que faltan
        Equipo Equipo1 = new Equipo();
        Equipo Equipo2 = new Equipo();

        //CONSULTA: SI EN EL METODO CREAR JUGADOR, INSTANCIO X CANT DE JUGADORES, IGUAL QUE EN EL METODO REPARTIRCARTAS Y ASIGNAR MANO, SON TODOS JUGADORES DIFERENTES? DIGAMOS QUE SE CREARIAN SIEMPRE JUGADORES DIFERETES
        //ENTONCES NO HABRIA QEU HACER UN METODO O ALGO QUE CREE LOS JUGADORES NECESARIOS PARA LA PARTIDA Y DESPUES USARLO EN TODOS LOS OTROS METODOS
        //public void CrearJugador(int CantidadJug)
        //{
        //    for (int i = 0; i < CantidadJug; i++)
        //    {
        //        //Jugador jugador[i] = new Jugador();
        //        Jugador jugador = new Jugador();
        //    }
        //}
        public void RepartirCartas(int CantidadJug)
        {
            Mazo MezclaMazo = new Mazo();
            MezclaMazo.MezclarCartas();

            for (int i = 0; i < CantidadJug; i++)
            {
                Jugador Jdr = new Jugador();
                for (int x = 0; x < 3; x++)
                {
                    foreach (var item in MezclaMazo.ListaOriginal)
                    {
                        if (item != null) //Si la carta esta disponible.
                        {
                            Jdr.ListaCartas.Add(item);
                            MezclaMazo.ListaOriginal.Remove(item);
                            break;
                        }
                    }
                }
                if (CantidadJug == 2)
                {
                    if (i == 0)
                    {
                        Equipo1.ListaJugadores.Add(Jdr);
                    }
                    else
                    {
                        Equipo2.ListaJugadores.Add(Jdr);
                    }
                }
                else
                {
                    if (i == 0 || i == 1)
                    {
                        Equipo1.ListaJugadores.Add(Jdr);
                    }
                    else
                    {
                        Equipo2.ListaJugadores.Add(Jdr);
                    }
                }
            }
        }

        //public void AsignarMano(int CantidadJug, int NumeroRonda) //Este metodo la primera vez, asigna a cada jugador un valor Mano de 0 a CantJug, y despues le aumenta +1
        //{
        //    for (int i = 0; i < CantidadJug; i++)
        //    {
        //        Jugador jugador = new Jugador();
        //        if (NumeroRonda == 0)
        //        {
        //            if (CantidadJug == 2)
        //            {
        //                if (i == 0)
        //                {
        //                    //jugador[i].mano = 0
        //                    jugador.Mano = 0;
        //                }
        //                else
        //                {
        //                    jugador.Mano = 1;
        //                }
        //            }
        //            if (CantidadJug == 4)
        //            {
        //                if (i == 0)
        //                {
        //                    jugador.Mano = 0;
        //                }
        //                else if (i == 1)
        //                {
        //                    jugador.Mano = 1;
        //                }
        //                else if (i == 2)
        //                {
        //                    jugador.Mano = 2;
        //                }
        //                else
        //                {
        //                    jugador.Mano = 3;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (CantidadJug == 2)
        //            {
        //                for (int x = 0; x < CantidadJug; x++)
        //                {
        //                    jugador.Mano++;
        //                    if (jugador.Mano == CantidadJug)
        //                    {
        //                        jugador.Mano = 0;
        //                    }
        //                }
        //            }
        //            if (CantidadJug == 4)
        //            {
        //                for (int z = 0; z < CantidadJug; z++)
        //                {
        //                    jugador.Mano++;
        //                    if (jugador.Mano == CantidadJug)
        //                    {
        //                        jugador.Mano = 0;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        public void ComenzarPartida(int CantidadJug)
        {
            Equipo Equipo = new Equipo();
            Mazo CartasMazo = new Mazo();
            this.RepartirCartas(CantidadJug);
        }
        public void MetodoJugarGeneral(int CantidadJugadores) //Este metodo tiene que tener todos los metodos necesarios para jugar una mano.
        {
            //Aca van todos los metodos que vayamos haciendo para jugar.
            ComenzarPartida(CantidadJugadores);
        }
    }
}
