using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Mazo
    {
        //public int CantidadCartas { get; set; }
        public List<Cartas> ListaOriginal = new List<Cartas>();

        public  Mazo() //Cargo las cartas.
        {
            // CARTA 1    
            ListaOriginal.Add(new Cartas(Palos.Espada, 1, 14));
            ListaOriginal.Add(new Cartas(Palos.Basto, 1, 13));

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    ListaOriginal.Add(new Cartas(Palos.Oro, 1, 8));
                }
                else
                {
                    ListaOriginal.Add(new Cartas(Palos.Copa, 1, 8));
                }
                
            }
            // CARTA 2
            for (int i = 0; i < 4; i++)
            {
                //CartaCarga2.Valor = 9;
                if (i == 0)
                {
                    ListaOriginal.Add(new Cartas(Palos.Copa, 2, 9));
                }
                else
                {
                    if (i == 1)
                    {
                        ListaOriginal.Add(new Cartas(Palos.Espada, 2, 9));
                    }
                    else
                    {
                        if (i == 2)
                        {
                            ListaOriginal.Add(new Cartas(Palos.Basto, 2, 9));
                        }
                        else
                        {
                            ListaOriginal.Add(new Cartas(Palos.Oro, 2, 9));
                        }
                    }
                }
            }
            // CARTA 3
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    ListaOriginal.Add(new Cartas(Palos.Oro, 3, 10));
                }
                else
                {
                    if (i == 1)
                    {
                        ListaOriginal.Add(new Cartas(Palos.Espada, 3, 10));
                    }
                    else
                    {
                        if (i == 2)
                        {
                            ListaOriginal.Add(new Cartas(Palos.Basto, 3, 10));
                        }
                        else
                        {
                            ListaOriginal.Add(new Cartas(Palos.Copa, 3, 10));
                        }
                    }
                }
                ListaOriginal.Add(CartaCarga3);
            }
            // CARTA 4
            for (int i = 0; i < 4; i++)
            {
                Cartas CartaCarga4 = new Cartas();
                CartaCarga4.Numero = 4;
                CartaCarga4.Valor = 1;
                if (i == 0)
                {
                    CartaCarga4.Palo = Palos.Oro;
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga4.Palo = Palos.Copa;
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga4.Palo = Palos.Espada;
                        }
                        else
                        {
                            CartaCarga4.Palo = Palos.Basto;
                        }
                    }
                }
                ListaOriginal.Add(CartaCarga4);
            }
            // CARTA 5
            for (int i = 0; i < 4; i++)
            {
                Cartas CartaCarga5 = new Cartas();
                CartaCarga5.Numero = 5;
                CartaCarga5.Valor = 2;
                if (i == 0)
                {
                    CartaCarga5.Palo = Palos.Espada;
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga5.Palo = Palos.Copa;
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga5.Palo = Palos.Oro;
                        }
                        else
                        {
                            CartaCarga5.Palo = Palos.Basto;
                        }
                    }
                }
                ListaOriginal.Add(CartaCarga5);
            }
            // CARTA 6
            for (int i = 0; i < 4; i++)
            {
                Cartas CartaCarga6 = new Cartas();
                CartaCarga6.Numero = 6;
                CartaCarga6.Valor = 3;
                if (i == 0)
                {
                    CartaCarga6.Palo = Palos.Espada;
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga6.Palo = Palos.Copa;
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga6.Palo = Palos.Oro;
                        }
                        else
                        {
                            CartaCarga6.Palo = Palos.Basto;
                        }
                    }
                }
                ListaOriginal.Add(CartaCarga6);
            }
            //CARTA 7
            for (int i = 0; i < 2; i++)
            {
                Cartas CartaCarga7 = new Cartas();
                CartaCarga7.Numero = 7;
                CartaCarga7.Valor = 4;
                if (i == 0)
                {
                    CartaCarga7.Palo = Palos.Copa;
                }
                else
                {
                    CartaCarga7.Palo = Palos.Basto;
                }
                ListaOriginal.Add(CartaCarga7);
            }
            Cartas CartaCarga7oro = new Cartas();
            CartaCarga7oro.Numero = 7;
            CartaCarga7oro.Valor = 11;
            CartaCarga7oro.Palo = Palos.Oro;
            ListaOriginal.Add(CartaCarga7oro);

            Cartas CartaCarga7espada = new Cartas();
            CartaCarga7espada.Numero = 7;
            CartaCarga7espada.Valor = 12;
            CartaCarga7espada.Palo = Palos.Espada;
            ListaOriginal.Add(CartaCarga7espada);

            //CARTA 10
            for (int i = 0; i < 4; i++)
            {
                Cartas CartaCarga10 = new Cartas();
                CartaCarga10.Numero = 10;
                CartaCarga10.Valor = 5;
                if (i == 0)
                {
                    CartaCarga10.Palo = Palos.Espada;
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga10.Palo = Palos.Copa;
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga10.Palo = Palos.Oro;
                        }
                        else
                        {
                            CartaCarga10.Palo = Palos.Basto;
                        }
                    }
                }
                ListaOriginal.Add(CartaCarga10);
            }
            //CARTA 11
            for (int i = 0; i < 4; i++)
            {
                Cartas CartaCarga11 = new Cartas();
                CartaCarga11.Numero = 11;
                CartaCarga11.Valor = 6;
                if (i == 0)
                {
                    CartaCarga11.Palo = Palos.Basto;
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga11.Palo = Palos.Copa;
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga11.Palo = Palos.Oro;
                        }
                        else
                        {
                            CartaCarga11.Palo = Palos.Espada;
                        }
                    }
                }
                ListaOriginal.Add(CartaCarga11);
            }
            //CARTA 12
            for (int i = 0; i < 4; i++)
            {
                Cartas CartaCarga12 = new Cartas();
                CartaCarga12.Numero = 12;
                CartaCarga12.Valor = 7;
                if (i == 0)
                {
                    CartaCarga12.Palo = Palos.Oro;
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga12.Palo = Palos.Espada;
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga12.Palo = Palos.Basto;
                        }
                        else
                        {
                            CartaCarga12.Palo = Palos.Copa;
                        }
                    }
                }
                ListaOriginal.Add(CartaCarga12);
            }
        }
        public List<Cartas> MezclarCartas()
        {
            List<Cartas> ListaMezclada = new List<Cartas>();
            Random RandNum = new Random();
            while (ListaOriginal.Count > 0)
            {
                int ran = RandNum.Next(0, ListaOriginal.Count - 1);
                ListaMezclada.Add(ListaOriginal[ran]);
                ListaOriginal.RemoveAt(ran);
            }
            ListaOriginal = ListaMezclada;
            return ListaOriginal;
        }
    }      
}

