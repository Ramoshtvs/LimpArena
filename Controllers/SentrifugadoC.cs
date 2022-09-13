﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LimpArena.Controllers
{
    public class SentrifugadoC
    {

        public int ID { get; set; }
        public decimal Temperatura { get; set; }
        public decimal Conductividad { get; set; }
        public decimal Ph { get; set; }
        public decimal Dureza { get; set; }
        public decimal Silice { get; set; }
        public decimal Fosfato { get; set; }
        public String Fecha { get; set; }
    }
}