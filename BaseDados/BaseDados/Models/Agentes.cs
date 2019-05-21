using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaseDados.Models {
    public class Agentes {
        public Agentes() {
            ListaDeMultas = new HashSet<Multas>();
        }

        [Key] //identifica este atributo como Primary Key
        public int ID { get; set; }

        [Required(ErrorMessage ="O nome é de preenchimento obrigatório.")]
        [StringLength(50,ErrorMessage ="O {0} deve ter, no máximo, {1} caracteres.")]
        [RegularExpression("[A-ZÁÉÍÓÚ][a-záéíóúàèÌòùãõîôûâç]+(( | e | de | do | das | da | dos |-|')[A-ZÁÉÍÓÚ][a-zzáéíóúàèÌòùãõîôûâç]+)*",
            ErrorMessage ="O {0} só pode conter letras. Cada palavra deve começar com Maiúscula.")]

        //[A-ZÁÉÍÓÚ][a-záéíóúàèÌòù]+(( | e | de | do | das | da | dos |-|')[A-ZÁÉÍÓÚ][a-z]+)* 
        //( ((e|de|do|das|da|dos) )?)
        //( ((e|d(a(s)?|o(s)?|e)) )?)
        public string Nome { get; set; }

        [Required(ErrorMessage = "A esquadra é de preenchimento obrigatório.")]
        [StringLength(20, ErrorMessage = "A {0} deve ter, no máximo, {1} caracteres.")]
        [RegularExpression("[A-Z][a-z]+(( |-)[A-Z][a-z]+)*",
            ErrorMessage = "O {0} só pode conter letras. Cada palavra deve começar com Maiúscula.")]
        public string Esquadra { get; set; }
        
        public string Fotografia { get; set; }

     
        /// <summary>
        /// lista das multas associadas ao Agente
        /// </summary>
        public virtual ICollection<Multas> ListaDeMultas { get; set; }
            //este termo virtual vai ativar a funcionalidade de lazy loading
    }
}