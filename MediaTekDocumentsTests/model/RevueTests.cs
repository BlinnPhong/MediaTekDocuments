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
    /// Classe de test sur la class Revue
    /// </summary>
    [TestClass()]
    public class RevueTests
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
        private const string periodicite = "QT";
        private const int delaiMiseADispo = 2;
        private static readonly Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);
        [TestMethod()]
        public void RevueTest()
        {
            Assert.AreEqual(id, revue.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, revue.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, revue.Image, "devrait réussir : image valorisé");
            Assert.AreEqual(idGenre, revue.IdGenre, "devrait réussir : idGenre valorisé");
            Assert.AreEqual(genre, revue.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, revue.IdPublic, "devrait réussir : idPublic valorisé");
            Assert.AreEqual(lePublic, revue.Public, "devrait réussir : lePublic valorisé");
            Assert.AreEqual(idRayon, revue.IdRayon, "devrait réussir : idRayon valorisé");
            Assert.AreEqual(rayon, revue.Rayon, "devrait réussir : rayon valorisé");
            Assert.AreEqual(periodicite, revue.Periodicite, "devrait réussir : periodicite valorisé");
            Assert.AreEqual(delaiMiseADispo, revue.DelaiMiseADispo, "devrait réussir : delaiMiseADispo valorisé");
        }
    }
}