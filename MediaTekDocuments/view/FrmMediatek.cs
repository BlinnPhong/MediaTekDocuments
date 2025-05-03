using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun

        private readonly FrmMediatekController controller;
        private readonly String service;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgSuivis = new BindingSource();

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek(string service)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            this.service = service;
        }

        /// <summary>
        /// L’événement Form_Load est utilisé pour initialiser les composants de l’interface utilisateur et charger les données nécessaires avant l’affichage du formulaire à l’utilisateur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMediatek_Load(object sender, EventArgs e)
        {
            if (service.Equals("administratif"))
            {
                List<Abonnement> abonnemntFin30Jours = controller.GetAllAbonnementsFin();

                if (abonnemntFin30Jours.Count == 0) return;

                Console.WriteLine(abonnemntFin30Jours.Count);
                FrmAlerteAbonnement alerteAbonnementForm = new FrmAlerteAbonnement(abonnemntFin30Jours);
                alerteAbonnementForm.ShowDialog();
            }
            else if(service.Equals("prêts")) {
                tabOngletsApplication.TabPages.Remove(tabCmdLivres);
                tabOngletsApplication.TabPages.Remove(tabCmdDvd);
                tabOngletsApplication.TabPages.Remove(tabCmdRevues);
                tabOngletsApplication.TabPages.Remove(tabReceptionRevue);

                grpLivresInfos.Enabled = false;
                grpDvdInfos.Enabled = false;
                grpRevuesInfos.Enabled = false;

                BtnDemandeModifLivre.Enabled = false;
                BtnDemandeModifDvd.Enabled = false;
                BtnDemandeModifRevue.Enabled = false;

                btnDemandeSupprLivre.Enabled = false;
                btnDemandeSupprDvd.Enabled = false;
                BtnDemandeSupprRevue.Enabled = false;
            }
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        private void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories"></param>
        /// <param name="cbx"></param>
        private void RemplirComboCategorieAjout(List<Categorie> lesCategories, ComboBox cbx)
        {
            cbx.DataSource = lesCategories;
            cbx.SelectedIndex = -1;
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        private void RemplirComboSuivi(List<Suivi> lesSuivis, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesSuivis;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Booléen pour savoir si une modification d'une livre est demandée
        /// </summary>
        private Boolean enCoursDeModifLivre = false;

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();

            RemplirComboCategorieAjout(controller.GetAllGenres(), cbxLivresGenresAjout);
            RemplirComboCategorieAjout(controller.GetAllPublics(), cbxLivresPublicsAjout);
            RemplirComboCategorieAjout(controller.GetAllRayons(), cbxLivresRayonsAjout);

            EnCoursModifLivre(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }

            cbxLivresGenresAjout.SelectedIndex = cbxLivresGenresAjout.FindStringExact(livre.Genre);
            cbxLivresPublicsAjout.SelectedIndex = cbxLivresPublicsAjout.FindStringExact(livre.Public);
            cbxLivresRayonsAjout.SelectedIndex = cbxLivresRayonsAjout.FindStringExact(livre.Rayon);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;

            cbxLivresGenresAjout.SelectedIndex = -1;
            cbxLivresRayonsAjout.SelectedIndex = -1;
            cbxLivresPublicsAjout.SelectedIndex = -1;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    //AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Modification d'affichage suivant si on est en cours de modif ou d'ajout d'un livre
        /// </summary>
        /// <param name="modif"></param>
        private void EnCoursModifLivre(Boolean modif)
        {
            enCoursDeModifLivre = modif;
            grpLivresRecherche.Enabled = !modif;
            if (modif)
            {
                grpLivresInfos.Text = "Modifier un livre";
                txbLivresNumero.ReadOnly = true;
            }
            else
            {
                grpLivresInfos.Text = "Ajouter un livre";
                txbLivresNumero.ReadOnly = false;
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton de modification, affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDemandeModifLivre_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.SelectedRows.Count > 0)
            {
                EnCoursModifLivre(true);

                Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                AfficheLivresInfos(livre);
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation de modification, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulLivre_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Voulez-vous vraiment annuler ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EnCoursModifLivre(false);
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'enregistrement, ajout/mise à jour du livre dans la bdd et affichage de la liste complète des livres
        /// </summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregLivre_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumero.Text.Equals("") && cbxLivresGenresAjout.SelectedIndex != -1 
                && cbxLivresPublicsAjout.SelectedIndex != -1
                && cbxLivresRayonsAjout.SelectedIndex != -1)
            {
                #region récupération des propriétés de l'interface 
                Genre genre = (Genre)cbxLivresGenresAjout.SelectedItem;
                Public lePublic = (Public)cbxLivresPublicsAjout.SelectedItem;
                Rayon rayon = (Rayon)cbxLivresRayonsAjout.SelectedItem;
                string id = txbLivresNumero.Text;
                string titre = txbLivresTitre.Text;
                string image = txbLivresImage.Text;
                string isbn = txbLivresIsbn.Text;
                string auteur = txbLivresAuteur.Text;
                string collection = txbLivresCollection.Text;
                string idGenre = genre.Id;
                string genreLibelle = genre.Libelle;
                string idPublic = lePublic.Id;
                string publicLibelle = lePublic.Libelle;
                string idRayon = rayon.Id;
                string rayonLibelle = rayon.Libelle;
                #endregion

                if (enCoursDeModifLivre)
                {
                    // Mise à jour livre
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    livre.Titre = titre;
                    livre.Image = image;
                    {
                        livre.Isbn = isbn;
                        livre.Auteur = auteur;
                        livre.Collection = collection;
                        livre.IdGenre = idGenre;
                        livre.Genre = genreLibelle;
                        livre.IdPublic = idPublic;
                        livre.Public = publicLibelle;
                        livre.IdRayon = idRayon;
                        livre.Rayon = rayonLibelle;
                    }

                    if (controller.ModifierLivre(livre))
                        MessageBox.Show("Le livre " + titre + " a été modifié.");
                }
                else
                {
                    // Ajout livre
                    Livre livre = new Livre(id, titre, image, isbn, auteur, collection,
                                            idGenre, genreLibelle,
                                            idPublic, publicLibelle,
                                            idRayon, rayonLibelle);

                    if (controller.CreerLivre(livre))
                        MessageBox.Show("Le livre " + titre + " a été ajouté.");
                }

                lesLivres = controller.GetAllLivres();
                RemplirLivresListeComplete();
                EnCoursModifLivre(false);
            }
            else
            {
                if (enCoursDeModifLivre) {
                    MessageBox.Show("Les champs suivant doivent être remplies : " +
                                    "genre / public / rayon", "Information");
                }
                else {
                    MessageBox.Show("Les champs suivant doivent être remplies : " +
                                    "id / genre / public / rayon", "Information");
                }
            }
        }

        /// <summary>
        /// Demande de suppression d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDemandeSupprLivre_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.SelectedRows.Count > 0)
            {
                Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];

                if (controller.GetExemplairesDocument(livre.Id).Count == 0
                    && controller.GetCommandes(livre.Id).Count == 0)
                {

                    if (MessageBox.Show("Voulez-vous vraiment supprimer " + livre.Titre + " ?",
                        "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (controller.SupprimerLivre(livre))
                        {
                            MessageBox.Show("Le livre " + livre.Titre + " a été supprimé.");
                        }
                    }


                    lesLivres = controller.GetAllLivres();
                    RemplirLivresListeComplete();
                }
                else
                {
                    MessageBox.Show("Vous ne pouvez pas supprimer un document " +
                                    "tant qu'il est rattaché à un exemplaire ou une commande", "Erreur");
                }

            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Booléen pour savoir si une modification d'une livre est demandée
        /// </summary>
        private Boolean enCoursDeModifDvd = false;

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();

            RemplirComboCategorieAjout(controller.GetAllGenres(), cbxDvdGenresAjout);
            RemplirComboCategorieAjout(controller.GetAllPublics(), cbxDvdPublicsAjout);
            RemplirComboCategorieAjout(controller.GetAllRayons(), cbxDvdRayonsAjout);

            EnCoursModifDvd(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }

            cbxDvdGenresAjout.SelectedIndex = cbxDvdGenresAjout.FindStringExact(dvd.Genre);
            cbxDvdPublicsAjout.SelectedIndex = cbxDvdPublicsAjout.FindStringExact(dvd.Public);
            cbxDvdRayonsAjout.SelectedIndex = cbxDvdRayonsAjout.FindStringExact(dvd.Rayon);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;

            cbxDvdGenresAjout.SelectedIndex = -1;
            cbxDvdPublicsAjout.SelectedIndex = -1;
            cbxDvdRayonsAjout.SelectedIndex = -1;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    //AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        /// <summary>
        /// Modification d'affichage suivant si on est en cours de modif ou d'ajout d'un dvd
        /// </summary>
        /// <param name="modif"></param>
        private void EnCoursModifDvd(Boolean modif)
        {
            enCoursDeModifDvd = modif;
            grpDvdRecherche.Enabled = !modif;
            if (modif)
            {
                grpDvdInfos.Text = "Modifier un Dvd";
                txbDvdNumero.ReadOnly = true;
            }
            else
            {
                grpDvdInfos.Text = "Ajouter un Dvd";
                txbDvdNumero.ReadOnly = false;
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Demande modification d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDemandeModifDvd_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.SelectedRows.Count > 0)
            {
                EnCoursModifDvd(true);

                Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                AfficheDvdInfos(dvd);
            }
        }

        /// <summary>
        /// ajout/modification d'un dvd en bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregDvd_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumero.Text.Equals("")
                && cbxDvdGenresAjout.SelectedIndex != -1
                && cbxDvdPublicsAjout.SelectedIndex != -1
                && cbxDvdRayonsAjout.SelectedIndex != -1)
            {
                Genre genre = (Genre)cbxDvdGenresAjout.SelectedItem;
                Public lePublic = (Public)cbxDvdPublicsAjout.SelectedItem;
                Rayon rayon = (Rayon)cbxDvdRayonsAjout.SelectedItem;
                string id = txbDvdNumero.Text;
                string titre = txbDvdTitre.Text;
                string image = txbDvdImage.Text;
                string synopsis = txbDvdSynopsis.Text;
                string realisateur = txbDvdRealisateur.Text;
                int duree = txbDvdDuree.Text.Equals("") ? 0 : int.Parse(txbDvdDuree.Text);
                string idGenre = genre.Id;
                string genreLibelle = genre.Libelle;
                string idPublic = lePublic.Id;
                string publicLibelle = lePublic.Libelle;
                string idRayon = rayon.Id;
                string rayonLibelle = rayon.Libelle;

                if (enCoursDeModifDvd)
                {
                    // Mise à jour dvd

                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    dvd.Titre = titre;
                    dvd.Image = image;
                    dvd.Synopsis = synopsis;
                    dvd.Realisateur = realisateur;
                    dvd.Duree = duree;
                    dvd.IdGenre = idGenre;
                    dvd.Genre = genreLibelle;
                    dvd.IdPublic = idPublic;
                    dvd.Public = publicLibelle;
                    dvd.IdRayon = idRayon;
                    dvd.Rayon = rayonLibelle;

                    if (controller.ModifierDvd(dvd))
                    {
                        MessageBox.Show("Le dvd " + titre + " a été modifié.");
                    }

                }
                else
                {
                    // Ajout dvd

                    Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis,
                                            idGenre, genreLibelle,
                                            idPublic, publicLibelle,
                                            idRayon, rayonLibelle);

                    if (controller.CreerDvd(dvd))
                    {
                        MessageBox.Show("Le dvd " + titre + " a été ajouté.");
                    }
                }

                lesDvd = controller.GetAllDvd();
                RemplirDvdListeComplete();
                EnCoursModifDvd(false);
            }
            else
            {
                if (enCoursDeModifDvd)
                {
                    MessageBox.Show("Les champs suivant doivent être remplies : " +
                                    "genre / public / rayon",
                                    "Information");
                }
                else
                {
                    MessageBox.Show("Les champs suivant doivent être remplies : " +
                                    "id / genre / public / rayon",
                                    "Information");
                }
            }
        }

        /// <summary>
        /// Demande annulation modification d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Voulez-vous vraiment annuler ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EnCoursModifDvd(false);
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Demande de suppression d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDemandeSupprDvd_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.SelectedRows.Count > 0)
            {
                Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];

                if (controller.GetExemplairesDocument(dvd.Id).Count == 0
                    && controller.GetCommandes(dvd.Id).Count == 0)
                {

                    if (MessageBox.Show("Voulez-vous vraiment supprimer " + dvd.Titre + " ?",
                        "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (controller.SupprimerDvd(dvd))
                        {
                            MessageBox.Show("Le dvd " + dvd.Titre + " a été supprimé.");
                        }
                    }


                    lesDvd = controller.GetAllDvd();
                    RemplirDvdListeComplete();
                }
                else
                {
                    MessageBox.Show("Vous ne pouvez pas supprimer un document " +
                                    "tant qu'il est rattaché à un exemplaire ou une commande", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Booléen pour savoir si une modification d'une livre est demandée
        /// </summary>
        private Boolean enCoursDeModifRevue = false;

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();

            RemplirComboCategorieAjout(controller.GetAllGenres(), cbxRevueGenresAjout);
            RemplirComboCategorieAjout(controller.GetAllPublics(), cbxRevuePublicsAjout);
            RemplirComboCategorieAjout(controller.GetAllRayons(), cbxRevueRayonsAjout);

            EnCoursModifRevue(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }

            cbxRevueGenresAjout.SelectedIndex = cbxRevueGenresAjout.FindStringExact(revue.Genre);
            cbxRevuePublicsAjout.SelectedIndex = cbxRevuePublicsAjout.FindStringExact(revue.Public);
            cbxRevueRayonsAjout.SelectedIndex = cbxRevueRayonsAjout.FindStringExact(revue.Rayon);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;

            cbxRevueGenresAjout.SelectedIndex = -1;
            cbxRevuePublicsAjout.SelectedIndex = -1;
            cbxRevueRayonsAjout.SelectedIndex = -1;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    //AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        /// <summary>
        /// Modification d'affichage suivant si on est en cours de modif ou d'ajout d'un dvd
        /// </summary>
        /// <param name="modif"></param>
        private void EnCoursModifRevue(Boolean modif)
        {
            enCoursDeModifRevue = modif;
            grpRevuesRecherche.Enabled = !modif;
            if (modif)
            {
                grpRevuesInfos.Text = "Modifier une revue";
                txbRevuesNumero.ReadOnly = true;
            }
            else
            {
                grpRevuesInfos.Text = "Ajouter une revue";
                txbRevuesNumero.ReadOnly = false;
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Demande modification d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDemandeModifRevue_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.SelectedRows.Count > 0)
            {
                EnCoursModifRevue(true);

                Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                AfficheRevuesInfos(revue);
            }
        }

        /// <summary>
        /// Demande annulation modification d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Voulez-vous vraiment annuler ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EnCoursModifRevue(false);
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// ajout/modification d'une revue en bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEnregRevue_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumero.Text.Equals("")
                && cbxRevueGenresAjout.SelectedIndex != -1
                && cbxRevuePublicsAjout.SelectedIndex != -1
                && cbxRevueRayonsAjout.SelectedIndex != -1)
            {
                Genre genre = (Genre)cbxRevueGenresAjout.SelectedItem;
                Public lePublic = (Public)cbxRevuePublicsAjout.SelectedItem;
                Rayon rayon = (Rayon)cbxRevueRayonsAjout.SelectedItem;
                string id = txbRevuesNumero.Text;
                string titre = txbRevuesTitre.Text;
                string image = txbRevuesImage.Text;
                string periodicite = txbRevuesPeriodicite.Text;
                int dateMiseADispo = txbRevuesDateMiseADispo.Text.Equals("") ? 0 : int.Parse(txbRevuesDateMiseADispo.Text);
                string idGenre = genre.Id;
                string genreLibelle = genre.Libelle;
                string idPublic = lePublic.Id;
                string publicLibelle = lePublic.Libelle;
                string idRayon = rayon.Id;
                string rayonLibelle = rayon.Libelle;

                if (enCoursDeModifRevue)
                {
                    // Mise à jour revue

                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    revue.Titre = titre;
                    revue.Image = image;
                    revue.Periodicite = periodicite;
                    revue.DelaiMiseADispo = dateMiseADispo;
                    revue.IdGenre = idGenre;
                    revue.Genre = genreLibelle;
                    revue.IdPublic = idPublic;
                    revue.Public = publicLibelle;
                    revue.IdRayon = idRayon;
                    revue.Rayon = rayonLibelle;

                    if (controller.ModifierRevue(revue))
                    {
                        MessageBox.Show("La revue " + titre + " a été modifié.");
                    }

                }
                else
                {
                    // Ajout revue

                    Revue revue = new Revue(id, titre, image,
                                            idGenre, genreLibelle,
                                            idPublic, publicLibelle,
                                            idRayon, rayonLibelle,
                                            periodicite, dateMiseADispo);

                    if (controller.CreerRevue(revue))
                    {
                        MessageBox.Show("La revue " + titre + " a été ajouté.");
                    }
                }

                lesRevues = controller.GetAllRevues();
                RemplirRevuesListeComplete();
                EnCoursModifRevue(false);
            }
            else
            {
                if (enCoursDeModifRevue)
                {
                    MessageBox.Show("Les champs suivant doivent être remplies : " +
                                    "genre / public / rayon",
                                    "Information");
                }
                else
                {
                    MessageBox.Show("Les champs suivant doivent être remplies : " +
                                    "id / genre / public / rayon",
                                    "Information");
                }
            }
        }


        /// <summary>
        /// Demande de suppression d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDemandeSupprRevue_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.SelectedRows.Count > 0)
            {
                Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];

                if (controller.GetExemplairesDocument(revue.Id).Count == 0
                    && controller.GetCommandes(revue.Id).Count == 0)
                {

                    if (MessageBox.Show("Voulez-vous vraiment supprimer " + revue.Titre + " ?",
                        "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (controller.SupprimerRevue(revue))
                        {
                            MessageBox.Show("La revue " + revue.Titre + " a été supprimé.");
                        }
                    }


                    lesRevues = controller.GetAllRevues();
                    RemplirRevuesListeComplete();
                }
                else
                {
                    MessageBox.Show("Vous ne pouvez pas supprimer un document " +
                                    "tant qu'il est rattaché à un exemplaire ou une commande", "Erreur");
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }
        #endregion

        #region Onglet Parutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region Onglet Commandes Livres
        private readonly BindingSource bdgCommandesLivresListe = new BindingSource();
        private List<CommandeDocument> lesCommandesLivres = new List<CommandeDocument>();
        private Livre livreCmdSelected = null;

        /// <summary>
        /// Ouverture de l'onglet Commandes Livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCmdLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboSuivi(controller.GetAllSuivis(), bdgSuivis, cbxSuiviCmdLivre);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirCommandesLivresListe(List<CommandeDocument> commandesLivres)
        {
            bdgCommandesLivresListe.DataSource = commandesLivres;
            dgvCmdLivresListe.DataSource = bdgCommandesLivresListe;
            dgvCmdLivresListe.Columns["id"].Visible = false;
            dgvCmdLivresListe.Columns["idLIvreDvd"].Visible = false;
            dgvCmdLivresListe.Columns["idSuivi"].Visible = false;
            dgvCmdLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCmdLivresListe.Columns["dateCommande"].DisplayIndex = 0;
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné onglet commandes livres
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheCommandeLivreInfos(Livre livre)
        {
            txbLivresAuteurCmd.Text = livre.Auteur;
            txbLivresCollectionCmd.Text = livre.Collection;
            txbLivresImageCmd.Text = livre.Image;
            txbLivresIsbnCmd.Text = livre.Isbn;
            txbLivresNumeroCmd.Text = livre.Id;
            txbLivresGenreCmd.Text = livre.Genre;
            txbLivresPublicCmd.Text = livre.Public;
            txbLivresRayonCmd.Text = livre.Rayon;
            txbLivresTitreCmd.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImageCmd.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImageCmd.Image = null;
            }
        }

        /// <summary>
        /// Affichage des informations de la commande sélectionné
        /// </summary>
        /// <param name="commandeDocument">le livre</param>
        private void AfficheSuiviCommandeLivreInfos(CommandeDocument commandeDocument)
        {
            txbSuiviCmdLivre.Text = commandeDocument.Suivi;
            cbxSuiviCmdLivre.SelectedIndex = -1;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre onglet commandes livres
        /// </summary>
        private void VideCommandeLivreInfos()
        {
            txbLivresAuteurCmd.Text = "";
            txbLivresCollectionCmd.Text = "";
            txbLivresImageCmd.Text = "";
            txbLivresIsbnCmd.Text = "";
            txbLivresNumeroCmd.Text = "";
            txbLivresGenreCmd.Text = "";
            txbLivresPublicCmd.Text = "";
            txbLivresRayonCmd.Text = "";
            txbLivresTitreCmd.Text = "";
            pcbLivresImageCmd.Image = null;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre onglet commandes livres
        /// </summary>
        private void VideSuiviCommandeLivreInfos()
        {
            txbSuiviCmdLivre.Text = "";
            cbxSuiviCmdLivre.SelectedIndex = -1;
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCmdLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "DateCommande":
                    sortedList = lesCommandesLivres.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandesLivres.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Suivi).ToList();
                    break;
            }
            RemplirCommandesLivresListe(sortedList);
        }

        /// <summary>
        /// Afficher les infomration de la commande du livre lorsque la selection du dgv change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdLivresListe.CurrentCell != null)
            {
                try
                {
                    CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesLivresListe.List[bdgCommandesLivresListe.Position];
                    AfficheSuiviCommandeLivreInfos(commandeDocument);
                }
                catch
                {
                    VideSuiviCommandeLivreInfos();
                }
            }
            else
            {
                VideSuiviCommandeLivreInfos();
            }
        }

        /// <summary>
        /// Afficher les commandes du livre recherché lors du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresCmdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRechercheCmd.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRechercheCmd.Text));
                if (livre != null)
                {
                    AfficheCommandeLivreInfos(livre);
                    livreCmdSelected = livre;
                    grpCmdLivreAjout.Enabled = true;
                    grpSuiviCmdLivre.Enabled = true;
                    lesCommandesLivres = controller.GetCommandesDocument(livre.Id);
                    RemplirCommandesLivresListe(lesCommandesLivres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    VideCommandeLivreInfos();
                    VideSuiviCommandeLivreInfos();
                    livreCmdSelected = null;
                    grpCmdLivreAjout.Enabled = false;
                    grpSuiviCmdLivre.Enabled = false;
                    txbLivresNumRechercheCmd.Text = "";
                }
            }
            else
            {
                VideCommandeLivreInfos();
                VideSuiviCommandeLivreInfos();
                livreCmdSelected = null;
                grpCmdLivreAjout.Enabled = false;
                grpSuiviCmdLivre.Enabled = false;
                RemplirCommandesLivresListe(new List<CommandeDocument>());
            }
        }

        /// <summary>
        /// Enregistrer une commande pour un livre lors du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregCmdLivre_Click(object sender, EventArgs e)
        {
            if (livreCmdSelected == null) return;

            string idCmd = txbLivresCmdNumero.Text;
            string montant = txbLivresCmdMontant.Text;
            string nbExemplaire = txbLivresCmdQte.Text;

            if (!idCmd.Equals("") && !montant.Equals("") && !nbExemplaire.Equals(""))
            {
                DateTime date = dtpLivresCmdDate.Value;
                string idLivreDvd = livreCmdSelected.Id;
                string idSuivi = "00001"; // Id suivi en cours
                string suivi = "en cours"; // Libelle suivi en cours
                CommandeDocument commandeLivre = new CommandeDocument(   idCmd, date, double.Parse(montant), 
                                                                    int.Parse(nbExemplaire),
                                                                    idLivreDvd, idSuivi, suivi);

                if (controller.CreerCommandeDocument(commandeLivre))
                    MessageBox.Show("La commande n° " + idCmd + " a été ajouté.");


                lesCommandesLivres = controller.GetCommandesDocument(idLivreDvd);
                RemplirCommandesLivresListe(lesCommandesLivres);
            }
        }

        /// <summary>
        /// Modifier le suivi d'une commande d'un livre lors du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModSuiviCmdLivre_Click(object sender, EventArgs e)
        {
            if (dgvCmdLivresListe.SelectedRows.Count > 0 && cbxSuiviCmdLivre.SelectedIndex != -1)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesLivresListe.List[bdgCommandesLivresListe.Position];
                if (commandeDocument != null)
                {
                    Suivi nouveauSuivi = (Suivi)cbxSuiviCmdLivre.SelectedItem;

                    bool commandeLivree = commandeDocument.IdSuivi.Equals("00003");
                    bool commandeReglee = commandeDocument.IdSuivi.Equals("00004");

                    if ((nouveauSuivi.Id.Equals("00001") || nouveauSuivi.Id.Equals("00002")) && (commandeLivree || commandeReglee))
                    {
                        MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir à une étape précédente (en cours ou relancée)");
                        return;
                    }

                    if(nouveauSuivi.Id.Equals("00004") && !commandeLivree)
                    {
                        MessageBox.Show("Une commande ne peut pas être réglée si elle n'est pas livrée");
                        return;
                    }

                    commandeDocument.IdSuivi = nouveauSuivi.Id;
                    commandeDocument.Suivi = nouveauSuivi.Libelle;

                    if (controller.ModifierCommandeDocument(commandeDocument))
                    {
                        MessageBox.Show("Le suivi de la commande a été modifié.");

                        lesCommandesLivres = controller.GetCommandesDocument(commandeDocument.IdLivreDvd);
                        RemplirCommandesLivresListe(lesCommandesLivres);
                    }
                }
            }
        }

        /// <summary>
        /// Supprimer la commande d'un livre lors du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprCmdLivre_Click(object sender, EventArgs e)
        {
            if (dgvCmdLivresListe.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesLivresListe.List[bdgCommandesLivresListe.Position];
                if (commandeDocument != null)
                {
                    if (commandeDocument.IdSuivi.Equals("00001") || commandeDocument.IdSuivi.Equals("00002"))
                    {
                        if (MessageBox.Show("Êtes vous certains de vouloir supprimer cette commande ?", "Confirmer la suppression",
                           MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            controller.SupprimerCommandeDocument(commandeDocument);
                        }

                        lesCommandesLivres = controller.GetCommandesDocument(commandeDocument.IdLivreDvd);
                        RemplirCommandesLivresListe(lesCommandesLivres);
                    }
                    else
                    {
                        MessageBox.Show("Vous ne pouvez pas supprimer une commande qui à été livrée", "Information");
                    }
                }
            }
        }
        #endregion

        #region Onglet Commandes Dvd
        private readonly BindingSource bdgCommandesDvdListe = new BindingSource();
        private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();
        private Dvd dvdCmdSelected = null;

        /// <summary>
        /// Ouverture de l'onglet Commandes Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCmdDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboSuivi(controller.GetAllSuivis(), bdgSuivis, cbxSuiviCmdDvd);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="commandesDvd">liste de livres</param>
        private void RemplirCommandesDvdListe(List<CommandeDocument> commandesDvd)
        {
            bdgCommandesDvdListe.DataSource = commandesDvd;
            dgvCmdDvdListe.DataSource = bdgCommandesDvdListe;
            dgvCmdDvdListe.Columns["id"].Visible = false;
            dgvCmdDvdListe.Columns["idLIvreDvd"].Visible = false;
            dgvCmdDvdListe.Columns["idSuivi"].Visible = false;
            dgvCmdDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCmdDvdListe.Columns["dateCommande"].DisplayIndex = 0;
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné onglet commandes livres
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheCommandeDvdInfos(Dvd dvd)
        {
            txbDvdRealisateurCmd.Text = dvd.Realisateur;
            txbDvdSynopsisCmd.Text = dvd.Synopsis;
            txbDvdImageCmd.Text = dvd.Image;
            txbDvdDureeCmd.Text = dvd.Duree.ToString();
            txbDvdNumeroCmd.Text = dvd.Id;
            txbDvdGenreCmd.Text = dvd.Genre;
            txbDvdPublicCmd.Text = dvd.Public;
            txbDvdRayonCmd.Text = dvd.Rayon;
            txbDvdTitreCmd.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImageCmd.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImageCmd.Image = null;
            }
        }

        /// <summary>
        /// Affichage des informations de la commande sélectionné
        /// </summary>
        /// <param name="commandeDocument">le livre</param>
        private void AfficheSuiviCommandeDvdInfos(CommandeDocument commandeDocument)
        {
            txbSuiviCmdDvd.Text = commandeDocument.Suivi;
            cbxSuiviCmdDvd.SelectedIndex = -1;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre onglet commandes livres
        /// </summary>
        private void VideCommandeDvdInfos()
        {
            txbDvdRealisateurCmd.Text = "";
            txbDvdSynopsisCmd.Text = "";
            txbDvdImageCmd.Text = "";
            txbDvdDureeCmd.Text = "";
            txbDvdNumeroCmd.Text = "";
            txbDvdGenreCmd.Text = "";
            txbDvdPublicCmd.Text = "";
            txbDvdRayonCmd.Text = "";
            txbDvdTitreCmd.Text = "";
            pcbDvdImageCmd.Image = null;
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de suivi de la commande dvd
        /// </summary>
        private void VideSuiviCommandeDvdInfos()
        {
            txbSuiviCmdDvd.Text = "";
            cbxSuiviCmdDvd.SelectedIndex = -1;
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCmdDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "DateCommande":
                    sortedList = lesCommandesDvd.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandesDvd.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Suivi).ToList();
                    break;
            }
            RemplirCommandesDvdListe(sortedList);
        }

        /// <summary>
        /// Afficher les infos du dvd lorsque la selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdDvdListe.CurrentCell != null)
            {
                try
                {
                    CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesDvdListe.List[bdgCommandesDvdListe.Position];
                    AfficheSuiviCommandeDvdInfos(commandeDocument);
                }
                catch
                {
                    VideSuiviCommandeDvdInfos();
                }
            }
            else
            {
                VideSuiviCommandeDvdInfos();
            }
        }

        /// <summary>
        /// Recher un dvd après le clic sur le bouton rechercher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdCmdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRechercheCmd.Text.Equals(""))
            {
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRechercheCmd.Text));
                if (dvd != null)
                {
                    AfficheCommandeDvdInfos(dvd);
                    dvdCmdSelected = dvd;
                    grpCmdDvdAjout.Enabled = true;
                    grpSuiviCmdDvd.Enabled = true;
                    lesCommandesDvd = controller.GetCommandesDocument(dvd.Id);
                    RemplirCommandesDvdListe(lesCommandesDvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    VideCommandeDvdInfos();
                    VideSuiviCommandeDvdInfos();
                    dvdCmdSelected = null;
                    grpCmdDvdAjout.Enabled = false;
                    grpSuiviCmdDvd.Enabled = false;
                    txbDvdNumRechercheCmd.Text = "";
                }
            }
            else
            {
                VideCommandeDvdInfos();
                VideSuiviCommandeDvdInfos();
                dvdCmdSelected = null;
                grpCmdDvdAjout.Enabled = false;
                grpSuiviCmdDvd.Enabled = false;
                RemplirCommandesDvdListe(new List<CommandeDocument>());
            }
        }

        /// <summary>
        /// Enregistrer une commande d'un dvd lors du clic du bouton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregCmdDvd_Click(object sender, EventArgs e)
        {
            Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRechercheCmd.Text));
            if (dvd == null) return;

            string idCmd = txbDvdCmdNumero.Text;
            string montant = txbDvdCmdMontant.Text;
            string nbExemplaire = txbDvdCmdQte.Text;

            if (!idCmd.Equals("") && !montant.Equals("") && !nbExemplaire.Equals(""))
            {
                DateTime date = dtpDvdCmdDate.Value;
                string idLivreDvd = dvdCmdSelected.Id;
                string idSuivi = "00001"; // Id suivi en cours
                string suivi = "en cours"; // Libelle suivi en cours
                CommandeDocument commandeDvd = new CommandeDocument(idCmd, date, double.Parse(montant),
                                                                    int.Parse(nbExemplaire),
                                                                    idLivreDvd, idSuivi, suivi);

                if (controller.CreerCommandeDocument(commandeDvd))
                    MessageBox.Show("La commande n° " + idCmd + " a été ajouté.");


                lesCommandesDvd = controller.GetCommandesDocument(idLivreDvd);
                RemplirCommandesDvdListe(lesCommandesDvd);
            }
        }

        /// <summary>
        /// Modifier le suivi d'une commande d'un dvd lors du clic sur le bouton modifier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModSuiviCmdDvd_Click(object sender, EventArgs e)
        {
            if (dgvCmdDvdListe.SelectedRows.Count > 0 && cbxSuiviCmdDvd.SelectedIndex != -1)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesDvdListe.List[bdgCommandesDvdListe.Position];
                if (commandeDocument != null)
                {
                    Suivi nouveauSuivi = (Suivi)cbxSuiviCmdDvd.SelectedItem;

                    bool commandeLivree = commandeDocument.IdSuivi.Equals("00003");
                    bool commandeReglee = commandeDocument.IdSuivi.Equals("00004");

                    if ((nouveauSuivi.Id.Equals("00001") || nouveauSuivi.Id.Equals("00002")) && (commandeLivree || commandeReglee))
                    {
                        MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir à une étape précédente (en cours ou relancée)");
                        return;
                    }

                    if (nouveauSuivi.Id.Equals("00004") && !commandeLivree)
                    {
                        MessageBox.Show("Une commande ne peut pas être réglée si elle n'est pas livrée");
                        return;
                    }

                    commandeDocument.IdSuivi = nouveauSuivi.Id;
                    commandeDocument.Suivi = nouveauSuivi.Libelle;

                    if (controller.ModifierCommandeDocument(commandeDocument))
                    {
                        MessageBox.Show("Le suivi de la commande a été modifié.");

                        lesCommandesDvd = controller.GetCommandesDocument(commandeDocument.IdLivreDvd);
                        RemplirCommandesDvdListe(lesCommandesDvd);
                    }
                }
            }
        }

        /// <summary>
        /// Supprimer une commande de dvd lors du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprCmdDvd_Click(object sender, EventArgs e)
        {
            if (dgvCmdDvdListe.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesDvdListe.List[bdgCommandesDvdListe.Position];
                if (commandeDocument != null)
                {
                    if (commandeDocument.IdSuivi.Equals("00001") || commandeDocument.IdSuivi.Equals("00002"))
                    {
                        if (MessageBox.Show("Êtes vous certains de vouloir supprimer cette commande ?", "Confirmer la suppression",
                           MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            controller.SupprimerCommandeDocument(commandeDocument);
                        }

                        lesCommandesDvd = controller.GetCommandesDocument(commandeDocument.IdLivreDvd);
                        RemplirCommandesDvdListe(lesCommandesDvd);
                    }
                    else
                    {
                        MessageBox.Show("Vous ne pouvez pas supprimer une commande qui à été livrée", "Information");
                    }
                }
            }
        }
        #endregion

        #region Onglet Commandes Revues
        private readonly BindingSource bdgCommandesRevueListe = new BindingSource();
        private List<Abonnement> lesAbonnementsRevue = new List<Abonnement>();
        private Revue revueCmdSelected = null;

        /// <summary>
        /// Ouverture de l'onglet Commandes Revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCmdRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            //RemplirComboSuivi(controller.GetAllSuivis(), bdgSuivis, cbxSuiviCmdLivre);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirCommandesRevuesListe(List<Abonnement> commandesRevue)
        {
            bdgCommandesRevueListe.DataSource = commandesRevue;
            dgvCmdRevueListe.DataSource = bdgCommandesRevueListe;
            dgvCmdRevueListe.Columns["id"].Visible = false;
            dgvCmdRevueListe.Columns["idRevue"].Visible = false;
            dgvCmdRevueListe.Columns["titreRevue"].Visible = false;
            dgvCmdRevueListe.Columns["dateCommande"].DisplayIndex = 0;
            dgvCmdRevueListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné onglet commandes livres
        /// </summary>
        /// <param name="revue">le livre</param>
        private void AfficheCommandeRevueInfos(Revue revue)
        {
            txbRevuePeriodiciteCmd.Text = revue.Periodicite;
            txbRevueDelaiCmd.Text = revue.DelaiMiseADispo.ToString();
            txbRevueNumeroCmd.Text = revue.Id;
            txbRevueGenreCmd.Text = revue.Genre;
            txbRevuePublicCmd.Text = revue.Public;
            txbRevueRayonCmd.Text = revue.Rayon;
            txbRevueTitreCmd.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevueImageCmd.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevueImageCmd.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la revue onglet commandes revues
        /// </summary>
        private void VideCommandeRevueInfos()
        {
            txbRevuePeriodiciteCmd.Text = "";
            txbRevueDelaiCmd.Text = "";
            txbRevueNumeroCmd.Text = "";
            txbRevueGenreCmd.Text = "";
            txbRevuePublicCmd.Text = "";
            txbLivresRayonCmd.Text = "";
            txbRevueTitreCmd.Text = "";
            pcbRevueImageCmd.Image = null;
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdRevueListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCmdRevueListe.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "DateCommande":
                    sortedList = lesAbonnementsRevue.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "DateFinAbonnement":
                    sortedList = lesAbonnementsRevue.OrderBy(o => o.DateFinAbonnement).ToList();
                    break;
                case "Montant":
                    sortedList = lesAbonnementsRevue.OrderBy(o => o.Montant).ToList();
                    break;
            }
            RemplirCommandesRevuesListe(sortedList);
        }

        /// <summary>
        /// Rechercher les commandes associées à une revue lors du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesCmdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevueNumRechercheCmd.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevueNumRechercheCmd.Text));
                if (revue != null)
                {
                    AfficheCommandeRevueInfos(revue);
                    revueCmdSelected = revue;
                    grpCmdRevueAjout.Enabled = true;
                    lesAbonnementsRevue = controller.GetAbonnementsRevue(revue.Id);
                    Console.WriteLine(lesAbonnementsRevue.Count);
                    RemplirCommandesRevuesListe(lesAbonnementsRevue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    VideCommandeRevueInfos();
                    revueCmdSelected = null;
                    grpCmdRevueAjout.Enabled = false;
                    txbRevueNumRechercheCmd.Text = "";
                }
            }
            else
            {
                VideCommandeRevueInfos();
                revueCmdSelected = null;
                grpCmdRevueAjout.Enabled = false;
                RemplirCommandesRevuesListe(new List<Abonnement>());
            }
        }

        /// <summary>
        /// Enregistrer une commande d'une revue lors du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregCmdRevue_Click(object sender, EventArgs e)
        {
            if (revueCmdSelected == null) return;

            string idCmd = txbRevueCmdNumero.Text;
            string montant = txbRevueCmdMontant.Text;

            if (!idCmd.Equals("") && !montant.Equals(""))
            {
                DateTime date = dtpRevueCmdDate.Value;
                DateTime dateFinAbo = dtpRevueCmdDateFinAbo.Value;
                string idRevue = revueCmdSelected.Id;
                string titreRevue = revueCmdSelected.Titre;
                Abonnement abonnement = new Abonnement(idCmd, date, double.Parse(montant),
                                                                    dateFinAbo,
                                                                    idRevue, titreRevue);

                if (controller.CreerAbonnementRevue(abonnement))
                    MessageBox.Show("L'abonnement n° " + idCmd + " a été ajouté.", "Information");
                else
                    MessageBox.Show("Un problème est survenu lors de l'ajout", "Erreur");

                lesAbonnementsRevue = controller.GetAbonnementsRevue(idRevue);
                RemplirCommandesRevuesListe(lesAbonnementsRevue);
            }
        }

        /// <summary>
        /// Retourne vrai si la date de parution est comprise entre deux dates
        /// </summary>
        /// <param name="dateCommande"></param>
        /// <param name="dateFinAbo"></param>
        /// <param name="dateParution"></param>
        /// <returns></returns>
        private bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbo, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbo;
        }

        /// <summary>
        /// Retourne vrai si un abonnement est rattaché à un exemplaire
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns></returns>
        private bool isAbonnementRattaché_a_Exemplaire(Abonnement abonnement)
        {
            List<Exemplaire> exemplaires = controller.GetExemplairesRevue(abonnement.IdRevue);
            foreach (Exemplaire exemplaire in exemplaires)
            {
                if (ParutionDansAbonnement(abonnement.DateCommande, abonnement.DateFinAbonnement, exemplaire.DateAchat))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Supprime la commande d'une revue lors du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprCmdRevue_Click(object sender, EventArgs e)
        {
            if (dgvCmdRevueListe.SelectedRows.Count > 0)
            {
                Abonnement abonnement = (Abonnement)bdgCommandesRevueListe.List[bdgCommandesRevueListe.Position];
                if (abonnement != null)
                {

                    if (!isAbonnementRattaché_a_Exemplaire(abonnement))
                    {
                        if (MessageBox.Show("Êtes vous certains de vouloir supprimer cette commande ?", "Confirmer la suppression",
                           MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            controller.SupprimerAbonnementRevue(abonnement);
                        }

                        lesAbonnementsRevue = controller.GetAbonnementsRevue(abonnement.IdRevue);
                        RemplirCommandesRevuesListe(lesAbonnementsRevue);
                    }
                    else
                    {
                        MessageBox.Show("Vous ne pouvez pas supprimer une commande de revue qui est rattaché à un exemplaire", "Information");
                    }
                }
            }
        }
        #endregion
    }
}
