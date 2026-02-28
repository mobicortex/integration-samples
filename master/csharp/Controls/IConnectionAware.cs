namespace SmartSdk.Controls
{
    /// <summary>
    /// Interface para controles que precisam saber quando a conexão é estabelecida
    /// </summary>
    public interface IConnectionAware
    {
        void OnConnected();
    }
}
