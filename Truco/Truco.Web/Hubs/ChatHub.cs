using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Truco.Web.Hubs
{

    public class ChatHub : Hub
    {
        public void EnviarMensaje(string nombre, string texto)
        {
            Clients.All.mostrarMensaje(nombre, texto);
        }

        public void MensajePrivado(string nombre, string texto)
        {
            Clients.Caller.mostrarMensaje(nombre, texto);
        }
    }
}