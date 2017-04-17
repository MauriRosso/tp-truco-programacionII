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
        public int Valor { get; set; }
        public List<Cartas> ListaOriginal { get; set; }
        public List<Cartas> ListaMezclada { get; set; }
        

        Cartas ancho = new Cartas();
        Cartas hembra = new Cartas();
        Cartas sieteOro = new Cartas();


        public void Inicializo()
        {
            ancho.Numero = 1;
            ancho.Palo = Palos.Espada;
            hembra.Numero = 1;
            hembra.Palo = Palos.Basto;
            sieteOro.Numero = 7;
            sieteOro.Palo = Palos.Oro;

            ListaOriginal.Add(ancho);
            ListaOriginal.Add(hembra);
            ListaOriginal.Add(sieteOro);

            
        }
        public void MezclarCartas()
        {
            Inicializo();
            for (int i = 0; i < ListaOriginal.Count(); i++)
            {
                Random Ran = new Random();
                
                
            }
            
            

        }
    }
}
