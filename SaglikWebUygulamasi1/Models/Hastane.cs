//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SaglikWebUygulamasi1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hastane
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hastane()
        {
            this.HastaneBilgisi = new HashSet<HastaneBilgisi>();
        }
    
        public int HastaneID { get; set; }
        public string HastaneAd { get; set; }
        public int IlID { get; set; }
        public int IlceID { get; set; }
        public string HastaneAdresi { get; set; }
    
        public virtual Il Il { get; set; }
        public virtual Ilce Ilce { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastaneBilgisi> HastaneBilgisi { get; set; }
    }
}
