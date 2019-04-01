using System;
namespace Transformer.Wars.Exceptions
{
    public class TransformerWarsException : Exception
    {
        public TransformerWarsException()
        {
        }

        public TransformerWarsException(string message)
            : base(message)
        {
        }

        public TransformerWarsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
