using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaseDados.Models {
    public class Agentes {

        [Key] //identifica este atributo como Primary Key
        public int ID { get; set; }

        public string Nome { get; set; }

        public string Esquadra { get; set; }

        public string Fotografia { get; set; }

        //*************************************
        //lista das multas associadas ao Agente
        public ICollection<Multas> ListaDeMultas { get; set; }
    }
}