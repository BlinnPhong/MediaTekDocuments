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
    /// Classe de test sur la class Document
    /// </summary>
    [TestClass()]
    public class DocumentTests
    {
        private const string id = "20";
        private const string titre = "un document";
        private const string image = "un chemin vers une image";
        private const string idGenre = "30";
        private const string genre = "roman policier";
        private const string idPublic = "40";
        private const string lePublic = "adulte";
        private const string idRayon = "50";
        private const string rayon = "fiction";
        private static readonly Document document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);
        [TestMethod()]
        public void DocumentTest()
        {
            Assert.AreEqual(id, document.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, document.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, document.Image, "devrait réussir : image valorisé");
            Assert.AreEqual(idGenre, document.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, document.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, document.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(lePublic, document.Public, "devrait réussir : lePublic valorisé");
            Assert.AreEqual(idRayon, document.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, document.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}