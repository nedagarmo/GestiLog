//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GestiLog.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Operativo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Operativo()
        {
            this.MblNav = new HashSet<Mbl>();
            this.MawbNav = new HashSet<Mawb>();
        }
    
        public System.Guid Id { get; set; }
        public string Nombre { get; set; }
        public System.Guid S_Usuario { get; set; }
        public System.DateTime S_Creacion { get; set; }
        public bool Habilitado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mbl> MblNav { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mawb> MawbNav { get; set; }
    }
}