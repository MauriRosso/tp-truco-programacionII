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
        public int NumeroRonda { get; set; }

        Equipo Equipo1 = new Equipo();
        Equipo Equipo2 = new Equipo();

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

        public void AsignarMano(int CantidadJug, int NumeroRonda) //Este metodo la primera vez, asigna a cada jugador un valor Mano de 0 a CantJug, y despues le aumenta +1
        {
            for (int i = 0; i < (CantidadJug/2); i++)
            {
                if (NumeroRonda == 0)
                {
                    if (CantidadJug == 2)
                    {
                        if (i == 0)
                        {
                            Equipo1.ListaJugadores[i].Mano = 0;
                        }
                        else
                        {
                            Equipo2.ListaJugadores[i].Mano = 1;
                        }
                    }
                    if (CantidadJug == 4)
                    {
                        if (i == 0)
                        {
                            Equipo1.ListaJugadores[i].Mano = 0;
                            Equipo2.ListaJugadores[i].Mano = 1;
                        }                                                 
                        else
                        {
                            Equipo1.ListaJugadores[i].Mano = 2;
                            Equipo2.ListaJugadores[i].Mano = 3;
                        }
                    }
                }
                else
                {
                    if (CantidadJug == 2)
                    {
                        for (int x = 0; x < (CantidadJug / 2); x++)
                        {
                            Equipo1.ListaJugadores[i].Mano++;
                            Equipo2.ListaJugadores[i].Mano++;
                            if (Equipo1.ListaJugadores[i].Mano == CantidadJug)
                            {
                                Equipo1.ListaJugadores[i].Mano = 0;
                            }
                            if (Equipo2.ListaJugadores[i].Mano == CantidadJug)
                            {
                                Equipo2.ListaJugadores[i].Mano = 0;
                            }
                        }
                    }
                    if (CantidadJug == 4)
                    {
                        for (int z = 0; z < (CantidadJug / 2); z++)
                        {
                            Equipo1.ListaJugadores[i].Mano++;
                            Equipo2.ListaJugadores[i].Mano++;
                            if (Equipo1.ListaJugadores[i].Mano == CantidadJug)
                            {
                                Equipo1.ListaJugadores[i].Mano = 0;
                            }
                            if (Equipo2.ListaJugadores[i].Mano == CantidadJug)
                            {
                                Equipo2.ListaJugadores[i].Mano = 0;
                            }
                        }
                    }
                }
            }
        }
        public void JugarMano(int CantidadJug)
        {
            this.RepartirCartas(CantidadJug);
            this.AsignarMano(CantidadJug, NumeroRonda);
        }
        public void MetodoJugarGeneral(int CantidadJugadores)
        {
            //Aca van todos los metodos que vayamos haciendo para jugar.
            CrearJugadores(CantidadJugadores);
            JugarMano(CantidadJugadores);
        }
    }
}
