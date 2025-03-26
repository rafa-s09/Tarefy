using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tarefy.Enumeradores;
using Tarefy.Models;
using Tarefy.Repositorio;

namespace Tarefy
{
    public partial class _Default : Page
    {
        private TarefaRepository _tarefaRepository;

        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _tarefaRepository = new TarefaRepository(connectionString);

            if (!IsPostBack)
            {
                MostrarPanel(pnlListagem);
                AtualizarGrid();
            }
        }


        private void MostrarPanel(Panel panel)
        {
            pnlCadastro.CssClass = "card hidden";
            pnlListagem.CssClass = "card hidden";
            pnlEdicao.CssClass = "card hidden";
            panel.CssClass = "card"; 
        }

        protected void btnNovaTarefa_Click(object sender, EventArgs e)
        {
            MostrarPanel(pnlCadastro);
            LimparCamposCadastro();
        }

        protected void btnListarTarefas_Click(object sender, EventArgs e)
        {
            MostrarPanel(pnlListagem);
            AtualizarGrid();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            TarefaModel tarefa = new TarefaModel
            {
                Descricao = txtDescricao.Text,
                Detalhes = txtDetalhes.Text,
                DataCriacao = DateTime.Now, 
                DataLimite = string.IsNullOrEmpty(txtDataLimite.Text) ? (DateTime?)null : DateTime.Parse(txtDataLimite.Text),
                Prioridade = (PrioridadeEnum)int.Parse(ddlPrioridade.SelectedValue),
                Status = StatusEnum.Aguardando
            };

            _tarefaRepository.InsertTarefa(tarefa);
            MostrarPanel(pnlListagem);
            AtualizarGrid();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            MostrarPanel(pnlListagem);
        }

        protected void gvTarefas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)(((Button)e.CommandSource).NamingContainer)).RowIndex;
            int id = Convert.ToInt32(gvTarefas.DataKeys[rowIndex].Value);            
            TarefaModel tarefa = _tarefaRepository.GetTarefaById(id);

            if (e.CommandName == "Editar" && tarefa.Status != StatusEnum.Concluido)
            {
                MostrarPanel(pnlEdicao);
                hfId.Value = tarefa.Id.ToString();
                txtEditDescricao.Text = tarefa.Descricao;
                txtEditDetalhes.Text = tarefa.Detalhes;
                txtEditDataLimite.Text = tarefa.DataLimite.HasValue ? tarefa.DataLimite.Value.ToString("yyyy-MM-dd") : "";
                ddlEditPrioridade.SelectedValue = ((int)tarefa.Prioridade).ToString();
            }
            else if (e.CommandName == "Concluir")
            {
                tarefa.Status = StatusEnum.Concluido;
                _tarefaRepository.UpdateTarefa(tarefa);
                AtualizarGrid();
            }
            else if (e.CommandName == "Remover")
            {
                if (Confirm("Deseja realmente remover esta tarefa?"))
                {
                    _tarefaRepository.DeleteTarefa(id);
                    AtualizarGrid();
                }
            }            
        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hfId.Value);
            TarefaModel tarefa = _tarefaRepository.GetTarefaById(id);

            if (tarefa != null)
            {
                tarefa.Descricao = txtEditDescricao.Text;
                tarefa.Detalhes = txtEditDetalhes.Text;
                tarefa.DataLimite = string.IsNullOrEmpty(txtEditDataLimite.Text) ? (DateTime?)null : DateTime.Parse(txtEditDataLimite.Text);
                tarefa.Prioridade = (PrioridadeEnum)int.Parse(ddlEditPrioridade.SelectedValue);

                _tarefaRepository.UpdateTarefa(tarefa);
            }

            MostrarPanel(pnlListagem);
            AtualizarGrid();
        }

        protected void btnCancelarEdicao_Click(object sender, EventArgs e)
        {
            MostrarPanel(pnlListagem);
        }

        protected void ddlFiltroStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void AtualizarGrid()
        {
            string filtro = ddlFiltroStatus.SelectedValue;
            List<TarefaModel> tarefas = _tarefaRepository.GetTarefas();

            if (filtro == "Pendente")
                tarefas = tarefas.Where(t => t.Status != StatusEnum.Concluido).ToList();
            else if (filtro == "Concluido")
                tarefas = tarefas.Where(t => t.Status == StatusEnum.Concluido).ToList();

            gvTarefas.DataSource = tarefas.OrderBy(t => t.DataCriacao);
            gvTarefas.DataBind();
        }

        private void LimparCamposCadastro()
        {
            txtDescricao.Text = "";
            txtDetalhes.Text = "";
            txtDataLimite.Text = "";
            ddlPrioridade.SelectedValue = "1"; // Normal
        }

        protected void gvTarefas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtém o status da tarefa da linha atual
                StatusEnum status = (StatusEnum)DataBinder.Eval(e.Row.DataItem, "Status");

                // Obtém os botões do TemplateField
                Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                Button btnConcluir = (Button)e.Row.FindControl("btnConcluir");
                Button btnRemover = (Button)e.Row.FindControl("btnRemover");

                // Verifica o status e oculta os botões se necessário
                if (status == StatusEnum.Concluido || status == StatusEnum.Cancelado)
                {
                    if (btnEditar != null) btnEditar.Visible = false;
                    if (btnConcluir != null) btnConcluir.Visible = false;
                    if (btnRemover != null) btnRemover.Visible = false;
                }
            }
        }

        private bool Confirm(string message)
        {
            return true; 
        }
    }
}