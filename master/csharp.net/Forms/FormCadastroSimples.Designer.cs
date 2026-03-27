namespace SmartSdk
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
            this.lblExplicacao = new Label();
            this.splitMain = new SplitContainer();
            this.panelEntidades = new Panel();
            this.listEntidades = new ListView();
            this.colEntId = new ColumnHeader();
            this.colEntTipo = new ColumnHeader();
            this.colEntNome = new ColumnHeader();
            this.colEntDoc = new ColumnHeader();
            this.colEntAtivo = new ColumnHeader();
            this.colEntCadId = new ColumnHeader();
            this.panelPaginacao = new FlowLayoutPanel();
            this.btnAnterior = new Button();
            this.lblPagina = new Label();
            this.btnProxima = new Button();
            this.panelEntBotoes = new FlowLayoutPanel();
            this.btnNovaEntidade = new Button();
            this.btnExcluirEntidade = new Button();
            this.txtFiltroEntidade = new TextBox();
            this.btnBuscarEntidade = new Button();
            this.lblStatusEntidades = new Label();
            this.lblEntidadesTitulo = new Label();
            this.panelMidias = new Panel();
            this.listMidias = new ListView();
            this.colMidId = new ColumnHeader();
            this.colMidTipo = new ColumnHeader();
            this.colMidDesc = new ColumnHeader();
            this.colMidHab = new ColumnHeader();
            this.panelMidBotoes = new FlowLayoutPanel();
            this.btnNovaMidia = new Button();
            this.btnExcluirMidia = new Button();
            this.lblStatusMidias = new Label();
            this.lblMidiasTitulo = new Label();
            this.txtLog = new TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.panelEntidades.SuspendLayout();
            this.panelPaginacao.SuspendLayout();
            this.panelEntBotoes.SuspendLayout();
            this.panelMidias.SuspendLayout();
            this.panelMidBotoes.SuspendLayout();
            this.SuspendLayout();
            //
            // lblExplicacao
            //
            this.lblExplicacao.BackColor = Color.FromArgb(0, 120, 60);
            this.lblExplicacao.Dock = DockStyle.Top;
            this.lblExplicacao.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblExplicacao.ForeColor = Color.White;
            this.lblExplicacao.Location = new Point(0, 0);
            this.lblExplicacao.Name = "lblExplicacao";
            this.lblExplicacao.Padding = new Padding(10, 5, 10, 5);
            this.lblExplicacao.Size = new Size(1050, 40);
            this.lblExplicacao.TabIndex = 2;
            this.lblExplicacao.Text = "SIMPLIFIED REGISTRY (2 levels): Create entities with createid=true — the controller generates IDs automatically. No need to create a central registry first.";
            this.lblExplicacao.TextAlign = ContentAlignment.MiddleLeft;
            //
            // splitMain
            //
            this.splitMain.Dock = DockStyle.Fill;
            this.splitMain.Location = new Point(0, 40);
            this.splitMain.Name = "splitMain";
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.panelEntidades);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.panelMidias);
            this.splitMain.Size = new Size(1050, 480);
            this.splitMain.SplitterDistance = 648;
            this.splitMain.TabIndex = 0;
            //
            // panelEntidades
            //
            this.panelEntidades.Controls.Add(this.listEntidades);
            this.panelEntidades.Controls.Add(this.panelPaginacao);
            this.panelEntidades.Controls.Add(this.panelEntBotoes);
            this.panelEntidades.Controls.Add(this.lblStatusEntidades);
            this.panelEntidades.Controls.Add(this.lblEntidadesTitulo);
            this.panelEntidades.Dock = DockStyle.Fill;
            this.panelEntidades.Location = new Point(0, 0);
            this.panelEntidades.Name = "panelEntidades";
            this.panelEntidades.Size = new Size(648, 480);
            this.panelEntidades.TabIndex = 0;
            //
            // listEntidades
            //
            this.listEntidades.Columns.AddRange(new ColumnHeader[] { this.colEntId, this.colEntTipo, this.colEntNome, this.colEntDoc, this.colEntAtivo, this.colEntCadId });
            this.listEntidades.Dock = DockStyle.Fill;
            this.listEntidades.FullRowSelect = true;
            this.listEntidades.GridLines = true;
            this.listEntidades.Location = new Point(0, 112);
            this.listEntidades.MultiSelect = false;
            this.listEntidades.Name = "listEntidades";
            this.listEntidades.Size = new Size(648, 368);
            this.listEntidades.TabIndex = 0;
            this.listEntidades.UseCompatibleStateImageBehavior = false;
            this.listEntidades.View = View.Details;
            this.listEntidades.SelectedIndexChanged += new System.EventHandler(this.listEntidades_SelectedIndexChanged);
            //
            // colEntId
            //
            this.colEntId.Text = "ID";
            this.colEntId.Width = 90;
            //
            // colEntTipo
            //
            this.colEntTipo.Text = "Type";
            //
            // colEntNome
            //
            this.colEntNome.Text = "Description";
            this.colEntNome.Width = 150;
            //
            // colEntDoc
            //
            this.colEntDoc.Text = "Doc/Plate";
            this.colEntDoc.Width = 90;
            //
            // colEntAtivo
            //
            this.colEntAtivo.Text = "Active";
            this.colEntAtivo.Width = 50;
            //
            // colEntCadId
            //
            this.colEntCadId.Text = "CadID";
            this.colEntCadId.Width = 70;
            //
            // panelPaginacao
            //
            this.panelPaginacao.Controls.Add(this.btnAnterior);
            this.panelPaginacao.Controls.Add(this.lblPagina);
            this.panelPaginacao.Controls.Add(this.btnProxima);
            this.panelPaginacao.Dock = DockStyle.Top;
            this.panelPaginacao.Location = new Point(0, 82);
            this.panelPaginacao.Name = "panelPaginacao";
            this.panelPaginacao.Size = new Size(648, 30);
            this.panelPaginacao.TabIndex = 1;
            this.panelPaginacao.WrapContents = false;
            //
            // btnAnterior
            //
            this.btnAnterior.AutoSize = true;
            this.btnAnterior.Enabled = false;
            this.btnAnterior.FlatStyle = FlatStyle.Flat;
            this.btnAnterior.Location = new Point(3, 3);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new Size(75, 27);
            this.btnAnterior.TabIndex = 0;
            this.btnAnterior.Text = "<< Prev";
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            //
            // lblPagina
            //
            this.lblPagina.AutoSize = true;
            this.lblPagina.Location = new Point(87, 6);
            this.lblPagina.Margin = new Padding(6, 6, 6, 0);
            this.lblPagina.Name = "lblPagina";
            this.lblPagina.Size = new Size(24, 15);
            this.lblPagina.TabIndex = 1;
            this.lblPagina.Text = "0/0";
            this.lblPagina.TextAlign = ContentAlignment.MiddleCenter;
            //
            // btnProxima
            //
            this.btnProxima.AutoSize = true;
            this.btnProxima.Enabled = false;
            this.btnProxima.FlatStyle = FlatStyle.Flat;
            this.btnProxima.Location = new Point(120, 3);
            this.btnProxima.Name = "btnProxima";
            this.btnProxima.Size = new Size(75, 27);
            this.btnProxima.TabIndex = 2;
            this.btnProxima.Text = "Next >>";
            this.btnProxima.Click += new System.EventHandler(this.btnProxima_Click);
            //
            // panelEntBotoes
            //
            this.panelEntBotoes.Controls.Add(this.btnNovaEntidade);
            this.panelEntBotoes.Controls.Add(this.btnExcluirEntidade);
            this.panelEntBotoes.Controls.Add(this.txtFiltroEntidade);
            this.panelEntBotoes.Controls.Add(this.btnBuscarEntidade);
            this.panelEntBotoes.Dock = DockStyle.Top;
            this.panelEntBotoes.Location = new Point(0, 48);
            this.panelEntBotoes.Name = "panelEntBotoes";
            this.panelEntBotoes.Size = new Size(648, 34);
            this.panelEntBotoes.TabIndex = 2;
            this.panelEntBotoes.WrapContents = false;
            //
            // btnNovaEntidade
            //
            this.btnNovaEntidade.AutoSize = true;
            this.btnNovaEntidade.BackColor = Color.FromArgb(0, 120, 60);
            this.btnNovaEntidade.Cursor = Cursors.Hand;
            this.btnNovaEntidade.FlatStyle = FlatStyle.Flat;
            this.btnNovaEntidade.ForeColor = Color.White;
            this.btnNovaEntidade.Location = new Point(3, 3);
            this.btnNovaEntidade.Name = "btnNovaEntidade";
            this.btnNovaEntidade.Size = new Size(76, 27);
            this.btnNovaEntidade.TabIndex = 0;
            this.btnNovaEntidade.Text = "+ Entity";
            this.btnNovaEntidade.UseVisualStyleBackColor = false;
            this.btnNovaEntidade.Click += new System.EventHandler(this.btnNovaEntidade_Click);
            //
            // btnExcluirEntidade
            //
            this.btnExcluirEntidade.AutoSize = true;
            this.btnExcluirEntidade.Cursor = Cursors.Hand;
            this.btnExcluirEntidade.FlatStyle = FlatStyle.Flat;
            this.btnExcluirEntidade.ForeColor = Color.Firebrick;
            this.btnExcluirEntidade.Location = new Point(85, 3);
            this.btnExcluirEntidade.Name = "btnExcluirEntidade";
            this.btnExcluirEntidade.Size = new Size(75, 27);
            this.btnExcluirEntidade.TabIndex = 1;
            this.btnExcluirEntidade.Text = "Delete";
            this.btnExcluirEntidade.Click += new System.EventHandler(this.btnExcluirEntidade_Click);
            //
            // txtFiltroEntidade
            //
            this.txtFiltroEntidade.Location = new Point(175, 4);
            this.txtFiltroEntidade.Margin = new Padding(12, 4, 0, 0);
            this.txtFiltroEntidade.Name = "txtFiltroEntidade";
            this.txtFiltroEntidade.Size = new Size(140, 23);
            this.txtFiltroEntidade.TabIndex = 2;
            this.txtFiltroEntidade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFiltroEntidade_KeyDown);
            //
            // btnBuscarEntidade
            //
            this.btnBuscarEntidade.AutoSize = true;
            this.btnBuscarEntidade.Cursor = Cursors.Hand;
            this.btnBuscarEntidade.FlatStyle = FlatStyle.Flat;
            this.btnBuscarEntidade.Location = new Point(318, 3);
            this.btnBuscarEntidade.Name = "btnBuscarEntidade";
            this.btnBuscarEntidade.Size = new Size(75, 27);
            this.btnBuscarEntidade.TabIndex = 3;
            this.btnBuscarEntidade.Text = "Search";
            this.btnBuscarEntidade.Click += new System.EventHandler(this.btnBuscarEntidade_Click);
            //
            // lblStatusEntidades
            //
            this.lblStatusEntidades.Dock = DockStyle.Top;
            this.lblStatusEntidades.Font = new Font("Segoe UI", 8F);
            this.lblStatusEntidades.ForeColor = Color.Gray;
            this.lblStatusEntidades.Location = new Point(0, 28);
            this.lblStatusEntidades.Name = "lblStatusEntidades";
            this.lblStatusEntidades.Size = new Size(648, 20);
            this.lblStatusEntidades.TabIndex = 3;
            this.lblStatusEntidades.Text = "Loading...";
            //
            // lblEntidadesTitulo
            //
            this.lblEntidadesTitulo.Dock = DockStyle.Top;
            this.lblEntidadesTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblEntidadesTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            this.lblEntidadesTitulo.Location = new Point(0, 0);
            this.lblEntidadesTitulo.Name = "lblEntidadesTitulo";
            this.lblEntidadesTitulo.Size = new Size(648, 28);
            this.lblEntidadesTitulo.TabIndex = 4;
            this.lblEntidadesTitulo.Text = "Entities";
            this.lblEntidadesTitulo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // panelMidias
            //
            this.panelMidias.Controls.Add(this.listMidias);
            this.panelMidias.Controls.Add(this.panelMidBotoes);
            this.panelMidias.Controls.Add(this.lblStatusMidias);
            this.panelMidias.Controls.Add(this.lblMidiasTitulo);
            this.panelMidias.Dock = DockStyle.Fill;
            this.panelMidias.Location = new Point(0, 0);
            this.panelMidias.Name = "panelMidias";
            this.panelMidias.Size = new Size(398, 480);
            this.panelMidias.TabIndex = 0;
            //
            // listMidias
            //
            this.listMidias.Columns.AddRange(new ColumnHeader[] { this.colMidId, this.colMidTipo, this.colMidDesc, this.colMidHab });
            this.listMidias.Dock = DockStyle.Fill;
            this.listMidias.FullRowSelect = true;
            this.listMidias.GridLines = true;
            this.listMidias.Location = new Point(0, 82);
            this.listMidias.MultiSelect = false;
            this.listMidias.Name = "listMidias";
            this.listMidias.Size = new Size(398, 398);
            this.listMidias.TabIndex = 0;
            this.listMidias.UseCompatibleStateImageBehavior = false;
            this.listMidias.View = View.Details;
            this.listMidias.DoubleClick += new System.EventHandler(this.listMidias_DoubleClick);
            //
            // colMidId
            //
            this.colMidId.Text = "ID";
            this.colMidId.Width = 90;
            //
            // colMidTipo
            //
            this.colMidTipo.Text = "Type";
            this.colMidTipo.Width = 80;
            //
            // colMidDesc
            //
            this.colMidDesc.Text = "Description";
            this.colMidDesc.Width = 150;
            //
            // colMidHab
            //
            this.colMidHab.Text = "Ena";
            this.colMidHab.Width = 40;
            //
            // panelMidBotoes
            //
            this.panelMidBotoes.Controls.Add(this.btnNovaMidia);
            this.panelMidBotoes.Controls.Add(this.btnExcluirMidia);
            this.panelMidBotoes.Dock = DockStyle.Top;
            this.panelMidBotoes.Location = new Point(0, 48);
            this.panelMidBotoes.Name = "panelMidBotoes";
            this.panelMidBotoes.Size = new Size(398, 34);
            this.panelMidBotoes.TabIndex = 1;
            this.panelMidBotoes.WrapContents = false;
            //
            // btnNovaMidia
            //
            this.btnNovaMidia.AutoSize = true;
            this.btnNovaMidia.BackColor = Color.FromArgb(0, 120, 60);
            this.btnNovaMidia.Cursor = Cursors.Hand;
            this.btnNovaMidia.FlatStyle = FlatStyle.Flat;
            this.btnNovaMidia.ForeColor = Color.White;
            this.btnNovaMidia.Location = new Point(3, 3);
            this.btnNovaMidia.Name = "btnNovaMidia";
            this.btnNovaMidia.Size = new Size(75, 27);
            this.btnNovaMidia.TabIndex = 0;
            this.btnNovaMidia.Text = "+ Media";
            this.btnNovaMidia.UseVisualStyleBackColor = false;
            this.btnNovaMidia.Click += new System.EventHandler(this.btnNovaMidia_Click);
            //
            // btnExcluirMidia
            //
            this.btnExcluirMidia.AutoSize = true;
            this.btnExcluirMidia.Cursor = Cursors.Hand;
            this.btnExcluirMidia.FlatStyle = FlatStyle.Flat;
            this.btnExcluirMidia.ForeColor = Color.Firebrick;
            this.btnExcluirMidia.Location = new Point(84, 3);
            this.btnExcluirMidia.Name = "btnExcluirMidia";
            this.btnExcluirMidia.Size = new Size(75, 27);
            this.btnExcluirMidia.TabIndex = 1;
            this.btnExcluirMidia.Text = "Delete";
            this.btnExcluirMidia.Click += new System.EventHandler(this.btnExcluirMidia_Click);
            //
            // lblStatusMidias
            //
            this.lblStatusMidias.Dock = DockStyle.Top;
            this.lblStatusMidias.Font = new Font("Segoe UI", 8F);
            this.lblStatusMidias.ForeColor = Color.Gray;
            this.lblStatusMidias.Location = new Point(0, 28);
            this.lblStatusMidias.Name = "lblStatusMidias";
            this.lblStatusMidias.Size = new Size(398, 20);
            this.lblStatusMidias.TabIndex = 2;
            this.lblStatusMidias.Text = "Select an entity";
            //
            // lblMidiasTitulo
            //
            this.lblMidiasTitulo.Dock = DockStyle.Top;
            this.lblMidiasTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblMidiasTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            this.lblMidiasTitulo.Location = new Point(0, 0);
            this.lblMidiasTitulo.Name = "lblMidiasTitulo";
            this.lblMidiasTitulo.Size = new Size(398, 28);
            this.lblMidiasTitulo.TabIndex = 3;
            this.lblMidiasTitulo.Text = "Media";
            this.lblMidiasTitulo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // txtLog
            //
            this.txtLog.BackColor = Color.FromArgb(30, 30, 30);
            this.txtLog.Dock = DockStyle.Bottom;
            this.txtLog.Font = new Font("Consolas", 8.5F);
            this.txtLog.ForeColor = Color.LightGreen;
            this.txtLog.Location = new Point(0, 520);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Size = new Size(1050, 100);
            this.txtLog.TabIndex = 1;
            //
            // FormCadastroSimples
            //
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1050, 620);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblExplicacao);
            this.Name = "FormCadastroSimples";
            this.Text = "Simplified Registry (Entity → Media) — createid=true";
            this.Load += new System.EventHandler(this.FormCadastroSimples_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.panelEntidades.ResumeLayout(false);
            this.panelPaginacao.ResumeLayout(false);
            this.panelPaginacao.PerformLayout();
            this.panelEntBotoes.ResumeLayout(false);
            this.panelEntBotoes.PerformLayout();
            this.panelMidias.ResumeLayout(false);
            this.panelMidBotoes.ResumeLayout(false);
            this.panelMidBotoes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
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
        private ColumnHeader colEntAtivo;
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
