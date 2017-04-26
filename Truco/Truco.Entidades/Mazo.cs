using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Mazo
    {
        //public int CantidadCartas { get; set; }
        public List<Cartas> ListaOriginal = new List<Cartas>();

        public  Mazo() //Cargo las cartas.
        {
            // CARTA 1    
            ListaOriginal.Add(new Cartas(Palos.Espada, 1, 14));
            ListaOriginal.Add(new Cartas(Palos.Basto, 1, 13));
            ListaOriginal.Add(new Cartas(Palos.Oro, 1, 8));
            ListaOriginal.Add(new Cartas(Palos.Copa, 1, 8));
            // CARTA 2 
            ListaOriginal.Add(new Cartas(Palos.Copa, 2, 9));
            ListaOriginal.Add(new Cartas(Palos.Basto, 2, 9));
            ListaOriginal.Add(new Cartas(Palos.Oro, 2, 9));
            ListaOriginal.Add(new Cartas(Palos.Espada, 2, 9));
            // CARTA 3 
            ListaOriginal.Add(new Cartas(Palos.Copa, 3, 10));
            ListaOriginal.Add(new Cartas(Palos.Basto, 3, 10));
            ListaOriginal.Add(new Cartas(Palos.Oro, 3, 10));
            ListaOriginal.Add(new Cartas(Palos.Espada, 3, 10));
            // CARTA 4 
            ListaOriginal.Add(new Cartas(Palos.Copa, 4, 1));
            ListaOriginal.Add(new Cartas(Palos.Oro, 4, 1));
            ListaOriginal.Add(new Cartas(Palos.Espada, 4, 1));
            ListaOriginal.Add(new Cartas(Palos.Basto, 4, 1));
            // CARTA 5 
            ListaOriginal.Add(new Cartas(Palos.Copa, 5, 2));
            ListaOriginal.Add(new Cartas(Palos.Oro, 5, 2));
            ListaOriginal.Add(new Cartas(Palos.Espada, 5, 2));
            ListaOriginal.Add(new Cartas(Palos.Basto, 5, 2));
            // CARTA 6 
            ListaOriginal.Add(new Cartas(Palos.Copa, 6, 3));
            ListaOriginal.Add(new Cartas(Palos.Oro, 6, 3));
            ListaOriginal.Add(new Cartas(Palos.Espada, 6, 3));
            ListaOriginal.Add(new Cartas(Palos.Basto, 6, 3));
            // CARTA 7 
            ListaOriginal.Add(new Cartas(Palos.Oro, 7, 11));
            ListaOriginal.Add(new Cartas(Palos.Espada, 7, 12));
            ListaOriginal.Add(new Cartas(Palos.Basto, 7, 4));
            ListaOriginal.Add(new Cartas(Palos.Copa, 7, 4));
            // CARTA 10
            ListaOriginal.Add(new Cartas(Palos.Copa, 10, 5));
            ListaOriginal.Add(new Cartas(Palos.Oro, 10, 5));
            ListaOriginal.Add(new Cartas(Palos.Espada, 10, 5));
            ListaOriginal.Add(new Cartas(Palos.Basto, 10, 5));
            // CARTA 11
            ListaOriginal.Add(new Cartas(Palos.Copa, 11, 6));
            ListaOriginal.Add(new Cartas(Palos.Oro, 11, 6));
            ListaOriginal.Add(new Cartas(Palos.Espada, 11, 6));
            ListaOriginal.Add(new Cartas(Palos.Basto, 11, 6));
            // CARTA 12
            ListaOriginal.Add(new Cartas(Palos.Copa, 12, 7));
            ListaOriginal.Add(new Cartas(Palos.Oro, 12, 7));
            ListaOriginal.Add(new Cartas(Palos.Espada, 12, 7));
            ListaOriginal.Add(new Cartas(Palos.Basto, 12, 7));
        }
        public List<Cartas> MezclarCartas()
        {
            List<Cartas> ListaMezclada = new List<Cartas>();
            Random RandNum = new Random();
            while (ListaOriginal.Count > 0)
            {
                int ran = RandNum.Next(0, ListaOriginal.Count - 1);
                ListaMezclada.Add(ListaOriginal[ran]);
                ListaOriginal.RemoveAt(ran);
            }
            ListaOriginal = ListaMezclada;
            return ListaOriginal;
        }
    }      
}

