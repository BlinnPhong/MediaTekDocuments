using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande (commande d'un document)
    /// </summary>
    public class Commande
    {
        public string Id { get; }
        public DateTime DateCommande { get; set; }
        public double Montant { get; set; }

        public Commande(string id, DateTime dateCommande, double montant)
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;
        }
    }
}
