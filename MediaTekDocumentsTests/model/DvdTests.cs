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
    /// Classe de test sur la class Dvd
    /// </summary>
    [TestClass()]
    public class DvdTests
    {
        private const string id = "20";
        private const string titre = "un document";
        private const string image = "un chemin vers une image";
        private const int duree = 2;
        private const string realisateur = "jean";
        private const string synopsis = "quelque chose";
        private const string idGenre = "30";
        private const string genre = "roman policier";
        private const string idPublic = "40";
        private const string lePublic = "adulte";
        private const string idRayon = "50";
        private const string rayon = "fiction";
        private static readonly Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);
        [TestMethod()]
        public void DvdTest()
        {
            Assert.AreEqual(id, dvd.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, dvd.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, dvd.Image, "devrait réussir : image valorisé");
            Assert.AreEqual(duree, dvd.Duree, "devrait réussir : image valorisé");
            Assert.AreEqual(realisateur, dvd.Realisateur, "devrait réussir : image valorisé");
            Assert.AreEqual(synopsis, dvd.Synopsis, "devrait réussir : image valorisé");
            Assert.AreEqual(idGenre, dvd.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, dvd.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, dvd.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(lePublic, dvd.Public, "devrait réussir : lePublic valorisé");
            Assert.AreEqual(idRayon, dvd.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, dvd.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}