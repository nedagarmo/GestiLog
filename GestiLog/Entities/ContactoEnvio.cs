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
    
    public partial class ContactoEnvio
    {
        public System.Guid Id { get; set; }
        public System.Guid Contacto { get; set; }
        public System.Guid Fuente { get; set; }
        public string Entidad { get; set; }
    
        public virtual Contacto ContactoNav { get; set; }
    }
}