using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    /// <summary>
    /// Classe de test sur la class Utilisateur
    /// </summary>
    [TestClass()]
    public class UtilisateurTests
    {
        private const string login = "20";
        private const string idService = "0";
        private const string service = "admin";
        private static readonly Utilisateur utilisateur = new Utilisateur(login, idService, service);
        [TestMethod()]
        public void UtilisateurTest()
        {
            Assert.AreEqual(login, utilisateur.Login, "devrait réussir : login valorisé");
            Assert.AreEqual(idService, utilisateur.IdService, "devrait réussir : idService valorisé");
            Assert.AreEqual(service, utilisateur.Service, "devrait réussir : service valorisé");
        }
    }
}