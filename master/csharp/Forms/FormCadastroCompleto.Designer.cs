namespace SmartSdk.Forms
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
            splitMain = new SplitContainer();
            splitEsquerda = new SplitContainer();
            panelCadastros = new Panel();
            listCadastros = new ListView();
            colCadId = new ColumnHeader();
            colCadNome = new ColumnHeader();
            colCadAtivo = new ColumnHeader();
            colCadContagem = new ColumnHeader();
            panelPaginacao = new FlowLayoutPanel();
            btnAnterior = new Button();
            lblPagina = new Label();
            btnProxima = new Button();
            panelCadastrosBotoes = new FlowLayoutPanel();
            btnNovoCadastro = new Button();
            btnExcluirCadastro = new Button();
            btnBuscarCadastro = new Button();
            txtFiltroCadastro = new TextBox();
            lblCadastrosTitulo = new Label();
            lblStatusCadastros = new Label();
            panelEntidades = new Panel();
            listEntidades = new ListView();
            colEntId = new ColumnHeader();
            colEntTipo = new ColumnHeader();
            colEntNome = new ColumnHeader();
            colEntDoc = new ColumnHeader();
            colEntLpr = new ColumnHeader();
            panelEntidadesBotoes = new FlowLayoutPanel();
            btnNovaEntidade = new Button();
            btnExcluirEntidade = new Button();
            lblEntidadesTitulo = new Label();
            lblStatusEntidades = new Label();
            panelMidias = new Panel();
            listMidias = new ListView();
            colMidId = new ColumnHeader();
            colMidTipo = new ColumnHeader();
            colMidDesc = new ColumnHeader();
            colMidAtivo = new ColumnHeader();
            panelMidiasBotoes = new FlowLayoutPanel();
            btnNovaMidia = new Button();
            btnExcluirMidia = new Button();
            lblMidiasTitulo = new Label();
            lblStatusMidias = new Label();
            txtLog = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitEsquerda).BeginInit();
            splitEsquerda.Panel1.SuspendLayout();
            splitEsquerda.Panel2.SuspendLayout();
            splitEsquerda.SuspendLayout();
            panelCadastros.SuspendLayout();
            panelPaginacao.SuspendLayout();
            panelEntidades.SuspendLayout();
            panelMidias.SuspendLayout();
            SuspendLayout();
            // 
            // splitMain - Divide: [Cadastros+Entidades | Mídias]
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            splitMain.Panel1.Controls.Add(splitEsquerda);
            splitMain.Panel2.Controls.Add(panelMidias);
            splitMain.Size = new Size(1100, 500);
            splitMain.SplitterDistance = 700;
            splitMain.TabIndex = 0;
            // 
            // splitEsquerda - Divide: [Cadastros | Entidades]
            // 
            splitEsquerda.Dock = DockStyle.Fill;
            splitEsquerda.Location = new Point(0, 0);
            splitEsquerda.Name = "splitEsquerda";
            splitEsquerda.Panel1.Controls.Add(panelCadastros);
            splitEsquerda.Panel2.Controls.Add(panelEntidades);
            splitEsquerda.Size = new Size(700, 500);
            splitEsquerda.SplitterDistance = 350;
            splitEsquerda.TabIndex = 0;
            // 
            // panelCadastros - Nível 1: Cadastros Centrais
            // 
            panelCadastros.Controls.Add(listCadastros);
            panelCadastros.Controls.Add(panelPaginacao);
            panelCadastros.Controls.Add(panelCadastrosBotoes);
            panelCadastros.Controls.Add(lblStatusCadastros);
            panelCadastros.Controls.Add(lblCadastrosTitulo);
            panelCadastros.Dock = DockStyle.Fill;
            panelCadastros.Location = new Point(0, 0);
            panelCadastros.Name = "panelCadastros";
            panelCadastros.Padding = new Padding(5);
            panelCadastros.Size = new Size(350, 500);
            panelCadastros.TabIndex = 0;
            // 
            // lblCadastrosTitulo
            // 
            lblCadastrosTitulo.Dock = DockStyle.Top;
            lblCadastrosTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCadastrosTitulo.Location = new Point(5, 5);
            lblCadastrosTitulo.Name = "lblCadastrosTitulo";
            lblCadastrosTitulo.Size = new Size(340, 22);
            lblCadastrosTitulo.TabIndex = 0;
            lblCadastrosTitulo.Text = "1. Cadastros Centrais";
            // 
            // lblStatusCadastros
            // 
            lblStatusCadastros.Dock = DockStyle.Top;
            lblStatusCadastros.ForeColor = Color.Gray;
            lblStatusCadastros.Location = new Point(5, 27);
            lblStatusCadastros.Name = "lblStatusCadastros";
            lblStatusCadastros.Size = new Size(340, 18);
            lblStatusCadastros.TabIndex = 1;
            lblStatusCadastros.Text = "Carregando...";
            // 
            // panelCadastrosBotoes
            // 
            panelCadastrosBotoes.Controls.Add(txtFiltroCadastro);
            panelCadastrosBotoes.Controls.Add(btnBuscarCadastro);
            panelCadastrosBotoes.Controls.Add(btnNovoCadastro);
            panelCadastrosBotoes.Controls.Add(btnExcluirCadastro);
            panelCadastrosBotoes.Dock = DockStyle.Top;
            panelCadastrosBotoes.Location = new Point(5, 45);
            panelCadastrosBotoes.Name = "panelCadastrosBotoes";
            panelCadastrosBotoes.Size = new Size(340, 32);
            panelCadastrosBotoes.TabIndex = 2;
            // 
            // txtFiltroCadastro
            // 
            txtFiltroCadastro.Location = new Point(0, 4);
            txtFiltroCadastro.Name = "txtFiltroCadastro";
            txtFiltroCadastro.PlaceholderText = "ID ou nome...";
            txtFiltroCadastro.Size = new Size(100, 23);
            txtFiltroCadastro.TabIndex = 0;
            txtFiltroCadastro.KeyDown += txtFiltroCadastro_KeyDown;
            // 
            // btnBuscarCadastro
            // 
            btnBuscarCadastro.Location = new Point(103, 3);
            btnBuscarCadastro.Name = "btnBuscarCadastro";
            btnBuscarCadastro.Size = new Size(55, 25);
            btnBuscarCadastro.TabIndex = 1;
            btnBuscarCadastro.Text = "Buscar";
            btnBuscarCadastro.Click += btnBuscarCadastro_Click;
            // 
            // btnNovoCadastro
            // 
            btnNovoCadastro.BackColor = Color.FromArgb(40, 167, 69);
            btnNovoCadastro.FlatStyle = FlatStyle.Flat;
            btnNovoCadastro.ForeColor = Color.White;
            btnNovoCadastro.Location = new Point(161, 3);
            btnNovoCadastro.Name = "btnNovoCadastro";
            btnNovoCadastro.Size = new Size(60, 25);
            btnNovoCadastro.TabIndex = 2;
            btnNovoCadastro.Text = "+ Novo";
            btnNovoCadastro.UseVisualStyleBackColor = false;
            btnNovoCadastro.Click += btnNovoCadastro_Click;
            // 
            // btnExcluirCadastro
            // 
            btnExcluirCadastro.BackColor = Color.FromArgb(220, 53, 69);
            btnExcluirCadastro.FlatStyle = FlatStyle.Flat;
            btnExcluirCadastro.ForeColor = Color.White;
            btnExcluirCadastro.Location = new Point(224, 3);
            btnExcluirCadastro.Name = "btnExcluirCadastro";
            btnExcluirCadastro.Size = new Size(60, 25);
            btnExcluirCadastro.TabIndex = 3;
            btnExcluirCadastro.Text = "Excluir";
            btnExcluirCadastro.UseVisualStyleBackColor = false;
            btnExcluirCadastro.Click += btnExcluirCadastro_Click;
            // 
            // panelPaginacao - Botões de paginação (<< Anterior | 1/5 | Próxima >>)
            // 
            panelPaginacao.Controls.Add(btnAnterior);
            panelPaginacao.Controls.Add(lblPagina);
            panelPaginacao.Controls.Add(btnProxima);
            panelPaginacao.Dock = DockStyle.Top;
            panelPaginacao.Location = new Point(5, 77);
            panelPaginacao.Name = "panelPaginacao";
            panelPaginacao.Size = new Size(340, 26);
            panelPaginacao.TabIndex = 3;
            // 
            // btnAnterior
            // 
            btnAnterior.Enabled = false;
            btnAnterior.Font = new Font("Segoe UI", 8F);
            btnAnterior.Location = new Point(0, 1);
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(65, 23);
            btnAnterior.TabIndex = 0;
            btnAnterior.Text = "<< Ant.";
            btnAnterior.Click += btnAnterior_Click;
            // 
            // lblPagina
            // 
            lblPagina.Font = new Font("Segoe UI", 8F);
            lblPagina.Location = new Point(68, 1);
            lblPagina.Name = "lblPagina";
            lblPagina.Size = new Size(50, 23);
            lblPagina.TabIndex = 1;
            lblPagina.Text = "0/0";
            lblPagina.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnProxima
            // 
            btnProxima.Enabled = false;
            btnProxima.Font = new Font("Segoe UI", 8F);
            btnProxima.Location = new Point(121, 1);
            btnProxima.Name = "btnProxima";
            btnProxima.Size = new Size(65, 23);
            btnProxima.TabIndex = 2;
            btnProxima.Text = "Próx. >>";
            btnProxima.Click += btnProxima_Click;
            // 
            // listCadastros
            // 
            listCadastros.Columns.AddRange(new ColumnHeader[] { colCadId, colCadNome, colCadAtivo, colCadContagem });
            listCadastros.Dock = DockStyle.Fill;
            listCadastros.FullRowSelect = true;
            listCadastros.GridLines = true;
            listCadastros.Location = new Point(5, 103);
            listCadastros.MultiSelect = false;
            listCadastros.Name = "listCadastros";
            listCadastros.Size = new Size(340, 392);
            listCadastros.TabIndex = 4;
            listCadastros.UseCompatibleStateImageBehavior = false;
            listCadastros.View = View.Details;
            listCadastros.SelectedIndexChanged += listCadastros_SelectedIndexChanged;
            // 
            // colCadId
            // 
            colCadId.Text = "ID";
            colCadId.Width = 50;
            // 
            // colCadNome
            // 
            colCadNome.Text = "Nome";
            colCadNome.Width = 130;
            // 
            // colCadAtivo
            // 
            colCadAtivo.Text = "Ativo";
            colCadAtivo.Width = 45;
            // 
            // colCadContagem
            // 
            colCadContagem.Text = "Entidades";
            colCadContagem.Width = 80;
            // 
            // panelEntidades - Nível 2: Entidades
            // 
            panelEntidades.Controls.Add(listEntidades);
            panelEntidades.Controls.Add(panelEntidadesBotoes);
            panelEntidades.Controls.Add(lblStatusEntidades);
            panelEntidades.Controls.Add(lblEntidadesTitulo);
            panelEntidades.Dock = DockStyle.Fill;
            panelEntidades.Location = new Point(0, 0);
            panelEntidades.Name = "panelEntidades";
            panelEntidades.Padding = new Padding(5);
            panelEntidades.Size = new Size(346, 500);
            panelEntidades.TabIndex = 0;
            // 
            // lblEntidadesTitulo
            // 
            lblEntidadesTitulo.Dock = DockStyle.Top;
            lblEntidadesTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEntidadesTitulo.Location = new Point(5, 5);
            lblEntidadesTitulo.Name = "lblEntidadesTitulo";
            lblEntidadesTitulo.Size = new Size(336, 22);
            lblEntidadesTitulo.TabIndex = 0;
            lblEntidadesTitulo.Text = "2. Entidades (selecione um cadastro)";
            // 
            // lblStatusEntidades
            // 
            lblStatusEntidades.Dock = DockStyle.Top;
            lblStatusEntidades.ForeColor = Color.Gray;
            lblStatusEntidades.Location = new Point(5, 27);
            lblStatusEntidades.Name = "lblStatusEntidades";
            lblStatusEntidades.Size = new Size(336, 18);
            lblStatusEntidades.TabIndex = 1;
            // 
            // panelEntidadesBotoes
            // 
            panelEntidadesBotoes.Controls.Add(btnNovaEntidade);
            panelEntidadesBotoes.Controls.Add(btnExcluirEntidade);
            panelEntidadesBotoes.Dock = DockStyle.Top;
            panelEntidadesBotoes.Location = new Point(5, 45);
            panelEntidadesBotoes.Name = "panelEntidadesBotoes";
            panelEntidadesBotoes.Size = new Size(336, 32);
            panelEntidadesBotoes.TabIndex = 2;
            // 
            // btnNovaEntidade
            // 
            btnNovaEntidade.BackColor = Color.FromArgb(40, 167, 69);
            btnNovaEntidade.FlatStyle = FlatStyle.Flat;
            btnNovaEntidade.ForeColor = Color.White;
            btnNovaEntidade.Location = new Point(0, 3);
            btnNovaEntidade.Name = "btnNovaEntidade";
            btnNovaEntidade.Size = new Size(100, 25);
            btnNovaEntidade.TabIndex = 0;
            btnNovaEntidade.Text = "+ Pessoa/Veíc.";
            btnNovaEntidade.UseVisualStyleBackColor = false;
            btnNovaEntidade.Click += btnNovaEntidade_Click;
            // 
            // btnExcluirEntidade
            // 
            btnExcluirEntidade.BackColor = Color.FromArgb(220, 53, 69);
            btnExcluirEntidade.FlatStyle = FlatStyle.Flat;
            btnExcluirEntidade.ForeColor = Color.White;
            btnExcluirEntidade.Location = new Point(103, 3);
            btnExcluirEntidade.Name = "btnExcluirEntidade";
            btnExcluirEntidade.Size = new Size(70, 25);
            btnExcluirEntidade.TabIndex = 1;
            btnExcluirEntidade.Text = "Excluir";
            btnExcluirEntidade.UseVisualStyleBackColor = false;
            btnExcluirEntidade.Click += btnExcluirEntidade_Click;
            // 
            // listEntidades
            // 
            listEntidades.Columns.AddRange(new ColumnHeader[] { colEntId, colEntTipo, colEntNome, colEntDoc, colEntLpr });
            listEntidades.Dock = DockStyle.Fill;
            listEntidades.FullRowSelect = true;
            listEntidades.GridLines = true;
            listEntidades.Location = new Point(5, 77);
            listEntidades.MultiSelect = false;
            listEntidades.Name = "listEntidades";
            listEntidades.Size = new Size(336, 418);
            listEntidades.TabIndex = 3;
            listEntidades.UseCompatibleStateImageBehavior = false;
            listEntidades.View = View.Details;
            listEntidades.SelectedIndexChanged += listEntidades_SelectedIndexChanged;
            // 
            // colEntId
            // 
            colEntId.Text = "ID";
            colEntId.Width = 50;
            // 
            // colEntTipo
            // 
            colEntTipo.Text = "Tipo";
            colEntTipo.Width = 60;
            // 
            // colEntNome
            // 
            colEntNome.Text = "Nome";
            colEntNome.Width = 100;
            // 
            // colEntDoc
            // 
            colEntDoc.Text = "Doc";
            colEntDoc.Width = 70;
            // 
            // colEntLpr
            // 
            colEntLpr.Text = "LPR";
            colEntLpr.Width = 35;
            // 
            // panelMidias - Nível 3: Mídias
            // 
            panelMidias.Controls.Add(listMidias);
            panelMidias.Controls.Add(panelMidiasBotoes);
            panelMidias.Controls.Add(lblStatusMidias);
            panelMidias.Controls.Add(lblMidiasTitulo);
            panelMidias.Dock = DockStyle.Fill;
            panelMidias.Location = new Point(0, 0);
            panelMidias.Name = "panelMidias";
            panelMidias.Padding = new Padding(5);
            panelMidias.Size = new Size(396, 500);
            panelMidias.TabIndex = 0;
            // 
            // lblMidiasTitulo
            // 
            lblMidiasTitulo.Dock = DockStyle.Top;
            lblMidiasTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMidiasTitulo.Location = new Point(5, 5);
            lblMidiasTitulo.Name = "lblMidiasTitulo";
            lblMidiasTitulo.Size = new Size(386, 22);
            lblMidiasTitulo.TabIndex = 0;
            lblMidiasTitulo.Text = "3. Mídias (selecione uma entidade)";
            // 
            // lblStatusMidias
            // 
            lblStatusMidias.Dock = DockStyle.Top;
            lblStatusMidias.ForeColor = Color.Gray;
            lblStatusMidias.Location = new Point(5, 27);
            lblStatusMidias.Name = "lblStatusMidias";
            lblStatusMidias.Size = new Size(386, 18);
            lblStatusMidias.TabIndex = 1;
            // 
            // panelMidiasBotoes
            // 
            panelMidiasBotoes.Controls.Add(btnNovaMidia);
            panelMidiasBotoes.Controls.Add(btnExcluirMidia);
            panelMidiasBotoes.Dock = DockStyle.Top;
            panelMidiasBotoes.Location = new Point(5, 45);
            panelMidiasBotoes.Name = "panelMidiasBotoes";
            panelMidiasBotoes.Size = new Size(386, 32);
            panelMidiasBotoes.TabIndex = 2;
            // 
            // btnNovaMidia
            // 
            btnNovaMidia.BackColor = Color.FromArgb(40, 167, 69);
            btnNovaMidia.FlatStyle = FlatStyle.Flat;
            btnNovaMidia.ForeColor = Color.White;
            btnNovaMidia.Location = new Point(0, 3);
            btnNovaMidia.Name = "btnNovaMidia";
            btnNovaMidia.Size = new Size(90, 25);
            btnNovaMidia.TabIndex = 0;
            btnNovaMidia.Text = "+ Mídia";
            btnNovaMidia.UseVisualStyleBackColor = false;
            btnNovaMidia.Click += btnNovaMidia_Click;
            // 
            // btnExcluirMidia
            // 
            btnExcluirMidia.BackColor = Color.FromArgb(220, 53, 69);
            btnExcluirMidia.FlatStyle = FlatStyle.Flat;
            btnExcluirMidia.ForeColor = Color.White;
            btnExcluirMidia.Location = new Point(93, 3);
            btnExcluirMidia.Name = "btnExcluirMidia";
            btnExcluirMidia.Size = new Size(70, 25);
            btnExcluirMidia.TabIndex = 1;
            btnExcluirMidia.Text = "Excluir";
            btnExcluirMidia.UseVisualStyleBackColor = false;
            btnExcluirMidia.Click += btnExcluirMidia_Click;
            // 
            // listMidias
            // 
            listMidias.Columns.AddRange(new ColumnHeader[] { colMidId, colMidTipo, colMidDesc, colMidAtivo });
            listMidias.Dock = DockStyle.Fill;
            listMidias.FullRowSelect = true;
            listMidias.GridLines = true;
            listMidias.Location = new Point(5, 77);
            listMidias.MultiSelect = false;
            listMidias.Name = "listMidias";
            listMidias.Size = new Size(386, 418);
            listMidias.TabIndex = 3;
            listMidias.UseCompatibleStateImageBehavior = false;
            listMidias.View = View.Details;
            // 
            // colMidId
            // 
            colMidId.Text = "ID";
            colMidId.Width = 60;
            // 
            // colMidTipo
            // 
            colMidTipo.Text = "Tipo";
            colMidTipo.Width = 100;
            // 
            // colMidDesc
            // 
            colMidDesc.Text = "Descrição";
            colMidDesc.Width = 120;
            // 
            // colMidAtivo
            // 
            colMidAtivo.Text = "Ativo";
            colMidAtivo.Width = 45;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.Dock = DockStyle.Bottom;
            txtLog.Font = new Font("Consolas", 8.25F);
            txtLog.ForeColor = Color.FromArgb(220, 220, 220);
            txtLog.Location = new Point(0, 500);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(1100, 61);
            txtLog.TabIndex = 1;
            // 
            // FormCadastroCompleto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 561);
            Controls.Add(splitMain);
            Controls.Add(txtLog);
            MinimumSize = new Size(900, 500);
            Name = "FormCadastroCompleto";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cadastro Completo - Modelo MobiCortex (Cadastro → Entidade → Mídia)";
            Load += FormCadastroCompleto_Load;
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            splitEsquerda.Panel1.ResumeLayout(false);
            splitEsquerda.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitEsquerda).EndInit();
            splitEsquerda.ResumeLayout(false);
            panelCadastros.ResumeLayout(false);
            panelPaginacao.ResumeLayout(false);
            panelEntidades.ResumeLayout(false);
            panelMidias.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private ColumnHeader colEntLpr;
        private Panel panelMidias;
        private Label lblMidiasTitulo;
        private Label lblStatusMidias;
        private FlowLayoutPanel panelMidiasBotoes;
        private Button btnNovaMidia;
        private Button btnExcluirMidia;
        private ListView listMidias;
        private ColumnHeader colMidId;
        private ColumnHeader colMidTipo;
        private ColumnHeader colMidDesc;
        private ColumnHeader colMidAtivo;
        private TextBox txtLog;
    }
}
