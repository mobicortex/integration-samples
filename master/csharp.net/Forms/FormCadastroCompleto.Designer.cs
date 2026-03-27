namespace SmartSdk
{
    partial class FormCadastroCompleto
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.splitMain = new SplitContainer();
            this.splitEsquerda = new SplitContainer();
            this.panelCadastros = new Panel();
            this.listCadastros = new ListView();
            this.colCadId = new ColumnHeader();
            this.colCadNome = new ColumnHeader();
            this.colCadAtivo = new ColumnHeader();
            this.colCadContagem = new ColumnHeader();
            this.panelPaginacao = new FlowLayoutPanel();
            this.btnAnterior = new Button();
            this.lblPagina = new Label();
            this.btnProxima = new Button();
            this.panelCadastrosBotoes = new FlowLayoutPanel();
            this.btnNovoCadastro = new Button();
            this.btnExcluirCadastro = new Button();
            this.btnBuscarCadastro = new Button();
            this.txtFiltroCadastro = new TextBox();
            this.lblCadastrosTitulo = new Label();
            this.lblStatusCadastros = new Label();
            this.panelEntidades = new Panel();
            this.listEntidades = new ListView();
            this.colEntId = new ColumnHeader();
            this.colEntTipo = new ColumnHeader();
            this.colEntNome = new ColumnHeader();
            this.colEntDoc = new ColumnHeader();
            this.colEntAtivo = new ColumnHeader();
            this.colEntLpr = new ColumnHeader();
            this.panelEntidadesBotoes = new FlowLayoutPanel();
            this.btnNovaEntidade = new Button();
            this.btnExcluirEntidade = new Button();
            this.lblEntidadesTitulo = new Label();
            this.lblStatusEntidades = new Label();
            this.panelMidias = new Panel();
            this.listMidias = new ListView();
            this.colMidId = new ColumnHeader();
            this.colMidTipo = new ColumnHeader();
            this.colMidDesc = new ColumnHeader();
            this.colMidAtivo = new ColumnHeader();
            this.panelMidiasBotoes = new FlowLayoutPanel();
            this.btnNovaMidia = new Button();
            this.btnExcluirMidia = new Button();
            this.lblMidiasTitulo = new Label();
            this.lblStatusMidias = new Label();
            this.txtLog = new TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitEsquerda)).BeginInit();
            this.splitEsquerda.Panel1.SuspendLayout();
            this.splitEsquerda.Panel2.SuspendLayout();
            this.splitEsquerda.SuspendLayout();
            this.panelCadastros.SuspendLayout();
            this.panelPaginacao.SuspendLayout();
            this.panelEntidades.SuspendLayout();
            this.panelMidias.SuspendLayout();
            this.SuspendLayout();
            //
            // splitMain - Divide: [Cadastros+Entidades | Midias]
            //
            this.splitMain.Dock = DockStyle.Fill;
            this.splitMain.Location = new Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Panel1.Controls.Add(this.splitEsquerda);
            this.splitMain.Panel2.Controls.Add(this.panelMidias);
            this.splitMain.Size = new Size(1100, 500);
            this.splitMain.SplitterDistance = 700;
            this.splitMain.TabIndex = 0;
            //
            // splitEsquerda - Divide: [Cadastros | Entidades]
            //
            this.splitEsquerda.Dock = DockStyle.Fill;
            this.splitEsquerda.Location = new Point(0, 0);
            this.splitEsquerda.Name = "splitEsquerda";
            this.splitEsquerda.Panel1.Controls.Add(this.panelCadastros);
            this.splitEsquerda.Panel2.Controls.Add(this.panelEntidades);
            this.splitEsquerda.Size = new Size(700, 500);
            this.splitEsquerda.SplitterDistance = 350;
            this.splitEsquerda.TabIndex = 0;
            //
            // panelCadastros - Nivel 1: Cadastros Centrais
            //
            this.panelCadastros.Controls.Add(this.listCadastros);
            this.panelCadastros.Controls.Add(this.panelPaginacao);
            this.panelCadastros.Controls.Add(this.panelCadastrosBotoes);
            this.panelCadastros.Controls.Add(this.lblStatusCadastros);
            this.panelCadastros.Controls.Add(this.lblCadastrosTitulo);
            this.panelCadastros.Dock = DockStyle.Fill;
            this.panelCadastros.Location = new Point(0, 0);
            this.panelCadastros.Name = "panelCadastros";
            this.panelCadastros.Padding = new Padding(5);
            this.panelCadastros.Size = new Size(350, 500);
            this.panelCadastros.TabIndex = 0;
            //
            // lblCadastrosTitulo
            //
            this.lblCadastrosTitulo.Dock = DockStyle.Top;
            this.lblCadastrosTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblCadastrosTitulo.Location = new Point(5, 5);
            this.lblCadastrosTitulo.Name = "lblCadastrosTitulo";
            this.lblCadastrosTitulo.Size = new Size(340, 22);
            this.lblCadastrosTitulo.TabIndex = 0;
            this.lblCadastrosTitulo.Text = "1. Central Registries";
            //
            // lblStatusCadastros
            //
            this.lblStatusCadastros.Dock = DockStyle.Top;
            this.lblStatusCadastros.ForeColor = Color.Gray;
            this.lblStatusCadastros.Location = new Point(5, 27);
            this.lblStatusCadastros.Name = "lblStatusCadastros";
            this.lblStatusCadastros.Size = new Size(340, 18);
            this.lblStatusCadastros.TabIndex = 1;
            this.lblStatusCadastros.Text = "Loading...";
            //
            // panelCadastrosBotoes
            //
            this.panelCadastrosBotoes.Controls.Add(this.txtFiltroCadastro);
            this.panelCadastrosBotoes.Controls.Add(this.btnBuscarCadastro);
            this.panelCadastrosBotoes.Controls.Add(this.btnRefreshCadastros);
            this.panelCadastrosBotoes.Controls.Add(this.btnNovoCadastro);
            this.panelCadastrosBotoes.Controls.Add(this.btnExcluirCadastro);
            this.panelCadastrosBotoes.Dock = DockStyle.Top;
            this.panelCadastrosBotoes.Location = new Point(5, 45);
            this.panelCadastrosBotoes.Name = "panelCadastrosBotoes";
            this.panelCadastrosBotoes.Size = new Size(340, 32);
            this.panelCadastrosBotoes.TabIndex = 2;
            //
            // txtFiltroCadastro
            //
            this.txtFiltroCadastro.Location = new Point(0, 4);
            this.txtFiltroCadastro.Name = "txtFiltroCadastro";
            this.txtFiltroCadastro.Size = new Size(100, 23);
            this.txtFiltroCadastro.TabIndex = 0;
            this.txtFiltroCadastro.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFiltroCadastro_KeyDown);
            //
            // btnBuscarCadastro
            //
            this.btnBuscarCadastro.Location = new Point(103, 3);
            this.btnBuscarCadastro.Name = "btnBuscarCadastro";
            this.btnBuscarCadastro.Size = new Size(55, 25);
            this.btnBuscarCadastro.TabIndex = 1;
            this.btnBuscarCadastro.Text = "Search";
            this.btnBuscarCadastro.Click += new System.EventHandler(this.btnBuscarCadastro_Click);
            //
            // btnRefreshCadastros
            //
            this.btnRefreshCadastros = new Button();
            this.btnRefreshCadastros.Location = new Point(161, 3);
            this.btnRefreshCadastros.Name = "btnRefreshCadastros";
            this.btnRefreshCadastros.Size = new Size(25, 25);
            this.btnRefreshCadastros.TabIndex = 2;
            this.btnRefreshCadastros.Text = "\uD83D\uDD04";
            this.btnRefreshCadastros.Click += new System.EventHandler(this.btnRefreshCadastros_Click);
            //
            // btnNovoCadastro
            //
            this.btnNovoCadastro.BackColor = Color.FromArgb(40, 167, 69);
            this.btnNovoCadastro.FlatStyle = FlatStyle.Flat;
            this.btnNovoCadastro.ForeColor = Color.White;
            this.btnNovoCadastro.Location = new Point(190, 3);
            this.btnNovoCadastro.Name = "btnNovoCadastro";
            this.btnNovoCadastro.Size = new Size(60, 25);
            this.btnNovoCadastro.TabIndex = 3;
            this.btnNovoCadastro.Text = "+ New";
            this.btnNovoCadastro.UseVisualStyleBackColor = false;
            this.btnNovoCadastro.Click += new System.EventHandler(this.btnNovoCadastro_Click);
            //
            // btnExcluirCadastro
            //
            this.btnExcluirCadastro.BackColor = Color.FromArgb(220, 53, 69);
            this.btnExcluirCadastro.FlatStyle = FlatStyle.Flat;
            this.btnExcluirCadastro.ForeColor = Color.White;
            this.btnExcluirCadastro.Location = new Point(253, 3);
            this.btnExcluirCadastro.Name = "btnExcluirCadastro";
            this.btnExcluirCadastro.Size = new Size(60, 25);
            this.btnExcluirCadastro.TabIndex = 3;
            this.btnExcluirCadastro.Text = "Delete";
            this.btnExcluirCadastro.UseVisualStyleBackColor = false;
            this.btnExcluirCadastro.Click += new System.EventHandler(this.btnExcluirCadastro_Click);
            //
            // panelPaginacao - Botoes de paginacao (<< Anterior | 1/5 | Proxima >>)
            //
            this.panelPaginacao.Controls.Add(this.btnAnterior);
            this.panelPaginacao.Controls.Add(this.lblPagina);
            this.panelPaginacao.Controls.Add(this.btnProxima);
            this.panelPaginacao.Dock = DockStyle.Top;
            this.panelPaginacao.Location = new Point(5, 77);
            this.panelPaginacao.Name = "panelPaginacao";
            this.panelPaginacao.Size = new Size(340, 26);
            this.panelPaginacao.TabIndex = 3;
            //
            // btnAnterior
            //
            this.btnAnterior.Enabled = false;
            this.btnAnterior.Font = new Font("Segoe UI", 8F);
            this.btnAnterior.Location = new Point(0, 1);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new Size(65, 23);
            this.btnAnterior.TabIndex = 0;
            this.btnAnterior.Text = "<< Prev";
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            //
            // lblPagina
            //
            this.lblPagina.Font = new Font("Segoe UI", 8F);
            this.lblPagina.Location = new Point(68, 1);
            this.lblPagina.Name = "lblPagina";
            this.lblPagina.Size = new Size(50, 23);
            this.lblPagina.TabIndex = 1;
            this.lblPagina.Text = "0/0";
            this.lblPagina.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnProxima
            //
            this.btnProxima.Enabled = false;
            this.btnProxima.Font = new Font("Segoe UI", 8F);
            this.btnProxima.Location = new Point(121, 1);
            this.btnProxima.Name = "btnProxima";
            this.btnProxima.Size = new Size(65, 23);
            this.btnProxima.TabIndex = 2;
            this.btnProxima.Text = "Next >>";
            this.btnProxima.Click += new System.EventHandler(this.btnProxima_Click);
            //
            // listCadastros
            //
            this.listCadastros.Columns.AddRange(new ColumnHeader[] { this.colCadId, this.colCadNome, this.colCadAtivo, this.colCadContagem });
            this.listCadastros.Dock = DockStyle.Fill;
            this.listCadastros.FullRowSelect = true;
            this.listCadastros.GridLines = true;
            this.listCadastros.Location = new Point(5, 103);
            this.listCadastros.MultiSelect = false;
            this.listCadastros.Name = "listCadastros";
            this.listCadastros.Size = new Size(340, 392);
            this.listCadastros.TabIndex = 4;
            this.listCadastros.UseCompatibleStateImageBehavior = false;
            this.listCadastros.View = View.Details;
            this.listCadastros.SelectedIndexChanged += new System.EventHandler(this.listCadastros_SelectedIndexChanged);
            this.listCadastros.DoubleClick += new System.EventHandler(this.listCadastros_DoubleClick);
            //
            // colCadId
            //
            this.colCadId.Text = "ID";
            this.colCadId.Width = 50;
            //
            // colCadNome
            //
            this.colCadNome.Text = "Name";
            this.colCadNome.Width = 130;
            //
            // colCadAtivo
            //
            this.colCadAtivo.Text = "Active";
            this.colCadAtivo.Width = 45;
            //
            // colCadContagem
            //
            this.colCadContagem.Text = "Entities";
            this.colCadContagem.Width = 80;
            //
            // panelEntidades - Nivel 2: Entidades
            //
            this.panelEntidades.Controls.Add(this.listEntidades);
            this.panelEntidades.Controls.Add(this.panelEntidadesBotoes);
            this.panelEntidades.Controls.Add(this.lblStatusEntidades);
            this.panelEntidades.Controls.Add(this.lblEntidadesTitulo);
            this.panelEntidades.Dock = DockStyle.Fill;
            this.panelEntidades.Location = new Point(0, 0);
            this.panelEntidades.Name = "panelEntidades";
            this.panelEntidades.Padding = new Padding(5);
            this.panelEntidades.Size = new Size(346, 500);
            this.panelEntidades.TabIndex = 0;
            //
            // lblEntidadesTitulo
            //
            this.lblEntidadesTitulo.Dock = DockStyle.Top;
            this.lblEntidadesTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblEntidadesTitulo.Location = new Point(5, 5);
            this.lblEntidadesTitulo.Name = "lblEntidadesTitulo";
            this.lblEntidadesTitulo.Size = new Size(336, 22);
            this.lblEntidadesTitulo.TabIndex = 0;
            this.lblEntidadesTitulo.Text = "2. Entities (select a registry)";
            //
            // lblStatusEntidades
            //
            this.lblStatusEntidades.Dock = DockStyle.Top;
            this.lblStatusEntidades.ForeColor = Color.Gray;
            this.lblStatusEntidades.Location = new Point(5, 27);
            this.lblStatusEntidades.Name = "lblStatusEntidades";
            this.lblStatusEntidades.Size = new Size(336, 18);
            this.lblStatusEntidades.TabIndex = 1;
            //
            // panelEntidadesBotoes
            //
            this.panelEntidadesBotoes.Controls.Add(this.btnRefreshEntidades);
            this.panelEntidadesBotoes.Controls.Add(this.btnNovaEntidade);
            this.panelEntidadesBotoes.Controls.Add(this.btnExcluirEntidade);
            this.panelEntidadesBotoes.Dock = DockStyle.Top;
            this.panelEntidadesBotoes.Location = new Point(5, 45);
            this.panelEntidadesBotoes.Name = "panelEntidadesBotoes";
            this.panelEntidadesBotoes.Size = new Size(336, 32);
            this.panelEntidadesBotoes.TabIndex = 2;
            //
            // btnRefreshEntidades
            //
            this.btnRefreshEntidades = new Button();
            this.btnRefreshEntidades.Location = new Point(0, 3);
            this.btnRefreshEntidades.Name = "btnRefreshEntidades";
            this.btnRefreshEntidades.Size = new Size(25, 25);
            this.btnRefreshEntidades.TabIndex = 0;
            this.btnRefreshEntidades.Text = "\uD83D\uDD04";
            this.btnRefreshEntidades.Click += new System.EventHandler(this.btnRefreshEntidades_Click);
            //
            // btnNovaEntidade
            //
            this.btnNovaEntidade.BackColor = Color.FromArgb(40, 167, 69);
            this.btnNovaEntidade.FlatStyle = FlatStyle.Flat;
            this.btnNovaEntidade.ForeColor = Color.White;
            this.btnNovaEntidade.Location = new Point(28, 3);
            this.btnNovaEntidade.Name = "btnNovaEntidade";
            this.btnNovaEntidade.Size = new Size(100, 25);
            this.btnNovaEntidade.TabIndex = 1;
            this.btnNovaEntidade.Text = "+ Person/Veh.";
            this.btnNovaEntidade.UseVisualStyleBackColor = false;
            this.btnNovaEntidade.Click += new System.EventHandler(this.btnNovaEntidade_Click);
            //
            // btnExcluirEntidade
            //
            this.btnExcluirEntidade.BackColor = Color.FromArgb(220, 53, 69);
            this.btnExcluirEntidade.FlatStyle = FlatStyle.Flat;
            this.btnExcluirEntidade.ForeColor = Color.White;
            this.btnExcluirEntidade.Location = new Point(131, 3);
            this.btnExcluirEntidade.Name = "btnExcluirEntidade";
            this.btnExcluirEntidade.Size = new Size(70, 25);
            this.btnExcluirEntidade.TabIndex = 1;
            this.btnExcluirEntidade.Text = "Delete";
            this.btnExcluirEntidade.UseVisualStyleBackColor = false;
            this.btnExcluirEntidade.Click += new System.EventHandler(this.btnExcluirEntidade_Click);
            //
            // listEntidades
            //
            this.listEntidades.Columns.AddRange(new ColumnHeader[] { this.colEntId, this.colEntTipo, this.colEntNome, this.colEntDoc, this.colEntAtivo, this.colEntLpr });
            this.listEntidades.Dock = DockStyle.Fill;
            this.listEntidades.FullRowSelect = true;
            this.listEntidades.GridLines = true;
            this.listEntidades.Location = new Point(5, 77);
            this.listEntidades.MultiSelect = false;
            this.listEntidades.Name = "listEntidades";
            this.listEntidades.Size = new Size(336, 418);
            this.listEntidades.TabIndex = 3;
            this.listEntidades.UseCompatibleStateImageBehavior = false;
            this.listEntidades.View = View.Details;
            this.listEntidades.SelectedIndexChanged += new System.EventHandler(this.listEntidades_SelectedIndexChanged);
            this.listEntidades.DoubleClick += new System.EventHandler(this.listEntidades_DoubleClick);
            //
            // colEntId
            //
            this.colEntId.Text = "ID";
            this.colEntId.Width = 50;
            //
            // colEntTipo
            //
            this.colEntTipo.Text = "Type";
            this.colEntTipo.Width = 60;
            //
            // colEntNome
            //
            this.colEntNome.Text = "Description";
            this.colEntNome.Width = 100;
            //
            // colEntDoc
            //
            this.colEntDoc.Text = "Doc";
            this.colEntDoc.Width = 70;
            //
            // colEntAtivo
            //
            this.colEntAtivo.Text = "Active";
            this.colEntAtivo.Width = 45;
            //
            // colEntLpr
            //
            this.colEntLpr.Text = "LPR";
            this.colEntLpr.Width = 35;
            //
            // panelMidias - Nivel 3: Midias
            //
            this.panelMidias.Controls.Add(this.listMidias);
            this.panelMidias.Controls.Add(this.panelMidiasBotoes);
            this.panelMidias.Controls.Add(this.lblStatusMidias);
            this.panelMidias.Controls.Add(this.lblMidiasTitulo);
            this.panelMidias.Dock = DockStyle.Fill;
            this.panelMidias.Location = new Point(0, 0);
            this.panelMidias.Name = "panelMidias";
            this.panelMidias.Padding = new Padding(5);
            this.panelMidias.Size = new Size(396, 500);
            this.panelMidias.TabIndex = 0;
            //
            // lblMidiasTitulo
            //
            this.lblMidiasTitulo.Dock = DockStyle.Top;
            this.lblMidiasTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblMidiasTitulo.Location = new Point(5, 5);
            this.lblMidiasTitulo.Name = "lblMidiasTitulo";
            this.lblMidiasTitulo.Size = new Size(386, 22);
            this.lblMidiasTitulo.TabIndex = 0;
            this.lblMidiasTitulo.Text = "3. Media (select an entity)";
            //
            // lblStatusMidias
            //
            this.lblStatusMidias.Dock = DockStyle.Top;
            this.lblStatusMidias.ForeColor = Color.Gray;
            this.lblStatusMidias.Location = new Point(5, 27);
            this.lblStatusMidias.Name = "lblStatusMidias";
            this.lblStatusMidias.Size = new Size(386, 18);
            this.lblStatusMidias.TabIndex = 1;
            //
            // panelMidiasBotoes
            //
            this.panelMidiasBotoes.Controls.Add(this.btnRefreshMidias);
            this.panelMidiasBotoes.Controls.Add(this.btnNovaMidia);
            this.panelMidiasBotoes.Controls.Add(this.btnExcluirMidia);
            this.panelMidiasBotoes.Dock = DockStyle.Top;
            this.panelMidiasBotoes.Location = new Point(5, 45);
            this.panelMidiasBotoes.Name = "panelMidiasBotoes";
            this.panelMidiasBotoes.Size = new Size(386, 32);
            this.panelMidiasBotoes.TabIndex = 2;
            //
            // btnRefreshMidias
            //
            this.btnRefreshMidias = new Button();
            this.btnRefreshMidias.Location = new Point(0, 3);
            this.btnRefreshMidias.Name = "btnRefreshMidias";
            this.btnRefreshMidias.Size = new Size(25, 25);
            this.btnRefreshMidias.TabIndex = 0;
            this.btnRefreshMidias.Text = "\uD83D\uDD04";
            this.btnRefreshMidias.Click += new System.EventHandler(this.btnRefreshMidias_Click);
            //
            // btnNovaMidia
            //
            this.btnNovaMidia.BackColor = Color.FromArgb(40, 167, 69);
            this.btnNovaMidia.FlatStyle = FlatStyle.Flat;
            this.btnNovaMidia.ForeColor = Color.White;
            this.btnNovaMidia.Location = new Point(28, 3);
            this.btnNovaMidia.Name = "btnNovaMidia";
            this.btnNovaMidia.Size = new Size(90, 25);
            this.btnNovaMidia.TabIndex = 1;
            this.btnNovaMidia.Text = "+ Media";
            this.btnNovaMidia.UseVisualStyleBackColor = false;
            this.btnNovaMidia.Click += new System.EventHandler(this.btnNovaMidia_Click);
            //
            // btnExcluirMidia
            //
            this.btnExcluirMidia.BackColor = Color.FromArgb(220, 53, 69);
            this.btnExcluirMidia.FlatStyle = FlatStyle.Flat;
            this.btnExcluirMidia.ForeColor = Color.White;
            this.btnExcluirMidia.Location = new Point(121, 3);
            this.btnExcluirMidia.Name = "btnExcluirMidia";
            this.btnExcluirMidia.Size = new Size(70, 25);
            this.btnExcluirMidia.TabIndex = 1;
            this.btnExcluirMidia.Text = "Delete";
            this.btnExcluirMidia.UseVisualStyleBackColor = false;
            this.btnExcluirMidia.Click += new System.EventHandler(this.btnExcluirMidia_Click);
            //
            // listMidias
            //
            this.listMidias.Columns.AddRange(new ColumnHeader[] { this.colMidId, this.colMidTipo, this.colMidDesc, this.colMidAtivo });
            this.listMidias.Dock = DockStyle.Fill;
            this.listMidias.FullRowSelect = true;
            this.listMidias.GridLines = true;
            this.listMidias.Location = new Point(5, 77);
            this.listMidias.MultiSelect = false;
            this.listMidias.Name = "listMidias";
            this.listMidias.Size = new Size(386, 418);
            this.listMidias.TabIndex = 3;
            this.listMidias.UseCompatibleStateImageBehavior = false;
            this.listMidias.View = View.Details;
            this.listMidias.DoubleClick += new System.EventHandler(this.listMidias_DoubleClick);
            //
            // colMidId
            //
            this.colMidId.Text = "ID";
            this.colMidId.Width = 60;
            //
            // colMidTipo
            //
            this.colMidTipo.Text = "Type";
            this.colMidTipo.Width = 100;
            //
            // colMidDesc
            //
            this.colMidDesc.Text = "Description";
            this.colMidDesc.Width = 120;
            //
            // colMidAtivo
            //
            this.colMidAtivo.Text = "Active";
            this.colMidAtivo.Width = 45;
            //
            // txtLog
            //
            this.txtLog.BackColor = Color.FromArgb(30, 30, 30);
            this.txtLog.Dock = DockStyle.Bottom;
            this.txtLog.Font = new Font("Consolas", 8.25F);
            this.txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            this.txtLog.Location = new Point(0, 500);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Size = new Size(1100, 61);
            this.txtLog.TabIndex = 1;
            //
            // FormCadastroCompleto
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1100, 561);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.txtLog);
            this.MinimumSize = new Size(900, 500);
            this.Name = "FormCadastroCompleto";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Full Registry - MobiCortex Model (Registry \u2192 Entity \u2192 Media)";
            this.Load += new System.EventHandler(this.FormCadastroCompleto_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitEsquerda.Panel1.ResumeLayout(false);
            this.splitEsquerda.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitEsquerda)).EndInit();
            this.splitEsquerda.ResumeLayout(false);
            this.panelCadastros.ResumeLayout(false);
            this.panelPaginacao.ResumeLayout(false);
            this.panelEntidades.ResumeLayout(false);
            this.panelMidias.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private SplitContainer splitMain;
        private SplitContainer splitEsquerda;
        private Panel panelCadastros;
        private Label lblCadastrosTitulo;
        private Label lblStatusCadastros;
        private FlowLayoutPanel panelCadastrosBotoes;
        private TextBox txtFiltroCadastro;
        private Button btnBuscarCadastro;
        private Button btnNovoCadastro;
        private Button btnExcluirCadastro;
        private FlowLayoutPanel panelPaginacao;
        private Button btnAnterior;
        private Label lblPagina;
        private Button btnProxima;
        private ListView listCadastros;
        private ColumnHeader colCadId;
        private ColumnHeader colCadNome;
        private ColumnHeader colCadAtivo;
        private ColumnHeader colCadContagem;
        private Panel panelEntidades;
        private Label lblEntidadesTitulo;
        private Label lblStatusEntidades;
        private FlowLayoutPanel panelEntidadesBotoes;
        private Button btnNovaEntidade;
        private Button btnExcluirEntidade;
        private ListView listEntidades;
        private ColumnHeader colEntId;
        private ColumnHeader colEntTipo;
        private ColumnHeader colEntNome;
        private ColumnHeader colEntDoc;
        private ColumnHeader colEntAtivo;
        private ColumnHeader colEntLpr;
        private Panel panelMidias;
        private Label lblMidiasTitulo;
        private Label lblStatusMidias;
        private FlowLayoutPanel panelMidiasBotoes;
        private Button btnNovaMidia;
        private Button btnExcluirMidia;
        private Button btnRefreshCadastros;
        private Button btnRefreshEntidades;
        private Button btnRefreshMidias;
        private ListView listMidias;
        private ColumnHeader colMidId;
        private ColumnHeader colMidTipo;
        private ColumnHeader colMidDesc;
        private ColumnHeader colMidAtivo;
        private TextBox txtLog;
    }
}
