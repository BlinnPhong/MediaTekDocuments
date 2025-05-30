﻿using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Serilog;
using Serilog.Formatting.Json;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "https://api.mediatekdocuments.com/rest_mediatekdocuments/";
        /// <summary>
        /// nom de connexion à l'api
        /// </summary>
        private static readonly string connectionName = "MediaTekDocuments.Properties.Settings.mediatekConnectionString";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update     
        /// </summary>
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour delete     
        /// </summary>
        private const string DELETE = "DELETE";

        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {


            String authenticationString = null;
            try
            {
                Log.Logger = new LoggerConfiguration()
                            .MinimumLevel
                            .Verbose()
                            .WriteTo.Console()
                            .WriteTo.File(new JsonFormatter(), "logs/log.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();

                authenticationString = GetConnectionStringByName(connectionName);
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Log.Fatal("Access.Access catch authenticationString={0} erreur={1}", authenticationString, e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Récupère une connection par nom
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Retourne la connection si elle existe, sinon null</returns>
        private static string GetConnectionStringByName(string name)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            return settings != null ? settings.ConnectionString : null;
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        public Utilisateur GetUtilisateur(string login, string pwd)
        {
            var utilisateurObj = new { login, pwd };
            string jsonUtilisateur = JsonConvert.SerializeObject(utilisateurObj);

            List<Utilisateur> utlisateur = TraitementRecup<Utilisateur>(GET, "utilisateur/" + jsonUtilisateur, null);
            return utlisateur.Any() ? utlisateur[0] : null;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les suivis à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi", null);
            return lesSuivis;
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }

        /// <summary>
        /// Retourne les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// Retourne les commandes d'un document
        /// </summary>
        /// <param name="idDocument">id du document concernée</param>
        /// <returns>Liste d'objets Documents</returns>
        public List<Commande> GetCommandes(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Commande> lesCommandes = TraitementRecup<Commande>(GET, "commande/" + jsonIdDocument, null);
            return lesCommandes;
        }

        /// <summary>
        /// Retourne les commandesdocuments d'un document
        /// </summary>
        /// <param name="idDocument">id du document concernée</param>
        /// <returns>Liste d'objets Documents</returns>
        public List<CommandeDocument> GetCommandesDocument(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<CommandeDocument> lesCommandesDocuments = TraitementRecup<CommandeDocument>(GET, "commandedocument/" + jsonIdDocument, null);
            return lesCommandesDocuments;
        }

        /// <summary>
        /// Retourne les commandesdocuments d'un document
        /// </summary>
        /// <param name="idDocument">id du document concernée</param>
        /// <returns>Liste d'objets Documents</returns>
        public List<Abonnement> GetAbonnementsRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "abonnementrevue/" + jsonIdDocument, null);
            return lesAbonnements;
        }

        /// <summary>
        /// Retourne tous les abonnements des revues qui arrive à échéance dans moins de 30 jours
        /// </summary>
        /// <param name="idDocument">id du document concernée</param>
        /// <returns>Liste d'objets Documents</returns>
        public List<Abonnement> GetAllAbonnementsFin()
        {
            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "abonnementfin", null);
            return lesAbonnements;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerExemplaire catch jsonExemplaire={0} erreur={1}", jsonExemplaire, ex.Message);
            }
            
            return false;
        }

        /// <summary>
        /// ecriture d'une commande d'un livre/dvd en base de données
        /// </summary>
        /// <param name="commandeDocument">commandeDocument à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            String jsonCommandeDocument = JsonConvert.SerializeObject(commandeDocument);
            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(POST, "commandedocument", "champs=" + jsonCommandeDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerCommandeDocument catch jsonCommandeDocument={0} erreur={1}", jsonCommandeDocument, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// ecriture d'une commande d'un livre/dvd en base de données
        /// </summary>
        /// <param name="abonnement">commandeDocument à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerAbonnementRevue(Abonnement abonnement)
        {
            String jsonAbonnement = JsonConvert.SerializeObject(abonnement);
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(POST, "abonnement", "champs=" + jsonAbonnement);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerAbonnementRevue catch jsonAbonnement={0} erreur={1}", jsonAbonnement, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// réecriture d'une commande d'un livre/dvd en base de données
        /// </summary>
        /// <param name="commandeDocument">commandeDocument à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool ModifierCommandeDocument(CommandeDocument commandeDocument)
        {
            String jsonCommandeDocument = JsonConvert.SerializeObject(commandeDocument);
            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(PUT, "commandedocument/" + commandeDocument.Id,
                                                                                "champs=" + jsonCommandeDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.ModifierCommandeDocument catch jsonCommandeDocument={0} erreur={1}", jsonCommandeDocument, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// suppression d'une commande d'un livre/dvd en base de données
        /// </summary>
        /// <param name="commandeDocument">commandeDocument à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool SupprimerCommandeDocument(CommandeDocument commandeDocument)
        {
            String jsonCommandeDocument = JsonConvert.SerializeObject(new { commandeDocument.Id });
            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(DELETE, "commandedocument/" + jsonCommandeDocument, null);

                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.SupprimerCommandeDocument catch jsonCommandeDocument={0} erreur={1}", jsonCommandeDocument, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// suppression d'une commande d'une revue en base de données
        /// </summary>
        /// <param name="abonnement">commandeDocument à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool SupprimerAbonnementRevue(Abonnement abonnement)
        {
            String jsonAbonnement = JsonConvert.SerializeObject(new { abonnement.Id });
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(DELETE, "abonnement/" + jsonAbonnement, null);

                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.SupprimerAbonnementRevue catch jsonAbonnement={0} erreur={1}", jsonAbonnement, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// ecriture d'un livre en base de données
        /// </summary>
        /// <param name="livre">livre à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre);
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(POST, "livre", "champs=" + jsonLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerLivre catch jsonLivre={0} erreur={1}", jsonLivre, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// ecriture d'un dvd en base de données
        /// </summary>
        /// <param name="dvd">dvd à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(POST, "dvd", "champs=" + jsonDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerDvd catch jsonDvd={0} erreur={1}", jsonDvd, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// ecriture d'un revue en base de données
        /// </summary>
        /// <param name="revue">dvd à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(POST, "revue", "champs=" + jsonRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.CreerRevue catch jsonRevue={0} erreur={1}", jsonRevue, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// modification d'un livre en base de données
        /// </summary>
        /// <param name="livre">livre à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre);
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(PUT, "livre/" + livre.Id, "champs=" + jsonLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.ModifierLivre catch jsonLivre={0} erreur={1}", jsonLivre, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// modification d'un dvd en base de données
        /// </summary>
        /// <param name="dvd">dvd à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(PUT, "dvd/" + dvd.Id, "champs=" + jsonDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.ModifierDvd catch jsonDvd={0} erreur={1}", jsonDvd, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// modification d'une revue en base de données
        /// </summary>
        /// <param name="revue">revue à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(PUT, "revue/" + revue.Id, "champs=" + jsonRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.ModifierRevue catch jsonRevue={0} erreur={1}", jsonRevue, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// suppression d'un livre en base de données
        /// </summary>
        /// <param name="livre">livre à supprimer</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(new { livre.Id });
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(DELETE, "livre/" + jsonLivre, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.SupprimerLivre catch jsonLivre={0} erreur={1}", jsonLivre, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// suppression d'un dvd en base de données
        /// </summary>
        /// <param name="dvd">dvd à supprimer</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(new { dvd.Id });
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(DELETE, "dvd/" + jsonDvd, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.SupprimerDvd catch jsonDvd={0} erreur={1}", jsonDvd, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// suppression d'une revue en base de données
        /// </summary>
        /// <param name="dvd">revue à supprimer</param>
        /// <returns>true si la suppression a pu se faire (retour != null)</returns>
        public bool SupprimerRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(new { revue.Id });
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(DELETE, "revue/" + jsonRevue, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Error("Access.SupprimerRevue catch jsonRevue={0} erreur={1}", jsonRevue, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                    Log.Error("Access.TraitementRecup try else code={0}", code);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
                Log.Error("Access.TraitementRecup catch liste={0} erreur={1}", liste, e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private static String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

    }
}
