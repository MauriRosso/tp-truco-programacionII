using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    class Acciones
    {
        public bool Quiero { get; set; }
        public bool NoQuiero { get; set; }
        public bool MeVoyAlMazo { get; set; }
        public bool Truco { get; set; }
        public bool ReTruco { get; set; }
        public bool ValeCuatro { get; set; }
        public bool Envido { get; set; }
        public bool RealEnvido { get; set; }
        public bool FaltaEnvido { get; set; }
        public bool Flor { get; set; }
        public bool ContraFlor { get; set; }
        public bool MostrarCartas { get; set; } //Muestra las cartas al final de la partida

    }
}
