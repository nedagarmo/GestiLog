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
    
    public partial class Contenedor
    {
        public System.Guid Id { get; set; }
        public System.Guid Mbl { get; set; }
        public System.Guid Equipo { get; set; }
        public string Numero { get; set; }
        public string Serial { get; set; }
        public string Observacion { get; set; }
    
        public virtual Mbl MblNav { get; set; }
        public virtual TipoContenedor TipoContenedorNav { get; set; }
    }
}
