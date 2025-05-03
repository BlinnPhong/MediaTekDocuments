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
    /// Classe de test sur la class Commande
    /// </summary>
    [TestClass()]
    public class CommandeTests
    {
        private const string id = "20";
        private static readonly DateTime dateCommande = DateTime.Today;
        private const double montant = 13;
        private static readonly Commande commande = new Commande(id, dateCommande, montant);
        [TestMethod()]
        public void CommandeTest()
        {
            Assert.AreEqual(id, commande.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, commande.DateCommande, "devrait réussir : titreRevue valorisé");
            Assert.AreEqual(montant, commande.Montant, "devrait réussir : montant valorisé");
        }
    }
}