using GestiLog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestiLog.Models
{
    public class AerialRateViewModel
    {
        public TarifarioAereo Tarifario { get; set; }
        public List<TarifarioAereoItem> Items { get; set; }
        public List<TarifarioViaAerea> Vias { get; set; }
    }

    public class FclRateViewModel
    {
        public TarifarioFcl Tarifario { get; set; }
    }

    public class LclRateViewModel
    {
        public TarifarioLcl Tarifario { get; set; }
    }
}