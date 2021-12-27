using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DurackServer.Model.Game;
using DurackServer.networking.PlayerIO;
using DurackServer.networking.Session;

namespace DurackServer.networking
{
    public class Server
    {
        private TcpListener _listener = new(IPAddress.Parse("127.0.0.1"), 8001);
        private SessionManager SessionManager = new();
        public delegate void PutCard(GameSession currentPlayer, Command cmd);
        public event PutCard OnPutCard;
        
        public void Start()
        {
            try
            {
                Console.WriteLine("Server started");
                _listener.Start();
                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    new Thread(() => HandleClient(client)).Start();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleClient(TcpClient client)
        {
            var player = new NetworkPlayer(client);
            GameSession? session;
            while (true)
            {
                try
                {
                    var message = player.GetMessage();
                    Console.WriteLine($"###########\n\n{message}");
                    var cmd = CommandParser.FromString(message);
                    switch (cmd.Code)
                    {
                        case CommandCodes.ConnectToSession:
                            session = SessionManager.GetFirstSession();
                            if (session is not null)
                            {
                                SessionManager.AddPlayerToSession(session, player);
                                Console.WriteLine($"Connected to Session: {session.Guid}");
                                return;
                            }
                            session = SessionManager.CreateSession(cmd.Name);
                            SessionManager.AddPlayerToSession(session, player);
                            Console.WriteLine($"Created Session: {session.Guid}");
                            player.SendMessageToClient($"{session.Guid}");
                            break;
                        
                        case CommandCodes.PutCard:
                            session = SessionManager.GetFirstSession();
                            if (session is not null)
                            {
                                OnPutCard?.Invoke(session,cmd);
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }
            }
        }
    }
}