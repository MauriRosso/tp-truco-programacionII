using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
    public class Partida
    {
        public int ID { get;}
        public int CantidadJugadores { get; set; }
        public string Nombre { get; set; }
        public int NumeroRonda { get; set; }

        Equipo Equipo1 = new Equipo();
        Equipo Equipo2 = new Equipo();

        public void CrearJugadores(int CantJug)
        {
            for (int i = 0; i < CantJug; i++)
            {
                Jugador Jdr = new Jugador ();
                if (CantJug == 2)
                {
                    if (i == 0)
                    {
                        Jdr.Nombre = "Pedro";
                        Equipo1.ListaJugadores.Add(Jdr);   
                    }
                    else
                    {
                        Jdr.Nombre = "Juan";
                        Equipo2.ListaJugadores.Add(Jdr);   
                    }
                }
                else
                {
                    if (i == 0 || i == 1)
                    {
                        Jdr.Nombre = "Pedro";
                        Equipo1.ListaJugadores.Add(Jdr);
                        Jdr.Nombre = "Pablo";
                        Equipo1.ListaJugadores.Add(Jdr);
                    }
                    else
                    {
                        Jdr.Nombre = "Juan";
                        Equipo2.ListaJugadores.Add(Jdr);
                        Jdr.Nombre = "Jose";
                        Equipo2.ListaJugadores.Add(Jdr);
                    }
                }               
            }
        }
        public void RepartirCartas(int CantidadJug)
        {
            Mazo MezclaMazo = new Mazo();
            MezclaMazo.MezclarCartas();
            //REPARTO CARTAS EQUIPO 1.
            foreach (var Jug in Equipo1.ListaJugadores)
            {
                foreach (var Car in MezclaMazo.ListaOriginal)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Jug.ListaCartas.Add(Car);
                        MezclaMazo.ListaOriginal.Remove(Car);  
                    }                   
                    break;
                }
            }
            //REPARTO CARTAS EQUIPO 2.
            foreach (var Jug in Equipo2.ListaJugadores)
            {
                foreach (var Car in MezclaMazo.ListaOriginal)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Jug.ListaCartas.Add(Car);
                        MezclaMazo.ListaOriginal.Remove(Car);
                    }
                    break;
                }
            }           
        }

        public void AsignarMano(int NroRonda) //ASIGNAMOS VALORES (como una especie de "prioridad"), PORQUE NECESITAMOS SABER QUIEN ES MANO DE QUIEN (cuando haya situaciones de empate, el que tenga el numero MAS ALTO en su propiedad "Mano", será el que gane / el que cante los puntos / etc.)
        {

                if (NroRonda == 0) //Si es la primera mano del juego
                {
                    Equipo1.ListaJugadores[0].Mano = 4;
                    Equipo2.ListaJugadores[0].Mano = 3;
                    Equipo1.ListaJugadores[1].Mano = 2;
                    Equipo2.ListaJugadores[1].Mano = 1;                 
                }
                else
                {
                    //Se incrementa +1 en el valor Mano de cada jugador.
                    foreach (var item in Equipo1.ListaJugadores)
                    {
                        if (item.Mano + 1 <= 4)
                        {
                            item.Mano += 1;  
                        }
                        else
                        {
                            item.Mano = 1;
                        }
                    }
                    foreach (var item in Equipo2.ListaJugadores)
                    {
                        if (item.Mano + 1 <= 4)
                        {
                            item.Mano += 1;
                        }
                        else
                        {
                            item.Mano = 1;
                        }   
                    }
                }
            // CONCLUSION: Para comenzar a jugar una mano, tenemos que preguntar que jugador tiene un valor "4" en su propiedad "Mano" (osea, el valor mas alto que se puede tener).
        }

        public void CalcularPuntosEnvido() //Este metodo calcula los puntos del envido de cada jugador de los equipos (Valores del 0 al 33).
        {           
            foreach (var item in Equipo1.ListaJugadores)
            {
                if (item.ListaCartas[0].Palo == item.ListaCartas[1].Palo && item.ListaCartas[0].Palo == item.ListaCartas[2].Palo) //con esta pregunta averiguo si tiene flor el jugador.
                {
                    item.TieneFlor = true;   
                }
                else //No tiene flor, calculo los puntos del envido.
                {
                    item.TieneFlor = false;
                    if (item.ListaCartas[0].Palo == item.ListaCartas[1].Palo || item.ListaCartas[0].Palo == item.ListaCartas[2].Palo || item.ListaCartas[1].Palo == item.ListaCartas[2].Palo) //Pregunto si hay 2 cartas que tengan el mismo palo
                    {
                        //COMPARO 1er carta con 2da carta
                        if (item.ListaCartas[0].Palo == item.ListaCartas[1].Palo)
                        {
                            if (item.ListaCartas[0].Numero >= 10 && item.ListaCartas[1].Numero < 10) //La primera es carta negra y la segunda no
                            {
                                item.PuntosEnvido = item.ListaCartas[1].Numero + 20;
                            }
                            else
                            {
                                if (item.ListaCartas[0].Numero < 10 && item.ListaCartas[1].Numero >= 10)// La segunda es carta negra y la primera no
                                {
                                    item.PuntosEnvido = item.ListaCartas[0].Numero + 20;   
                                }
                                else
                                {
                                    if (item.ListaCartas[0].Numero >= 10 && item.ListaCartas[1].Numero >= 10)//Las dos son cartas negras
                                    {
                                        item.PuntosEnvido = 20;          
                                    }
                                    else //Ninguna de las 2 son cartas negras
                                    {
                                        item.PuntosEnvido = item.ListaCartas[0].Numero + item.ListaCartas[1].Numero + 20;
                                    }
                                }
                            }
                        }  
                        else
                        {
                            //COMPARO 1er carta con 3er carta
                            if (item.ListaCartas[0].Palo == item.ListaCartas[2].Palo)
                            {
                                if (item.ListaCartas[0].Numero >= 10 && item.ListaCartas[2].Numero < 10) //La primera es carta negra y la tercera no
                                {
                                    item.PuntosEnvido = item.ListaCartas[2].Numero + 20;
                                }
                                else
                                {
                                    if (item.ListaCartas[0].Numero < 10 && item.ListaCartas[2].Numero >= 10)// La tercera es carta negra y la primera no
                                    {
                                        item.PuntosEnvido = item.ListaCartas[0].Numero + 20;
                                    }
                                    else
                                    {
                                        if (item.ListaCartas[0].Numero >= 10 && item.ListaCartas[2].Numero >= 10)//Las dos son cartas negras
                                        {
                                            item.PuntosEnvido = 20;
                                        }
                                        else //Ninguna de las 2 son cartas negras
                                        {
                                            item.PuntosEnvido = item.ListaCartas[0].Numero + item.ListaCartas[2].Numero + 20;
                                        }
                                    }
                                }       
                            }
                            else
                            //Por descarte, la carta 2 y 3 son del mismo palo
                            {
                                if (item.ListaCartas[1].Numero >= 10 && item.ListaCartas[2].Numero < 10) //La segunda es carta negra y la tercera no
                                {
                                    item.PuntosEnvido = item.ListaCartas[2].Numero + 20;
                                }
                                else
                                {
                                    if (item.ListaCartas[1].Numero < 10 && item.ListaCartas[2].Numero >= 10)// La tercera es carta negra y la segunda no
                                    {
                                        item.PuntosEnvido = item.ListaCartas[1].Numero + 20;
                                    }
                                    else
                                    {
                                        if (item.ListaCartas[1].Numero >= 10 && item.ListaCartas[2].Numero >= 10)//Las dos son cartas negras
                                        {
                                            item.PuntosEnvido = 20;
                                        }
                                        else //Ninguna de las 2 son cartas negras
                                        {
                                            item.PuntosEnvido = item.ListaCartas[1].Numero + item.ListaCartas[2].Numero + 20;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else //Tiene 3 cartas de distinto palo
                    {  
                        int NumMayor = 0;
                        int x = 0;
                        foreach (var Carta in item.ListaCartas)
                        {
                            if (x == 0) //Si es la primer carta
                            {
                                if (Carta.Numero < 10) //Pregunto si es carta negra (10, 11 o 12) (porque las cartas negras por si solas, valen 0)
                                {
                                    NumMayor = Carta.Numero;
                                }   
                                else
                                {
                                    NumMayor = 0;
                                }
                            }                          
                            else
                            {
                                if (Carta.Numero < 10)
                                {
                                    if (Carta.Numero > NumMayor)
                                    {
                                        NumMayor = Carta.Numero;
                                    }   
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in Equipo2.ListaJugadores)
            {
                if (item.ListaCartas[0].Palo == item.ListaCartas[1].Palo && item.ListaCartas[0].Palo == item.ListaCartas[2].Palo) //con esta pregunta averiguo si tiene flor el jugador.
                {
                    item.TieneFlor = true;
                }
                else //No tiene flor, calculo los puntos del envido.
                {
                    item.TieneFlor = false;
                    if (item.ListaCartas[0].Palo == item.ListaCartas[1].Palo || item.ListaCartas[0].Palo == item.ListaCartas[2].Palo || item.ListaCartas[1].Palo == item.ListaCartas[2].Palo) //Pregunto si hay 2 cartas que tengan el mismo palo
                    {
                        //COMPARO 1er carta con 2da carta
                        if (item.ListaCartas[0].Palo == item.ListaCartas[1].Palo)
                        {
                            if (item.ListaCartas[0].Numero >= 10 && item.ListaCartas[1].Numero < 10) //La primera es carta negra y la segunda no
                            {
                                item.PuntosEnvido = item.ListaCartas[1].Numero + 20;
                            }
                            else
                            {
                                if (item.ListaCartas[0].Numero < 10 && item.ListaCartas[1].Numero >= 10)// La segunda es carta negra y la primera no
                                {
                                    item.PuntosEnvido = item.ListaCartas[0].Numero + 20;
                                }
                                else
                                {
                                    if (item.ListaCartas[0].Numero >= 10 && item.ListaCartas[1].Numero >= 10)//Las dos son cartas negras
                                    {
                                        item.PuntosEnvido = 20;
                                    }
                                    else //Ninguna de las 2 son cartas negras
                                    {
                                        item.PuntosEnvido = item.ListaCartas[0].Numero + item.ListaCartas[1].Numero + 20;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //COMPARO 1er carta con 3er carta
                            if (item.ListaCartas[0].Palo == item.ListaCartas[2].Palo)
                            {
                                if (item.ListaCartas[0].Numero >= 10 && item.ListaCartas[2].Numero < 10) //La primera es carta negra y la tercera no
                                {
                                    item.PuntosEnvido = item.ListaCartas[2].Numero + 20;
                                }
                                else
                                {
                                    if (item.ListaCartas[0].Numero < 10 && item.ListaCartas[2].Numero >= 10)// La tercera es carta negra y la primera no
                                    {
                                        item.PuntosEnvido = item.ListaCartas[0].Numero + 20;
                                    }
                                    else
                                    {
                                        if (item.ListaCartas[0].Numero >= 10 && item.ListaCartas[2].Numero >= 10)//Las dos son cartas negras
                                        {
                                            item.PuntosEnvido = 20;
                                        }
                                        else //Ninguna de las 2 son cartas negras
                                        {
                                            item.PuntosEnvido = item.ListaCartas[0].Numero + item.ListaCartas[2].Numero + 20;
                                        }
                                    }
                                }
                            }
                            else
                            //Por descarte, la carta 2 y 3 son del mismo palo
                            {
                                if (item.ListaCartas[1].Numero >= 10 && item.ListaCartas[2].Numero < 10) //La segunda es carta negra y la tercera no
                                {
                                    item.PuntosEnvido = item.ListaCartas[2].Numero + 20;
                                }
                                else
                                {
                                    if (item.ListaCartas[1].Numero < 10 && item.ListaCartas[2].Numero >= 10)// La tercera es carta negra y la segunda no
                                    {
                                        item.PuntosEnvido = item.ListaCartas[1].Numero + 20;
                                    }
                                    else
                                    {
                                        if (item.ListaCartas[1].Numero >= 10 && item.ListaCartas[2].Numero >= 10)//Las dos son cartas negras
                                        {
                                            item.PuntosEnvido = 20;
                                        }
                                        else //Ninguna de las 2 son cartas negras
                                        {
                                            item.PuntosEnvido = item.ListaCartas[1].Numero + item.ListaCartas[2].Numero + 20;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else //Tiene 3 cartas de distinto palo
                    {
                        int NumMayor = 0;
                        int x = 0;
                        foreach (var Carta in item.ListaCartas)
                        {
                            if (x == 0) //Si es la primer carta
                            {
                                if (Carta.Numero < 10) //Pregunto si es carta negra (10, 11 o 12) (porque las cartas negras por si solas, valen 0)
                                {
                                    NumMayor = Carta.Numero;
                                }
                                else
                                {
                                    NumMayor = 0;
                                }
                            }
                            else
                            {
                                if (Carta.Numero < 10)
                                {
                                    if (Carta.Numero > NumMayor)
                                    {
                                        NumMayor = Carta.Numero;
                                    }
                                }
                            }
                        }
                        item.PuntosEnvido = NumMayor;
                    }
                }
            }
        }
        public void JugarMano()
        {
            
        }
        
        public void PrepararMano(int CantidadJug)
        {
            this.RepartirCartas(CantidadJug);
            this.AsignarMano(NumeroRonda);
            this.CalcularPuntosEnvido();
        }
        public void MetodoJugarGeneral(int CantidadJugadores)
        {
            //Aca van todos los metodos que vayamos haciendo para jugar.
            CrearJugadores(CantidadJugadores);
            PrepararMano(CantidadJugadores);
        }
    }
}
