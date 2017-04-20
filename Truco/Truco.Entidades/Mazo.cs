using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Mazo
    {
        public int CantidadCartas { get; set; }
        public List<Cartas> ListaOriginal { get; set; }
        public List<Cartas> ListaMezclada { get; set; }

        public List<Cartas> DesordenarLista(List<Cartas> ListaOriginal)
        {
            List<Cartas> ListaMezclada = new List<Cartas>();

            Random RandNum = new Random();
            while (ListaOriginal.Count > 0)
            {
                int val = RandNum.Next(0, ListaOriginal.Count - 1);
                ListaMezclada.Add(ListaOriginal[val]);
                ListaOriginal.RemoveAt(val);
            }
            return ListaMezclada;
        }       
        public List<Cartas> RepartirCartas(List<Cartas> ListaMezclada)
        {
            ListaMezclada = DesordenarLista(ListaOriginal);
            foreach (var item in ListaMezclada)
            {
                  
            }
            return (RepartirCartas);
        }

        public Mazo mazo = new Mazo();
        public List<Mazo> CartasPlayer1 = new List<Mazo>();
        public List<Mazo> CartasPlayer2 = new List<Mazo>();
        public List<Mazo> CartasPlayer3 = new List<Mazo>();
        public List<Mazo> CartasPlayer4 = new List<Mazo>();

        public void RepartirCartasParaJugador()
        {
            for (int i = 0; i < 3; i++)
			{
                CartasPlayer1.Add(Mazo.RepartirCartas);
                CartasPlayer2.Add(Mazo.RepartirCartas);
                CartasPlayer3.Add(Mazo.RepartirCartas);
                CartasPlayer4.Add(Mazo.RepartirCartas);
            }
        }


        
    }
  

}
