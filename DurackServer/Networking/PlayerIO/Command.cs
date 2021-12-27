using System.Collections.Generic;
using DurackServer.Model.DataType;

namespace DurackServer.networking.PlayerIO
{
    public class Command
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public int PlayerId { get; set; }
        public List<CardType> Cards { get; set; }
    }
}