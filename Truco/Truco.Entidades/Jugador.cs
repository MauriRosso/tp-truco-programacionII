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
        public bool Turno { get; set; } //Si esta en true, le toca jugar a ese jugador.
        public int PuntosEnvido { get; set; }
        public bool TieneFlor { get; set; }
        public int ID { get; set; }

        public List<Cartas> ListaCartas = new List<Cartas>();
    }
}
