using System;



namespace Mino
{
	[Serializable]
    public class MissingCommandException : Exception
    {
        public MissingCommandException () : base("Command has no assignment") { }
        public MissingCommandException (string message) : base(message) { }
        public MissingCommandException (string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected MissingCommandException (System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
