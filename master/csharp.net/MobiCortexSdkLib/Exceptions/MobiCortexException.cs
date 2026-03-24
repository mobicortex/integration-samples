namespace MobiCortex.Sdk.Exceptions
{
    /// <summary>
    /// Exceção lançada quando ocorre um erro na comunicação com o controlador MobiCortex.
    /// </summary>
    public class MobiCortexException : Exception
    {
        /// <summary>
        /// Código de erro retornado pela API (se houver).
        /// </summary>
        public int? ErrorCode { get; }

        /// <summary>
        /// Resposta raw da API (se houver).
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
