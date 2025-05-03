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
    /// Classe de test sur la class Public
    /// </summary>
    [TestClass()]
    public class PublicTests
    {
        private const string id = "20";
        private const string libelle = "roman policier";
        private static readonly Public lePublic = new Public(id, libelle);
        [TestMethod()]
        public void PublicTest()
        {
            Assert.AreEqual(id, lePublic.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, lePublic.Libelle, "devrait réussir : libelle valorisé");
        }
    }
}