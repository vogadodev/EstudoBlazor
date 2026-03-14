using System.Security.Cryptography;

namespace TarefasBlazor.Shared.INFRA.ServicesComum.GeradorDeIDsService
{
    public static class CriarIDService
    {
        /// <summary>
        /// Cria um GUID sequencial baseado em timestamp (similar ao Guid v7).
        /// Melhora ordenação e performance em índices clusterizados.
        /// </summary>
        public static Guid CriarNovoID()
        {           
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(timestampBytes);
            
            byte[] randomBytes = new byte[8];
            RandomNumberGenerator.Fill(randomBytes);
            
            byte[] guidBytes = new byte[16];
            Buffer.BlockCopy(timestampBytes, 0, guidBytes, 0, 8); 
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 8, 8);  

            return new Guid(guidBytes);
        }
    }
}
