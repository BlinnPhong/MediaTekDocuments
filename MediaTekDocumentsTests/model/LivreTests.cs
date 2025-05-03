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
    /// Classe de test sur la class Livre
    /// </summary>
    [TestClass()]
    public class LivreTests
    {
        private const string id = "20";
        private const string titre = "un document";
        private const string image = "un chemin vers une image";
        private const string isbn = "isbn";
        private const string auteur = "jean";
        private const string collection = "une collection";
        private const string idGenre = "30";
        private const string genre = "roman policier";
        private const string idPublic = "40";
        private const string lePublic = "adulte";
        private const string idRayon = "50";
        private const string rayon = "fiction";
        private static readonly Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, lePublic, idRayon, rayon);
        [TestMethod()]
        public void LivreTest()
        {
            Assert.AreEqual(id, livre.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, livre.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, livre.Image, "devrait réussir : image valorisé");
            Assert.AreEqual(isbn, livre.Isbn, "devrait réussir : isbn valorisé");
            Assert.AreEqual(auteur, livre.Auteur, "devrait réussir : auteur valorisé");
            Assert.AreEqual(collection, livre.Collection, "devrait réussir : collection valorisé");
            Assert.AreEqual(idGenre, livre.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, livre.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, livre.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(lePublic, livre.Public, "devrait réussir : lePublic valorisé");
            Assert.AreEqual(idRayon, livre.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, livre.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}