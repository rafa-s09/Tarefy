<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Tarefy._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

       <main>
            <div class="container mt-4">
            <!-- Menu de Navegação -->
            <div class="d-flex justify-content-between mb-4">
                <h1 class="h3">Lista de Afazeres</h1>
                <div>
                    <asp:Button ID="btnNovaTarefa" runat="server" Text="Nova Tarefa" CssClass="btn btn-primary me-2" OnClick="btnNovaTarefa_Click" />
                    <asp:Button ID="btnListarTarefas" runat="server" Text="Listar Tarefas" CssClass="btn btn-secondary" OnClick="btnListarTarefas_Click" />
                </div>
            </div>

            <!-- Panel para Cadastro de Tarefas -->
            <asp:Panel ID="pnlCadastro" runat="server" CssClass="card hidden">
                <div class="card-body">
                    <h2 class="card-title h5">Nova Tarefa</h2>
                    <div class="mb-3">
                        <asp:Label ID="lblDescricao" runat="server" Text="Descrição" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" placeholder="Digite a descrição da tarefa"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblDetalhes" runat="server" Text="Detalhes" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtDetalhes" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3" placeholder="Detalhes opcionais"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblDataLimite" runat="server" Text="Data Limite" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtDataLimite" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblPrioridade" runat="server" Text="Prioridade" CssClass="form-label"></asp:Label>
                        <asp:DropDownList ID="ddlPrioridade" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Baixa" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Normal" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Alta" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Urgente" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="d-flex justify-content-end">
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success me-2" OnClick="btnSalvar_Click" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary" OnClick="btnCancelar_Click" />
                    </div>
                </div>
            </asp:Panel>

            <!-- Panel para Listagem de Tarefas -->
            <asp:Panel ID="pnlListagem" runat="server" CssClass="card hidden">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h2 class="card-title h5 mb-0">Lista de Tarefas</h2>
                        <asp:DropDownList ID="ddlFiltroStatus" runat="server" CssClass="form-select w-auto" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroStatus_SelectedIndexChanged">
                            <asp:ListItem Text="Pendentes" Value="Pendente"></asp:ListItem>
                            <asp:ListItem Text="Concluídas" Value="Concluido"></asp:ListItem>
                            <asp:ListItem Text="Todas" Value="Todas" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvTarefas" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover" OnRowDataBound="gvTarefas_RowDataBound" OnRowCommand="gvTarefas_RowCommand" DataKeyNames="Id">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                                <asp:BoundField DataField="Detalhes" HeaderText="Detalhes" />
                                <asp:BoundField DataField="DataCriacao" HeaderText="Criada em" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="DataLimite" HeaderText="Data Limite" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="Prioridade" HeaderText="Prioridade" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnEditar" Text="Editar" CommandName="Editar" CssClass="btn btn-sm btn-outline-primary" />
                                        <asp:Button runat="server" ID="btnConcluir" Text="Concluir" CommandName="Concluir" CssClass="btn btn-sm btn-outline-success" />
                                        <asp:Button runat="server" ID="btnRemover" Text="Remover" CommandName="Remover" CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Deseja realmente remover esta tarefa?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>

            <!-- Panel para Edição de Tarefas -->
            <asp:Panel ID="pnlEdicao" runat="server" CssClass="card hidden">
                <div class="card-body">
                    <h2 class="card-title h5">Editar Tarefa</h2>
                    <asp:HiddenField ID="hfId" runat="server" />
                    <div class="mb-3">
                        <asp:Label ID="lblEditDescricao" runat="server" Text="Descrição" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtEditDescricao" runat="server" CssClass="form-control" placeholder="Digite a descrição da tarefa"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblEditDetalhes" runat="server" Text="Detalhes" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtEditDetalhes" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3" placeholder="Detalhes opcionais"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblEditDataLimite" runat="server" Text="Data Limite" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtEditDataLimite" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:Label ID="lblEditPrioridade" runat="server" Text="Prioridade" CssClass="form-label"></asp:Label>
                        <asp:DropDownList ID="ddlEditPrioridade" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Baixa" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Alta" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Urgente" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="d-flex justify-content-end">
                        <asp:Button ID="btnAtualizar" runat="server" Text="Atualizar" CssClass="btn btn-success me-2" OnClick="btnAtualizar_Click" />
                        <asp:Button ID="btnCancelarEdicao" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary" OnClick="btnCancelarEdicao_Click" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        </main>

</asp:Content>
