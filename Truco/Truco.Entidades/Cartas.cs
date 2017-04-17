using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    class Cartas
    {
        public Palos Palo { get; set; }
        public int Numero { get; set; }
    }
    enum Palos
    {
        Espada = 1,
        Basto = 2,
        Oro = 3,
        Copa = 4,
    }
}
