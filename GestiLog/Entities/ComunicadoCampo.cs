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
    
    public partial class ComunicadoCampo
    {
        public System.Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Entidad { get; set; }
        public bool Habilitado { get; set; }
        public string Campo { get; set; }
        public string Tipo { get; set; }
        public string Fuente { get; set; }
        public System.Guid Modulo { get; set; }
    
        public virtual Modulo ModuloNav { get; set; }
    }
}