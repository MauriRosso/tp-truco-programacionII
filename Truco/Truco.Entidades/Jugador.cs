using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Jugador
    {
        public string Nombre { get; set; }
        public string NombreInterno { get; set; }
        public int Orden { get; set; }
        public string IdConexion { get; set; }
        public bool CantoTruco { get; set; }
        public int Mano { get; set; }
        public bool Turno { get; set; } //Si esta en true, le toca jugar a ese jugador.
        public int PuntosEnvido { get; set; }
        public bool TieneFlor { get; set; }
        public int ID { get; set; }
        public string Equipo { get; set; } //Equipo del jugador

        public List<Cartas> ListaCartas = new List<Cartas>();
    }
}
