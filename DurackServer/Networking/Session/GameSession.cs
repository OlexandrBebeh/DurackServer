using System;
using System.Collections.Generic;

namespace DurackServer.networking.Session
{
    public class GameSession
    {
        public string Name { get; set; }
        public Guid Guid { get; }
        private List<NetworkPlayer> Players { get; } = new();
        public GameSession(string name)
        {
            Guid = Guid.NewGuid();
            Name = name;
        }

        public void Close()
        {
            foreach (var player in Players)
            {
                player.Close();
            }
        }

        public void AddPlayer(NetworkPlayer networkPlayer)
        {
            networkPlayer.GameSession = this;
            Players.Add(networkPlayer);
        }
    }
}