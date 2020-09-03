using Continental.API.Core.Helpers;

namespace Continental.API.Core.Entities
{
    public class CredencialesFinansys
    {
        public string UsuarioOracle { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public CredencialesFinansys(string token)
        {
            Token         = token;
            UsuarioOracle = EncryptionHelper.Desencriptar(TokenHelper.ExtraerClaimDelToken(token, "sub"));
            Password      = EncryptionHelper.Desencriptar(TokenHelper.ExtraerClaimDelToken(token, "fsys-sen"));
        }
    }
}
