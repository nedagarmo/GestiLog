using GestiLog.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GestiLog.Models
{
    public class MblViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Do { get; set; }

        public string Master { get; set; }

        public Nullable<Guid> Agente { get; set; }

        public Nullable<Guid> Naviera { get; set; }

        public Nullable<Guid> Operativo { get; set; }

        public Nullable<Guid> TipoEmbarque { get; set; }

        public Nullable<Guid> PuertoOrigen { get; set; }

        public Nullable<Guid> PuertoDestino { get; set; }

        public Nullable<DateTime> Etd { get; set; }

        public Nullable<DateTime> Eta { get; set; }

        public bool Transbordo { get; set; }

        public Nullable<DateTime> FechaEstimadaArribo { get; set; }

        public Nullable<Guid> PuertoTransbordo { get; set; }

        public Nullable<Guid> MotonaveEstimadaArribo { get; set; }

        public string ViajeEstimadoArribo { get; set; }

        [Required]
        public Nullable<double> Peso { get; set; }

        [Required]
        public Nullable<double> Volumen { get; set; }

        public Nullable<Guid> Embalaje { get; set; }

        public Nullable<int> Piezas { get; set; }

        public bool ExoneracionDeposito { get; set; }

        public bool ExoneracionDroopOff { get; set; }

        public string DiaLibreNaviera { get; set; }

        public Nullable<Guid> RangoDiaLibre { get; set; }

        public Nullable<Guid> MuelleArribo { get; set; }

        public Nullable<Guid> EmisionBl { get; set; }

        public Nullable<int> NumeroManifiesto { get; set; }

        public Nullable<Guid> Flete { get; set; }

        public Nullable<DateTime> RadicadoMuisca { get; set; }

        public Nullable<DateTime> Desconsolidacion { get; set; }

        public Nullable<DateTime> Finalizacion { get; set; }

        public Guid S_Usuario { get; set; }

        public DateTime S_Creacion { get; set; }

        public bool Habilitado { get; set; }

        public List<Contenedor> Contenedores { get; set; }
        public List<Hbl> Hbls { get; set; }
    }

    public class NoticeViewModel
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Contenido { get; set; }
        public Guid Modulo { get; set; }
        public Guid S_Usuario { get; set; }
        public DateTime S_Creacion { get; set; }
        public bool Habilitado { get; set; }
        public List<string> AdjuntosMBL { get; set; }
        public List<string> AdjuntosHBL { get; set; }
    }

    public class ContactViewModel
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        public string Celular { get; set; }
        public string Cargo { get; set; }
        public string Entidad { get; set; }
        public Guid Fuente { get; set; }
        public DateTime S_Creacion { get; set; }
        public Guid S_Usuario { get; set; }
        public List<Guid> Modulos { get; set; }

        public Contacto ObtenerContacto()
        {
            Contacto contacto = new Contacto();
            contacto.Id = this.Id;
            contacto.Nombre = this.Nombre;
            contacto.Correo = this.Correo;
            contacto.Celular = this.Celular;
            contacto.Cargo = this.Cargo;
            contacto.Entidad = this.Entidad;
            contacto.Fuente = this.Fuente;
            contacto.S_Creacion = this.S_Creacion;
            contacto.S_Usuario = this.S_Usuario;

            return contacto;
        }
    }

    public class ContactPartialViewModel
    {
        public Guid Fuente { get; set; }
        public string Entidad { get; set; }
    }
}