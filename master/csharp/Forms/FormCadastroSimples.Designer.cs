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
            lblExplicacao = new Label();
            splitMain = new SplitContainer();
            panelEntidades = new Panel();
            listEntidades = new ListView();
            colEntId = new ColumnHeader();
            colEntTipo = new ColumnHeader();
            colEntNome = new ColumnHeader();
            colEntDoc = new ColumnHeader();
            colEntCadId = new ColumnHeader();
            panelPaginacao = new FlowLayoutPanel();
            btnAnterior = new Button();
            lblPagina = new Label();
            btnProxima = new Button();
            panelEntBotoes = new FlowLayoutPanel();
            btnNovaEntidade = new Button();
            btnExcluirEntidade = new Button();
            txtFiltroEntidade = new TextBox();
            btnBuscarEntidade = new Button();
            lblStatusEntidades = new Label();
            lblEntidadesTitulo = new Label();
            panelMidias = new Panel();
            listMidias = new ListView();
            colMidId = new ColumnHeader();
            colMidTipo = new ColumnHeader();
            colMidDesc = new ColumnHeader();
            colMidHab = new ColumnHeader();
            panelMidBotoes = new FlowLayoutPanel();
            btnNovaMidia = new Button();
            btnExcluirMidia = new Button();
            lblStatusMidias = new Label();
            lblMidiasTitulo = new Label();
            txtLog = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            panelEntidades.SuspendLayout();
            panelPaginacao.SuspendLayout();
            panelEntBotoes.SuspendLayout();
            panelMidias.SuspendLayout();
            panelMidBotoes.SuspendLayout();
            SuspendLayout();
            // 
            // lblExplicacao
            // 
            lblExplicacao.BackColor = Color.FromArgb(0, 120, 60);
            lblExplicacao.Dock = DockStyle.Top;
            lblExplicacao.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblExplicacao.ForeColor = Color.White;
            lblExplicacao.Location = new Point(0, 0);
            lblExplicacao.Name = "lblExplicacao";
            lblExplicacao.Padding = new Padding(10, 5, 10, 5);
            lblExplicacao.Size = new Size(1050, 40);
            lblExplicacao.TabIndex = 2;
            lblExplicacao.Text = "CADASTRO SIMPLIFICADO (2 níveis): Crie entidades com createid=true — o controlador gera os IDs automaticamente. Não é necessário criar cadastro central antes.";
            lblExplicacao.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 40);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(panelEntidades);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(panelMidias);
            splitMain.Size = new Size(1050, 480);
            splitMain.SplitterDistance = 648;
            splitMain.TabIndex = 0;
            // 
            // panelEntidades
            // 
            panelEntidades.Controls.Add(listEntidades);
            panelEntidades.Controls.Add(panelPaginacao);
            panelEntidades.Controls.Add(panelEntBotoes);
            panelEntidades.Controls.Add(lblStatusEntidades);
            panelEntidades.Controls.Add(lblEntidadesTitulo);
            panelEntidades.Dock = DockStyle.Fill;
            panelEntidades.Location = new Point(0, 0);
            panelEntidades.Name = "panelEntidades";
            panelEntidades.Size = new Size(648, 480);
            panelEntidades.TabIndex = 0;
            // 
            // listEntidades
            // 
            listEntidades.Columns.AddRange(new ColumnHeader[] { colEntId, colEntTipo, colEntNome, colEntDoc, colEntCadId });
            listEntidades.Dock = DockStyle.Fill;
            listEntidades.FullRowSelect = true;
            listEntidades.GridLines = true;
            listEntidades.Location = new Point(0, 112);
            listEntidades.MultiSelect = false;
            listEntidades.Name = "listEntidades";
            listEntidades.Size = new Size(648, 368);
            listEntidades.TabIndex = 0;
            listEntidades.UseCompatibleStateImageBehavior = false;
            listEntidades.View = View.Details;
            listEntidades.SelectedIndexChanged += listEntidades_SelectedIndexChanged;
            // 
            // colEntId
            // 
            colEntId.Text = "ID";
            colEntId.Width = 90;
            // 
            // colEntTipo
            // 
            colEntTipo.Text = "Tipo";
            // 
            // colEntNome
            // 
            colEntNome.Text = "Nome";
            colEntNome.Width = 150;
            // 
            // colEntDoc
            // 
            colEntDoc.Text = "Doc/Placa";
            colEntDoc.Width = 90;
            // 
            // colEntCadId
            // 
            colEntCadId.Text = "CadID";
            colEntCadId.Width = 70;
            // 
            // panelPaginacao
            // 
            panelPaginacao.Controls.Add(btnAnterior);
            panelPaginacao.Controls.Add(lblPagina);
            panelPaginacao.Controls.Add(btnProxima);
            panelPaginacao.Dock = DockStyle.Top;
            panelPaginacao.Location = new Point(0, 82);
            panelPaginacao.Name = "panelPaginacao";
            panelPaginacao.Size = new Size(648, 30);
            panelPaginacao.TabIndex = 1;
            panelPaginacao.WrapContents = false;
            // 
            // btnAnterior
            // 
            btnAnterior.AutoSize = true;
            btnAnterior.Enabled = false;
            btnAnterior.FlatStyle = FlatStyle.Flat;
            btnAnterior.Location = new Point(3, 3);
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(75, 27);
            btnAnterior.TabIndex = 0;
            btnAnterior.Text = "<< Ant.";
            btnAnterior.Click += btnAnterior_Click;
            // 
            // lblPagina
            // 
            lblPagina.AutoSize = true;
            lblPagina.Location = new Point(87, 6);
            lblPagina.Margin = new Padding(6, 6, 6, 0);
            lblPagina.Name = "lblPagina";
            lblPagina.Size = new Size(24, 15);
            lblPagina.TabIndex = 1;
            lblPagina.Text = "0/0";
            lblPagina.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnProxima
            // 
            btnProxima.AutoSize = true;
            btnProxima.Enabled = false;
            btnProxima.FlatStyle = FlatStyle.Flat;
            btnProxima.Location = new Point(120, 3);
            btnProxima.Name = "btnProxima";
            btnProxima.Size = new Size(75, 27);
            btnProxima.TabIndex = 2;
            btnProxima.Text = "Próx. >>";
            btnProxima.Click += btnProxima_Click;
            // 
            // panelEntBotoes
            // 
            panelEntBotoes.Controls.Add(btnNovaEntidade);
            panelEntBotoes.Controls.Add(btnExcluirEntidade);
            panelEntBotoes.Controls.Add(txtFiltroEntidade);
            panelEntBotoes.Controls.Add(btnBuscarEntidade);
            panelEntBotoes.Dock = DockStyle.Top;
            panelEntBotoes.Location = new Point(0, 48);
            panelEntBotoes.Name = "panelEntBotoes";
            panelEntBotoes.Size = new Size(648, 34);
            panelEntBotoes.TabIndex = 2;
            panelEntBotoes.WrapContents = false;
            // 
            // btnNovaEntidade
            // 
            btnNovaEntidade.AutoSize = true;
            btnNovaEntidade.BackColor = Color.FromArgb(0, 120, 60);
            btnNovaEntidade.Cursor = Cursors.Hand;
            btnNovaEntidade.FlatStyle = FlatStyle.Flat;
            btnNovaEntidade.ForeColor = Color.White;
            btnNovaEntidade.Location = new Point(3, 3);
            btnNovaEntidade.Name = "btnNovaEntidade";
            btnNovaEntidade.Size = new Size(76, 27);
            btnNovaEntidade.TabIndex = 0;
            btnNovaEntidade.Text = "+ Entidade";
            btnNovaEntidade.UseVisualStyleBackColor = false;
            btnNovaEntidade.Click += btnNovaEntidade_Click;
            // 
            // btnExcluirEntidade
            // 
            btnExcluirEntidade.AutoSize = true;
            btnExcluirEntidade.Cursor = Cursors.Hand;
            btnExcluirEntidade.FlatStyle = FlatStyle.Flat;
            btnExcluirEntidade.ForeColor = Color.Firebrick;
            btnExcluirEntidade.Location = new Point(85, 3);
            btnExcluirEntidade.Name = "btnExcluirEntidade";
            btnExcluirEntidade.Size = new Size(75, 27);
            btnExcluirEntidade.TabIndex = 1;
            btnExcluirEntidade.Text = "Excluir";
            btnExcluirEntidade.Click += btnExcluirEntidade_Click;
            // 
            // txtFiltroEntidade
            // 
            txtFiltroEntidade.Location = new Point(175, 4);
            txtFiltroEntidade.Margin = new Padding(12, 4, 0, 0);
            txtFiltroEntidade.Name = "txtFiltroEntidade";
            txtFiltroEntidade.PlaceholderText = "ID ou nome...";
            txtFiltroEntidade.Size = new Size(140, 23);
            txtFiltroEntidade.TabIndex = 2;
            txtFiltroEntidade.KeyDown += txtFiltroEntidade_KeyDown;
            // 
            // btnBuscarEntidade
            // 
            btnBuscarEntidade.AutoSize = true;
            btnBuscarEntidade.Cursor = Cursors.Hand;
            btnBuscarEntidade.FlatStyle = FlatStyle.Flat;
            btnBuscarEntidade.Location = new Point(318, 3);
            btnBuscarEntidade.Name = "btnBuscarEntidade";
            btnBuscarEntidade.Size = new Size(75, 27);
            btnBuscarEntidade.TabIndex = 3;
            btnBuscarEntidade.Text = "Buscar";
            btnBuscarEntidade.Click += btnBuscarEntidade_Click;
            // 
            // lblStatusEntidades
            // 
            lblStatusEntidades.Dock = DockStyle.Top;
            lblStatusEntidades.Font = new Font("Segoe UI", 8F);
            lblStatusEntidades.ForeColor = Color.Gray;
            lblStatusEntidades.Location = new Point(0, 28);
            lblStatusEntidades.Name = "lblStatusEntidades";
            lblStatusEntidades.Size = new Size(648, 20);
            lblStatusEntidades.TabIndex = 3;
            lblStatusEntidades.Text = "Carregando...";
            // 
            // lblEntidadesTitulo
            // 
            lblEntidadesTitulo.Dock = DockStyle.Top;
            lblEntidadesTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEntidadesTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            lblEntidadesTitulo.Location = new Point(0, 0);
            lblEntidadesTitulo.Name = "lblEntidadesTitulo";
            lblEntidadesTitulo.Size = new Size(648, 28);
            lblEntidadesTitulo.TabIndex = 4;
            lblEntidadesTitulo.Text = "Entidades";
            lblEntidadesTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelMidias
            // 
            panelMidias.Controls.Add(listMidias);
            panelMidias.Controls.Add(panelMidBotoes);
            panelMidias.Controls.Add(lblStatusMidias);
            panelMidias.Controls.Add(lblMidiasTitulo);
            panelMidias.Dock = DockStyle.Fill;
            panelMidias.Location = new Point(0, 0);
            panelMidias.Name = "panelMidias";
            panelMidias.Size = new Size(398, 480);
            panelMidias.TabIndex = 0;
            // 
            // listMidias
            // 
            listMidias.Columns.AddRange(new ColumnHeader[] { colMidId, colMidTipo, colMidDesc, colMidHab });
            listMidias.Dock = DockStyle.Fill;
            listMidias.FullRowSelect = true;
            listMidias.GridLines = true;
            listMidias.Location = new Point(0, 82);
            listMidias.MultiSelect = false;
            listMidias.Name = "listMidias";
            listMidias.Size = new Size(398, 398);
            listMidias.TabIndex = 0;
            listMidias.UseCompatibleStateImageBehavior = false;
            listMidias.View = View.Details;
            // 
            // colMidId
            // 
            colMidId.Text = "ID";
            colMidId.Width = 90;
            // 
            // colMidTipo
            // 
            colMidTipo.Text = "Tipo";
            colMidTipo.Width = 80;
            // 
            // colMidDesc
            // 
            colMidDesc.Text = "Descrição";
            colMidDesc.Width = 150;
            // 
            // colMidHab
            // 
            colMidHab.Text = "Hab";
            colMidHab.Width = 40;
            // 
            // panelMidBotoes
            // 
            panelMidBotoes.Controls.Add(btnNovaMidia);
            panelMidBotoes.Controls.Add(btnExcluirMidia);
            panelMidBotoes.Dock = DockStyle.Top;
            panelMidBotoes.Location = new Point(0, 48);
            panelMidBotoes.Name = "panelMidBotoes";
            panelMidBotoes.Size = new Size(398, 34);
            panelMidBotoes.TabIndex = 1;
            panelMidBotoes.WrapContents = false;
            // 
            // btnNovaMidia
            // 
            btnNovaMidia.AutoSize = true;
            btnNovaMidia.BackColor = Color.FromArgb(0, 120, 60);
            btnNovaMidia.Cursor = Cursors.Hand;
            btnNovaMidia.FlatStyle = FlatStyle.Flat;
            btnNovaMidia.ForeColor = Color.White;
            btnNovaMidia.Location = new Point(3, 3);
            btnNovaMidia.Name = "btnNovaMidia";
            btnNovaMidia.Size = new Size(75, 27);
            btnNovaMidia.TabIndex = 0;
            btnNovaMidia.Text = "+ Mídia";
            btnNovaMidia.UseVisualStyleBackColor = false;
            btnNovaMidia.Click += btnNovaMidia_Click;
            // 
            // btnExcluirMidia
            // 
            btnExcluirMidia.AutoSize = true;
            btnExcluirMidia.Cursor = Cursors.Hand;
            btnExcluirMidia.FlatStyle = FlatStyle.Flat;
            btnExcluirMidia.ForeColor = Color.Firebrick;
            btnExcluirMidia.Location = new Point(84, 3);
            btnExcluirMidia.Name = "btnExcluirMidia";
            btnExcluirMidia.Size = new Size(75, 27);
            btnExcluirMidia.TabIndex = 1;
            btnExcluirMidia.Text = "Excluir";
            btnExcluirMidia.Click += btnExcluirMidia_Click;
            // 
            // lblStatusMidias
            // 
            lblStatusMidias.Dock = DockStyle.Top;
            lblStatusMidias.Font = new Font("Segoe UI", 8F);
            lblStatusMidias.ForeColor = Color.Gray;
            lblStatusMidias.Location = new Point(0, 28);
            lblStatusMidias.Name = "lblStatusMidias";
            lblStatusMidias.Size = new Size(398, 20);
            lblStatusMidias.TabIndex = 2;
            lblStatusMidias.Text = "Selecione uma entidade";
            // 
            // lblMidiasTitulo
            // 
            lblMidiasTitulo.Dock = DockStyle.Top;
            lblMidiasTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMidiasTitulo.ForeColor = Color.FromArgb(0, 120, 60);
            lblMidiasTitulo.Location = new Point(0, 0);
            lblMidiasTitulo.Name = "lblMidiasTitulo";
            lblMidiasTitulo.Size = new Size(398, 28);
            lblMidiasTitulo.TabIndex = 3;
            lblMidiasTitulo.Text = "Mídias";
            lblMidiasTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.Dock = DockStyle.Bottom;
            txtLog.Font = new Font("Consolas", 8.5F);
            txtLog.ForeColor = Color.LightGreen;
            txtLog.Location = new Point(0, 520);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(1050, 100);
            txtLog.TabIndex = 1;
            // 
            // FormCadastroSimples
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1050, 620);
            Controls.Add(splitMain);
            Controls.Add(txtLog);
            Controls.Add(lblExplicacao);
            Name = "FormCadastroSimples";
            Text = "Cadastro Simplificado (Entidade → Mídia) — createid=true";
            Load += FormCadastroSimples_Load;
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            panelEntidades.ResumeLayout(false);
            panelPaginacao.ResumeLayout(false);
            panelPaginacao.PerformLayout();
            panelEntBotoes.ResumeLayout(false);
            panelEntBotoes.PerformLayout();
            panelMidias.ResumeLayout(false);
            panelMidBotoes.ResumeLayout(false);
            panelMidBotoes.PerformLayout();
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
