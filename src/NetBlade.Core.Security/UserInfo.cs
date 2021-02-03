namespace NetBlade.Core.Security
{
    public class UserInfo
    {
        public UserInfo
            (
            string env = null,
            int id = 0,
            string identifier = null,
            bool master = false,
            string representedCpfCnpj = null,
            string representedName = null,
            string sessionId = null,
            string userCpf = null,
            string userEmail = null,
            string userLogin = null,
            string userName = null,
            string userPhone = null,
            string[] userStamps = null,
            TipoUsuarioEnum userType = TipoUsuarioEnum.Desconhecido
            )
        {
            this.Env = env;
            this.Id = id;
            this.Identifier = identifier;
            this.Master = master;
            this.RepresentedCpfCnpj = representedCpfCnpj;
            this.RepresentedName = representedName;
            this.SessionId = sessionId;
            this.UserCpf = userCpf;
            this.UserEmail = userEmail;
            this.UserLogin = userLogin;
            this.UserName = userName;
            this.UserPhone = userPhone;
            this.UserStamps = userStamps;
            this.UserType = userType;
        }

        public string Env { get; }

        public int Id { get; }

        public string Identifier { get; }

        public bool Master { get; }

        public string RepresentedCpfCnpj { get; }

        public string RepresentedName { get; }

        public string SessionId { get; }

        public string UserCpf { get; }

        public string UserEmail { get; }

        public string UserLogin { get; }

        public string UserName { get; }

        public string UserPhone { get; }

        public string[] UserStamps { get; }

        public TipoUsuarioEnum UserType { get; }
    }
}
