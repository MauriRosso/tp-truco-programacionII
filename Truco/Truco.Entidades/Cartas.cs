using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Cartas
    {
        public Palos Palo { get; }
        public int Numero { get; }
        public int Valor { get; }
        public string Codigo { get; set; }
        public string Imagen { get; set; }

        public Cartas(Palos palo, int numero, int valor)
        {
            Palo = palo;
            Numero = numero;
            Valor = valor;
            Codigo = string.Format("{0}{1}", numero.ToString(), palo.ToString());
            Imagen = string.Format("Images/{1}{0}.jpg", numero.ToString(), palo.ToString()[0]);
        }
    }
    public enum Palos
    {
        Espada,Basto,Oro,Copa
    }

    
}
