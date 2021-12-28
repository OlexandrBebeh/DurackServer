using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        private Controller.Controller _controller;

        public Server(Controller.Controller controller)
        {
            _controller = controller;
        }
        
        
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
                                player.SendMessageToClient(
                                new Command{
                                    Code=CommandCodes.ConnectedToSession,
                                    PlayerId = 1
                                });
                                Thread.Sleep(50);
                                session.Players[0].SendMessageToClient(new Command
                                {
                                    Code = CommandCodes.YouTurn,
                                });
                                return;
                            }
                            session = SessionManager.CreateSession(cmd.Name);
                            SessionManager.AddPlayerToSession(session, player);
                            Console.WriteLine($"Created Session: {session.Guid}");
                            player.SendMessageToClient(new Command{
                                Code=CommandCodes.SessionCreated,
                                PlayerId = 0
                            });
                            break;
                        
                        case CommandCodes.ThrowCards:
                        case CommandCodes.Pass:
                        case CommandCodes.BeatCards:
                        case CommandCodes.TakeCards:
                            session = SessionManager.GetFirstSession();
                            if (session is not null)
                            {
                                OnPutCard?.Invoke(session,cmd);
                                _controller.StartGameRound(session, cmd);
                                var gameState = _controller.GetGameState();
                                session.SendCommandToAllPlayers(new Command
                                {
                                    PlayerId = _controller.GetNextPlayerId(),
                                    BottomCard = gameState.DeckType.GetBotomCard(),
                                    EnemyPlayerCardsLeft = _controller.GetCurrentPlayer().hand.Count,
                                    DeckCardsLeft = _controller.GetGameState().DeckType.GetCardsAmount(),
                                    Code = cmd.Code,
                                    Cards = _controller.GetNextPlayer().hand,
                                    CardCouplets = gameState.FieldState
                                });
                                Thread.Sleep(50);
                                session.Players[_controller.GetCurrentPlayerId()]
                                    .SendMessageToClient(new Command
                                    {
                                        Code = CommandCodes.YouTurn,
                                        PlayerId = _controller.GetCurrentPlayerId()
                                    });
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