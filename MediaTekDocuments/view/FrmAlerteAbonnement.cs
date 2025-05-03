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
    /// Classe d'affichage d'alerte concernant les abonnements
    /// </summary>
    public partial class FrmAlerteAbonnement : Form
    {
        /// <summary>
        /// Constructeur : affichage des abonnement qui arrive bientot à échéance
        /// </summary>
        /// <param name="abonnements"></param>
        public FrmAlerteAbonnement(List<Abonnement> abonnements)
        {
            InitializeComponent();

            foreach(Abonnement abonnement in abonnements)
            {
                lbRevueFinAbonnement.Items.Add($"{abonnement.TitreRevue} - {abonnement.DateFinAbonnement.ToShortDateString()}");
            }
        }
    }
}
