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
    
    public partial class Contacto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contacto()
        {
            this.ContactoModuloNav = new HashSet<ContactoModulo>();
            this.ContactoEnvioNav = new HashSet<ContactoEnvio>();
        }
    
        public System.Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Cargo { get; set; }
        public string Celular { get; set; }
        public System.Guid Fuente { get; set; }
        public string Entidad { get; set; }
        public System.Guid S_Usuario { get; set; }
        public System.DateTime S_Creacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactoModulo> ContactoModuloNav { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactoEnvio> ContactoEnvioNav { get; set; }
    }
}
