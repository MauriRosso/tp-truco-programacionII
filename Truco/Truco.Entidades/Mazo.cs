using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    class Mazo
    {
        public int CantidadCartas { get; set; }
        public List<Cartas> ListaOriginal { get; set; }
        public List<Cartas> ListaMezclada { get; set; }
        public enum Palo { Espada, Oro, Copa, Basto }
        public enum Valor { Uno, Dos, Tres, Cuatro, Cinco, Seis, Siete, Diez, Once, Doce }
        

        //Cartas ancho = new Cartas();
        //Cartas hembra = new Cartas();
        //Cartas sieteOro = new Cartas(); 


        //public void Inicializo()
        //{
        //    ancho.Numero = 1;
        //    ancho.Palo = Palos.Espada;
        //    hembra.Numero = 1;
        //    hembra.Palo = Palos.Basto;
        //    sieteOro.Numero = 7;
        //    sieteOro.Palo = Palos.Oro;

        //    ListaOriginal.Add(ancho);
        //    ListaOriginal.Add(hembra);
        //    ListaOriginal.Add(sieteOro);
            
        //}


        private static List<Cartas> DesordenarLista<Cartas>(List<Cartas> Input)
        {
            List<Cartas> ListaOriginal = Input;
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
    }
}
