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
    
    public partial class Ilac
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ilac()
        {
            this.HastalikBilgisi = new HashSet<HastalikBilgisi>();
        }
    
        public int IlacID { get; set; }
        public string IlacAd { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HastalikBilgisi> HastalikBilgisi { get; set; }
    }
}
