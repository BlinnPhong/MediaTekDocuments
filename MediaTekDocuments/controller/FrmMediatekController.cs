using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System.Xml.Linq;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }

        /// <summary>
        /// getter sur les suivis
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// récupère les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocument)
        {
            return access.GetExemplairesDocument(idDocument);
        }

        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            return access.GetExemplairesRevue(idDocument);
        }

        /// <summary>
        /// récupère les commandes d'un document
        /// </summary>
        /// <param name="idDocument">id du document concernée</param>
        /// <returns>Liste d'objets Commande</returns>
        public List<Commande> GetCommandes(string idDocument)
        {
            return access.GetCommandes(idDocument);
        }

        /// <summary>
        /// récupère les commandes d'un document
        /// </summary>
        /// <param name="idDocument">id du document concernée</param>
        /// <returns>Liste d'objets Commande</returns>
        public List<CommandeDocument> GetCommandesDocument(string idDocument)
        {
            return access.GetCommandesDocument(idDocument);
        }

        /// <summary>
        /// récupère les abonnements d'une revue
        /// </summary>
        /// <param name="idDocument">id du document concernée</param>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAbonnementsRevue(string idDocument)
        {
            return access.GetAbonnementsRevue(idDocument);
        }

        /// <summary>
        /// récupère les abonnements de revues qui arrive à échéance dans moins de 30 jours
        /// </summary>
        /// <returns>Liste d'objets Abonnement</returns>
        public List<Abonnement> GetAllAbonnementsFin()
        {
            return access.GetAllAbonnementsFin();
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Crée une commande d'un livre/dvd dans la bdd
        /// </summary>
        /// <param name="commandeDocument">L'objet CommandeDocument concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.CreerCommandeDocument(commandeDocument);
        }

        /// <summary>
        /// Crée un abonnement d'une revue dans la bdd
        /// </summary>
        /// <param name="abonnement">L'objet CommandeDocument concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerAbonnementRevue(Abonnement abonnement)
        {
            return access.CreerAbonnementRevue(abonnement);
        }

        /// <summary>
        /// Modifie une commande d'un livre/dvd dans la bdd
        /// </summary>
        /// <param name="commandeDocument">L'objet CommandeDocument concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool ModifierCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.ModifierCommandeDocument(commandeDocument);
        }

        /// <summary>
        /// Supprimer une commande d'un livre/dvd dans la bdd
        /// </summary>
        /// <param name="commandeDocument">L'objet CommandeDocument concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool SupprimerCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.SupprimerCommandeDocument(commandeDocument);
        }

        /// <summary>
        /// Supprimer une commande de revue dans la bdd
        /// </summary>
        /// <param name="abonnement">L'objet CommandeDocument concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool SupprimerAbonnementRevue(Abonnement abonnement)
        {
            return access.SupprimerAbonnementRevue(abonnement);
        }

        /// <summary>
        /// Crée un livre dans la bdd
        /// </summary>
        /// <param name="livre">L'objet Livre concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerLivre(Livre livre)
        {
            return access.CreerLivre(livre);
        }

        /// <summary>
        /// Crée un dvd dans la bdd
        /// </summary>
        /// <param name="dvd">L'objet dvd concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerDvd(Dvd dvd)
        {
            return access.CreerDvd(dvd);
        }

        /// <summary>
        /// Crée un revue dans la bdd
        /// </summary>
        /// <param name="revue">L'objet revue concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerRevue(Revue revue)
        {
            return access.CreerRevue(revue);
        }

        /// <summary>
        /// Modifier un livre dans la bdd
        /// </summary>
        /// <param name="livre">L'objet livre concerné</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierLivre(Livre livre)
        {
            return access.ModifierLivre(livre);
        }

        /// <summary>
        /// Modifier un dvd dans la bdd
        /// </summary>
        /// <param name="dvd">L'objet dvd concerné</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierDvd(Dvd dvd)
        {
            return access.ModifierDvd(dvd);
        }

        /// <summary>
        /// Modifier une revue dans la bdd
        /// </summary>
        /// <param name="revue">L'objet revue concerné</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierRevue(Revue revue)
        {
            return access.ModifierRevue(revue);
        }

        /// <summary>
        /// Supprimer un livre dans la bdd
        /// </summary>
        /// <param name="livre">L'objet livre concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerLivre(Livre livre)
        {
            return access.SupprimerLivre(livre);
        }

        /// <summary>
        /// Supprimer un dvd dans la bdd
        /// </summary>
        /// <param name="dvd">L'objet dvd concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerDvd(Dvd dvd)
        {
            return access.SupprimerDvd(dvd);
        }

        /// <summary>
        /// Supprimer une revue dans la bdd
        /// </summary>
        /// <param name="revue">L'objet revue concerné</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerRevue(Revue revue)
        {
            return access.SupprimerRevue(revue);
        }
    }
}
