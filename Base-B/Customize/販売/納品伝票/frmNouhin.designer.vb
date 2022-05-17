<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNouhin
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNouhin))
        Me.ToolStripMenu = New MyControls.ToolStripEx()
        Me.toolBtnEnd = New System.Windows.Forms.ToolStripButton()
        Me.toolBtnNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnNextNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnInsertRow = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnDeleteRow = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator26 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnCopyRow = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnSearch = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnSearchCopy = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnPrint = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnPreview = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnUpdate = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnCopyNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnTankaRireki = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnMitumori = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnJutyu = New System.Windows.Forms.ToolStripButton()
        Me.toolSepaJutyu = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnSiire = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnExpandMeisai = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolBtnExport = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuNextNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator22 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSepaResetForm = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuResetForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator24 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuEnd = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuInsertRow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeleteRow = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRowUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRowDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator25 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuCopyRow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCopyNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator23 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExpandMeisai = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSearchMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSearch = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSearchCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMitumori = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuJutyu = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSiire = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuTankaRireki = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEnvironment = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEnvTantousha = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEnvTokuisaki = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEnvNounyuuSaki = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEnvSiiresaki = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEnvShouhin = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPrintMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLblForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCmbForm = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuLblPrinter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCmbPrinter = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPrint = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPreview = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSepa1RyoushuSho = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSepa2RyoushuSho = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuLblFormRyoushuSho = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCmbFormRyoushuSho = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuLblPrinterRyoushuSho = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCmbPrinterRyoushuSho = New System.Windows.Forms.ToolStripComboBox()
        Me.mnuSepa3RyoushuSho = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPrintRyoushuSho = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPreviewRyoushuSho = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.TableLayoutPanelBase = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnPreviewRyoushuSho = New System.Windows.Forms.Button()
        Me.btnPrintRyoushuSho = New System.Windows.Forms.Button()
        Me.edtLblTadasiGaki = New GrapeCity.Win.Input.Edit()
        Me.edtTadasiGaki = New GrapeCity.Win.Input.Edit()
        Me.cmbUriageKubun = New GrapeCity.Win.Input.Combo()
        Me.lblUriageKubun = New System.Windows.Forms.Label()
        Me.sheetGoukei = New GrapeCity.Win.ElTabelle.Sheet()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnClearReferenceCode = New System.Windows.Forms.Button()
        Me.datSeikyuDate = New GrapeCity.Win.Input.[Date]()
        Me.datNouhinDate = New GrapeCity.Win.Input.[Date]()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.chkKariDen = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblTekiyou = New System.Windows.Forms.Label()
        Me.numArariKei = New GrapeCity.Win.Input.Number()
        Me.edtTekiyou = New GrapeCity.Win.Input.Edit()
        Me.MRowSheet = New GrapeCity.Win.ElTabelle.MultiRowSheet()
        Me.lblTokuiName = New System.Windows.Forms.Label()
        Me.lblTantouName = New System.Windows.Forms.Label()
        Me.btnSearchTantou = New System.Windows.Forms.Button()
        Me.edtTantouCode = New GrapeCity.Win.Input.Edit()
        Me.lblTantouCode = New System.Windows.Forms.Label()
        Me.lblSoukoName = New System.Windows.Forms.Label()
        Me.btnSearchSouko = New System.Windows.Forms.Button()
        Me.edtSoukoCode = New GrapeCity.Win.Input.Edit()
        Me.lblSoukoCode = New System.Windows.Forms.Label()
        Me.edtNounyuuName2 = New GrapeCity.Win.Input.Edit()
        Me.edtNounyuuName = New GrapeCity.Win.Input.Edit()
        Me.cmbNounyuuKeisho = New GrapeCity.Win.Input.Combo()
        Me.lblNounyuuCode = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.btnSearchNounyu = New System.Windows.Forms.Button()
        Me.edtNounyuuCode = New GrapeCity.Win.Input.Edit()
        Me.lblTokuiCode = New System.Windows.Forms.Label()
        Me.lblTokuiZeiKubun = New System.Windows.Forms.Label()
        Me.lblTokuiBikou = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblTokuiName2 = New System.Windows.Forms.Label()
        Me.btnSearchTokui = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.edtTokuiCode = New GrapeCity.Win.Input.Edit()
        Me.edtDenpyouCode = New GrapeCity.Win.Input.Edit()
        Me.lblNouhinDate = New System.Windows.Forms.Label()
        Me.lblSeikyuDate = New System.Windows.Forms.Label()
        Me.lblReferenceCode = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblReferenceCodeTitle = New System.Windows.Forms.Label()
        Me.lblRendouSaki = New System.Windows.Forms.Label()
        Me.PanelTitle = New System.Windows.Forms.Panel()
        Me.lblSeikyuZumi = New System.Windows.Forms.Label()
        Me.picInfo = New System.Windows.Forms.PictureBox()
        Me.lblShusei = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblFormPrinter = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ToolStripMenu.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.TableLayoutPanelBase.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.edtLblTadasiGaki, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.edtTadasiGaki, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbUriageKubun, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sheetGoukei, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.datSeikyuDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.datNouhinDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numArariKei, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.edtTekiyou, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MRowSheet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.edtTantouCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.edtSoukoCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.edtNounyuuName2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.edtNounyuuName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbNounyuuKeisho, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.edtNounyuuCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.edtTokuiCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.edtDenpyouCode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelTitle.SuspendLayout()
        CType(Me.picInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStripMenu
        '
        Me.ToolStripMenu.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStripMenu.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStripMenu.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ToolStripMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolBtnEnd, Me.toolBtnNew, Me.ToolStripSeparator2, Me.toolBtnNextNew, Me.ToolStripSeparator3, Me.toolBtnInsertRow, Me.ToolStripSeparator10, Me.toolBtnDeleteRow, Me.ToolStripSeparator26, Me.toolBtnCopyRow, Me.ToolStripSeparator4, Me.toolBtnDelete, Me.ToolStripSeparator5, Me.toolBtnSearch, Me.ToolStripSeparator6, Me.toolBtnSearchCopy, Me.ToolStripSeparator9, Me.toolBtnPrint, Me.ToolStripSeparator7, Me.toolBtnPreview, Me.ToolStripSeparator8, Me.toolBtnUpdate, Me.ToolStripSeparator11, Me.toolBtnCopyNew, Me.ToolStripSeparator15, Me.toolBtnTankaRireki, Me.ToolStripSeparator13, Me.toolBtnMitumori, Me.ToolStripSeparator14, Me.toolBtnJutyu, Me.toolSepaJutyu, Me.toolBtnSiire, Me.ToolStripSeparator12, Me.toolBtnExpandMeisai, Me.ToolStripSeparator17, Me.toolBtnExport, Me.ToolStripSeparator21})
        Me.ToolStripMenu.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripMenu.Name = "ToolStripMenu"
        Me.ToolStripMenu.Size = New System.Drawing.Size(1122, 62)
        Me.ToolStripMenu.Stretch = True
        Me.ToolStripMenu.TabIndex = 0
        '
        'toolBtnEnd
        '
        Me.toolBtnEnd.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.toolBtnEnd.BackColor = System.Drawing.SystemColors.Control
        Me.toolBtnEnd.Image = Global.販売管理C.My.Resources.Resources.close_window_16_B22222
        Me.toolBtnEnd.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnEnd.Margin = New System.Windows.Forms.Padding(2)
        Me.toolBtnEnd.Name = "toolBtnEnd"
        Me.toolBtnEnd.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me.toolBtnEnd.Size = New System.Drawing.Size(43, 58)
        Me.toolBtnEnd.Text = "終了" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&X)"
        Me.toolBtnEnd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'toolBtnNew
        '
        Me.toolBtnNew.BackColor = System.Drawing.SystemColors.Control
        Me.toolBtnNew.Image = Global.販売管理C.My.Resources.Resources.file_16_307EA9
        Me.toolBtnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnNew.Name = "toolBtnNew"
        Me.toolBtnNew.Size = New System.Drawing.Size(43, 59)
        Me.toolBtnNew.Text = "新規" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&I)"
        Me.toolBtnNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnNextNew
        '
        Me.toolBtnNextNew.BackColor = System.Drawing.SystemColors.Control
        Me.toolBtnNextNew.Image = Global.販売管理C.My.Resources.Resources.document_16_307EA9
        Me.toolBtnNextNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnNextNew.Name = "toolBtnNextNew"
        Me.toolBtnNextNew.Size = New System.Drawing.Size(58, 59)
        Me.toolBtnNextNew.Text = "次伝票" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&N)"
        Me.toolBtnNextNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnInsertRow
        '
        Me.toolBtnInsertRow.BackColor = System.Drawing.SystemColors.Control
        Me.toolBtnInsertRow.Image = Global.販売管理C.My.Resources.Resources.add_row_16_307EA9
        Me.toolBtnInsertRow.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnInsertRow.Name = "toolBtnInsertRow"
        Me.toolBtnInsertRow.Size = New System.Drawing.Size(58, 59)
        Me.toolBtnInsertRow.Text = "行挿入" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&A)"
        Me.toolBtnInsertRow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnDeleteRow
        '
        Me.toolBtnDeleteRow.Image = Global.販売管理C.My.Resources.Resources.delete_row_16_307EA9
        Me.toolBtnDeleteRow.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnDeleteRow.Name = "toolBtnDeleteRow"
        Me.toolBtnDeleteRow.Size = New System.Drawing.Size(58, 59)
        Me.toolBtnDeleteRow.Text = "行削除" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&B)"
        Me.toolBtnDeleteRow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator26
        '
        Me.ToolStripSeparator26.Name = "ToolStripSeparator26"
        Me.ToolStripSeparator26.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnCopyRow
        '
        Me.toolBtnCopyRow.Image = Global.販売管理C.My.Resources.Resources.copy_row_16_307EA9
        Me.toolBtnCopyRow.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnCopyRow.Name = "toolBtnCopyRow"
        Me.toolBtnCopyRow.Size = New System.Drawing.Size(61, 59)
        Me.toolBtnCopyRow.Text = "行コピー" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&Y)"
        Me.toolBtnCopyRow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnDelete
        '
        Me.toolBtnDelete.Image = Global.販売管理C.My.Resources.Resources.delete_2_16_307EA9
        Me.toolBtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnDelete.Name = "toolBtnDelete"
        Me.toolBtnDelete.Size = New System.Drawing.Size(73, 59)
        Me.toolBtnDelete.Text = "伝票削除" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&D)"
        Me.toolBtnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnSearch
        '
        Me.toolBtnSearch.Image = Global.販売管理C.My.Resources.Resources.search_3_16_307EA9
        Me.toolBtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnSearch.Name = "toolBtnSearch"
        Me.toolBtnSearch.Size = New System.Drawing.Size(43, 59)
        Me.toolBtnSearch.Text = "検索" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&S)"
        Me.toolBtnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnSearchCopy
        '
        Me.toolBtnSearchCopy.Image = Global.販売管理C.My.Resources.Resources.search_add_16_307EA9
        Me.toolBtnSearchCopy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnSearchCopy.Name = "toolBtnSearchCopy"
        Me.toolBtnSearchCopy.Size = New System.Drawing.Size(73, 59)
        Me.toolBtnSearchCopy.Text = "複写入力" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&C)"
        Me.toolBtnSearchCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnPrint
        '
        Me.toolBtnPrint.Image = Global.販売管理C.My.Resources.Resources.printer_16_307EA9
        Me.toolBtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnPrint.Name = "toolBtnPrint"
        Me.toolBtnPrint.Size = New System.Drawing.Size(43, 59)
        Me.toolBtnPrint.Text = "印刷" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&P)"
        Me.toolBtnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnPreview
        '
        Me.toolBtnPreview.Image = Global.販売管理C.My.Resources.Resources.paper_16_307EA9
        Me.toolBtnPreview.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnPreview.Name = "toolBtnPreview"
        Me.toolBtnPreview.Size = New System.Drawing.Size(68, 59)
        Me.toolBtnPreview.Text = "プレビュー" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&V)"
        Me.toolBtnPreview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnUpdate
        '
        Me.toolBtnUpdate.Image = Global.販売管理C.My.Resources.Resources.save_16_307EA9
        Me.toolBtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnUpdate.Name = "toolBtnUpdate"
        Me.toolBtnUpdate.Size = New System.Drawing.Size(43, 59)
        Me.toolBtnUpdate.Text = "登録" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&U)"
        Me.toolBtnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnCopyNew
        '
        Me.toolBtnCopyNew.Image = Global.販売管理C.My.Resources.Resources.copy_16_307EA9
        Me.toolBtnCopyNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnCopyNew.Name = "toolBtnCopyNew"
        Me.toolBtnCopyNew.Size = New System.Drawing.Size(46, 59)
        Me.toolBtnCopyNew.Text = "コピー" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&G)"
        Me.toolBtnCopyNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnTankaRireki
        '
        Me.toolBtnTankaRireki.Image = Global.販売管理C.My.Resources.Resources.list_2_16_307EA9
        Me.toolBtnTankaRireki.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnTankaRireki.Name = "toolBtnTankaRireki"
        Me.toolBtnTankaRireki.Size = New System.Drawing.Size(43, 59)
        Me.toolBtnTankaRireki.Text = "単価" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&H)"
        Me.toolBtnTankaRireki.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnMitumori
        '
        Me.toolBtnMitumori.Image = Global.販売管理C.My.Resources.Resources.search_add_16_307EA9
        Me.toolBtnMitumori.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnMitumori.Name = "toolBtnMitumori"
        Me.toolBtnMitumori.Size = New System.Drawing.Size(43, 59)
        Me.toolBtnMitumori.Text = "見積" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&J)"
        Me.toolBtnMitumori.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnJutyu
        '
        Me.toolBtnJutyu.Image = Global.販売管理C.My.Resources.Resources.search_add_16_307EA9
        Me.toolBtnJutyu.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnJutyu.Name = "toolBtnJutyu"
        Me.toolBtnJutyu.Size = New System.Drawing.Size(43, 59)
        Me.toolBtnJutyu.Text = "受注" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&K)"
        Me.toolBtnJutyu.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'toolSepaJutyu
        '
        Me.toolSepaJutyu.Name = "toolSepaJutyu"
        Me.toolSepaJutyu.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnSiire
        '
        Me.toolBtnSiire.Image = Global.販売管理C.My.Resources.Resources.search_add_16_307EA9
        Me.toolBtnSiire.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnSiire.Name = "toolBtnSiire"
        Me.toolBtnSiire.Size = New System.Drawing.Size(43, 59)
        Me.toolBtnSiire.Text = "仕入" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&M)"
        Me.toolBtnSiire.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnExpandMeisai
        '
        Me.toolBtnExpandMeisai.Image = Global.販売管理C.My.Resources.Resources.resize2_16_307EA9
        Me.toolBtnExpandMeisai.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnExpandMeisai.Name = "toolBtnExpandMeisai"
        Me.toolBtnExpandMeisai.Size = New System.Drawing.Size(38, 59)
        Me.toolBtnExpandMeisai.Text = "行" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&O)"
        Me.toolBtnExpandMeisai.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(6, 62)
        '
        'toolBtnExport
        '
        Me.toolBtnExport.BackColor = System.Drawing.SystemColors.Control
        Me.toolBtnExport.Image = Global.販売管理C.My.Resources.Resources.downloading_updates_16_307EA9
        Me.toolBtnExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolBtnExport.Name = "toolBtnExport"
        Me.toolBtnExport.Size = New System.Drawing.Size(80, 59)
        Me.toolBtnExport.Text = "エクスポート" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(&W)"
        Me.toolBtnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(6, 62)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuEdit, Me.mnuSearchMenu, Me.mnuEnvironment, Me.mnuPrintMenu})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1122, 27)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNew, Me.mnuNextNew, Me.ToolStripSeparator22, Me.mnuUpdate, Me.mnuDelete, Me.ToolStripSeparator20, Me.mnuExport, Me.mnuSepaResetForm, Me.mnuResetForm, Me.ToolStripSeparator24, Me.mnuEnd})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(86, 23)
        Me.mnuFile.Text = "ファイル(&F)"
        '
        'mnuNew
        '
        Me.mnuNew.Image = Global.販売管理C.My.Resources.Resources.file_16_307EA9
        Me.mnuNew.Name = "mnuNew"
        Me.mnuNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.mnuNew.Size = New System.Drawing.Size(285, 24)
        Me.mnuNew.Text = "全てクリアし新規伝票"
        '
        'mnuNextNew
        '
        Me.mnuNextNew.Image = Global.販売管理C.My.Resources.Resources.document_16_307EA9
        Me.mnuNextNew.Name = "mnuNextNew"
        Me.mnuNextNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuNextNew.Size = New System.Drawing.Size(285, 24)
        Me.mnuNextNew.Text = "同じ得意先で新規の伝票"
        '
        'ToolStripSeparator22
        '
        Me.ToolStripSeparator22.Name = "ToolStripSeparator22"
        Me.ToolStripSeparator22.Size = New System.Drawing.Size(282, 6)
        '
        'mnuUpdate
        '
        Me.mnuUpdate.Image = Global.販売管理C.My.Resources.Resources.save_16_307EA9
        Me.mnuUpdate.Name = "mnuUpdate"
        Me.mnuUpdate.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.U), System.Windows.Forms.Keys)
        Me.mnuUpdate.Size = New System.Drawing.Size(285, 24)
        Me.mnuUpdate.Text = "登録/更新"
        '
        'mnuDelete
        '
        Me.mnuDelete.Image = Global.販売管理C.My.Resources.Resources.delete_2_16_307EA9
        Me.mnuDelete.Name = "mnuDelete"
        Me.mnuDelete.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.mnuDelete.Size = New System.Drawing.Size(285, 24)
        Me.mnuDelete.Text = "伝票削除"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(282, 6)
        '
        'mnuExport
        '
        Me.mnuExport.Image = Global.販売管理C.My.Resources.Resources.downloading_updates_16_307EA9
        Me.mnuExport.Name = "mnuExport"
        Me.mnuExport.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.mnuExport.Size = New System.Drawing.Size(285, 24)
        Me.mnuExport.Text = "エクスポート"
        '
        'mnuSepaResetForm
        '
        Me.mnuSepaResetForm.Name = "mnuSepaResetForm"
        Me.mnuSepaResetForm.Size = New System.Drawing.Size(282, 6)
        '
        'mnuResetForm
        '
        Me.mnuResetForm.Image = Global.販売管理C.My.Resources.Resources.undo_4_16_307EA9
        Me.mnuResetForm.Name = "mnuResetForm"
        Me.mnuResetForm.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.mnuResetForm.Size = New System.Drawing.Size(285, 24)
        Me.mnuResetForm.Text = "画面サイズ　リセット"
        Me.mnuResetForm.ToolTipText = "画面サイズをデフォルトに戻します"
        '
        'ToolStripSeparator24
        '
        Me.ToolStripSeparator24.Name = "ToolStripSeparator24"
        Me.ToolStripSeparator24.Size = New System.Drawing.Size(282, 6)
        '
        'mnuEnd
        '
        Me.mnuEnd.Image = Global.販売管理C.My.Resources.Resources.close_window_16_B22222
        Me.mnuEnd.Name = "mnuEnd"
        Me.mnuEnd.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.mnuEnd.Size = New System.Drawing.Size(285, 24)
        Me.mnuEnd.Text = "終了"
        '
        'mnuEdit
        '
        Me.mnuEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuInsertRow, Me.mnuDeleteRow, Me.ToolStripSeparator16, Me.mnuRowUp, Me.mnuRowDown, Me.ToolStripSeparator25, Me.mnuCopyRow, Me.mnuCopyNew, Me.ToolStripSeparator23, Me.mnuExpandMeisai})
        Me.mnuEdit.Name = "mnuEdit"
        Me.mnuEdit.Size = New System.Drawing.Size(74, 23)
        Me.mnuEdit.Text = "編集(&E)"
        '
        'mnuInsertRow
        '
        Me.mnuInsertRow.Image = Global.販売管理C.My.Resources.Resources.add_row_16_307EA9
        Me.mnuInsertRow.Name = "mnuInsertRow"
        Me.mnuInsertRow.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.mnuInsertRow.Size = New System.Drawing.Size(235, 24)
        Me.mnuInsertRow.Text = "行挿入"
        '
        'mnuDeleteRow
        '
        Me.mnuDeleteRow.Image = Global.販売管理C.My.Resources.Resources.delete_row_16_307EA9
        Me.mnuDeleteRow.Name = "mnuDeleteRow"
        Me.mnuDeleteRow.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.mnuDeleteRow.Size = New System.Drawing.Size(235, 24)
        Me.mnuDeleteRow.Text = "行削除"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(232, 6)
        '
        'mnuRowUp
        '
        Me.mnuRowUp.Image = Global.販売管理C.My.Resources.Resources.arrow_186_16_307EA9
        Me.mnuRowUp.Name = "mnuRowUp"
        Me.mnuRowUp.ShortcutKeys = CType(((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.mnuRowUp.Size = New System.Drawing.Size(235, 24)
        Me.mnuRowUp.Text = "行移動↑"
        '
        'mnuRowDown
        '
        Me.mnuRowDown.Image = Global.販売管理C.My.Resources.Resources.arrow_189_16_307EA9
        Me.mnuRowDown.Name = "mnuRowDown"
        Me.mnuRowDown.ShortcutKeys = CType(((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.mnuRowDown.Size = New System.Drawing.Size(235, 24)
        Me.mnuRowDown.Text = "行移動↓"
        '
        'ToolStripSeparator25
        '
        Me.ToolStripSeparator25.Name = "ToolStripSeparator25"
        Me.ToolStripSeparator25.Size = New System.Drawing.Size(232, 6)
        '
        'mnuCopyRow
        '
        Me.mnuCopyRow.Image = Global.販売管理C.My.Resources.Resources.copy_row_16_307EA9
        Me.mnuCopyRow.Name = "mnuCopyRow"
        Me.mnuCopyRow.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.mnuCopyRow.Size = New System.Drawing.Size(235, 24)
        Me.mnuCopyRow.Text = "行コピー"
        '
        'mnuCopyNew
        '
        Me.mnuCopyNew.Image = Global.販売管理C.My.Resources.Resources.copy_16_307EA9
        Me.mnuCopyNew.Name = "mnuCopyNew"
        Me.mnuCopyNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.G), System.Windows.Forms.Keys)
        Me.mnuCopyNew.Size = New System.Drawing.Size(235, 24)
        Me.mnuCopyNew.Text = "伝票コピー"
        '
        'ToolStripSeparator23
        '
        Me.ToolStripSeparator23.Name = "ToolStripSeparator23"
        Me.ToolStripSeparator23.Size = New System.Drawing.Size(232, 6)
        '
        'mnuExpandMeisai
        '
        Me.mnuExpandMeisai.Image = Global.販売管理C.My.Resources.Resources.resize2_16_307EA9
        Me.mnuExpandMeisai.Name = "mnuExpandMeisai"
        Me.mnuExpandMeisai.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mnuExpandMeisai.Size = New System.Drawing.Size(235, 24)
        Me.mnuExpandMeisai.Text = "行表示の拡縮"
        '
        'mnuSearchMenu
        '
        Me.mnuSearchMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSearch, Me.mnuSearchCopy, Me.ToolStripSeparator19, Me.mnuMitumori, Me.mnuJutyu, Me.mnuSiire, Me.ToolStripSeparator18, Me.mnuTankaRireki})
        Me.mnuSearchMenu.Name = "mnuSearchMenu"
        Me.mnuSearchMenu.Size = New System.Drawing.Size(73, 23)
        Me.mnuSearchMenu.Text = "検索(&L)"
        '
        'mnuSearch
        '
        Me.mnuSearch.Image = Global.販売管理C.My.Resources.Resources.search_3_16_307EA9
        Me.mnuSearch.Name = "mnuSearch"
        Me.mnuSearch.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuSearch.Size = New System.Drawing.Size(251, 24)
        Me.mnuSearch.Text = "納品伝票検索"
        '
        'mnuSearchCopy
        '
        Me.mnuSearchCopy.Image = Global.販売管理C.My.Resources.Resources.search_add_16_307EA9
        Me.mnuSearchCopy.Name = "mnuSearchCopy"
        Me.mnuSearchCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.mnuSearchCopy.Size = New System.Drawing.Size(251, 24)
        Me.mnuSearchCopy.Text = "複写入力"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(248, 6)
        '
        'mnuMitumori
        '
        Me.mnuMitumori.Image = Global.販売管理C.My.Resources.Resources.search_add_16_307EA9
        Me.mnuMitumori.Name = "mnuMitumori"
        Me.mnuMitumori.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.J), System.Windows.Forms.Keys)
        Me.mnuMitumori.Size = New System.Drawing.Size(251, 24)
        Me.mnuMitumori.Text = "見積書参照入力"
        '
        'mnuJutyu
        '
        Me.mnuJutyu.Image = Global.販売管理C.My.Resources.Resources.search_add_16_307EA9
        Me.mnuJutyu.Name = "mnuJutyu"
        Me.mnuJutyu.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.mnuJutyu.Size = New System.Drawing.Size(251, 24)
        Me.mnuJutyu.Text = "受注伝票参照入力"
        '
        'mnuSiire
        '
        Me.mnuSiire.Image = Global.販売管理C.My.Resources.Resources.search_add_16_307EA9
        Me.mnuSiire.Name = "mnuSiire"
        Me.mnuSiire.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.mnuSiire.Size = New System.Drawing.Size(251, 24)
        Me.mnuSiire.Text = "仕入伝票参照入力"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(248, 6)
        '
        'mnuTankaRireki
        '
        Me.mnuTankaRireki.Image = Global.販売管理C.My.Resources.Resources.list_2_16_307EA9
        Me.mnuTankaRireki.Name = "mnuTankaRireki"
        Me.mnuTankaRireki.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.mnuTankaRireki.Size = New System.Drawing.Size(251, 24)
        Me.mnuTankaRireki.Text = "単価履歴"
        '
        'mnuEnvironment
        '
        Me.mnuEnvironment.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEnvTantousha, Me.mnuEnvTokuisaki, Me.mnuEnvNounyuuSaki, Me.mnuEnvSiiresaki, Me.mnuEnvShouhin})
        Me.mnuEnvironment.Name = "mnuEnvironment"
        Me.mnuEnvironment.Size = New System.Drawing.Size(105, 23)
        Me.mnuEnvironment.Text = "環境設定(&T)"
        '
        'mnuEnvTantousha
        '
        Me.mnuEnvTantousha.Name = "mnuEnvTantousha"
        Me.mnuEnvTantousha.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.D1), System.Windows.Forms.Keys)
        Me.mnuEnvTantousha.Size = New System.Drawing.Size(203, 24)
        Me.mnuEnvTantousha.Text = "担当者登録"
        '
        'mnuEnvTokuisaki
        '
        Me.mnuEnvTokuisaki.Name = "mnuEnvTokuisaki"
        Me.mnuEnvTokuisaki.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.D2), System.Windows.Forms.Keys)
        Me.mnuEnvTokuisaki.Size = New System.Drawing.Size(203, 24)
        Me.mnuEnvTokuisaki.Text = "得意先登録"
        '
        'mnuEnvNounyuuSaki
        '
        Me.mnuEnvNounyuuSaki.Name = "mnuEnvNounyuuSaki"
        Me.mnuEnvNounyuuSaki.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.D3), System.Windows.Forms.Keys)
        Me.mnuEnvNounyuuSaki.Size = New System.Drawing.Size(203, 24)
        Me.mnuEnvNounyuuSaki.Text = "納入先登録"
        '
        'mnuEnvSiiresaki
        '
        Me.mnuEnvSiiresaki.Name = "mnuEnvSiiresaki"
        Me.mnuEnvSiiresaki.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.D4), System.Windows.Forms.Keys)
        Me.mnuEnvSiiresaki.Size = New System.Drawing.Size(203, 24)
        Me.mnuEnvSiiresaki.Text = "仕入先登録"
        '
        'mnuEnvShouhin
        '
        Me.mnuEnvShouhin.Name = "mnuEnvShouhin"
        Me.mnuEnvShouhin.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.D5), System.Windows.Forms.Keys)
        Me.mnuEnvShouhin.Size = New System.Drawing.Size(203, 24)
        Me.mnuEnvShouhin.Text = "商品登録"
        '
        'mnuPrintMenu
        '
        Me.mnuPrintMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuLblForm, Me.mnuCmbForm, Me.mnuLblPrinter, Me.mnuCmbPrinter, Me.ToolStripSeparator1, Me.mnuPrint, Me.mnuPreview, Me.mnuSepa1RyoushuSho, Me.mnuSepa2RyoushuSho, Me.mnuLblFormRyoushuSho, Me.mnuCmbFormRyoushuSho, Me.mnuLblPrinterRyoushuSho, Me.mnuCmbPrinterRyoushuSho, Me.mnuSepa3RyoushuSho, Me.mnuPrintRyoushuSho, Me.mnuPreviewRyoushuSho})
        Me.mnuPrintMenu.Name = "mnuPrintMenu"
        Me.mnuPrintMenu.Size = New System.Drawing.Size(136, 23)
        Me.mnuPrintMenu.Text = "納品伝票印刷(&Q)"
        '
        'mnuLblForm
        '
        Me.mnuLblForm.Name = "mnuLblForm"
        Me.mnuLblForm.Size = New System.Drawing.Size(300, 24)
        Me.mnuLblForm.Text = "◇納品伝票フォーム："
        '
        'mnuCmbForm
        '
        Me.mnuCmbForm.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.mnuCmbForm.Margin = New System.Windows.Forms.Padding(2, 0, 0, 2)
        Me.mnuCmbForm.Name = "mnuCmbForm"
        Me.mnuCmbForm.Size = New System.Drawing.Size(240, 23)
        '
        'mnuLblPrinter
        '
        Me.mnuLblPrinter.Name = "mnuLblPrinter"
        Me.mnuLblPrinter.Size = New System.Drawing.Size(300, 24)
        Me.mnuLblPrinter.Text = "◇納品伝票プリンタ："
        '
        'mnuCmbPrinter
        '
        Me.mnuCmbPrinter.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.mnuCmbPrinter.Margin = New System.Windows.Forms.Padding(2, 0, 0, 2)
        Me.mnuCmbPrinter.Name = "mnuCmbPrinter"
        Me.mnuCmbPrinter.Size = New System.Drawing.Size(240, 23)
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(297, 6)
        '
        'mnuPrint
        '
        Me.mnuPrint.Image = Global.販売管理C.My.Resources.Resources.printer_16_307EA9
        Me.mnuPrint.Name = "mnuPrint"
        Me.mnuPrint.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mnuPrint.Size = New System.Drawing.Size(300, 24)
        Me.mnuPrint.Text = "納品伝票印刷"
        '
        'mnuPreview
        '
        Me.mnuPreview.Image = Global.販売管理C.My.Resources.Resources.paper_16_307EA9
        Me.mnuPreview.Name = "mnuPreview"
        Me.mnuPreview.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.mnuPreview.Size = New System.Drawing.Size(300, 24)
        Me.mnuPreview.Text = "納品伝票プレビュー"
        '
        'mnuSepa1RyoushuSho
        '
        Me.mnuSepa1RyoushuSho.Name = "mnuSepa1RyoushuSho"
        Me.mnuSepa1RyoushuSho.Size = New System.Drawing.Size(297, 6)
        '
        'mnuSepa2RyoushuSho
        '
        Me.mnuSepa2RyoushuSho.Name = "mnuSepa2RyoushuSho"
        Me.mnuSepa2RyoushuSho.Size = New System.Drawing.Size(297, 6)
        '
        'mnuLblFormRyoushuSho
        '
        Me.mnuLblFormRyoushuSho.Name = "mnuLblFormRyoushuSho"
        Me.mnuLblFormRyoushuSho.Size = New System.Drawing.Size(300, 24)
        Me.mnuLblFormRyoushuSho.Text = "◇領収書フォーム："
        '
        'mnuCmbFormRyoushuSho
        '
        Me.mnuCmbFormRyoushuSho.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.mnuCmbFormRyoushuSho.Margin = New System.Windows.Forms.Padding(2, 0, 0, 2)
        Me.mnuCmbFormRyoushuSho.Name = "mnuCmbFormRyoushuSho"
        Me.mnuCmbFormRyoushuSho.Size = New System.Drawing.Size(240, 23)
        '
        'mnuLblPrinterRyoushuSho
        '
        Me.mnuLblPrinterRyoushuSho.Name = "mnuLblPrinterRyoushuSho"
        Me.mnuLblPrinterRyoushuSho.Size = New System.Drawing.Size(300, 24)
        Me.mnuLblPrinterRyoushuSho.Text = "◇領収書プリンタ："
        '
        'mnuCmbPrinterRyoushuSho
        '
        Me.mnuCmbPrinterRyoushuSho.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.mnuCmbPrinterRyoushuSho.Margin = New System.Windows.Forms.Padding(2, 0, 0, 2)
        Me.mnuCmbPrinterRyoushuSho.Name = "mnuCmbPrinterRyoushuSho"
        Me.mnuCmbPrinterRyoushuSho.Size = New System.Drawing.Size(240, 23)
        '
        'mnuSepa3RyoushuSho
        '
        Me.mnuSepa3RyoushuSho.Name = "mnuSepa3RyoushuSho"
        Me.mnuSepa3RyoushuSho.Size = New System.Drawing.Size(297, 6)
        '
        'mnuPrintRyoushuSho
        '
        Me.mnuPrintRyoushuSho.Image = Global.販売管理C.My.Resources.Resources.printer_16_307EA9
        Me.mnuPrintRyoushuSho.Name = "mnuPrintRyoushuSho"
        Me.mnuPrintRyoushuSho.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.mnuPrintRyoushuSho.Size = New System.Drawing.Size(300, 24)
        Me.mnuPrintRyoushuSho.Text = "領収書印刷"
        '
        'mnuPreviewRyoushuSho
        '
        Me.mnuPreviewRyoushuSho.Image = Global.販売管理C.My.Resources.Resources.paper_16_307EA9
        Me.mnuPreviewRyoushuSho.Name = "mnuPreviewRyoushuSho"
        Me.mnuPreviewRyoushuSho.ShortcutKeys = CType(((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.mnuPreviewRyoushuSho.Size = New System.Drawing.Size(300, 24)
        Me.mnuPreviewRyoushuSho.Text = "領収書プレビュー"
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.TableLayoutPanelBase)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(1122, 622)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 27)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(1122, 684)
        Me.ToolStripContainer1.TabIndex = 0
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ToolStripMenu)
        '
        'TableLayoutPanelBase
        '
        Me.TableLayoutPanelBase.AutoScroll = True
        Me.TableLayoutPanelBase.ColumnCount = 1
        Me.TableLayoutPanelBase.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelBase.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanelBase.Controls.Add(Me.PanelTitle, 0, 0)
        Me.TableLayoutPanelBase.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelBase.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelBase.Name = "TableLayoutPanelBase"
        Me.TableLayoutPanelBase.RowCount = 2
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelBase.Size = New System.Drawing.Size(1122, 622)
        Me.TableLayoutPanelBase.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnPreviewRyoushuSho)
        Me.Panel1.Controls.Add(Me.btnPrintRyoushuSho)
        Me.Panel1.Controls.Add(Me.edtLblTadasiGaki)
        Me.Panel1.Controls.Add(Me.edtTadasiGaki)
        Me.Panel1.Controls.Add(Me.cmbUriageKubun)
        Me.Panel1.Controls.Add(Me.lblUriageKubun)
        Me.Panel1.Controls.Add(Me.sheetGoukei)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.btnClearReferenceCode)
        Me.Panel1.Controls.Add(Me.datSeikyuDate)
        Me.Panel1.Controls.Add(Me.datNouhinDate)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.chkKariDen)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.lblTekiyou)
        Me.Panel1.Controls.Add(Me.numArariKei)
        Me.Panel1.Controls.Add(Me.edtTekiyou)
        Me.Panel1.Controls.Add(Me.MRowSheet)
        Me.Panel1.Controls.Add(Me.lblTokuiName)
        Me.Panel1.Controls.Add(Me.lblTantouName)
        Me.Panel1.Controls.Add(Me.btnSearchTantou)
        Me.Panel1.Controls.Add(Me.edtTantouCode)
        Me.Panel1.Controls.Add(Me.lblTantouCode)
        Me.Panel1.Controls.Add(Me.lblSoukoName)
        Me.Panel1.Controls.Add(Me.btnSearchSouko)
        Me.Panel1.Controls.Add(Me.edtSoukoCode)
        Me.Panel1.Controls.Add(Me.lblSoukoCode)
        Me.Panel1.Controls.Add(Me.edtNounyuuName2)
        Me.Panel1.Controls.Add(Me.edtNounyuuName)
        Me.Panel1.Controls.Add(Me.cmbNounyuuKeisho)
        Me.Panel1.Controls.Add(Me.lblNounyuuCode)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.btnSearchNounyu)
        Me.Panel1.Controls.Add(Me.edtNounyuuCode)
        Me.Panel1.Controls.Add(Me.lblTokuiCode)
        Me.Panel1.Controls.Add(Me.lblTokuiZeiKubun)
        Me.Panel1.Controls.Add(Me.lblTokuiBikou)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.lblTokuiName2)
        Me.Panel1.Controls.Add(Me.btnSearchTokui)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.edtTokuiCode)
        Me.Panel1.Controls.Add(Me.edtDenpyouCode)
        Me.Panel1.Controls.Add(Me.lblNouhinDate)
        Me.Panel1.Controls.Add(Me.lblSeikyuDate)
        Me.Panel1.Controls.Add(Me.lblReferenceCode)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.lblReferenceCodeTitle)
        Me.Panel1.Controls.Add(Me.lblRendouSaki)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 30)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1116, 589)
        Me.Panel1.TabIndex = 0
        '
        'btnPreviewRyoushuSho
        '
        Me.btnPreviewRyoushuSho.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPreviewRyoushuSho.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnPreviewRyoushuSho.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPreviewRyoushuSho.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnPreviewRyoushuSho.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnPreviewRyoushuSho.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPreviewRyoushuSho.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPreviewRyoushuSho.ForeColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPreviewRyoushuSho.Image = Global.販売管理C.My.Resources.Resources.paper_16_307EA9
        Me.btnPreviewRyoushuSho.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPreviewRyoushuSho.Location = New System.Drawing.Point(866, 563)
        Me.btnPreviewRyoushuSho.Name = "btnPreviewRyoushuSho"
        Me.btnPreviewRyoushuSho.Size = New System.Drawing.Size(83, 25)
        Me.btnPreviewRyoushuSho.TabIndex = 21
        Me.btnPreviewRyoushuSho.Text = "プレビュー"
        Me.btnPreviewRyoushuSho.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnPreviewRyoushuSho.UseVisualStyleBackColor = False
        '
        'btnPrintRyoushuSho
        '
        Me.btnPrintRyoushuSho.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPrintRyoushuSho.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnPrintRyoushuSho.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPrintRyoushuSho.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnPrintRyoushuSho.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnPrintRyoushuSho.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrintRyoushuSho.Font = New System.Drawing.Font("Meiryo UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrintRyoushuSho.ForeColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnPrintRyoushuSho.Image = Global.販売管理C.My.Resources.Resources.printer_16_307EA9
        Me.btnPrintRyoushuSho.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrintRyoushuSho.Location = New System.Drawing.Point(785, 563)
        Me.btnPrintRyoushuSho.Name = "btnPrintRyoushuSho"
        Me.btnPrintRyoushuSho.Size = New System.Drawing.Size(75, 25)
        Me.btnPrintRyoushuSho.TabIndex = 20
        Me.btnPrintRyoushuSho.Text = "領収書"
        Me.btnPrintRyoushuSho.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnPrintRyoushuSho.UseVisualStyleBackColor = False
        '
        'edtLblTadasiGaki
        '
        Me.edtLblTadasiGaki.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.edtLblTadasiGaki.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.edtLblTadasiGaki.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtLblTadasiGaki.Location = New System.Drawing.Point(458, 563)
        Me.edtLblTadasiGaki.Name = "edtLblTadasiGaki"
        Me.edtLblTadasiGaki.ReadOnly = True
        Me.edtLblTadasiGaki.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtLblTadasiGaki.Size = New System.Drawing.Size(69, 25)
        Me.edtLblTadasiGaki.TabIndex = 185
        Me.edtLblTadasiGaki.TabStop = False
        Me.edtLblTadasiGaki.Text = "但し書き"
        Me.edtLblTadasiGaki.TextHAlign = GrapeCity.Win.Input.AlignHorizontal.Center
        Me.edtLblTadasiGaki.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        '
        'edtTadasiGaki
        '
        Me.edtTadasiGaki.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.edtTadasiGaki.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtTadasiGaki.Ellipsis = GrapeCity.Win.Input.EllipsisMode.EllipsisEnd
        Me.edtTadasiGaki.HighlightText = True
        Me.edtTadasiGaki.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.edtTadasiGaki.Location = New System.Drawing.Point(529, 563)
        Me.edtTadasiGaki.MaxLength = 255
        Me.edtTadasiGaki.Name = "edtTadasiGaki"
        Me.edtTadasiGaki.OverflowTip = True
        Me.edtTadasiGaki.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtTadasiGaki.Size = New System.Drawing.Size(250, 25)
        Me.edtTadasiGaki.TabIndex = 19
        '
        'cmbUriageKubun
        '
        Me.cmbUriageKubun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.cmbUriageKubun.DescriptionAutoFit = False
        Me.cmbUriageKubun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUriageKubun.Location = New System.Drawing.Point(839, 122)
        Me.cmbUriageKubun.MaxDropDownItems = 20
        Me.cmbUriageKubun.Name = "cmbUriageKubun"
        Me.cmbUriageKubun.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.cmbUriageKubun.Size = New System.Drawing.Size(90, 25)
        Me.cmbUriageKubun.TabIndex = 14
        Me.cmbUriageKubun.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        Me.cmbUriageKubun.Value = ""
        '
        'lblUriageKubun
        '
        Me.lblUriageKubun.BackColor = System.Drawing.Color.Red
        Me.lblUriageKubun.Location = New System.Drawing.Point(837, 120)
        Me.lblUriageKubun.Name = "lblUriageKubun"
        Me.lblUriageKubun.Size = New System.Drawing.Size(94, 29)
        Me.lblUriageKubun.TabIndex = 287
        Me.lblUriageKubun.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sheetGoukei
        '
        Me.sheetGoukei.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.sheetGoukei.Data = CType(resources.GetObject("sheetGoukei.Data"), GrapeCity.Win.ElTabelle.SheetData)
        Me.sheetGoukei.Location = New System.Drawing.Point(458, 506)
        Me.sheetGoukei.Name = "sheetGoukei"
        Me.sheetGoukei.Size = New System.Drawing.Size(654, 80)
        Me.sheetGoukei.TabIndex = 253
        Me.sheetGoukei.TabStop = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Location = New System.Drawing.Point(234, 506)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 25)
        Me.Label3.TabIndex = 254
        Me.Label3.Text = "粗利益計"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnClearReferenceCode
        '
        Me.btnClearReferenceCode.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnClearReferenceCode.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnClearReferenceCode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnClearReferenceCode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnClearReferenceCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClearReferenceCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnClearReferenceCode.Image = Global.販売管理C.My.Resources.Resources.eraser_16_307EA9
        Me.btnClearReferenceCode.Location = New System.Drawing.Point(938, 32)
        Me.btnClearReferenceCode.Name = "btnClearReferenceCode"
        Me.btnClearReferenceCode.Size = New System.Drawing.Size(32, 25)
        Me.btnClearReferenceCode.TabIndex = 234
        Me.btnClearReferenceCode.TabStop = False
        Me.btnClearReferenceCode.UseVisualStyleBackColor = False
        '
        'datSeikyuDate
        '
        Me.datSeikyuDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.datSeikyuDate.DisplayFormat = New GrapeCity.Win.Input.DateDisplayFormat("yyyy年 M月 d日", "", "")
        Me.datSeikyuDate.DropDownCalendar.Size = New System.Drawing.Size(200, 223)
        Me.datSeikyuDate.DropDownCalendar.Weekdays = New GrapeCity.Win.Input.WeekdaysStyle(New GrapeCity.Win.Input.DayOfWeekStyle("日", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Red, False, False), CType((((((GrapeCity.Win.Input.WeekFlags.FirstWeek Or GrapeCity.Win.Input.WeekFlags.SecondWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.ThirdWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.FourthWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.FifthWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.LastWeek), GrapeCity.Win.Input.WeekFlags)), New GrapeCity.Win.Input.DayOfWeekStyle("月", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("火", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("水", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("木", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("金", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("土", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Blue, False, False), CType((((((GrapeCity.Win.Input.WeekFlags.FirstWeek Or GrapeCity.Win.Input.WeekFlags.SecondWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.ThirdWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.FourthWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.FifthWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.LastWeek), GrapeCity.Win.Input.WeekFlags)))
        Me.datSeikyuDate.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.datSeikyuDate.Location = New System.Drawing.Point(839, 90)
        Me.datSeikyuDate.MaxDate = New GrapeCity.Win.Input.DateTimeEx(New Date(9999, 12, 31, 23, 59, 59, 0))
        Me.datSeikyuDate.MinDate = New GrapeCity.Win.Input.DateTimeEx(New Date(1970, 1, 1, 0, 0, 0, 0))
        Me.datSeikyuDate.Name = "datSeikyuDate"
        Me.datSeikyuDate.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "F5", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.Now, CType((GrapeCity.Win.Input.KeyActions.NextControl Or GrapeCity.Win.Input.KeyActions.NextField), GrapeCity.Win.Input.KeyActions), CType((GrapeCity.Win.Input.KeyActions.PreviousControl Or GrapeCity.Win.Input.KeyActions.PreviousField), GrapeCity.Win.Input.KeyActions)})
        Me.datSeikyuDate.Size = New System.Drawing.Size(156, 25)
        Me.datSeikyuDate.Spin = New GrapeCity.Win.Input.Spin(0, 1, True, True, GrapeCity.Win.Input.ButtonPosition.Inside, False, GrapeCity.Win.Input.Visibility.NotShown, System.Windows.Forms.FlatStyle.System)
        Me.datSeikyuDate.TabIndex = 13
        Me.datSeikyuDate.Value = New GrapeCity.Win.Input.DateTimeEx(New Date(2017, 1, 1, 0, 0, 0, 0))
        '
        'datNouhinDate
        '
        Me.datNouhinDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.datNouhinDate.DisplayFormat = New GrapeCity.Win.Input.DateDisplayFormat("yyyy年 M月 d日", "", "")
        Me.datNouhinDate.DropDownCalendar.Size = New System.Drawing.Size(200, 223)
        Me.datNouhinDate.DropDownCalendar.Weekdays = New GrapeCity.Win.Input.WeekdaysStyle(New GrapeCity.Win.Input.DayOfWeekStyle("日", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Red, False, False), CType((((((GrapeCity.Win.Input.WeekFlags.FirstWeek Or GrapeCity.Win.Input.WeekFlags.SecondWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.ThirdWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.FourthWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.FifthWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.LastWeek), GrapeCity.Win.Input.WeekFlags)), New GrapeCity.Win.Input.DayOfWeekStyle("月", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("火", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("水", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("木", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("金", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.SystemColors.WindowText, False, False), GrapeCity.Win.Input.WeekFlags.None), New GrapeCity.Win.Input.DayOfWeekStyle("土", GrapeCity.Win.Input.ReflectTitle.None, New GrapeCity.Win.Input.SubStyle(System.Drawing.SystemColors.Window, System.Drawing.Color.Blue, False, False), CType((((((GrapeCity.Win.Input.WeekFlags.FirstWeek Or GrapeCity.Win.Input.WeekFlags.SecondWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.ThirdWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.FourthWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.FifthWeek) _
                    Or GrapeCity.Win.Input.WeekFlags.LastWeek), GrapeCity.Win.Input.WeekFlags)))
        Me.datNouhinDate.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.datNouhinDate.Location = New System.Drawing.Point(839, 63)
        Me.datNouhinDate.MaxDate = New GrapeCity.Win.Input.DateTimeEx(New Date(9999, 12, 31, 23, 59, 59, 0))
        Me.datNouhinDate.MinDate = New GrapeCity.Win.Input.DateTimeEx(New Date(1970, 1, 1, 0, 0, 0, 0))
        Me.datNouhinDate.Name = "datNouhinDate"
        Me.datNouhinDate.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "F5", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.Now, CType((GrapeCity.Win.Input.KeyActions.NextControl Or GrapeCity.Win.Input.KeyActions.NextField), GrapeCity.Win.Input.KeyActions), CType((GrapeCity.Win.Input.KeyActions.PreviousControl Or GrapeCity.Win.Input.KeyActions.PreviousField), GrapeCity.Win.Input.KeyActions)})
        Me.datNouhinDate.Size = New System.Drawing.Size(156, 25)
        Me.datNouhinDate.Spin = New GrapeCity.Win.Input.Spin(0, 1, True, True, GrapeCity.Win.Input.ButtonPosition.Inside, False, GrapeCity.Win.Input.Visibility.NotShown, System.Windows.Forms.FlatStyle.System)
        Me.datNouhinDate.TabIndex = 12
        Me.datNouhinDate.Value = New GrapeCity.Win.Input.DateTimeEx(New Date(2017, 1, 1, 0, 0, 0, 0))
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label5.Location = New System.Drawing.Point(5, 5)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(25, 137)
        Me.Label5.TabIndex = 161
        Me.Label5.Text = "得意先"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkKariDen
        '
        Me.chkKariDen.AutoSize = True
        Me.chkKariDen.Location = New System.Drawing.Point(1014, 123)
        Me.chkKariDen.Name = "chkKariDen"
        Me.chkKariDen.Size = New System.Drawing.Size(73, 23)
        Me.chkKariDen.TabIndex = 15
        Me.chkKariDen.TabStop = False
        Me.chkKariDen.Text = "仮伝票"
        Me.chkKariDen.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label9.Location = New System.Drawing.Point(353, 5)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(25, 76)
        Me.Label9.TabIndex = 164
        Me.Label9.Text = "納入先"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTekiyou
        '
        Me.lblTekiyou.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTekiyou.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblTekiyou.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTekiyou.Location = New System.Drawing.Point(5, 512)
        Me.lblTekiyou.Name = "lblTekiyou"
        Me.lblTekiyou.Size = New System.Drawing.Size(90, 25)
        Me.lblTekiyou.TabIndex = 17
        Me.lblTekiyou.Text = "摘要(&0)"
        Me.lblTekiyou.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'numArariKei
        '
        Me.numArariKei.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.numArariKei.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.numArariKei.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.numArariKei.DisplayFormat = New GrapeCity.Win.Input.NumberDisplayFormat("#,###,###,###,###,##0", "", "", "-", "", "", "Null")
        Me.numArariKei.DropDown = New GrapeCity.Win.Input.DropDown(GrapeCity.Win.Input.ButtonPosition.Inside, True, GrapeCity.Win.Input.Visibility.NotShown, System.Windows.Forms.FlatStyle.System)
        Me.numArariKei.Format = New GrapeCity.Win.Input.NumberFormat("#,###,###,###,###,##0", "", "", "-", "", "", "")
        Me.numArariKei.Location = New System.Drawing.Point(323, 506)
        Me.numArariKei.MaxValue = New Decimal(New Integer() {1874919423, 2328306, 0, 0})
        Me.numArariKei.MinValue = New Decimal(New Integer() {1874919423, 2328306, 0, -2147483648})
        Me.numArariKei.Name = "numArariKei"
        Me.numArariKei.ReadOnly = True
        Me.numArariKei.Size = New System.Drawing.Size(135, 25)
        Me.numArariKei.TabIndex = 184
        Me.numArariKei.TabStop = False
        Me.numArariKei.TextMargins = New GrapeCity.Win.Input.Margins(1, 3, 1, 1)
        Me.numArariKei.Value = New Decimal(New Integer() {0, 0, 0, 0})
        '
        'edtTekiyou
        '
        Me.edtTekiyou.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.edtTekiyou.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtTekiyou.HighlightText = True
        Me.edtTekiyou.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.edtTekiyou.Location = New System.Drawing.Point(5, 536)
        Me.edtTekiyou.MaxLength = 255
        Me.edtTekiyou.Multiline = True
        Me.edtTekiyou.Name = "edtTekiyou"
        Me.edtTekiyou.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.edtTekiyou.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtTekiyou.Size = New System.Drawing.Size(340, 43)
        Me.edtTekiyou.TabIndex = 18
        '
        'MRowSheet
        '
        Me.MRowSheet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MRowSheet.Data = CType(resources.GetObject("MRowSheet.Data"), GrapeCity.Win.ElTabelle.TemplateData)
        Me.MRowSheet.Location = New System.Drawing.Point(5, 155)
        Me.MRowSheet.MouseWheelAction = GrapeCity.Win.ElTabelle.MouseWheelAction.ScrollOnSheet
        Me.MRowSheet.Name = "MRowSheet"
        Me.MRowSheet.Size = New System.Drawing.Size(1107, 352)
        Me.MRowSheet.TabIndex = 16
        Me.MRowSheet.Text = "MRowSheet"
        '
        'lblTokuiName
        '
        Me.lblTokuiName.AutoEllipsis = True
        Me.lblTokuiName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTokuiName.Location = New System.Drawing.Point(99, 32)
        Me.lblTokuiName.Name = "lblTokuiName"
        Me.lblTokuiName.Size = New System.Drawing.Size(245, 25)
        Me.lblTokuiName.TabIndex = 139
        Me.lblTokuiName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTantouName
        '
        Me.lblTantouName.AutoEllipsis = True
        Me.lblTantouName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTantouName.Location = New System.Drawing.Point(586, 120)
        Me.lblTantouName.Name = "lblTantouName"
        Me.lblTantouName.Size = New System.Drawing.Size(165, 25)
        Me.lblTantouName.TabIndex = 140
        Me.lblTantouName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSearchTantou
        '
        Me.btnSearchTantou.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnSearchTantou.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSearchTantou.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnSearchTantou.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnSearchTantou.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchTantou.ForeColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSearchTantou.Image = Global.販売管理C.My.Resources.Resources.search_3_16_307EA9
        Me.btnSearchTantou.Location = New System.Drawing.Point(547, 120)
        Me.btnSearchTantou.Name = "btnSearchTantou"
        Me.btnSearchTantou.Size = New System.Drawing.Size(35, 25)
        Me.btnSearchTantou.TabIndex = 10
        Me.btnSearchTantou.TabStop = False
        Me.btnSearchTantou.UseVisualStyleBackColor = False
        '
        'edtTantouCode
        '
        Me.edtTantouCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtTantouCode.HighlightText = True
        Me.edtTantouCode.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.edtTantouCode.Location = New System.Drawing.Point(447, 120)
        Me.edtTantouCode.Name = "edtTantouCode"
        Me.edtTantouCode.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtTantouCode.Size = New System.Drawing.Size(96, 25)
        Me.edtTantouCode.TabIndex = 9
        Me.edtTantouCode.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        '
        'lblTantouCode
        '
        Me.lblTantouCode.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblTantouCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTantouCode.Location = New System.Drawing.Point(353, 120)
        Me.lblTantouCode.Name = "lblTantouCode"
        Me.lblTantouCode.Size = New System.Drawing.Size(92, 25)
        Me.lblTantouCode.TabIndex = 168
        Me.lblTantouCode.Text = "担当者"
        Me.lblTantouCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSoukoName
        '
        Me.lblSoukoName.AutoEllipsis = True
        Me.lblSoukoName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSoukoName.Location = New System.Drawing.Point(586, 88)
        Me.lblSoukoName.Name = "lblSoukoName"
        Me.lblSoukoName.Size = New System.Drawing.Size(164, 25)
        Me.lblSoukoName.TabIndex = 157
        Me.lblSoukoName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSearchSouko
        '
        Me.btnSearchSouko.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnSearchSouko.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSearchSouko.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnSearchSouko.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnSearchSouko.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchSouko.ForeColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSearchSouko.Image = Global.販売管理C.My.Resources.Resources.search_3_16_307EA9
        Me.btnSearchSouko.Location = New System.Drawing.Point(547, 88)
        Me.btnSearchSouko.Name = "btnSearchSouko"
        Me.btnSearchSouko.Size = New System.Drawing.Size(35, 25)
        Me.btnSearchSouko.TabIndex = 8
        Me.btnSearchSouko.TabStop = False
        Me.btnSearchSouko.UseVisualStyleBackColor = False
        '
        'edtSoukoCode
        '
        Me.edtSoukoCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtSoukoCode.HighlightText = True
        Me.edtSoukoCode.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.edtSoukoCode.Location = New System.Drawing.Point(447, 88)
        Me.edtSoukoCode.Name = "edtSoukoCode"
        Me.edtSoukoCode.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtSoukoCode.Size = New System.Drawing.Size(96, 25)
        Me.edtSoukoCode.TabIndex = 7
        Me.edtSoukoCode.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        '
        'lblSoukoCode
        '
        Me.lblSoukoCode.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblSoukoCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSoukoCode.Location = New System.Drawing.Point(353, 88)
        Me.lblSoukoCode.Name = "lblSoukoCode"
        Me.lblSoukoCode.Size = New System.Drawing.Size(92, 25)
        Me.lblSoukoCode.TabIndex = 165
        Me.lblSoukoCode.Text = "倉庫"
        Me.lblSoukoCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'edtNounyuuName2
        '
        Me.edtNounyuuName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtNounyuuName2.DisabledForeColor = System.Drawing.SystemColors.WindowText
        Me.edtNounyuuName2.Ellipsis = GrapeCity.Win.Input.EllipsisMode.EllipsisEnd
        Me.edtNounyuuName2.HighlightText = True
        Me.edtNounyuuName2.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.edtNounyuuName2.Location = New System.Drawing.Point(447, 56)
        Me.edtNounyuuName2.Name = "edtNounyuuName2"
        Me.edtNounyuuName2.OverflowTip = True
        Me.edtNounyuuName2.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtNounyuuName2.Size = New System.Drawing.Size(245, 25)
        Me.edtNounyuuName2.TabIndex = 5
        Me.edtNounyuuName2.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        '
        'edtNounyuuName
        '
        Me.edtNounyuuName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtNounyuuName.DisabledForeColor = System.Drawing.SystemColors.WindowText
        Me.edtNounyuuName.Ellipsis = GrapeCity.Win.Input.EllipsisMode.EllipsisEnd
        Me.edtNounyuuName.HighlightText = True
        Me.edtNounyuuName.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.edtNounyuuName.Location = New System.Drawing.Point(447, 32)
        Me.edtNounyuuName.Name = "edtNounyuuName"
        Me.edtNounyuuName.OverflowTip = True
        Me.edtNounyuuName.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtNounyuuName.Size = New System.Drawing.Size(245, 25)
        Me.edtNounyuuName.TabIndex = 4
        Me.edtNounyuuName.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        '
        'cmbNounyuuKeisho
        '
        Me.cmbNounyuuKeisho.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.cmbNounyuuKeisho.DisabledForeColor = System.Drawing.SystemColors.WindowText
        Me.cmbNounyuuKeisho.HighlightText = GrapeCity.Win.Input.HighlightText.All
        Me.cmbNounyuuKeisho.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.cmbNounyuuKeisho.Location = New System.Drawing.Point(695, 56)
        Me.cmbNounyuuKeisho.Name = "cmbNounyuuKeisho"
        Me.cmbNounyuuKeisho.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.cmbNounyuuKeisho.Size = New System.Drawing.Size(55, 25)
        Me.cmbNounyuuKeisho.TabIndex = 6
        Me.cmbNounyuuKeisho.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        Me.cmbNounyuuKeisho.Value = ""
        '
        'lblNounyuuCode
        '
        Me.lblNounyuuCode.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblNounyuuCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblNounyuuCode.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblNounyuuCode.Location = New System.Drawing.Point(377, 5)
        Me.lblNounyuuCode.Name = "lblNounyuuCode"
        Me.lblNounyuuCode.Size = New System.Drawing.Size(68, 25)
        Me.lblNounyuuCode.TabIndex = 162
        Me.lblNounyuuCode.Text = "コード/ｶﾅ"
        Me.lblNounyuuCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label12.Location = New System.Drawing.Point(377, 32)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(68, 49)
        Me.Label12.TabIndex = 163
        Me.Label12.Text = "名称"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSearchNounyu
        '
        Me.btnSearchNounyu.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnSearchNounyu.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSearchNounyu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnSearchNounyu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnSearchNounyu.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchNounyu.ForeColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSearchNounyu.Image = Global.販売管理C.My.Resources.Resources.search_3_16_307EA9
        Me.btnSearchNounyu.Location = New System.Drawing.Point(547, 5)
        Me.btnSearchNounyu.Name = "btnSearchNounyu"
        Me.btnSearchNounyu.Size = New System.Drawing.Size(35, 25)
        Me.btnSearchNounyu.TabIndex = 3
        Me.btnSearchNounyu.TabStop = False
        Me.btnSearchNounyu.UseVisualStyleBackColor = False
        '
        'edtNounyuuCode
        '
        Me.edtNounyuuCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtNounyuuCode.HighlightText = True
        Me.edtNounyuuCode.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.edtNounyuuCode.Location = New System.Drawing.Point(447, 5)
        Me.edtNounyuuCode.Name = "edtNounyuuCode"
        Me.edtNounyuuCode.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtNounyuuCode.Size = New System.Drawing.Size(96, 25)
        Me.edtNounyuuCode.TabIndex = 2
        Me.edtNounyuuCode.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        '
        'lblTokuiCode
        '
        Me.lblTokuiCode.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblTokuiCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTokuiCode.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblTokuiCode.Location = New System.Drawing.Point(29, 5)
        Me.lblTokuiCode.Name = "lblTokuiCode"
        Me.lblTokuiCode.Size = New System.Drawing.Size(68, 25)
        Me.lblTokuiCode.TabIndex = 126
        Me.lblTokuiCode.Text = "コード/ｶﾅ"
        Me.lblTokuiCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTokuiZeiKubun
        '
        Me.lblTokuiZeiKubun.AutoSize = True
        Me.lblTokuiZeiKubun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTokuiZeiKubun.Location = New System.Drawing.Point(239, 7)
        Me.lblTokuiZeiKubun.Name = "lblTokuiZeiKubun"
        Me.lblTokuiZeiKubun.Size = New System.Drawing.Size(103, 21)
        Me.lblTokuiZeiKubun.TabIndex = 151
        Me.lblTokuiZeiKubun.Text = "外税 / 請求時"
        Me.lblTokuiZeiKubun.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTokuiBikou
        '
        Me.lblTokuiBikou.AutoEllipsis = True
        Me.lblTokuiBikou.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTokuiBikou.Location = New System.Drawing.Point(99, 83)
        Me.lblTokuiBikou.Name = "lblTokuiBikou"
        Me.lblTokuiBikou.Size = New System.Drawing.Size(245, 59)
        Me.lblTokuiBikou.TabIndex = 147
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label8.Location = New System.Drawing.Point(29, 83)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 59)
        Me.Label8.TabIndex = 149
        Me.Label8.Text = "備考"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.Location = New System.Drawing.Point(29, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 49)
        Me.Label2.TabIndex = 141
        Me.Label2.Text = "名称"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTokuiName2
        '
        Me.lblTokuiName2.AutoEllipsis = True
        Me.lblTokuiName2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTokuiName2.Location = New System.Drawing.Point(99, 56)
        Me.lblTokuiName2.Name = "lblTokuiName2"
        Me.lblTokuiName2.Size = New System.Drawing.Size(245, 25)
        Me.lblTokuiName2.TabIndex = 146
        Me.lblTokuiName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSearchTokui
        '
        Me.btnSearchTokui.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnSearchTokui.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSearchTokui.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnSearchTokui.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.btnSearchTokui.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchTokui.ForeColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.btnSearchTokui.Image = Global.販売管理C.My.Resources.Resources.search_3_16_307EA9
        Me.btnSearchTokui.Location = New System.Drawing.Point(199, 5)
        Me.btnSearchTokui.Name = "btnSearchTokui"
        Me.btnSearchTokui.Size = New System.Drawing.Size(35, 25)
        Me.btnSearchTokui.TabIndex = 1
        Me.btnSearchTokui.TabStop = False
        Me.btnSearchTokui.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.Location = New System.Drawing.Point(759, 5)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(78, 25)
        Me.Label7.TabIndex = 144
        Me.Label7.Text = "伝票コード"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'edtTokuiCode
        '
        Me.edtTokuiCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtTokuiCode.HighlightText = True
        Me.edtTokuiCode.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.edtTokuiCode.Location = New System.Drawing.Point(99, 5)
        Me.edtTokuiCode.Name = "edtTokuiCode"
        Me.edtTokuiCode.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtTokuiCode.Size = New System.Drawing.Size(96, 25)
        Me.edtTokuiCode.TabIndex = 0
        Me.edtTokuiCode.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        '
        'edtDenpyouCode
        '
        Me.edtDenpyouCode.AllowSpace = GrapeCity.Win.Input.AllowSpace.None
        Me.edtDenpyouCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.edtDenpyouCode.Format = "H"
        Me.edtDenpyouCode.HighlightText = True
        Me.edtDenpyouCode.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.edtDenpyouCode.Location = New System.Drawing.Point(839, 5)
        Me.edtDenpyouCode.Name = "edtDenpyouCode"
        Me.edtDenpyouCode.Shortcuts = New GrapeCity.Win.Input.ShortcutCollection(New String() {"F2", "Enter", "Shift+Enter"}, New GrapeCity.Win.Input.KeyActions() {GrapeCity.Win.Input.KeyActions.Clear, GrapeCity.Win.Input.KeyActions.NextControl, GrapeCity.Win.Input.KeyActions.PreviousControl})
        Me.edtDenpyouCode.Size = New System.Drawing.Size(96, 25)
        Me.edtDenpyouCode.TabIndex = 11
        Me.edtDenpyouCode.TextVAlign = GrapeCity.Win.Input.AlignVertical.Middle
        '
        'lblNouhinDate
        '
        Me.lblNouhinDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblNouhinDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblNouhinDate.Location = New System.Drawing.Point(759, 63)
        Me.lblNouhinDate.Name = "lblNouhinDate"
        Me.lblNouhinDate.Size = New System.Drawing.Size(78, 25)
        Me.lblNouhinDate.TabIndex = 148
        Me.lblNouhinDate.Text = "納品日"
        Me.lblNouhinDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSeikyuDate
        '
        Me.lblSeikyuDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblSeikyuDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSeikyuDate.Location = New System.Drawing.Point(759, 90)
        Me.lblSeikyuDate.Name = "lblSeikyuDate"
        Me.lblSeikyuDate.Size = New System.Drawing.Size(78, 25)
        Me.lblSeikyuDate.TabIndex = 154
        Me.lblSeikyuDate.Text = "請求日"
        Me.lblSeikyuDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblReferenceCode
        '
        Me.lblReferenceCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblReferenceCode.Location = New System.Drawing.Point(839, 32)
        Me.lblReferenceCode.Name = "lblReferenceCode"
        Me.lblReferenceCode.Size = New System.Drawing.Size(96, 25)
        Me.lblReferenceCode.TabIndex = 159
        Me.lblReferenceCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.Label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label15.Location = New System.Drawing.Point(759, 122)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(78, 25)
        Me.Label15.TabIndex = 155
        Me.Label15.Text = "区分"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblReferenceCodeTitle
        '
        Me.lblReferenceCodeTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(194, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblReferenceCodeTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblReferenceCodeTitle.Location = New System.Drawing.Point(759, 32)
        Me.lblReferenceCodeTitle.Name = "lblReferenceCodeTitle"
        Me.lblReferenceCodeTitle.Size = New System.Drawing.Size(78, 25)
        Me.lblReferenceCodeTitle.TabIndex = 158
        Me.lblReferenceCodeTitle.Text = "参照コード"
        Me.lblReferenceCodeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRendouSaki
        '
        Me.lblRendouSaki.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRendouSaki.AutoEllipsis = True
        Me.lblRendouSaki.Location = New System.Drawing.Point(938, 8)
        Me.lblRendouSaki.Name = "lblRendouSaki"
        Me.lblRendouSaki.Size = New System.Drawing.Size(169, 19)
        Me.lblRendouSaki.TabIndex = 207
        Me.lblRendouSaki.Text = "連動先:仕入000001"
        '
        'PanelTitle
        '
        Me.PanelTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(169, Byte), Integer))
        Me.PanelTitle.Controls.Add(Me.lblSeikyuZumi)
        Me.PanelTitle.Controls.Add(Me.picInfo)
        Me.PanelTitle.Controls.Add(Me.lblShusei)
        Me.PanelTitle.Controls.Add(Me.lblTitle)
        Me.PanelTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelTitle.ForeColor = System.Drawing.Color.White
        Me.PanelTitle.Location = New System.Drawing.Point(0, 0)
        Me.PanelTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.PanelTitle.Name = "PanelTitle"
        Me.PanelTitle.Size = New System.Drawing.Size(1122, 27)
        Me.PanelTitle.TabIndex = 6
        '
        'lblSeikyuZumi
        '
        Me.lblSeikyuZumi.AutoSize = True
        Me.lblSeikyuZumi.BackColor = System.Drawing.Color.Crimson
        Me.lblSeikyuZumi.Font = New System.Drawing.Font("Meiryo UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSeikyuZumi.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.lblSeikyuZumi.Location = New System.Drawing.Point(273, 2)
        Me.lblSeikyuZumi.Name = "lblSeikyuZumi"
        Me.lblSeikyuZumi.Padding = New System.Windows.Forms.Padding(2)
        Me.lblSeikyuZumi.Size = New System.Drawing.Size(93, 24)
        Me.lblSeikyuZumi.TabIndex = 186
        Me.lblSeikyuZumi.Text = "請求書確認"
        '
        'picInfo
        '
        Me.picInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picInfo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picInfo.Image = Global.販売管理C.My.Resources.Resources.info_16_FFFFFF
        Me.picInfo.Location = New System.Drawing.Point(988, 6)
        Me.picInfo.Name = "picInfo"
        Me.picInfo.Size = New System.Drawing.Size(16, 16)
        Me.picInfo.TabIndex = 95
        Me.picInfo.TabStop = False
        '
        'lblShusei
        '
        Me.lblShusei.AutoSize = True
        Me.lblShusei.BackColor = System.Drawing.Color.LightPink
        Me.lblShusei.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblShusei.Location = New System.Drawing.Point(172, 3)
        Me.lblShusei.Name = "lblShusei"
        Me.lblShusei.Padding = New System.Windows.Forms.Padding(2, 1, 2, 1)
        Me.lblShusei.Size = New System.Drawing.Size(53, 21)
        Me.lblShusei.TabIndex = 94
        Me.lblShusei.Text = "修　正"
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Left
        Me.lblTitle.Font = New System.Drawing.Font("Meiryo UI", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.lblTitle.Size = New System.Drawing.Size(166, 27)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "■ 納品伝票"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFormPrinter
        '
        Me.lblFormPrinter.AutoEllipsis = True
        Me.lblFormPrinter.AutoSize = True
        Me.lblFormPrinter.BackColor = System.Drawing.SystemColors.Info
        Me.lblFormPrinter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFormPrinter.Location = New System.Drawing.Point(556, 2)
        Me.lblFormPrinter.Name = "lblFormPrinter"
        Me.lblFormPrinter.Padding = New System.Windows.Forms.Padding(2)
        Me.lblFormPrinter.Size = New System.Drawing.Size(130, 23)
        Me.lblFormPrinter.TabIndex = 118
        Me.lblFormPrinter.Text = "◇フォーム / プリンタ"
        Me.lblFormPrinter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'frmNouhin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1122, 711)
        Me.Controls.Add(Me.lblFormPrinter)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Meiryo UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmNouhin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmNouhin"
        Me.ToolStripMenu.ResumeLayout(False)
        Me.ToolStripMenu.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.TableLayoutPanelBase.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.edtLblTadasiGaki, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.edtTadasiGaki, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbUriageKubun, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sheetGoukei, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.datSeikyuDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.datNouhinDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numArariKei, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.edtTekiyou, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MRowSheet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.edtTantouCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.edtSoukoCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.edtNounyuuName2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.edtNounyuuName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbNounyuuKeisho, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.edtNounyuuCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.edtTokuiCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.edtDenpyouCode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelTitle.ResumeLayout(False)
        Me.PanelTitle.PerformLayout()
        CType(Me.picInfo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStripContainer1 As ToolStripContainer
    Friend WithEvents TableLayoutPanelBase As TableLayoutPanel
    Friend WithEvents toolBtnNew As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents toolBtnNextNew As ToolStripButton
    Friend WithEvents toolBtnInsertRow As ToolStripButton
    Friend WithEvents toolBtnDeleteRow As ToolStripButton
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents toolBtnDelete As ToolStripButton
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents toolBtnSearch As ToolStripButton
    Friend WithEvents toolBtnPrint As ToolStripButton
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents toolBtnPreview As ToolStripButton
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents toolBtnEnd As ToolStripButton
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents toolBtnSearchCopy As ToolStripButton
    Friend WithEvents toolBtnExpandMeisai As ToolStripButton
    Friend WithEvents ToolStripSeparator12 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator11 As ToolStripSeparator
    Friend WithEvents toolBtnUpdate As ToolStripButton
    Friend WithEvents ToolStripMenu As MyControls.ToolStripEx
    Friend WithEvents PanelTitle As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents mnuFile As ToolStripMenuItem
    Friend WithEvents mnuUpdate As ToolStripMenuItem
    Friend WithEvents mnuDelete As ToolStripMenuItem
    Friend WithEvents mnuEnd As ToolStripMenuItem
    Friend WithEvents mnuEdit As ToolStripMenuItem
    Friend WithEvents mnuInsertRow As ToolStripMenuItem
    Friend WithEvents mnuDeleteRow As ToolStripMenuItem
    Friend WithEvents mnuSearchMenu As ToolStripMenuItem
    Friend WithEvents mnuEnvironment As ToolStripMenuItem
    Friend WithEvents mnuEnvTantousha As ToolStripMenuItem
    Friend WithEvents mnuEnvTokuisaki As ToolStripMenuItem
    Friend WithEvents mnuEnvNounyuuSaki As ToolStripMenuItem
    Friend WithEvents mnuEnvSiiresaki As ToolStripMenuItem
    Friend WithEvents mnuEnvShouhin As ToolStripMenuItem
    Friend WithEvents mnuPrintMenu As ToolStripMenuItem
    Friend WithEvents mnuLblForm As ToolStripMenuItem
    Friend WithEvents mnuCmbForm As ToolStripComboBox
    Friend WithEvents mnuLblPrinter As ToolStripMenuItem
    Friend WithEvents mnuCmbPrinter As ToolStripComboBox
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents mnuPrint As ToolStripMenuItem
    Friend WithEvents mnuPreview As ToolStripMenuItem
    Friend WithEvents lblShusei As Label
    Friend WithEvents lblFormPrinter As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblTekiyou As Label
    Friend WithEvents edtLblTadasiGaki As GrapeCity.Win.Input.Edit
    Friend WithEvents edtTadasiGaki As GrapeCity.Win.Input.Edit
    Friend WithEvents numArariKei As GrapeCity.Win.Input.Number
    Friend WithEvents edtTekiyou As GrapeCity.Win.Input.Edit
    Friend WithEvents MRowSheet As GrapeCity.Win.ElTabelle.MultiRowSheet
    Friend WithEvents lblTokuiName As Label
    Friend WithEvents lblTantouName As Label
    Friend WithEvents btnSearchTantou As Button
    Friend WithEvents edtTantouCode As GrapeCity.Win.Input.Edit
    Friend WithEvents lblTantouCode As Label
    Friend WithEvents lblSoukoName As Label
    Friend WithEvents btnSearchSouko As Button
    Friend WithEvents edtSoukoCode As GrapeCity.Win.Input.Edit
    Friend WithEvents lblSoukoCode As Label
    Friend WithEvents edtNounyuuName2 As GrapeCity.Win.Input.Edit
    Friend WithEvents edtNounyuuName As GrapeCity.Win.Input.Edit
    Friend WithEvents Label9 As Label
    Friend WithEvents cmbNounyuuKeisho As GrapeCity.Win.Input.Combo
    Friend WithEvents lblNounyuuCode As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents btnSearchNounyu As Button
    Friend WithEvents edtNounyuuCode As GrapeCity.Win.Input.Edit
    Friend WithEvents Label5 As Label
    Friend WithEvents lblTokuiCode As Label
    Friend WithEvents lblTokuiZeiKubun As Label
    Friend WithEvents lblTokuiBikou As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblTokuiName2 As Label
    Friend WithEvents btnSearchTokui As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents edtTokuiCode As GrapeCity.Win.Input.Edit
    Friend WithEvents edtDenpyouCode As GrapeCity.Win.Input.Edit
    Friend WithEvents lblNouhinDate As Label
    Friend WithEvents lblSeikyuDate As Label
    Friend WithEvents lblReferenceCode As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents lblReferenceCodeTitle As Label
    Friend WithEvents chkKariDen As CheckBox
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents datNouhinDate As GrapeCity.Win.Input.Date
    Friend WithEvents datSeikyuDate As GrapeCity.Win.Input.Date
    Friend WithEvents toolBtnTankaRireki As ToolStripButton
    Friend WithEvents ToolStripSeparator13 As ToolStripSeparator
    Friend WithEvents toolBtnMitumori As ToolStripButton
    Friend WithEvents ToolStripSeparator14 As ToolStripSeparator
    Friend WithEvents toolBtnJutyu As ToolStripButton
    Friend WithEvents toolSepaJutyu As ToolStripSeparator
    Friend WithEvents toolBtnSiire As ToolStripButton
    Friend WithEvents ToolStripSeparator17 As ToolStripSeparator
    Friend WithEvents picInfo As PictureBox
    Friend WithEvents toolBtnCopyNew As ToolStripButton
    Friend WithEvents ToolStripSeparator15 As ToolStripSeparator
    Friend WithEvents mnuNew As ToolStripMenuItem
    Friend WithEvents mnuNextNew As ToolStripMenuItem
    Friend WithEvents mnuSearch As ToolStripMenuItem
    Friend WithEvents mnuSearchCopy As ToolStripMenuItem
    Friend WithEvents mnuMitumori As ToolStripMenuItem
    Friend WithEvents mnuJutyu As ToolStripMenuItem
    Friend WithEvents mnuSiire As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator16 As ToolStripSeparator
    Friend WithEvents mnuCopyNew As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator19 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator18 As ToolStripSeparator
    Friend WithEvents mnuTankaRireki As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator22 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator20 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator23 As ToolStripSeparator
    Friend WithEvents mnuExpandMeisai As ToolStripMenuItem
    Friend WithEvents toolBtnExport As ToolStripButton
    Friend WithEvents ToolStripSeparator21 As ToolStripSeparator
    Friend WithEvents mnuExport As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator24 As ToolStripSeparator
    Friend WithEvents lblRendouSaki As Label
    Friend WithEvents mnuSepa1RyoushuSho As ToolStripSeparator
    Friend WithEvents mnuSepa2RyoushuSho As ToolStripSeparator
    Friend WithEvents mnuLblFormRyoushuSho As ToolStripMenuItem
    Friend WithEvents mnuCmbFormRyoushuSho As ToolStripComboBox
    Friend WithEvents mnuLblPrinterRyoushuSho As ToolStripMenuItem
    Friend WithEvents mnuCmbPrinterRyoushuSho As ToolStripComboBox
    Friend WithEvents mnuSepa3RyoushuSho As ToolStripSeparator
    Friend WithEvents mnuPrintRyoushuSho As ToolStripMenuItem
    Friend WithEvents mnuPreviewRyoushuSho As ToolStripMenuItem
    Friend WithEvents mnuSepaResetForm As ToolStripSeparator
    Friend WithEvents mnuResetForm As ToolStripMenuItem
    Friend WithEvents btnClearReferenceCode As Button
    Friend WithEvents sheetGoukei As GrapeCity.Win.ElTabelle.Sheet
    Friend WithEvents Label3 As Label
    Friend WithEvents lblSeikyuZumi As Label
    Friend WithEvents cmbUriageKubun As GrapeCity.Win.Input.Combo
    Friend WithEvents lblUriageKubun As Label
    Friend WithEvents ToolStripSeparator25 As ToolStripSeparator
    Friend WithEvents mnuRowUp As ToolStripMenuItem
    Friend WithEvents mnuRowDown As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator26 As ToolStripSeparator
    Friend WithEvents toolBtnCopyRow As ToolStripButton
    Friend WithEvents mnuCopyRow As ToolStripMenuItem
    Friend WithEvents btnPrintRyoushuSho As Button
    Friend WithEvents btnPreviewRyoushuSho As Button
End Class
