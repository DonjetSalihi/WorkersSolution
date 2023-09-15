using System;

namespace WorkersSolution.Exceptions
{
    public class NoInternetException : Exception
    {
        public string NoInternetMessage { get; set; }
    }
}
