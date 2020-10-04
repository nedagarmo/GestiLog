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
    
    public partial class TarifarioAereo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TarifarioAereo()
        {
            this.TarifarioViaAereaNav = new HashSet<TarifarioViaAerea>();
            this.TarifarioAereoItemNav = new HashSet<TarifarioAereoItem>();
        }
    
        public System.Guid Id { get; set; }
        public int Consecutivo { get; set; }
        public System.Guid Area { get; set; }
        public Nullable<System.Guid> Aerolinea { get; set; }
        public Nullable<System.Guid> Agente { get; set; }
        public Nullable<System.Guid> AeropuertoOrigen { get; set; }
        public Nullable<System.Guid> AeropuertoDestino { get; set; }
        public Nullable<System.Guid> Frecuencia { get; set; }
        public Nullable<int> TiempoTransito { get; set; }
        public Nullable<System.DateTime> VigenciaDesde { get; set; }
        public Nullable<System.DateTime> VigenciaHasta { get; set; }
        public string Mercancia { get; set; }
        public bool Dg { get; set; }
        public Nullable<System.Guid> Divisa { get; set; }
    
        public virtual Aerolinea AerolineaNav { get; set; }
        public virtual Aeropuerto AeropuertoOrigenNav { get; set; }
        public virtual Aeropuerto AeropuertoDestinoNav { get; set; }
        public virtual Agente AgenteNav { get; set; }
        public virtual Area AreaNav { get; set; }
        public virtual Frecuencia FrecuenciaNav { get; set; }
        public virtual Moneda MonedaNav { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarifarioViaAerea> TarifarioViaAereaNav { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TarifarioAereoItem> TarifarioAereoItemNav { get; set; }
    }
}