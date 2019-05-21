using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BaseDados.Models {
    public class Multas {

        public int ID { get; set; }

        public string Infracao { get; set; }
        [Display(Name ="Local da Multa")]
        public string LocalDaMulta { get; set; }

        public decimal ValorMulta { get; set; }
        [Display(Name = "Data da aplicaçao da Multa")]
        public DateTime DataDaMulta { get; set; }

        //*********************************
        //criação das Chaves Forasteiras
        //*********************************

        //FK para Viatura
        /*anotação  --> anota sempre o que está por baixo */
        [ForeignKey("Viatura")]
        public int ViaturaFK { get; set; } //Base de dados
        public Viaturas Viatura { get; set; } //C#

        //FK para Condutor
        [ForeignKey("Condutor")]
        public int CondutorFK { get; set; } //Base de dados
        public Condutores Condutor { get; set; } //C#

        //FK para agentes
        [ForeignKey("Agente")]
        public int AgenteFK { get; set; } //Base de dados
        public Agentes Agente { get; set; } //C#


    }
}