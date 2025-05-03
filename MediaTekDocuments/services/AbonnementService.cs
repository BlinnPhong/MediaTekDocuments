using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.services
{
    public class AbonnementService
    {
        public static bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbo, DateTime dateParution)
        {
            bool estDansIntervalle = dateParution >= dateCommande && dateParution <= dateFinAbo;

            return estDansIntervalle;
        }
    }
}
