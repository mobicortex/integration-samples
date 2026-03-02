namespace SmartSdk.Forms
{
    partial class FormCadastroSimples
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // ====== Componentes principais ======
            lblExplicacao = new Label();
            splitMain = new SplitContainer();

            // -- Painel Entidades (esquerdo) --
            panelEntidades = new Panel();
            lblEntidadesTitulo = new Label();
            lblStatusEntidades = new Label();
            panelEntBotoes = new FlowLayoutPanel();
            btnNovaEntidade = new Button();
            btnExcluirEntidade = new Button();
            txtFiltroEntidade = new TextBox();
            btnBuscarEntidade = new Button();
            panelPaginacao = new FlowLayoutPanel();
            btnAnterior = new Button();
            lblPagina = new Label();
            btnProxima = new Button();
            listEntidades = new ListView();
            colEntId = new ColumnHeader();
            colEntTipo = new ColumnHeader();
            colEntNome = new ColumnHeader();
            colEntDoc = new ColumnHeader();
            colEntCadId = new ColumnHeader();

            // -- Painel Mídias (direito) --
            panelMidias = new Panel();
            lblMidiasTitulo = new Label();
            lblStatusMidias = new Label();
            panelMidBotoes = new FlowLayoutPanel();
            btnNovaMidia = new Button();
            btnExcluirMidia = new Button();
            listMidias = new ListView();
            colMidId = new ColumnHeader();
            colMidTipo = new ColumnHeader();
            colMidDesc = new ColumnHeader();
            colMidHab = new ColumnHeader();

            // -- Log --
            txtLog = new TextBox();

            // ====== Begin Init ======
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            panelEntidades.SuspendLayout();
            panelMidias.SuspendLayout();
            panelEntBotoes.SuspendLayout();
            panelMidBotoes.SuspendLayout();
            panelPaginacao.SuspendLayout();
            SuspendLayout();

            // ====== lblExplicacao ======
            lblExplicacao.BackColor = Color.FromArgb(0, 120, 60);
            lblExplicacao.Dock = DockStyle.Top;
            lblExplicacao.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblExplicacao.ForeColor = Color.White;
            lblExplicacao.Padding = new Padding(10, 5, 10, 5);
            lblExplicacao.Text = "CADASTRO SIMPLIFICADO (2 níveis): Crie entidades com createid=true — " +
                "o controlador gera os IDs automaticamente. Não é necessário criar cadastro central antes.";
            lblExplicacao.TextAlign = ContentAlignment.MiddleLeft;
            lblExplicacao.Height = 40;
            lblExplicacao.Name = "lblExplicacao";

            // ====== splitMain ======
            splitMain.Dock = DockStyle.Fill;
            splitMain.SplitterDistance = 500;
            splitMain.Name = "splitMain";

            // ====================================================================
            //  PAINEL ENTIDADES (Panel1 do split)
            // ====================================================================
            splitMain.Panel1.Controls.Add(panelEntidades);

            // -- panelEntidades --
            panelEntidades.Dock = DockStyle.Fill;
            panelEntidades.Name = "panelEntidades";

            // -- lblEntidadesTitulo --
            lblEntidadesTitulo.Dock = DockStyle.Top;
            lblEntidadesTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEntidadesTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            lblEntidadesTitulo.Text = "Entidades";
            lblEntidadesTitulo.Height = 28;
            lblEntidadesTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblEntidadesTitulo.Name = "lblEntidadesTitulo";

            // -- lblStatusEntidades --
            lblStatusEntidades.Dock = DockStyle.Top;
            lblStatusEntidades.Font = new Font("Segoe UI", 8F);
            lblStatusEntidades.ForeColor = Color.Gray;
            lblStatusEntidades.Text = "Carregando...";
            lblStatusEntidades.Height = 20;
            lblStatusEntidades.Name = "lblStatusEntidades";

            // -- panelEntBotoes (FlowLayout) --
            panelEntBotoes.Dock = DockStyle.Top;
            panelEntBotoes.Height = 34;
            panelEntBotoes.AutoSize = false;
            panelEntBotoes.WrapContents = false;
            panelEntBotoes.Name = "panelEntBotoes";

            // btnNovaEntidade
            btnNovaEntidade.Text = "+ Entidade";
            btnNovaEntidade.AutoSize = true;
            btnNovaEntidade.BackColor = Color.FromArgb(0, 120, 60);
            btnNovaEntidade.ForeColor = Color.White;
            btnNovaEntidade.FlatStyle = FlatStyle.Flat;
            btnNovaEntidade.Cursor = Cursors.Hand;
            btnNovaEntidade.Click += btnNovaEntidade_Click;
            btnNovaEntidade.Name = "btnNovaEntidade";

            // btnExcluirEntidade
            btnExcluirEntidade.Text = "Excluir";
            btnExcluirEntidade.AutoSize = true;
            btnExcluirEntidade.FlatStyle = FlatStyle.Flat;
            btnExcluirEntidade.ForeColor = Color.Firebrick;
            btnExcluirEntidade.Cursor = Cursors.Hand;
            btnExcluirEntidade.Click += btnExcluirEntidade_Click;
            btnExcluirEntidade.Name = "btnExcluirEntidade";

            // txtFiltroEntidade
            txtFiltroEntidade.Width = 140;
            txtFiltroEntidade.PlaceholderText = "ID ou nome...";
            txtFiltroEntidade.Margin = new Padding(12, 4, 0, 0);
            txtFiltroEntidade.KeyDown += txtFiltroEntidade_KeyDown;
            txtFiltroEntidade.Name = "txtFiltroEntidade";

            // btnBuscarEntidade
            btnBuscarEntidade.Text = "Buscar";
            btnBuscarEntidade.AutoSize = true;
            btnBuscarEntidade.FlatStyle = FlatStyle.Flat;
            btnBuscarEntidade.Cursor = Cursors.Hand;
            btnBuscarEntidade.Click += btnBuscarEntidade_Click;
            btnBuscarEntidade.Name = "btnBuscarEntidade";

            panelEntBotoes.Controls.Add(btnNovaEntidade);
            panelEntBotoes.Controls.Add(btnExcluirEntidade);
            panelEntBotoes.Controls.Add(txtFiltroEntidade);
            panelEntBotoes.Controls.Add(btnBuscarEntidade);

            // -- panelPaginacao --
            panelPaginacao.Dock = DockStyle.Top;
            panelPaginacao.Height = 30;
            panelPaginacao.AutoSize = false;
            panelPaginacao.WrapContents = false;
            panelPaginacao.Name = "panelPaginacao";

            // btnAnterior
            btnAnterior.Text = "<< Ant.";
            btnAnterior.AutoSize = true;
            btnAnterior.FlatStyle = FlatStyle.Flat;
            btnAnterior.Enabled = false;
            btnAnterior.Click += btnAnterior_Click;
            btnAnterior.Name = "btnAnterior";

            // lblPagina
            lblPagina.Text = "0/0";
            lblPagina.AutoSize = true;
            lblPagina.TextAlign = ContentAlignment.MiddleCenter;
            lblPagina.Margin = new Padding(6, 6, 6, 0);
            lblPagina.Name = "lblPagina";

            // btnProxima
            btnProxima.Text = "Próx. >>";
            btnProxima.AutoSize = true;
            btnProxima.FlatStyle = FlatStyle.Flat;
            btnProxima.Enabled = false;
            btnProxima.Click += btnProxima_Click;
            btnProxima.Name = "btnProxima";

            panelPaginacao.Controls.Add(btnAnterior);
            panelPaginacao.Controls.Add(lblPagina);
            panelPaginacao.Controls.Add(btnProxima);

            // -- listEntidades --
            listEntidades.Dock = DockStyle.Fill;
            listEntidades.View = View.Details;
            listEntidades.FullRowSelect = true;
            listEntidades.GridLines = true;
            listEntidades.MultiSelect = false;
            listEntidades.Columns.AddRange(new ColumnHeader[] { colEntId, colEntTipo, colEntNome, colEntDoc, colEntCadId });
            listEntidades.SelectedIndexChanged += listEntidades_SelectedIndexChanged;
            listEntidades.Name = "listEntidades";
            colEntId.Text = "ID"; colEntId.Width = 90;
            colEntTipo.Text = "Tipo"; colEntTipo.Width = 60;
            colEntNome.Text = "Nome"; colEntNome.Width = 150;
            colEntDoc.Text = "Doc/Placa"; colEntDoc.Width = 90;
            colEntCadId.Text = "CadID"; colEntCadId.Width = 70;

            // Ordem de adição (Dock.Top adiciona de cima pra baixo):
            panelEntidades.Controls.Add(listEntidades);       // Fill (fica por último = ocupa o restante)
            panelEntidades.Controls.Add(panelPaginacao);       // Top
            panelEntidades.Controls.Add(panelEntBotoes);       // Top
            panelEntidades.Controls.Add(lblStatusEntidades);   // Top
            panelEntidades.Controls.Add(lblEntidadesTitulo);   // Top

            // ====================================================================
            //  PAINEL MÍDIAS (Panel2 do split)
            // ====================================================================
            splitMain.Panel2.Controls.Add(panelMidias);

            // -- panelMidias --
            panelMidias.Dock = DockStyle.Fill;
            panelMidias.Name = "panelMidias";

            // -- lblMidiasTitulo --
            lblMidiasTitulo.Dock = DockStyle.Top;
            lblMidiasTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMidiasTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            lblMidiasTitulo.Text = "Mídias";
            lblMidiasTitulo.Height = 28;
            lblMidiasTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblMidiasTitulo.Name = "lblMidiasTitulo";

            // -- lblStatusMidias --
            lblStatusMidias.Dock = DockStyle.Top;
            lblStatusMidias.Font = new Font("Segoe UI", 8F);
            lblStatusMidias.ForeColor = Color.Gray;
            lblStatusMidias.Text = "Selecione uma entidade";
            lblStatusMidias.Height = 20;
            lblStatusMidias.Name = "lblStatusMidias";

            // -- panelMidBotoes (FlowLayout) --
            panelMidBotoes.Dock = DockStyle.Top;
            panelMidBotoes.Height = 34;
            panelMidBotoes.AutoSize = false;
            panelMidBotoes.WrapContents = false;
            panelMidBotoes.Name = "panelMidBotoes";

            // btnNovaMidia
            btnNovaMidia.Text = "+ Mídia";
            btnNovaMidia.AutoSize = true;
            btnNovaMidia.BackColor = Color.FromArgb(0, 120, 60);
            btnNovaMidia.ForeColor = Color.White;
            btnNovaMidia.FlatStyle = FlatStyle.Flat;
            btnNovaMidia.Cursor = Cursors.Hand;
            btnNovaMidia.Click += btnNovaMidia_Click;
            btnNovaMidia.Name = "btnNovaMidia";

            // btnExcluirMidia
            btnExcluirMidia.Text = "Excluir";
            btnExcluirMidia.AutoSize = true;
            btnExcluirMidia.FlatStyle = FlatStyle.Flat;
            btnExcluirMidia.ForeColor = Color.Firebrick;
            btnExcluirMidia.Cursor = Cursors.Hand;
            btnExcluirMidia.Click += btnExcluirMidia_Click;
            btnExcluirMidia.Name = "btnExcluirMidia";

            panelMidBotoes.Controls.Add(btnNovaMidia);
            panelMidBotoes.Controls.Add(btnExcluirMidia);

            // -- listMidias --
            listMidias.Dock = DockStyle.Fill;
            listMidias.View = View.Details;
            listMidias.FullRowSelect = true;
            listMidias.GridLines = true;
            listMidias.MultiSelect = false;
            listMidias.Columns.AddRange(new ColumnHeader[] { colMidId, colMidTipo, colMidDesc, colMidHab });
            listMidias.Name = "listMidias";
            colMidId.Text = "ID"; colMidId.Width = 90;
            colMidTipo.Text = "Tipo"; colMidTipo.Width = 80;
            colMidDesc.Text = "Descrição"; colMidDesc.Width = 150;
            colMidHab.Text = "Hab"; colMidHab.Width = 40;

            // Ordem de adição para mídias
            panelMidias.Controls.Add(listMidias);          // Fill
            panelMidias.Controls.Add(panelMidBotoes);      // Top
            panelMidias.Controls.Add(lblStatusMidias);     // Top
            panelMidias.Controls.Add(lblMidiasTitulo);     // Top

            // ====== txtLog (console escuro no rodapé) ======
            txtLog.Dock = DockStyle.Bottom;
            txtLog.Multiline = true;
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Height = 100;
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.ForeColor = Color.LightGreen;
            txtLog.Font = new Font("Consolas", 8.5F);
            txtLog.Name = "txtLog";

            // ====== Form ======
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1050, 620);
            Controls.Add(splitMain);
            Controls.Add(txtLog);
            Controls.Add(lblExplicacao);
            Text = "Cadastro Simplificado (Entidade → Mídia) — createid=true";
            Load += FormCadastroSimples_Load;
            Name = "FormCadastroSimples";

            // ====== End Init ======
            panelPaginacao.ResumeLayout(false);
            panelPaginacao.PerformLayout();
            panelEntBotoes.ResumeLayout(false);
            panelEntBotoes.PerformLayout();
            panelMidBotoes.ResumeLayout(false);
            panelMidBotoes.PerformLayout();
            panelEntidades.ResumeLayout(false);
            panelMidias.ResumeLayout(false);
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // -- Topo --
        private Label lblExplicacao;

        // -- Split --
        private SplitContainer splitMain;

        // -- Entidades --
        private Panel panelEntidades;
        private Label lblEntidadesTitulo;
        private Label lblStatusEntidades;
        private FlowLayoutPanel panelEntBotoes;
        private Button btnNovaEntidade;
        private Button btnExcluirEntidade;
        private TextBox txtFiltroEntidade;
        private Button btnBuscarEntidade;
        private FlowLayoutPanel panelPaginacao;
        private Button btnAnterior;
        private Label lblPagina;
        private Button btnProxima;
        private ListView listEntidades;
        private ColumnHeader colEntId;
        private ColumnHeader colEntTipo;
        private ColumnHeader colEntNome;
        private ColumnHeader colEntDoc;
        private ColumnHeader colEntCadId;

        // -- Mídias --
        private Panel panelMidias;
        private Label lblMidiasTitulo;
        private Label lblStatusMidias;
        private FlowLayoutPanel panelMidBotoes;
        private Button btnNovaMidia;
        private Button btnExcluirMidia;
        private ListView listMidias;
        private ColumnHeader colMidId;
        private ColumnHeader colMidTipo;
        private ColumnHeader colMidDesc;
        private ColumnHeader colMidHab;

        // -- Log --
        private TextBox txtLog;
    }
}
