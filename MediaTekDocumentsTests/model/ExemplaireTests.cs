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
    /// Classe de test sur la class Exemplaire
    /// </summary>
    [TestClass()]
    public class ExemplaireTests
    {
        private const int numero = 20;
        private static readonly DateTime dateAchat = DateTime.Today;
        private const string photo = "un chemin vers une photo";
        private const string idEtat = "27";
        private const string idDocument = "26";
        private static readonly Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
        [TestMethod()]
        public void ExemplaireTest()
        {
            Assert.AreEqual(numero, exemplaire.Numero, "devrait réussir : numero valorisé");
            Assert.AreEqual(dateAchat, exemplaire.DateAchat, "devrait réussir : dateAchat valorisé");
            Assert.AreEqual(photo, exemplaire.Photo, "devrait réussir : photo valorisé");
            Assert.AreEqual(idEtat, exemplaire.IdEtat, "devrait réussir : idEtat valorisé");
            Assert.AreEqual(idDocument, exemplaire.Id, "devrait réussir : idDocument valorisé");
        }
    }
}