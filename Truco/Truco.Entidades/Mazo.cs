using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Mazo
    {
        public int CantidadCartas { get; set; }
        public List<Cartas> ListaOriginal { get; set; }
        public List<Cartas> ListaMezclada { get; set; }

        public void Inicializo() //Cargo las cartas.
        {
            // CARTA 1
            Cartas CartaCarga1espada = new Cartas();
            CartaCarga1espada.Numero = 1;
            CartaCarga1espada.Palo = "Espada";
            CartaCarga1espada.Valor = 14;  //VALOR MAXIMO DE UNA CARTA

            ListaOriginal.Add(CartaCarga1espada);

            Cartas CartaCarga1basto = new Cartas();
            CartaCarga1basto.Numero = 1;
            CartaCarga1basto.Palo = "Basto";
            CartaCarga1basto.Valor = 13;

            ListaOriginal.Add(CartaCarga1basto);

            for (int i = 0; i < 2; i++)
            {
                Cartas CartaCarga1restantes = new Cartas();
                CartaCarga1restantes.Numero = 1;
                CartaCarga1restantes.Valor = 8;
                if (i == 0)
                {
                    CartaCarga1restantes.Palo = "Oro";
                }
                else
                {
                    CartaCarga1restantes.Palo = "Copa";
                }
                ListaOriginal.Add(CartaCarga1restantes);
            }
            // CARTA 2
            for (int i = 0; i < 4; i++)
            {
                Cartas CartaCarga2 = new Cartas();
                CartaCarga2.Numero = 2;
                CartaCarga2.Valor = 9;
                if (i == 0)
                {
                    CartaCarga2.Palo = "Oro";
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga2.Palo = "Copa";
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga2.Palo = "Espada";
                        }
                        else
                        {
                            CartaCarga2.Palo = "Basto";
                        }
                    }
                }
                ListaOriginal.Add(CartaCarga2);
            }
            // CARTA 3
            for (int i = 0; i < 4; i++)
            {
                Cartas CartaCarga3 = new Cartas();
                CartaCarga3.Numero = 3;
                CartaCarga3.Valor = 10;
                if (i == 0)
                {
                    CartaCarga3.Palo = "Oro";
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga3.Palo = "Copa";
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga3.Palo = "Espada";
                        }
                        else
                        {
                            CartaCarga3.Palo = "Basto";
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
                    CartaCarga4.Palo = "Espada";
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga4.Palo = "Copa";
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga4.Palo = "Oro";
                        }
                        else
                        {
                            CartaCarga4.Palo = "Basto";
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
                    CartaCarga5.Palo = "Espada";
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga5.Palo = "Copa";
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga5.Palo = "Oro";
                        }
                        else
                        {
                            CartaCarga5.Palo = "Basto";
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
                    CartaCarga6.Palo = "Espada";
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga6.Palo = "Copa";
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga6.Palo = "Oro";
                        }
                        else
                        {
                            CartaCarga6.Palo = "Basto";
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
                    CartaCarga7.Palo = "Copa";
                }
                else
                {
                    CartaCarga7.Palo = "Basto";
                }
                ListaOriginal.Add(CartaCarga7);
            }
            Cartas CartaCarga7oro = new Cartas();
            CartaCarga7oro.Numero = 7;
            CartaCarga7oro.Valor = 11;
            CartaCarga7oro.Palo = "Oro";
            ListaOriginal.Add(CartaCarga7oro);

            Cartas CartaCarga7espada = new Cartas();
            CartaCarga7espada.Numero = 7;
            CartaCarga7espada.Valor = 12;
            CartaCarga7espada.Palo = "Espada";
            ListaOriginal.Add(CartaCarga7espada);

            //CARTA 10
            for (int i = 0; i < 4; i++)
            {
                Cartas CartaCarga10 = new Cartas();
                CartaCarga10.Numero = 10;
                CartaCarga10.Valor = 5;
                if (i == 0)
                {
                    CartaCarga10.Palo = "Espada";
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga10.Palo = "Copa";
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga10.Palo = "Oro";
                        }
                        else
                        {
                            CartaCarga10.Palo = "Basto";
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
                    CartaCarga11.Palo = "Espada";
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga11.Palo = "Copa";
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga11.Palo = "Oro";
                        }
                        else
                        {
                            CartaCarga11.Palo = "Basto";
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
                    CartaCarga12.Palo = "Espada";
                }
                else
                {
                    if (i == 1)
                    {
                        CartaCarga12.Palo = "Copa";
                    }
                    else
                    {
                        if (i == 2)
                        {
                            CartaCarga12.Palo = "Oro";
                        }
                        else
                        {
                            CartaCarga12.Palo = "Basto";
                        }
                    }
                }
                ListaOriginal.Add(CartaCarga12);
            }
        }      
    }
}
