//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projet_Navette.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LigneDemande
    {
        public int id_LigneDemande { get; set; }
        public int id_Client { get; set; }
        public int id_Demande { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual DemandeAbonnement DemandeAbonnement { get; set; }
    }
}
