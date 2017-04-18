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
        public Valores Valor { get; set; }

    }
    public enum Palos { Espada, Oro, Copa, Basto }
    public enum Valores { Uno, Dos, Tres, Cuatro, Cinco, Seis, Siete, Diez, Once, Doce }
}
