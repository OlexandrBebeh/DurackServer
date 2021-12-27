using System;

namespace DurackServer.Exceptions
{
    public class GameException : Exception
    {
        public GameException()
        {
        }
        
        public GameException(string message)
            : base(message)
        {
        }

        public GameException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}