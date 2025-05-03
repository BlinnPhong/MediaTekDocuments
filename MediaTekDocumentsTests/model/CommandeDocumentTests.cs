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
    /// Classe de test sur la class CommandeDocument
    /// </summary>
    [TestClass()]
    public class CommandeDocumentTests
    {
        private const string id = "20";
        private const double montant = 13;
        private const int nbExemplaire = 2;
        private const string idLivreDvd = "30";
        private const string idSuivi = "40";
        private const string suivi = "en cours";
        private static readonly DateTime dateCommande = DateTime.Today;
        private static readonly CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, suivi);
        [TestMethod()]
        public void CommandeDocumentTest()
        {
            Assert.AreEqual(id, commandeDocument.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, commandeDocument.DateCommande, "devrait réussir : titreRevue valorisé");
            Assert.AreEqual(montant, commandeDocument.Montant, "devrait réussir : montant valorisé");
            Assert.AreEqual(nbExemplaire, commandeDocument.NbExemplaire, "devrait réussir : idRevue valorisé");
            Assert.AreEqual(idLivreDvd, commandeDocument.IdLivreDvd, "devrait réussir : dateCommande valorisé");
            Assert.AreEqual(idSuivi, commandeDocument.IdSuivi, "devrait réussir : dateCommande valorisé");
            Assert.AreEqual(suivi, commandeDocument.Suivi, "devrait réussir : dateFinAbo valorisé");
        }
    }
}