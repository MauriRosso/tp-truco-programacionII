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
        public void CrearJugadores(int CantJug)
        {
            for (int i = 0; i < CantJug; i++)
            {
                Jugador Jdr = new Jugador ();
                if (CantJug == 2)
                {
                    if (i == 0)
                    {
                        Jdr.Nombre = "Pedro";
                        Equipo1.ListaJugadores.Add(Jdr);   
                    }
                    else
                    {
                        Jdr.Nombre = "Juan";
                        Equipo2.ListaJugadores.Add(Jdr);   
                    }
                }
                else
                {
                    if (i == 0 || i == 1)
                    {
                        Jdr.Nombre = "Pedro";
                        Equipo1.ListaJugadores.Add(Jdr);
                        Jdr.Nombre = "Pablo";
                        Equipo1.ListaJugadores.Add(Jdr);
                    }
                    else
                    {
                        Jdr.Nombre = "Juan";
                        Equipo2.ListaJugadores.Add(Jdr);
                        Jdr.Nombre = "Jose";
                        Equipo2.ListaJugadores.Add(Jdr);
                    }
                }               
            }
        }
        public void RepartirCartas(int CantidadJug)
        {
            Mazo MezclaMazo = new Mazo();
            MezclaMazo.MezclarCartas();
            //REPARTO CARTAS EQUIPO 1.
            foreach (var Jug in Equipo1.ListaJugadores)
            {
                foreach (var Car in MezclaMazo.ListaOriginal)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Jug.ListaCartas.Add(Car);
                        MezclaMazo.ListaOriginal.Remove(Car);  
                    }                   
                    break;
                }
            }
            //REPARTO CARTAS EQUIPO 2.
            foreach (var Jug in Equipo2.ListaJugadores)
            {
                foreach (var Car in MezclaMazo.ListaOriginal)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Jug.ListaCartas.Add(Car);
                        MezclaMazo.ListaOriginal.Remove(Car);
                    }
                    break;
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
        public void JugarMano(int CantidadJug)
        {
            this.RepartirCartas(CantidadJug);
        }
        public void MetodoJugarGeneral(int CantidadJugadores)
        {
            //Aca van todos los metodos que vayamos haciendo para jugar.
            CrearJugadores(CantidadJugadores);
            JugarMano(CantidadJugadores);
        }
    }
}
