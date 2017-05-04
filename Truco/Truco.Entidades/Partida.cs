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

        public void AsignarMano(int NroRonda) //ASIGNAMOS VALORES (como una especie de "prioridad"), PORQUE NECESITAMOS SABER QUIEN ES MANO DE QUIEN (cuando haya situaciones de empate, el que tenga el numero MAS ALTO en su propiedad "Mano", será el que gane / el que cante los puntos / etc.)
        {

                if (NroRonda == 0) //Si es la primera mano del juego
                {
                    Equipo1.ListaJugadores[0].Mano = 4;
                    Equipo2.ListaJugadores[0].Mano = 3;
                    Equipo1.ListaJugadores[1].Mano = 2;
                    Equipo2.ListaJugadores[1].Mano = 1;                 
                }
                else
                {
                    //Se incrementa +1 en el valor Mano de cada jugador.
                    foreach (var item in Equipo1.ListaJugadores)
                    {
                        if (item.Mano + 1 <= 4)
                        {
                            item.Mano += 1;  
                        }
                        else
                        {
                            item.Mano = 1;
                        }
                    }
                    foreach (var item in Equipo2.ListaJugadores)
                    {
                        if (item.Mano + 1 <= 4)
                        {
                            item.Mano += 1;
                        }
                        else
                        {
                            item.Mano = 1;
                        }   
                    }
                }
            // CONCLUSION: Para comenzar a jugar una mano, tenemos que preguntar que jugador tiene un valor "4" en su propiedad "Mano" (osea, el valor mas alto que se puede tener).
        }
        
        
        public void JugarMano(int CantidadJug)
        {
            this.RepartirCartas(CantidadJug);
            this.AsignarMano(NumeroRonda);

        }
        public void MetodoJugarGeneral(int CantidadJugadores)
        {
            //Aca van todos los metodos que vayamos haciendo para jugar.
            CrearJugadores(CantidadJugadores);
            JugarMano(CantidadJugadores);
        }
    }
}
