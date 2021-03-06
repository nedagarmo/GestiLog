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
    
    public partial class Aerolinea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Aerolinea()
        {
            this.MawbNav = new HashSet<Mawb>();
            this.MawbConexionNav = new HashSet<Mawb>();
            this.TarifarioAereoNav = new HashSet<TarifarioAereo>();
        }
    
        public System.Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Nit { get; set; }
        public string Dv { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public System.Guid Ciudad { get; set; }
        public string Representante { get; set; }
        public System.Guid S_Usuario { get; set; }
        public System.DateTime S_Creacion { get; set; }
        public bool Habilitado { get; set; }
        public Nullable<System.Guid> TarifarioEscala { get; set; }
    
        public virtual Ciudad CiudadNav { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mawb> MawbNav { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mawb> MawbConexionNav { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarifarioAereo> TarifarioAereoNav { get; set; }
        public virtual TarifarioEscala TarifarioEscalaNav { get; set; }
    }
}
