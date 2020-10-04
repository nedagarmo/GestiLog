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
    
    public partial class Hbl
    {
        public System.Guid Id { get; set; }
        public System.Guid Mbl { get; set; }
        public string Do { get; set; }
        public string Hbl1 { get; set; }
        public Nullable<System.Guid> Proveedor { get; set; }
        public Nullable<System.Guid> Cliente { get; set; }
        public Nullable<System.Guid> Notify { get; set; }
        public Nullable<System.Guid> Sucursal { get; set; }
        public bool ExoneracionDeposito { get; set; }
        public bool ExoneracionDroopOff { get; set; }
        public Nullable<int> DiaLibreCliente { get; set; }
        public Nullable<System.Guid> RangoDiaLibre { get; set; }
        public Nullable<System.DateTime> FechaCargue { get; set; }
        public Nullable<System.DateTime> FechaInstruccion { get; set; }
        public Nullable<System.DateTime> FechaNotificacion { get; set; }
        public Nullable<System.Guid> Tipo { get; set; }
        public Nullable<System.Guid> PuertoOrigen { get; set; }
        public Nullable<System.Guid> PuertoDestino { get; set; }
        public Nullable<System.DateTime> Etd { get; set; }
        public Nullable<System.DateTime> Eta { get; set; }
        public Nullable<System.Guid> MotonaveEstimadaArribo { get; set; }
        public string ViajeEstimadoArribo { get; set; }
        public Nullable<double> Peso { get; set; }
        public Nullable<double> Volumen { get; set; }
        public string DescripcionMercancia { get; set; }
        public bool Peligrosa { get; set; }
        public Nullable<System.Guid> Incoterm { get; set; }
        public bool Imo { get; set; }
        public string Un { get; set; }
        public Nullable<System.Guid> MuelleArribo { get; set; }
        public Nullable<System.Guid> EmisionBl { get; set; }
        public Nullable<System.Guid> Flete { get; set; }
        public Nullable<double> Valor { get; set; }
        public Nullable<System.Guid> ServicioCliente { get; set; }
        public Nullable<System.DateTime> FechaLiberacion { get; set; }
        public string QuienLibera { get; set; }
        public System.Guid S_Usuario { get; set; }
        public System.DateTime S_Creacion { get; set; }
        public bool Habilitado { get; set; }
    
        public virtual Cliente ClienteNav { get; set; }
        public virtual EmisionBl EmisionBlNav { get; set; }
        public virtual Flete FleteNav { get; set; }
        public virtual HblTipo HblTipoNav { get; set; }
        public virtual Incoterm IncotermNav { get; set; }
        public virtual Mbl MblNav { get; set; }
        public virtual Motonave MotonaveNav { get; set; }
        public virtual Muelle MuelleNav { get; set; }
        public virtual Notify NotifyNav { get; set; }
        public virtual Proveedor ProveedorNav { get; set; }
        public virtual Puerto PuertoOrigenNav { get; set; }
        public virtual Puerto PuertoDestinoNav { get; set; }
        public virtual RangoDiaLibre RangoDiaLibreNav { get; set; }
        public virtual Sucursal SucursalNav { get; set; }
    }
}