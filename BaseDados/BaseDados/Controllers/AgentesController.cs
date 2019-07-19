using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BaseDados.Models;
using Microsoft.AspNet.Identity;

namespace BaseDados.Controllers
{
    [Authorize]  //obriga a que os utilizadores estejam autenticados
    public class AgentesController : Controller
    {
        private MultasDB db = new MultasDB();

        // GET: Agentes
        [Authorize(Roles = "RecursosHumanos,Agentes")]//alem de autenticado, so os utilizadores do tipo RecursosHumanos ou Agentes tem acesso
        public ActionResult Index()
        {
            //SELECt * FROM Agentes ORDER BY Nome
            var lista = db.Agentes
                        .OrderBy( a => a.Nome)
                        .ToList();

            //filtrar os dados se a pessoa nao pertence ao ROLL ´RecursosHumanos`
            if (!User.IsInRole("RecursosHumanos")) {
                //Mostrar apenas os dados da pessoa
                lista = lista.Where(a => a.UserNameID == User.Identity.GetUserId()).ToList();
                }

            return View(lista);
        }
        // GET: Agentes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Create
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// recolhe os dados da View, sobre um novo agente
        /// </summary>
        /// <param name="agente">dados do novo agente</param>
        /// <param name="fotografia">ficheiro com a foto do novo Agente</param>
        /// <returns></returns>
        [Authorize(Roles = "RecursosHumanos")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente, HttpPostedFileBase fotografia)
        {

            //variaveis auxiliares
            string caminho = " ";
            bool ficheiroValido = false;

			//1º será que foi enviado um ficheiro ?
			if (fotografia == null) {
                agente.Fotografia = "noUser.png";
			} else{
                //2º será que o ficheiro, se foi fornecido, é do tipo correto ?
                string mimeType = fotografia.ContentType;
                if(mimeType=="image/jpeg" || mimeType == "image/png") {
                    //o ficheiro é do tipo correto
                    
                //3º qual o nome a atribuir ao ficheiro?

                Guid g;
                g = Guid.NewGuid(); // obtem os dados para o nome do ficheiro
                //e qual a extensao do ficheiro
                string extensao = Path.GetExtension(fotografia.FileName).ToLower();
                //montar o novo nome
                string nomeFicheiro = g.ToString() + extensao;
                //onde guardar o ficheiro ?
                caminho = Path.Combine(Server.MapPath("~/imagens/"),nomeFicheiro);


                //4º como o associar ao novo agente?
                agente.Fotografia = nomeFicheiro;

                    //marcar ficheiro como valido
                    ficheiroValido = true;
                //5º como guardar no disco rígido? e onde?
                }else {
                    agente.Fotografia = "noUser.png";
                }
            }

                

              
				            

            //confronta os dados que vem da View com a forma que os dados devem ter
            //isto é, valida os dados com o Modelo
            if (ModelState.IsValid){
                try {
                    db.Agentes.Add(agente);
                    db.SaveChanges();

                    if(ficheiroValido) fotografia.SaveAs(caminho);
                    return RedirectToAction("Index");
                }
                catch (Exception) {

                    throw;
                }
                
            }

            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// concretizar a operacao de remocao de um agente
        /// </summary>
        /// <param name="agentes">identificador do agente</param>
        /// <returns></returns>
        [Authorize(Roles = "RecursosHumanos")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        /// <summary>
        /// mostra na view os dados de um agente para posterior, eventual, remoçao
        /// </summary>
        /// <param name="id">indentificador do agente a remover</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            //o ID do agente não foi fornecido
            //nao é possivel procurar o Agente
            //o que devo fazer?
            
            if (id == null)
            {
                //opçao por defeito do template
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                ///e não há id do agente, uma de duas coisas aconteceu
                ///   - há um erro nos links da aplicação
                ///   - há um 'chico esperto' a fazer asneiras no URL

                ///redirecciono o utilizador para o ecrã principal
                return RedirectToAction("Index");
            }

            //avaliar se o Método de delete que é fornecido
            //é o mesmo id que foi apresentado no ecra
            string operacao = "Agentes/Delete";
            if (operacao != (string)Session["Método"]) {
                return RedirectToAction("Index");
                }


            //avaliar se o id do agente que é fornecido
            //é o mesmo id que foi apresentado no ecra

            if (id != (int)Session["IdAgente"]) {
                return RedirectToAction("Index");
                }
            //procura os dados do agente cujos dados sao fornecidos
            Agentes agente = db.Agentes.Find(id);

            ///se o agente nao for encontrado
            if (agente == null)
            {
                ///Ou há um erro no código,
                ///ou há um chico esperto a alterar o URL
                ///redirecciono o utilizador para o ecrã principal
                return RedirectToAction("Index");
            }
            

            //para o caso do utilizador alterar, de forma fraudulenta os dados, vamos guardá-los internamente
            //para isso, vou guardar o valor do ID do agente
            // - guardar o ID do Agente num cookie cifrado
            //- guardar o ID numa var. de sessao (quem estiver a user Asp.net Core já nao tem esta ferramenta...)
            // - Outras opçoes 

            Session["IdAgente"] = agente.ID;
            Session["Método"] = "Agentes/Delete";
            


            //envia para a View os dados do agente encontrado
            return View(agente);
        }

        // POST: Agentes/Delete/5
        [Authorize(Roles = "RecursosHumanos")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null) {
				//se entrei aqui, é porque há um erro
				//nao se sabe o ID do agente a remover
				return RedirectToAction("Index");
            }

			//procurar os dados do Agente, na BD
			Agentes agente = db.Agentes.Find(id);
			if (agente==null) {
				//nao foi possivel encontrar o agente
				return RedirectToAction("Index");
			}
            

            try
            {
                db.Agentes.Remove(agente);
                db.SaveChanges();
            }
            catch(Exception)
            {
                //captura a excessão e processa o código para resolver o problema
                //pode haver mais de que um 'catch' associado a um 'try'

                //enviar mensagem de erro para o utilizador
                ModelState.AddModelError("", "Ocorreu um erro com a eliminação do Agente "
                                             + agente.Nome+
                                             ". Provavelmente relacionado com o facto do agente ter" +
                                             "emitido multas...");

                //devolver os dados do agente à View
                return View(agente);
            }
            //redirecciona o interface para view Index associada ao controller Agentes
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
