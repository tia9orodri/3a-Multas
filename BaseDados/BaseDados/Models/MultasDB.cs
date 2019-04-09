using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BaseDados.Models {
    public class MultasDB : DbContext {

        //identificar qual o sGBD a usar

        //Definir as tabelas da BD

        public MultasDB() : base("MultasDBConnectionString") { }

        // vamos colocar, aqui, as instruções relativas às tabelas do 'negócio'
        // descrever os nomes das tabelas na Base de Dados
        public virtual DbSet<Multas> Multas { get; set; } // tabela Multas
        public virtual DbSet<Condutores> Condutores { get; set; } // tabela Condutores
        public virtual DbSet<Agentes> Agentes { get; set; } // tabela Agentes
        public virtual DbSet<Viaturas> Viaturas { get; set; } // tabela Viaturas

        //método a ser executado no inicio da criação do Modelo
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            //eliminar a convenção de atribuir automáticamente 'on Delete Cascade' nas FKs
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }


    }
}