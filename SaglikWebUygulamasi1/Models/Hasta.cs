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
    
    public class Hasta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hasta()
        {
            this.HastalikBilgisi = new HashSet<HastalikBilgisi>();
            this.Randevu = new HashSet<Randevu>();
            this.TahlilBilgisi = new HashSet<TahlilBilgisi>();
        }
    
        public int HastaTC { get; set; }
        public string HastaAd { get; set; }
        public string HastaSoyad { get; set; }
        public int HastaTel { get; set; }
        public string HastaCinsiyet { get; set; }
        public System.DateTime HastaDogumTarihi { get; set; }
        public int HastaYas { get; set; }
        public string HastaKanGrubu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastalikBilgisi> HastalikBilgisi { get; set; }
        public virtual Login Login { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Randevu> Randevu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TahlilBilgisi> TahlilBilgisi { get; set; }
    }
}
