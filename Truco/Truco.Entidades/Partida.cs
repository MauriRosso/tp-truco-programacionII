using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truco.Entidades
{
	public class Partida
	{
		public int ID { get; set; }
		public int CantidadJugadores { get; set; }
		public string Nombre { get; set; }
		public int NumeroRonda { get; set; } // Numero de rondas que se jugaron en la partida
        public int NumeroMano { get; set; }// Numero x de las 3 manos
        public int CartasJugadas { get; set; } // Numero de cartas jugadas en la mano

        public List<Jugador> ListaJugadores = new List<Jugador>();
        public List<CartasMesa> ListaCartasJugadas = new List<CartasMesa>();
        public Equipo Equipo1 = new Equipo();
        public Equipo Equipo2 = new Equipo();

       
        public void RepartirCartas(int CantidadJug)
		{
			Mazo MezclaMazo = new Mazo();
			MezclaMazo.MezclarCartas();
			//REPARTO CARTAS EQUIPO 1.
			foreach (var Jug in Equipo1.ListaJugadores)
			{
				for (int i = 0; i < 3; i++)
				{
					foreach (var Car in MezclaMazo.ListaOriginal)
					{
						Jug.ListaCartas.Add(Car);
						MezclaMazo.ListaOriginal.Remove(Car);
						break;
					}
				}
			}
			//REPARTO CARTAS EQUIPO 2.
			foreach (var Jug in Equipo2.ListaJugadores)
			{
				for (int i = 0; i < 3; i++)
				{
					foreach (var Car in MezclaMazo.ListaOriginal)
					{
						Jug.ListaCartas.Add(Car);
						MezclaMazo.ListaOriginal.Remove(Car);
						break;
					}
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
								x = 1;
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
								x = 1;
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
		private void MetodoEnvido()
		{
			int mayorEquipo1 = Equipo1.ListaJugadores.Max(x1 => x1.PuntosEnvido);
			int idMayorEquipo1 = Equipo1.ListaJugadores.Find(Jug => Jug.PuntosEnvido == mayorEquipo1).ID;//Saco el mayor del equipo 1

			int mayorEquipo2 = Equipo2.ListaJugadores.Max(x2 => x2.PuntosEnvido);
			int idMayorEquipo2 = Equipo2.ListaJugadores.Find(Jug => Jug.PuntosEnvido == mayorEquipo2).ID;//Saco el mayor del equipo 2

			if (mayorEquipo1 > mayorEquipo2)
			{
				Equipo1.Puntos += 2;
			}
			else if (mayorEquipo2 > mayorEquipo1)
			{
				Equipo2.Puntos += 2;
			}
			else
			{
				int NroMano1 = 0;
				int NroMano2 = 0;
				foreach (var JugE1 in Equipo1.ListaJugadores)
				{
					if (JugE1.ID == idMayorEquipo1)
					{
						NroMano1 = JugE1.Mano;
					}
				}
				foreach (var JugE2 in Equipo2.ListaJugadores)
				{
					if (JugE2.ID == idMayorEquipo2)
					{
						NroMano2 = JugE2.Mano;
					}
				}

				if (NroMano1 > NroMano2)
				{
					Equipo1.Puntos += 2;
				}
				else
				{
					Equipo2.Puntos += 2;
				}
			}

		}

		public void JugarMano(int CantJug)
		{
			PrepararMano(CantJug);
			Acciones Acc = new Acciones();
			if (Acc.Envido)
			{
				MetodoEnvido();             
			}
		}

        private void MetodoDobleEnvido()
        {
            int mayorEquipo1 = Equipo1.ListaJugadores.Max(x1 => x1.PuntosEnvido);
            int idMayorEquipo1 = Equipo1.ListaJugadores.Find(Jug => Jug.PuntosEnvido == mayorEquipo1).ID;//Saco el mayor del equipo 1

            int mayorEquipo2 = Equipo2.ListaJugadores.Max(x2 => x2.PuntosEnvido);
            int idMayorEquipo2 = Equipo2.ListaJugadores.Find(Jug => Jug.PuntosEnvido == mayorEquipo2).ID;//Saco el mayor del equipo 2

            if (mayorEquipo1 > mayorEquipo2)
            {
                Equipo1.Puntos += 4;
            }
            else if (mayorEquipo2 > mayorEquipo1)
            {
                Equipo2.Puntos += 4;
            }
            else
            {
                int NroMano1 = 0;
                int NroMano2 = 0;
                foreach (var JugE1 in Equipo1.ListaJugadores)
                {
                    if (JugE1.ID == idMayorEquipo1)
                    {
                        NroMano1 = JugE1.Mano;
                    }
                }
                foreach (var JugE2 in Equipo2.ListaJugadores)
                {
                    if (JugE2.ID == idMayorEquipo2)
                    {
                        NroMano2 = JugE2.Mano;
                    }
                }

                if (NroMano1 > NroMano2)
                {
                    Equipo1.Puntos += 4;
                }
                else
                {
                    Equipo2.Puntos += 4;
                }
            }
        }

        private void MetodoRealEnvido()
        {
            int mayorEquipo1 = Equipo1.ListaJugadores.Max(x1 => x1.PuntosEnvido);
            int idMayorEquipo1 = Equipo1.ListaJugadores.Find(Jug => Jug.PuntosEnvido == mayorEquipo1).ID;//Saco el mayor del equipo 1

            int mayorEquipo2 = Equipo2.ListaJugadores.Max(x2 => x2.PuntosEnvido);
            int idMayorEquipo2 = Equipo2.ListaJugadores.Find(Jug => Jug.PuntosEnvido == mayorEquipo2).ID;//Saco el mayor del equipo 2

            if (mayorEquipo1 > mayorEquipo2)
            {
                Equipo1.Puntos += 3;
            }
            else if (mayorEquipo2 > mayorEquipo1)
            {
                Equipo2.Puntos += 3;
            }
            else
            {
                int NroMano1 = 0;
                int NroMano2 = 0;
                foreach (var JugE1 in Equipo1.ListaJugadores)
                {
                    if (JugE1.ID == idMayorEquipo1)
                    {
                        NroMano1 = JugE1.Mano;
                    }
                }
                foreach (var JugE2 in Equipo2.ListaJugadores)
                {
                    if (JugE2.ID == idMayorEquipo2)
                    {
                        NroMano2 = JugE2.Mano;
                    }
                }

                if (NroMano1 > NroMano2)
                {
                    Equipo1.Puntos += 3;
                }
                else
                {
                    Equipo2.Puntos += 3;
                }
            }
        }

        private void MetodoFaltaEnvido()
        {
            int mayorEquipo1 = Equipo1.ListaJugadores.Max(x1 => x1.PuntosEnvido);
            int idMayorEquipo1 = Equipo1.ListaJugadores.Find(Jug => Jug.PuntosEnvido == mayorEquipo1).ID;//Saco el mayor del equipo 1

            int mayorEquipo2 = Equipo2.ListaJugadores.Max(x2 => x2.PuntosEnvido);
            int idMayorEquipo2 = Equipo2.ListaJugadores.Find(Jug => Jug.PuntosEnvido == mayorEquipo2).ID;//Saco el mayor del equipo 2

            if (mayorEquipo1 > mayorEquipo2)
            {
                Equipo1.Puntos += 30 - Equipo2.Puntos;
            }
            else if (mayorEquipo2 > mayorEquipo1)
            {
                Equipo2.Puntos += 30 - Equipo1.Puntos;
            }
            else
            {
                int NroMano1 = 0;
                int NroMano2 = 0;
                foreach (var JugE1 in Equipo1.ListaJugadores)
                {
                    if (JugE1.ID == idMayorEquipo1)
                    {
                        NroMano1 = JugE1.Mano;
                    }
                }
                foreach (var JugE2 in Equipo2.ListaJugadores)
                {
                    if (JugE2.ID == idMayorEquipo2)
                    {
                        NroMano2 = JugE2.Mano;
                    }
                }

                if (NroMano1 > NroMano2)
                {
                    Equipo1.Puntos += 30 - Equipo2.Puntos;
                }
                else
                {
                    Equipo2.Puntos += 30 - Equipo1.Puntos;
                }
            }
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
			JugarMano(CantidadJugadores);
            MetodoEnvido();
            MetodoDobleEnvido();
            MetodoRealEnvido();
            MetodoFaltaEnvido();
		}
	}
}
