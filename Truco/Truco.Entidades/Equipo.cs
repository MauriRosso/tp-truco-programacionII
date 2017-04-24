using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    class Equipo
    {
        public string Nombre { get; set; }
        public int Puntos { get; set; }
        public List<Jugador> ListaJugadores = new List<Jugador>();
    }
}
