using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilsateur
    /// </summary>
    public class Utilisateur
    {
        public string Login { get; set; }
        public string IdService { get; set; }
        public string Service { get; set; }

        public Utilisateur(string login, string idService, string service)
        {
            this.Login = login;
            this.IdService = idService;
            this.Service = service;
        }
    }
}
