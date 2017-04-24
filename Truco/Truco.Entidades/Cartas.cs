using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Cartas
    {
        public Palos Palo { get; set; }
        public int Numero { get; set; }
        public int Valor { get; set; }
    }
    public enum Palos
    {
        Espada,Basto,Oro,Copa,
    }
}
