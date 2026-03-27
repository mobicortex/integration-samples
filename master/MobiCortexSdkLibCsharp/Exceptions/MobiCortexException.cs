namespace MobiCortex.Sdk.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs in communication with the MobiCortex controller.
    /// </summary>
    public class MobiCortexException : Exception
    {
        /// <summary>
        /// Error code returned by the API (if any).
        /// </summary>
        public int? ErrorCode { get; }

        /// <summary>
        /// Raw response from the API (if any).
        /// </summary>
        public string? RawResponse { get; }

        public MobiCortexException(string message) : base(message) { }

        public MobiCortexException(string message, Exception innerException) 
            : base(message, innerException) { }

        public MobiCortexException(string message, int errorCode, string? rawResponse = null) 
            : base(message)
        {
            ErrorCode = errorCode;
            RawResponse = rawResponse;
        }
    }
}
