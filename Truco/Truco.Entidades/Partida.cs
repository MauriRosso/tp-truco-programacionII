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
        public void RepartirCartas(int CantidadJug)
        {
            Mazo MezclaMazo = new Mazo();
            MezclaMazo.MezclarCartas();

            for (int i = 0; i < CantidadJug; i++)
            {
                Jugador Jdr = new Jugador();
                for (int x = 0; x < 3; x++)
                {
                    foreach (var item in MezclaMazo.ListaOriginal) // preguntar porque se pone MezclaMazo.ListaOriginal y no ListaOriginal directamente
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
