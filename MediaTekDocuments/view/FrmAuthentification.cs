using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Classe d'afficage d'authenfication
    /// </summary>
    public partial class FrmAuthentification : Form
    {
        private readonly FrmAuthentificationController controller;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        public FrmAuthentification()
        {
            InitializeComponent();
            controller = new FrmAuthentificationController();
        }

        /// <summary>
        /// Permet d'accéder à l'application en se connectant lors du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            string login = txbLogin.Text;
            string pwd = txbPwd.Text;
            
            if(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(pwd))
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Information");
            }
            else
            {
                Utilisateur utilisateur = controller.GetUtilisateur(login, pwd);
                if(utilisateur != null)
                {
                    if (utilisateur.Service == "culture")
                    {
                        MessageBox.Show("Les membres du service Culture n'ont pas accès à cette application", "Alerte");
                    }
                    else
                    {
                        FrmMediatek frm = new FrmMediatek(utilisateur.Service);
                        frm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Authentification incorrecte", "Alerte");
                }
            }
        }
    }
}
