using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    class Jugador
    {
        public string Nombre { get; set; }
        public int Mano { get; set; }
        public List<Cartas> ListaCartas = new List<Cartas>();
    }
}
