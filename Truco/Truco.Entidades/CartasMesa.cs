using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class CartasMesa
    {
        public string IdJugador { get; set; } //id del jugador que tiro la carta
        public string Equipo { get; set; } //equipo del jugador que tiro la carta
        public int Mano { get; set; }//mano en la que jugo esa carta (1, 2 o 3)
        public Cartas Carta { get; set; }//carta que jugo el jugador
    }
}
