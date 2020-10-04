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
    
    public partial class Mbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mbl()
        {
            this.ContenedorNav = new HashSet<Contenedor>();
            this.HblNav = new HashSet<Hbl>();
        }
    
        public System.Guid Id { get; set; }
        public string Do { get; set; }
        public string Master { get; set; }
        public Nullable<System.Guid> Agente { get; set; }
        public Nullable<System.Guid> Naviera { get; set; }
        public Nullable<System.Guid> Operativo { get; set; }
        public Nullable<System.Guid> TipoEmbarque { get; set; }
        public Nullable<System.Guid> PuertoOrigen { get; set; }
        public Nullable<System.Guid> PuertoDestino { get; set; }
        public Nullable<System.DateTime> Etd { get; set; }
        public Nullable<System.DateTime> Eta { get; set; }
        public bool Transbordo { get; set; }
        public Nullable<System.DateTime> FechaEstimadaArribo { get; set; }
        public Nullable<System.Guid> PuertoTransbordo { get; set; }
        public Nullable<System.Guid> MotonaveEstimadaArribo { get; set; }
        public string ViajeEstimadoArribo { get; set; }
        public Nullable<double> Peso { get; set; }
        public Nullable<double> Volumen { get; set; }
        public Nullable<System.Guid> Embalaje { get; set; }
        public Nullable<int> Piezas { get; set; }
        public System.Guid S_Usuario { get; set; }
        public System.DateTime S_Creacion { get; set; }
        public bool Habilitado { get; set; }
        public bool ExoneracionDeposito { get; set; }
        public bool ExoneracionDroopOff { get; set; }
        public string DiaLibreNaviera { get; set; }
        public Nullable<System.Guid> RangoDiaLibre { get; set; }
        public Nullable<System.Guid> MuelleArribo { get; set; }
        public Nullable<System.Guid> EmisionBl { get; set; }
        public Nullable<int> NumeroManifiesto { get; set; }
        public Nullable<System.Guid> Flete { get; set; }
        public Nullable<System.DateTime> RadicadoMuisca { get; set; }
        public Nullable<System.DateTime> Desconsolidacion { get; set; }
        public Nullable<System.DateTime> Finalizacion { get; set; }
    
        public virtual Embalaje EmbalajeNav { get; set; }
        public virtual Agente AgenteNav { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contenedor> ContenedorNav { get; set; }
        public virtual EmisionBl EmisionBlNav { get; set; }
        public virtual Flete FleteNav { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hbl> HblNav { get; set; }
        public virtual Motonave MotonaveNav { get; set; }
        public virtual Muelle MuelleNav { get; set; }
        public virtual Naviera NavieraNav { get; set; }
        public virtual Operativo OperativoNav { get; set; }
        public virtual Puerto PuertoOrigenNav { get; set; }
        public virtual Puerto PuertoDestinoNav { get; set; }
        public virtual Puerto PuertoTransbordoNav { get; set; }
        public virtual RangoDiaLibre RangoDiaLibreNav { get; set; }
        public virtual TipoEmbarque TipoEmbarqueNav { get; set; }
    }
}