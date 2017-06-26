using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Equipo
    {
        public string Nombre { get; set; }
        public int Puntos { get; set; }
        public int ManoGanada { get; set; }
        public List<Jugador> ListaJugadores = new List<Jugador>();
    }
}
