using System;
using System.Xml.Linq;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Document (réunit les infomations communes à tous les documents : Livre, Revue, Dvd)
    /// </summary>
    public class Document
    {
        public string Id { get; }
        public string Titre { get; set; }
        public string Image { get; set;  }
        public string IdGenre { get; set; }
        public string Genre { get; set; }
        public string IdPublic { get; set; }
        public string Public { get; set; }
        public string IdRayon { get; set; }
        public string Rayon { get; set; }

        public Document(string id, string titre, string image, string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
        {
            Id = id;
            Titre = titre;
            Image = image;
            IdGenre = idGenre;
            Genre = genre;
            IdPublic = idPublic;
            Public = lePublic;
            IdRayon = idRayon;
            Rayon = rayon;
        }
    }
}
