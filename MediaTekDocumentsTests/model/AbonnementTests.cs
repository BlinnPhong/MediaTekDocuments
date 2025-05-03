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
    /// Classe de test sur la class Abonnement
    /// </summary>
    [TestClass()]
    public class AbonnementTests
    {
        private const string id = "20";
        private const string titreRevue = "Une revue";
        private const double montant = 13;
        private const string idRevue = "30";
        private static readonly DateTime dateCommande = DateTime.Today;
        private static readonly DateTime dateFinAbo = DateTime.Today;
        private static readonly Abonnement abo = new Abonnement(id, dateCommande, montant, dateFinAbo, idRevue, titreRevue);

        [TestMethod()]
        public void AbonnementTest()
        {
            Assert.AreEqual(id, abo.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titreRevue, abo.TitreRevue, "devrait réussir : titreRevue valorisé");
            Assert.AreEqual(montant, abo.Montant, "devrait réussir : montant valorisé");
            Assert.AreEqual(idRevue, abo.IdRevue, "devrait réussir : idRevue valorisé");
            Assert.AreEqual(dateCommande, abo.DateCommande, "devrait réussir : dateCommande valorisé");
            Assert.AreEqual(dateFinAbo, abo.DateFinAbonnement, "devrait réussir : dateFinAbo valorisé");
        }
    }
}