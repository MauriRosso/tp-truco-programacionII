using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Cartas
    {
        public Palos Palo { get;}
        public int Numero { get;}
        public int Valor { get;}

        public Cartas(Palos palo, int numero, int valor)
        {
            Palo = palo;
            Numero = numero;
            Valor = valor;
        }
    }
    public enum Palos
    {
        Espada,Basto,Oro,Copa,
    }

    
}
