''' <summary>
''' 納品伝票【V5.1用】
''' </summary>
''' <remarks></remarks>
Public Class frmNouhin

    '引数
    Private isWhenCopy As Boolean  '「コピー」の時True

    Private dsDenpyou As DataSet
    Private daDenpyou As SqlDataAdapter
    Private dtDenpyou As DataTable
    Private daMeisai As SqlDataAdapter
    Private dtMeisai As DataTable
    Private iMeisaiCnt As Integer  '伝票検索時の明細件数をHold

    Private drJisha As DataRow

    Private defaultSoukoNo As Integer  'デフォルト倉庫がある場合、デフォルトの倉庫マスタNo

    Private defaultShouhin As strctDefaultShouhin   '商品コード未入力時のデフォルトの商品
    Private Structure strctDefaultShouhin
        Dim MasterNo As Integer
        Dim Code As String
        Dim ZeiKubun As Byte
        Dim TaxRateKubun As Byte
    End Structure

    Private Denpyou As DenpyouDefine

    Private MaxKoumoku As Integer = 7

    '*信和* 領収書フォルダと領収書フォームのデフォルト
    Private RyoushuShoFolder As String = CSingleton.CSetting.PrintFormFolder & "\納品伝票"
    Private RyoushuShoForm As String = "\信和領収証.hzf"


    '変更入力されたかどうかの判定に使用
    Private isChangedTokuiCode As Boolean    '得意先コードを入力したかどうか
    Private isChangedNounyuuCode As Boolean  '納入先コードを入力したかどうか
    Private isChangedSoukoCode As Boolean    '倉庫コードを入力したかどうか
    Private isChangedTantouCode As Boolean   '担当者コードを入力したかどうか
    Private isChangedNouhinDate As Boolean   '納品日を入力したかどうか
    Private isChangedSeikyuDate As Boolean   '請求日を入力したかどうか
    Private isChangedUriageKubun As Boolean  '売上区分を入力したかどうか
    Private isChangedJutyuCode As Boolean    '受注コードを入力したかどうか
    Private isChangedShouhinCode As Boolean  '商品コードを入力したかどうか
    Private isChangedShouhinKana As Boolean  '商品名称カナを入力したかどうか

    '変更入力される前の各項目をHold（変更前の値で単価の変更入力があったか判定するため）
    Private oldKakeritu As Decimal
    Private oldZeikubun As Byte
    Private oldHasuu As Short
    Private oldSeikyuMasterNo As Integer

    '複写や見積参照等で消費税率が変わったかどうか判定するためHold
    Private oldAryRate() As Decimal

    Private isSearchedDenpyou As Boolean    '検索した伝票の時（修正する伝票の時）True（複写入力の時False）（旧blFindFlag）
    Private isShowTankaMSG As Boolean       '得意先単価登録メッセージを表示する場合True
    Private isChanged As Boolean            '変更入力するとTrue（旧bUpdate）
    Private isNewInputtable As Boolean      '新規伝票入力が可の時True、不可の時False (旧bInput)
    Private isUpdated As Boolean            '伝票を登録・更新・削除した時True (旧bEdited)
    Private isDeleting As Boolean           '伝票削除中の時True（旧bDelFlg）
    Private isEnd As Boolean                '処理を終了する時True（旧bEnd）
    Private isLeaveErrorNoCheck As Boolean  '各Leaveイベントのエラーチェックを行わないようにする時True (旧bFunctionPress)
    Private isInitialSet As Boolean         '画面の初期設定中はTrue。初期設定が終了するとFalse。

    Private sheetKeyDownCode As Integer     'MRowSheetのKeyDownで押されたKeyCode

    Private defaultUriageKubun As Integer   '売上区分コンボの１行目をデフォルトとする

    '日付のチェックに使用
    Private checkMinDate As Date  '入力可能な最小日付
    Private checkMaxDate As Date  '入力可能な最大日付

    'タイトル
    Private Const TITLE As String = "納品伝票"      'Me.Textに使用

    '売上区分のコード
    Private Const CON_UriageCode As String = "10"
    '売上区分の区分
    Private Enum enUriKubunKubun
        Nebiki = 3
    End Enum

    '検索画面が１つしか表示されないようにするため、フォームのインスタンスを保持
    Private frmNouhinList As frmNouhinList  '伝票検索
    Private frmNouhinListCopy As frmNouhinList  '複写入力
    Private frmMitumoriList As frmMitumoriList  '見積書参照
    Private frmJutyuList As frmJutyuList  '受注伝票参照
    Private frmSiireList As frmSiireList  '仕入伝票参照
    Private frmJutyuListMeisai As frmJutyuList  '受注明細参照

    '明細シートの列(COL)と行(ROW)の設定
    Private Enum enSheetRow
        Row1 = 0  '１行目
        Row2 = 1  '２行目
    End Enum
    Private Enum enSheetCol1  '１行目の列の項目
        受注コード = 0
        商品コード = 1
        商品名称カナ = 2
        商品検索ボタン = 3
        商品税区分 = 4
        入数 = 5
        セット数 = 6
        原価単価 = 7  '入力用
        金額 = 8  '入力用。売上時プラスのみ
        備考 = 9
        テーブルNo = 10  'シートのカーソルを消すために使用
    End Enum
    Private Enum enSheetCol2  '２行目の列の項目
        受注明細検索ボタン = 0
        商品名称 = 1
        消費税率 = 3
        軽減税率 = 4
        数量 = 5
        単位IN = 6  '入力用（手入力値がテーブルとバインドできないため、バインド列と手入力列とに分けた）
        商品単価 = 7  '入力用
    End Enum
    Private LastColumn As Integer = enSheetCol1.備考  '入力最終列

    '合計シートの列(COL)と行(ROW)の設定
    Private Enum enSheetGoukeiCol
        消費税率テキスト = 0
        消費税率 = 1
        税抜額 = 3
        消費税額 = 5
        ダミー列 = 6  '（参考消費税表示時の桁合せ用）
        参考消費税 = 8  '請求時の時
        合計 = 10
    End Enum
    Private Enum enSheetGoukeiRow
        合計行 = 0
        税率開始行 = 1
    End Enum

    '納品伝票の定義
    Private Structure DenpyouDefine
        Dim NewCode As Decimal
        Dim TableNo As Integer
        Dim Code As String                '伝票コード
        Dim Tokuisaki As Tokuisaki        '得意先
        Dim NounyuuSaki As NounyuuSaki    '納入先
        Dim Souko As Souko                '倉庫
        Dim Tantousha As Tantousha        '担当者
        Dim NouhinDate As Date            '納品日
        Dim SeikyuDate As Date            '請求日（処理日）
        Dim UriageKubun As UriageKubun    '売上区分
        Dim Tekiyou As String             '摘要

        Dim TadasiGaki As String          '*信和*　但書
        Dim KariDen As Boolean            '*信和*　仮伝票

        Dim NyukinNo As Integer           '入金伝票No（連動先）
        Dim MitumoriNo As Integer         '見積書No
        Dim MitumoriCode As String        '見積書コード
        Dim JutyuDenpyouNo As Integer     '受注伝票No
        Dim JutyuCode As String           '受注伝票コード
        Dim SiireDenpyouNo As Integer     '仕入伝票No
        Dim SiireCode As String           '仕入伝票コード

        Dim RendouSakiSiireCode As String  '連動先の仕入伝票コード（複数可）
        Dim RendouSakiNyukinCode As String  '連動先の入金伝票コード

        Dim KoumokuSu As Integer
        Dim NouhinDenpyou As String
        Dim aryRate() As Decimal  '消費税率1,2（消費税マスタから請求日を基準に取得）
    End Structure

    '納品伝票で使用する得意先の定義
    Private Structure Tokuisaki
        Dim MasterNo As Integer
        Dim Code As String
        Dim Name As String
        Dim Name2 As String
        Dim Keishou As String
        Dim NameKana As String
        Dim Hyoujun As Boolean
        Dim SeikyuSaki As Integer
        Dim Hasuu As Short
        Dim Kakeritu As Decimal
        Dim ZeiKubun As Byte                '税区分
        Dim ShouhizeiKeisan As Short        '消費税計算方法
        Dim DenpyouCodeUpdate As Boolean    '納品伝票コード自動更新
        Dim DenpyouCodeFlag As Boolean
        Dim Simebi As Short
        Dim Bikou As String
        Dim Shokuchi As Boolean             '諸口フラグ
        Dim YosinGendo As Decimal           '与信限度額
        Dim NounyusakiExist As Boolean      '納入先有無（カーソル移動に使用）
    End Structure

    '納品伝票で使用する納入先の定義
    Private Structure NounyuuSaki
        Dim MasterNo As Integer
        Dim Code As String
        Dim Name As String
        Dim Name2 As String
        Dim Keishou As String
    End Structure

    '納品伝票で使用する倉庫の定義
    Private Structure Souko
        Dim MasterNo As Integer
        Dim Code As String
        Dim Name As String
    End Structure

    '納品伝票で使用する担当者の定義
    Private Structure Tantousha
        Dim MasterNo As Integer
        Dim Code As String
        Dim Name As String
        Dim NameKana As String
    End Structure

    '納品伝票で使用する売上区分の定義
    Private Structure UriageKubun
        Dim MasterNo As Integer    '売上区分マスタNo（納品伝票はこちらを使用）
        Dim Code As String         'コード（売上区分）
        Dim Zougen As Integer      '増減
        Dim Kubun As Byte          '売上区分の区分
    End Structure

    'インフォメーション(i)のバルーンチップ
    Private myBalloonTip As New GrapeCity.Win.Input.BalloonTip
    Private myBalloonTipInfo As New GrapeCity.Win.Input.BalloonTipInfo
    Private mySheetRitchTip As GrapeCity.Win.ElTabelle.RichTip  'MRowSheet用（明細用）

    '伝票入力処理の共通クラス
    Private CDenpyouCommon As New CDenpyouCommon

    'DB共通クラス
    Private CDBCommon As New CDBCommon

    '画面の共通処理設定クラス
    Private CFormCommon As New CFormCommon


    Public Sub New(Optional ByVal argWhenCopy As Boolean = False)

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        Me.Icon = My.Resources.販売管理5_ico

        isWhenCopy = argWhenCopy  '「コピー」で画面を開いたかどうか
    End Sub

    '納品伝票入力画面の表示
    Public Sub DenpyouInput()
        SetCursorWait()
        SheetRedrawOFF()
        Try
            SetInit()  '画面表示の初期設定

            isNewInputtable = True  '新規伝票入力を可とする

            AllNew()  '初期表示状態

            ChangeButton(isNewInputtable, False)
            isEnd = False

            '画面表示
            SheetRedrawON()
            Me.Show()

        Catch ex As Exception
            ErrProc(ex, Me.Text)

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '指定伝票対応処理
    '納品伝票入力画面の表示（指定伝票画面から納品伝票へ、得意先を指定して切り替える）
    Public Sub DenpyouInput(ByVal TokuiNo As Integer)
        SetCursorWait()
        SheetRedrawOFF()
        Try
            SetInit()    '画面表示の初期設定

            isNewInputtable = True  '新規伝票入力を可とする

            AllNew()     '初期表示状態

            '得意先情報をセットして表示
            Dim CTokuisaki As New HanbaikanriDialog.CTokuisaki()
            Dim drTokuisaki As DataRow = CTokuisaki.GetMaster(TokuiNo)
            If drTokuisaki IsNot Nothing Then
                ChangeTokuiCode(True, drTokuisaki)
                MRowSheet.Enabled = True
            End If
            SetForm(False)

            isEnd = False

            '画面表示
            SheetRedrawON()
            Me.Show()
            ControlFocusCode(edtTokuiCode)

        Catch ex As Exception
            ErrProc(ex, Me.Text)

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '納品伝票Noから、納品伝票の修正画面を表示する
    Public Function Edit(ByVal TableNo As Integer) As Boolean
        SetCursorWait()
        SheetRedrawOFF()
        Try
            SetInit()    '画面表示の初期設定

            isNewInputtable = False  '新規伝票入力を不可とする
            isUpdated = False

            '該当の納品伝票データを得る
            If GetRecord(TableNo, False) > 0 Then
                MRowSheet.Enabled = True
                SetForm(True, False, False)
                Me.Text = TITLE & "（修正）"
                lblShusei.Visible = True
            End If
            If isEnd Then  '指定伝票に移動する時
                Return False
            End If
            isEnd = False

            '画面表示
            SheetRedrawON()
            SetCursorDefault()
            Me.ShowDialog()

            Return isUpdated  '伝票を更新した時、Trueを返す

        Catch ex As Exception
            ErrProc(ex, Me.Text)
            Return False

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Function

    '受注伝票Noから、受注伝票参照入力画面を表示する（受注明細表から、納品伝票作成を選んだ時）
    Public Function Nounyu(ByVal JutyuNo As Integer) As Boolean
        If My.Settings.HanbaiKanriType <> "C" Then
            Return True
        ElseIf My.Settings.EndUserName = "信和通信工業株式会社" Then
            '*信和*　Cタイプでも受注は使用しない
            Return True
        End If

        SetCursorWait()
        SheetRedrawOFF()
        Try
            SetInit()    '画面表示の初期設定

            isNewInputtable = False  '新規伝票入力を不可とする
            isUpdated = False

            '該当の受注伝票データを得る
            If GetRecordJutyu(JutyuNo) Then
                MRowSheet.Enabled = True
                SetForm(True, False, True)
                If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
                    WhenChangeRate()  '消費税率変更時、金額を再計算
                End If
                SetGoukei()    '合計金額を計算
                UpdateFlagOn()
                Me.Text = TITLE & "（新規）"
                lblShusei.Visible = False
                If MRowSheet.MaxMRows > 0 Then
                    'カーソル移動
                    Me.ActiveControl = MRowSheet  'Form Load前ではFocusが効かないためActiveControlでセット
                    If MRowSheet.MRows(0)("入数").Value = 0 Then
                        MRowSheet.ActiveCellKey = "数量"    'セルの移動
                        MRowSheet.MRows(0)("入数").CanActivate = False
                        MRowSheet.MRows(0)("セット数").CanActivate = False
                    Else
                        MRowSheet.ActiveCellKey = "セット数"    'セルの移動
                        MRowSheet.MRows(0)("入数").CanActivate = False
                    End If
                End If
            End If
            isEnd = False

            '画面表示
            SheetRedrawON()
            SetCursorDefault()
            Me.ShowDialog()

            Return isUpdated  '伝票を更新した時、Trueを返す

        Catch ex As Exception
            ErrProc(ex, Me.Text)
            Return False

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Function

    '指定伝票対応処理
    '納品伝票No又は受注参照時の受注伝票No又は見積参照時の見積書Noを指定し、納品伝票登録の修正or複写or受注参照or見積参照画面を表示
    '  isCopy:複写入力画面を表示する時True
    Public Function FindEdit(ByVal TableNo As Integer, Optional ByVal isCopy As Boolean = False, Optional ByVal JutyuNo As Integer = 0, Optional ByVal MitumoriNo As Integer = 0) As Boolean
        SetCursorWait()
        SheetRedrawOFF()
        Try
            SetInit()    '画面表示の初期設定

            isNewInputtable = True  '新規伝票入力を可とする

            If JutyuNo > 0 Then
                '受注伝票を参照し、納品伝票の新規入力画面を表示
                If GetRecordJutyu(JutyuNo) Then
                    MRowSheet.Enabled = True
                    SetForm(True, False, True)
                    If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
                        WhenChangeRate()  '消費税率変更時、金額を再計算
                    End If
                    SetGoukei()    '合計金額を計算
                    UpdateFlagOn()
                    Me.Text = TITLE & "（新規）"
                    lblShusei.Visible = False
                End If
            ElseIf MitumoriNo > 0 Then
                '見積書を参照し、納品伝票の新規入力画面を表示
                If GetRecordMitumori(MitumoriNo) Then
                    MRowSheet.Enabled = True
                    SetForm(True, True, True)
                    '（SetFormで計算し直しているので、ここでは再計算しない）
                    'If oldRate <> Denpyou.Rate Then
                    '    WhenChangeRate()    '消費税率変更時、金額を再計算
                    'End If
                    SetGoukei()    '合計金額を計算
                    UpdateFlagOn()
                    Me.Text = TITLE & "（新規）"
                    lblShusei.Visible = False
                End If
            Else
                '納品伝票の修正画面or複写入力画面を表示
                If GetRecord(TableNo, isCopy) > 0 Then
                    MRowSheet.Enabled = True
                    If isCopy Then
                        SetForm(True, False, True)
                        If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
                            WhenChangeRate()  '消費税率変更時、金額を再計算
                        End If
                        CheckNewTanka()    '商品単価、原価単価が最新かどうかチェック
                        UpdateFlagOn()
                        Me.Text = TITLE & "（新規）"
                        lblShusei.Visible = False
                    Else
                        SetForm(True, False, False)
                        Me.Text = TITLE & "（修正）"
                        lblShusei.Visible = True
                    End If
                End If
            End If
            isEnd = False

            '画面表示
            SheetRedrawON()
            Me.Show()
            ControlFocusCode(edtTokuiCode)

            Return isUpdated  '伝票を更新した時、Trueを返す

        Catch ex As Exception
            ErrProc(ex, Me.Text)
            Return False

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Function

    ''ProcessKeyPreviewメソッドをオーバーライド(VistaでのF10押下時の処理）
    ''(VistaのIMEがメッセージをフックするため、F10押下時にInputManのFunctionKeyPressイベントが発生しない現象の回避）
    ''(FunctionKeyPressイベントが発生した時は、e.Handled = TrueでWindowsキーを無効にしているので、この処理は動かない）
    ''(IMEがONのコントロールにフォーカスがある時はこちらが動き、IMEがOffのコントロールにある時はFunctionKeyPressが動く）
    'Protected Overrides Function ProcessKeyPreview(ByRef m As System.Windows.Forms.Message) As Boolean
    '    Const WM_CHAR As Integer = &H102    '文字入力
    '    If m.Msg <> WM_CHAR AndAlso m.WParam = CType(Keys.F10, IntPtr) Then
    '        If FunctionKey.ActiveKeySet = "Shift" Then
    '            If FunctionKey.KeySets("Shift")(enFunction.F10).Enabled Then
    '                Dim mposActivePosition As GrapeCity.Win.ElTabelle.MPosition     'フォーカスを移動させる前の場所
    '                Dim objActiveControl As Object = Nothing                        'フォーカスを移動させる前のコントロール
    '                BeforeFunctionKeyPress(mposActivePosition, objActiveControl)    'ファンクション押下でフォーカス移動しないため、強制的にフォーカス移動させる
    '                FindMitumori()           '見積書参照
    '            End If
    '        Else
    '            If FunctionKey.KeySets("Normal")(enFunction.F10).Enabled Then
    '                Dim mposActivePosition As GrapeCity.Win.ElTabelle.MPosition     'フォーカスを移動させる前の場所
    '                Dim objActiveControl As Object = Nothing                        'フォーカスを移動させる前のコントロール
    '                BeforeFunctionKeyPress(mposActivePosition, objActiveControl)    'ファンクション押下でフォーカス移動しないため、強制的にフォーカス移動させる
    '                Me.edtTekiyou.select()    '摘要
    '            End If
    '        End If
    '        FunctionKey.ActiveKeySet = "Normal"  'Shiftキー押下時のファンクション表示を元に戻す
    '    End If

    '    '基底クラスのメソッドを呼び出します
    '    Return MyBase.ProcessKeyPreview(m)
    'End Function

    'フォームが初めて表示される直前に発生
    Private Sub frm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        '初回表示時のみ、SetFormでSheetに空行を追加しているにもかかわらずクリアされてしまうので、ここでセット
        If MRowSheet.MaxMRows <= 0 Then
            MRowSheet.MaxMRows = Denpyou.KoumokuSu
        End If

        '画面の共通処理設定
        CFormCommon.SetBackColorEvent(Nothing, True)  '入力項目の背景色設定イベントの登録
        Dim orgActiveContol As Control = CFormCommon.GetActiveControl(Me)  '元のActiveControlを保存（SettingFormSizeでWindowStateを設定するとフォームがActiveになり、ActiveControlが変わってしまう）
        If isWhenCopy = False Then  '「コピー」時は、画面サイズの保存と復元をしない（表示位置をカーソル位置に移動させているため、デフォルト位置がずれてしまう）
            CFormCommon.SettingFormSize(Me, Me.GetType().Name)  '画面サイズ保存/復元の共通設定
        End If
        'CFormCommon.SetBtnUpdateEnabledEvent(Nothing, toolBtnUpdate)  '更新ボタンの有効設定イベントの登録

        'Idleイベントの設定（ボタン２度押し防止のため使用）
        AddHandler Application.Idle, New EventHandler(AddressOf Application_Idle)

        If orgActiveContol IsNot MRowSheet Then  'MrowSheetにカーソルがある時は背景色が付くため除外
            If orgActiveContol IsNot Nothing Then
                'フォーカスに背景色が付かないための対応（SettingFormSizeでWindowStateを設定するとフォームがActiveになりフォーカスが変わってしまうため、元に戻す）
                CFormCommon.ResetActiveControl(Me)  '（ActiveControlをリセットしてから再設定しないと背景色が付かない）
                Me.ActiveControl = orgActiveContol  'ActiveControlを元に戻す
            End If
        End If
    End Sub

    'アプリケーションが処理を完了し、アイドル状態に入ろうとすると発生（イベントを完了した時に発生）
    Private _IsEventProcessing As Boolean  'イベント処理中フラグ（True:処理中、False:処理中でない）
    Private Sub Application_Idle(ByVal sender As Object, ByVal e As EventArgs)
        _IsEventProcessing = False  'イベントを完了したため、処理を有効化する
    End Sub

    'フォームが閉じた後に発生
    Private Sub frm_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
        'Idleイベントの解除
        RemoveHandler Application.Idle, New EventHandler(AddressOf Application_Idle)

        'BalloonTipを閉じてもApplication.OpenFormsに残ってしまうため解放する
        myBalloonTip.Dispose()

        '検索画面が開いていたら閉じる
        CloseFormListIfOpend(Nothing)
    End Sub

    'フォームが閉じる前に発生し、更新確認を行う
    Private Sub frm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        If isEnd Then Exit Sub  '終了決定なら、更新確認を行わない

        SetCursorWait()

        'LeaveCellイベントが発生しないので、セルを移動させてLeaveCellを発生させる（×ボタンの前に入力したセルが確定されないため）
        isLeaveErrorNoCheck = True  'Leaveイベントでのエラーチェックをしない（UpdateRecordでチェック）
        If edtTokuiCode.Focused Then
            edtNounyuuCode.Select()
        Else
            edtTokuiCode.Select()
        End If

        SheetRedrawOFF()
        If UpdateCaution() = False Then
            '閉じる処理をキャンセル
            e.Cancel = True
            isLeaveErrorNoCheck = False  'Leaveイベントでのエラーチェックをするように元に戻す
        End If

        SheetRedrawON()
        SetCursorDefault()
    End Sub

    'フォームにフォーカスがあるときにキーが押されると発生（Me.KeyPreview=Trueにしないと、このイベントは発生しない）
    Private Sub frm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        'Shiftキーが押された時、ボタンをShiftキー用に設定
        If (e.Modifiers And Keys.Shift) = Keys.Shift Then
            SetToolBtnShiftKey(True)
        End If
    End Sub

    'フォームにフォーカスがあるときにキーが離されると発生（Me.KeyPreview=Trueにしないと、このイベントは発生しない）
    Private Sub frm_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        '押していたShiftキーを離した時、ボタンを通常に戻す   
        If e.KeyCode = Keys.ShiftKey Then
            SetToolBtnShiftKey(False)
        End If
    End Sub

    'Shiftキーの押下によるボタン表示の設定
    Private Sub SetToolBtnShiftKey(ByVal isPressed As Boolean)
        If isPressed Then
            'Shiftキーが押された時
            toolBtnInsertRow.Text = "行上げ" & vbCrLf & "(&A)"
            toolBtnInsertRow.Image = My.Resources.arrow_186_16_307EA9
            toolBtnInsertRow.ToolTipText = "選択した行を上に移動します"

            toolBtnDeleteRow.Text = "行下げ" & vbCrLf & "(&B)"
            toolBtnDeleteRow.Image = My.Resources.arrow_189_16_307EA9
            toolBtnDeleteRow.ToolTipText = "選択した行を下に移動します"

            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                '*信和*  領収書/プレビューを切り替える
                toolBtnRyoushuSho.Text = "領収ﾌﾟﾚ" & vbCrLf & "(&R)"
                toolBtnRyoushuSho.Image = My.Resources.paper_16_307EA9
            End If
        Else
            'Shiftキーを離した時
            toolBtnInsertRow.Text = "行挿入" & vbCrLf & "(&A)"
            toolBtnInsertRow.Image = My.Resources.add_row_16_307EA9
            toolBtnInsertRow.ToolTipText = "明細に、１行挿入します（選択行の上に挿入）"

            toolBtnDeleteRow.Text = "行削除" & vbCrLf & "(&B)"
            toolBtnDeleteRow.Image = My.Resources.delete_row_16_307EA9
            toolBtnDeleteRow.ToolTipText = "明細から、１行削除します"

            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                '*信和*  領収書/プレビューを切り替える
                toolBtnRyoushuSho.Text = "領収書" & vbCrLf & "(&R)"
                toolBtnRyoushuSho.Image = My.Resources.printer_16_307EA9
            End If
        End If
    End Sub

    '画面表示のための各種初期設定
    Private Sub SetInit()
        'マスタ登録の権限により、MenuStrip「環境設定」の使用可/不可を設定
        mnuEnvironment.Enabled = basMain.IsLogin And basMain.CUser_Login.Permission(HanbaikanriDialog.COperations.enumHKOperations.マスタ登録)

        '自社情報マスタからコード桁数等の情報を得る
        Dim CJisha As New CJisha
        If CJisha.GetJisha(drJisha, CSingleton.CSetting.Connect) = False Then
            MessageBox.Show("自社情報マスタにデータがありません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        '単価登録メッセージを表示するかどうかの設定（表示する場合True)
        isShowTankaMSG = GetBoolean(drJisha("得意先単価登録MSG"))

        'デフォルト倉庫がある場合、デフォルトの倉庫マスタNoを得る（デフォルト倉庫が無い時はゼロ）
        Dim CSouko As New HanbaikanriDialog.CSouko
        defaultSoukoNo = CSouko.GetDefaultSoukoNo()

        'デフォルト商品を得る（商品コード未入力時の自動セットで使用）
        Dim CShouhin As New HanbaikanriDialog.CShouhin
        Dim drShouhin As DataRow = CShouhin.GetMaster(GetInt(drJisha("商品デフォルトNo")))
        If drShouhin Is Nothing Then
            defaultShouhin.MasterNo = 0
            MessageBox.Show("デフォルト商品が登録されていません。" & vbCrLf & "納品伝票画面を閉じ、自社情報のデフォルト商品コードを登録してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            defaultShouhin.MasterNo = drShouhin("マスタNo")
            defaultShouhin.Code = drShouhin("コード").ToString
            defaultShouhin.ZeiKubun = drShouhin("税区分")
            defaultShouhin.TaxRateKubun = drShouhin("消費税率区分")
        End If

        '日付項目の未入力不可イベントの登録
        CFormCommon.SetDateNullValidatingEvent(TableLayoutPanelBase)

        '納品日、請求日のチェックで使用する基準日を設定
        ErrorProvider1.Icon = My.Resources.StatusInvalid_16x
        If drJisha("伝票日付チェック") Then
            ErrorProvider1.SetIconAlignment(datNouhinDate, ErrorIconAlignment.MiddleLeft)
            ErrorProvider1.SetIconAlignment(datSeikyuDate, ErrorIconAlignment.MiddleLeft)
            ErrorProvider1.SetIconPadding(datNouhinDate, 3)
            ErrorProvider1.SetIconPadding(datSeikyuDate, 3)
            checkMinDate = CDenpyouCommon.CheckDate_GetMinDate(drJisha("締日"))  '前々月の自社締日以前はワーニング
            checkMaxDate = CDenpyouCommon.CheckDate_GetMaxDate()  '本日の1ヵ月後以降はワーニング
        End If

        '納品伝票コードの採番
        If drJisha("納品伝票コード自動更新") Then
            Denpyou.NewCode = drJisha("納品伝票現コード")
        Else
            Denpyou.NewCode = GetDenpyouCode(0)
        End If

        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            '*信和*　明細行のデフォルトMax行数を6とする
            MaxKoumoku = 6
        End If

        SetInitForm()  '画面の初期設定
        SetInitMultiRow()  'MultiRowの初期設定
        SetInitSheetGoukei()  '合計シートの初期設定
    End Sub

    'フォームの初期設定
    Private Sub SetInitForm()
        isInitialSet = True

        '画面設定共通クラスの初期設定
        CFormCommon.SetFormControls(Me)  'フォームの全コントロールを取得
        CFormCommon.SetMeFont(Me)  'フォントの初期設定
        CFormCommon.SetEnabledChangedEvent(Nothing)  'ボタン/ラベル等のEnabledによる枠線色/背景色設定イベントの登録

        '納入先
        CFormCommon.IniCtrlText(edtNounyuuName, 50)
        CFormCommon.IniCtrlText(edtNounyuuName2, 50)
        CFormCommon.MakeKeishouCombo(cmbNounyuuKeisho)  '納入先敬称コンボボックス

        '倉庫
        CFormCommon.IniCtrlText(edtSoukoCode, drJisha("倉庫コード桁数"), "9", True)

        '売上区分のコンボボックス設定
        Dim CUriageKubun As New CUriageKubun()
        CUriageKubun.MakeUriageKubunCombo(cmbUriageKubun)
        defaultUriageKubun = cmbUriageKubun.Items(0).Value '１行目をHold（初期値に使用）

        '年月日の設定
        Dim datControl() As GrapeCity.Win.Input.Date = {datNouhinDate, datSeikyuDate}
        For i As Integer = 0 To datControl.Length - 1
            CFormCommon.IniCtrlDate(datControl(i), drJisha("和暦"))
        Next

        If drJisha("日付選択") = False Then
            lblNouhinDate.Visible = False
            datNouhinDate.Visible = False
            lblSeikyuDate.Text = "日付"
        End If

        '摘要
        CFormCommon.IniCtrlTextMultiLine(edtTekiyou, 255)

        '*信和*　但し書き
        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            edtLblTadasiGaki.Visible = True
            edtTadasiGaki.Visible = True

            CFormCommon.IniCtrlText(edtTadasiGaki, 100)
        Else
            edtLblTadasiGaki.Visible = False
            edtTadasiGaki.Visible = False
        End If

        '*信和*　仮伝票
        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            chkKariDen.Visible = True
        Else
            chkKariDen.Visible = False
        End If

        '納品伝票印刷のToolStripメニュー設定
        '　納品伝票フォームのコンボボックス作成（自社情報の納品伝票フォームと、納品伝票フォルダから作成）
        Dim formFolder As String = PrintFormFolderMirrorPrincipal(CSingleton.CSetting.PrintFormFolder) & "\納品伝票"
        CFormCommon.MakeComboBoxForm(mnuCmbForm, BindingContext, PrintFormFolderMirrorPrincipal(drJisha("納品伝票ﾌｫｰﾑ").ToString), formFolder)
        '　納品伝票プリンタのコンボボックス作成
        CFormCommon.MakeComboBoxPrinter(mnuCmbPrinter)

        '*信和*　領収書印刷のToolStripメニュー設定
        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            mnuLblFormRyoushuSho.Visible = True
            mnuCmbFormRyoushuSho.Visible = True
            mnuLblPrinterRyoushuSho.Visible = True
            mnuCmbPrinterRyoushuSho.Visible = True
            mnuPrintRyoushuSho.Visible = True
            mnuPreviewRyoushuSho.Visible = True
            mnuSepa1RyoushuSho.Visible = True
            mnuSepa2RyoushuSho.Visible = True
            mnuSepa3RyoushuSho.Visible = True

            '領収書フォームのコンボボックス作成（領収書フォームと、納品伝票フォルダから作成）
            Dim formFolderRyoushuSho As String = PrintFormFolderMirrorPrincipal(RyoushuShoFolder)
            CFormCommon.MakeComboBoxForm(mnuCmbFormRyoushuSho, BindingContext, formFolderRyoushuSho & RyoushuShoForm, formFolderRyoushuSho)
            '領収書プリンタのコンボボックス作成
            CFormCommon.MakeComboBoxPrinter(mnuCmbPrinterRyoushuSho)
        Else
            mnuLblFormRyoushuSho.Visible = False
            mnuCmbFormRyoushuSho.Visible = False
            mnuLblPrinterRyoushuSho.Visible = False
            mnuCmbPrinterRyoushuSho.Visible = False
            mnuPrintRyoushuSho.Visible = False
            mnuPreviewRyoushuSho.Visible = False
            mnuSepa1RyoushuSho.Visible = False
            mnuSepa2RyoushuSho.Visible = False
            mnuSepa3RyoushuSho.Visible = False
        End If

        'カーソル移動の設定
        Using cnTable As New SqlConnection(CSingleton.CSetting.Connect)
            cnTable.Open()
            Dim sSQL As String
            '　倉庫が１件しかない時、倉庫にカーソルを移動させない
            If defaultSoukoNo > 0 Then
                sSQL = "SELECT COUNT(*) FROM 倉庫マスタ"
                Dim iSoukoCnt As Object = CDBCommon.SQLExecuteScalar(cnTable, sSQL)
                If iSoukoCnt IsNot DBNull.Value AndAlso iSoukoCnt IsNot Nothing AndAlso iSoukoCnt = 1 Then
                    edtSoukoCode.TabStop = False
                End If
            End If
            '　担当者が１件もない時、担当者にカーソルを移動させない
            sSQL = "SELECT TOP 1 マスタNo FROM 担当者マスタ"
            Dim iTantouMasterNo As Object = CDBCommon.SQLExecuteScalar(cnTable, sSQL)
            If iTantouMasterNo Is Nothing Then
                edtTantouCode.TabStop = False
            End If
        End Using

        If My.Settings.EndUserName = "海政インキ株式会社" Then
            '*海政*  担当者・伝票コードにカーソル移動しない（日付までカーソルを移動させる）
            edtTantouCode.TabStop = False
            edtDenpyouCode.TabStop = False
        End If

        Me.KeyPreview = True  'KeyDown/KeyPressイベントを有効にする（Shiftキーによるボタン表示の変更のため）

        'ボタンの初期設定
        If My.Settings.HanbaiKanriType = "C" Then
            '「受注」参照
            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                '*信和*　Cタイプでも受注は使用しない
                toolBtnJutyu.Visible = False
                toolSepaJutyu.Visible = False
                mnuJutyu.Visible = False
            Else
                toolBtnJutyu.Visible = True
                toolSepaJutyu.Visible = True
                mnuJutyu.Visible = True
            End If
        Else
            toolBtnJutyu.Visible = False
            toolSepaJutyu.Visible = False
            mnuJutyu.Visible = False
        End If
        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            '*信和*　「領収書」ボタンあり
            toolBtnRyoushuSho.Visible = True
            toolSepaRyoushuSho.Visible = True
        Else
            toolBtnRyoushuSho.Visible = False
            toolSepaRyoushuSho.Visible = False
        End If
        If My.Settings.EndUserName = "株式会社　日の出" Then
            '*日の出*　「印刷」「プレビュー」ボタンを緑に
            toolBtnPrint.BackColor = Color.LightGreen
            toolBtnPreview.BackColor = Color.LightGreen
        End If

        If isWhenCopy Then  '「コピー」時は、画面サイズの保存と復元をしない（コピー時に画面位置を復元すると、カーソル位置に表示できず、コピー元の画面と重なってしまう）
            mnuResetForm.Visible = False
            mnuSepaResetForm.Visible = False
        End If

        'ツールチップ設定
        toolBtnNew.ToolTipText = "全てクリアし、新規伝票を入力します"
        toolBtnNextNew.ToolTipText = "同じ得意先で、新規伝票を入力します"
        toolBtnInsertRow.ToolTipText = "明細に、１行挿入します（選択行の上に挿入）"
        toolBtnDeleteRow.ToolTipText = "明細から、１行削除します"
        toolBtnDelete.ToolTipText = "伝票を削除します"
        toolBtnSearch.ToolTipText = "納品伝票を検索します"
        toolBtnSearchCopy.ToolTipText = "既存の納品伝票を検索し、内容を複写し新規の納品伝票を作成します"
        toolBtnPrint.ToolTipText = "納品伝票を印刷します"
        toolBtnPreview.ToolTipText = "納品伝票を印刷プレビューします"
        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            toolBtnRyoushuSho.ToolTipText = "領収書を印刷します。" & vbCrLf & "Shiftキーを押しながら押すと、領収書を印刷プレビューします。"
        End If
        toolBtnUpdate.ToolTipText = "入力データを登録/更新します"
        toolBtnCopyNew.ToolTipText = "表示中の内容を、別画面にコピーします"
        toolBtnTankaRireki.ToolTipText = "選択行の商品の、単価履歴一覧を表示します"
        toolBtnMitumori.ToolTipText = "見積書を参照入力します"
        toolBtnJutyu.ToolTipText = "受注伝票を参照入力します"
        toolBtnSiire.ToolTipText = "仕入伝票を参照入力します"
        toolBtnExpandMeisai.ToolTipText = "明細行の表示を縦に伸ばします⇔縮めます"
        toolBtnExport.ToolTipText = "表示中の内容をエクスポートします"
        toolBtnEnd.ToolTipText = "終了します"
        Dim toolTipMsg As New ToolTip
        toolTipMsg.SetToolTip(btnSearchTokui, "得意先一覧を表示します")
        toolTipMsg.SetToolTip(btnSearchNounyu, "納入先一覧を表示します")
        toolTipMsg.SetToolTip(btnSearchSouko, "倉庫一覧を表示します")
        toolTipMsg.SetToolTip(btnSearchTantou, "担当者一覧を表示します")
        toolTipMsg.SetToolTip(btnClearReferenceCode, "参照情報をクリアします")
        toolTipMsg.SetToolTip(lblTekiyou, "改行は[Ctlr]+[Enter]")

        'インフォメーション(i)のバルーンチップ設定（伝票の更新情報は画面右上のインフォメーション画像クリックで表示し、明細の更新情報は行ヘッダ右クリックで表示する）
        CFormCommon.IniBalloonTipInformation(picInfo, myBalloonTip, myBalloonTipInfo, lblTitle.Text & "情報")
        AddHandler picInfo.Click, AddressOf picInfo_Click  'インフォメーション画像をクリックした時のイベントを登録

        mySheetRitchTip = New GrapeCity.Win.ElTabelle.RichTip()
        CFormCommon.IniSheetRitchTip(mySheetRitchTip, "■ 納品明細情報", MRowSheet.Font)  'MRowSheet用（明細用）
        CFormCommon.SetRowHeaderClickEvent(MRowSheet, mySheetRitchTip)  '行ヘッダ右クリックの設定（明細情報表示）

        isInitialSet = False  '初期表示終了
    End Sub

    'MurtiRowSheetの初期設定
    Private Sub SetInitMultiRow()
        MRowSheet.DataSource = Nothing
        MRowSheet.DataMember = ""
        MRowSheet.MaxMRows = 0

        ROWHEIGHT = CFormCommon.GetSheetHeight(ROWHEIGHT_ORIGINAL, Me.CurrentAutoScaleDimensions) '行の高さを設定（スケールDPIにより設定）
        CFormCommon.SetCommonInitMultiRowTemplate(MRowSheet, ROWHEIGHT)  'MurtiRow Templateの共通初期設定
        CFormCommon.SetCommonInitMultiRow(MRowSheet)  'MurtiRowの共通初期設定
        CFormCommon.SetInitInputMultiRow(MRowSheet, True)  '入力用MurtiRowの共通設定

        '*** MurtiRow テンプレートの設定（共通初期設定以外の設定） ***
        Dim mRowTemplate As New GrapeCity.Win.ElTabelle.Template()
        mRowTemplate = MRowSheet.Template

        '受注コードの使用可/不可
        If My.Settings.HanbaiKanriType = "C" Then
            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                '*信和*　Cタイプでも受注は使用しない
                mRowTemplate.Cells(enSheetCol1.受注コード, enSheetRow.Row1).Enabled = False
                mRowTemplate.Cells(enSheetCol2.受注明細検索ボタン, enSheetRow.Row2).Enabled = False
                mRowTemplate.Cells.SetColumnWidth(enSheetCol1.受注コード, 0)
            Else
                mRowTemplate.Cells(enSheetCol1.受注コード, enSheetRow.Row1).Enabled = True
                mRowTemplate.Cells(enSheetCol2.受注明細検索ボタン, enSheetRow.Row2).Enabled = True
            End If
        Else
            mRowTemplate.Cells(enSheetCol1.受注コード, enSheetRow.Row1).Enabled = False
            mRowTemplate.Cells(enSheetCol2.受注明細検索ボタン, enSheetRow.Row2).Enabled = False
            mRowTemplate.Cells.SetColumnWidth(enSheetCol1.受注コード, 0)
        End If

        'セルの設定（データ型自体は、デザイナで設定しておく）
        Dim blankWhenZero As String = " "  '値がゼロの時、空白を表示する

        mRowTemplate.Cells(enSheetCol2.受注明細検索ボタン, enSheetRow.Row2).Font = New Font(MRowSheet.Font.FontFamily, MRowSheet.Font.Size - FONT_UNIT)
        mRowTemplate.Cells(enSheetCol1.商品検索ボタン, enSheetRow.Row1).Font = New Font(MRowSheet.Font.FontFamily, MRowSheet.Font.Size - FONT_UNIT)

        '  <受注コード>
        If My.Settings.HanbaiKanriType = "C" Then
            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                '*信和*　Cタイプでも受注は使用しない
            Else
                CFormCommon.IniCtrlText(mRowTemplate.Cells(enSheetCol1.受注コード, enSheetRow.Row1).Editor, CInt(drJisha("受注伝票ｺｰﾄﾞ桁数")))
            End If
        End If

        '  <商品コード>
        CFormCommon.IniCtrlText(mRowTemplate.Cells(enSheetCol1.商品コード, enSheetRow.Row1).Editor, CInt(drJisha("商品コード桁数")))
        If drJisha("商品英数字可") Then
            mRowTemplate.Cells(enSheetCol1.商品コード, enSheetRow.Row1).ImeMode = ImeMode.Off
        Else
            mRowTemplate.Cells(enSheetCol1.商品コード, enSheetRow.Row1).ImeMode = ImeMode.Disable
        End If

        '  <商品名称カナ> 
        CFormCommon.IniCtrlText(mRowTemplate.Cells(enSheetCol1.商品名称カナ, enSheetRow.Row1).Editor, 50)
        ''''mRowTemplate.Cells(enSheetCol1.商品名称カナ, enSheetRow.Row1).Font = New System.Drawing.Font("ＭＳ ゴシック", MRowSheet.Font.Size)
        If My.Settings.EndUserName = "田中コルク工業株式会社" Then
            '*田中コルク*　商品名称の幅を広げるため、名称カナを広げる
            mRowTemplate.Cells.SetColumnWidth(enSheetCol1.商品名称カナ, mRowTemplate.Cells.GetColumnWidth(enSheetCol1.商品名称カナ) + 65)
        End If

        '  <商品名称> 
        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            CFormCommon.IniCtrlText(mRowTemplate.Cells(enSheetCol2.商品名称, enSheetRow.Row2).Editor, 30)
        Else
            CFormCommon.IniCtrlText(mRowTemplate.Cells(enSheetCol2.商品名称, enSheetRow.Row2).Editor, 50)
        End If
        ''''mRowTemplate.Cells(enSheetCol2.商品名, enSheetRow.Row2).Font = New System.Drawing.Font("ＭＳ ゴシック", MRowSheet.Font.Size)

        '  <消費税率>（TextEditorで定義しておき、消費税マスタにより、セルにコンボボックスを指定）
        Dim txtEditor As New GrapeCity.Win.ElTabelle.Editors.TextEditor()
        mRowTemplate.Cells(enSheetCol2.消費税率, enSheetRow.Row2).Editor = txtEditor

        '  <商品税区分> 
        Dim cmbEditor As New GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor()
        CFormCommon.MakeZeikubunCombo(cmbEditor)
        cmbEditor.ShowDropDown = GrapeCity.Win.ElTabelle.Editors.Visibility.NotShown
        mRowTemplate.Cells(enSheetCol1.商品税区分, enSheetRow.Row1).Editor = cmbEditor

        '  <入数><数量>
        CFormCommon.IniCtrlNumber(mRowTemplate.Cells(enSheetCol1.入数, enSheetRow.Row1).Editor, 6, CInt(GetByte(drJisha("売掛数量少数桁数"))), False)
        CFormCommon.IniCtrlNumber(mRowTemplate.Cells(enSheetCol2.数量, enSheetRow.Row2).Editor, 6, CInt(GetByte(drJisha("売掛数量少数桁数"))), False)
        If My.Settings.EndUserName = "信和通信工業株式会社" _
          OrElse My.Settings.EndUserName = "株式会社　日の出" _
          OrElse My.Settings.EndUserName = "株式会社　山松" Then
            mRowTemplate.Cells(enSheetCol1.入数, enSheetRow.Row1).TabStop = False
        End If
        If My.Settings.EndUserName = "株式会社サンオカ" Then
            '*サンオカ*　入数/セット数を使用しない（表示しない）
            mRowTemplate.Cells(enSheetCol1.入数, enSheetRow.Row1).Lock = True  '編集禁止
            mRowTemplate.Cells(enSheetCol1.入数, enSheetRow.Row1).TabStop = False
            CFormCommon.IniCtrlNumber(mRowTemplate.Cells(enSheetCol1.入数, enSheetRow.Row1).Editor, 6, CInt(GetByte(drJisha("売掛数量少数桁数"))), False, blankWhenZero)  '値がゼロならスペースを表示でゼロを表示させないようにする
            mRowTemplate.ColumnHeaders.Merge(New GrapeCity.Win.ElTabelle.TRange(enSheetCol1.入数, enSheetRow.Row1, enSheetCol1.入数, enSheetRow.Row2))
            mRowTemplate.ColumnHeaders(enSheetCol1.入数, enSheetRow.Row1).Caption = "数量"
        End If

        '  <セット数>
        CFormCommon.IniCtrlNumber(mRowTemplate.Cells(enSheetCol1.セット数, enSheetRow.Row1).Editor, 6, 0, False)
        If My.Settings.EndUserName = "信和通信工業株式会社" _
          OrElse My.Settings.EndUserName = "株式会社　日の出" _
          OrElse My.Settings.EndUserName = "株式会社　山松" Then
            mRowTemplate.Cells(enSheetCol1.セット数, enSheetRow.Row1).TabStop = False
        End If
        If My.Settings.EndUserName = "株式会社サンオカ" Then
            '*サンオカ*　入数/セット数を使用しない（表示しない）
            mRowTemplate.Cells(enSheetCol1.セット数, enSheetRow.Row1).Lock = True  '編集禁止
            mRowTemplate.Cells(enSheetCol1.セット数, enSheetRow.Row1).TabStop = False
            CFormCommon.IniCtrlNumber(mRowTemplate.Cells(enSheetCol1.セット数, enSheetRow.Row1).Editor, 6, 0, False, blankWhenZero)
            mRowTemplate.ColumnHeaders.Merge(New GrapeCity.Win.ElTabelle.TRange(enSheetCol1.セット数, enSheetRow.Row1, enSheetCol1.セット数, enSheetRow.Row2))
            mRowTemplate.ColumnHeaders(enSheetCol1.セット数, enSheetRow.Row1).Caption = "単位"
        End If

        '  <単位>
        cmbEditor = New GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor()
        Dim CShouhin As New HanbaikanriDialog.CShouhin()
        Dim dtTanni As DataTable = CShouhin.GetTanniMaster()  '単位マスタ取得
        CFormCommon.MakeTanniCombo(cmbEditor, dtTanni)
        mRowTemplate.Cells(enSheetCol2.単位IN, enSheetRow.Row2).Editor = cmbEditor
        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            mRowTemplate.Cells(enSheetCol2.単位IN, enSheetRow.Row2).TabStop = False
        End If

        '  <原価単価>
        CFormCommon.IniCtrlNumber(mRowTemplate.Cells(enSheetCol1.原価単価, enSheetRow.Row1).Editor, 9, CInt(GetByte(drJisha("買掛単価少数桁数"))), False, blankWhenZero)
        If My.Settings.EndUserName = "信和通信工業株式会社" _
          OrElse My.Settings.EndUserName = "株式会社　日の出" _
          OrElse My.Settings.EndUserName = "株式会社　山松" Then
            mRowTemplate.Cells(enSheetCol1.原価単価, enSheetRow.Row1).TabStop = False
        End If

        '  <商品単価>
        CFormCommon.IniCtrlNumber(mRowTemplate.Cells(enSheetCol2.商品単価, enSheetRow.Row2).Editor, 9, CInt(GetByte(drJisha("売掛単価少数桁数"))), False, blankWhenZero)

        '  <金額>
        CFormCommon.IniCtrlNumber(mRowTemplate.Cells(enSheetCol1.金額, enSheetRow.Row1).Editor, 12, 0, False)

        '  <備考> 
        CFormCommon.IniCtrlTextMultiLine(mRowTemplate.Cells(enSheetCol1.備考, enSheetRow.Row1).Editor, 255)
        ''''mRowTemplate.Cells(enSheetCol1.備考, enSheetRow.Row1).Font = New System.Drawing.Font("ＭＳ ゴシック", MRowSheet.Font.Size)
        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            mRowTemplate.Cells(enSheetCol1.備考, enSheetRow.Row1).TabStop = False
        End If

        '入力最終列より後ろの列を表示しない
        For iCol As Integer = LastColumn + 1 To mRowTemplate.Cells.MaxColumns - 1
            mRowTemplate.Cells.SetColumnWidth(iCol, 0)
        Next

        'スケールDPIによりシートの幅と高さの設定を行なう
        CFormCommon.SetMRowSheetWidthHeight(mRowTemplate, Me.CurrentAutoScaleDimensions)

        'テンプレートを適用する
        MRowSheet.Template = mRowTemplate

        CFormCommon.SetCommonInitMultiRow2(MRowSheet)  'MurtiRowの共通初期設定2


        '*** MultiRowの設定（共通初期設定以外の設定） ***
        If My.Settings.EndUserName = "株式会社　日の出" Then
            '日の出は、セルのハイライトを個別に設定するため、ここでの設定をしない
            MRowSheet.HighlightEditText = False
        End If
    End Sub

    '合計シートの初期設定
    Private Sub SetInitSheetGoukei()
        CFormCommon.SetCommonInitSheet(sheetGoukei, ROWHEIGHT)  'Sheetの共通設定
        CFormCommon.SetSheetColumnWidth(sheetGoukei, Me.CurrentAutoScaleDimensions)  'Sheetの列幅をスケール(DPI)により設定
        sheetGoukei.GrayAreaColor = SystemColors.Control  '非セル領域の色

        '罫線
        sheetGoukei.SetBorder(New GrapeCity.Win.ElTabelle.Range(0, 0, sheetGoukei.MaxColumns - 1, sheetGoukei.MaxRows - 1),
                New GrapeCity.Win.ElTabelle.BorderLine(GRID_COLOR, GrapeCity.Win.ElTabelle.BorderLineStyle.Thin), GrapeCity.Win.ElTabelle.Borders.All)

        '表示書式の設定
        Dim numEditor As New GrapeCity.Win.ElTabelle.Editors.NumberEditor()
        CFormCommon.IniCtrlNumber(numEditor, 12, 0, False)
        sheetGoukei.Columns(enSheetGoukeiCol.税抜額).Editor = numEditor
        sheetGoukei.Columns(enSheetGoukeiCol.消費税額).Editor = numEditor
        sheetGoukei.Columns(enSheetGoukeiCol.合計).Editor = numEditor

        numEditor = New GrapeCity.Win.ElTabelle.Editors.NumberEditor()
        CFormCommon.IniCtrlNumber(numEditor, 12, 4, False)
        numEditor.DisplayFormat = New GrapeCity.Win.ElTabelle.Editors.NumberFormat("###,###,###,##0.####", "", "", "-", "", "", "")
        sheetGoukei.Columns(enSheetGoukeiCol.参考消費税).Editor = numEditor

        '合計行（1行目）
        sheetGoukei(enSheetGoukeiCol.消費税率テキスト, enSheetGoukeiRow.合計行).Value = "合計"
        sheetGoukei(enSheetGoukeiCol.消費税率, enSheetGoukeiRow.合計行).Value = -1
        sheetGoukei.Rows(enSheetGoukeiRow.合計行).Font = New Font(sheetGoukei.Rows(enSheetGoukeiRow.合計行).Font.FontFamily, sheetGoukei.Rows(enSheetGoukeiRow.合計行).Font.Size, FontStyle.Bold)
        '　太線で囲む
        sheetGoukei.SetBorder(New GrapeCity.Win.ElTabelle.Range(0, enSheetGoukeiRow.合計行, sheetGoukei.MaxColumns - 1, enSheetGoukeiRow.合計行),
                New GrapeCity.Win.ElTabelle.BorderLine(GRID_COLOR, GrapeCity.Win.ElTabelle.BorderLineStyle.Medium), GrapeCity.Win.ElTabelle.Borders.OutLine)
        sheetGoukei.FreezeRows = enSheetGoukeiRow.合計行 + 1
    End Sub

    'インフォメーション(i)を押した時、バルーンチップを表示する
    Private Sub picInfo_Click(sender As Object, e As EventArgs)
        If Denpyou.TableNo <= 0 Then Exit Sub

        SetCursorWait()
        myBalloonTipInfo.Text = ""
        Dim drDenpyou As DataRow = dtDenpyou.Rows(0)
        If GetInt(drDenpyou("請求書No")) > 0 Then
            Dim sSQL As String = "SELECT テーブルNo FROM 請求書 WHERE テーブルNo = " & drDenpyou("請求書No").ToString
            Dim iNo As Object = CDBCommon.SQLExecuteScalar(CSingleton.CSetting.Connect, sSQL) '単一データを取得
            If iNo IsNot Nothing Then  '請求書発行後請求書が削除された時は、請求書Noのみ残り請求書が無い
                myBalloonTipInfo.Text &= "◎請求書発行済" & vbCrLf
            End If
        End If
        If GetInt(drDenpyou("月次元帳No")) > 0 Then
            myBalloonTipInfo.Text &= "◎売上月次元帳計算済" & vbCrLf
        End If
        If GetInt(drDenpyou("締日元帳No")) > 0 Then
            myBalloonTipInfo.Text &= "◎売上締日元帳計算済" & vbCrLf
        End If
        myBalloonTipInfo.Text &= "消費税計算：" & GetZeikubunName(drDenpyou("得意先税区分")) & "、" & GetShouhizeiKeisanName(drDenpyou("消費税計算方法")) & "、" & GetHasuuName(drDenpyou("端数")) & vbCrLf
        CFormCommon.SetUpdateInfoToBalloonTip(myBalloonTipInfo, drDenpyou, True)  'バルーンチップの表示内容に、更新日時等の内容をセットする
        myBalloonTip.Show(sender)
        SetCursorDefault()
    End Sub

    'メニュー項目（ToolStripButton、ToolStripMenu）押下時の処理
    Private Sub toolBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles toolBtnNew.Click, mnuNew.Click, toolBtnNextNew.Click, mnuNextNew.Click,
            toolBtnInsertRow.Click, mnuInsertRow.Click, toolBtnDeleteRow.Click, mnuDeleteRow.Click, mnuRowUp.Click, mnuRowDown.Click,
            toolBtnDelete.Click, mnuDelete.Click, toolBtnSearch.Click, mnuSearch.Click, toolBtnSearchCopy.Click, mnuSearchCopy.Click,
            toolBtnPrint.Click, mnuPrint.Click, toolBtnPreview.Click, mnuPreview.Click, toolBtnRyoushuSho.Click, mnuPrintRyoushuSho.Click, mnuPreviewRyoushuSho.Click,
            toolBtnUpdate.Click, mnuUpdate.Click, toolBtnCopyNew.Click, mnuCopyNew.Click, toolBtnTankaRireki.Click, mnuTankaRireki.Click,
            toolBtnMitumori.Click, mnuMitumori.Click, toolBtnJutyu.Click, mnuJutyu.Click, toolBtnSiire.Click, mnuSiire.Click, toolBtnExpandMeisai.Click, mnuExpandMeisai.Click,
            toolBtnExport.Click, mnuExport.Click,
            mnuEnvTantousha.Click, mnuEnvTokuisaki.Click, mnuEnvNounyuuSaki.Click, mnuEnvSiiresaki.Click, mnuEnvShouhin.Click,
            toolBtnEnd.Click, mnuEnd.Click

        If _IsEventProcessing Then Exit Sub Else _IsEventProcessing = True 'イベント処理中は処理しない（ボタンの２度押し防止のため）
        SetCursorWait()
        Try
            Dim originalPosition As GrapeCity.Win.ElTabelle.MPosition = MRowSheet.ActivePosition  'フォーカスを移動させる前の場所
            isLeaveErrorNoCheck = True  'Leaveイベントのエラーチェックを行わない（エラーで先に進めないため、UpdateRecordでチェックする）

            'Sheetの文字項目にカーソルがある時にToolStrip/MenuStripを選択すると、Sheetの値が確定しないためフォーカス移動させる
            '  （SheetのLeaveCellイベントが発生し、e.Cancel=Trueにしても先に進んでしまうため、商品コードはUpdateRecordでチェックする）
            Me.Validate()
            If sender.GetType Is GetType(ToolStripButton) Then
                ToolStripMenu.Select()
            ElseIf sender.GetType Is GetType(ToolStripMenuItem) Then
                MenuStrip1.Select()
            Else
                Exit Sub
            End If

            SetCursorWait()  '（上記SelectでLeaveイベントが発生し、CursorがDefaultになることがあるため、再度設定）
            SheetRedrawOFF()  '（上記SelectでLeaveイベントが発生し、Redraw=Trueになることがあるため、Select後にセット）
            If sender Is toolBtnNew OrElse sender Is mnuNew Then
                '「新規］、[ファイル]-[新規伝票]：新規伝票の入力
                AllNew()

            ElseIf sender Is toolBtnNextNew OrElse sender Is mnuNextNew Then
                '「次伝票」、[ファイル]-[同じ得意先で新規の伝票]：次伝票の入力
                NextNew()

            ElseIf sender Is toolBtnInsertRow OrElse sender Is mnuInsertRow OrElse sender Is mnuRowUp Then
                If sender Is mnuRowUp OrElse (sender Is toolBtnInsertRow AndAlso (Control.ModifierKeys And Keys.Shift) = Keys.Shift) Then  'Shiftキー押下時
                    '「行上げ」、[編集]-[行移動↑]：選択した行を上に移動
                    RowUp(originalPosition.MRow)

                    If MRowSheet.Enabled AndAlso originalPosition.IsEmpty = False Then
                        MRowSheet.Select()
                        If originalPosition.MRow > 0 Then
                            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(originalPosition.MRow - 1, originalPosition.Column, originalPosition.Row)
                        Else
                            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(originalPosition.MRow, originalPosition.Column, originalPosition.Row)
                        End If
                    End If
                Else
                    '「行挿入」、[編集]-[行挿入]：明細に行挿入（選択行の上に追加）
                    InsertLine(MRowSheet.ActivePosition.MRow)

                    If MRowSheet.Enabled AndAlso originalPosition.IsEmpty = False Then
                        MRowSheet.Select()
                        If My.Settings.HanbaiKanriType = "C" Then
                            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                                '*信和*　Cタイプでも受注は使用しない
                                MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(originalPosition.MRow, enSheetCol1.商品コード, enSheetRow.Row1)
                            Else
                                MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(originalPosition.MRow, enSheetCol1.受注コード, enSheetRow.Row1)
                            End If
                        Else
                            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(originalPosition.MRow, enSheetCol1.商品コード, enSheetRow.Row1)
                        End If
                    End If
                End If

            ElseIf sender Is toolBtnDeleteRow OrElse sender Is mnuDeleteRow OrElse sender Is mnuRowDown Then
                If sender Is mnuRowDown OrElse (sender Is toolBtnDeleteRow AndAlso (Control.ModifierKeys And Keys.Shift) = Keys.Shift) Then  'Shiftキー押下時
                    '「行下げ」、[編集]-[行移動↓]：選択した行を下に移動
                    RowDown(originalPosition.MRow)

                    If MRowSheet.Enabled AndAlso originalPosition.IsEmpty = False Then
                        MRowSheet.Select()
                        If originalPosition.MRow < (MRowSheet.MaxMRows - 1) Then
                            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(originalPosition.MRow + 1, originalPosition.Column, originalPosition.Row)
                        Else
                            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(originalPosition.MRow, originalPosition.Column, originalPosition.Row)
                        End If
                    End If
                Else
                    '「行削除」、[編集]-[行削除]：明細の行削除
                    DeleteLine(MRowSheet.ActivePosition.MRow)

                    If MRowSheet.Enabled AndAlso originalPosition.IsEmpty = False Then
                        MRowSheet.Select()
                        MRowSheet.ActivePosition = originalPosition  'シートのActivePositionを元に戻す
                    End If
                End If

            ElseIf sender Is toolBtnDelete OrElse sender Is mnuDelete Then
                '「伝票削除」、[ファイル]-[削除]：伝票削除（削除フラグを立てる）
                DeleteRecord()

            ElseIf sender Is toolBtnSearch OrElse sender Is mnuSearch Then
                '「検索」、[検索]-[伝票検索]：伝票検索
                FindDenpyou()

            ElseIf sender Is toolBtnSearchCopy OrElse sender Is mnuSearchCopy Then
                '「複写入力」、[検索]-[複写入力]：複写元の伝票を検索して、新規伝票入力画面を表示する
                If My.Settings.EndUserName = "有限会社長万部" AndAlso Denpyou.Tokuisaki.MasterNo > 0 Then
                    '*長万部*  複写入力（得意先を指定している場合、検索画面を表示せず、最新の伝票を複写して表示する）
                    CopyPrevReference()
                Else
                    CopyDenpyou()
                End If

            ElseIf sender Is toolBtnPrint OrElse sender Is mnuPrint Then
                '「印刷」、[納品伝票印刷]-[印刷]
                PrintDenpyou(False)

            ElseIf sender Is toolBtnPreview OrElse sender Is mnuPreview Then
                '「プレビュー」、[納品伝票印刷]-[プレビュー]
                PrintDenpyou(True)

            ElseIf sender Is toolBtnRyoushuSho Then
                '「領収書」：領収書　印刷/プレビュー（信和通信のみ）
                If (Control.ModifierKeys And Keys.Shift) = Keys.Shift Then  'Shiftキー押下時
                    PrintRyoushuSho(True)   '領収書　プレビュー

                    'ボタン表示を元に戻す（Shiftキーを押しながら領収書ボタンを押すと、KeyUpイベントが発生しないため）
                    toolBtnRyoushuSho.Text = "領収書" & vbCrLf & "(&R)"
                    toolBtnRyoushuSho.Image = My.Resources.printer_16_307EA9
                Else
                    PrintRyoushuSho(False)  '領収書　印刷
                End If

            ElseIf sender Is mnuPrintRyoushuSho Then
                '[納品伝票印刷]-[領収書印刷]
                PrintRyoushuSho(False)

            ElseIf sender Is mnuPreviewRyoushuSho Then
                '[納品伝票印刷]-[領収書プレビュー]
                PrintRyoushuSho(True)

            ElseIf sender Is toolBtnUpdate OrElse sender Is mnuUpdate Then
                '「登録」、[ファイル]-[登録/更新]
                UpdateRecord()

            ElseIf sender Is toolBtnCopyNew OrElse sender Is mnuCopyNew Then
                '「コピー」、[編集]-[伝票コピー]：現在表示している内容を別ウィンドウの新規伝票入力画面に複写する
                CopyNew()

            ElseIf sender Is toolBtnTankaRireki OrElse sender Is mnuTankaRireki Then
                '「単価」、[検索]-[単価履歴]：単価履歴表示
                If GetTankaRireki() = False Then
                    '単価を選択しなかった時、カーソル位置を元に戻す
                    If MRowSheet.Enabled AndAlso originalPosition.IsEmpty = False Then
                        MRowSheet.Select()
                        MRowSheet.ActivePosition = originalPosition  'シートのActivePositionを元に戻す
                    End If
                End If

            ElseIf sender Is toolBtnMitumori OrElse sender Is mnuMitumori Then
                '「見積」、[検索]-[見積参照]：見積参照
                FindMitumori()

            ElseIf sender Is toolBtnJutyu OrElse sender Is mnuJutyu Then
                '「受注」、[検索]-[受注参照]：受注参照
                FindJutyu()

            ElseIf sender Is toolBtnSiire OrElse sender Is mnuSiire Then
                '「仕入」、[検索]-[仕入参照]：仕入参照
                FindSiire()

            ElseIf sender Is toolBtnExpandMeisai OrElse sender Is mnuExpandMeisai Then
                '「行」、[編集]-[行表示の拡縮]：明細行表示の拡大縮小
                ExpandMeisai()

            ElseIf sender Is toolBtnExport OrElse sender Is mnuExport Then
                '「エクスポート」、[ファイル]-[エクスポート]：エクスポート
                ExportDenpyou()

            ElseIf sender Is mnuEnvTantousha Then
                '[環境登録]-[担当者登録]：担当者マスタの登録画面表示
                Dim CTantousha As New HanbaikanriDialog.CTantousha(drJisha("担当者コード桁数"))
                CTantousha.Edit()

            ElseIf sender Is mnuEnvTokuisaki Then
                '[環境登録]-[得意先登録]：得意先マスタの登録画面表示
                Dim CTokuisaki As New HanbaikanriDialog.CTokuisaki(drJisha("得意先コード桁数"))
                CTokuisaki.Edit()

            ElseIf sender Is mnuEnvNounyuuSaki Then
                '[環境登録]-[納入先登録]：納入先マスタの登録画面表示
                Dim CNounyuu As New HanbaikanriDialog.CNounyuu(drJisha("納入先コード桁数"))
                CNounyuu.Edit(drJisha("得意先コード桁数"))

            ElseIf sender Is mnuEnvSiiresaki Then
                '[環境登録]-[仕入先登録]：仕入先マスタの登録画面表示
                Dim CSiiresaki As New HanbaikanriDialog.CSiiresaki(drJisha("仕入先コード桁数"))
                CSiiresaki.Edit()

            ElseIf sender Is mnuEnvShouhin Then
                '[環境登録]-[商品登録]：商品マスタの登録画面表示
                Dim CShouhin As New HanbaikanriDialog.CShouhin(drJisha("商品コード桁数"))
                CShouhin.Edit(, , enHanbaiKubun.すべて)

            ElseIf sender Is toolBtnEnd OrElse sender Is mnuEnd Then
                '「終了」、[ファイル]-[終了]
                EndDenpyou()
            End If

        Finally
            isLeaveErrorNoCheck = False
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '「画面サイズ　リセット」：画面サイズを元に戻す
    Private Sub mnuResetForm_Click(sender As Object, e As EventArgs) Handles mnuResetForm.Click
        SetCursorWait()
        CFormCommon.ResetFormSize(Me)
        SetCursorDefault()
    End Sub

    '得意先コード「検索」ボタンを押した時、得意先の一覧を表示し、選択した得意先の項目を得る
    Private Sub btnSearchTokui_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchTokui.Click
        SetCursorWait()
        SheetRedrawOFF()
        If FindTokuisaki(True) Then
            isChangedTokuiCode = False

            'カーソル移動の制御
            If My.Settings.EndUserName = "江東高周波工業" Then
                '*江東高周波*  得意先入力後、商品名カナにカーソル移動させる
                MRowSheet.Select()
                MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(0, enSheetCol1.商品名称カナ, enSheetRow.Row1)
            Else
                ControlFocusCode(edtTokuiCode)
            End If
        End If
        SheetRedrawON()
        SetCursorDefault()
    End Sub

    '納入先コード「検索」ボタンを押した時、納入先の一覧を表示する
    Private Sub btnSearchNounyu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchNounyu.Click
        SetCursorWait()
        If FindNounyusaki(True) Then
            isChangedNounyuuCode = False

            'カーソル移動の制御
            ControlFocusCode(edtNounyuuCode)
        End If
        SetCursorDefault()
    End Sub

    '倉庫の「検索」ボタンを押した時、倉庫の一覧を表示し、選択した倉庫の項目を得る
    Private Sub btnSearchSouko_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchSouko.Click
        SetCursorWait()
        If FindSouko(True) Then
            isChangedSoukoCode = False

            'カーソル移動の制御
            ControlFocusCode(edtSoukoCode)
        End If
        SetCursorDefault()
    End Sub

    '担当者コード「検索」ボタンを押した時、担当者一覧を表示する
    Private Sub btnSearchTantou_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchTantou.Click
        SetCursorWait()
        If FindTantou(True) Then
            isChangedTantouCode = False

            'カーソル移動の制御
            ControlFocusCode(edtTantouCode)
        End If
        SetCursorDefault()
    End Sub

    '得意先コードラベルを押した時、得意先の詳細を表示する
    Private Sub lblTokuiCode_Click(sender As Object, e As EventArgs) Handles lblTokuiCode.Click
        Dim CTokuisaki As New HanbaikanriDialog.CTokuisaki()
        CTokuisaki.DispInfo(Denpyou.Tokuisaki.MasterNo, False)
    End Sub

    '納入先コードラベルを押した時、納入先の詳細を表示する
    Private Sub lblNounyuuCode_Click(sender As Object, e As EventArgs) Handles lblNounyuuCode.Click
        Dim CNounyuu As New HanbaikanriDialog.CNounyuu()
        CNounyuu.DispInfo(Denpyou.NounyuuSaki.MasterNo, False)
    End Sub

    '「参照クリア」ボタンを押した時、参照（見積/仕入/受注）をクリアする
    Private Sub btnClearReferenceCode_Click(sender As Object, e As EventArgs) Handles btnClearReferenceCode.Click
        If MessageBox.Show("参照をクリアします。よろしいですか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> DialogResult.Yes Then
            Exit Sub
        End If
        '参照元は、見積/仕入/受注のどれか１つのため、判断せず全てクリアする
        Denpyou.MitumoriNo = 0
        Denpyou.MitumoriCode = ""
        Denpyou.SiireDenpyouNo = 0
        Denpyou.SiireCode = ""
        Denpyou.JutyuDenpyouNo = 0
        Denpyou.JutyuCode = ""
        lblReferenceCode.Text = ""
        btnClearReferenceCode.Enabled = False
        UpdateFlagOn()
    End Sub

    '得意先コードの値を変更した時
    Private Sub edtTokuiCode_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles edtTokuiCode.TextChanged
        isChangedTokuiCode = True
        Denpyou.Tokuisaki.Code = edtTokuiCode.Text
        If isChanged OrElse Denpyou.TableNo <> 0 Then  '既に変更済or修正の時（他の変更がない時は登録ボタンを使用可にしないため）
            UpdateFlagOn()
        End If
    End Sub
    '得意先コードのフォーカス喪失時
    Private Sub edtTokuiCode_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles edtTokuiCode.Leave
        SetCursorWait()
        SheetRedrawOFF()
        Try
            If isChangedTokuiCode = False Then Exit Sub  '得意先コードを変更入力してなければ以下の処理をしない

            If edtTokuiCode.Text = "" Then
                ChangeTokuiCode(False, Nothing)  '得意先クリア
                Exit Sub
            End If

            '入力されたコードorカナに合致するマスタ一覧からコードを得る（１件しかない時は、一覧を表示せずそのデータを得る）
            If FindTokuisaki(False) Then
                'カーソル移動の制御（Leaveの中でTabStopを設定しても効かない）
                If My.Settings.EndUserName = "江東高周波工業" Then
                    '*江東高周波*  得意先入力後、商品名カナにカーソル移動させる
                    MRowSheet.Select()
                    MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(0, enSheetCol1.商品名称カナ, enSheetRow.Row1)
                Else
                    ControlFocusCode(sender)
                End If
            Else
                ChangeTokuiCode(False, Nothing)  '得意先クリア
                edtTokuiCode.Select()
                edtTokuiCode.SelectionStart = 0  'フィールドを移動しないと、ハイライト表示が効かないため設定
                edtTokuiCode.SelectionLength = edtTokuiCode.Text.Length
            End If

        Finally
            isChangedTokuiCode = False
            If edtTokuiCode.Text = "" Then
                '得意先コードが未入力でシフトもALTもマウスも押されていなければ、検索ボタンへ移動する
                If (Control.ModifierKeys And Keys.Shift) <> Keys.Shift AndAlso (Control.ModifierKeys And Keys.Alt) <> Keys.Alt AndAlso Control.MouseButtons = MouseButtons.None Then
                    btnSearchTokui.Select()
                End If
            End If
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '納入先コードの値を変更した時
    Private Sub edtNounyuuCode_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles edtNounyuuCode.TextChanged
        isChangedNounyuuCode = True
        Denpyou.NounyuuSaki.Code = edtNounyuuCode.Text
        UpdateFlagOn()
    End Sub
    '納入先コードのフォーカス喪失時
    Private Sub edtNounyuuCode_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles edtNounyuuCode.Leave
        SetCursorWait()
        Try
            If isChangedNounyuuCode = False Then Exit Sub  '納入先コードを変更入力してなければ以下の処理をしない

            If edtNounyuuCode.Text = "" Then
                ChangeNounyuuCode(False, Nothing)  '納入先クリア
                Exit Sub
            End If

            '入力されたコードorカナに合致する納入先一覧を表示（１件しかない時は、一覧を表示せずそのデータを得る）
            If FindNounyusaki(False) Then
                'カーソル移動の制御（Leaveの中でTabStopを設定しても効かない）
                ControlFocusCode(sender)
            Else
                ChangeNounyuuCode(False, Nothing)  '納入先クリア
                edtNounyuuCode.Select()
                edtNounyuuCode.SelectionStart = 0  'フィールドを移動しないと、ハイライト表示が効かないため設定
                edtNounyuuCode.SelectionLength = edtNounyuuCode.Text.Length
            End If

        Finally
            isChangedNounyuuCode = False
            If edtNounyuuCode.Text = "" Then
                '納入先コードが未入力でシフトもALTもマウスも押されていなければ、検索ボタンへ移動する
                If (Control.ModifierKeys And Keys.Shift) <> Keys.Shift AndAlso (Control.ModifierKeys And Keys.Alt) <> Keys.Alt AndAlso Control.MouseButtons = MouseButtons.None Then
                    btnSearchNounyu.Select()
                End If
            End If
            SetCursorDefault()
        End Try
    End Sub

    '納入先名称の値を変更した時
    Private Sub edtNounyuuName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles edtNounyuuName.TextChanged
        Denpyou.NounyuuSaki.Name = edtNounyuuName.Text
        UpdateFlagOn()
    End Sub

    '納入先名称2の値を変更した時
    Private Sub edtNounyuuName2_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles edtNounyuuName2.TextChanged
        Denpyou.NounyuuSaki.Name2 = edtNounyuuName2.Text
        UpdateFlagOn()
    End Sub

    '納入先敬称を変更した時
    Private Sub cmbNounyuuKeisho_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbNounyuuKeisho.SelectedIndexChanged
        Denpyou.NounyuuSaki.Keishou = cmbNounyuuKeisho.Text
        UpdateFlagOn()
    End Sub
    Private Sub cmbKeishou_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbNounyuuKeisho.TextChanged
        Denpyou.NounyuuSaki.Keishou = cmbNounyuuKeisho.Text
        UpdateFlagOn()
    End Sub
    '敬称のフォーカス喪失時、文字数制限を行う（指定の文字数で制限）
    Private Sub cmbKeishou_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles cmbNounyuuKeisho.Leave
        '入力文字が指定の文字数を超えている時、指定の文字数分の文字列を取り出しセットする
        cmbNounyuuKeisho.Text = StringsLeftB(cmbNounyuuKeisho.Text, FormKeishouLength)
        ControlFocusCode(cmbNounyuuKeisho)
    End Sub

    '倉庫の値を変更した時
    Private Sub edtSoukoCode_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles edtSoukoCode.TextChanged
        isChangedSoukoCode = True
        Denpyou.Souko.Code = edtSoukoCode.Text
        UpdateFlagOn()
    End Sub
    '倉庫のフォーカス喪失時
    Private Sub edtSoukoCode_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles edtSoukoCode.Leave
        SetCursorWait()
        Try
            If isChangedSoukoCode = False Then Exit Sub  '倉庫コードを変更入力してなければ以下の処理をしない

            If edtSoukoCode.Text = "" Then
                ChangeSoukoCode(False, Nothing)  '倉庫クリア
                Exit Sub
            End If

            '入力されたコードorカナに合致する倉庫一覧を表示（１件しかない時は、一覧を表示せずそのデータを得る）
            If FindSouko(False) Then
                'カーソル移動の制御（Leaveの中でTabStopを設定しても効かない）
                ControlFocusCode(sender)
            Else
                ChangeSoukoCode(False, Nothing)  '倉庫クリア
                edtSoukoCode.Select()
                edtSoukoCode.SelectionStart = 0  'フィールドを移動しないと、ハイライト表示が効かないため設定
                edtSoukoCode.SelectionLength = edtSoukoCode.Text.Length
            End If

        Finally
            isChangedSoukoCode = False
            If edtSoukoCode.Text = "" Then
                '倉庫コードが未入力でシフトもALTもマウスも押されていなければ、検索ボタンへ移動する
                If (Control.ModifierKeys And Keys.Shift) <> Keys.Shift AndAlso (Control.ModifierKeys And Keys.Alt) <> Keys.Alt AndAlso Control.MouseButtons = MouseButtons.None Then
                    btnSearchSouko.Select()
                End If
            End If
            SetCursorDefault()
        End Try
    End Sub

    '担当者コードの値を変更した時
    Private Sub edtTantouCode_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles edtTantouCode.TextChanged
        isChangedTantouCode = True
        Denpyou.Tantousha.Code = edtTantouCode.Text
        UpdateFlagOn()
    End Sub
    '担当者コードのフォーカス喪失時
    Private Sub edtTantouCode_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles edtTantouCode.Leave
        SetCursorWait()
        Try
            If isChangedTantouCode = False Then Exit Sub  '担当者コードを変更入力してなければ以下の処理をしない

            If edtTantouCode.Text = "" Then
                ChangeTantouCode(False, Nothing)  '担当者クリア
                Exit Sub
            End If

            '入力されたコードorカナに合致する担当者一覧を表示（１件しかない時は、一覧を表示せずそのデータを得る）
            If FindTantou(False) Then
                'カーソル移動の制御（Leaveの中でTabStopを設定しても効かない）
                ControlFocusCode(sender)
            Else
                ChangeTantouCode(False, Nothing)  '担当者クリア
                edtTantouCode.Select()
                edtTantouCode.SelectionStart = 0   'フィールドを移動しないと、ハイライト表示が効かないため設定
                edtTantouCode.SelectionLength = edtTantouCode.Text.Length
            End If

        Finally
            isChangedTantouCode = False
            If edtTantouCode.Text = "" Then
                '担当者コードが未入力でシフトもALTもマウスも押されていなければ、検索ボタンへ移動する
                If (Control.ModifierKeys And Keys.Shift) <> Keys.Shift AndAlso (Control.ModifierKeys And Keys.Alt) <> Keys.Alt AndAlso Control.MouseButtons = MouseButtons.None Then
                    btnSearchTantou.Select()
                End If
            End If
            SetCursorDefault()
        End Try
    End Sub

    '伝票コードの値を変更した時
    Private Sub edtDenpyouCode_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles edtDenpyouCode.TextChanged
        Denpyou.Code = edtDenpyouCode.Text
        UpdateFlagOn()
    End Sub
    '伝票コードのフォーカス喪失時
    Private Sub edtDenpyouCode_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles edtDenpyouCode.Leave
        SetCursorWait()
        '入力された文字を半角に変換し、設定桁の長さに前ゼロを付加
        edtDenpyouCode.Text = CFormCommon.GetAddZeroToCode(edtDenpyouCode.Text, drJisha("納品伝票コード桁数"))

        If isLeaveErrorNoCheck = False Then  'メニュー押下時は、チェックしない（UpdateRecordでチェック）
            CheckDenpyouCode()  '伝票コードの重複チェック
        End If
        SetCursorDefault()
    End Sub

    '納品日の値を変更した時
    Private Sub datNouhinDate_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles datNouhinDate.ValueChanged
        isChangedNouhinDate = True
        UpdateFlagOn()
    End Sub
    Private Sub datNouhinDate_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles datNouhinDate.Validated
        '入力可能日付の範囲をチェックしワーニングメッセージを表示
        CheckNouhinDate()

        If isChangedNouhinDate = False Then Exit Sub  '変更入力してなければ以下の処理をしない
        isChangedNouhinDate = False

        SetCursorWait()
        SheetRedrawOFF()

        Denpyou.NouhinDate = CDate(datNouhinDate.Value)

        '基準日が変わったので、在庫数のセットとチェックをやり直す
        CheckZaiko(True)

        SheetRedrawON()
        SetCursorDefault()
    End Sub

    '請求日の値を変更した時
    Private Sub datSeikyuDate_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles datSeikyuDate.ValueChanged
        isChangedSeikyuDate = True
        UpdateFlagOn()
    End Sub
    Private Sub datSeikyuDate_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles datSeikyuDate.Validated
        '入力可能日付の範囲をチェックしワーニングメッセージを表示
        CheckSeikyuDate()

        If isChangedSeikyuDate = False Then Exit Sub  '変更入力してなければ以下の処理をしない
        isChangedSeikyuDate = False

        SetCursorWait()
        SheetRedrawOFF()

        Denpyou.SeikyuDate = CDate(datSeikyuDate.Value)

        '消費税率を得、金額を再計算
        oldAryRate = Denpyou.aryRate.Clone  '変更前の消費税率をHold
        Dim CShouhiZei As New HanbaikanriDialog.CShouhiZei()
        Denpyou.aryRate = CShouhiZei.GetRate2(Denpyou.SeikyuDate)  '消費税率1,2取得
        If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
            '消費税率が変わった時
            WhenChangeRate()  '消費税率変更時、金額を再計算
            oldAryRate = Denpyou.aryRate.Clone  '変更後の消費税率をセット
        End If

        '請求日=納品日の時
        If drJisha("日付選択") = False Then
            Denpyou.NouhinDate = Denpyou.SeikyuDate

            '基準日が変わったので、在庫数のセットとチェックをやり直す
            CheckZaiko(True)
        End If

        '請求書発行済かどうかの表示
        If CheckSeikyuShoHakkou() Then
            lblSeikyuZumi.Visible = True
        Else
            lblSeikyuZumi.Visible = False
        End If

        SheetRedrawON()
        SetCursorDefault()
    End Sub

    '売上区分コンボを変更した時、名称を表示し直す
    Private Sub cmbUriageKubun_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbUriageKubun.SelectedIndexChanged
        If isInitialSet Then Exit Sub '初期設定時は処理しない
        isChangedUriageKubun = True
        If cmbUriageKubun.SelectedItem Is Nothing Then
            MessageBox.Show("売上区分を確認してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Denpyou.UriageKubun.MasterNo = cmbUriageKubun.SelectedItem.Value
            '増減がマイナスの区分については、背景色を赤にする
            If cmbUriageKubun.SelectedItem.Image < 0 Then
                '（コンボボックスに色が設定できないため、ラベルの色を変更）
                lblUriageKubun.BackColor = Color.Red
            Else
                lblUriageKubun.BackColor = Color.Transparent
            End If
        End If
        UpdateFlagOn()
    End Sub
    '売上区分のフォーカス喪失時
    Private Sub cmbUriageKubun_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles cmbUriageKubun.Leave
        SetCursorWait()
        SheetRedrawOFF()
        Try
            If isChangedUriageKubun = False Then Exit Sub  '変更入力してなければ以下の処理をしない

            Using cnTable As New SqlConnection(CSingleton.CSetting.Connect)
                cnTable.Open()
                '売上区分の増減を売上区分マスタから得る（更新時に使用）
                SetUriageKubunInfo(cnTable, Denpyou.UriageKubun.MasterNo)
            End Using

            If Denpyou.UriageKubun.Code = CON_UriageCode Then  '売上の時
                SetSheetPlus(False)  '明細入力はマイナスも可能とする
            Else
                'マイナスデータはプラスに変換
                For i As Integer = 0 To MRowSheet.MaxMRows - 1
                    MRowSheet.MRows(i)("セット数").Value = Math.Abs(MRowSheet.MRows(i)("セット数").Value)
                    MRowSheet.MRows(i)("数量").Value = Math.Abs(MRowSheet.MRows(i)("数量").Value)
                    MRowSheet.MRows(i)("原価単価").Value = Math.Abs(MRowSheet.MRows(i)("原価単価").Value)
                    MRowSheet.MRows(i)("商品単価").Value = Math.Abs(MRowSheet.MRows(i)("商品単価").Value)
                    MRowSheet.MRows(i)("金額").Value = Math.Abs(MRowSheet.MRows(i)("金額").Value)
                    MRowSheet.MRows(i)("税抜商品単価").Value = Math.Abs(MRowSheet.MRows(i)("税抜商品単価").Value)
                    MRowSheet.MRows(i)("税込商品単価").Value = Math.Abs(MRowSheet.MRows(i)("税込商品単価").Value)
                    MRowSheet.MRows(i)("税抜金額").Value = Math.Abs(MRowSheet.MRows(i)("税抜金額").Value)
                    MRowSheet.MRows(i)("税込金額").Value = Math.Abs(MRowSheet.MRows(i)("税込金額").Value)
                    MRowSheet.MRows(i)("税抜原価単価").Value = Math.Abs(MRowSheet.MRows(i)("税抜原価単価").Value)
                    MRowSheet.MRows(i)("税込原価単価").Value = Math.Abs(MRowSheet.MRows(i)("税込原価単価").Value)
                    MRowSheet.MRows(i)("税抜原価").Value = Math.Abs(MRowSheet.MRows(i)("税抜原価").Value)
                    MRowSheet.MRows(i)("消費税").Value = Math.Abs(MRowSheet.MRows(i)("消費税").Value)
                Next
                SetGoukei()
                SetSheetPlus(True)  '明細入力は、プラスのみ可能とする
            End If

        Finally
            isChangedUriageKubun = False
            If (Control.ModifierKeys And Keys.Shift) <> Keys.Shift AndAlso (Control.ModifierKeys And Keys.Alt) <> Keys.Alt AndAlso Control.MouseButtons = MouseButtons.None Then
                'シフトもALTもマウスも押されていなければ、アクティブセルを先頭にセット
                If MRowSheet.MaxMRows > 0 Then
                    If My.Settings.HanbaiKanriType = "C" Then
                        If My.Settings.EndUserName = "信和通信工業株式会社" Then
                            '*信和*　Cタイプでも受注は使用しない
                            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(0, enSheetCol1.商品コード, enSheetRow.Row1)
                        Else
                            If MRowSheet.MRows(0)("受注明細No").Value = 0 Then
                                MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(0, enSheetCol1.受注コード, enSheetRow.Row1)
                            Else
                                MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(0, enSheetCol1.商品コード, enSheetRow.Row1)
                            End If
                        End If
                    Else
                        MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(0, enSheetCol1.商品コード, enSheetRow.Row1)
                    End If
                End If
            End If
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '摘要の値を変更した時
    Private Sub edtTekiyou_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles edtTekiyou.TextChanged
        Denpyou.Tekiyou = edtTekiyou.Text
        UpdateFlagOn()
    End Sub

    '*信和*　但し書きの値を変更した時
    Private Sub edtTadasiGaki_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles edtTadasiGaki.TextChanged
        Denpyou.TadasiGaki = edtTadasiGaki.Text
        UpdateFlagOn()
    End Sub

    '*信和*　注文書送付のチェックを変更した時
    Private Sub chkKariDen_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkKariDen.CheckedChanged
        Denpyou.KariDen = chkKariDen.Checked
        UpdateFlagOn()
    End Sub

    '印刷フォーム/プリンタを変更した時、画面右上に選択したフォーム/プリンタを表示する
    Private Sub mnuCmbFormPrinter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles mnuCmbPrinter.SelectedIndexChanged, mnuCmbForm.SelectedIndexChanged
        Dim formNameOnly As String = ""
        If mnuCmbForm.ComboBox.SelectedValue IsNot Nothing AndAlso TypeOf mnuCmbForm.ComboBox.SelectedValue Is String Then
            formNameOnly = IO.Path.GetFileNameWithoutExtension(mnuCmbForm.ComboBox.SelectedValue)  'フルパスからフォーム名を得る
        End If

        If sender Is mnuCmbForm Then
            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                '*信和*　納品伝票フォームに"医師会"が含まれれば、管理資料プリンターを使用
                If formNameOnly.IndexOf("医師会") >= 0 Then
                    mnuCmbPrinter.ComboBox.SelectedIndex = mnuCmbPrinter.FindStringExact(My.Settings.KanriSiryouPrinterName)
                End If
            End If
        End If

        Dim printerName As String = ""
        If mnuCmbPrinter.SelectedIndex >= 0 AndAlso mnuCmbPrinter.SelectedItem IsNot Nothing Then
            printerName = mnuCmbPrinter.SelectedItem
        End If

        lblFormPrinter.Text = "◇" & formNameOnly & " / " & printerName
    End Sub

    '列幅が変更された後に発生。入力最終列以降の列幅が変更された時はゼロに戻す。
    Private Sub MRowSheet_ColumnWidthChanged(ByVal sender As Object, ByVal e As GrapeCity.Win.ElTabelle.ResizedEventArgs) Handles MRowSheet.ColumnWidthChanged
        If e.SpanSize.Size <= 0 Then Exit Sub '列幅がゼロに変更された時は処理しない

        SheetRedrawOFF()
        For iCol As Integer = e.SpanSize.Start To (e.SpanSize.Start + e.SpanSize.Count - 1)
            '変更開始列から変更列数まで繰り返す
            If iCol >= MRowSheet.MaxColumns Then Exit For
            If iCol > LastColumn Then
                MRowSheet.SetColumnWidth(iCol, 0)
            ElseIf My.Settings.HanbaiKanriType <> "C" AndAlso iCol = enSheetCol1.受注コード Then
                MRowSheet.SetColumnWidth(iCol, 0)
            ElseIf My.Settings.EndUserName = "信和通信工業株式会社" AndAlso iCol = enSheetCol1.受注コード Then
                '*信和*　Cタイプでも受注は使用しない
                MRowSheet.SetColumnWidth(iCol, 0)
            End If
        Next
        SheetRedrawON()
    End Sub

    'MultiRowSheetを抜ける時、カーソル位置を見えない場所に移動させる
    Private Sub MRowSheet_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles MRowSheet.Leave
        If MRowSheet.ActivePosition.MRow >= 0 Then
            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(MRowSheet.ActivePosition.MRow, enSheetCol1.テーブルNo, enSheetRow.Row1)
        End If
    End Sub

    'MultiRowSheetのセルが編集モードに入る時
    Private Sub MRowSheet_EnterEdit(ByVal sender As Object, ByVal e As GrapeCity.Win.ElTabelle.MEnterEditEventArgs) Handles MRowSheet.EnterEdit
        'カーソル移動不可を可能に戻す
        If My.Settings.HanbaiKanriType = "C" Then
            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                '*信和*　Cタイプでも受注は使用しない
            Else
                If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("受注明細検索ボタン").CanActivate = False Then
                    MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("受注明細検索ボタン").CanActivate = True
                End If
            End If
        End If
        If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品コード").CanActivate = False Then
            MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品コード").CanActivate = True
        End If
        If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品名称カナ").CanActivate = False Then
            MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品名称カナ").CanActivate = True
        End If
        If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品検索ボタン").CanActivate = False Then
            MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品検索ボタン").CanActivate = True
        End If
        If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品名称").CanActivate = False Then
            MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品名称").CanActivate = True
        End If
        If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("入数").CanActivate = False Then
            MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("入数").CanActivate = True
        End If
        If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("セット数").CanActivate = False Then
            MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("セット数").CanActivate = True
        End If
        If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("数量").CanActivate = False Then
            MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("数量").CanActivate = True
        End If
        If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("単位IN").CanActivate = False Then  '*江東高周波*
            MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("単位IN").CanActivate = True
        End If

        '日の出は、商品名にカーソル移動時、商品名の最後にカーソルをセットする
        '  （「MRowSheet.HighlightEditText=True」にしているとこの処理が効かないため、日の出はFalseとした）
        If My.Settings.EndUserName = "株式会社　日の出" Then
            If MRowSheet.ActiveCellKey = "商品名称" Then
                '商品名称の時、商品名の最後にカーソルを移動
                Dim objTextEditor As GrapeCity.Win.ElTabelle.Editors.TextEditor = MRowSheet.ActiveCell.Editor
                objTextEditor.SelectionStart = MRowSheet.ActiveCell.Text.Length  '選択テキストの開始位置を設定
                objTextEditor.SelectionLength = 0  '選択テキストの長さを設定
                MRowSheet.ActiveCell.Editor = objTextEditor
            Else
                '商品名称以外の項目は、テキストを選択状態にする（コンボ型はデフォルトで選択状態となる）
                If TypeOf MRowSheet.ActiveCell.Editor Is GrapeCity.Win.ElTabelle.Editors.TextEditor Then
                    '文字型セルの時
                    Dim objTextEditor As GrapeCity.Win.ElTabelle.Editors.TextEditor = MRowSheet.ActiveCell.Editor
                    objTextEditor.HighlightText = True
                    MRowSheet.ActiveCell.Editor = objTextEditor

                ElseIf TypeOf MRowSheet.ActiveCell.Editor Is GrapeCity.Win.ElTabelle.Editors.NumberEditor Then
                    '数値型セルの時
                    Dim objNumberEditor As GrapeCity.Win.ElTabelle.Editors.NumberEditor = MRowSheet.ActiveCell.Editor
                    objNumberEditor.SelectionStart = 0  '選択テキストの開始位置を設定
                    objNumberEditor.SelectionLength = MRowSheet.ActiveCell.Text.Length  '選択テキストの長さを設定
                    MRowSheet.ActiveCell.Editor = objNumberEditor
                End If
            End If
        End If
    End Sub

    'MultiRowSheetのセルが編集モードを抜ける時
    Private Sub MRowSheet_LeaveEdit(ByVal sender As Object, ByVal e As GrapeCity.Win.ElTabelle.MLeaveEditEventArgs) Handles MRowSheet.LeaveEdit
        'MultiRowSheetの編集前と後のチェック
        Dim currentPosition As GrapeCity.Win.ElTabelle.MPosition = MRowSheet.ActivePosition
        '  変更入力された時、変更をチェックする（変更前の値<>変更後の値）
        If MRowSheet(currentPosition).Value <> MRowSheet.ActiveCell.Value Then
            UpdateFlagOn()
        End If
    End Sub

    'MultiRowSheetのセルのイベントが発生した時、それぞれの項目の処理を行う
    Private Sub MRowSheet_CellNotify(ByVal sender As Object, ByVal e As GrapeCity.Win.ElTabelle.MCellNotifyEventArgs) Handles MRowSheet.CellNotify
        SetCursorWait()
        SheetRedrawOFF()
        Try
            If TypeOf MRowSheet.ActiveCell.Editor Is GrapeCity.Win.ElTabelle.Editors.ButtonEditor Then
                Select Case e.Name
                    Case GrapeCity.Win.ElTabelle.CellNotifyEvents.Click
                        'ボタンがクリックされた時の処理
                        If e.Position.Column = enSheetCol2.受注明細検索ボタン And e.Position.Row = enSheetRow.Row2 Then
                            '<受注明細検索ボタン>の時
                            FindJutyuMeisai("", MRowSheet.ActivePosition.MRow)

                        ElseIf e.Position.Column = enSheetCol1.商品検索ボタン And e.Position.Row = enSheetRow.Row1 Then
                            '<商品検索ボタン>の時
                            FindShouhin("", "", MRowSheet.ActivePosition.MRow)
                        End If

                        Exit Sub
                End Select
            End If

            Select Case e.Name
                Case GrapeCity.Win.ElTabelle.CellNotifyEvents.TextChanged
                    '入力されている文字列が変更された時の処理
                    UpdateFlagOn()

                    If e.Position.Column = enSheetCol1.受注コード AndAlso e.Position.Row = enSheetRow.Row1 Then
                        '<受注コード>列の時
                        isChangedJutyuCode = True  '変更した時、Leaveイベントで処理

                    ElseIf e.Position.Column = enSheetCol1.商品コード AndAlso e.Position.Row = enSheetRow.Row1 Then
                        '<商品コード>列の時
                        isChangedShouhinCode = True  '変更した時、Leaveイベントで処理

                    ElseIf e.Position.Column = enSheetCol1.商品名称カナ AndAlso e.Position.Row = enSheetRow.Row1 Then
                        '<商品名称カナ>列の時
                        isChangedShouhinKana = True  '変更した時、Leaveイベントで処理

                    ElseIf e.Position.Column = enSheetCol1.入数 AndAlso e.Position.Row = enSheetRow.Row1 Then
                        '<入数>列の時
                        '  数量の計算を行う
                        ChangeIriSu(MRowSheet.ActivePosition.MRow, MRowSheet.ActiveCell.Value)

                    ElseIf e.Position.Column = enSheetCol1.セット数 AndAlso e.Position.Row = enSheetRow.Row1 Then
                        '<セット数>列の時
                        '  数量の計算を行う
                        ChangeSetSu(MRowSheet.ActivePosition.MRow, MRowSheet.ActiveCell.Value)

                    ElseIf e.Position.Column = enSheetCol2.数量 AndAlso e.Position.Row = enSheetRow.Row2 Then
                        '<数量>列の時
                        '  金額の計算を行う
                        MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("数量").Value = MRowSheet.ActiveCell.Value
                        ChangeSuryou(MRowSheet.ActivePosition.MRow, MRowSheet.ActiveCell.Value)

                    ElseIf e.Position.Column = enSheetCol1.原価単価 AndAlso e.Position.Row = enSheetRow.Row1 Then
                        '<原価単価>列の時
                        '  原価単価、合計金額をセット
                        ChangeGenkaTanka(MRowSheet.ActivePosition.MRow, MRowSheet.ActiveCell.Value)

                    ElseIf e.Position.Column = enSheetCol2.商品単価 AndAlso e.Position.Row = enSheetRow.Row2 Then
                        '<単価>列の時
                        '  金額、合計金額をセット
                        ChangeTanka(MRowSheet.ActivePosition.MRow, MRowSheet.ActiveCell.Value)

                    ElseIf e.Position.Column = enSheetCol1.金額 AndAlso e.Position.Row = enSheetRow.Row1 Then
                        '<金額>列の時
                        MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("金額").Value = MRowSheet.ActiveCell.Value
                        '  金額、合計金額をセット
                        ChangeKingaku(MRowSheet.ActivePosition.MRow, MRowSheet.ActiveCell.Value)
                    End If

                Case GrapeCity.Win.ElTabelle.CellNotifyEvents.SelectedIndexChanged
                    '選択している項目が変更された時の処理
                    UpdateFlagOn()

                    If e.Position.Column = enSheetCol2.消費税率 AndAlso e.Position.Row = enSheetRow.Row2 Then
                        '<消費税率>列の時
                        MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("消費税率").Value = MRowSheet.ActiveCell.Value
                        '  単価、原価単価、金額、合計金額をセット
                        ChangeTaxRate(MRowSheet.ActivePosition.MRow)

                        MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("消費税率").Note = Nothing
                        If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品税区分").Value = enZeikubun.非課税 AndAlso MRowSheet.ActiveCell.Value <> 0 Then
                            Dim mRowRichTip As New GrapeCity.Win.ElTabelle.RichTip()
                            Dim rect As Rectangle = MRowSheet.PositionToRectangle(MRowSheet.ActivePosition)
                            Dim richText As String = "非課税の商品は0%以外にできません"  '→Leaveで0%にしている
                            mRowRichTip.ShowTip(MRowSheet, rect.Right - (rect.Width / 2), rect.Bottom, "消費税率変更", richText, GrapeCity.Win.ElTabelle.IconType.Error, 300)  '3秒表示

                        ElseIf Denpyou.Tokuisaki.ZeiKubun = enZeikubun.非課税 AndAlso MRowSheet.ActiveCell.Value <> 0 Then
                            Dim mRowRichTip As New GrapeCity.Win.ElTabelle.RichTip()
                            Dim rect As Rectangle = MRowSheet.PositionToRectangle(MRowSheet.ActivePosition)
                            Dim richText As String = "非課税の得意先は0%以外にできません"  '→Leaveで0%にしている
                            mRowRichTip.ShowTip(MRowSheet, rect.Right - (rect.Width / 2), rect.Bottom, "消費税率変更", richText, GrapeCity.Win.ElTabelle.IconType.Error, 300)  '3秒表示

                        ElseIf Denpyou.Tokuisaki.ZeiKubun = enZeikubun.内税 AndAlso MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品税区分").Value <> enZeikubun.非課税 Then
                            Dim mRowRichTip As New GrapeCity.Win.ElTabelle.RichTip()
                            Dim rect As Rectangle = MRowSheet.PositionToRectangle(MRowSheet.ActivePosition)
                            Dim richText As String = "単価を確認してください"
                            mRowRichTip.ShowTip(MRowSheet, rect.Right - (rect.Width / 2), rect.Bottom, "消費税率変更", richText, GrapeCity.Win.ElTabelle.IconType.Warning, 0)
                        End If
                    End If

                Case GrapeCity.Win.ElTabelle.CellNotifyEvents.CheckedChanged
                    'チェック状態が変更された時の処理
                    UpdateFlagOn()

                    If e.Position.Column = enSheetCol2.軽減税率 AndAlso e.Position.Row = enSheetRow.Row2 Then
                        '<軽減税率>列の時
                        MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("軽減税率").Value = MRowSheet.ActiveCell.Value
                        '  合計金額をセット
                        SetGoukei()

                        MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("軽減税率").Note = Nothing
                        If MRowSheet.ActiveCell.Value AndAlso MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("消費税率区分").Value <> enTaxRateKubun.税率2 Then
                            Dim mRowRichTip As New GrapeCity.Win.ElTabelle.RichTip()
                            Dim rect As Rectangle = MRowSheet.PositionToRectangle(MRowSheet.ActivePosition)
                            Dim richText As String = "「税率2」ではない商品に、軽減税率のチェックが付いています"
                            mRowRichTip.ShowTip(MRowSheet, rect.Right - (rect.Width / 2), rect.Bottom, "軽減税率チェック", richText, GrapeCity.Win.ElTabelle.IconType.Warning, 0)

                            CFormCommon.SetSelNote(MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("軽減税率"), "「税率2」ではない商品に、軽減税率のチェックが付いています")
                        End If
                    End If
            End Select

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    'MultiRowSheetのセルがフォーカスを失う時、各セルの処理
    Private Sub MRowSheet_LeaveCell(ByVal sender As Object, ByVal e As GrapeCity.Win.ElTabelle.MLeaveCellEventArgs) Handles MRowSheet.LeaveCell
        If isDeleting Then Exit Sub  '伝票削除の時は処理しない（伝票削除時に、このイベントが動いてしまうことがあるため）

        Dim mRow As Integer = MRowSheet.ActivePosition.MRow
        If mRow < 0 Then Exit Sub

        SetCursorWait()
        SheetRedrawOFF()
        Try
            If TypeOf MRowSheet.ActiveCell.Editor Is GrapeCity.Win.ElTabelle.Editors.TextEditor Then
                '（文字列セルをDeleteやBackSpaceキーで削除すると値がNullになってしまうため、""をセットし直す。CellNotifyでセットしても、Leaveに来るとNullになっていた。）
                If MRowSheet.ActiveCell.Value Is Nothing Then
                    MRowSheet.ActiveCell.Value = ""
                End If
            End If

            If mRow = (MRowSheet.MaxMRows - 1) _
              AndAlso (MRowSheet.ActivePosition.Column = LastColumn _
              OrElse (MRowSheet.ActivePosition.Column = enSheetCol1.金額 AndAlso MRowSheet.MRows(mRow)("備考").TabStop = False)) Then
                '<最終行の最終項目>の時、<備考のTabStopがFalseで金額の最終行>の時、
                '  フォーカスを摘要に移動させる
                If e.MoveStatus = GrapeCity.Win.ElTabelle.MoveStatus.NextCellWithWrap Then
                    edtTekiyou.Select()
                    e.Cancel = True  'セルのフォーカス喪失をキャンセル（キャンセルしないとLeaveでテーブルNoに移動させてもトップに移ってしまう）
                End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol1.受注コード AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row1 Then
                '<受注コード>列の時
                If isChangedJutyuCode Then  '受注コードを変更した時
                    If MRowSheet.ActiveCell.Text = "" Then
                        MRowSheet.MRows(mRow)("受注明細No").Value = 0
                    Else
                        '入力された文字を半角に変換し、設定桁の長さに前ゼロを付加
                        MRowSheet.ActiveCell.Text = CFormCommon.GetAddZeroToCode(MRowSheet.ActiveCell.Text, drJisha("受注伝票ｺｰﾄﾞ桁数"), False)

                        '入力された受注コードに合致する受注一覧を表示
                        If FindJutyuMeisai(MRowSheet.ActiveCell.Text, mRow) = False Then
                            MRowSheet.MRows(mRow)("受注明細No").Value = 0
                            e.Cancel = True  'セルの移動をキャンセル
                        End If
                    End If
                    isChangedJutyuCode = False
                End If

                '  受注コードが入力済の時、受注明細検索ボタンにカーソルを移動しない
                If e.Cancel = False AndAlso MRowSheet.ActiveCell.Text <> "" AndAlso e.MoveStatus = GrapeCity.Win.ElTabelle.MoveStatus.NextCellWithWrap Then
                    MRowSheet.ActiveCellKey = "商品コード"  'セルの移動
                    MRowSheet.MRows(mRow)("受注明細検索ボタン").CanActivate = False
                End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol2.受注明細検索ボタン AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row2 Then
                '<受注明細検索ボタン>列の時
                '  受注明細検索ボタンでEnterキーが押された時、受注明細検索画面を表示する
                '  （Enterキーがひろえないので、Tabキーが押されていないか、次の項目に移動しているかで判断）
                If e.MoveStatus = GrapeCity.Win.ElTabelle.MoveStatus.NextCellWithWrap AndAlso sheetKeyDownCode <> Keys.Tab Then
                    If FindJutyuMeisai("", mRow) = False Then
                        e.Cancel = True  'セルの移動をキャンセル（モードレス表示なので実際はキャンセルできない）
                    End If
                End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol1.商品コード AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row1 Then
                '<商品コード>列の時
                If isChangedShouhinCode Then  '商品コードを変更した時
                    If MRowSheet.ActiveCell.Text = "" Then
                        ChangeShouhin(False, mRow, Nothing) '商品クリア
                    Else
                        '入力された文字を半角に変換し、設定桁の長さに前ゼロを付加
                        MRowSheet.ActiveCell.Text = CFormCommon.GetAddZeroToCode(MRowSheet.ActiveCell.Text, drJisha("商品コード桁数"), drJisha("商品コード入力方法"))

                        '入力された商品コードに合致する商品一覧を表示（１件しかない時は、一覧を表示せずそのデータを得る）
                        If FindShouhin(MRowSheet.ActiveCell.Text, "", mRow) = False Then
                            ChangeShouhin(False, mRow, Nothing) '商品クリア
                            e.Cancel = True  'セルの移動をキャンセル
                        Else
                            'カーソル移動（セルの移動はFindShouhinで。CanActivateの設定はLeaveCellで）
                            If My.Settings.EndUserName = "株式会社　日の出" Then
                                '*日の出*  商品後、商品名称にカーソル移動させる
                                MRowSheet.MRows(mRow)("商品名称カナ").CanActivate = False
                            ElseIf My.Settings.EndUserName = "信和通信工業株式会社" AndAlso MRowSheet.MRows(mRow)("商品名称").Text.Trim = "" Then
                                '*信和*  商品名称未設定の商品時、商品名称にカーソル移動
                                MRowSheet.MRows(mRow)("商品名称カナ").CanActivate = False
                                'ElseIf My.Settings.EndUserName = "有限会社山田商店" Then
                                '    '*山田商店*　商品後、入数にカーソル移動
                                '    MRowSheet.MRows(mRow)("商品名称カナ").CanActivate = False
                                '    MRowSheet.MRows(mRow)("商品名称").CanActivate = False
                            Else
                                '商品後、入数＝ゼロなら数量に移動、入数≠ゼロならセット数に移動
                                If MRowSheet.MRows(mRow)("入数").Value = 0 Then
                                    MRowSheet.MRows(mRow)("入数").CanActivate = False
                                    MRowSheet.MRows(mRow)("セット数").CanActivate = False
                                Else
                                    MRowSheet.MRows(mRow)("入数").CanActivate = False
                                End If
                                MRowSheet.MRows(mRow)("商品名称カナ").CanActivate = False
                                MRowSheet.MRows(mRow)("商品名称").CanActivate = False
                            End If
                        End If
                    End If
                    isChangedShouhinCode = False
                End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol1.商品名称カナ AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row1 Then
                '<商品名称カナ>列の時
                If isChangedShouhinKana Then  '商品名称カナを変更した時
                    If MRowSheet.ActiveCell.Text <> "" AndAlso MRowSheet.MRows(mRow)("商品コード").Value = "" Then
                        Dim changedText As String = CFormCommon.GetChangedHankaku(MRowSheet.ActiveCell.Text)  '全角を半角に変換
                        If changedText <> MRowSheet.ActiveCell.Text Then
                            MRowSheet.ActiveCell.Text = changedText
                        End If
                        '入力された商品名称カナに合致する商品一覧を表示（１件しかない時は、一覧を表示せずそのデータを得る）
                        If FindShouhin("", MRowSheet.ActiveCell.Text, mRow) = False Then
                            e.Cancel = True  'セルの移動をキャンセル
                        Else
                            'カーソル移動（セルの移動はFindShouhinで。CanActivateの設定はLeaveCellで）
                            If My.Settings.EndUserName = "株式会社　日の出" Then
                                '*日の出*  商品後、商品名称にカーソル移動させる
                                MRowSheet.MRows(mRow)("商品名称カナ").CanActivate = False
                            ElseIf My.Settings.EndUserName = "信和通信工業株式会社" AndAlso MRowSheet.MRows(mRow)("商品名称").Text.Trim = "" Then
                                '*信和*  商品名称未設定の商品時、商品名称にカーソル移動
                                MRowSheet.MRows(mRow)("商品名称カナ").CanActivate = False
                                'ElseIf My.Settings.EndUserName = "有限会社山田商店" Then
                                '    '*山田商店*　商品後、入数にカーソル移動
                                '    MRowSheet.MRows(mRow)("商品名称カナ").CanActivate = False
                                '    MRowSheet.MRows(mRow)("商品名称").CanActivate = False
                            Else
                                '商品後、入数＝ゼロなら数量に移動、入数≠ゼロならセット数に移動
                                If MRowSheet.MRows(mRow)("入数").Value = 0 Then
                                    MRowSheet.MRows(mRow)("入数").CanActivate = False
                                    MRowSheet.MRows(mRow)("セット数").CanActivate = False
                                Else
                                    MRowSheet.MRows(mRow)("入数").CanActivate = False
                                End If
                                MRowSheet.MRows(mRow)("商品名称カナ").CanActivate = False
                                MRowSheet.MRows(mRow)("商品名称").CanActivate = False
                            End If
                        End If
                    End If
                    isChangedShouhinKana = False
                End If

                '  商品コード、商品名称カナのどちらかが入力済の時、商品検索ボタンにカーソルを移動しない
                If MRowSheet.MRows(mRow)("商品コード").Value <> "" OrElse MRowSheet.MRows(mRow)("商品名称カナ").Value <> "" Then
                    MRowSheet.ActiveCellKey = "商品名称"  'セルの移動
                    MRowSheet.MRows(mRow)("商品検索ボタン").CanActivate = False
                End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol1.商品検索ボタン AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row1 Then
                '<商品検索ボタン>列の時
                '  商品検索ボタンでEnterキーが押された時、商品検索画面を表示する
                '  （Enterキーがひろえないので、Tabキーが押されていないか、次の項目に移動しているかで判断）
                If e.MoveStatus = GrapeCity.Win.ElTabelle.MoveStatus.NextCellWithWrap AndAlso sheetKeyDownCode <> Keys.Tab Then
                    If FindShouhin("", "", mRow) = False Then
                        e.Cancel = True  'セルの移動をキャンセル
                    End If
                End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol2.商品名称 AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row2 Then
                '<商品名>列の時
                '  入数がゼロの時は数量にカーソル移動させる
                'If My.Settings.EndUserName = "有限会社山田商店" Then
                '    '*山田商店*　商品後のカーソル移動は入数のまま
                'Else
                If MRowSheet.MRows(mRow)("入数").Value = 0 Then
                    MRowSheet.ActiveCellKey = "数量"    'セルの移動
                    MRowSheet.MRows(mRow)("入数").CanActivate = False
                    MRowSheet.MRows(mRow)("セット数").CanActivate = False
                End If
                'End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol2.消費税率 AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row2 Then
                '<消費税率>列の時
                If (MRowSheet.MRows(mRow)("商品税区分").Value = enZeikubun.非課税 AndAlso MRowSheet.ActiveCell.Value <> 0) OrElse
                  (Denpyou.Tokuisaki.ZeiKubun = enZeikubun.非課税 AndAlso MRowSheet.ActiveCell.Value <> 0) Then
                    '非課税商品で0%以外の時/非課税得意先で0%以外の時、強制的に0%にする
                    MRowSheet.ActiveCell.Value = CDec(0)
                    MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("消費税率").Value = MRowSheet.ActiveCell.Value
                    '単価、原価単価、金額、合計金額をセット
                    ChangeTaxRate(MRowSheet.ActivePosition.MRow)

                    MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("消費税率").Note = Nothing
                End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol1.セット数 AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row1 Then
                '<セット数>列の時
                If MRowSheet.ActiveCell.Value = 0 Then
                    'MRowSheet.ActiveCellKey = "数量"  'セルの移動
                Else
                    If My.Settings.EndUserName = "信和通信工業株式会社" Then
                        MRowSheet.ActiveCellKey = "商品単価"  'セルの移動
                    Else
                        MRowSheet.ActiveCellKey = "単位IN"  'セルの移動
                    End If
                    MRowSheet.MRows(mRow)("数量").CanActivate = False
                End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol2.数量 AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row2 Then
                '<数量>列の時
                If My.Settings.EndUserName = "江東高周波工業" Then
                    If e.MoveStatus = GrapeCity.Win.ElTabelle.MoveStatus.NextCellWithWrap AndAlso sheetKeyDownCode <> Keys.Tab Then
                        '*江東高周波*  数量入力後Enereキーなら、単価にカーソル移動させる
                        MRowSheet.ActiveCellKey = "商品単価"  'セルの移動
                        MRowSheet.MRows(mRow)("単位IN").CanActivate = False
                    End If
                End If

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol2.単位IN AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row2 Then
                '<単位>列の時
                '  入力文字が指定の文字数を超えている時、指定の文字数分の文字列を取り出しセットする
                MRowSheet.ActiveCell.Text = StringsLeftB(MRowSheet.ActiveCell.Text, FormTanniLength)
                MRowSheet.MRows(mRow)("単位").Value = MRowSheet.ActiveCell.Text  '入力用列から、データバウンド用列にセット

            ElseIf MRowSheet.ActivePosition.Column = enSheetCol2.商品単価 AndAlso MRowSheet.ActivePosition.Row = enSheetRow.Row2 Then
                '<単価>列の時
                '  単価が得意先別単価と違う場合、得意先別単価を登録させる
                If MRowSheet.MRows(mRow)("商品単価").Value <> MRowSheet.MRows(mRow)("得意先別単価").Value Then
                    If isShowTankaMSG AndAlso MRowSheet.MRows(mRow)("商品マスタNo").Value <> 0 AndAlso MRowSheet.MRows(mRow)("商品マスタNo").Value <> defaultShouhin.MasterNo Then
                        Dim DefButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button2  '単価登録確認のデフォルトボタン「いいえ」
                        If My.Settings.EndUserName = "海政インキ株式会社" Then
                            '*海政*　単価登録確認のデフォルトボタンを「はい」とする
                            DefButton = MessageBoxDefaultButton.Button1
                        End If
                        If MessageBox.Show("この商品の単価を得意先別単価として登録しますか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, DefButton) = DialogResult.Yes Then
                            RegistTanka(mRow)  '得意先別単価の登録
                        End If

                        MRowSheet.MRows(mRow)("得意先別単価").Value = MRowSheet.MRows(mRow)("商品単価").Value
                        e.Cancel = True  'セルの移動をキャンセル（単価列のままとする）
                        Exit Sub
                    End If

                    MRowSheet.MRows(mRow)("得意先別単価").Value = MRowSheet.MRows(mRow)("商品単価").Value
                End If

                '  カーソル移動
                If MRowSheet.ActiveCell.Value = 0 Then
                    MRowSheet.MRows(mRow)("金額").Lock = False
                    If e.MoveStatus = GrapeCity.Win.ElTabelle.MoveStatus.NextCellWithWrap Then
                        '単価のあと次のセルへ移動する時、金額セルへ移動し備考セルへの移動をキャンセルする（ここでLockを解除しても一度セルを抜けないと効かない）
                        MRowSheet.ActiveCellKey = "金額"  'セルの移動
                        e.Cancel = True  '次のセル（備考）への移動をキャンセル
                    End If
                Else
                    '単価が入力されている時、金額は入力不可
                    MRowSheet.MRows(mRow)("金額").Lock = True
                    If e.MoveStatus = GrapeCity.Win.ElTabelle.MoveStatus.NextCellWithWrap Then
                        If My.Settings.EndUserName = "信和通信工業株式会社" Then
                            If mRow = (MRowSheet.MaxMRows - 1) Then
                                '最終行の時、摘要へ移動
                                edtTekiyou.Select()
                            Else
                                '次の行へ移動
                                MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(mRow + 1, enSheetCol1.商品コード, enSheetRow.Row1)
                            End If
                        Else
                            MRowSheet.ActiveCellKey = "備考"  'セルの移動
                        End If
                    End If
                End If

            End If

            sheetKeyDownCode = 0  '入力したKeyCodeをクリア（EnterキーではKeyDownイベントがおきないので、ここでクリア）

            '商品コード未入力で入力済行なら、デフォルト商品をセットする
            SetDefaultShouhin(mRow)

        Catch ex As Exception
            ErrProc(ex, Me.Text)

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    'MRowSheetでキーが押された時（Enter等のショートカットキーでは、このイベントがおきないので注意）
    Private Sub MRowSheet_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MRowSheet.KeyDown
        sheetKeyDownCode = e.KeyCode  '入力したKeyCodeを得る（Tabキーが押されたか、Enterが押されたかの判別に使用）
    End Sub

    ''データソースから、シートへのデータ入力時に不正データを検知した時に発生
    'Private Sub MRowSheet_MRowBindingError(sender As Object, e As GrapeCity.Win.ElTabelle.MRowBindingErrorEventArgs) Handles MRowSheet.MRowBindingError
    '    e.Ignore = True  '不正データ検知時、処理を継続する

    '    Dim dispMessage As Boolean = True  'エラーメッセージ表示

    '    If e.Position.Column = enSheetCol2.消費税率 AndAlso e.Position.Row = enSheetRow.Row2 Then
    '        '消費税率コンボボックスの時
    '        '　コンボ型セルに設定されていない値が入力された場合、コンボボックスに値を追加
    '        '　（対象のセルにのみ設定しているが、何故か他行のコンボボックスにも追加されてしまう）
    '        Dim cmbEditor As GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor = MRowSheet(e.Position).Editor
    '        cmbEditor.Items.Add(New GrapeCity.Win.ElTabelle.Editors.ComboItem(0, Nothing, CDec(e.Value).ToString("#0%").PadLeft(3), "", e.Value))
    '        MRowSheet(e.Position).Editor = cmbEditor
    '        MRowSheet(e.Position).Value = e.Value

    '        dispMessage = False  'エラーメッセージは表示しない

    '    Else
    '        '上記以外の不正データの場合は、Nullに置き換えられる
    '    End If

    '    If dispMessage Then
    '        MessageBox.Show("エラーデータがあります。データ内容を確認して下さい。" & vbCrLf & vbCrLf &
    '            "（" & (e.Position.MRow + 1).ToString & "行目、" & e.Position.Column.ToString & "列目：" & e.Value.ToString & "）", "MRowBindingError", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End If
    'End Sub

    '新規伝票入力
    Private Sub AllNew()
        If UpdateCaution() = False Then
            Exit Sub
        End If

        MakeEditCommandsDenpyou(0)  'テーブルNo=0でSelect

        Dim rowNewDenpyou As DataRow = dtDenpyou.NewRow  '行を生成
        dtDenpyou.Rows.Add(rowNewDenpyou)  '行をDataTableに追加

        InitDenpyou()
        SetForm(False, False)
        MRowSheet.Enabled = False

        Me.Text = TITLE & "（新規）"
        lblShusei.Visible = False
        'If Me.Visible Then  '（画面Load前にSelectすると背景色が付かないため、画面非表示時はSelectしない）
        edtTokuiCode.Select()
        'End If
    End Sub

    '次伝票の新規入力（同じ得意先で新規の伝票）
    Private Sub NextNew()
        If UpdateCaution() = False Then
            Exit Sub
        End If

        MakeEditCommandsDenpyou(0)  'テーブルNo=0でSelect

        Dim rowNewDenpyou As DataRow = dtDenpyou.NewRow  '行を生成
        dtDenpyou.Rows.Add(rowNewDenpyou)  '行をDataTableに追加

        InitMeisai()
        SetForm(False, False)

        Me.Text = TITLE & "（新規）"
        lblShusei.Visible = False

        If My.Settings.EndUserName = "株式会社　日の出" Then
            '日の出は、明細の商品コードにカーソル移動させる
            MRowSheet.Select()
            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(0, enSheetCol1.商品コード, enSheetRow.Row1)
        Else
            If My.Settings.EndUserName = "山田商店" Then
                '*山田商店*　納入先がある時は納入先にカーソル移動し、ない時は日付にカーソル移動
                ControlFocusCode(edtTokuiCode)
            Else
                If drJisha("日付選択") = False Then
                    datSeikyuDate.Select()
                Else
                    datNouhinDate.Select()
                End If
            End If
        End If
    End Sub

    '納品明細の行挿入（選択行の上に１行追加）
    Private Sub InsertLine(ByVal targetMRow As Integer)
        If MRowSheet.Enabled = False Then Exit Sub
        If targetMRow < 0 Then Exit Sub

        'すでにMax行なら挿入しない
        Dim meisaiMaxRow As Integer = DataInSheet(MRowSheet)
        If meisaiMaxRow >= Denpyou.KoumokuSu Then
            Exit Sub
        End If

        '交互色を設定し直す（背景色を変更したセルは交互色より背景色が優先されるため、一度背景色をクリアしてから交互色を再設定）
        CFormCommon.SetAlternateColor(MRowSheet, 0, LastColumn)

        '１行追加
        InsertMrowNullMeisai(targetMRow)  'カーソル位置に１行挿入
        MRowSheet.RemoveMRow(MRowSheet.MaxMRows - 1, False)  '挿入した分、最終行を削除
        UpdateFlagOn()
    End Sub

    '納品明細の行削除
    Private Sub DeleteLine(ByVal targetMRow As Integer)
        If MRowSheet.Enabled = False Then Exit Sub
        If targetMRow < 0 Then Exit Sub

        If MessageBox.Show("納品明細の " & (targetMRow + 1) & "行目を削除してよろしいですか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _
          <> DialogResult.Yes Then
            Exit Sub
        End If

        '交互色を設定し直す（背景色を変更したセルは交互色より背景色が優先されるため、一度背景色をクリアしてから交互色を再設定）
        CFormCommon.SetAlternateColor(MRowSheet, 0, LastColumn)

        MRowSheet.RemoveMRow(targetMRow, False)  '行削除
        InsertMrowNullMeisai(MRowSheet.MaxMRows)  '削除した分の行を追加
        SetGoukei()  '合計金額をセット
        UpdateFlagOn()
    End Sub

    '選択した行を上に移動
    Private Sub RowUp(ByVal targetMRow As Integer)
        If targetMRow <= 0 Then Exit Sub

        '交互色を設定し直す（背景色を変更したセルは交互色より背景色が優先されるため、一度背景色をクリアしてから交互色を再設定）
        CFormCommon.SetAlternateColor(MRowSheet, 0, LastColumn)

        InsertMrowNullMeisai(targetMRow - 1)  '１行上に行を挿入
        Dim orgCmbEditor As GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor = MRowSheet.MRows(targetMRow + 1)("消費税率").Editor
        MRowSheet.MRows(targetMRow - 1)("消費税率").Editor = orgCmbEditor  'デフォルトの消費税率以外の税率がある時、データのセット(Move)より先にコンボボックスを設定しておかないと、後ろの値が全てセットされなくなってしまう（画面上は表示されていても登録すると値がセットされていない）
        MRowSheet.MRows(targetMRow + 1).Move(targetMRow - 1, GrapeCity.Win.ElTabelle.DataTransferMode.All)  '選択行を挿入した行に移動
        MRowSheet.RemoveMRow(targetMRow + 1, False)  '移動した行を削除
        UpdateFlagOn()
    End Sub

    '選択した行を下に移動
    Private Sub RowDown(ByVal targetMRow As Integer)
        If targetMRow < 0 Then Exit Sub
        If targetMRow >= (MRowSheet.MaxMRows - 1) Then Exit Sub

        '交互色を設定し直す（背景色を変更したセルは交互色より背景色が優先されるため、一度背景色をクリアしてから交互色を再設定）
        CFormCommon.SetAlternateColor(MRowSheet, 0, LastColumn)

        InsertMrowNullMeisai(targetMRow + 2)  '１行下に行を挿入
        Dim orgCmbEditor As GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor = MRowSheet.MRows(targetMRow)("消費税率").Editor
        MRowSheet.MRows(targetMRow + 2)("消費税率").Editor = orgCmbEditor
        MRowSheet.MRows(targetMRow).Move(targetMRow + 2, GrapeCity.Win.ElTabelle.DataTransferMode.All)  '選択行を挿入した行に移動
        MRowSheet.RemoveMRow(targetMRow, False)  '移動した行を削除
        UpdateFlagOn()
    End Sub

    '納品伝票、納品明細の削除処理
    Private Function DeleteRecord() As Boolean
        If MessageBox.Show("この納品伝票を" & vbCrLf & "削除します。よろしいですか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) _
          <> DialogResult.Yes Then
            Return True
        End If

        '請求書発行済の日付かどうかをチェック
        If CheckSeikyuShoMsg("削除") = False Then
            Return True  '削除しない
        End If

        isDeleting = True  '削除処理中
        Try
            Using cnDenpyou As New SqlConnection(CSingleton.CSetting.Connect)
                cnDenpyou.Open()

                Using trDenpyou As SqlTransaction = cnDenpyou.BeginTransaction(IsolationLevel.Serializable)  'トランザクションの開始
                    '他で変更されたかをチェック
                    If Denpyou.TableNo > 0 Then
                        If CDenpyouCommon.CheckDBChanged("納品伝票", "納品明細", "削除", Denpyou.TableNo, iMeisaiCnt, dtMeisai, cnDenpyou, trDenpyou) Then
                            trDenpyou.Rollback()
                            Return True
                        End If
                    End If

                    '納品明細、納品伝票の削除
                    dtMeisai.RejectChanges()  'DataTableに加えられた変更を取り消す（明細の変更内容が更新されてから、削除されてしまうため）
                    SetSheetPlus(False)  '明細はマイナスも可能とする
                    MRowSheet.DataSource = dtMeisai
                    For mRow As Integer = MRowSheet.MaxMRows - 1 To 0 Step -1
                        MRowSheet.RemoveMRow(mRow, False)
                    Next
                    dtDenpyou.Rows(0).Delete()

                    CDBCommon.SetConnectionToDataAdapter(daMeisai, cnDenpyou)
                    CDBCommon.SetTransactionToDataAdapter(daMeisai, trDenpyou)
                    daMeisai.Update(dtMeisai)  '明細から先に削除

                    CDBCommon.SetConnectionToDataAdapter(daDenpyou, cnDenpyou)
                    CDBCommon.SetTransactionToDataAdapter(daDenpyou, trDenpyou)
                    daDenpyou.Update(dtDenpyou)

                    '現金売り
                    If Denpyou.Tokuisaki.Simebi = enSimeDay.現金 AndAlso Denpyou.NyukinNo <> 0 Then
                        If MessageBox.Show("この納品伝票には同時に登録した入金伝票が有ります。" & vbCrLf & "入金伝票を削除します。よろしいですか？", Me.Text,
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                            Dim sSQL As String
                            Dim sUpdateDate As String = "更新日時 = GetDate(), 更新ユーザー = '" & HanbaikanriDialog.CSingleton.CCommonPara.LoginUserName & "', 更新コンピュータ名 = '" & My.Computer.Name & "'"

                            '他で変更されたかをチェック（変更すると元伝に削除フラグがたち、新規で登録される）
                            sSQL = "SELECT 削除 FROM 入金伝票 WHERE テーブルNo = " & Denpyou.NyukinNo
                            Dim bDelete As Object = CDBCommon.SQLExecuteScalar(CSingleton.CSetting.Connect, sSQL) '単一データを取得
                            If bDelete Is Nothing OrElse bDelete Then
                                MessageBox.Show("同時に登録した入金伝票は、ほかで更新/削除されたため" & vbCrLf & "削除できませんでした。（納品伝票は削除しました）", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If

                            '入金伝票削除
                            sSQL = "UPDATE 入金伝票 SET 削除 = 1, 修正済 = 1, 修正済締日 = 1, " & sUpdateDate & " WHERE テーブルNo = " & Denpyou.NyukinNo & " AND 削除 = 0"
                            If CDBCommon.SQLExecute(cnDenpyou, sSQL, trDenpyou) = False Then
                                trDenpyou.Rollback()
                                Return False
                            End If

                            '入金明細削除
                            sSQL = "UPDATE 入金明細 SET 削除 = 1, " & sUpdateDate & " WHERE 入金伝票No = " & Denpyou.NyukinNo & " AND 削除 = 0"
                            If CDBCommon.SQLExecute(cnDenpyou, sSQL, trDenpyou) = False Then
                                trDenpyou.Rollback()
                                Return False
                            End If
                        End If
                    End If

                    trDenpyou.Commit()
                End Using
            End Using

            isUpdated = True  '更新済
            isChanged = False

            If isNewInputtable = False Then
                '他の画面から呼ばれて来た時、削除したら終了する
                EndDenpyou()  '終了
                Return True
            End If

            AllNew()  '新規入力状態にする

            Return True

        Catch ex As Exception
            ErrProc(ex, Me.Text)
            Return False

        Finally
            isDeleting = False
        End Try
    End Function

    '納品伝票の検索
    '　[続けて検索]にした場合、対象の伝票を削除/変更した時は、再検索する必要あり
    Private Sub FindDenpyou()
        '納品明細が入力済みなら、登録するかどうかの確認を行う
        If DataInSheet(MRowSheet) > 0 Then
            If UpdateCaution() = False Then
                Exit Sub
            End If
        End If
        Try
            'ほかの検索画面が開いていたら閉じる
            CloseFormListIfOpend(frmNouhinList)

            '納品伝票の検索画面表示
            If frmNouhinList Is Nothing OrElse frmNouhinList.IsDisposed Then
                '伝票検索画面が表示されていない時
                frmNouhinList = New frmNouhinList("納品伝票検索", AddressOf Me.NouhinListCallBack)  '伝票検索画面から動かしたいアドレスを渡す
                frmNouhinList.SearchTokuiCode = edtTokuiCode.Text
                If isNewInputtable = False Then  '他画面から納品伝票を開いた時（元帳や請求書から伝票変更する時等）
                    frmNouhinList.TokuisakiDisabled = True  '納品伝票検索で、得意先を他に変更できないようにする
                End If
                frmNouhinList.SearchTantouCode = edtTantouCode.Text
                frmNouhinList.PrintForm = Denpyou.NouhinDenpyou
                frmNouhinList.MeisaiOKbtn = False
                If frmNouhinList.BeforeLoad() Then
                    '一覧画面表示（エラー発生時は一覧画面を表示しない）
                    frmNouhinList.Opener = Me
                    'frmNouhinList.Show(Me)  '自分を所有者としてモードレスで表示する（自分の後ろに隠れないようにする/自分を閉じると所有される側も同時に閉じる）
                    frmNouhinList.Show()
                End If
            Else
                '伝票検索画面が表示済の時
                If frmNouhinList.WindowState = FormWindowState.Minimized Then
                    frmNouhinList.WindowState = FormWindowState.Normal  '最小化されていた場合、通常に戻す
                End If
                frmNouhinList.BringToFront()  '最前面に表示する
            End If

        Catch ex As Exception
            ErrProc(ex, Me.Text)
        End Try
    End Sub
    '伝票検索のモードレスウインドウから選択したテーブルNoがコールバックされる
    Private Sub NouhinListCallBack(ByVal SelectedTableNo As Integer)
        SetCursorWait()
        SheetRedrawOFF()
        Try
            '納品明細が入力済みなら、登録するかどうかの確認を行う
            If DataInSheet(MRowSheet) > 0 Then
                If UpdateCaution() = False Then
                    Exit Sub
                End If
            End If

            If SelectedTableNo > 0 Then
                '検索結果を表示
                SetCursorWait()
                SheetRedrawOFF()
                If GetRecord(SelectedTableNo, False) > 0 Then
                    MRowSheet.Enabled = True
                    SetForm(True, False, False)
                    Me.Text = TITLE & "（修正）"
                    lblShusei.Visible = True
                    edtTokuiCode.Select()
                    Me.BringToFront()  '最前面に表示する
                End If
            End If

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '納品伝票の印刷
    '  引数：Preview = True:プレビュー , False:印刷
    Private Sub PrintDenpyou(ByVal Preview As Boolean)
        '明細行の有効行をカウント
        Dim meisaiMaxRow As Integer = DataInSheet(MRowSheet)
        If meisaiMaxRow <= 0 Then
            Exit Sub
        End If

        '新規登録で伝票コードが自動採番の時は、伝票コードが確定しないと印刷できない
        If Denpyou.Tokuisaki.DenpyouCodeUpdate AndAlso Denpyou.TableNo <= 0 Then
            If isChanged Then
                If MessageBox.Show("印刷するためには伝票を登録する必要が有ります。" & vbCrLf & "この納品伝票を登録しますか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _
                  = DialogResult.Yes Then
                    If Not UpdateRecord() Then
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
            End If
        End If

        Dim CNouhinDenpyouPrint As New CNouhinDenpyouPrint()
        Try
            'OPEN
            '　ツールメニューで選択した納品伝票フォーム/プリンタを使用する
            If mnuCmbForm.ComboBox.SelectedValue Is Nothing Then
                MessageBox.Show("納品伝票フォームを選択してください", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                mnuPrintMenu.ShowDropDown()
                mnuCmbForm.Select()
                Exit Sub
            End If
            Dim selectedForm As String = mnuCmbForm.ComboBox.SelectedValue  'Valueにフルパス
            If mnuCmbPrinter.SelectedIndex < 0 OrElse mnuCmbPrinter.SelectedItem Is Nothing Then
                MessageBox.Show("納品伝票プリンタを選択してください", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                mnuPrintMenu.ShowDropDown()
                mnuCmbPrinter.Select()
                Exit Sub
            End If
            Dim selectedPrinter As String = mnuCmbPrinter.SelectedItem
            If CNouhinDenpyouPrint.OpenPrinter(selectedForm, selectedPrinter) = False Then
                Exit Sub
            End If

            'プリントデータのセット
            Dim DenpyouData As New CNouhinDenpyouPrint.Denpyou

            With DenpyouData
                'ヘッダ部のセット
                .Code = Denpyou.Code
                .TokuisakiNo = Denpyou.Tokuisaki.MasterNo
                .TokuisakiZeiKubun = Denpyou.Tokuisaki.ZeiKubun
                .ShouhizeiKeisan = Denpyou.Tokuisaki.ShouhizeiKeisan
                .NounyusakiNo = Denpyou.NounyuuSaki.MasterNo
                .NounyusakiCode = Denpyou.NounyuuSaki.Code
                .NounyusakiName = Denpyou.NounyuuSaki.Name
                .NounyusakiName2 = Denpyou.NounyuuSaki.Name2
                .NounyusakiKeisho = Denpyou.NounyuuSaki.Keishou
                .SoukoNo = Denpyou.Souko.MasterNo
                .TantoushaNo = Denpyou.Tantousha.MasterNo
                .TantoushaName = Denpyou.Tantousha.Name
                .NouhinDate = Denpyou.NouhinDate
                .SeikyuDate = Denpyou.SeikyuDate
                .UriageKubunCode = Denpyou.UriageKubun.Code
                .UriageKubunName = cmbUriageKubun.Text
                .Tekiyou = Denpyou.Tekiyou

                '0:総合計、1～:税率別計
                ReDim .Goukei(sheetGoukei.MaxRows - 1)
                For idx As Integer = 0 To sheetGoukei.MaxRows - 1
                    .Goukei(idx).TaxRateText = sheetGoukei(enSheetGoukeiCol.消費税率テキスト, idx).Value
                    .Goukei(idx).TaxRate = sheetGoukei(enSheetGoukeiCol.消費税率, idx).Value
                    .Goukei(idx).KeigenZeiritsu = If(sheetGoukei(enSheetGoukeiCol.消費税率テキスト, idx).Text.IndexOf("軽") >= 0, True, False)
                    .Goukei(idx).ZeinukiGaku += sheetGoukei(enSheetGoukeiCol.税抜額, idx).Value * Denpyou.UriageKubun.Zougen
                    .Goukei(idx).ShouhiZeiGaku += sheetGoukei(enSheetGoukeiCol.消費税額, idx).Value * Denpyou.UriageKubun.Zougen
                    .Goukei(idx).GoukeiGaku += sheetGoukei(enSheetGoukeiCol.合計, idx).Value * Denpyou.UriageKubun.Zougen
                Next

                '消費税率1種類のフォームを使用する時
                If .Goukei.Length > 1 Then
                    .Rate1 = .Goukei(1).TaxRate  '入力した最大の消費税率
                Else
                    .Rate1 = Denpyou.aryRate(0)
                End If

                '明細部のセット
                ReDim .Meisai(meisaiMaxRow - 1)
                For mRow As Integer = 0 To meisaiMaxRow - 1
                    With .Meisai(mRow)
                        .Code = MRowSheet.MRows(mRow)("商品コード").Text
                        .Name = MRowSheet.MRows(mRow)("商品名称").Text
                        .TaxRate = MRowSheet.MRows(mRow)("消費税率").Value
                        .KeigenZeiritsu = MRowSheet.MRows(mRow)("軽減税率").Value
                        .IriSu = MRowSheet.MRows(mRow)("入数").Value
                        .SetSu = MRowSheet.MRows(mRow)("セット数").Value
                        .Suryou = MRowSheet.MRows(mRow)("数量").Value * Denpyou.UriageKubun.Zougen
                        .Tanni = MRowSheet.MRows(mRow)("単位").Text
                        .Tanka = MRowSheet.MRows(mRow)("商品単価").Value
                        .Kingaku = MRowSheet.MRows(mRow)("金額").Value * Denpyou.UriageKubun.Zougen
                        .ShouhiZei = MRowSheet.MRows(mRow)("消費税").Value * Denpyou.UriageKubun.Zougen
                        If Denpyou.Tokuisaki.ZeiKubun <> enZeikubun.非課税 Then
                            If Denpyou.Tokuisaki.ShouhizeiKeisan = enZeiKeisan.取引時 Then
                                '取引時は、明細の消費税額を円単位に丸める（明細毎以外は小数以下あり）
                                .ShouhiZei = Marume(.ShouhiZei, 0, Denpyou.Tokuisaki.Hasuu)
                            ElseIf Denpyou.Tokuisaki.ShouhizeiKeisan = enZeiKeisan.請求時 Then
                                '請求時は、消費税ゼロ
                                .ShouhiZei = 0
                            End If
                        End If
                        .Bikou = MRowSheet.MRows(mRow)("備考").Text
                        .Zeikubun = MRowSheet.MRows(mRow)("商品税区分").Value
                    End With
                Next
            End With

            If CNouhinDenpyouPrint.PrintDenpyouData(DenpyouData) = False Then
                MessageBox.Show("PrintDenpyouDataエラー。プリント出来ません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'プリント/プレビュー
            If Preview Then
                CNouhinDenpyouPrint.Preview()
            Else
                CNouhinDenpyouPrint.Print(drJisha("印刷ダイアログ"))
            End If

        Catch ex As Exception
            CNouhinDenpyouPrint.DisposePrintForm()
            ErrProc(ex, Me.Text)
        End Try
    End Sub

    '領収書の印刷（信和のみ）
    Private Sub PrintRyoushuSho(ByVal Preview As Boolean)
        '新規登録で伝票コードが自動採番の時は、伝票コードが確定しないと印刷できない
        If Denpyou.Tokuisaki.DenpyouCodeUpdate AndAlso Denpyou.TableNo = 0 Then
            If isChanged Then
                If MessageBox.Show("印刷するためには伝票を登録する必要が有ります。" & vbCrLf & "この納品伝票を登録しますか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _
                  = DialogResult.Yes Then
                    If Not UpdateRecord() Then
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
            End If
        End If

        '明細行がなければ印刷しない
        If DataInSheet(MRowSheet) <= 0 Then
            Exit Sub
        End If

        Try
            'OPEN
            '　ツールメニューで選択した領収書フォーム/プリンタを使用する
            If mnuCmbFormRyoushuSho.ComboBox.SelectedValue Is Nothing Then
                MessageBox.Show("領収書フォームを選択してください", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                mnuPrintMenu.ShowDropDown()
                mnuCmbFormRyoushuSho.Select()
                Exit Sub
            End If
            Dim selectedForm As String = mnuCmbFormRyoushuSho.ComboBox.SelectedValue  'Valueにフルパス
            If mnuCmbPrinterRyoushuSho.SelectedIndex < 0 OrElse mnuCmbPrinterRyoushuSho.SelectedItem Is Nothing Then
                MessageBox.Show("領収書プリンタを選択してください", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                mnuPrintMenu.ShowDropDown()
                mnuCmbPrinterRyoushuSho.Select()
                Exit Sub
            End If
            Dim selectedPrinter As String = mnuCmbPrinterRyoushuSho.SelectedItem
            Dim frmPrintForm As New frmPrintForm
            If frmPrintForm.PrintOpen(selectedForm, selectedPrinter, , Me.Text, CSingleton.CSetting.Connect) = False Then
                frmPrintForm.Dispose()
                Exit Sub
            End If

            'プリントデータのセット
            With frmPrintForm

                .set_TagData("ナンバー", Denpyou.Code)
                .set_TagData("但", Denpyou.TadasiGaki)

                .set_TagData("年", DateFormat(Denpyou.NouhinDate, drJisha("和暦"), "yyyy", "ggyy"))
                .set_TagData("年号無年", DateFormat(Denpyou.NouhinDate, drJisha("和暦"), "yyyy", "yy"))
                .set_TagData("元号", DateYearFormatGengou(Denpyou.NouhinDate, drJisha("和暦")))
                .set_TagData("月", Denpyou.NouhinDate.Month)
                .set_TagData("日", Denpyou.NouhinDate.Day)

                Dim sNameWork As String = Denpyou.Tokuisaki.Name
                If Denpyou.Tokuisaki.Name2 <> "" Then
                    sNameWork &= vbCrLf & Denpyou.Tokuisaki.Name2
                End If
                .set_TagData("名称", sNameWork)
                .set_TagData("名称1", Denpyou.Tokuisaki.Name)
                .set_TagData("名称2", Denpyou.Tokuisaki.Name2)
                .set_TagData("敬称", Denpyou.Tokuisaki.Keishou)

                If Denpyou.Tokuisaki.ZeiKubun <> enZeikubun.非課税 AndAlso Denpyou.Tokuisaki.ShouhizeiKeisan = enZeiKeisan.請求時 Then
                End If
                If Denpyou.Tokuisaki.ZeiKubun = enZeikubun.外税 Then
                    .set_TagData("税抜", "○")
                Else
                    .set_TagData("税込", "○")
                End If

                For idx As Integer = 0 To sheetGoukei.MaxRows - 1
                    Dim idxText As String
                    If idx = 0 Then
                        idxText = ""
                    Else
                        If sheetGoukei(enSheetGoukeiCol.消費税率, idx).Value = 0 Then Continue For  '非課税は表示しない（非課税はsheetGoukeiの最下行）
                        idxText = idx.ToString("#0")
                    End If

                    If idx <> 0 Then
                        .set_TagData("消費税率" & idxText, sheetGoukei(enSheetGoukeiCol.消費税率, idx).Value * 100)
                        .set_TagData("軽減税率" & idxText, If(sheetGoukei(enSheetGoukeiCol.消費税率テキスト, idx).Text.IndexOf("軽") >= 0, "軽", ""))
                    Else
                        If Denpyou.Tokuisaki.ZeiKubun = enZeikubun.非課税 OrElse Denpyou.Tokuisaki.ShouhizeiKeisan = enZeiKeisan.請求時 Then
                            '非課税 または 請求時（内税、外税）の時、消費税率は表示しない
                        Else
                            If sheetGoukei.MaxRows > 1 Then
                                .set_TagData("消費税率", sheetGoukei(enSheetGoukeiCol.消費税率, 1).Value * 100)  '入力した最大の消費税率
                            Else
                                .set_TagData("消費税率", Denpyou.aryRate(0) * 100)
                            End If
                        End If
                    End If
                    .set_TagData("今回買上額" & idxText, sheetGoukei(enSheetGoukeiCol.税抜額, idx).Value * Denpyou.UriageKubun.Zougen)
                    .set_TagData("消費税" & idxText, sheetGoukei(enSheetGoukeiCol.消費税額, idx).Value * Denpyou.UriageKubun.Zougen)
                    .set_TagData("金額" & idxText, sheetGoukei(enSheetGoukeiCol.合計, idx).Value * Denpyou.UriageKubun.Zougen)
                Next

            End With

            'プリント/プレビュー
            If Preview Then
                frmPrintForm.Preview(FormWindowState.Maximized)  'プレビュー
            Else
                frmPrintForm.Print(drJisha("印刷ダイアログ"))  '直接プリント
                frmPrintForm.Dispose()
            End If

        Catch ex As Exception
            ErrProc(ex, Me.Text)
        End Try
    End Sub

    '納品伝票、納品明細の更新処理
    Private Function UpdateRecord() As Boolean
        Try
            Me.Validate()

            If Denpyou.Tokuisaki.MasterNo = 0 Then  '得意先が未入力の時
                Return True
            End If

            If isChanged = False Then  '未変更の時
                Return True
            End If

            Dim sSQL As String

            'エラーチェック
            If edtTokuiCode.Text.Trim = "" Then
                MessageBox.Show("得意先を入力してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                edtTokuiCode.Select()
                Return False
            End If
            If edtNounyuuCode.Text <> "" AndAlso Denpyou.NounyuuSaki.MasterNo = 0 Then
                MessageBox.Show("納入先が正しくありません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                edtNounyuuCode.Select()
                Return False
            End If
            If Denpyou.Souko.MasterNo = 0 Then
                MessageBox.Show("倉庫を入力してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                edtSoukoCode.Select()
                Return False
            End If
            If edtTantouCode.Text <> "" AndAlso Denpyou.Tantousha.MasterNo = 0 Then
                MessageBox.Show("担当者が正しくありません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                edtTantouCode.Select()
                Return False
            End If
            If edtDenpyouCode.Text.Trim = "" AndAlso edtDenpyouCode.Enabled Then  '自動更新でない時のみチェック
                MessageBox.Show("伝票コードを入力してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                edtDenpyouCode.Select()
                Return False
            End If
            If datNouhinDate.Value Is Nothing Then
                MessageBox.Show("納品日を入力してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                datNouhinDate.Select()
                Return False
            End If
            If datSeikyuDate.Value Is Nothing Then
                MessageBox.Show(lblSeikyuDate.Text & "を入力してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                datSeikyuDate.Select()
                Return False
            End If
            If drJisha("日付選択") Then
                If Denpyou.NouhinDate >= Date.Today.AddYears(5) Then
                    MessageBox.Show("５年以上先の納品日は登録できません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    datNouhinDate.Select()
                    Return False
                End If
            End If
            If Denpyou.SeikyuDate >= Date.Today.AddYears(5) Then
                MessageBox.Show("５年以上先の" & lblSeikyuDate.Text & "は登録できません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                datSeikyuDate.Select()
                Return False
            End If

            '請求書発行済の日付かどうかをチェック
            If CheckSeikyuShoMsg(If(isSearchedDenpyou, "変更", "登録")) = False Then
                Return True  '登録しない
            End If

            '検索された伝票の時（修正する伝票の時）
            If isSearchedDenpyou Then
                If MessageBox.Show("すでに登録された伝票です。変更を上書きしてもよろしいですか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
                  = DialogResult.No Then
                    Return True
                End If
            End If

            '納品明細が入力されているかどうかのチェック
            Dim meisaiMaxRow As Integer = 0
            Dim mRow As Integer = MRowSheet.MaxMRows - 1
            Do While mRow >= 0
                If DataInSheet(mRow, MRowSheet) Then  'データが入力済の行かどうか
                    meisaiMaxRow = mRow + 1  '入力済明細行の最終行数
                    Exit Do
                End If
                '未入力行を削除（ゴミ行が残ってしまうことがあるため削除）
                MRowSheet.RemoveMRow(mRow, False)
                'InsertMrowNullMeisai(mRow)  '（InsertするとUpdate時にNullデータが出力されてしまう）
                mRow -= 1
            Loop
            If meisaiMaxRow <= 0 Then
                isChanged = False
                Return True
            End If
            For mRow = 0 To MRowSheet.MaxMRows - 1
                '不正な商品コードはエラー
                If MRowSheet.MRows(mRow)("商品コード").Text <> "" AndAlso GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value) = 0 Then
                    MessageBox.Show("商品コードが正しくありません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    InsertMrowNullMeisai()  '未入力行を削除したので、伝票項目数分の空明細行を追加（ActivePositionでLeaveCellが動きRedrawONになってしまうため、Finallyでなくここで追加）
                    MRowSheet.Select()
                    MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(mRow, enSheetCol1.商品コード, enSheetRow.Row1)
                    Return False
                End If
                '不正な受注コードはエラー
                If My.Settings.HanbaiKanriType = "C" Then
                    If MRowSheet.MRows(mRow)("受注コード").Text <> "" AndAlso GetInt(MRowSheet.MRows(mRow)("受注明細No").Value) = 0 Then
                        MessageBox.Show("受注コードが正しくありません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        InsertMrowNullMeisai()  '未入力行を削除したので、伝票項目数分の空明細行を追加（ActivePositionでLeaveCellが動きRedrawONになってしまうため、Finallyでなくここで追加）
                        MRowSheet.Select()
                        MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(mRow, enSheetCol1.受注コード, enSheetRow.Row1)
                        Return False
                    End If
                End If

                '新規行の時、入力されている明細行に納品伝票（親）の納品伝票Noをセットする
                If GetInt(MRowSheet.MRows(mRow)("納品伝票No").Value) = 0 Then
                    MRowSheet.MRows(mRow)("納品伝票No").Value = dtDenpyou.Rows(0)("テーブルNo")
                End If

                '行番号をセット
                MRowSheet.MRows(mRow)("行番号").Value = mRow + 1

                '納品伝票の倉庫をセット
                MRowSheet.MRows(mRow)("倉庫マスタNo").Value = Denpyou.Souko.MasterNo

                '手入力商品の税区分をセット
                If GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value) = 0 Then
                    MRowSheet.MRows(mRow)("商品税区分").Value = Denpyou.Tokuisaki.ZeiKubun
                End If
            Next

            Using cnDenpyou As New SqlConnection(CSingleton.CSetting.Connect)
                cnDenpyou.Open()

                '修正の時、他で変更されたかをチェック（トランザクション前にもチェック。他で変更があった時、コード重複エラーになってしまうことがあるため）
                If Denpyou.TableNo > 0 Then
                    If CDenpyouCommon.CheckDBChanged("納品伝票", "納品明細", "修正", Denpyou.TableNo, iMeisaiCnt, dtMeisai, cnDenpyou, Nothing) Then
                        Return False
                    End If
                End If

                '伝票コードの重複チェック
                If Not CheckDenpyouCode(True, cnDenpyou) Then
                    Return False
                End If

                '総合計
                Dim dZeinuki As Decimal = sheetGoukei(enSheetGoukeiCol.税抜額, enSheetGoukeiRow.合計行).Value * Denpyou.UriageKubun.Zougen  '税抜額
                Dim dGoukei As Decimal = sheetGoukei(enSheetGoukeiCol.合計, enSheetGoukeiRow.合計行).Value * Denpyou.UriageKubun.Zougen  '合計金額
                Dim dShouhiZei As Decimal  '消費税
                If Denpyou.Tokuisaki.ZeiKubun <> enZeikubun.非課税 AndAlso Denpyou.Tokuisaki.ShouhizeiKeisan = enZeiKeisan.請求時 Then
                    '消費税計算方法=請求時の時
                    dShouhiZei = sheetGoukei(enSheetGoukeiCol.参考消費税, enSheetGoukeiRow.合計行).Value * Denpyou.UriageKubun.Zougen
                Else
                    dShouhiZei = sheetGoukei(enSheetGoukeiCol.消費税額, enSheetGoukeiRow.合計行).Value * Denpyou.UriageKubun.Zougen
                End If

                '与信限度額のチェック（最新の締日元帳の残高を使用）
                '  （締日元帳の繰越金額＋今回の売上金額）が、得意先マスタの与信限度額を超えたらワーニングメッセージ
                If Denpyou.Tokuisaki.YosinGendo <> 0 Then
                    sSQL = "SELECT TOP 1 繰越金額 FROM 売上締日元帳 " _
                         & "WHERE 請求先マスタNo = " & Denpyou.Tokuisaki.SeikyuSaki & " " _
                         & "ORDER BY 締日 DESC"
                    Dim kurikoshiGaku As Object = CDBCommon.SQLExecuteScalar(cnDenpyou, sSQL) '単一データを取得
                    If kurikoshiGaku Is Nothing Then
                        kurikoshiGaku = 0
                    End If
                    Dim zeikomiGaku As Decimal = dZeinuki + Marume(dShouhiZei, 0, Denpyou.Tokuisaki.Hasuu)
                    If Denpyou.Tokuisaki.YosinGendo < (kurikoshiGaku + zeikomiGaku) Then  '税込額でチェック
                        If MessageBox.Show("与信限度額を超えています。登録してよろしいですか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) _
                          = DialogResult.No Then
                            Return False
                        End If
                    End If
                End If

                Using trDenpyou As SqlTransaction = cnDenpyou.BeginTransaction(IsolationLevel.Serializable)  'トランザクションの開始
                    Try
                        '修正の時、他で変更されたかをチェック
                        If Denpyou.TableNo > 0 Then
                            If CDenpyouCommon.CheckDBChanged("納品伝票", "納品明細", "修正", Denpyou.TableNo, iMeisaiCnt, dtMeisai, cnDenpyou, trDenpyou) Then
                                trDenpyou.Rollback()
                                Return False
                            End If
                        End If

                        '納品伝票の各項目セット
                        Dim drDenpyou As DataRow = dtDenpyou.Rows(0)
                        drDenpyou("得意先マスタNo") = Denpyou.Tokuisaki.MasterNo
                        drDenpyou("納入先マスタNo") = Denpyou.NounyuuSaki.MasterNo
                        drDenpyou("納入先コード") = Denpyou.NounyuuSaki.Code
                        drDenpyou("納入先名称") = Denpyou.NounyuuSaki.Name
                        drDenpyou("納入先名称2") = Denpyou.NounyuuSaki.Name2
                        drDenpyou("納入先敬称") = Denpyou.NounyuuSaki.Keishou
                        drDenpyou("倉庫マスタNo") = Denpyou.Souko.MasterNo
                        drDenpyou("担当者マスタNo") = Denpyou.Tantousha.MasterNo
                        drDenpyou("コード") = Denpyou.Code  '（新規登録で自動更新の時は[自動更新]とセットされるので、採番後セットし直している）
                        drDenpyou("処理日") = Denpyou.SeikyuDate
                        If drJisha("日付選択") = False Then
                            drDenpyou("納品日") = Denpyou.SeikyuDate
                        Else
                            drDenpyou("納品日") = Denpyou.NouhinDate
                        End If
                        drDenpyou("売上区分マスタNo") = Denpyou.UriageKubun.MasterNo
                        drDenpyou("摘要") = Denpyou.Tekiyou

                        drDenpyou("税抜額") = dZeinuki
                        drDenpyou("合計金額") = dGoukei
                        drDenpyou("消費税") = dShouhiZei
                        drDenpyou("返品額") = 0  '現在未使用

                        drDenpyou("得意先税区分") = Denpyou.Tokuisaki.ZeiKubun
                        drDenpyou("消費税計算方法") = Denpyou.Tokuisaki.ShouhizeiKeisan
                        drDenpyou("端数") = Denpyou.Tokuisaki.Hasuu

                        drDenpyou("月次元帳No") = 0
                        drDenpyou("締日元帳No") = 0
                        drDenpyou("請求書No") = DBNull.Value
                        drDenpyou("入金伝票テーブルNo") = Denpyou.NyukinNo
                        drDenpyou("見積書No") = Denpyou.MitumoriNo
                        If My.Settings.HanbaiKanriType = "C" Then
                            drDenpyou("受注伝票No") = Denpyou.JutyuDenpyouNo
                        End If
                        drDenpyou("仕入伝票No") = Denpyou.SiireDenpyouNo
                        If Denpyou.TableNo > 0 Then
                            '修正の時、元伝票のテーブルNoをセット
                            drDenpyou("修正元テーブルNo") = Denpyou.TableNo
                        Else
                            drDenpyou("修正元テーブルNo") = 0
                        End If

                        If My.Settings.EndUserName = "信和通信工業株式会社" Then
                            '*信和*　信和のみの追加項目
                            drDenpyou("仮伝票") = Denpyou.KariDen
                            drDenpyou("但し書き") = Denpyou.TadasiGaki
                        End If

                        '修正時、納品伝票の値を変更しているかどうかチェックし、値変更していない時は元に戻す
                        Dim isChangedDenpyou As Boolean = True
                        If Denpyou.TableNo > 0 Then  '修正時
                            Dim jogaiList As New List(Of String)() From {"テーブルNo", "月次元帳No", "締日元帳No", "請求書No", "修正元テーブルNo", "修正済締日", "削除", "修正済", "登録日時", "更新日時", "更新ユーザー", "更新コンピュータ名"}  '値変更チェックの除外項目
                            If CDBCommon.CheckChangedDataRow(drDenpyou, jogaiList) = False Then
                                '値変更していない時、変更無し状態に戻す
                                drDenpyou.RejectChanges()
                                isChangedDenpyou = False
                            End If
                        End If

                        '納品伝票を変更した時（新規伝票の時、修正の時は納品伝票の内容を変更した時）
                        If isChangedDenpyou Then
                            '現金売りの時、入金伝票の登録を行う
                            If Denpyou.Tokuisaki.Simebi = enSimeDay.現金 Then
                                If MessageBox.Show("入金伝票" & If(Denpyou.NyukinNo > 0, "も修正", "に登録") & "しますか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
                                  = DialogResult.Yes Then
                                    Using frmNyukin As New frmNyukin
                                        Dim nyukinNo As Integer = Denpyou.NyukinNo
                                        If Not frmNyukin.RegistorNyukin(cnDenpyou, trDenpyou, nyukinNo, Denpyou.Tokuisaki.SeikyuSaki, sheetGoukei(enSheetGoukeiCol.合計, enSheetGoukeiRow.合計行).Value, Denpyou.SeikyuDate) Then
                                            trDenpyou.Rollback()
                                            MessageBox.Show("入金伝票" & If(Denpyou.NyukinNo > 0, "を修正", "に登録") & "できませんでした。" & vbCrLf & "内容を確認してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            Return False
                                        End If
                                        drDenpyou("入金伝票テーブルNo") = nyukinNo
                                    End Using
                                End If
                            End If

                            '納品伝票コード自動更新で新規登録の場合、納品伝票現コードを採番し更新
                            If Denpyou.Tokuisaki.DenpyouCodeUpdate AndAlso Denpyou.TableNo <= 0 Then
                                Dim masterName As String
                                Dim genCode As Decimal
                                If Denpyou.Tokuisaki.Hyoujun OrElse Not Denpyou.Tokuisaki.DenpyouCodeFlag Then  '標準ﾌｫｰﾑ
                                    '自社情報マスタを読む
                                    masterName = "自社情報"
                                    Dim drJishaGenCode As DataRow = Nothing
                                    Dim CJisha As New CJisha
                                    If CJisha.GetJisha(drJishaGenCode, cnDenpyou, trDenpyou) = False Then
                                        trDenpyou.Rollback()
                                        MessageBox.Show("自社情報マスタにデータがありません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        Return False
                                    End If
                                    genCode = drJishaGenCode("納品伝票現コード")
                                Else
                                    '得意先マスタを読む
                                    masterName = "得意先マスタ"
                                    Dim CTokuisaki As New HanbaikanriDialog.CTokuisaki()
                                    Dim drTokuiGenCode As DataRow = CTokuisaki.GetMaster(Denpyou.Tokuisaki.SeikyuSaki, cnDenpyou, trDenpyou)
                                    If drTokuiGenCode Is Nothing Then
                                        trDenpyou.Rollback()
                                        MessageBox.Show("得意先マスタにデータがありません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                                        Return False
                                    End If
                                    genCode = drTokuiGenCode("納品伝票現コード")
                                End If

                                Denpyou.Code = genCode.ToString(New String("0", drJisha("納品伝票コード桁数")))
                                drDenpyou("コード") = Denpyou.Code

                                '納品伝票現コードを更新（次回使用するコードをセット）
                                Dim newCode As Decimal = GetCountUpNextCode(genCode, drJisha("納品伝票コード桁数"), 1)  '次回の伝票コードを採番
                                sSQL = "UPDATE " & masterName & " SET [納品伝票現コード] = " & newCode
                                If masterName = "得意先マスタ" Then
                                    sSQL &= " WHERE マスタNo=" & Denpyou.Tokuisaki.SeikyuSaki
                                End If
                                If CDBCommon.SQLExecute(cnDenpyou, sSQL, trDenpyou) = False Then
                                    trDenpyou.Rollback()
                                    Return False
                                End If

                                edtDenpyouCode.MaxLength = drJisha("納品伝票コード桁数")
                                edtDenpyouCode.Text = Denpyou.Code
                                edtDenpyouCode.Format = "9"
                                edtDenpyouCode.Enabled = True
                            End If
                        End If

                        '納品伝票更新
                        CDBCommon.SetConnectionToDataAdapter(daDenpyou, cnDenpyou)
                        CDBCommon.SetTransactionToDataAdapter(daDenpyou, trDenpyou)
                        Dim denpyouUpdateCnt As Integer  '納品伝票の更新件数
                        denpyouUpdateCnt = daDenpyou.Update(dtDenpyou)

                        If Denpyou.TableNo > 0 AndAlso Denpyou.TableNo <> drDenpyou("テーブルNo") Then
                            '修正時、仕入伝票への連動データがあれば、仕入伝票の納品伝票Noを新テーブルNoに変更する
                            sSQL = "UPDATE 仕入伝票 SET 納品伝票No = " & drDenpyou("テーブルNo") _
                                 & " WHERE 納品伝票No = " & Denpyou.TableNo & " AND 削除 = 0"
                            If CDBCommon.SQLExecute(cnDenpyou, sSQL, trDenpyou) = False Then
                                trDenpyou.Rollback()
                                Return False
                            End If
                        End If


                        '納品明細更新
                        '  増減を掛けて、プラス or マイナスにする
                        SetSheetPlus(False)  '明細はマイナスも可能とする
                        For i As Integer = 0 To MRowSheet.MaxMRows - 1
                            SetDBSheetPlus(False, i)  '増減を掛ける
                        Next

                        '  変更していない行は、元に戻す（金額のプラス/マイナスで、実際に変更無しのデータも変更になってしまう。同じ値を入力しても変更になってしまう。）
                        If Denpyou.TableNo > 0 AndAlso denpyouUpdateCnt <= 0 Then  '修正で、納品伝票が未更新の時
                            Dim drMeisai As DataRow
                            Dim jogaiList As New List(Of String)() From {"テーブルNo", "納品伝票No", "削除", "修正済", "登録日時", "更新日時", "更新ユーザー", "更新コンピュータ名"}  '値変更チェックの除外項目
                            For i As Integer = 0 To dtMeisai.Rows.Count - 1
                                drMeisai = dtMeisai.Rows(i)
                                If drMeisai.RowState <> DataRowState.Modified Then Continue For

                                If CDBCommon.CheckChangedDataRow(drMeisai, jogaiList) = False Then
                                    '値変更していない時、変更無し状態に戻す
                                    drMeisai.RejectChanges()
                                End If
                            Next
                        End If

                        CDBCommon.SetConnectionToDataAdapter(daMeisai, cnDenpyou)
                        CDBCommon.SetTransactionToDataAdapter(daMeisai, trDenpyou)
                        daMeisai.Update(dtMeisai)

                        trDenpyou.Commit()

                    Catch ex As SqlException
                        If trDenpyou.Connection IsNot Nothing Then trDenpyou.Rollback()
                        If ex.Number = 1205 Then  'デッドロック時
                            MessageBox.Show("他の更新処理とバッティングし、登録できませんでした。" & vbCrLf & "再度「登録」ボタンを押して更新してください。" & vbCrLf & vbCrLf &
                                            "<エラー内容>" & vbCrLf & ex.Source & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return False
                        Else
                            ErrProc(ex, Me.Text)
                            Return False
                        End If

                    Catch ex As Exception
                        If trDenpyou.Connection IsNot Nothing Then trDenpyou.Rollback()
                        ErrProc(ex, Me.Text)
                        Return False
                    End Try
                End Using
            End Using

            If Denpyou.TableNo = 0 Then  '追加の時
                Denpyou.NewCode = CDec(Denpyou.Code) + 1
            End If

            '登録/修正したデータを取得し直す
            If GetRecord(dtDenpyou.Rows(0)("テーブルNo"), False) > 0 Then
                SetForm(True, False, False)
            End If

            isUpdated = True  '更新済

            Me.Text = TITLE & "（修正）"
            lblShusei.Visible = True
            edtTokuiCode.Select()

            Return True

        Catch ex As Exception
            ErrProc(ex, Me.Text)
            Return False

        Finally
            InsertMrowNullMeisai()  '未入力行を削除したので、伝票項目数分の空明細行を追加
        End Try
    End Function

    '複写入力（複写元の伝票を検索して、新規作成）
    Private Sub CopyDenpyou()
        '納品明細が入力済みなら、登録するかどうかの確認を行う
        If DataInSheet(MRowSheet) > 0 Then
            If UpdateCaution() = False Then
                Exit Sub
            End If
        End If
        Try
            'ほかの検索画面が開いていたら閉じる
            CloseFormListIfOpend(frmNouhinListCopy)

            '納品伝票の検索画面表示
            If frmNouhinListCopy Is Nothing OrElse frmNouhinListCopy.IsDisposed Then
                '伝票検索画面が表示されていない時
                frmNouhinListCopy = New frmNouhinList("納品伝票複写", AddressOf Me.NouhinListCopyCallBack)
                frmNouhinListCopy.SearchTokuiCode = edtTokuiCode.Text
                frmNouhinListCopy.SearchTantouCode = edtTantouCode.Text
                frmNouhinListCopy.PrintForm = Denpyou.NouhinDenpyou
                frmNouhinListCopy.MeisaiOKbtn = False
                If frmNouhinListCopy.BeforeLoad() Then
                    '一覧画面表示（エラー発生時は一覧画面を表示しない）
                    frmNouhinListCopy.Opener = Me
                    frmNouhinListCopy.Show()
                End If
            Else
                '伝票検索画面が表示済の時
                If frmNouhinListCopy.WindowState = FormWindowState.Minimized Then
                    frmNouhinListCopy.WindowState = FormWindowState.Normal  '最小化されていた場合、通常に戻す
                End If
                frmNouhinListCopy.BringToFront()  '最前面に表示する
            End If

        Catch ex As Exception
            ErrProc(ex, Me.Text)
        End Try
    End Sub
    '伝票検索のモードレスウインドウから選択したテーブルNoがコールバックされる
    Private Sub NouhinListCopyCallBack(ByVal SelectedTableNo As Integer)
        SetCursorWait()
        SheetRedrawOFF()
        Try
            '納品明細が入力済みなら、登録するかどうかの確認を行う
            If DataInSheet(MRowSheet) > 0 Then
                If UpdateCaution() = False Then
                    Exit Sub
                End If
            End If

            If SelectedTableNo > 0 Then
                '検索結果を表示
                SetCursorWait()
                SheetRedrawOFF()
                If GetRecord(SelectedTableNo, True) > 0 Then
                    MRowSheet.Enabled = True
                    SetForm(True, False, True)
                    If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
                        WhenChangeRate()  '消費税率変更時、金額を再計算
                    End If
                    CheckNewTanka()  '商品単価、原価単価が最新かどうかチェック
                    UpdateFlagOn()
                    Me.Text = TITLE & "（新規）"
                    lblShusei.Visible = False
                    edtTokuiCode.Select()
                    Me.BringToFront()  '最前面に表示する
                End If
            End If

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '*長万部*  長万部用 複写入力
    '  伝票検索画面を表示させず、指定得意先の最新の伝票を複写する。日付は＋１して表示する。
    Private Sub CopyPrevReference()
        Try
            '納品明細が入力済みなら、登録するかどうかの確認を行う
            If DataInSheet(MRowSheet) > 0 Then
                If UpdateCaution() = False Then
                    Exit Sub
                End If
            End If

            '該当得意先の、処理日・伝票コードのなかで最新の伝票を取得
            Dim sSQL As String
            sSQL = "SELECT 納品伝票.テーブルNo " _
                 & "FROM 納品伝票 " _
                 & "WHERE 納品伝票.得意先マスタNo = " & Denpyou.Tokuisaki.MasterNo & " AND 納品伝票.削除 = 0 " _
                 & "ORDER BY 納品伝票.処理日 DESC, 納品伝票.コード DESC"
            Dim iTableNo As Object = CDBCommon.SQLExecuteScalar(CSingleton.CSetting.Connect, sSQL)
            If iTableNo Is Nothing Then
                MessageBox.Show("検索条件に該当する伝票がありません", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            If iTableNo > 0 Then
                '検索結果を表示
                If GetRecord(iTableNo, True) > 0 Then
                    MRowSheet.Enabled = True
                    SetForm(True, False, True)
                    If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
                        WhenChangeRate()  '消費税率変更時、金額を再計算
                    End If
                    CheckNewTanka()  '商品単価、原価単価が最新かどうかチェック
                    UpdateFlagOn()
                    Me.Text = TITLE & "（新規）"
                    lblShusei.Visible = False
                    edtTokuiCode.Select()
                End If
            End If

        Catch ex As Exception
            ErrProc(ex, Me.Text)
        End Try
    End Sub

    'コピー（現在表示している内容を別ウィンドウの新規登録画面に複写する）
    Private frmNouhinCopy As frmNouhin
    Private Sub CopyNew()
        '既に開いていたら、二重表示しない
        If frmNouhinCopy IsNot Nothing AndAlso frmNouhinCopy.Visible Then
            MessageBox.Show("既にコピー画面を表示しています。" & vbCrLf & "再度コピーするには、一度コピー画面を閉じてからコピーし直してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            frmNouhinCopy.BringToFront()  '最前面に表示する
            Exit Sub
        End If

        '新規登録時は、登録しないとコピーできない（伝票コードがだぶってしまうため）
        If Denpyou.TableNo <= 0 AndAlso isChanged Then
            If MessageBox.Show("コピーするには伝票を登録する必要が有ります。" & vbCrLf & "この伝票を登録しますか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _
              = DialogResult.Yes Then
                If UpdateRecord() = False Then
                    Exit Sub
                End If
            Else
                Exit Sub
            End If
        End If

        '別画面で表示する
        frmNouhinCopy = New frmNouhin(True)

        With frmNouhinCopy  '別画面のインスタンスに設定する
            .SheetRedrawOFF()

            .SetInit()  '画面表示の初期設定

            .isNewInputtable = True  '新規伝票入力を可とする

            .InitDenpyou()

            '現在の内容をコピーする前の状態を保存（複写しない項目）
            Dim orgTableNo As Integer = .Denpyou.TableNo
            Dim orgSeikyuDate As Date = .Denpyou.SeikyuDate
            Dim orgNouhinDate As Date = .Denpyou.NouhinDate
            Dim orgKariDen As Boolean = .Denpyou.KariDen
            Dim orgNyukinNo As Integer = .Denpyou.NyukinNo
            Dim orgRendouSakiSiireCode As String = .Denpyou.RendouSakiSiireCode
            Dim orgRendouSakiNyukinCode As String = .Denpyou.RendouSakiNyukinCode
            Dim orgAryRate() As Decimal = .Denpyou.aryRate.Clone

            .Denpyou = Denpyou  '現在の内容を新規画面にコピー

            .Denpyou.Code = Denpyou.NewCode.ToString(New String("0", drJisha("納品伝票コード桁数")))

            .Denpyou.TableNo = orgTableNo
            .Denpyou.SeikyuDate = orgSeikyuDate
            .Denpyou.NouhinDate = orgNouhinDate
            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                .Denpyou.KariDen = orgKariDen
            End If
            .Denpyou.NyukinNo = orgNyukinNo
            .Denpyou.RendouSakiSiireCode = orgRendouSakiSiireCode
            .Denpyou.RendouSakiNyukinCode = orgRendouSakiNyukinCode
            .Denpyou.aryRate = orgAryRate.Clone

            .sheetGoukei(enSheetGoukeiCol.税抜額, enSheetGoukeiRow.合計行).Value = sheetGoukei(enSheetGoukeiCol.税抜額, enSheetGoukeiRow.合計行).Value
            .sheetGoukei(enSheetGoukeiCol.消費税額, enSheetGoukeiRow.合計行).Value = sheetGoukei(enSheetGoukeiCol.消費税額, enSheetGoukeiRow.合計行).Value
            .sheetGoukei(enSheetGoukeiCol.合計, enSheetGoukeiRow.合計行).Value = sheetGoukei(enSheetGoukeiCol.合計, enSheetGoukeiRow.合計行).Value
            .sheetGoukei(enSheetGoukeiCol.参考消費税, enSheetGoukeiRow.合計行).Value = sheetGoukei(enSheetGoukeiCol.参考消費税, enSheetGoukeiRow.合計行).Value

            '請求先マスタNoをキーに、得意先マスタのレコードを得る
            Dim CSeikyu As New HanbaikanriDialog.CTokuisaki()
            Dim drSeikyu As DataRow = CSeikyu.GetMaster(.Denpyou.Tokuisaki.SeikyuSaki)
            If drSeikyu Is Nothing Then
                MessageBox.Show("コピー元納品伝票の請求先が見つかりません", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                frmNouhinCopy.Dispose()
                Exit Sub
            End If

            'コピー元の税区分or消費税計算方法or端数が、得意先マスタの現在の設定と違う時、注意メッセージを表示
            Dim message As String = ""
            If drSeikyu("税区分") <> Denpyou.Tokuisaki.ZeiKubun Then
                message = "税区分"
            End If
            If (drSeikyu("税区分") <> enZeikubun.非課税 AndAlso Denpyou.Tokuisaki.ZeiKubun <> enZeikubun.非課税) _
                  AndAlso drSeikyu("消費税計算方法") <> Denpyou.Tokuisaki.ShouhizeiKeisan Then
                If message <> "" Then message &= "・"
                message &= "消費税計算方法"
            End If
            If drSeikyu("端数") <> Denpyou.Tokuisaki.Hasuu Then
                If message <> "" Then message &= "・"
                message &= "端数"
            End If
            If message <> "" Then
                MessageBox.Show("コピー元の" & message & "が、現在の得意先マスタの設定と違います。" & vbCrLf & "内容を確認してください。" _
                        & vbCrLf & "（得意先を入力し直すと、最新の設定になります。）", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If


            '納品伝票 追加
            .MakeEditCommandsDenpyou(0)  'テーブルNo=0でSelect

            Dim rowNewDenpyou As DataRow = frmNouhinCopy.dtDenpyou.NewRow  '行を生成
            .dtDenpyou.Rows.Add(rowNewDenpyou)  '行をDataTableに追加

            '納品明細 追加
            Dim meisaiMaxRow As Integer = DataInSheet(MRowSheet)
            Dim rowNewMeisai As DataRow
            For mRow As Integer = 0 To meisaiMaxRow - 1
                rowNewMeisai = .dtMeisai.NewRow  '行を生成

                rowNewMeisai("商品コード") = MRowSheet.MRows(mRow)("商品コード").Value
                rowNewMeisai("商品名称カナ") = MRowSheet.MRows(mRow)("商品名称カナ").Value
                rowNewMeisai("商品名称") = MRowSheet.MRows(mRow)("商品名称").Value
                rowNewMeisai("消費税率") = MRowSheet.MRows(mRow)("消費税率").Value
                rowNewMeisai("軽減税率") = MRowSheet.MRows(mRow)("軽減税率").Value
                rowNewMeisai("入数") = MRowSheet.MRows(mRow)("入数").Value
                rowNewMeisai("セット数") = MRowSheet.MRows(mRow)("セット数").Value
                rowNewMeisai("数量") = MRowSheet.MRows(mRow)("数量").Value
                rowNewMeisai("単位") = MRowSheet.MRows(mRow)("単位IN").Text
                rowNewMeisai("税抜原価単価") = MRowSheet.MRows(mRow)("税抜原価単価").Value
                rowNewMeisai("税込原価単価") = MRowSheet.MRows(mRow)("税込原価単価").Value
                rowNewMeisai("税抜商品単価") = MRowSheet.MRows(mRow)("税抜商品単価").Value
                rowNewMeisai("税込商品単価") = MRowSheet.MRows(mRow)("税込商品単価").Value
                rowNewMeisai("税抜金額") = MRowSheet.MRows(mRow)("税抜金額").Value
                rowNewMeisai("税込金額") = MRowSheet.MRows(mRow)("税込金額").Value
                rowNewMeisai("税抜原価") = MRowSheet.MRows(mRow)("税抜原価").Value
                rowNewMeisai("消費税") = MRowSheet.MRows(mRow)("消費税").Value
                rowNewMeisai("備考") = MRowSheet.MRows(mRow)("備考").Value

                rowNewMeisai("テーブルNo") = 0
                rowNewMeisai("納品伝票No") = rowNewDenpyou("テーブルNo") '納品明細の納品伝票Noに納品伝票のテーブルNoをセット
                rowNewMeisai("行番号") = MRowSheet.MRows(mRow)("行番号").Value
                rowNewMeisai("商品マスタNo") = MRowSheet.MRows(mRow)("商品マスタNo").Value
                rowNewMeisai("商品税区分") = MRowSheet.MRows(mRow)("商品税区分").Value
                If My.Settings.HanbaiKanriType = "C" Then
                    'rowNewMeisai("受注明細No") = 0
                    rowNewMeisai("受注明細No") = MRowSheet.MRows(mRow)("受注明細No").Value
                End If

                .dtMeisai.Rows.Add(rowNewMeisai)  '行をDataTableに追加
            Next

            .isSearchedDenpyou = False


            '元伝票の消費税率を取得
            Dim CShouhiZei As New HanbaikanriDialog.CShouhiZei()
            .oldAryRate = CShouhiZei.GetRate2(Denpyou.SeikyuDate)  '消費税率1,2取得

            .MRowSheet.Enabled = True
            .SetForm(True, False, True)
            If DirectCast(.oldAryRate, IStructuralEquatable).Equals(.Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
                .WhenChangeRate()  '消費税率変更時、金額を再計算
            End If
            .CheckNewTanka()  '商品単価、原価単価が最新かどうかチェック
            .UpdateFlagOn()
            .Text = TITLE & "（新規）"
            .lblShusei.Visible = False

            .isEnd = False

            .SheetRedrawON()
        End With

        frmNouhinCopy.StartPosition = FormStartPosition.Manual
        frmNouhinCopy.Location = New Point(Cursor.Position.X, Cursor.Position.Y)  'マウスポインタの位置に表示する
        frmNouhinCopy.Size = New Size(Me.Width, Me.Height)  '現在の画面サイズと同じサイズにする
        frmNouhinCopy.Show()
    End Sub

    '単価履歴の表示
    Private Function GetTankaRireki() As Boolean
        Try
            Dim mRow As Integer = MRowSheet.ActivePosition.MRow
            If edtTokuiCode.Text = "" OrElse mRow < 0 OrElse MRowSheet.MRows(mRow)("商品マスタNo").Value <= 0 Then
                Return False
            End If

            'カレント行の標準単価・仕入単価を取得
            '  商品マスタのレコードを得る
            Dim CShouhin As New HanbaikanriDialog.CShouhin()
            Dim drShouhin As DataRow = CShouhin.GetMaster(MRowSheet.MRows(mRow)("商品マスタNo").Value)
            If drShouhin Is Nothing Then
                MessageBox.Show("該当の商品がないため、単価履歴を表示できません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            '単価履歴の表示
            Using frmTankaList As New HanbaikanriDialog.frmTankaList(HanbaikanriDialog.frmTankaList.enListType.売上, CSingleton.CSetting.Connect, MRowSheet.MRows(mRow)("商品マスタNo").Value, Denpyou.Tokuisaki.MasterNo)
                frmTankaList.ShowDialog()
                If frmTankaList.Selected = False Then
                    Return False  '選択されなかった時
                End If
                SetCursorWait()

                '選択された商品単価をセット
                MRowSheet.MRows(mRow)("商品単価").Value = frmTankaList.SelectedTanka
                '  変更された単価により、金額、合計金額を再計算
                ChangeTanka(mRow, MRowSheet.MRows(mRow)("商品単価").Value)
                UpdateFlagOn()
            End Using

            MRowSheet.Select()
            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(mRow, enSheetCol2.商品単価, enSheetRow.Row2)
            Return True

        Catch ex As Exception
            ErrProc(ex, Me.Text)
            Return False
        End Try
    End Function

    '見積書参照
    Private Sub FindMitumori()
        '納品明細が入力済みなら、登録するかどうかの確認を行う
        If DataInSheet(MRowSheet) > 0 Then
            If UpdateCaution() = False Then
                Exit Sub
            End If
        End If
        Try
            'ほかの検索画面が開いていたら閉じる
            CloseFormListIfOpend(frmMitumoriList)

            '見積書の検索画面表示
            If frmMitumoriList Is Nothing OrElse frmMitumoriList.IsDisposed Then
                '伝票検索画面が表示されていない時
                frmMitumoriList = New frmMitumoriList("見積書参照", AddressOf Me.MitumoriListCallBack)  '伝票検索画面から動かしたいアドレスを渡す
                frmMitumoriList.SearchTokuiCode = edtTokuiCode.Text
                frmMitumoriList.SearchTantouCode = edtTantouCode.Text
                frmMitumoriList.MeisaiOKbtn = False
                If frmMitumoriList.BeforeLoad() Then
                    '一覧画面表示（エラー発生時は一覧画面を表示しない）
                    frmMitumoriList.Opener = Me
                    frmMitumoriList.Show()
                End If
            Else
                '伝票検索画面が表示済の時
                If frmMitumoriList.WindowState = FormWindowState.Minimized Then
                    frmMitumoriList.WindowState = FormWindowState.Normal  '最小化されていた場合、通常に戻す
                End If
                frmMitumoriList.BringToFront()  '最前面に表示する
            End If

        Catch ex As Exception
            ErrProc(ex, Me.Text)
        End Try
    End Sub
    '伝票検索のモードレスウインドウから選択したテーブルNoがコールバックされる
    Private Sub MitumoriListCallBack(ByVal SelectedTableNo As Integer)
        SetCursorWait()
        SheetRedrawOFF()
        Try
            '納品明細が入力済みなら、登録するかどうかの確認を行う
            If DataInSheet(MRowSheet) > 0 Then
                If UpdateCaution() = False Then
                    Exit Sub
                End If
            End If

            If SelectedTableNo > 0 Then
                '検索結果を表示
                SetCursorWait()
                SheetRedrawOFF()
                If GetRecordMitumori(SelectedTableNo) Then
                    MRowSheet.Enabled = True
                    SetForm(True, True, True)
                    '（SetFormで計算し直しているので、ここでは再計算しない）
                    'If oldRate <> Denpyou.Rate Then
                    '    WhenChangeRate()  '消費税率変更時、金額を再計算
                    'End If
                    SetGoukei()  '合計金額を計算
                    UpdateFlagOn()
                    Me.Text = TITLE & "（新規）"
                    lblShusei.Visible = False
                    edtTokuiCode.Select()
                    Me.BringToFront()  '最前面に表示する
                End If
            End If

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '受注伝票参照
    Private Sub FindJutyu()
        '納品明細が入力済みなら、登録するかどうかの確認を行う
        If DataInSheet(MRowSheet) > 0 Then
            If UpdateCaution() = False Then
                Exit Sub
            End If
        End If
        Try
            'ほかの検索画面が開いていたら閉じる
            CloseFormListIfOpend(frmJutyuList)

            '受注伝票の検索画面表示
            If frmJutyuList Is Nothing OrElse frmJutyuList.IsDisposed Then
                '伝票検索画面が表示されていない時
                frmJutyuList = New frmJutyuList("受注伝票参照", AddressOf Me.JutyuListCallBack)  '伝票検索画面から動かしたいアドレスを渡す
                frmJutyuList.SearchTokuiCode = edtTokuiCode.Text
                frmJutyuList.SearchTantouCode = edtTantouCode.Text
                frmJutyuList.MeisaiOKbtn = False
                If frmJutyuList.BeforeLoad() Then
                    '一覧画面表示（エラー発生時は一覧画面を表示しない）
                    frmJutyuList.Opener = Me
                    frmJutyuList.Show()
                End If
            Else
                '伝票検索画面が表示済の時
                If frmJutyuList.WindowState = FormWindowState.Minimized Then
                    frmJutyuList.WindowState = FormWindowState.Normal  '最小化されていた場合、通常に戻す
                End If
                frmJutyuList.BringToFront()  '最前面に表示する
            End If

        Catch ex As Exception
            ErrProc(ex, Me.Text)
        End Try
    End Sub
    '伝票検索のモードレスウインドウから選択したテーブルNoがコールバックされる
    Private Sub JutyuListCallBack(ByVal SelectedTableNo As Integer)
        SetCursorWait()
        SheetRedrawOFF()
        Try
            '納品明細が入力済みなら、登録するかどうかの確認を行う
            If DataInSheet(MRowSheet) > 0 Then
                If UpdateCaution() = False Then
                    Exit Sub
                End If
            End If

            If SelectedTableNo > 0 Then
                '検索結果を表示
                SetCursorWait()
                SheetRedrawOFF()
                If GetRecordJutyu(SelectedTableNo) Then
                    MRowSheet.Enabled = True
                    SetForm(True, False, True)
                    If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
                        WhenChangeRate()  '消費税率変更時、金額を再計算
                    End If
                    SetGoukei()  '合計金額を計算
                    UpdateFlagOn()
                    Me.Text = TITLE & "（新規）"
                    lblShusei.Visible = False
                    If MRowSheet.MaxMRows > 0 Then
                        'カーソル移動
                        MRowSheet.Select()
                        If MRowSheet.MRows(0)("入数").Value = 0 Then
                            MRowSheet.ActiveCellKey = "数量"    'セルの移動
                            MRowSheet.MRows(0)("入数").CanActivate = False
                            MRowSheet.MRows(0)("セット数").CanActivate = False
                        Else
                            MRowSheet.ActiveCellKey = "セット数"    'セルの移動
                            MRowSheet.MRows(0)("入数").CanActivate = False
                        End If
                    End If
                    Me.BringToFront()  '最前面に表示する
                End If
            End If

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '仕入伝票参照（明細のみの参照となる）
    Private Sub FindSiire()
        If Denpyou.Tokuisaki.MasterNo = 0 Then
            MessageBox.Show("先に得意先を入力して下さい。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            edtTokuiCode.Select()
            Exit Sub
        End If

        '納品明細が入力済みなら、登録するかどうかの確認を行う
        If DataInSheet(MRowSheet) > 0 Then
            If UpdateCaution() = False Then
                Exit Sub
            End If
        End If
        Try
            'ほかの検索画面が開いていたら閉じる
            CloseFormListIfOpend(frmSiireList)

            If frmSiireList Is Nothing OrElse frmSiireList.IsDisposed Then
                '伝票検索画面が表示されていない時
                frmSiireList = New frmSiireList("仕入伝票参照", AddressOf Me.SiireListCallBack)  '伝票検索画面から動かしたいアドレスを渡す
                '仕入伝票の検索画面表示
                frmSiireList.MeisaiOKbtn = False
                If frmSiireList.BeforeLoad() Then
                    '一覧画面表示（エラー発生時は一覧画面を表示しない）
                    frmSiireList.Opener = Me
                    frmSiireList.Show()
                End If
            Else
                '伝票検索画面が表示済の時
                If frmSiireList.WindowState = FormWindowState.Minimized Then
                    frmSiireList.WindowState = FormWindowState.Normal  '最小化されていた場合、通常に戻す
                End If
                frmSiireList.BringToFront()  '最前面に表示する
            End If

        Catch ex As Exception
            ErrProc(ex, Me.Text)
        End Try
    End Sub
    '伝票検索のモードレスウインドウから選択したテーブルNoがコールバックされる
    Private Sub SiireListCallBack(ByVal SelectedTableNo As Integer)
        SetCursorWait()
        SheetRedrawOFF()
        Try
            '納品明細が入力済みなら、登録するかどうかの確認を行う
            If DataInSheet(MRowSheet) > 0 Then
                If UpdateCaution() = False Then
                    Exit Sub
                End If
            End If

            If SelectedTableNo > 0 Then
                '検索結果を表示
                SetCursorWait()
                SheetRedrawOFF()
                If GetRecordSiire(SelectedTableNo) Then
                    MRowSheet.Enabled = True
                    SetForm(True, True, True, True)
                    '（SetFormで計算し直しているので、ここでは再計算しない）
                    'If oldRate <> Denpyou.Rate Then
                    '    WhenChangeRate()  '消費税率変更時、金額を再計算
                    'End If
                    CheckNewGenkaTanka()  '原価単価が最新かどうかチェック
                    SetGoukei()  '合計金額を計算
                    UpdateFlagOn()
                    Me.Text = TITLE & "（新規）"
                    lblShusei.Visible = False
                    If drJisha("日付選択") = False Then
                        datSeikyuDate.Select()
                    Else
                        datNouhinDate.Select()
                    End If
                    Me.BringToFront()  '最前面に表示する
                End If
            End If

        Finally
            SheetRedrawON()
            SetCursorDefault()
        End Try
    End Sub

    '明細行の表示を縦に拡大縮小する
    Private originalMeisaiLocationY As Integer  '元の縦位置
    Private originalMeisaiHeight As Integer  '元の高さ
    Private Sub ExpandMeisai()
        If MRowSheet.Location.Y > 40 Then
            MRowSheet.BringToFront()  '最前面にする
            originalMeisaiLocationY = MRowSheet.Location.Y
            originalMeisaiHeight = MRowSheet.Size.Height
            MRowSheet.Location = New Point(MRowSheet.Location.X, 40)
            MRowSheet.Size = New Size(MRowSheet.Size.Width, originalMeisaiHeight + (originalMeisaiLocationY - 40))
        Else
            MRowSheet.SendToBack()  '最背面にする（合計を前面にするため）
            MRowSheet.Location = New Point(MRowSheet.Location.X, originalMeisaiLocationY)
            MRowSheet.Size = New Size(MRowSheet.Size.Width, originalMeisaiHeight)
        End If
    End Sub

    'エクスポート（表示中の内容をエクスポートする）
    Private Sub ExportDenpyou()
        'エクスポート列の定義
        '　（Dictionaryだと列挙順序が不定のためListを使用）
        Dim seikyubi As String, nouhinbi As String
        If drJisha("日付選択") Then
            seikyubi = "請求日"
            nouhinbi = "納品日"
        Else
            seikyubi = "日付"
            nouhinbi = ""
        End If
        Dim fieldList As New List(Of KeyValuePair(Of String, SqlDbType)) From {
            New KeyValuePair(Of String, SqlDbType)("伝票コード", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)(seikyubi, SqlDbType.Date),
            New KeyValuePair(Of String, SqlDbType)(nouhinbi, SqlDbType.Date),
            New KeyValuePair(Of String, SqlDbType)("得意先マスタNo", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("得意先コード", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("得意先名称", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("得意先名称2", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("得意先敬称", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("納入先マスタNo", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("納入先コード", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("納入先名称", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("納入先名称2", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("納入先敬称", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("倉庫マスタNo", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("倉庫コード", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("倉庫名称", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("担当者マスタNo", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("担当者コード", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("担当者氏名", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("売上区分名称", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("税抜額", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("消費税", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("合計金額", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("得意先税区分", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("得意先税区分名", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("消費税計算方法", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("消費税計算方法名", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("端数", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("端数名", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("摘要", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("行番号", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("商品マスタNo", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("商品コード", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("商品名称", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("消費税率", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("軽減税率", SqlDbType.Bit),
            New KeyValuePair(Of String, SqlDbType)("入数", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("セット数", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("数量", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("単位", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("商品単価", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("原価単価", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("金額", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("明細消費税", SqlDbType.Money),
            New KeyValuePair(Of String, SqlDbType)("商品税区分", SqlDbType.Int),
            New KeyValuePair(Of String, SqlDbType)("商品税区分名", SqlDbType.NVarChar),
            New KeyValuePair(Of String, SqlDbType)("備考", SqlDbType.NVarChar)
        }
        'fieldList.Add(New KeyValuePair(Of String, SqlDbType)("帳票名", SqlDbType.NVarChar))

        'KeyとValueをそれぞれのListに変換（Keyでインデックスを取得したいため）
        Dim fieldNameList As New List(Of String)
        Dim dataTypeList As New List(Of SqlDbType)
        For Each pair As KeyValuePair(Of String, SqlDbType) In fieldList
            fieldNameList.Add(pair.Key)
            dataTypeList.Add(pair.Value)
        Next

        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            '*信和*　項目追加
            Dim index As Integer = fieldNameList.IndexOf("摘要") + 1  '[摘要]の次に追加する
            fieldNameList.Insert(index, "但し書き") : dataTypeList.Insert(index, SqlDbType.NVarChar)
            fieldNameList.Insert(index + 1, "仮伝票") : dataTypeList.Insert(index + 1, SqlDbType.Bit)
        End If

        'エクスポート用シート作成
        Dim sheetExport As New GrapeCity.Win.ElTabelle.Sheet
        CDenpyouCommon.MakeExportSheetColumn(sheetExport, fieldNameList, dataTypeList)

        '　データをセット
        Dim meisaiMaxRow As Integer = DataInSheet(MRowSheet)
        For mRow As Integer = 0 To meisaiMaxRow - 1
            sheetExport.MaxRows += 1
            If edtDenpyouCode.Text = "自動更新" Then
                '新規登録で伝票コード自動更新の場合、伝票コードがまだ採番されていない
                sheetExport(fieldNameList.IndexOf("伝票コード"), mRow).Value = ""
            Else
                sheetExport(fieldNameList.IndexOf("伝票コード"), mRow).Value = Denpyou.Code
            End If
            sheetExport(fieldNameList.IndexOf(seikyubi), mRow).Value = Denpyou.SeikyuDate
            If drJisha("日付選択") Then
                sheetExport(fieldNameList.IndexOf("納品日"), mRow).Value = Denpyou.NouhinDate
            End If
            sheetExport(fieldNameList.IndexOf("得意先マスタNo"), mRow).Value = Denpyou.Tokuisaki.MasterNo
            sheetExport(fieldNameList.IndexOf("得意先コード"), mRow).Value = Denpyou.Tokuisaki.Code
            sheetExport(fieldNameList.IndexOf("得意先名称"), mRow).Value = Denpyou.Tokuisaki.Name
            sheetExport(fieldNameList.IndexOf("得意先名称2"), mRow).Value = Denpyou.Tokuisaki.Name2
            sheetExport(fieldNameList.IndexOf("得意先敬称"), mRow).Value = Denpyou.Tokuisaki.Keishou
            sheetExport(fieldNameList.IndexOf("納入先マスタNo"), mRow).Value = Denpyou.NounyuuSaki.MasterNo
            sheetExport(fieldNameList.IndexOf("納入先コード"), mRow).Value = Denpyou.NounyuuSaki.Code
            sheetExport(fieldNameList.IndexOf("納入先名称"), mRow).Value = Denpyou.NounyuuSaki.Name
            sheetExport(fieldNameList.IndexOf("納入先名称2"), mRow).Value = Denpyou.NounyuuSaki.Name2
            sheetExport(fieldNameList.IndexOf("納入先敬称"), mRow).Value = Denpyou.NounyuuSaki.Keishou
            sheetExport(fieldNameList.IndexOf("倉庫マスタNo"), mRow).Value = Denpyou.Souko.MasterNo
            sheetExport(fieldNameList.IndexOf("倉庫コード"), mRow).Value = Denpyou.Souko.Code
            sheetExport(fieldNameList.IndexOf("倉庫名称"), mRow).Value = Denpyou.Souko.Name
            sheetExport(fieldNameList.IndexOf("担当者マスタNo"), mRow).Value = Denpyou.Tantousha.MasterNo
            sheetExport(fieldNameList.IndexOf("担当者コード"), mRow).Value = Denpyou.Tantousha.Code
            sheetExport(fieldNameList.IndexOf("担当者氏名"), mRow).Value = Denpyou.Tantousha.Name
            sheetExport(fieldNameList.IndexOf("売上区分名称"), mRow).Value = cmbUriageKubun.Text
            sheetExport(fieldNameList.IndexOf("税抜額"), mRow).Value = sheetGoukei(enSheetGoukeiCol.税抜額, enSheetGoukeiRow.合計行).Value
            If sheetGoukei.Columns(enSheetGoukeiCol.参考消費税).Hidden = False Then
                sheetExport(fieldNameList.IndexOf("消費税"), mRow).Value = sheetGoukei(enSheetGoukeiCol.参考消費税, enSheetGoukeiRow.合計行).Value
            Else
                sheetExport(fieldNameList.IndexOf("消費税"), mRow).Value = sheetGoukei(enSheetGoukeiCol.消費税額, enSheetGoukeiRow.合計行).Value
            End If
            sheetExport(fieldNameList.IndexOf("合計金額"), mRow).Value = sheetGoukei(enSheetGoukeiCol.合計, enSheetGoukeiRow.合計行).Value
            sheetExport(fieldNameList.IndexOf("得意先税区分"), mRow).Value = Denpyou.Tokuisaki.ZeiKubun
            sheetExport(fieldNameList.IndexOf("得意先税区分名"), mRow).Value = GetZeikubunName(Denpyou.Tokuisaki.ZeiKubun)
            sheetExport(fieldNameList.IndexOf("消費税計算方法"), mRow).Value = Denpyou.Tokuisaki.ShouhizeiKeisan
            sheetExport(fieldNameList.IndexOf("消費税計算方法名"), mRow).Value = GetShouhizeiKeisanName(Denpyou.Tokuisaki.ShouhizeiKeisan)
            sheetExport(fieldNameList.IndexOf("端数"), mRow).Value = Denpyou.Tokuisaki.Hasuu
            sheetExport(fieldNameList.IndexOf("端数名"), mRow).Value = GetHasuuName(Denpyou.Tokuisaki.Hasuu)
            sheetExport(fieldNameList.IndexOf("摘要"), mRow).Value = Denpyou.Tekiyou
            If My.Settings.EndUserName = "信和通信工業株式会社" Then
                '*信和*　項目追加
                sheetExport(fieldNameList.IndexOf("但し書き"), mRow).Value = Denpyou.TadasiGaki
                sheetExport(fieldNameList.IndexOf("仮伝票"), mRow).Value = Denpyou.KariDen
            End If

            sheetExport(fieldNameList.IndexOf("行番号"), mRow).Value = mRow + 1  '（新規行には行番号が振られていないため、カウントしてセット）
            sheetExport(fieldNameList.IndexOf("商品マスタNo"), mRow).Value = MRowSheet.MRows(mRow)("商品マスタNo").Value
            sheetExport(fieldNameList.IndexOf("商品コード"), mRow).Value = MRowSheet.MRows(mRow)("商品コード").Text
            sheetExport(fieldNameList.IndexOf("商品名称"), mRow).Value = MRowSheet.MRows(mRow)("商品名称").Text
            sheetExport(fieldNameList.IndexOf("消費税率"), mRow).Value = MRowSheet.MRows(mRow)("消費税率").Value * 100
            sheetExport(fieldNameList.IndexOf("軽減税率"), mRow).Value = MRowSheet.MRows(mRow)("軽減税率").Value
            sheetExport(fieldNameList.IndexOf("入数"), mRow).Value = MRowSheet.MRows(mRow)("入数").Value
            sheetExport(fieldNameList.IndexOf("セット数"), mRow).Value = MRowSheet.MRows(mRow)("セット数").Value
            sheetExport(fieldNameList.IndexOf("数量"), mRow).Value = MRowSheet.MRows(mRow)("数量").Value
            sheetExport(fieldNameList.IndexOf("単位"), mRow).Value = MRowSheet.MRows(mRow)("単位").Text
            sheetExport(fieldNameList.IndexOf("商品単価"), mRow).Value = MRowSheet.MRows(mRow)("商品単価").Value
            sheetExport(fieldNameList.IndexOf("原価単価"), mRow).Value = MRowSheet.MRows(mRow)("原価単価").Value
            sheetExport(fieldNameList.IndexOf("金額"), mRow).Value = MRowSheet.MRows(mRow)("金額").Value
            sheetExport(fieldNameList.IndexOf("明細消費税"), mRow).Value = MRowSheet.MRows(mRow)("消費税").Value
            sheetExport(fieldNameList.IndexOf("商品税区分"), mRow).Value = MRowSheet.MRows(mRow)("商品税区分").Value
            sheetExport(fieldNameList.IndexOf("商品税区分名"), mRow).Value = GetZeikubunName(MRowSheet.MRows(mRow)("商品税区分").Value)
            sheetExport(fieldNameList.IndexOf("備考"), mRow).Value = MRowSheet.MRows(mRow)("備考").Text
        Next

        'エクスポート
        Dim CSheetExport As New CSheetExport
        CSheetExport.ShowExportDialog(sheetExport, "納品伝票エクスポート", "納品伝票" & Now.ToString("yyyyMMdd"))
    End Sub

    '終了
    Private Sub EndDenpyou()
        If UpdateCaution() = False Then
            Exit Sub
        End If

        isEnd = True  '終了
        Me.Close()
    End Sub

    '納品伝票、納品明細の取得と更新用SQLコマンド（Update/Insert/Delete）の作成
    Private Sub MakeEditCommandsDenpyou(Optional ByVal tableNo As Integer = 0)
        Using cnDenpyou As New SqlConnection(CSingleton.CSetting.Connect)
            MakeEditCommandsDenpyou(cnDenpyou, tableNo)
        End Using
    End Sub
    Private Sub MakeEditCommandsDenpyou(ByRef cnDenpyou As SqlConnection, Optional ByVal tableNo As Integer = 0)
        Dim sSQL, sSQLValues As String

        daDenpyou = New SqlDataAdapter()
        dsDenpyou = New DataSet

        '納品伝票
        '  Select Command
        sSQL = "SELECT * FROM 納品伝票 WHERE テーブルNo = @テーブルNo And 削除 = 0"
        Dim cmdDenpyouSelect As New SqlCommand(sSQL, cnDenpyou)
        cmdDenpyouSelect.CommandTimeout = DBCommandTimeout
        cmdDenpyouSelect.Parameters.Add("@テーブルNo", SqlDbType.Int)
        cmdDenpyouSelect.Parameters("@テーブルNo").Value = tableNo
        daDenpyou.SelectCommand = cmdDenpyouSelect

        '  スキーマ情報取得
        Dim dtSchemaDenpyou As New DataTable
        daDenpyou.FillSchema(dtSchemaDenpyou, SchemaType.Source)

        '  InsertのSQL定義作成(InsertとUpdateで使用)
        sSQL = "INSERT 納品伝票 (修正済, 修正済締日, 削除, 登録日時, 更新日時, 更新ユーザー, 更新コンピュータ名"
        sSQLValues = ") VALUES (1, 1, 0, GetDate(), GetDate(), '" & HanbaikanriDialog.CSingleton.CCommonPara.LoginUserName & "', '" & My.Computer.Name & "'"
        Dim jogaiListDenpyou As New List(Of String) From {"テーブルNo", "修正済", "修正済締日", "削除", "登録日時", "更新日時", "更新ユーザー", "更新コンピュータ名"}  'SQL文作成時の除外項目
        For i As Integer = 0 To dtSchemaDenpyou.Columns.Count - 1
            If jogaiListDenpyou.Contains(dtSchemaDenpyou.Columns(i).ColumnName) Then Continue For  'SQL文作成から除外する項目
            If i > 0 AndAlso sSQL.Substring(sSQL.Length - 1) <> " " Then
                sSQL &= ", "
                sSQLValues &= ", "
            End If
            sSQL &= dtSchemaDenpyou.Columns(i).ColumnName
            sSQLValues &= "@" & dtSchemaDenpyou.Columns(i).ColumnName
        Next
        Dim sInsertSQLDenpyou As String = sSQL & sSQLValues & ")"

        '  Insert Command
        sSQL = sInsertSQLDenpyou & vbCrLf _
             & "SELECT SCOPE_IDENTITY() AS テーブルNo"
        Dim cmdDenpyouInsert As New SqlCommand(sSQL, cnDenpyou)
        cmdDenpyouInsert.CommandTimeout = DBCommandTimeout
        For i As Integer = 0 To dtSchemaDenpyou.Columns.Count - 1
            'パラメータの設定
            cmdDenpyouInsert.Parameters.Add("@" & dtSchemaDenpyou.Columns(i).ColumnName,
                SQLTypeConv(dtSchemaDenpyou.Columns(i).DataType), dtSchemaDenpyou.Columns(i).MaxLength,
                dtSchemaDenpyou.Columns(i).ColumnName)
        Next
        daDenpyou.InsertCommand = cmdDenpyouInsert

        '  Update Command
        sSQL = "UPDATE 納品伝票 SET 削除 = 1, 修正済 = 1, 修正済締日 = 1, 更新日時 = GetDate(), 更新ユーザー = '" & HanbaikanriDialog.CSingleton.CCommonPara.LoginUserName & "', 更新コンピュータ名 = '" & My.Computer.Name & "' " _
             & "WHERE テーブルNo = @テーブルNo" & vbCrLf _
             & sInsertSQLDenpyou & vbCrLf _
             & "SELECT SCOPE_IDENTITY() AS テーブルNo"
        Dim cmdDenpyouUpdate As New SqlCommand(sSQL, cnDenpyou)
        cmdDenpyouUpdate.CommandTimeout = DBCommandTimeout
        For i As Integer = 0 To dtSchemaDenpyou.Columns.Count - 1
            'パラメータの設定
            cmdDenpyouUpdate.Parameters.Add("@" & dtSchemaDenpyou.Columns(i).ColumnName,
                SQLTypeConv(dtSchemaDenpyou.Columns(i).DataType), dtSchemaDenpyou.Columns(i).MaxLength,
                dtSchemaDenpyou.Columns(i).ColumnName)
        Next
        daDenpyou.UpdateCommand = cmdDenpyouUpdate

        '  Delete Command
        sSQL = "UPDATE 納品伝票 SET 削除 = 1, 修正済 = 1, 修正済締日 = 1, 更新日時 = GetDate(), 更新ユーザー = '" & HanbaikanriDialog.CSingleton.CCommonPara.LoginUserName & "', 更新コンピュータ名 = '" & My.Computer.Name & "' " _
             & "WHERE テーブルNo = @テーブルNo"
        Dim cmdDenpyouDelete As New SqlCommand(sSQL, cnDenpyou)
        cmdDenpyouDelete.CommandTimeout = DBCommandTimeout
        cmdDenpyouDelete.Parameters.Add("@テーブルNo", SqlDbType.Int, 0, "テーブルNo")
        daDenpyou.DeleteCommand = cmdDenpyouDelete

        '  DataSet/DataTable取得
        daDenpyou.Fill(dsDenpyou, "納品伝票")
        dtDenpyou = dsDenpyou.Tables("納品伝票")

        '  AutoNumber列の設定
        With dtDenpyou.Columns("テーブルNo")
            .AutoIncrement = True
            .AutoIncrementSeed = -1
            .AutoIncrementStep = -1
            .ReadOnly = True
        End With


        '納品明細
        daMeisai = New SqlDataAdapter(sSQL, cnDenpyou)

        '  Select Command
        sSQL = "SELECT * FROM 納品明細 WHERE 納品伝票No = @納品伝票No AND 削除 = 0 ORDER BY 行番号"
        Dim cmdMeisaiSelect As New SqlCommand(sSQL, cnDenpyou)
        cmdMeisaiSelect.CommandTimeout = DBCommandTimeout
        cmdMeisaiSelect.Parameters.Add("@納品伝票No", SqlDbType.Int)
        If dtDenpyou.Rows.Count = 0 Then  '新規入力の時
            cmdMeisaiSelect.Parameters("@納品伝票No").Value = 0
        Else
            cmdMeisaiSelect.Parameters("@納品伝票No").Value = dtDenpyou.Rows(0)("テーブルNo")
        End If
        daMeisai.SelectCommand = cmdMeisaiSelect

        '  スキーマ情報取得
        Dim dtSchemaMeisai As New DataTable
        daMeisai.FillSchema(dtSchemaMeisai, SchemaType.Source)

        '  InsertのSQL定義(InsertとUpdateで使用)
        sSQL = "INSERT 納品明細 (修正済, 削除, 登録日時, 更新日時, 更新ユーザー, 更新コンピュータ名"
        sSQLValues = ") VALUES (1, 0, GetDate(), GetDate(), '" & HanbaikanriDialog.CSingleton.CCommonPara.LoginUserName & "', '" & My.Computer.Name & "'"
        Dim jogaiListMeisai As New List(Of String) From {"テーブルNo", "修正済", "削除", "登録日時", "更新日時", "更新ユーザー", "更新コンピュータ名"}  'SQL文作成時の除外項目
        For i As Integer = 0 To dtSchemaMeisai.Columns.Count - 1
            If jogaiListMeisai.Contains(dtSchemaMeisai.Columns(i).ColumnName) Then Continue For  'SQL文作成から除外する項目
            If i > 0 AndAlso sSQL.Substring(sSQL.Length - 1) <> " " Then
                sSQL &= ", "
                sSQLValues &= ", "
            End If
            sSQL &= dtSchemaMeisai.Columns(i).ColumnName
            sSQLValues &= "@" & dtSchemaMeisai.Columns(i).ColumnName
        Next
        Dim sInsertSQLMeisai As String = sSQL & sSQLValues & ")"

        '  Insert Command
        sSQL = sInsertSQLMeisai & vbCrLf _
            & "SELECT SCOPE_IDENTITY() AS テーブルNo"
        Dim cmdMeisaiInsert As New SqlCommand(sSQL, cnDenpyou)
        cmdMeisaiInsert.CommandTimeout = DBCommandTimeout
        For i As Integer = 0 To dtSchemaMeisai.Columns.Count - 1
            'パラメータの設定
            cmdMeisaiInsert.Parameters.Add("@" & dtSchemaMeisai.Columns(i).ColumnName,
                SQLTypeConv(dtSchemaMeisai.Columns(i).DataType), dtSchemaMeisai.Columns(i).MaxLength,
                dtSchemaMeisai.Columns(i).ColumnName)
        Next
        daMeisai.InsertCommand = cmdMeisaiInsert

        '  Update Command
        sSQL = "UPDATE 納品明細 SET 削除 = 1, 修正済 = 1, 更新日時 = GetDate(), 更新ユーザー = '" & HanbaikanriDialog.CSingleton.CCommonPara.LoginUserName & "', 更新コンピュータ名 = '" & My.Computer.Name & "' " _
             & "WHERE テーブルNo = @テーブルNo" & vbCrLf _
             & sInsertSQLMeisai & vbCrLf _
             & "SELECT SCOPE_IDENTITY() AS テーブルNo"
        Dim cmdMeisaiUpdate As New SqlCommand(sSQL, cnDenpyou)
        cmdMeisaiUpdate.CommandTimeout = DBCommandTimeout
        For i As Integer = 0 To dtSchemaMeisai.Columns.Count - 1
            'パラメータの設定
            cmdMeisaiUpdate.Parameters.Add("@" & dtSchemaMeisai.Columns(i).ColumnName,
                SQLTypeConv(dtSchemaMeisai.Columns(i).DataType), dtSchemaMeisai.Columns(i).MaxLength,
                dtSchemaMeisai.Columns(i).ColumnName)
        Next
        daMeisai.UpdateCommand = cmdMeisaiUpdate

        '  Delete Command
        sSQL = "UPDATE 納品明細 SET 削除 = 1, 修正済 = 1, 更新日時 = GetDate(), 更新ユーザー = '" & HanbaikanriDialog.CSingleton.CCommonPara.LoginUserName & "', 更新コンピュータ名 = '" & My.Computer.Name & "' " _
             & "WHERE テーブルNo = @テーブルNo"
        Dim cmdMeisaiDelete As New SqlCommand(sSQL, cnDenpyou)
        cmdMeisaiDelete.CommandTimeout = DBCommandTimeout
        cmdMeisaiDelete.Parameters.Add("@テーブルNo", SqlDbType.Int, 0, "テーブルNo")
        daMeisai.DeleteCommand = cmdMeisaiDelete

        '  DataSet/DataTable取得
        daMeisai.Fill(dsDenpyou, "納品明細")
        dtMeisai = dsDenpyou.Tables("納品明細")
        iMeisaiCnt = dtMeisai.Rows.Count  '明細件数をHold（ほかで変更されたかのチェックに使用）

        '  AutoNumber列の設定
        With dtMeisai.Columns("テーブルNo")
            .AutoIncrement = True
            .AutoIncrementSeed = -1
            .AutoIncrementStep = -1
            .ReadOnly = True
        End With

        'リレーションの設定（これが無いと、納品伝票修正でテーブルNoが変わった時、納品明細の納品伝票Noが自動で変わらない）
        dsDenpyou.Relations.Add("納品伝票", dsDenpyou.Tables("納品伝票").Columns("テーブルNo"), dsDenpyou.Tables("納品明細").Columns("納品伝票No"))
    End Sub

    '納品伝票(Denpyou)の初期設定（「次伝票」でクリアしない分の初期設定）
    Private Sub InitDenpyou()
        With Denpyou
            .NouhinDenpyou = drJisha("納品伝票ﾌｫｰﾑ")
            .KoumokuSu = drJisha("納品伝票項目数")

            InitMeisai()  '納品明細の初期設定

            With .Tokuisaki
                .MasterNo = 0
                .Code = ""
                .Name = ""
                .Name2 = ""
                .NameKana = ""
                .Hyoujun = True
                .SeikyuSaki = 0
                .Hasuu = enHasuu.切り捨て
                .Kakeritu = 1
                .ZeiKubun = enZeikubun.外税
                .ShouhizeiKeisan = enZeiKeisan.請求時
                .DenpyouCodeUpdate = drJisha("納品伝票コード自動更新")
                .DenpyouCodeFlag = False
                .Simebi = 0
                .Bikou = ""
                .Shokuchi = False
                .YosinGendo = 0
                .NounyusakiExist = False
            End With

            With .NounyuuSaki
                .MasterNo = 0
                .Code = ""
                .Name = ""
                .Name2 = ""
                .Keishou = ""
            End With

            With .Souko
                If defaultSoukoNo = 0 Then
                    .MasterNo = 0
                    .Code = ""
                    .Name = ""
                Else
                    'デフォルト倉庫がある場合、デフォルトの倉庫を得る
                    .MasterNo = defaultSoukoNo
                    Dim CSouko As New HanbaikanriDialog.CSouko()
                    Dim drSouko As DataRow = CSouko.GetMaster(.MasterNo)
                    If drSouko IsNot Nothing Then
                        .Code = drSouko("コード").ToString
                        .Name = drSouko("名称").ToString
                    Else
                        .MasterNo = 0
                        .Code = ""
                        .Name = ""
                    End If
                End If
            End With

            With .Tantousha
                .MasterNo = 0
                .Code = ""
                .Name = ""
                .NameKana = ""
            End With
        End With
    End Sub

    '納品伝票(Denpyou)の初期設定
    Private Sub InitMeisai()
        isChanged = False
        isSearchedDenpyou = False

        MRowSheet.DataSource = Nothing
        MRowSheet.MaxMRows = 0
        MRowSheet.DataSource = dtMeisai

        Using cnTable As New SqlConnection(CSingleton.CSetting.Connect)
            cnTable.Open()

            With Denpyou
                'デフォルトの納品伝票フォーム/プリンタをメニューのコンボボックスにセットする
                CFormCommon.FindComboBoxFormValue(mnuCmbForm, PrintFormFolderMirrorPrincipal(.NouhinDenpyou))
                mnuCmbPrinter.ComboBox.SelectedIndex = mnuCmbPrinter.FindStringExact(CSingleton.CSetting.PrinterName(CSetting.SelPrinter.納品伝票))

                If My.Settings.EndUserName = "信和通信工業株式会社" Then
                    '*信和*　デフォルトの領収書フォーム/プリンタをメニューのコンボボックスにセットする
                    CFormCommon.FindComboBoxFormValue(mnuCmbFormRyoushuSho, PrintFormFolderMirrorPrincipal(RyoushuShoFolder & RyoushuShoForm))
                    mnuCmbPrinterRyoushuSho.ComboBox.SelectedIndex = mnuCmbPrinterRyoushuSho.FindStringExact(CSingleton.CSetting.PrinterName(CSetting.SelPrinter.宛名))
                End If

                .TableNo = 0
                .Code = .NewCode.ToString(New String("0", drJisha("納品伝票コード桁数")))

                ErrorProvider1.SetError(datSeikyuDate, "")
                ErrorProvider1.SetError(datNouhinDate, "")
                .SeikyuDate = Date.Today
                .NouhinDate = Date.Today
                If GetByte(drJisha("伝票デフォルト日付")) <> 0 Then
                    '前回入力日付をセット（最後に入力した伝票を取得）
                    Dim sSQL As String = "SELECT TOP 1 処理日, 納品日 FROM 納品伝票 WHERE 削除 = 0 ORDER BY テーブルNo DESC"
                    Using drDenpyou As SqlDataReader = CDBCommon.SQLExecuteReader(cnTable, sSQL)
                        If drDenpyou IsNot Nothing Then
                            If drDenpyou.Read Then
                                .SeikyuDate = drDenpyou("処理日")
                                .NouhinDate = drDenpyou("納品日")
                            End If
                        End If
                    End Using
                End If

                '  売上区分の増減を売上区分マスタから得る（更新時に使用）
                .UriageKubun.MasterNo = defaultUriageKubun
                SetUriageKubunInfo(cnTable, defaultUriageKubun)

                .Tekiyou = ""
                .TadasiGaki = ""  '信和通信のみ使用

                .NyukinNo = 0
                .MitumoriNo = 0
                .MitumoriCode = ""
                .JutyuDenpyouNo = 0
                .JutyuCode = ""
                .SiireDenpyouNo = 0
                .SiireCode = ""
                lblReferenceCode.Text = ""

                .RendouSakiSiireCode = ""
                .RendouSakiNyukinCode = ""

                .KariDen = False  '*信和*　項目追加

                If My.Settings.EndUserName = "山田商店" Then
                    '*山田商店*　「次伝票」で納入先もクリアする
                    With .NounyuuSaki
                        .MasterNo = 0
                        .Code = ""
                        .Name = ""
                        .Name2 = ""
                        If Denpyou.Tokuisaki.Shokuchi Then  '諸口の時
                            Denpyou.NounyuuSaki.Keishou = "御中"
                        Else
                            Denpyou.NounyuuSaki.Keishou = ""
                        End If
                    End With
                End If

                If .KoumokuSu = 0 Then
                    .KoumokuSu = MaxKoumoku
                End If

                Dim CShouhiZei As New HanbaikanriDialog.CShouhiZei()
                .aryRate = CShouhiZei.GetRate2(cnTable, Denpyou.SeikyuDate)  '消費税率1,2取得
                oldAryRate = Denpyou.aryRate
            End With
        End Using

        '合計シートのクリア
        sheetGoukei.MaxRows = enSheetGoukeiRow.合計行 + 1
        sheetGoukei(enSheetGoukeiCol.税抜額, enSheetGoukeiRow.合計行).Value = 0
        sheetGoukei(enSheetGoukeiCol.消費税額, enSheetGoukeiRow.合計行).Value = 0
        sheetGoukei(enSheetGoukeiCol.合計, enSheetGoukeiRow.合計行).Value = 0
        sheetGoukei(enSheetGoukeiCol.参考消費税, enSheetGoukeiRow.合計行).Value = 0
        For iCol As Integer = 0 To sheetGoukei.MaxColumns - 1
            sheetGoukei(iCol, enSheetGoukeiRow.合計行).ErrorText = ""
        Next
    End Sub

    '納品伝票・納品明細からデータを得る（Denpyouに内容をセット）
    '  引数：  isCopy:複写入力時True
    '  戻値：  1=正常, 0=指定伝票 , -1=データなし
    Private Function GetRecord(ByVal tableNo As Integer, ByVal isCopy As Boolean) As Integer
        Using cnTable As New SqlConnection(CSingleton.CSetting.Connect)
            cnTable.Open()

            '対象データが変更され別テーブルNoに変わっている可能性があるため、変更後のテーブルNoを取得する（削除されていたら取得されない。変更していなければ、そのままのテーブルNoが取得される。）
            Dim newDenpyouNo As Integer = CDenpyouCommon.GetNewDenpyouNo("納品伝票", cnTable, tableNo)
            If newDenpyouNo > 0 Then
                tableNo = newDenpyouNo  '変更後のテーブルNo
            End If

            '納品伝票、納品明細の取得と更新用SQLコマンドの作成
            MakeEditCommandsDenpyou(cnTable, tableNo)
            If dtDenpyou.Rows.Count <= 0 Then
                MessageBox.Show("検索条件に該当する納品伝票は見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                isChanged = False
                AllNew()  '新規入力状態にする（MakeEditCommandsDenpyouでデータ無しでDataTableを取得してしまっているため、元画面のDataTableを使用できない）
                Return -1
            End If
            Dim drDenpyou As DataRow = dtDenpyou.Rows(0)

            '得意先マスタNoをキーに、得意先マスタのレコードを得る
            Dim CTokuisaki As New HanbaikanriDialog.CTokuisaki()
            Dim drTokui As DataRow = CTokuisaki.GetMaster(drDenpyou("得意先マスタNo"), cnTable)
            If drTokui Is Nothing Then
                MessageBox.Show("検索条件に該当する納品伝票の得意先が見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                isChanged = False
                AllNew()  '新規入力状態にする（MakeEditCommandsDenpyouでデータ無しでDataTableを取得してしまっているため、元画面のDataTableを使用できない）
                Return -1
            End If

            InitDenpyou()

            '納品伝票の内容をセット
            Dim masterZeikubun As Byte  '得意先マスタの設定をHold
            Dim masterShouhizeiKeisan As Short  '得意先マスタの設定をHold
            Dim masterHasuu As Short  '得意先マスタの設定をHold

            With Denpyou
                If isCopy = False Then
                    .TableNo = drDenpyou("テーブルNo")
                    .Code = drDenpyou("コード").ToString
                    .SeikyuDate = drDenpyou("処理日")
                    .NouhinDate = drDenpyou("納品日")

                    If My.Settings.EndUserName = "信和通信工業株式会社" Then
                        '*信和*　項目追加。但し、複写入力時は複写しない。
                        .KariDen = GetBoolean(drDenpyou("仮伝票"))
                    End If
                End If

                If My.Settings.EndUserName = "有限会社長万部" AndAlso isCopy Then
                    '*長万部*　複写入力時、元伝票の日付を＋１アップさせて表示する
                    .SeikyuDate = CDate(drDenpyou("処理日")).AddDays(1)
                    .NouhinDate = CDate(drDenpyou("納品日")).AddDays(1)
                End If

                With .Tokuisaki
                    .MasterNo = drDenpyou("得意先マスタNo")
                    .Code = drTokui("コード").ToString
                    .Name = drTokui("名称").ToString
                    .Name2 = drTokui("名称2").ToString
                    .NameKana = drTokui("名称カナ").ToString
                    .Keishou = drTokui("敬称").ToString
                    .Bikou = drTokui("備考").ToString
                    .Shokuchi = GetBoolean(drTokui("諸口フラグ"))
                    .NounyusakiExist = CTokuisaki.NounyusakiExist(drDenpyou("得意先マスタNo"))

                    If drTokui("請求先フラグ") Then
                        '請求先マスタNoをキーに、得意先マスタのレコードを得る
                        Dim CSeikyu As New HanbaikanriDialog.CTokuisaki()
                        Dim drSeikyu As DataRow = CSeikyu.GetMaster(drTokui("請求先マスタNo"), cnTable)
                        If drSeikyu Is Nothing Then
                            MessageBox.Show("検索条件に該当する納品伝票の請求先が見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return -1
                        End If

                        .SeikyuSaki = drTokui("請求先マスタNo")
                        .Kakeritu = GetDecimal(drSeikyu("掛率"))
                        .Hyoujun = GetBoolean(drSeikyu("標準ﾌｫｰﾑ"))
                        .DenpyouCodeFlag = GetBoolean(drSeikyu("伝票コードフラグ"))
                        .Simebi = GetShort(drSeikyu("締日"))
                        .YosinGendo = GetDecimal(drSeikyu("与信限度額"))

                        masterZeikubun = GetByte(drSeikyu("税区分"))  '得意先マスタの設定をHold
                        masterShouhizeiKeisan = GetShort(drSeikyu("消費税計算方法"))  '得意先マスタの設定をHold
                        masterHasuu = GetShort(drSeikyu("端数"))  '得意先マスタの設定をHold

                        If GetBoolean(drSeikyu("標準ﾌｫｰﾑ")) = False Then
                            If Denpyou.Tokuisaki.DenpyouCodeFlag Then
                                Denpyou.Tokuisaki.DenpyouCodeUpdate = GetBoolean(drSeikyu("納品伝票コード自動更新"))
                            End If
                            Denpyou.NouhinDenpyou = drSeikyu("納品伝票フォーム").ToString
                            Denpyou.KoumokuSu = GetInt(drSeikyu("納品伝票項目数"))
                        End If
                        If Denpyou.KoumokuSu = 0 Then
                            Denpyou.KoumokuSu = MaxKoumoku
                        End If
                        Denpyou.NewCode = GetDenpyouCode(Denpyou.Tokuisaki.SeikyuSaki)
                    Else
                        .SeikyuSaki = drDenpyou("得意先マスタNo")
                        .Kakeritu = GetDecimal(drTokui("掛率"))
                        .Hyoujun = GetBoolean(drTokui("標準ﾌｫｰﾑ"))
                        .DenpyouCodeFlag = GetBoolean(drTokui("伝票コードフラグ"))
                        .Simebi = GetShort(drTokui("締日"))
                        .YosinGendo = GetDecimal(drTokui("与信限度額"))

                        masterZeikubun = GetByte(drTokui("税区分"))  '得意先マスタの設定をHold
                        masterShouhizeiKeisan = GetShort(drTokui("消費税計算方法"))  '得意先マスタの設定をHold
                        masterHasuu = GetShort(drTokui("端数"))  '得意先マスタの設定をHold

                        If .Hyoujun = False Then
                            If Denpyou.Tokuisaki.DenpyouCodeFlag Then
                                Denpyou.Tokuisaki.DenpyouCodeUpdate = GetBoolean(drTokui("納品伝票コード自動更新"))
                            End If
                            Denpyou.NouhinDenpyou = drTokui("納品伝票フォーム").ToString
                            Denpyou.KoumokuSu = GetInt(drTokui("納品伝票項目数"))
                        End If
                        If Denpyou.KoumokuSu = 0 Then
                            Denpyou.KoumokuSu = MaxKoumoku
                        End If
                        Denpyou.NewCode = GetDenpyouCode(Denpyou.Tokuisaki.SeikyuSaki)
                    End If

                    .ZeiKubun = GetByte(drDenpyou("得意先税区分"))
                    .ShouhizeiKeisan = GetShort(drDenpyou("消費税計算方法"))
                    .Hasuu = GetShort(drDenpyou("端数"))
                End With

                '指定伝票かどうかを判定
                If CNouhin.CNouhinInstance.ChkSiteiDenpyou(Denpyou.NouhinDenpyou) Then
                    '指定伝票なら、納品伝票を終了し指定伝票画面へ
                    isEnd = True  '終了
                    CNouhin.CNouhinInstance.TokuisakiNo = 0
                    CNouhin.CNouhinInstance.TableNo = tableNo
                    CNouhin.CNouhinInstance.Reference = isCopy
                    CNouhin.CNouhinInstance.JutyuNo = 0
                    CNouhin.CNouhinInstance.MitumoriNo = 0
                    Me.DialogResult = DialogResult.OK  '結果を渡す
                    Me.Close()
                    Return 0  '指定伝票
                End If

                If isCopy Then  '複写入力の時
                    .Code = .NewCode.ToString(New String("0", drJisha("納品伝票コード桁数")))
                End If

                '納入先
                With .NounyuuSaki
                    .MasterNo = GetInt(drDenpyou("納入先マスタNo"))
                    .Code = drDenpyou("納入先コード").ToString
                    .Name = drDenpyou("納入先名称").ToString
                    .Name2 = drDenpyou("納入先名称2").ToString
                    .Keishou = drDenpyou("納入先敬称").ToString
                End With

                '倉庫マスタNoをキーに、倉庫マスタのレコードを得る
                With .Souko
                    .MasterNo = GetInt(drDenpyou("倉庫マスタNo"))
                    If .MasterNo > 0 Then
                        Dim CSouko As New HanbaikanriDialog.CSouko()
                        Dim drSouko As DataRow = CSouko.GetMaster(.MasterNo, cnTable)
                        If drSouko IsNot Nothing Then
                            .Code = drSouko("コード").ToString
                            .Name = drSouko("名称").ToString
                        End If
                    End If
                End With

                '担当者マスタNoをキーに、担当者マスタのレコードを得る
                With .Tantousha
                    .MasterNo = GetInt(drDenpyou("担当者マスタNo"))
                    If .MasterNo > 0 Then
                        Dim CTantousha As New HanbaikanriDialog.CTantousha()
                        Dim drTantou As DataRow = CTantousha.GetMaster(.MasterNo, cnTable)
                        If drTantou IsNot Nothing Then
                            .Code = drTantou("コード").ToString
                            .Name = drTantou("氏名").ToString
                            .NameKana = drTantou("氏名カナ").ToString
                        End If
                    End If
                End With

                '  売上区分の増減を売上区分マスタから得る（更新時に使用）
                .UriageKubun.MasterNo = drDenpyou("売上区分マスタNo")
                SetUriageKubunInfo(cnTable, drDenpyou("売上区分マスタNo"))

                If Denpyou.UriageKubun.Code = CON_UriageCode Then  '売上の時
                    sheetGoukei(enSheetGoukeiCol.税抜額, enSheetGoukeiRow.合計行).Value = drDenpyou("税抜額")
                    sheetGoukei(enSheetGoukeiCol.合計, enSheetGoukeiRow.合計行).Value = drDenpyou("合計金額")
                    If Denpyou.Tokuisaki.ZeiKubun <> enZeikubun.非課税 AndAlso Denpyou.Tokuisaki.ShouhizeiKeisan = enZeiKeisan.請求時 Then
                        '請求時は、消費税は参考消費税へ
                        sheetGoukei(enSheetGoukeiCol.消費税額, enSheetGoukeiRow.合計行).Value = 0
                        sheetGoukei(enSheetGoukeiCol.参考消費税, enSheetGoukeiRow.合計行).Value = drDenpyou("消費税")
                    Else
                        sheetGoukei(enSheetGoukeiCol.消費税額, enSheetGoukeiRow.合計行).Value = drDenpyou("消費税")
                        sheetGoukei(enSheetGoukeiCol.参考消費税, enSheetGoukeiRow.合計行).Value = 0
                    End If
                Else
                    sheetGoukei(enSheetGoukeiCol.税抜額, enSheetGoukeiRow.合計行).Value = Math.Abs(drDenpyou("税抜額"))
                    sheetGoukei(enSheetGoukeiCol.合計, enSheetGoukeiRow.合計行).Value = Math.Abs(drDenpyou("合計金額"))
                    If Denpyou.Tokuisaki.ZeiKubun <> enZeikubun.非課税 AndAlso Denpyou.Tokuisaki.ShouhizeiKeisan = enZeiKeisan.請求時 Then
                        '請求時は、消費税は参考消費税へ
                        sheetGoukei(enSheetGoukeiCol.消費税額, enSheetGoukeiRow.合計行).Value = 0
                        sheetGoukei(enSheetGoukeiCol.参考消費税, enSheetGoukeiRow.合計行).Value = Math.Abs(drDenpyou("消費税"))
                    Else
                        sheetGoukei(enSheetGoukeiCol.消費税額, enSheetGoukeiRow.合計行).Value = Math.Abs(drDenpyou("消費税"))
                        sheetGoukei(enSheetGoukeiCol.参考消費税, enSheetGoukeiRow.合計行).Value = 0
                    End If
                End If

                .Tekiyou = drDenpyou("摘要").ToString
                If My.Settings.EndUserName = "信和通信工業株式会社" Then
                    '*信和*　項目追加
                    .TadasiGaki = drDenpyou("但し書き").ToString
                End If

                If isCopy = False Then
                    .NyukinNo = GetInt(drDenpyou("入金伝票テーブルNo"))
                End If

                .MitumoriNo = GetInt(drDenpyou("見積書No"))
                '見積書より、見積書コードを得る
                If .MitumoriNo > 0 Then
                    .MitumoriCode = GetReferenceCode("見積書", .MitumoriNo)
                End If

                If My.Settings.HanbaiKanriType = "C" Then
                    .JutyuDenpyouNo = GetInt(drDenpyou("受注伝票No"))
                    '受注伝票より、受注伝票コードを得る
                    If .JutyuDenpyouNo > 0 Then
                        .JutyuCode = GetReferenceCode("受注伝票", .JutyuDenpyouNo)
                    End If
                End If

                .SiireDenpyouNo = GetInt(drDenpyou("仕入伝票No"))
                '仕入伝票より、仕入伝票コードを得る
                If .SiireDenpyouNo > 0 Then
                    .SiireCode = GetReferenceCode("仕入伝票", .SiireDenpyouNo)
                End If

                '連動先を取得
                If isCopy = False Then
                    Dim sSQL As String
                    '仕入伝票へ連動した時、仕入伝票コードを取得
                    sSQL = "SELECT コード FROM 仕入伝票 WHERE 納品伝票No = " & tableNo & " AND 削除 = 0 ORDER BY コード"
                    Dim dtSiire As DataTable = CDBCommon.GetDataTable(cnTable, sSQL)
                    For iRow As Integer = 0 To dtSiire.Rows.Count - 1
                        If .RendouSakiSiireCode <> "" Then .RendouSakiSiireCode &= "/"
                        .RendouSakiSiireCode &= dtSiire.Rows(iRow)("コード").ToString
                    Next

                    '入金伝票へ連動した時、入金伝票コードを取得
                    If .NyukinNo > 0 Then
                        sSQL = "SELECT コード FROM 入金伝票 WHERE テーブルNo = " & .NyukinNo & " AND 削除 = 0"
                        Dim dtNyukin As DataTable = CDBCommon.GetDataTable(cnTable, sSQL)
                        If dtNyukin.Rows.Count > 0 Then
                            .RendouSakiNyukinCode &= dtNyukin.Rows(0)("コード").ToString
                        End If
                    End If
                End If
            End With

            '元伝票の消費税率をHold
            Dim CShouhiZei As New HanbaikanriDialog.CShouhiZei()
            oldAryRate = CShouhiZei.GetRate2(cnTable, drDenpyou("処理日"))  '消費税率1,2取得
            If isCopy = False Then
                Denpyou.aryRate = oldAryRate.Clone
            End If

            '納品伝票明細の内容をセット
            If Denpyou.KoumokuSu < dtMeisai.Rows.Count Then
                Denpyou.KoumokuSu = dtMeisai.Rows.Count
            End If

            If isCopy Then  '複写入力の時
                '複写元の税区分or消費税計算方法or端数が、得意先マスタの現在の設定と違う時、注意メッセージを表示
                Dim message As String = ""
                If masterZeikubun <> Denpyou.Tokuisaki.ZeiKubun Then
                    message = "税区分"
                End If
                If (masterZeikubun <> enZeikubun.非課税 AndAlso Denpyou.Tokuisaki.ZeiKubun <> enZeikubun.非課税) _
                  AndAlso masterShouhizeiKeisan <> Denpyou.Tokuisaki.ShouhizeiKeisan Then
                    If message <> "" Then message &= "・"
                    message &= "消費税計算方法"
                End If
                If masterHasuu <> Denpyou.Tokuisaki.Hasuu Then
                    If message <> "" Then message &= "・"
                    message &= "端数"
                End If
                If message <> "" Then
                    MessageBox.Show("複写元の" & message & "が、現在の得意先マスタの設定と違います。" & vbCrLf & "内容を確認してください。" _
                        & vbCrLf & "（得意先を入力し直すと、最新の設定になります。）", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

                '複写入力の時、納品伝票、納品明細よりDataSetを新規で追加する
                Dim dtMeisaiCopy As DataTable = dtMeisai.Copy  '検索した納品明細をHold

                '  新規DataSetを作成
                MakeEditCommandsDenpyou(cnTable, 0)  'テーブルNo=0でSelect
                '    納品伝票 追加
                Dim rowNewDenpyou As DataRow = dtDenpyou.NewRow  '行を生成
                dtDenpyou.Rows.Add(rowNewDenpyou)  '行をDataTableに追加
                '    納品明細 追加
                Dim rowNewMeisai As DataRow
                For iRow As Integer = 0 To dtMeisaiCopy.Rows.Count - 1
                    Dim drMeisaiCopy As DataRow = dtMeisaiCopy.Rows(iRow)

                    rowNewMeisai = dtMeisai.NewRow  '行を生成

                    rowNewMeisai("商品コード") = drMeisaiCopy("商品コード").ToString
                    rowNewMeisai("商品名称カナ") = drMeisaiCopy("商品名称カナ").ToString
                    rowNewMeisai("商品名称") = drMeisaiCopy("商品名称").ToString
                    rowNewMeisai("消費税率") = drMeisaiCopy("消費税率")
                    rowNewMeisai("軽減税率") = drMeisaiCopy("軽減税率")
                    rowNewMeisai("入数") = drMeisaiCopy("入数")
                    rowNewMeisai("セット数") = drMeisaiCopy("セット数")
                    rowNewMeisai("数量") = drMeisaiCopy("数量")
                    rowNewMeisai("単位") = drMeisaiCopy("単位").ToString
                    rowNewMeisai("税抜原価単価") = drMeisaiCopy("税抜原価単価")
                    rowNewMeisai("税込原価単価") = drMeisaiCopy("税込原価単価")
                    rowNewMeisai("税抜商品単価") = drMeisaiCopy("税抜商品単価")
                    rowNewMeisai("税込商品単価") = drMeisaiCopy("税込商品単価")
                    rowNewMeisai("税抜金額") = drMeisaiCopy("税抜金額")
                    rowNewMeisai("税込金額") = drMeisaiCopy("税込金額")
                    rowNewMeisai("税抜原価") = drMeisaiCopy("税抜原価")
                    rowNewMeisai("消費税") = drMeisaiCopy("消費税")
                    rowNewMeisai("備考") = drMeisaiCopy("備考").ToString

                    rowNewMeisai("テーブルNo") = 0
                    rowNewMeisai("納品伝票No") = rowNewDenpyou("テーブルNo") '納品明細の納品伝票Noに納品伝票のテーブルNoをセット
                    rowNewMeisai("行番号") = drMeisaiCopy("行番号")
                    rowNewMeisai("商品マスタNo") = GetInt(drMeisaiCopy("商品マスタNo"))
                    rowNewMeisai("商品税区分") = GetByte(drMeisaiCopy("商品税区分"))
                    If My.Settings.HanbaiKanriType = "C" Then
                        rowNewMeisai("受注明細No") = GetInt(drMeisaiCopy("受注明細No"))
                    End If

                    dtMeisai.Rows.Add(rowNewMeisai)  '行をDataTableに追加
                Next

                isSearchedDenpyou = False
            Else
                '検索した伝票の時
                isSearchedDenpyou = True
            End If
        End Using

        Return 1  '正常
    End Function

    '見積書・見積明細からデータを得、納品伝票DataSetを作成する（Denpyouに内容をセット）
    '  戻値：  True=正常, False=データなし
    Private Function GetRecordMitumori(ByVal tableNo As Integer) As Boolean
        Using cnTable As New SqlConnection(CSingleton.CSetting.Connect)
            cnTable.Open()

            Dim sSQL As String

            '対象データが変更され別テーブルNoに変わっている可能性があるため、変更後のテーブルNoを取得する（削除されていたら取得されない。変更していなければ、そのままのテーブルNoが取得される。）
            Dim newDenpyouNo As Integer = CDenpyouCommon.GetNewDenpyouNo("見積書", cnTable, tableNo)
            If newDenpyouNo > 0 Then
                tableNo = newDenpyouNo  '変更後のテーブルNo
            End If

            'TableNoをキーに見積書からデータを得る
            sSQL = "SELECT 見積書.テーブルNo, 見積書.コード, 見積書.処理日, 見積書.担当者マスタNo, 見積書.得意先税区分 AS 見積書税区分, 見積書.端数 AS 見積書端数, 見積書.掛率 AS 見積書掛率, 見積書.消費税率 AS 見積書消費税率, " _
                 & "得意先マスタ.マスタNo AS 得意先マスタNo, 得意先マスタ.コード AS 得意先コード, 得意先マスタ.名称, 得意先マスタ.名称カナ, 得意先マスタ.名称2, 得意先マスタ.敬称, 得意先マスタ.備考, 得意先マスタ.諸口フラグ, 得意先マスタ.請求先フラグ, 得意先マスタ.請求先マスタNo, " _
                 & "得意先マスタ.標準ﾌｫｰﾑ, 得意先マスタ.納品伝票フォーム, 得意先マスタ.納品伝票コード自動更新, 得意先マスタ.掛率, 得意先マスタ.端数, 得意先マスタ.伝票コードフラグ, 得意先マスタ.納品伝票項目数, 得意先マスタ.締日, 得意先マスタ.与信限度額, 得意先マスタ.税区分, 得意先マスタ.消費税計算方法, " _
                 & "請求先.標準ﾌｫｰﾑ AS 請求先標準ﾌｫｰﾑ, 請求先.納品伝票フォーム AS 請求先納品伝票フォーム, 請求先.納品伝票コード自動更新 AS 請求先納品伝票コード自動更新, 請求先.掛率 AS 請求先掛率, 請求先.端数 AS 請求先端数, 請求先.伝票コードフラグ AS 請求先伝票コードフラグ, 請求先.納品伝票項目数 AS 請求先納品伝票項目数, 請求先.締日 AS 請求先締日, 請求先.与信限度額 AS 請求先与信限度額, 請求先.税区分 AS 請求先税区分, 請求先.消費税計算方法 AS 請求先消費税計算方法, " _
                 & "CAST(ISNULL(納入先.得意先マスタNo,0) AS BIT) AS 納入先有無 " _
                 & "FROM 見積書 LEFT OUTER JOIN 得意先マスタ ON 見積書.得意先マスタNo = 得意先マスタ.マスタNo " _
                 & "LEFT OUTER JOIN 得意先マスタ AS 請求先 ON 得意先マスタ.請求先マスタNo = 請求先.マスタNo " _
                 & "LEFT OUTER JOIN (SELECT 得意先マスタNo FROM 納入先マスタ GROUP BY 得意先マスタNo) AS 納入先 ON 得意先マスタ.マスタNo = 納入先.得意先マスタNo " _
                 & "WHERE 見積書.テーブルNo = " & tableNo & " AND 見積書.削除 = 0 "
            Dim dtMitumori As DataTable = CDBCommon.GetDataTable(cnTable, sSQL)
            If CDBCommon.DBError OrElse dtMitumori.Rows.Count <= 0 Then
                MessageBox.Show("検索条件に該当する見積書は見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
            Dim drMitumori As DataRow = dtMitumori.Rows(0)

            If GetInt(drMitumori("得意先マスタNo")) = 0 Then
                MessageBox.Show("得意先コードが入力されていない見積書は、参照できません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            '見積書入力時の税区分・端数・掛率とマスタの設定が同じかどうかのチェック
            Dim masterZeikubun As Byte, masterHasuu As Short
            If drMitumori("請求先フラグ") Then
                masterZeikubun = drMitumori("請求先税区分")
                masterHasuu = drMitumori("請求先端数")
            Else
                masterZeikubun = drMitumori("税区分")
                masterHasuu = drMitumori("端数")
            End If
            Dim message As String = ""
            If drMitumori("見積書税区分") <> masterZeikubun Then
                message &= "税区分"
            End If
            If drMitumori("見積書端数") <> masterHasuu Then
                If message <> "" Then message &= "・"
                message &= "端数"
            End If
            If message <> "" Then
                If MessageBox.Show("見積書入力時の" & message & "が、得意先マスタの設定と違います。" & vbCrLf & vbCrLf _
                    & "得意先マスタの設定にあわせて再計算されますが、よろしいですか？", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
                  = DialogResult.No Then
                    Return False
                End If
            End If
            oldZeikubun = drMitumori("見積書税区分")  '見積書入力時の区分をHold（明細再計算の時、単価が変更入力されているかの判断に使用）
            oldHasuu = drMitumori("見積書端数")  '見積書入力時の区分をHold

            '倉庫はクリアしないようにHoldしておく
            Dim soukoMasterNo As Integer = Denpyou.Souko.MasterNo
            Dim soukoCode As String = Denpyou.Souko.Code
            Dim soukoName As String = Denpyou.Souko.Name

            '伝票クリア
            InitDenpyou()

            '倉庫が未入力でなければ、Holdした倉庫をセット
            If soukoMasterNo > 0 Then
                Denpyou.Souko.MasterNo = soukoMasterNo
                Denpyou.Souko.Code = soukoCode
                Denpyou.Souko.Name = soukoName
            End If

            '見積書の内容をDenpyouにセット
            With Denpyou
                .MitumoriNo = drMitumori("テーブルNo")
                .MitumoriCode = drMitumori("コード")

                With .Tokuisaki
                    .MasterNo = drMitumori("得意先マスタNo")
                    .Code = drMitumori("得意先コード").ToString
                    .Name = drMitumori("名称").ToString
                    .Name2 = drMitumori("名称2").ToString
                    .Keishou = drMitumori("敬称").ToString
                    .NameKana = drMitumori("名称カナ").ToString
                    .Bikou = drMitumori("備考").ToString
                    .Shokuchi = GetBoolean(drMitumori("諸口フラグ"))
                    .NounyusakiExist = drMitumori("納入先有無")

                    If GetBoolean(drMitumori("請求先フラグ")) Then
                        .SeikyuSaki = drMitumori("請求先マスタNo")
                        .Kakeritu = GetDecimal(drMitumori("請求先掛率"))
                        .Hyoujun = GetBoolean(drMitumori("請求先標準ﾌｫｰﾑ"))
                        .DenpyouCodeFlag = GetBoolean(drMitumori("請求先伝票コードフラグ"))
                        .Simebi = GetShort(drMitumori("請求先締日"))
                        .YosinGendo = GetDecimal(drMitumori("請求先与信限度額"))

                        If .Hyoujun = False Then
                            If Denpyou.Tokuisaki.DenpyouCodeFlag Then
                                Denpyou.Tokuisaki.DenpyouCodeUpdate = GetBoolean(drMitumori("請求先納品伝票コード自動更新"))
                            End If
                            Denpyou.NouhinDenpyou = drMitumori("請求先納品伝票フォーム").ToString
                            Denpyou.KoumokuSu = GetInt(drMitumori("請求先納品伝票項目数"))
                        End If
                        If Denpyou.KoumokuSu = 0 Then
                            Denpyou.KoumokuSu = MaxKoumoku
                        End If
                        Denpyou.Code = GetDenpyouCode(Denpyou.Tokuisaki.SeikyuSaki).ToString(New String("0", drJisha("納品伝票コード桁数")))
                        Denpyou.NewCode = CDec(Denpyou.Code)

                        .ZeiKubun = GetByte(drMitumori("請求先税区分"))  'マスタからセットし直す
                        .ShouhizeiKeisan = GetShort(drMitumori("請求先消費税計算方法"))
                        .Hasuu = GetShort(drMitumori("請求先端数"))
                    Else
                        .SeikyuSaki = drMitumori("得意先マスタNo")
                        .Kakeritu = GetDecimal(drMitumori("掛率"))
                        .Hyoujun = GetBoolean(drMitumori("標準ﾌｫｰﾑ"))
                        .DenpyouCodeFlag = GetBoolean(drMitumori("伝票コードフラグ"))
                        .Simebi = GetShort(drMitumori("締日"))
                        .YosinGendo = GetDecimal(drMitumori("与信限度額"))

                        If .Hyoujun = False Then
                            If Denpyou.Tokuisaki.DenpyouCodeFlag Then
                                Denpyou.Tokuisaki.DenpyouCodeUpdate = GetBoolean(drMitumori("納品伝票コード自動更新"))
                            End If
                            Denpyou.NouhinDenpyou = drMitumori("納品伝票フォーム").ToString
                            Denpyou.KoumokuSu = GetInt(drMitumori("納品伝票項目数"))
                        End If
                        If Denpyou.KoumokuSu = 0 Then
                            Denpyou.KoumokuSu = MaxKoumoku
                        End If
                        Denpyou.Code = GetDenpyouCode(Denpyou.Tokuisaki.SeikyuSaki).ToString(New String("0", drJisha("納品伝票コード桁数")))
                        Denpyou.NewCode = CDec(Denpyou.Code)

                        .ZeiKubun = GetByte(drMitumori("税区分"))  'マスタからセットし直す
                        .ShouhizeiKeisan = GetShort(drMitumori("消費税計算方法"))
                        .Hasuu = GetShort(drMitumori("端数"))
                    End If
                End With
                oldKakeritu = Denpyou.Tokuisaki.Kakeritu  '現在のマスタの掛率をセット（掛率変更は手入力とみなすため。そうしないと、マスタから再計算してしまう）
                oldSeikyuMasterNo = Denpyou.Tokuisaki.SeikyuSaki  '請求先をセット（SetFormのReCalcMeisaiで使用。見積書は、見積書の得意先を使用するため、そのままセット）

                '指定伝票かどうかを判定
                If CNouhin.CNouhinInstance.ChkSiteiDenpyou(Denpyou.NouhinDenpyou) Then
                    '指定伝票なら、納品伝票を終了し指定伝票の見積参照へ
                    isEnd = True  '終了
                    CNouhin.CNouhinInstance.TokuisakiNo = 0
                    CNouhin.CNouhinInstance.TableNo = 0
                    CNouhin.CNouhinInstance.JutyuNo = 0
                    CNouhin.CNouhinInstance.MitumoriNo = tableNo
                    Me.DialogResult = DialogResult.OK  '結果を渡す
                    Me.Close()
                    Return False  '指定伝票
                End If

                With .Tantousha
                    .MasterNo = GetInt(drMitumori("担当者マスタNo"))
                    Dim CTantousha As New HanbaikanriDialog.CTantousha()
                    Dim drTantou As DataRow = CTantousha.GetMaster(.MasterNo, cnTable)
                    If drTantou IsNot Nothing Then
                        .Code = drTantou("コード").ToString
                        .Name = drTantou("氏名").ToString
                        .NameKana = drTantou("氏名カナ").ToString
                    End If
                End With
            End With

            '参照元伝票の消費税率をHold
            Dim CShouhiZei As New HanbaikanriDialog.CShouhiZei()
            oldAryRate = CShouhiZei.GetRate2(cnTable, drMitumori("処理日"))  '消費税率1,2取得
            'If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
            '    MessageBox.Show("見積書の消費税率は[" & oldAryRate(0).ToString("#0%") & "]ですが、[" & Denpyou.aryRate(0).ToString("#0%") & "]に変更します。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            'End If
            MessageBox.Show("見積時の消費税率と、現在の消費税マスタ/商品マスタの設定が違う場合、現在の設定で計算し直します。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

            '納品伝票、納品明細の新規DataSetを作成
            MakeEditCommandsDenpyou(cnTable, 0)  'テーブルNo=0でSelect

            'Denpyouより、納品伝票のDataTableを追加
            '  納品伝票 追加
            Dim rowNewDenpyou As DataRow = dtDenpyou.NewRow  '行を生成
            dtDenpyou.Rows.Add(rowNewDenpyou)  '行をDataTableに追加

            '見積明細からデータを得る
            sSQL = "SELECT * FROM 見積明細 " _
                 & "WHERE 見積明細.見積書No = " & tableNo & " AND 見積明細.削除 = 0 " _
                 & "ORDER BY 見積明細.行番号"
            Dim dtMitumoriMeisai As DataTable = CDBCommon.GetDataTable(cnTable, sSQL)
            If CDBCommon.DBError OrElse dtMitumoriMeisai.Rows.Count <= 0 Then
                MessageBox.Show("検索条件に該当する見積明細は見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            '見積明細の内容をセット
            If Denpyou.KoumokuSu < dtMitumoriMeisai.Rows.Count Then
                Denpyou.KoumokuSu = dtMitumoriMeisai.Rows.Count
            End If

            '  納品明細 追加
            Dim isChangedDigit As Boolean = False  '見積書と納品伝票で、小数点以下の桁数設定が違う時True
            Dim rowNewMeisai As DataRow
            For iRow As Integer = 0 To dtMitumoriMeisai.Rows.Count - 1
                Dim drMitumoriMeisai As DataRow = dtMitumoriMeisai.Rows(iRow)

                rowNewMeisai = dtMeisai.NewRow  '行を生成

                rowNewMeisai("商品コード") = drMitumoriMeisai("商品コード").ToString
                rowNewMeisai("商品名称カナ") = drMitumoriMeisai("商品名称カナ").ToString
                rowNewMeisai("商品名称") = drMitumoriMeisai("商品名称").ToString
                rowNewMeisai("消費税率") = drMitumoriMeisai("消費税率")
                rowNewMeisai("軽減税率") = drMitumoriMeisai("軽減税率")
                rowNewMeisai("入数") = drMitumoriMeisai("入数")
                rowNewMeisai("セット数") = drMitumoriMeisai("セット数")
                rowNewMeisai("数量") = drMitumoriMeisai("数量")
                rowNewMeisai("単位") = drMitumoriMeisai("単位").ToString
                rowNewMeisai("税抜原価単価") = drMitumoriMeisai("税抜原価単価")
                rowNewMeisai("税込原価単価") = drMitumoriMeisai("税込原価単価")
                rowNewMeisai("税抜商品単価") = drMitumoriMeisai("税抜商品単価")
                rowNewMeisai("税込商品単価") = drMitumoriMeisai("税込商品単価")
                rowNewMeisai("税抜金額") = drMitumoriMeisai("税抜金額")
                rowNewMeisai("税込金額") = drMitumoriMeisai("税込金額")
                rowNewMeisai("税抜原価") = drMitumoriMeisai("税抜原価")
                rowNewMeisai("消費税") = drMitumoriMeisai("消費税")
                rowNewMeisai("備考") = ""

                '  見積時の小数点以下桁数が納品伝票の小数点以下桁数より大きい時、得意先の端数処理方法で丸めて桁数をカットする
                '    （金額の再計算は、SetFormで行なう）
                If GetByte(drJisha("売掛数量少数桁数")) < GetDigitDecimalPoint(drMitumoriMeisai("数量").ToString()) Then
                    isChangedDigit = True
                    rowNewMeisai("数量") = Marume(drMitumoriMeisai("数量"), GetByte(drJisha("売掛数量少数桁数")) * -1, Denpyou.Tokuisaki.Hasuu)
                End If
                If GetByte(drJisha("売掛数量少数桁数")) < GetDigitDecimalPoint(drMitumoriMeisai("入数").ToString()) Then
                    isChangedDigit = True
                    rowNewMeisai("入数") = Marume(drMitumoriMeisai("入数"), GetByte(drJisha("売掛数量少数桁数")) * -1, Denpyou.Tokuisaki.Hasuu)
                End If
                If Denpyou.Tokuisaki.ZeiKubun = enZeikubun.外税 Then
                    If GetByte(drJisha("売掛単価少数桁数")) < GetDigitDecimalPoint(drMitumoriMeisai("税抜商品単価").ToString()) Then
                        isChangedDigit = True
                        rowNewMeisai("税抜商品単価") = Marume(drMitumoriMeisai("税抜商品単価"), GetByte(drJisha("売掛単価少数桁数")) * -1, Denpyou.Tokuisaki.Hasuu)
                    End If
                Else
                    If GetByte(drJisha("売掛単価少数桁数")) < GetDigitDecimalPoint(drMitumoriMeisai("税込商品単価").ToString()) Then
                        isChangedDigit = True
                        rowNewMeisai("税込商品単価") = Marume(drMitumoriMeisai("税込商品単価"), GetByte(drJisha("売掛単価少数桁数")) * -1, Denpyou.Tokuisaki.Hasuu)
                    End If
                End If

                rowNewMeisai("テーブルNo") = 0
                rowNewMeisai("納品伝票No") = rowNewDenpyou("テーブルNo") '納品明細の納品伝票Noに納品伝票のテーブルNoをセット
                rowNewMeisai("行番号") = drMitumoriMeisai("行番号")
                rowNewMeisai("商品マスタNo") = GetInt(drMitumoriMeisai("商品マスタNo"))
                rowNewMeisai("商品税区分") = GetByte(drMitumoriMeisai("商品税区分"))
                If My.Settings.HanbaiKanriType = "C" Then
                    rowNewMeisai("受注明細No") = 0
                End If

                dtMeisai.Rows.Add(rowNewMeisai)  '行をDataTableに追加
            Next

            If isChangedDigit Then
                MessageBox.Show("参照元見積書の小数点以下桁数が、納品伝票の小数点以下桁数より大きいため、" & vbCrLf _
                    & "得意先の端数処理方法で丸めて数量・単価をセットし、金額を再計算します。" & vbCrLf & vbCrLf _
                    & "内容を確認してください。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using

        isSearchedDenpyou = False

        Return True
    End Function

    '受注伝票・受注明細からデータを得、納品伝票DataSetを作成する（Denpyouに内容をセット）
    '  戻値：  True=正常, False=データなし
    Private Function GetRecordJutyu(ByVal tableNo As Integer) As Boolean
        Using cnTable As New SqlConnection(CSingleton.CSetting.Connect)
            cnTable.Open()

            Dim sSQL As String

            '対象データが変更され別テーブルNoに変わっている可能性があるため、変更後のテーブルNoを取得する（削除されていたら取得されない。変更していなければ、そのままのテーブルNoが取得される。）
            Dim newDenpyouNo As Integer = CDenpyouCommon.GetNewDenpyouNo("受注伝票", cnTable, tableNo)
            If newDenpyouNo > 0 Then
                tableNo = newDenpyouNo  '変更後のテーブルNo
            End If

            'TableNoをキーに受注伝票からデータを得る
            sSQL = "SELECT 受注伝票.テーブルNo, 受注伝票.コード, 受注伝票.納期日, 受注伝票.納入先マスタNo, 受注伝票.納入先コード, 受注伝票.納入先名称, 受注伝票.納入先名称2, 受注伝票.納入先敬称, 受注伝票.担当者マスタNo, 受注伝票.得意先税区分, 受注伝票.消費税計算方法, 受注伝票.端数, " _
                 & "得意先マスタ.マスタNo AS 得意先マスタNo, 得意先マスタ.コード AS 得意先コード, 得意先マスタ.名称, 得意先マスタ.名称カナ, 得意先マスタ.名称2, 得意先マスタ.敬称, 得意先マスタ.備考, 得意先マスタ.諸口フラグ, 得意先マスタ.請求先フラグ, 得意先マスタ.請求先マスタNo, " _
                 & "得意先マスタ.標準ﾌｫｰﾑ, 得意先マスタ.納品伝票フォーム, 得意先マスタ.納品伝票コード自動更新, 得意先マスタ.掛率, 得意先マスタ.伝票コードフラグ, 得意先マスタ.納品伝票項目数, 得意先マスタ.締日, 得意先マスタ.与信限度額, " _
                 & "請求先.標準ﾌｫｰﾑ AS 請求先標準ﾌｫｰﾑ, 請求先.納品伝票フォーム AS 請求先納品伝票フォーム, 請求先.納品伝票コード自動更新 AS 請求先納品伝票コード自動更新, 請求先.掛率 AS 請求先掛率, 請求先.端数 AS 請求先端数, 請求先.税区分 AS 請求先税区分, 請求先.消費税計算方法 AS 請求先消費税計算方法, 請求先.伝票コードフラグ AS 請求先伝票コードフラグ, 請求先.納品伝票項目数 AS 請求先納品伝票項目数, 請求先.締日 AS 請求先締日, 請求先.与信限度額 AS 請求先与信限度額, " _
                 & "CAST(ISNULL(納入先.得意先マスタNo,0) AS BIT) AS 納入先有無 " _
                 & "FROM 受注伝票 LEFT OUTER JOIN 得意先マスタ ON 受注伝票.得意先マスタNo = 得意先マスタ.マスタNo " _
                 & "LEFT OUTER JOIN 得意先マスタ AS 請求先 ON 得意先マスタ.請求先マスタNo = 請求先.マスタNo " _
                 & "LEFT OUTER JOIN (SELECT 得意先マスタNo FROM 納入先マスタ GROUP BY 得意先マスタNo) AS 納入先 ON 得意先マスタ.マスタNo = 納入先.得意先マスタNo " _
                 & "WHERE 受注伝票.テーブルNo = " & tableNo & " AND 受注伝票.削除 = 0 "
            Dim dtJutyu As DataTable = CDBCommon.GetDataTable(cnTable, sSQL)
            If CDBCommon.DBError OrElse dtJutyu.Rows.Count <= 0 Then
                MessageBox.Show("検索条件に該当する受注伝票は見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
            Dim drJutyu As DataRow = dtJutyu.Rows(0)

            If GetInt(drJutyu("得意先マスタNo")) = 0 Then
                MessageBox.Show("得意先コードが入力されていない受注伝票は、参照できません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            '受注入力時の税区分・消費税計算方法・端数とマスタの設定が同じかどうかのチェック
            Dim message As String = ""
            If drJutyu("請求先税区分") <> drJutyu("得意先税区分") Then
                message = "税区分"
            End If
            If (drJutyu("請求先税区分") <> enZeikubun.非課税 AndAlso drJutyu("得意先税区分") <> enZeikubun.非課税) _
              AndAlso drJutyu("請求先消費税計算方法") <> drJutyu("消費税計算方法") Then
                If message <> "" Then message &= "・"
                message &= "消費税計算方法"
            End If
            If drJutyu("請求先端数") <> drJutyu("端数") Then
                If message <> "" Then message &= "・"
                message &= "端数"
            End If
            If message <> "" Then
                MessageBox.Show("複写元の" & message & "が、現在の得意先マスタの設定と違います。" & vbCrLf & "内容を確認してください。" _
                        & vbCrLf & "（得意先を入力し直すと、最新の設定になります。）", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            '倉庫はクリアしないようにHoldしておく
            Dim soukoMasterNo As Integer = Denpyou.Souko.MasterNo
            Dim soukoCode As String = Denpyou.Souko.Code
            Dim soukoName As String = Denpyou.Souko.Name

            '伝票クリア
            InitDenpyou()

            '倉庫が未入力でなければ、Holdした倉庫をセット
            If soukoMasterNo <> 0 Then
                Denpyou.Souko.MasterNo = soukoMasterNo
                Denpyou.Souko.Code = soukoCode
                Denpyou.Souko.Name = soukoName
            End If

            '受注伝票の内容をDenpyouにセット
            With Denpyou
                .JutyuDenpyouNo = drJutyu("テーブルNo")
                .JutyuCode = drJutyu("コード").ToString

                With .Tokuisaki
                    .MasterNo = drJutyu("得意先マスタNo")
                    .Code = drJutyu("得意先コード").ToString
                    .Name = drJutyu("名称").ToString
                    .Name2 = drJutyu("名称2").ToString
                    .Keishou = drJutyu("敬称").ToString
                    .NameKana = drJutyu("名称カナ").ToString
                    .Bikou = drJutyu("備考").ToString
                    .Shokuchi = GetBoolean(drJutyu("諸口フラグ"))
                    .NounyusakiExist = drJutyu("納入先有無")

                    If GetBoolean(drJutyu("請求先フラグ")) Then
                        .SeikyuSaki = drJutyu("請求先マスタNo")
                        .Kakeritu = GetDecimal(drJutyu("請求先掛率"))
                        .Hyoujun = drJutyu("請求先標準ﾌｫｰﾑ")
                        .DenpyouCodeFlag = GetBoolean(drJutyu("請求先伝票コードフラグ"))
                        .Simebi = GetShort(drJutyu("請求先締日"))
                        .YosinGendo = GetDecimal(drJutyu("請求先与信限度額"))

                        If .Hyoujun = False Then
                            If Denpyou.Tokuisaki.DenpyouCodeFlag Then
                                Denpyou.Tokuisaki.DenpyouCodeUpdate = GetBoolean(drJutyu("請求先納品伝票コード自動更新"))
                            End If
                            Denpyou.NouhinDenpyou = drJutyu("請求先納品伝票フォーム").ToString
                            Denpyou.KoumokuSu = GetInt(drJutyu("請求先納品伝票項目数"))
                        End If
                        If Denpyou.KoumokuSu = 0 Then
                            Denpyou.KoumokuSu = MaxKoumoku
                        End If
                        Denpyou.Code = GetDenpyouCode(Denpyou.Tokuisaki.SeikyuSaki).ToString(New String("0", drJisha("納品伝票コード桁数")))
                        Denpyou.NewCode = CDec(Denpyou.Code)

                    Else
                        .SeikyuSaki = drJutyu("得意先マスタNo")
                        .Kakeritu = GetDecimal(drJutyu("掛率"))
                        .Hyoujun = GetBoolean(drJutyu("標準ﾌｫｰﾑ"))
                        .DenpyouCodeFlag = GetBoolean(drJutyu("伝票コードフラグ"))
                        .Simebi = GetShort(drJutyu("締日"))
                        .YosinGendo = GetDecimal(drJutyu("与信限度額"))

                        If .Hyoujun = False Then
                            If Denpyou.Tokuisaki.DenpyouCodeFlag Then
                                Denpyou.Tokuisaki.DenpyouCodeUpdate = GetBoolean(drJutyu("納品伝票コード自動更新"))
                            End If
                            Denpyou.NouhinDenpyou = drJutyu("納品伝票フォーム").ToString
                            Denpyou.KoumokuSu = GetInt(drJutyu("納品伝票項目数"))
                        End If
                        If Denpyou.KoumokuSu = 0 Then
                            Denpyou.KoumokuSu = MaxKoumoku
                        End If
                        Denpyou.Code = GetDenpyouCode(Denpyou.Tokuisaki.SeikyuSaki).ToString(New String("0", drJisha("納品伝票コード桁数")))
                        Denpyou.NewCode = CDec(Denpyou.Code)
                    End If

                    .ZeiKubun = GetByte(drJutyu("得意先税区分"))
                    .ShouhizeiKeisan = GetShort(drJutyu("消費税計算方法"))
                    .Hasuu = GetShort(drJutyu("端数"))
                End With

                '指定伝票かどうかを判定
                If CNouhin.CNouhinInstance.ChkSiteiDenpyou(Denpyou.NouhinDenpyou) Then
                    '指定伝票なら、納品伝票を終了し指定伝票の見積参照へ
                    isEnd = True  '終了
                    CNouhin.CNouhinInstance.TokuisakiNo = 0
                    CNouhin.CNouhinInstance.TableNo = 0
                    CNouhin.CNouhinInstance.JutyuNo = tableNo
                    CNouhin.CNouhinInstance.MitumoriNo = 0
                    Me.DialogResult = DialogResult.OK  '結果を渡す
                    Me.Close()
                    Return 0  '指定伝票
                End If

                With .NounyuuSaki
                    .MasterNo = GetInt(drJutyu("納入先マスタNo"))
                    .Code = drJutyu("納入先コード").ToString
                    .Name = drJutyu("納入先名称").ToString
                    .Name2 = drJutyu("納入先名称2").ToString
                    .Keishou = drJutyu("納入先敬称").ToString
                End With

                With .Tantousha
                    .MasterNo = GetInt(drJutyu("担当者マスタNo"))
                    Dim CTantousha As New HanbaikanriDialog.CTantousha()
                    Dim drTantou As DataRow = CTantousha.GetMaster(.MasterNo, cnTable)
                    If drTantou IsNot Nothing Then
                        .Code = drTantou("コード").ToString
                        .Name = drTantou("氏名").ToString
                        .NameKana = drTantou("氏名カナ").ToString
                    End If
                End With
            End With

            '参照元伝票の消費税率をHold
            Dim CShouhiZei As New HanbaikanriDialog.CShouhiZei()
            oldAryRate = CShouhiZei.GetRate2(cnTable, drJutyu("納期日"))  '消費税率1,2取得

            '納品伝票、納品明細の新規DataSetを作成
            MakeEditCommandsDenpyou(cnTable, 0)  'テーブルNo=0でSelect

            'Denpyouより、納品伝票のDataTableを追加
            '  納品伝票 追加
            Dim rowNewDenpyou As DataRow = dtDenpyou.NewRow  '行を生成
            dtDenpyou.Rows.Add(rowNewDenpyou)  '行をDataTableに追加

            '受注明細からデータを得る
            sSQL = "SELECT * FROM 受注明細 " _
                 & "WHERE 受注明細.受注伝票No = " & tableNo & " And 受注明細.削除 = 0 " _
                 & "ORDER BY 受注明細.行番号"
            Dim dtJutyuMeisai As DataTable = CDBCommon.GetDataTable(cnTable, sSQL)
            If CDBCommon.DBError OrElse dtJutyuMeisai.Rows.Count <= 0 Then
                MessageBox.Show("検索条件に該当する受注明細は見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            '受注明細の内容をセット
            If Denpyou.KoumokuSu < dtJutyuMeisai.Rows.Count Then
                Denpyou.KoumokuSu = dtJutyuMeisai.Rows.Count
            End If

            '  納品明細 追加
            Dim rowNewMeisai As DataRow
            For iRow As Integer = 0 To dtJutyuMeisai.Rows.Count - 1
                Dim drJutyuMeisai As DataRow = dtJutyuMeisai.Rows(iRow)

                rowNewMeisai = dtMeisai.NewRow  '行を生成

                rowNewMeisai("商品コード") = drJutyuMeisai("商品コード").ToString
                rowNewMeisai("商品名称カナ") = drJutyuMeisai("商品名称カナ").ToString
                rowNewMeisai("商品名称") = drJutyuMeisai("商品名称").ToString
                rowNewMeisai("消費税率") = drJutyuMeisai("消費税率")
                rowNewMeisai("軽減税率") = drJutyuMeisai("軽減税率")
                rowNewMeisai("入数") = drJutyuMeisai("入数")
                rowNewMeisai("セット数") = 0
                rowNewMeisai("数量") = 0
                rowNewMeisai("単位") = drJutyuMeisai("単位").ToString
                rowNewMeisai("税抜原価単価") = drJutyuMeisai("税抜原価単価")
                rowNewMeisai("税込原価単価") = drJutyuMeisai("税込原価単価")
                rowNewMeisai("税抜商品単価") = drJutyuMeisai("税抜商品単価")
                rowNewMeisai("税込商品単価") = drJutyuMeisai("税込商品単価")
                rowNewMeisai("税抜金額") = 0
                rowNewMeisai("税込金額") = 0
                rowNewMeisai("税抜原価") = 0
                rowNewMeisai("消費税") = 0
                rowNewMeisai("備考") = drJutyuMeisai("備考").ToString

                rowNewMeisai("テーブルNo") = 0
                rowNewMeisai("納品伝票No") = rowNewDenpyou("テーブルNo")  '納品明細の納品伝票Noに納品伝票のテーブルNoをセット
                rowNewMeisai("行番号") = drJutyuMeisai("行番号")
                rowNewMeisai("商品マスタNo") = GetInt(drJutyuMeisai("商品マスタNo"))
                rowNewMeisai("商品税区分") = GetByte(drJutyuMeisai("商品税区分"))
                If My.Settings.HanbaiKanriType = "C" Then
                    rowNewMeisai("受注明細No") = drJutyuMeisai("テーブルNo")
                End If

                dtMeisai.Rows.Add(rowNewMeisai)  '行をDataTableに追加
            Next
        End Using

        isSearchedDenpyou = False

        Return True
    End Function

    '受注明細からデータを得、明細行にセット
    '  戻値：  True=正常, False=データなし
    Private Function GetRecordJutyuMeisai(ByVal mRow As Integer, ByVal tableNo As Integer) As Boolean
        Using cnTable As New SqlConnection(CSingleton.CSetting.Connect)
            cnTable.Open()

            '受注明細からデータを得る
            Dim sSQL As String
            sSQL = "SELECT 受注伝票.コード, 受注伝票.納期日, 受注明細.* " _
                 & "FROM 受注明細 INNER JOIN 受注伝票 ON 受注明細.受注伝票No = 受注伝票.テーブルNo " _
                 & "WHERE 受注明細.テーブルNo = " & tableNo & " And 受注明細.削除 = 0 And 受注伝票.削除 = 0"
            Dim dtJutyuMeisai As DataTable = CDBCommon.GetDataTable(cnTable, sSQL)
            If CDBCommon.DBError OrElse dtJutyuMeisai.Rows.Count <= 0 Then
                MessageBox.Show("検索条件に該当する受注明細は見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
            Dim drJutyuMeisai As DataRow = dtJutyuMeisai.Rows(0)

            '参照元伝票の消費税率をHold
            Dim CShouhiZei As New HanbaikanriDialog.CShouhiZei()
            oldAryRate = CShouhiZei.GetRate2(cnTable, drJutyuMeisai("納期日"))  '消費税率1,2取得

            '受注明細の内容をSheetにセット
            MRowSheet.MRows(mRow)("受注コード").Value = drJutyuMeisai("コード").ToString
            MRowSheet.MRows(mRow)("商品コード").Value = drJutyuMeisai("商品コード").ToString
            MRowSheet.MRows(mRow)("商品名称カナ").Value = drJutyuMeisai("商品名称カナ").ToString
            MRowSheet.MRows(mRow)("商品名称").Value = drJutyuMeisai("商品名称").ToString
            MRowSheet.MRows(mRow)("消費税率").Value = drJutyuMeisai("消費税率")
            MRowSheet.MRows(mRow)("軽減税率").Value = drJutyuMeisai("軽減税率")
            MRowSheet.MRows(mRow)("入数").Value = drJutyuMeisai("入数")
            MRowSheet.MRows(mRow)("セット数").Value = 0
            MRowSheet.MRows(mRow)("数量").Value = 0
            MRowSheet.MRows(mRow)("単位IN").Value = Nothing    'クリアしておかないと、単位が""の時クリアされない
            MRowSheet.MRows(mRow)("単位IN").Text = drJutyuMeisai("単位").ToString
            MRowSheet.MRows(mRow)("金額").Value = 0
            MRowSheet.MRows(mRow)("備考").Value = drJutyuMeisai("備考").ToString

            MRowSheet.MRows(mRow)("単位").Value = drJutyuMeisai("単位").ToString
            MRowSheet.MRows(mRow)("商品マスタNo").Value = drJutyuMeisai("商品マスタNo")
            MRowSheet.MRows(mRow)("税抜原価単価").Value = drJutyuMeisai("税抜原価単価")
            MRowSheet.MRows(mRow)("税込原価単価").Value = drJutyuMeisai("税込原価単価")
            MRowSheet.MRows(mRow)("税抜商品単価").Value = drJutyuMeisai("税抜商品単価")
            MRowSheet.MRows(mRow)("税込商品単価").Value = drJutyuMeisai("税込商品単価")
            MRowSheet.MRows(mRow)("税抜金額").Value = 0
            MRowSheet.MRows(mRow)("税込金額").Value = 0
            MRowSheet.MRows(mRow)("税抜原価").Value = 0
            MRowSheet.MRows(mRow)("消費税").Value = 0
            MRowSheet.MRows(mRow)("商品税区分").Value = drJutyuMeisai("商品税区分")
            MRowSheet.MRows(mRow)("受注明細No").Value = drJutyuMeisai("テーブルNo")

            If Denpyou.Tokuisaki.ZeiKubun = enZeikubun.外税 Then
                '得意先が外税の時は、税抜価格を表示
                MRowSheet.MRows(mRow)("原価単価").Value = MRowSheet.MRows(mRow)("税抜原価単価").Value
                MRowSheet.MRows(mRow)("商品単価").Value = MRowSheet.MRows(mRow)("税抜商品単価").Value
            Else
                '得意先が内税、非課税の時は、税込価格を表示
                MRowSheet.MRows(mRow)("原価単価").Value = MRowSheet.MRows(mRow)("税込原価単価").Value
                MRowSheet.MRows(mRow)("商品単価").Value = MRowSheet.MRows(mRow)("税込商品単価").Value
            End If
            MRowSheet.MRows(mRow)("得意先別単価").Value = Math.Abs(MRowSheet.MRows(mRow)("商品単価").Value)

            '入力可/不可項目の設定
            If MRowSheet.MRows(mRow)("商品単価").Value = 0 Then
                MRowSheet.MRows(mRow)("金額").Lock = False
            Else
                '単価が入力されている時、金額は入力不可
                MRowSheet.MRows(mRow)("金額").Lock = True
            End If

            '  商品マスタNoをキーに、商品マスタのレコードを得る
            Dim drShouhin As DataRow = Nothing
            If GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value) > 0 Then
                Dim CShouhin As New HanbaikanriDialog.CShouhin()
                drShouhin = CShouhin.GetMaster(GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value), cnTable)
            End If
            If drShouhin IsNot Nothing Then
                MRowSheet.MRows(mRow)("在庫管理有効").Value = drShouhin("在庫管理有効")
                MRowSheet.MRows(mRow)("消費税率区分").Value = drShouhin("消費税率区分")
                '在庫数のセットとチェック（実際の在庫数チェックは数量入力時に行なう）
                CheckZaiko(mRow, False)
            Else
                '商品マスタなしのためクリア
                MRowSheet.MRows(mRow)("商品マスタNo").Value = 0
                MRowSheet.MRows(mRow)("商品コード").Value = ""

                MRowSheet.MRows(mRow)("在庫管理有効").Value = False
                MRowSheet.MRows(mRow)("消費税率区分").Value = 0
            End If
        End Using

        Return True
    End Function

    '仕入伝票・仕入明細からデータを得、納品伝票DataSetを作成する（Denpyouに内容をセット）
    '  戻値：  True=正常, False=データなし
    Private Function GetRecordSiire(ByVal tableNo As Integer) As Boolean
        Using cnTable As New SqlConnection(CSingleton.CSetting.Connect)
            cnTable.Open()

            '対象データが変更され別テーブルNoに変わっている可能性があるため、変更後のテーブルNoを取得する（削除されていたら取得されない。変更していなければ、そのままのテーブルNoが取得される。）
            Dim newDenpyouNo As Integer = CDenpyouCommon.GetNewDenpyouNo("仕入伝票", cnTable, tableNo)
            If newDenpyouNo > 0 Then
                tableNo = newDenpyouNo  '変更後のテーブルNo
            End If

            'TableNoをキーに仕入伝票・仕入明細からデータを得る
            Dim sSQL As String
            sSQL = "SELECT 仕入伝票.テーブルNo, 仕入伝票.コード, 仕入伝票.処理日, 仕入伝票.倉庫マスタNo, 仕入伝票.仕入先税区分, " _
                 & "仕入明細.行番号, 仕入明細.商品マスタNo, 仕入明細.商品税区分, 仕入明細.商品コード, 仕入明細.商品名称カナ, 仕入明細.商品名称, " _
                 & "ISNULL(仕入明細.消費税率,0) AS 消費税率, 仕入明細.軽減税率, " _
                 & "仕入明細.入数, 仕入明細.単位, 仕入明細.税抜商品単価, 仕入明細.税込商品単価 " _
                 & "FROM 仕入伝票 INNER JOIN 仕入明細 ON 仕入伝票.テーブルNo = 仕入明細.仕入伝票No " _
                 & "WHERE 仕入伝票.テーブルNo = " & tableNo & " AND 仕入伝票.削除 = 0 AND 仕入明細.削除 = 0 " _
                 & "ORDER BY 仕入明細.行番号"
            Dim dtSiire As DataTable = CDBCommon.GetDataTable(cnTable, sSQL)
            If CDBCommon.DBError OrElse dtSiire.Rows.Count <= 0 Then
                MessageBox.Show("検索条件に該当する仕入伝票は見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            Dim drSiire As DataRow = dtSiire.Rows(0)

            InitMeisai()  'ヘッダ部はクリアしない

            '仕入伝票の内容をDenpyouにセット
            With Denpyou
                .SiireDenpyouNo = drSiire("テーブルNo")
                .SiireCode = drSiire("コード").ToString

                '倉庫マスタNoをキーに、倉庫マスタのレコードを得る
                With .Souko
                    .MasterNo = GetInt(drSiire("倉庫マスタNo"))
                    Dim CSouko As New HanbaikanriDialog.CSouko()
                    Dim drSouko As DataRow = CSouko.GetMaster(.MasterNo, cnTable)
                    If drSouko IsNot Nothing Then
                        .Code = drSouko("コード").ToString
                        .Name = drSouko("名称").ToString
                    Else
                        .Code = ""
                        .Name = ""
                    End If
                End With
            End With

            '参照元伝票の消費税率をHold
            Dim CShouhiZei As New HanbaikanriDialog.CShouhiZei()
            oldAryRate = CShouhiZei.GetRate2(cnTable, drSiire("処理日"))  '消費税率1,2取得

            '納品伝票、納品明細の新規DataSetを作成
            MakeEditCommandsDenpyou(cnTable, 0)  'テーブルNo=0でSelect

            'Denpyouより、納品伝票のDataTableを追加
            '  納品伝票 追加
            Dim rowNewDenpyou As DataRow = dtDenpyou.NewRow  '行を生成
            dtDenpyou.Rows.Add(rowNewDenpyou)  '行をDataTableに追加

            '仕入明細の内容をセット
            If Denpyou.KoumokuSu < dtSiire.Rows.Count Then
                Denpyou.KoumokuSu = dtSiire.Rows.Count
            End If

            '  納品明細 追加
            Dim rowNewMeisai As DataRow
            For iRow As Integer = 0 To dtSiire.Rows.Count - 1
                Dim drSiireMeisai As DataRow = dtSiire.Rows(iRow)

                rowNewMeisai = dtMeisai.NewRow      '行を生成

                rowNewMeisai("商品コード") = drSiireMeisai("商品コード").ToString
                rowNewMeisai("商品名称カナ") = drSiireMeisai("商品名称カナ").ToString
                rowNewMeisai("商品名称") = drSiireMeisai("商品名称").ToString
                If Denpyou.Tokuisaki.ZeiKubun = enZeikubun.非課税 Then
                    rowNewMeisai("消費税率") = 0
                    rowNewMeisai("軽減税率") = False
                Else
                    rowNewMeisai("消費税率") = drSiireMeisai("消費税率")
                    rowNewMeisai("軽減税率") = drSiireMeisai("軽減税率")
                End If
                rowNewMeisai("入数") = drSiireMeisai("入数")
                rowNewMeisai("セット数") = 0
                rowNewMeisai("数量") = 0
                rowNewMeisai("単位") = drSiireMeisai("単位").ToString

                '税抜/税込原価単価のセット
                Dim genkaTankaNuki As Decimal = drSiireMeisai("税抜商品単価")
                Dim genkaTankaKomi As Decimal = drSiireMeisai("税込商品単価")
                If GetByte(drSiire("仕入先税区分")) = enZeikubun.非課税 Then
                    genkaTankaKomi = GetZeikomiFromZeinuki(genkaTankaNuki, GetByte(drJisha("買掛単価少数桁数")), Denpyou.Tokuisaki.Hasuu, drSiireMeisai("消費税率"))
                End If
                If GetByte(drSiireMeisai("商品税区分")) = enZeikubun.非課税 Then
                    genkaTankaKomi = genkaTankaNuki
                End If
                '  得意先の税区分により、原価単価をセット
                Select Case Denpyou.Tokuisaki.ZeiKubun
                    Case enZeikubun.外税, enZeikubun.内税
                        rowNewMeisai("税抜原価単価") = genkaTankaNuki
                        rowNewMeisai("税込原価単価") = genkaTankaKomi

                    Case enZeikubun.非課税
                        rowNewMeisai("税抜原価単価") = genkaTankaNuki
                        rowNewMeisai("税込原価単価") = genkaTankaNuki
                End Select

                rowNewMeisai("税抜商品単価") = 0
                rowNewMeisai("税込商品単価") = 0
                rowNewMeisai("税抜金額") = 0
                rowNewMeisai("税込金額") = 0
                rowNewMeisai("税抜原価") = 0
                rowNewMeisai("消費税") = 0
                rowNewMeisai("備考") = ""

                rowNewMeisai("テーブルNo") = 0
                rowNewMeisai("納品伝票No") = rowNewDenpyou("テーブルNo") '納品明細の納品伝票Noに納品伝票のテーブルNoをセット
                rowNewMeisai("行番号") = drSiireMeisai("行番号")
                rowNewMeisai("商品マスタNo") = GetInt(drSiireMeisai("商品マスタNo"))
                rowNewMeisai("商品税区分") = GetByte(drSiireMeisai("商品税区分"))
                If My.Settings.HanbaiKanriType = "C" Then
                    rowNewMeisai("受注明細No") = 0
                End If

                dtMeisai.Rows.Add(rowNewMeisai)  '行をDataTableに追加
            Next
        End Using

        isSearchedDenpyou = False

        Return True
    End Function

    '納品伝票の定義(Denpyou)から、各項目を画面にセットする
    '  引数：AfterSerch=GetRecord後にこの処理を実行する時のみTrue
    '  引数：ReCalc=明細金額を再計算させる時True（得意先変更後にこの処理を実行する時、仕入参照後、見積参照後）
    '  引数：ZaikoCheck=最小在庫数チェックしない場合はFalse（伝票検索時の初回のみチェックしない）（GetRecord後にのみ使用）
    '  引数：GetSiire=明細の単価をマスタから再セットする時True（仕入伝票参照後）
    Private Sub SetForm(Optional ByVal AfterSerch As Boolean = False, Optional ByVal ReCalc As Boolean = False,
                        Optional ByVal ZaikoCheck As Boolean = True, Optional ByVal GetSiire As Boolean = False)
        '納品伝票のセット
        If Denpyou.TableNo <= 0 AndAlso Denpyou.Tokuisaki.DenpyouCodeUpdate Then
            '新規登録で、伝票コード自動更新の場合
            CFormCommon.IniCtrlText(edtDenpyouCode, 0, "", True)
            edtDenpyouCode.Format = ""
            edtDenpyouCode.Enabled = False
            edtDenpyouCode.Text = "自動更新"
        Else
            If Denpyou.Tokuisaki.MasterNo = 0 Then
                edtDenpyouCode.Enabled = False
            Else
                edtDenpyouCode.Enabled = True
            End If
            CFormCommon.IniCtrlText(edtDenpyouCode, drJisha("納品伝票コード桁数"), "", True)
            edtDenpyouCode.Text = Denpyou.Code
            edtDenpyouCode.Format = "9"
        End If

        lblRendouSaki.Text = ""
        If Denpyou.RendouSakiSiireCode <> "" Then
            lblRendouSaki.Text = "連動先:"
            lblRendouSaki.Text &= "仕入" & Denpyou.RendouSakiSiireCode
        End If
        If Denpyou.RendouSakiNyukinCode <> "" Then
            If lblRendouSaki.Text = "" Then
                lblRendouSaki.Text = "連動先:"
            Else
                lblRendouSaki.Text &= ", "
            End If
            lblRendouSaki.Text &= "入金" & Denpyou.RendouSakiNyukinCode
        End If

        edtTokuiCode.Text = Denpyou.Tokuisaki.Code
        lblTokuiName.Text = Denpyou.Tokuisaki.Name
        lblTokuiName2.Text = Denpyou.Tokuisaki.Name2
        lblTokuiBikou.Text = Denpyou.Tokuisaki.Bikou
        lblTokuiZeiKubun.Text = GetZeikubunName(Denpyou.Tokuisaki.ZeiKubun)
        If Denpyou.Tokuisaki.ZeiKubun <> enZeikubun.非課税 Then
            lblTokuiZeiKubun.Text &= " / " & GetShouhizeiKeisanName(Denpyou.Tokuisaki.ShouhizeiKeisan)
        End If

        edtNounyuuCode.Text = Denpyou.NounyuuSaki.Code
        edtNounyuuName.Text = Denpyou.NounyuuSaki.Name
        edtNounyuuName2.Text = Denpyou.NounyuuSaki.Name2
        cmbNounyuuKeisho.Text = Denpyou.NounyuuSaki.Keishou
        If Denpyou.Tokuisaki.Shokuchi Then
            '諸口の時、納入先名、敬称は手入力可とする
            edtNounyuuName.Enabled = True
            edtNounyuuName2.Enabled = True
            cmbNounyuuKeisho.Enabled = True
        Else
            edtNounyuuName.Enabled = False
            edtNounyuuName2.Enabled = False
            cmbNounyuuKeisho.Enabled = False

            If Denpyou.Tokuisaki.NounyusakiExist Then
                edtNounyuuCode.TabStop = True  '得意先に納入先がある時
            Else
                edtNounyuuCode.TabStop = False
            End If
            edtNounyuuName.TabStop = False
            edtNounyuuName2.TabStop = False
            cmbNounyuuKeisho.TabStop = False
        End If

        edtSoukoCode.Text = Denpyou.Souko.Code
        lblSoukoName.Text = Denpyou.Souko.Name

        edtTantouCode.Text = Denpyou.Tantousha.Code
        lblTantouName.Text = Denpyou.Tantousha.Name

        lblReferenceCodeTitle.Enabled = True
        lblReferenceCode.Enabled = True
        btnClearReferenceCode.Enabled = True
        If Denpyou.MitumoriNo <> 0 Then
            lblReferenceCodeTitle.Text = "見積コード"
            lblReferenceCode.Text = Denpyou.MitumoriCode
        ElseIf Denpyou.SiireDenpyouNo <> 0 Then
            lblReferenceCodeTitle.Text = "仕入コード"
            lblReferenceCode.Text = Denpyou.SiireCode
        ElseIf Denpyou.JutyuDenpyouNo <> 0 Then
            lblReferenceCodeTitle.Text = "受注コード"
            lblReferenceCode.Text = Denpyou.JutyuCode
        Else
            lblReferenceCodeTitle.Text = "参照コード"
            lblReferenceCodeTitle.Enabled = False
            lblReferenceCode.Enabled = False
            btnClearReferenceCode.Enabled = False
        End If

        datNouhinDate.Value = Denpyou.NouhinDate
        datSeikyuDate.Value = Denpyou.SeikyuDate
        '入力可能日付の範囲をチェックしワーニングメッセージを表示
        CheckNouhinDate()
        CheckSeikyuDate()

        '請求書発行済かどうかの表示
        If Denpyou.Tokuisaki.SeikyuSaki <> 0 AndAlso CheckSeikyuShoHakkou() Then
            lblSeikyuZumi.Visible = True
        Else
            lblSeikyuZumi.Visible = False
        End If

        edtTekiyou.Text = Denpyou.Tekiyou

        If My.Settings.EndUserName = "信和通信工業株式会社" Then
            '*信和*　項目追加
            edtTadasiGaki.Text = Denpyou.TadasiGaki
            chkKariDen.Checked = Denpyou.KariDen
        End If

        'デフォルトの納品伝票フォームをToolStripメニューの印刷フォームコンボにセットする
        CFormCommon.FindComboBoxFormValue(mnuCmbForm, PrintFormFolderMirrorPrincipal(Denpyou.NouhinDenpyou))

        '合計シートの設定
        SetSheetGoukeiSettings()

        '納品明細のセット
        SetSheetPlus(False)  '明細はマイナスも可能とする（この段階ではDataTableにマイナスのレコードがあるため）

        If AfterSerch Then
            MRowSheet.DataSource = Nothing
            MRowSheet.MaxMRows = 0  '（これをしないと、非連結項目の値が残っている）
            MRowSheet.DataSource = dtMeisai  '納品明細をシートに連結
        End If

        '  売上区分コンボボックスの表示（Edit時なぜか"MRowSheet.DataSource=dtMeisai"後でないと、正しい区分がセットされないのでここでセット）
        cmbUriageKubun.SelectedIndex = cmbUriageKubun.FindObject(Denpyou.UriageKubun.MasterNo, -1, GrapeCity.Win.Input.TargetMember.ValueMember)

        '  納品明細に、商品マスタより各種情報をセットし、金額の再計算を行う
        For mRow As Integer = 0 To MRowSheet.MaxMRows - 1
            MRowSheet.MRows(mRow)("商品単価").Note = Nothing
            MRowSheet.MRows(mRow)("原価単価").Note = Nothing
            MRowSheet.MRows(mRow)("消費税率").Note = Nothing
            MRowSheet.MRows(mRow)("軽減税率").Note = Nothing

            If AfterSerch Then  'GetRecord後の時
                MRowSheet.MRows(mRow).ErrorText = ""
                '絶対値でセット
                If Denpyou.UriageKubun.Code <> CON_UriageCode Then  '売上以外の時
                    SetDBSheetPlus(True, mRow)  '絶対値でセット
                End If

                'テーブルの項目からSheetの項目へセット
                If Denpyou.Tokuisaki.ZeiKubun = enZeikubun.外税 Then
                    '得意先が外税の時は、税抜価格を表示
                    MRowSheet.MRows(mRow)("原価単価").Value = MRowSheet.MRows(mRow)("税抜原価単価").Value
                    MRowSheet.MRows(mRow)("商品単価").Value = MRowSheet.MRows(mRow)("税抜商品単価").Value
                    MRowSheet.MRows(mRow)("金額").Value = MRowSheet.MRows(mRow)("税抜金額").Value
                Else
                    '得意先が内税、非課税の時は、税込価格を表示
                    MRowSheet.MRows(mRow)("原価単価").Value = MRowSheet.MRows(mRow)("税込原価単価").Value
                    MRowSheet.MRows(mRow)("商品単価").Value = MRowSheet.MRows(mRow)("税込商品単価").Value
                    MRowSheet.MRows(mRow)("金額").Value = MRowSheet.MRows(mRow)("税込金額").Value
                End If
                MRowSheet.MRows(mRow)("得意先別単価").Value = MRowSheet.MRows(mRow)("商品単価").Value

                '納品明細の単位を単位入力用列にセット（GetRecord後の時、単位コンボの設定が必要）
                MRowSheet.MRows(mRow)("単位IN").Value = Nothing  'クリアしておかないと、単位が""の時なぜかクリアされない
                MRowSheet.MRows(mRow)("単位IN").Text = MRowSheet.MRows(mRow)("単位").Text

                '受注伝票より、明細の受注コードを得る
                If My.Settings.HanbaiKanriType = "C" Then
                    If My.Settings.EndUserName = "信和通信工業株式会社" Then
                        '*信和*　Cタイプでも受注は使用しない
                    Else
                        MRowSheet.MRows(mRow)("受注コード").Value = ""
                        MRowSheet.MRows(mRow)("受注コード").ErrorText = ""
                        If MRowSheet.MRows(mRow)("受注明細No").Value > 0 Then
                            Dim sSQL As String =
                                "SELECT 受注伝票.コード " _
                                & "FROM 受注明細 INNER JOIN 受注伝票 ON 受注明細.受注伝票No = 受注伝票.テーブルNo " _
                                & "WHERE 受注明細.テーブルNo = " & MRowSheet.MRows(mRow)("受注明細No").Value
                            Dim dCode As String = CDBCommon.SQLExecuteScalar(CSingleton.CSetting.Connect, sSQL) '単一データを取得
                            If dCode Is Nothing Then
                                MRowSheet.MRows(mRow)("受注コード").ErrorIcon = My.Resources.StatusInvalid_16x
                                MRowSheet.MRows(mRow)("受注コード").ErrorText = "該当の受注明細がありません"
                            Else
                                MRowSheet.MRows(mRow)("受注コード").Text = dCode
                            End If
                        End If
                    End If
                End If
            End If

            '  商品マスタNoをキーに、商品マスタのレコードを得る
            Dim drShouhin As DataRow = Nothing
            If GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value) > 0 Then
                Dim CShouhin As New HanbaikanriDialog.CShouhin()
                drShouhin = CShouhin.GetMaster(GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value))
            End If
            If drShouhin IsNot Nothing Then  '商品マスタあり
                If AfterSerch Then  'GetRecord後の時
                    MRowSheet.MRows(mRow)("在庫管理有効").Value = drShouhin("在庫管理有効")
                    MRowSheet.MRows(mRow)("消費税率区分").Value = drShouhin("消費税率区分")

                    '伝票検索時、初回表示時のみ在庫数のチェックを行わない(Falseをセット）
                    MRowSheet.MRows(mRow)("在庫数チェック").Value = ZaikoCheck

                    '在庫数のセットとチェック
                    CheckZaiko(mRow, True)

                    MRowSheet.MRows(mRow)("在庫数チェック").Value = True  '次回からは在庫チェックは必ず行う

                    '新規の時（複写、コピー、XX参照等）、商品コードを最新のマスタから取得する（既存データの時はそのまま。既存データでも商品コード無しの時はセットする。）
                    If Denpyou.TableNo <= 0 OrElse MRowSheet.MRows(mRow)("商品コード").Text.Trim = "" Then
                        If MRowSheet.MRows(mRow)("商品コード").Text <> drShouhin("コード").ToString Then
                            MRowSheet.MRows(mRow)("商品コード").Value = drShouhin("コード").ToString
                        End If
                    End If
                End If

                '仕入伝票参照時、単価をマスタからセット（数量/金額はゼロ）
                If GetSiire Then
                    '得意先別単価、商品単価セット
                    SetTanka(mRow, drShouhin)
                End If

            Else   '商品マスタなし
                MRowSheet.MRows(mRow).ErrorText = ""
                MRowSheet.MRows(mRow)("在庫管理有効").Value = False
                MRowSheet.MRows(mRow)("消費税率区分").Value = 0

                If AfterSerch Then  'GetRecord後の時
                    '新規の時（複写、コピー、XX参照等）、商品マスタなしなのに商品がセットされていたらクリア
                    If Denpyou.TableNo <= 0 Then
                        If GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value) > 0 Then
                            MRowSheet.MRows(mRow)("商品マスタNo").Value = 0
                        End If
                        If MRowSheet.MRows(mRow)("商品コード").Text <> "" Then
                            MRowSheet.MRows(mRow)("商品コード").Value = ""
                        End If
                    End If
                End If

                '仕入伝票参照時、単価なしでセット（数量/金額はゼロ）
                If GetSiire Then
                    MRowSheet.MRows(mRow)("商品単価").Value = 0
                End If
            End If

            If TypeOf MRowSheet.MRows(mRow)("消費税率").Editor IsNot GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor Then
                Dim orgTaxRate As Decimal = MRowSheet.MRows(mRow)("消費税率").Value  'コンボボックスを設定すると値が消えてしまうため保存
                SetComboItemTaxRate(mRow)  '消費税率コンボボックスの設定

                '既存行で消費税マスタにない消費税率の時、消費税コンボに追加する
                If orgTaxRate <> 0 AndAlso Array.IndexOf(Denpyou.aryRate, orgTaxRate) < 0 Then
                    Dim cmbEditor As GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor = MRowSheet.MRows(mRow)("消費税率").Editor
                    cmbEditor.Items.Add(New GrapeCity.Win.ElTabelle.Editors.ComboItem(0, Nothing, orgTaxRate.ToString("#0%").PadLeft(3), "", orgTaxRate))
                End If
                MRowSheet.MRows(mRow)("消費税率").Value = orgTaxRate  '保存した値を元に戻す
            End If

            If AfterSerch Then  'GetRecord後の時
                'デフォルト商品を付加（金額/数量は入力されているが、商品がセットされていない行に、デフォルト商品をセットする）
                SetDefaultShouhin(mRow)
            End If

            '明細金額の再計算（税区分等の変更に対応）
            If ReCalc Then
                ReCalcMeisai(mRow, drShouhin, oldSeikyuMasterNo)  '明細行の再計算
            End If

            'ワーニング表示（エラーアイコンでは表示が隠れてしまうため、セルノートを使用）
            If (Denpyou.Tokuisaki.ZeiKubun = enZeikubun.非課税 OrElse MRowSheet.MRows(mRow)("商品税区分").Value = enZeikubun.非課税) AndAlso
              MRowSheet.MRows(mRow)("消費税率").Value <> 0 Then
                CFormCommon.SetSelNote(MRowSheet.MRows(mRow)("消費税率"), "非課税の得意先/商品は、消費税率を[0%]にしてください")
            End If
            If MRowSheet.MRows(mRow)("軽減税率").Value AndAlso MRowSheet.MRows(mRow)("消費税率区分").Value <> enTaxRateKubun.税率2 Then
                CFormCommon.SetSelNote(MRowSheet.MRows(mRow)("軽減税率"), "「税率2」ではない商品に、軽減税率のチェックが付いています")
            End If

            '入力可/不可項目の設定
            If MRowSheet.MRows(mRow)("商品単価").Value = 0 Then
                MRowSheet.MRows(mRow)("金額").Lock = False
            Else
                '単価が入力されている時、金額は入力不可
                MRowSheet.MRows(mRow)("金額").Lock = True
            End If
        Next

        '  明細行が納品伝票項目数より少ない時、納品伝票項目数分の空明細行を追加
        InsertMrowNullMeisai()

        '合計金額の表示
        If AfterSerch Then
            'GetRecord後の時は、総合計はデータから得るので再計算しない。税別計は計算してセット。
            SetGoukei(False)
        Else
            SetGoukei()
        End If

        If Denpyou.UriageKubun.Code = CON_UriageCode Then  '売上の時
            SetSheetPlus(False)  '明細入力はマイナスも可能とする
        Else
            SetSheetPlus(True)  '明細入力は、プラスのみ可能とする
        End If

        If MRowSheet.MaxMRows > 0 Then
            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(0, enSheetCol1.テーブルNo, enSheetRow.Row1) 'アクティブセルを先頭にセット（但しシートのカーソルを表示させない）
            SheetRedrawOFF()  '（LeaveCellが動くとActivePositionがONになってしまう）
        End If

        isChanged = False
        ChangeButton(isNewInputtable, False)

        isChangedTokuiCode = False
        isChangedNounyuuCode = False
        isChangedSoukoCode = False
        isChangedTantouCode = False
        isChangedNouhinDate = False
        isChangedSeikyuDate = False
        isChangedUriageKubun = False

        oldKakeritu = Denpyou.Tokuisaki.Kakeritu
        oldZeikubun = Denpyou.Tokuisaki.ZeiKubun
        oldHasuu = Denpyou.Tokuisaki.Hasuu
        oldSeikyuMasterNo = Denpyou.Tokuisaki.SeikyuSaki
    End Sub

    '合計シートの設定
    Private Sub SetSheetGoukeiSettings()
        If Denpyou.Tokuisaki.ZeiKubun = enZeikubun.非課税 Then
            '非課税の時、税抜額/消費税額/参考消費税は表示しない
            sheetGoukei.Columns(enSheetGoukeiCol.税抜額).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.税抜額 - 1).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額 - 1).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.参考消費税).Hidden = True
            sheetGoukei.Columns(enSheetGoukeiCol.参考消費税 - 1).Hidden = True
            sheetGoukei.Columns(enSheetGoukeiCol.ダミー列).Hidden = True

            sheetGoukei.Columns(enSheetGoukeiCol.税抜額).Enabled = False
            sheetGoukei.Columns(enSheetGoukeiCol.税抜額 - 1).Enabled = False
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額).Enabled = False
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額 - 1).Enabled = False

        ElseIf Denpyou.Tokuisaki.ShouhizeiKeisan = enZeiKeisan.請求時 Then
            '請求時、税抜額/消費税額は表示せず、参考消費税を表示
            sheetGoukei.Columns(enSheetGoukeiCol.税抜額).Hidden = True
            sheetGoukei.Columns(enSheetGoukeiCol.税抜額 - 1).Hidden = True
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額).Hidden = True
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額 - 1).Hidden = True
            sheetGoukei.Columns(enSheetGoukeiCol.参考消費税).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.参考消費税 - 1).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.ダミー列).Hidden = False

        Else
            '取引時/明細毎、参考消費税は表示しない
            sheetGoukei.Columns(enSheetGoukeiCol.税抜額).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.税抜額 - 1).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額 - 1).Hidden = False
            sheetGoukei.Columns(enSheetGoukeiCol.参考消費税).Hidden = True
            sheetGoukei.Columns(enSheetGoukeiCol.参考消費税 - 1).Hidden = True
            sheetGoukei.Columns(enSheetGoukeiCol.ダミー列).Hidden = True

            sheetGoukei.Columns(enSheetGoukeiCol.税抜額).Enabled = True
            sheetGoukei.Columns(enSheetGoukeiCol.税抜額 - 1).Enabled = True
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額).Enabled = True
            sheetGoukei.Columns(enSheetGoukeiCol.消費税額 - 1).Enabled = True
        End If
    End Sub

    '納品伝票の伝票コードを採番して返す
    Private Function GetDenpyouCode(ByVal masterNo As Integer) As Decimal
        Dim sSQL As String
        If masterNo > 0 AndAlso Denpyou.Tokuisaki.DenpyouCodeFlag Then
            '伝票コードを個別にする時
            sSQL = "SELECT TOP 1 納品伝票.テーブルNo, 納品伝票.[コード], 納品伝票.[修正元テーブルNo] " _
                 & "FROM (得意先マスタ INNER JOIN 納品伝票 ON 得意先マスタ.マスタNo = 納品伝票.得意先マスタNo) INNER JOIN 得意先マスタ AS 請求先 ON 得意先マスタ.請求先マスタNo = 請求先.マスタNo " _
                 & "WHERE 請求先.伝票コードフラグ = 1 AND 得意先マスタ.請求先マスタNo=" & masterNo & " AND 納品伝票.削除 = 0 " _
                 & "AND 納品伝票.テーブルNo < @テーブルNo " _
                 & "ORDER BY テーブルNo DESC"
        Else
            '伝票コードを個別にしない時
            sSQL = "SELECT TOP 1 納品伝票.テーブルNo, 納品伝票.[コード], 納品伝票.[修正元テーブルNo] " _
                 & "FROM (得意先マスタ INNER JOIN 納品伝票 ON 得意先マスタ.マスタNo = 納品伝票.得意先マスタNo) INNER JOIN 得意先マスタ AS 請求先 ON 得意先マスタ.請求先マスタNo = 請求先.マスタNo " _
                 & "WHERE 請求先.伝票コードフラグ <> 1 AND 納品伝票.削除 = 0 " _
                 & "AND 納品伝票.テーブルNo < @テーブルNo " _
                 & "ORDER BY テーブルNo DESC"
        End If

        '伝票コード採番
        Return CDenpyouCommon.GetDenpyouCode(sSQL, drJisha("納品伝票コード桁数"))
    End Function

    '伝票コードが自動更新でない時、伝票コードの重複チェックを行う
    Private Function CheckDenpyouCode(Optional ByVal whenUpdateRec As Boolean = False, Optional ByRef connection As SqlConnection = Nothing) As Boolean
        '伝票コード自動更新なら重複チェックしない
        If Denpyou.Tokuisaki.DenpyouCodeUpdate Then
            Return True
        End If

        Dim sSQL As String
        If Denpyou.Tokuisaki.DenpyouCodeFlag Then
            '伝票コードを個別にする時
            sSQL = "SELECT 納品伝票.コード " _
                 & "FROM 納品伝票 INNER JOIN 得意先マスタ ON 納品伝票.得意先マスタNo = 得意先マスタ.マスタNo " _
                 & "WHERE 納品伝票.コード = '" & Denpyou.Code & "' " _
                 & "AND 得意先マスタ.請求先マスタNo = " & Denpyou.Tokuisaki.SeikyuSaki & " " _
                 & "AND 納品伝票.削除 = 0"
        Else
            '伝票コードを個別にしない時
            sSQL = "SELECT 納品伝票.コード " _
                 & "FROM 納品伝票 INNER JOIN (得意先マスタ INNER JOIN 得意先マスタ AS 請求先 ON 得意先マスタ.請求先マスタNo = 請求先.マスタNo) ON 納品伝票.得意先マスタNo = 得意先マスタ.マスタNo " _
                 & "WHERE 納品伝票.コード = '" & Denpyou.Code & "' " _
                 & "AND 請求先.伝票コードフラグ = 0 " _
                 & "AND 納品伝票.削除 = 0"
        End If
        If Denpyou.TableNo > 0 Then
            sSQL &= " AND テーブルNo <> " & Denpyou.TableNo
        End If

        '伝票コードの重複チェックを行い、重複ならFalseを返す
        Return CDenpyouCommon.CheckDenpyouCodeDuplicate(connection, sSQL, edtDenpyouCode, whenUpdateRec)
    End Function

    '参照元の伝票コードを得る
    Private Function GetReferenceCode(ByVal tableName As String, ByVal tableNo As Integer) As String
        Dim sSQL As String =
            "SELECT コード FROM " & tableName & " WHERE テーブルNo = " & tableNo
        Dim denpyouCode As String = CDBCommon.SQLExecuteScalar(CSingleton.CSetting.Connect, sSQL) '単一データを取得
        If denpyouCode Is Nothing Then
            MessageBox.Show("検索条件に該当する納品伝票の、参照元" & tableName & "が見つかりません。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return ""  'データなし
        Else
            Return denpyouCode
        End If
    End Function

    '納品伝票項目数分の空の明細行を追加
    Private Sub InsertMrowNullMeisai()
        If MRowSheet.MaxMRows < Denpyou.KoumokuSu Then
            '納品伝票項目数分の空行を追加
            For i As Integer = MRowSheet.MaxMRows To Denpyou.KoumokuSu - 1
                InsertMrowNullMeisai(i)
            Next

        ElseIf MRowSheet.MaxMRows > Denpyou.KoumokuSu Then
            '納品伝票項目数より明細行が多い時、未入力行なら削除する
            Dim i As Integer = Denpyou.KoumokuSu
            Do While i < MRowSheet.MaxMRows
                If DataInSheet(i, MRowSheet) = False Then
                    '未入力行を削除
                    MRowSheet.RemoveMRow(i, False)
                Else
                    i += 1
                End If
            Loop
        End If
    End Sub
    '納品明細を１行追加し、初期値設定を行う
    Private Sub InsertMrowNullMeisai(ByVal mRow As Integer)
        MRowSheet.InsertMRow(mRow, False)

        MRowSheet.MRows(mRow)("納品伝票No").Value = dtDenpyou.Rows(0)("テーブルNo")
        If My.Settings.HanbaiKanriType = "C" Then
            MRowSheet.MRows(mRow)("受注コード").Value = ""
        End If
        MRowSheet.MRows(mRow)("商品コード").Value = ""
        MRowSheet.MRows(mRow)("商品名称カナ").Value = ""
        MRowSheet.MRows(mRow)("商品名称").Value = ""
        SetComboItemTaxRate(mRow)  '消費税率コンボボックスの設定
        MRowSheet.MRows(mRow)("消費税率").Value = CDec(0)
        MRowSheet.MRows(mRow)("軽減税率").Value = False
        MRowSheet.MRows(mRow)("入数").Value = 0
        MRowSheet.MRows(mRow)("セット数").Value = 0
        MRowSheet.MRows(mRow)("数量").Value = 0
        MRowSheet.MRows(mRow)("単位IN").Value = Nothing
        MRowSheet.MRows(mRow)("原価単価").Value = 0
        MRowSheet.MRows(mRow)("商品単価").Value = 0
        MRowSheet.MRows(mRow)("金額").Value = 0
        MRowSheet.MRows(mRow)("備考").Value = ""

        MRowSheet.MRows(mRow)("単位").Value = ""
        MRowSheet.MRows(mRow)("税抜原価単価").Value = 0
        MRowSheet.MRows(mRow)("税込原価単価").Value = 0
        MRowSheet.MRows(mRow)("税抜商品単価").Value = 0
        MRowSheet.MRows(mRow)("税込商品単価").Value = 0
        MRowSheet.MRows(mRow)("税抜金額").Value = 0
        MRowSheet.MRows(mRow)("税込金額").Value = 0
        MRowSheet.MRows(mRow)("税抜原価").Value = 0
        MRowSheet.MRows(mRow)("消費税").Value = 0
        MRowSheet.MRows(mRow)("商品税区分").Value = 0
        MRowSheet.MRows(mRow)("消費税率区分").Value = 0
        MRowSheet.MRows(mRow)("在庫管理有効").Value = False
        MRowSheet.MRows(mRow)("在庫数チェック").Value = True
        MRowSheet.MRows(mRow)("在庫数").Value = 0
        MRowSheet.MRows(mRow)("最小在庫数").Value = 0
        MRowSheet.MRows(mRow)("得意先別単価").Value = 0
        If My.Settings.HanbaiKanriType = "C" Then
            MRowSheet.MRows(mRow)("受注明細No").Value = 0
        End If

        SetSheetPlus(True, mRow)  '明細入力は、プラスのみ可能とする
    End Sub

    '得意先別単価の登録（請求先に登録）
    Private Sub RegistTanka(ByVal mRow As Integer)
        Try
            Using cnTanka As New SqlConnection(CSingleton.CSetting.Connect)
                cnTanka.Open()
                Using trTanka As SqlTransaction = cnTanka.BeginTransaction(IsolationLevel.Serializable)
                    Dim sSQL As String
                    sSQL = "SELECT 得意先別単価マスタ.* " _
                         & "FROM 得意先別単価マスタ " _
                         & "WHERE 得意先別単価マスタ.得意先マスタNo = @得意先マスタNo AND 得意先別単価マスタ.商品マスタNo = @商品マスタNo"
                    Dim daTanka As New SqlDataAdapter(sSQL, cnTanka)
                    daTanka.SelectCommand.Parameters.Add("@得意先マスタNo", SqlDbType.Int)
                    daTanka.SelectCommand.Parameters("@得意先マスタNo").Value = Denpyou.Tokuisaki.SeikyuSaki
                    daTanka.SelectCommand.Parameters.Add("@商品マスタNo", SqlDbType.Int)
                    daTanka.SelectCommand.Parameters("@商品マスタNo").Value = MRowSheet.MRows(mRow)("商品マスタNo").Value
                    CDBCommon.MakeEditCommands(daTanka, "マスタNo", , , trTanka)
                    Dim dtTanka As New DataTable
                    daTanka.Fill(dtTanka)

                    If dtTanka.Rows.Count = 0 Then '該当の得意先別単価がない時、追加
                        dtTanka.Rows.Add()
                    End If

                    dtTanka.Rows(0)("得意先マスタNo") = Denpyou.Tokuisaki.SeikyuSaki
                    dtTanka.Rows(0)("商品マスタNo") = MRowSheet.MRows(mRow)("商品マスタNo").Value
                    dtTanka.Rows(0)("単価") = MRowSheet.MRows(mRow)("商品単価").Value
                    dtTanka.Rows(0)("更新日時") = Date.Now
                    daTanka.Update(dtTanka)

                    trTanka.Commit()
                End Using
            End Using

        Catch ex As Exception
            ErrProc(ex, Me.Text)
            Throw
        End Try
    End Sub

    '得意先検索
    Private Function FindTokuisaki(ByVal isButton As Boolean) As Boolean
        Dim selectedCode As String
        If isButton Then  '検索ボタン押下時
            'マスタ一覧の表示
            selectedCode = CFormCommon.FindMasterList("得意先", drJisha("得意先コード桁数"), "", "")
        Else  'コード入力時
            '入力されたコードorカナに合致するマスタ一覧からコードを得る（１件しかない時は、一覧を表示せずそのデータを得る）
            selectedCode = CFormCommon.FindMasterText(edtTokuiCode.Text, "得意先", drJisha("得意先コード桁数"), drJisha("得意先コード入力方法"), False, True)
        End If

        If selectedCode IsNot Nothing Then  '得意先を決定した時
            ChangeTokuiCode(True, CFormCommon.MasterDataRow)  '選択した得意先の内容からDenpyouの内容を置き換える

            '指定伝票かどうかを判定
            If CNouhin.CNouhinInstance.ChkSiteiDenpyou(Denpyou.NouhinDenpyou) Then
                '指定伝票なら、納品伝票を終了し指定伝票画面へ
                isEnd = True  '終了
                CNouhin.CNouhinInstance.TokuisakiNo = Denpyou.Tokuisaki.MasterNo
                CNouhin.CNouhinInstance.TableNo = 0
                CNouhin.CNouhinInstance.Reference = False
                CNouhin.CNouhinInstance.JutyuNo = 0
                CNouhin.CNouhinInstance.MitumoriNo = 0
                Me.DialogResult = DialogResult.OK  '結果を渡す
                Me.Close()
                Return False
            End If

            MRowSheet.Enabled = True
            Dim isChangedOriginal As Boolean = isChanged  'SetFormでisChangedがFalseになってしまうので、SetForm後元に戻すためHold
            SetForm(False, True)
            isChanged = isChangedOriginal
            If isChanged OrElse Denpyou.TableNo <> 0 Then  '既に変更済or修正の時（他の変更がない時は登録ボタンを使用可にしないため）
                UpdateFlagOn()
            End If

            Return True
        Else
            Return False
        End If
    End Function

    '納入先検索
    Private Function FindNounyusaki(ByVal isButton As Boolean) As Boolean
        Dim selectedCode As String
        If isButton Then  '検索ボタン押下時
            'マスタ一覧の表示
            selectedCode = CFormCommon.FindMasterList("納入先", drJisha("納入先コード桁数"), "", "",,, Denpyou.Tokuisaki.MasterNo)
        Else  'コード入力時
            '入力されたコードorカナに合致するマスタ一覧からコードを得る（１件しかない時は、一覧を表示せずそのデータを得る）
            selectedCode = CFormCommon.FindMasterText(edtNounyuuCode.Text, "納入先", drJisha("納入先コード桁数"), drJisha("納入先コード入力方法"), False, True, Denpyou.Tokuisaki.MasterNo)
        End If

        If selectedCode IsNot Nothing Then  '納入先を決定した時
            ChangeNounyuuCode(True, CFormCommon.MasterDataRow)  '納入先の内容からDenpyouの内容を置き換える
            Return True
        Else
            Return False
        End If
    End Function

    '倉庫検索
    Private Function FindSouko(ByVal isButton As Boolean) As Boolean
        Dim selectedCode As String
        If isButton Then  '検索ボタン押下時
            'マスタ一覧の表示
            selectedCode = CFormCommon.FindMasterList("倉庫", drJisha("倉庫コード桁数"), "", "")
        Else  'コード入力時
            '入力されたコードorカナに合致するマスタ一覧からコードを得る（１件しかない時は、一覧を表示せずそのデータを得る）
            selectedCode = CFormCommon.FindMasterText(edtSoukoCode.Text, "倉庫", drJisha("倉庫コード桁数"), drJisha("倉庫コード入力方法"), False, True)
        End If

        If selectedCode IsNot Nothing Then  '倉庫を決定した時
            ChangeSoukoCode(True, CFormCommon.MasterDataRow)  '倉庫の内容からDenpyouの内容を置き換える
            Return True
        Else
            Return False
        End If
    End Function

    '担当者検索
    Private Function FindTantou(ByVal isButton As Boolean) As Boolean
        Dim selectedCode As String
        If isButton Then  '検索ボタン押下時
            'マスタ一覧の表示
            selectedCode = CFormCommon.FindMasterList("担当者", drJisha("担当者コード桁数"), "", "")
        Else  'コード入力時
            '入力されたコードorカナに合致するマスタ一覧からコードを得る（１件しかない時は、一覧を表示せずそのデータを得る）
            selectedCode = CFormCommon.FindMasterText(edtTantouCode.Text, "担当者", drJisha("担当者コード桁数"), drJisha("担当者コード入力方法"), False, True)
        End If

        If selectedCode IsNot Nothing Then  '担当者を決定した時
            ChangeTantouCode(True, CFormCommon.MasterDataRow)  '担当者の内容からDenpyouの内容を置き換える
            Return True
        Else
            Return False
        End If
    End Function

    '商品検索
    '  検索ボタンの時は、Code,NameKanaは""
    Private Function FindShouhin(ByVal code As String, ByVal nameKana As String, ByVal mRow As Integer) As Boolean
        'マスタ一覧の表示
        '入力されたコードorカナに合致するマスタ一覧からコードを得る（１件しかない時は、一覧を表示せずそのデータを得る）
        Dim selectedCode As String
        selectedCode = CFormCommon.FindMasterList("商品", drJisha("商品コード桁数"), code, nameKana, enHanbaiKubun.販売用, "SaleOrBuy", Denpyou.Souko.MasterNo, Denpyou.NouhinDate)  '販売区分=販売用(1)で絞り込む

        If selectedCode IsNot Nothing Then  '商品を決定した時
            MRowSheet.MRows(mRow)("商品コード").Value = CFormCommon.MasterDataRow("コード").ToString
            ChangeShouhin(True, mRow, CFormCommon.MasterDataRow)  '選択された商品の内容からSheetの内容を置き換える
            UpdateFlagOn()

            'カーソル移動（セルの移動はFindShouhinで。CanActivateの設定はLeaveCellで。商品検索ボタンの時はCanActivateを設定しないようにするため。）
            If My.Settings.EndUserName = "株式会社　日の出" Then
                '*日の出*  商品後、商品名称にカーソル移動させる
                MRowSheet.ActiveCellKey = "商品名称"  'セルの移動
            ElseIf My.Settings.EndUserName = "信和通信工業株式会社" AndAlso MRowSheet.MRows(mRow)("商品名称").Text.Trim = "" Then
                '*信和*  商品名称未設定の商品時、商品名称にカーソル移動
                MRowSheet.ActiveCellKey = "商品名称"  'セルの移動
                'ElseIf My.Settings.EndUserName = "有限会社山田商店" Then
                '    '*山田商店*　商品後、入数にカーソル移動
                '    MRowSheet.ActiveCellKey = "入数"  'セルの移動
            Else
                If MRowSheet.MRows(mRow)("入数").Value = 0 Then
                    MRowSheet.ActiveCellKey = "数量"  'セルの移動
                Else
                    MRowSheet.ActiveCellKey = "セット数"  'セルの移動
                End If
            End If
            Return True
        Else
            Return False
        End If
    End Function

    '受注明細検索
    '  検索ボタンの時は、Codeは""
    Private Function FindJutyuMeisai(ByVal Code As String, ByVal mRow As Integer) As Boolean
        If Denpyou.Tokuisaki.MasterNo = 0 Then
            MessageBox.Show("先に得意先を入力して下さい。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            edtTokuiCode.Select()
            Return False
        End If
        Try
            'ほかの検索画面が開いていたら閉じる
            CloseFormListIfOpend(frmJutyuListMeisai)

            '受注伝票の検索画面表示
            If Code = "" Then
                '検索ボタン押下時（モードレス表示）
                If frmJutyuListMeisai Is Nothing OrElse frmJutyuListMeisai.IsDisposed Then
                    '伝票検索画面が表示されていない時
                    frmJutyuListMeisai = New frmJutyuList("受注明細参照", AddressOf Me.JutyuListMeisaiCallBack)  '伝票検索画面から動かしたいアドレスを渡す
                    frmJutyuListMeisai.SearchTokuiCode = edtTokuiCode.Text
                    frmJutyuListMeisai.SearchTantouCode = edtTantouCode.Text
                    frmJutyuListMeisai.MeisaiOKbtn = True  '明細一覧の「決定」ボタンを表示
                    frmJutyuListMeisai.SearchDenpyouCode = Code  '伝票コードを指定すれば、直接受注明細一覧を表示
                    If frmJutyuListMeisai.BeforeLoad() Then
                        '一覧画面表示（エラー発生時は一覧画面を表示しない）
                        frmJutyuListMeisai.Opener = Me
                        frmJutyuListMeisai.Show()
                    End If
                Else
                    '伝票検索画面が表示済の時
                    If frmJutyuListMeisai.WindowState = FormWindowState.Minimized Then
                        frmJutyuListMeisai.WindowState = FormWindowState.Normal  '最小化されていた場合、通常に戻す
                    End If
                    frmJutyuListMeisai.BringToFront()  '最前面に表示する
                End If

                Return True
            Else
                'コード直接入力時（モーダル表示）
                Using frmJutyuList As New frmJutyuList("受注明細参照")
                    frmJutyuList.SearchTokuiCode = edtTokuiCode.Text
                    frmJutyuList.SearchTantouCode = edtTantouCode.Text
                    frmJutyuList.MeisaiOKbtn = True  '明細一覧の「決定」ボタンを表示
                    frmJutyuList.SearchDenpyouCode = Code  '伝票コードを指定すれば、直接受注明細一覧を表示
                    If frmJutyuList.BeforeLoad() Then
                        '一覧画面表示（エラー発生時は一覧画面を表示しない）
                        frmJutyuList.ShowDialog()
                        SetCursorWait()
                    End If

                    If frmJutyuList.SelectedTableNo > 0 Then
                        '検索結果を表示
                        If GetRecordJutyuMeisai(mRow, frmJutyuList.SelectedTableNo) Then
                            If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
                                WhenChangeRate()  '消費税率変更時、金額を再計算
                            End If
                            UpdateFlagOn()
                            'カーソル移動
                            If MRowSheet.MRows(mRow)("入数").Value = 0 Then
                                MRowSheet.ActiveCellKey = "数量"    'セルの移動
                                MRowSheet.MRows(mRow)("入数").CanActivate = False
                                MRowSheet.MRows(mRow)("セット数").CanActivate = False
                            Else
                                MRowSheet.ActiveCellKey = "セット数"    'セルの移動
                                MRowSheet.MRows(mRow)("入数").CanActivate = False
                            End If
                            MRowSheet.MRows(mRow)("商品コード").CanActivate = False
                            Return True
                        End If
                    End If

                    Return False
                End Using
            End If

        Catch ex As Exception
            ErrProc(ex, Me.Text)
        End Try
    End Function
    '伝票検索のモードレスウインドウから選択したテーブルNoがコールバックされる
    Private Sub JutyuListMeisaiCallBack(ByVal SelectedTableNo As Integer)
        If SelectedTableNo > 0 AndAlso MRowSheet.ActivePosition.MRow >= 0 Then
            '検索結果を表示
            SetCursorWait()
            SheetRedrawOFF()
            MRowSheet.ActivePosition = New GrapeCity.Win.ElTabelle.MPosition(MRowSheet.ActivePosition.MRow, enSheetCol2.受注明細検索ボタン, enSheetRow.Row2)  'カーソル位置を他に移動させておかないと、カーソル位置の値が更新されない
            If GetRecordJutyuMeisai(MRowSheet.ActivePosition.MRow, SelectedTableNo) Then
                If DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
                    WhenChangeRate()    '消費税率変更時、金額を再計算
                End If
                UpdateFlagOn()
                'カーソル移動
                If MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("入数").Value = 0 Then
                    MRowSheet.ActiveCellKey = "数量"    'セルの移動
                    MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("入数").CanActivate = False
                    MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("セット数").CanActivate = False
                Else
                    MRowSheet.ActiveCellKey = "セット数"    'セルの移動
                    MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("入数").CanActivate = False
                End If
                MRowSheet.MRows(MRowSheet.ActivePosition.MRow)("商品コード").CanActivate = False
                Me.BringToFront()  '最前面に表示する
            End If
            SheetRedrawON()
            SetCursorDefault()
        End If
    End Sub

    '得意先コードを変更した時、得意先マスタの内容からDenpyouの内容を置き換える（この後SetForm）
    Private Sub ChangeTokuiCode(ByVal Exist As Boolean, ByVal drTokui As DataRow)
        If Exist Then
            '得意先が存在する時
            Denpyou.SiireDenpyouNo = 0    '仕入伝票参照も無しとする（原価単価の端数が狂ってしまうことがあるため）

            Denpyou.Tokuisaki.MasterNo = drTokui("マスタNo")
            Denpyou.Tokuisaki.Code = drTokui("コード").ToString
            Denpyou.Tokuisaki.Name = drTokui("名称").ToString
            Denpyou.Tokuisaki.Name2 = drTokui("名称2").ToString
            Denpyou.Tokuisaki.Keishou = drTokui("敬称").ToString
            Denpyou.Tokuisaki.NameKana = drTokui("名称カナ").ToString
            Denpyou.Tokuisaki.Bikou = drTokui("備考").ToString
            Denpyou.Tokuisaki.Shokuchi = GetBoolean(drTokui("諸口フラグ"))
            Dim CTokuisaki As New HanbaikanriDialog.CTokuisaki()
            Denpyou.Tokuisaki.NounyusakiExist = CTokuisaki.NounyusakiExist(drTokui("マスタNo"))

            If GetBoolean(drTokui("請求先フラグ")) Then
                '他の得意先に請求する時
                '  請求先マスタNoをキーに、請求先（得意先マスタ）のレコードを得る
                Dim drSeikyu As DataRow = CTokuisaki.GetMaster(drTokui("請求先マスタNo"))
                If drSeikyu IsNot Nothing Then
                    Denpyou.Tokuisaki.SeikyuSaki = drSeikyu("マスタNo")
                    Denpyou.Tokuisaki.Hasuu = GetShort(drSeikyu("端数"))
                    Denpyou.Tokuisaki.Kakeritu = GetDecimal(drSeikyu("掛率"))
                    Denpyou.Tokuisaki.ZeiKubun = GetByte(drSeikyu("税区分"))
                    Denpyou.Tokuisaki.ShouhizeiKeisan = GetShort(drSeikyu("消費税計算方法"))
                    Denpyou.Tokuisaki.Hyoujun = GetBoolean(drSeikyu("標準ﾌｫｰﾑ"))
                    Denpyou.Tokuisaki.DenpyouCodeFlag = GetBoolean(drSeikyu("伝票コードフラグ"))
                    Denpyou.Tokuisaki.Simebi = GetShort(drSeikyu("締日"))
                    Denpyou.Tokuisaki.YosinGendo = GetDecimal(drSeikyu("与信限度額"))

                    If Not Denpyou.Tokuisaki.Hyoujun Then
                        '標準フォームを使用しない時
                        If Denpyou.Tokuisaki.DenpyouCodeFlag Then
                            '伝票コードを個別に設定する時
                            Denpyou.NewCode = GetDenpyouCode(Denpyou.Tokuisaki.SeikyuSaki)
                            Denpyou.Tokuisaki.DenpyouCodeUpdate = GetBoolean(drSeikyu("納品伝票コード自動更新"))
                        Else
                            '伝票コードを個別に設定しない時
                            Denpyou.NewCode = GetDenpyouCode(0)
                            Denpyou.Tokuisaki.DenpyouCodeUpdate = GetBoolean(drJisha("納品伝票コード自動更新"))
                        End If

                        Denpyou.NouhinDenpyou = drSeikyu("納品伝票フォーム").ToString
                        Denpyou.KoumokuSu = GetInt(drSeikyu("納品伝票項目数"))

                    Else
                        '標準フォームを使用する時
                        Denpyou.NewCode = GetDenpyouCode(0)
                        Denpyou.Tokuisaki.DenpyouCodeUpdate = drJisha("納品伝票コード自動更新")
                        Denpyou.NouhinDenpyou = drJisha("納品伝票ﾌｫｰﾑ")
                        Denpyou.KoumokuSu = drJisha("納品伝票項目数")
                    End If
                    If Denpyou.KoumokuSu = 0 Then
                        Denpyou.KoumokuSu = MaxKoumoku
                    End If
                Else
                    MessageBox.Show("得意先マスタの設定異常です。請求先がありません。" & vbCrLf & "得意先マスタを確認して下さい。", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Denpyou.Tokuisaki.SeikyuSaki = 0
                End If

            Else
                '他の得意先に請求しない時
                Denpyou.Tokuisaki.SeikyuSaki = drTokui("マスタNo")
                Denpyou.Tokuisaki.Hasuu = GetShort(drTokui("端数"))
                Denpyou.Tokuisaki.Kakeritu = GetDecimal(drTokui("掛率"))
                Denpyou.Tokuisaki.ZeiKubun = GetByte(drTokui("税区分"))
                Denpyou.Tokuisaki.ShouhizeiKeisan = GetShort(drTokui("消費税計算方法"))
                Denpyou.Tokuisaki.Hyoujun = GetBoolean(drTokui("標準ﾌｫｰﾑ"))
                Denpyou.Tokuisaki.DenpyouCodeFlag = GetBoolean(drTokui("伝票コードフラグ"))
                Denpyou.Tokuisaki.Simebi = GetShort(drTokui("締日"))
                Denpyou.Tokuisaki.YosinGendo = GetDecimal(drTokui("与信限度額"))

                If Not Denpyou.Tokuisaki.Hyoujun Then
                    '標準フォームを使用しない時
                    If Denpyou.Tokuisaki.DenpyouCodeFlag Then
                        '伝票コードを個別に設定する時
                        Denpyou.NewCode = GetDenpyouCode(Denpyou.Tokuisaki.SeikyuSaki)
                        Denpyou.Tokuisaki.DenpyouCodeUpdate = GetBoolean(drTokui("納品伝票コード自動更新"))
                    Else
                        '伝票コードを個別に設定しない時
                        Denpyou.NewCode = GetDenpyouCode(0)
                        Denpyou.Tokuisaki.DenpyouCodeUpdate = drJisha("納品伝票コード自動更新")
                    End If

                    Denpyou.NouhinDenpyou = drTokui("納品伝票フォーム").ToString
                    Denpyou.KoumokuSu = GetInt(drTokui("納品伝票項目数"))
                Else
                    '標準フォームを使用する時
                    Denpyou.NewCode = GetDenpyouCode(0)
                    Denpyou.Tokuisaki.DenpyouCodeUpdate = drJisha("納品伝票コード自動更新")
                    Denpyou.NouhinDenpyou = drJisha("納品伝票ﾌｫｰﾑ")
                    Denpyou.KoumokuSu = drJisha("納品伝票項目数")
                End If
                If Denpyou.KoumokuSu = 0 Then
                    Denpyou.KoumokuSu = MaxKoumoku
                End If
            End If
            If Denpyou.TableNo = 0 Then  '新規のときだけ伝票コードを設定する
                Denpyou.Code = Denpyou.NewCode.ToString(New String("0", drJisha("納品伝票コード桁数")))
            End If

            '納入先をクリア
            Denpyou.NounyuuSaki.MasterNo = 0
            Denpyou.NounyuuSaki.Code = ""
            Denpyou.NounyuuSaki.Name = ""
            Denpyou.NounyuuSaki.Name2 = ""
            Denpyou.NounyuuSaki.Keishou = ""
            If Denpyou.Tokuisaki.Shokuchi Then  '諸口の時
                Denpyou.NounyuuSaki.Keishou = "御中"
            End If

            '得意先の担当者がいればセット
            If GetInt(drTokui("担当者マスタNo")) > 0 Then
                With Denpyou.Tantousha
                    .MasterNo = GetInt(drTokui("担当者マスタNo"))
                    Dim CTantousha As New HanbaikanriDialog.CTantousha()
                    Dim drTantou As DataRow = CTantousha.GetMaster(.MasterNo)
                    If drTantou IsNot Nothing Then
                        .Code = drTantou("コード").ToString
                        .Name = drTantou("氏名").ToString
                        .NameKana = drTantou("氏名カナ").ToString
                    Else
                        .Code = ""
                        .Name = ""
                        .NameKana = ""
                    End If
                End With
            End If

        Else
            '得意先が存在しない時
            lblTokuiName.Text = ""
            lblTokuiName2.Text = ""
            lblTokuiBikou.Text = ""

            edtNounyuuCode.Text = ""
            edtNounyuuName.Text = ""
            edtNounyuuName2.Text = ""
            cmbNounyuuKeisho.Text = ""

            Denpyou.Tokuisaki.MasterNo = 0
            Denpyou.Tokuisaki.Code = ""
            Denpyou.Tokuisaki.Name = ""
            Denpyou.Tokuisaki.Name2 = ""
            Denpyou.Tokuisaki.Keishou = ""
            Denpyou.Tokuisaki.Bikou = ""
            Denpyou.Tokuisaki.ZeiKubun = enZeikubun.外税
            lblTokuiZeiKubun.Text = GetZeikubunName(Denpyou.Tokuisaki.ZeiKubun)
            Denpyou.Tokuisaki.SeikyuSaki = 0
            Denpyou.Tokuisaki.NounyusakiExist = False

            Denpyou.NounyuuSaki.MasterNo = 0
            Denpyou.NounyuuSaki.Code = ""
            Denpyou.NounyuuSaki.Name = ""
            Denpyou.NounyuuSaki.Name2 = ""
            Denpyou.NounyuuSaki.Keishou = ""

            lblSeikyuZumi.Visible = False   '請求書発行済を表示しない
        End If
    End Sub

    '納入先コードを変更した時、納入先マスタの内容からDenpyouの内容を置き換える
    Private Sub ChangeNounyuuCode(ByVal Exist As Boolean, ByVal drTokuui As DataRow)
        If Exist Then
            '納入先が存在する時
            edtNounyuuCode.Text = drTokuui("コード").ToString
            edtNounyuuName.Text = drTokuui("名称").ToString
            edtNounyuuName2.Text = drTokuui("名称2").ToString
            cmbNounyuuKeisho.Text = drTokuui("敬称").ToString

            Denpyou.NounyuuSaki.MasterNo = drTokuui("マスタNo")
            Denpyou.NounyuuSaki.Code = drTokuui("コード").ToString
            Denpyou.NounyuuSaki.Name = drTokuui("名称").ToString
            Denpyou.NounyuuSaki.Name2 = drTokuui("名称2").ToString
            Denpyou.NounyuuSaki.Keishou = drTokuui("敬称").ToString
        Else
            '納入先が存在しない時
            edtNounyuuName.Text = ""
            edtNounyuuName2.Text = ""
            cmbNounyuuKeisho.Text = ""

            Denpyou.NounyuuSaki.MasterNo = 0
            Denpyou.NounyuuSaki.Code = ""
            Denpyou.NounyuuSaki.Name = ""
            Denpyou.NounyuuSaki.Name2 = ""
            Denpyou.NounyuuSaki.Keishou = ""
        End If

        UpdateFlagOn()
    End Sub

    '倉庫コードを変更した時、倉庫マスタの内容からDenpyouの内容を置き換える
    Private Sub ChangeSoukoCode(ByVal Exist As Boolean, ByVal drSouko As DataRow)
        If Exist Then
            '倉庫が存在する時
            edtSoukoCode.Text = drSouko("コード").ToString
            lblSoukoName.Text = drSouko("名称").ToString

            Denpyou.Souko.MasterNo = drSouko("マスタNo")
            Denpyou.Souko.Code = drSouko("コード").ToString
            Denpyou.Souko.Name = drSouko("名称").ToString

            '倉庫が変わったので、在庫数のセットとチェックをやり直す
            CheckZaiko(True)

        Else
            '倉庫が存在しない時
            lblSoukoName.Text = ""

            Denpyou.Souko.MasterNo = 0
            Denpyou.Souko.Code = ""
            Denpyou.Souko.Name = ""

            Dim Dummy As Decimal
            For mRow As Integer = 0 To MRowSheet.MaxMRows - 1
                If MRowSheet.MRows(mRow)("在庫管理有効").Value Then
                    '倉庫がないので、ワーニングメッセージ表示
                    CheckZaikoMaster(Dummy, mRow, MRowSheet.MRows(mRow)("商品マスタNo").Value, Denpyou.Souko.MasterNo, Denpyou.NouhinDate)
                    MRowSheet.MRows(mRow)("最小在庫数").Value = 0
                    MRowSheet.MRows(mRow)("在庫数").Value = 0
                End If
            Next
        End If

        UpdateFlagOn()
    End Sub

    '担当者コードを変更した時、担当者マスタの内容からDenpyouの内容を置き換える
    Private Sub ChangeTantouCode(ByVal Exist As Boolean, ByVal drTantou As DataRow)
        If Exist Then
            '担当者が存在する時
            edtTantouCode.Text = drTantou("コード").ToString
            lblTantouName.Text = drTantou("氏名").ToString

            Denpyou.Tantousha.MasterNo = drTantou("マスタNo")
            Denpyou.Tantousha.Code = drTantou("コード").ToString
            Denpyou.Tantousha.Name = drTantou("氏名").ToString
        Else
            '担当者存在しない時
            lblTantouName.Text = ""

            Denpyou.Tantousha.MasterNo = 0
            Denpyou.Tantousha.Code = ""
            Denpyou.Tantousha.Name = ""
        End If

        UpdateFlagOn()
    End Sub

    '商品コードを変更した時、名称、単価等をセット
    Private Sub ChangeShouhin(ByVal Exist As Boolean, ByVal mRow As Integer, ByVal drShouhin As DataRow)
        MRowSheet.MRows(mRow).ErrorText = ""
        MRowSheet.MRows(mRow)("商品単価").Note = Nothing
        MRowSheet.MRows(mRow)("原価単価").Note = Nothing
        MRowSheet.MRows(mRow)("消費税率").Note = Nothing
        MRowSheet.MRows(mRow)("軽減税率").Note = Nothing
        If Exist Then
            '商品が存在する時
            MRowSheet.MRows(mRow)("商品名称").Value = drShouhin("名称").ToString
            MRowSheet.MRows(mRow)("商品名称カナ").Value = drShouhin("名称カナ").ToString
            If My.Settings.EndUserName = "株式会社サンオカ" Then
                '*サンオカ*　入数/セット数を使用しない
            Else
                MRowSheet.MRows(mRow)("入数").Value = GetDecimal(drShouhin("入数"))
                MRowSheet.MRows(mRow)("セット数").Value = 0
            End If
            MRowSheet.MRows(mRow)("単位IN").Value = Nothing  'クリアしておかないと、単位が""の時クリアされない
            MRowSheet.MRows(mRow)("単位IN").Text = drShouhin("単位").ToString
            MRowSheet.MRows(mRow)("単位").Value = drShouhin("単位").ToString
            MRowSheet.MRows(mRow)("商品マスタNo").Value = GetInt(drShouhin("マスタNo"))
            MRowSheet.MRows(mRow)("商品税区分").Value = GetByte(drShouhin("税区分"))
            MRowSheet.MRows(mRow)("消費税率区分").Value = GetByte(drShouhin("消費税率区分"), enTaxRateKubun.税率1)

            Dim taxRate As Tuple(Of Decimal, Boolean) = CDenpyouCommon.GetTaxRate(Denpyou.Tokuisaki.ZeiKubun, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.SeikyuDate, Denpyou.aryRate, MRowSheet.MRows(mRow)("消費税率区分").Value)
            MRowSheet.MRows(mRow)("消費税率").Value = taxRate.Item1
            MRowSheet.MRows(mRow)("軽減税率").Value = taxRate.Item2

            MRowSheet.MRows(mRow)("在庫管理有効").Value = drShouhin("在庫管理有効")
            MRowSheet.MRows(mRow)("在庫数チェック").Value = True

            '在庫数のセットとチェック（実際の在庫数チェックは数量入力時に行なう）
            CheckZaiko(mRow, False)

            If drShouhin("在庫管理有効") Then
                '今日の伝票でないなら、在庫数はチェックしない
                If Denpyou.NouhinDate = Date.Today Then
                Else
                    MRowSheet.MRows(mRow)("在庫数").Value = 0
                End If
            Else
                MRowSheet.MRows(mRow)("在庫数").Value = 0
                MRowSheet.MRows(mRow)("最小在庫数").Value = 0
            End If

            '　得意先別単価、商品単価、原価単価をセットする
            MRowSheet.MRows(mRow)("原価単価").Value = 0    '原価単価をクリアしてセットし直す
            SetTanka(mRow, drShouhin)
            '　入力可/不可項目の設定
            If MRowSheet.MRows(mRow)("商品単価").Value = 0 Then
                MRowSheet.MRows(mRow)("金額").Lock = False
            Else
                '単価が入力されている時、金額は入力不可
                MRowSheet.MRows(mRow)("金額").Lock = True
            End If
            '    値引時は、原価はゼロとする
            If Denpyou.UriageKubun.Kubun = enUriKubunKubun.Nebiki Then
                MRowSheet.MRows(mRow)("税抜原価単価").Value = 0
                MRowSheet.MRows(mRow)("税込原価単価").Value = 0
                MRowSheet.MRows(mRow)("原価単価").Value = 0
            Else
                SetGenka(mRow, drShouhin)
            End If

            '  数量、金額、原価クリア
            MRowSheet.MRows(mRow)("数量").Value = 0
            MRowSheet.MRows(mRow)("金額").Value = 0
            MRowSheet.MRows(mRow)("税込金額").Value = 0
            MRowSheet.MRows(mRow)("税抜金額").Value = 0
            MRowSheet.MRows(mRow)("税抜原価").Value = 0
            MRowSheet.MRows(mRow)("消費税").Value = 0

        Else
            '商品が存在しない時
            MRowSheet.MRows(mRow)("商品名称").Value = ""
            MRowSheet.MRows(mRow)("商品名称カナ").Value = ""
            MRowSheet.MRows(mRow)("消費税率").Value = CDec(0)
            MRowSheet.MRows(mRow)("軽減税率").Value = False
            MRowSheet.MRows(mRow)("入数").Value = 0
            MRowSheet.MRows(mRow)("セット数").Value = 0
            MRowSheet.MRows(mRow)("単位IN").Value = Nothing
            MRowSheet.MRows(mRow)("単位").Value = ""
            MRowSheet.MRows(mRow)("原価単価").Value = 0
            MRowSheet.MRows(mRow)("商品単価").Value = 0
            MRowSheet.MRows(mRow)("商品マスタNo").Value = 0
            MRowSheet.MRows(mRow)("税抜商品単価").Value = 0
            MRowSheet.MRows(mRow)("税込商品単価").Value = 0
            MRowSheet.MRows(mRow)("税抜原価単価").Value = 0
            MRowSheet.MRows(mRow)("税込原価単価").Value = 0
            MRowSheet.MRows(mRow)("商品税区分").Value = 0
            MRowSheet.MRows(mRow)("消費税率区分").Value = 0
            MRowSheet.MRows(mRow)("得意先別単価").Value = 0
            MRowSheet.MRows(mRow)("在庫管理有効").Value = False
            MRowSheet.MRows(mRow)("在庫数チェック").Value = False
            MRowSheet.MRows(mRow)("在庫数").Value = 0
            MRowSheet.MRows(mRow)("最小在庫数").Value = 0
            MRowSheet.MRows(mRow)("数量").Value = 0
            MRowSheet.MRows(mRow)("金額").Value = 0
            MRowSheet.MRows(mRow)("税込金額").Value = 0
            MRowSheet.MRows(mRow)("税抜金額").Value = 0
            MRowSheet.MRows(mRow)("税抜原価").Value = 0
            MRowSheet.MRows(mRow)("消費税").Value = 0
        End If

        '合計金額の再計算
        SetGoukei()
    End Sub

    '消費税率を変更した時、単価、原価単価、金額、合計金額をセット
    Private Sub ChangeTaxRate(ByVal mRow As Integer)
        If MRowSheet.MRows(mRow)("商品単価").Value = 0 Then
            '変更した消費税率で、金額セット、合計金額の計算
            ChangeKingaku(mRow, MRowSheet.MRows(mRow)("金額").Value)
        Else
            '単価から、変更した消費税率で税抜or税込単価をセット、金額セット、合計金額の計算
            '（単価は変更していないが、税抜or税込単価を、変更した消費税率から計算し直す）
            ChangeTanka(mRow, MRowSheet.MRows(mRow)("商品単価").Value)
        End If

        '原価から変更した消費税率で税抜or税込原価単価をセット、税抜原価セット、合計金額の計算
        ChangeGenkaTanka(mRow, MRowSheet.MRows(mRow)("原価単価").Value)
    End Sub

    '入数を変更した時、数量、金額、合計金額をセット
    Private Sub ChangeIriSu(ByVal mRow As Integer, ByVal IriSu As Decimal)
        '数量セット
        MRowSheet.MRows(mRow)("数量").Value = CDenpyouCommon.GetCalcSuryou(IriSu, MRowSheet.MRows(mRow)("セット数").Value)

        '数量を変更した時
        ChangeSuryou(mRow, MRowSheet.MRows(mRow)("数量").Value)
    End Sub

    'セット数を変更した時、数量、金額、合計金額をセット
    Private Sub ChangeSetSu(ByVal mRow As Integer, ByVal SetSu As Decimal)
        '数量セット
        MRowSheet.MRows(mRow)("数量").Value = CDenpyouCommon.GetCalcSuryou(MRowSheet.MRows(mRow)("入数").Value, SetSu)

        '数量を変更した時
        ChangeSuryou(mRow, MRowSheet.MRows(mRow)("数量").Value)
    End Sub

    '数量を変更した時、金額、原価金額、合計金額をセット
    Private Sub ChangeSuryou(ByVal mRow As Integer, ByVal Suryou As Decimal)
        '新規でかつ今日の伝票なら最小在庫数のチェックを行う
        If MRowSheet.MRows(mRow)("在庫管理有効").Value Then
            If Denpyou.NouhinDate = Date.Today Then
                CheckMinZaiko(mRow, Suryou)
            End If
        End If

        '税抜原価のセット
        MRowSheet.MRows(mRow)("税抜原価").Value = CDenpyouCommon.GetCalcGenkaKingaku(MRowSheet.MRows(mRow)("税抜原価単価").Value, Suryou, Denpyou.Tokuisaki.Hasuu)

        If MRowSheet.MRows(mRow)("商品単価").Value <> 0 Then
            '金額セット
            SetKingaku(mRow, MRowSheet.MRows(mRow)("商品単価").Value, Suryou)
        End If

        '合計金額の計算を行う
        SetGoukei()
    End Sub

    '明細の原価単価を変更した時、原価単価、税抜原価、合計金額をセット
    Private Sub ChangeGenkaTanka(ByVal mRow As Integer, ByVal Genka As Decimal)
        '入力した原価から税抜/税込原価単価をセット
        Dim zeinukiZeikomi As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetCalcZeinukiZeikomi(Genka, Denpyou.Tokuisaki.ZeiKubun, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.Hasuu, GetByte(drJisha("買掛単価少数桁数")), MRowSheet.MRows(mRow)("消費税率").Value)
        MRowSheet.MRows(mRow)("税抜原価単価").Value = zeinukiZeikomi.ZeinukiGaku
        MRowSheet.MRows(mRow)("税込原価単価").Value = zeinukiZeikomi.ZeikomiGaku

        '税抜原価セット
        MRowSheet.MRows(mRow)("税抜原価").Value = CDenpyouCommon.GetCalcGenkaKingaku(MRowSheet.MRows(mRow)("税抜原価単価").Value, MRowSheet.MRows(mRow)("数量").Value, Denpyou.Tokuisaki.Hasuu)

        '合計金額の計算を行う
        SetGoukei()
    End Sub

    '明細の単価を変更した時、金額、合計金額をセット
    Private Sub ChangeTanka(ByVal mRow As Integer, ByVal Tanka As Decimal)
        '入力した単価から税抜/税込商品単価をセット
        Dim zeinukiZeikomi As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetCalcZeinukiZeikomi(Tanka, Denpyou.Tokuisaki.ZeiKubun, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.Hasuu, GetByte(drJisha("売掛単価少数桁数")), MRowSheet.MRows(mRow)("消費税率").Value)
        MRowSheet.MRows(mRow)("税抜商品単価").Value = zeinukiZeikomi.ZeinukiGaku
        MRowSheet.MRows(mRow)("税込商品単価").Value = zeinukiZeikomi.ZeikomiGaku

        '金額セット
        SetKingaku(mRow, Tanka, MRowSheet.MRows(mRow)("数量").Value)

        '合計金額の計算を行う
        SetGoukei()
    End Sub

    '明細の金額を変更した時、金額、合計金額をセット
    Private Sub ChangeKingaku(ByVal mRow As Integer, ByVal Kingaku As Decimal)
        '入力した金額から税抜/税込金額をセット
        Dim zeinukiZeikomi As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetCalcZeinukiZeikomi(Kingaku, Denpyou.Tokuisaki.ZeiKubun, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.Hasuu, 0, MRowSheet.MRows(mRow)("消費税率").Value, Denpyou.Tokuisaki.ShouhizeiKeisan)
        MRowSheet.MRows(mRow)("税抜金額").Value = zeinukiZeikomi.ZeinukiGaku
        MRowSheet.MRows(mRow)("税込金額").Value = zeinukiZeikomi.ZeikomiGaku
        MRowSheet.MRows(mRow)("消費税").Value = zeinukiZeikomi.ShouhizeiGaku  '（明細毎以外は、小数点以下4桁まで保持）

        '合計金額の計算を行う
        SetGoukei()
    End Sub

    '得意先別単価、商品単価をセットする
    Private Sub SetTanka(ByVal mRow As Integer, ByVal drShouhin As DataRow)
        '商品単価を得る
        Dim shouhintanka As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetUriageTanka(drShouhin("マスタNo"), drShouhin("標準単価"), Denpyou.Tokuisaki.SeikyuSaki, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.Kakeritu, Denpyou.Tokuisaki.Hasuu, GetByte(drJisha("売掛単価少数桁数")), MRowSheet.MRows(mRow)("消費税率").Value, Denpyou.aryRate(MRowSheet.MRows(mRow)("消費税率区分").Value - 1))

        '得意先別単価、商品単価をセット
        MRowSheet.MRows(mRow)("税抜商品単価").Value = shouhintanka.ZeinukiGaku
        MRowSheet.MRows(mRow)("税込商品単価").Value = shouhintanka.ZeikomiGaku
        MRowSheet.MRows(mRow)("商品単価").Value = shouhintanka.Kingaku
        MRowSheet.MRows(mRow)("得意先別単価").Value = shouhintanka.Kingaku
    End Sub

    '原価単価をセットする
    Private Sub SetGenka(ByVal mRow As Integer, ByVal drShouhin As DataRow)
        '原価単価を得る
        Dim genkatanka As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetGenkaTanka(drShouhin("仕入単価"), MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.Hasuu, GetByte(drJisha("買掛単価少数桁数")), MRowSheet.MRows(mRow)("消費税率").Value, Denpyou.aryRate(MRowSheet.MRows(mRow)("消費税率区分").Value - 1))

        '原価単価をセット
        MRowSheet.MRows(mRow)("税抜原価単価").Value = genkatanka.ZeinukiGaku
        MRowSheet.MRows(mRow)("税込原価単価").Value = genkatanka.ZeikomiGaku
        MRowSheet.MRows(mRow)("原価単価").Value = genkatanka.Kingaku
    End Sub

    '金額をセットする
    Private Sub SetKingaku(ByVal mRow As Integer, ByVal Tanka As Decimal, ByVal Suryou As Decimal)
        '金額セット
        Dim kingaku As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetCalcKingaku(Tanka, Suryou, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.Hasuu, MRowSheet.MRows(mRow)("消費税率").Value,,, Denpyou.Tokuisaki.ShouhizeiKeisan)
        MRowSheet.MRows(mRow)("金額").Value = kingaku.Kingaku
        MRowSheet.MRows(mRow)("税抜金額").Value = kingaku.ZeinukiGaku
        MRowSheet.MRows(mRow)("税込金額").Value = kingaku.ZeikomiGaku
        MRowSheet.MRows(mRow)("消費税").Value = kingaku.ShouhizeiGaku  '（明細毎以外は、小数点以下4桁まで保持）
    End Sub

    '合計金額を計算し画面にセットする
    '  isSetGoukei=False：合計シートの合計行(1行目)はセットしない（既存データからの表示時は既存データから表示するため計算しない。消費税率別集計はデータに持っていないため計算が必要。）
    Private Sub SetGoukei(Optional ByVal isSetGoukei As Boolean = True)
        Try
            Dim calcGoukei() As CDenpyouCommon.strctGetCalcGoukei2
            If My.Settings.EndUserName = "山田商店" Then
                '*山田商店*　取引時で現金の時の消費税計算は、1円の位で丸めて10円単位にする（GetCalcGoukeiに締日を渡す）
                calcGoukei = CDenpyouCommon.GetCalcGoukei(Me, True, MRowSheet, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.ShouhizeiKeisan, Denpyou.Tokuisaki.Hasuu, Denpyou.Tokuisaki.Simebi)  '粗利計算をする
            Else
                calcGoukei = CDenpyouCommon.GetCalcGoukei(Me, True, MRowSheet, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.ShouhizeiKeisan, Denpyou.Tokuisaki.Hasuu)  '粗利計算をする
            End If

            numArariKei.Value = calcGoukei(0).ArariKei  '粗利計

            sheetGoukei.MaxRows = enSheetGoukeiRow.合計行 + 1
            sheetGoukei.MaxRows = calcGoukei.Length
            For idx As Integer = 0 To calcGoukei.Length - 1
                If isSetGoukei = False AndAlso idx = 0 Then
                    '合計行にセットしない時、計算結果と既存データの値が違う場合、エラーアイコンを表示する
                    If sheetGoukei(enSheetGoukeiCol.税抜額, idx).Value <> calcGoukei(idx).Zeinuki Then
                        sheetGoukei(enSheetGoukeiCol.税抜額 - 1, idx).ErrorIcon = My.Resources.StatusInvalid_16x
                        sheetGoukei(enSheetGoukeiCol.税抜額 - 1, idx).ErrorText = "金額が合いません（計算値：" & calcGoukei(idx).Zeinuki.ToString("#,0.###") & ")"
                    End If
                    If sheetGoukei(enSheetGoukeiCol.消費税額, idx).Value <> calcGoukei(idx).ShouhiZeiKei Then
                        sheetGoukei(enSheetGoukeiCol.消費税額 - 1, idx).ErrorIcon = My.Resources.StatusInvalid_16x
                        sheetGoukei(enSheetGoukeiCol.消費税額 - 1, idx).ErrorText = "金額が合いません（計算値：" & calcGoukei(idx).ShouhiZeiKei.ToString("#,0.###") & ")"
                    End If
                    If sheetGoukei(enSheetGoukeiCol.参考消費税, idx).Value <> calcGoukei(idx).SankouShouhiZei Then
                        sheetGoukei(enSheetGoukeiCol.参考消費税 - 1, idx).ErrorIcon = My.Resources.StatusInvalid_16x
                        sheetGoukei(enSheetGoukeiCol.参考消費税 - 1, idx).ErrorText = "金額が合いません（計算値：" & calcGoukei(idx).SankouShouhiZei.ToString("#,0.###") & ")"
                    End If
                    If sheetGoukei(enSheetGoukeiCol.合計, idx).Value <> calcGoukei(idx).Goukei Then
                        sheetGoukei(enSheetGoukeiCol.合計 - 1, idx).ErrorIcon = My.Resources.StatusInvalid_16x
                        sheetGoukei(enSheetGoukeiCol.合計 - 1, idx).ErrorText = "金額が合いません（計算値：" & calcGoukei(idx).Goukei.ToString("#,0.###") & ")"
                    End If
                    Continue For
                ElseIf isSetGoukei AndAlso idx = 0 Then
                    For iCol As Integer = 0 To sheetGoukei.MaxColumns - 1
                        sheetGoukei(iCol, enSheetGoukeiRow.合計行).ErrorText = ""
                    Next
                End If

                sheetGoukei(enSheetGoukeiCol.消費税率テキスト, idx).Value = calcGoukei(idx).TaxRateText
                sheetGoukei(enSheetGoukeiCol.消費税率, idx).Value = calcGoukei(idx).TaxRate
                sheetGoukei(enSheetGoukeiCol.税抜額 - 1, idx).Value = "税抜額"
                sheetGoukei(enSheetGoukeiCol.税抜額, idx).Value = calcGoukei(idx).Zeinuki
                sheetGoukei(enSheetGoukeiCol.消費税額 - 1, idx).Value = "消費税額"
                sheetGoukei(enSheetGoukeiCol.消費税額, idx).Value = calcGoukei(idx).ShouhiZeiKei
                sheetGoukei(enSheetGoukeiCol.参考消費税 - 1, idx).Value = "参考消費税"
                sheetGoukei(enSheetGoukeiCol.参考消費税, idx).Value = calcGoukei(idx).SankouShouhiZei
                sheetGoukei(enSheetGoukeiCol.合計 - 1, idx).Value = "合計"
                sheetGoukei(enSheetGoukeiCol.合計, idx).Value = calcGoukei(idx).Goukei
            Next

            '罫線
            sheetGoukei.SetBorder(New GrapeCity.Win.ElTabelle.Range(0, 0, sheetGoukei.MaxColumns - 1, sheetGoukei.MaxRows - 1),
                New GrapeCity.Win.ElTabelle.BorderLine(GRID_COLOR, GrapeCity.Win.ElTabelle.BorderLineStyle.Thin), GrapeCity.Win.ElTabelle.Borders.All)
            '　合計行を太線で囲む
            sheetGoukei.SetBorder(New GrapeCity.Win.ElTabelle.Range(0, enSheetGoukeiRow.合計行, sheetGoukei.MaxColumns - 1, enSheetGoukeiRow.合計行),
                New GrapeCity.Win.ElTabelle.BorderLine(GRID_COLOR, GrapeCity.Win.ElTabelle.BorderLineStyle.Medium), GrapeCity.Win.ElTabelle.Borders.OutLine)
            If sheetGoukei.MaxRows > (enSheetGoukeiRow.合計行 + 1) Then
                sheetGoukei.FreezeRows = enSheetGoukeiRow.合計行 + 1
            End If

        Catch ex As Exception
            ErrProc(ex, Me.Text)
        End Try
    End Sub

    'デフォルト商品を付加（金額/数量は入力されているが、商品がセットされていない行に、デフォルト商品をセットする）
    Private Sub SetDefaultShouhin(ByVal mRow As Integer)
        If defaultShouhin.MasterNo <= 0 Then Exit Sub
        If GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value) > 0 Then Exit Sub

        If MRowSheet.MRows(mRow)("金額").Value <> 0 OrElse MRowSheet.MRows(mRow)("数量").Value <> 0 Then  '金額/数量が入力されている行の時
            MRowSheet.MRows(mRow)("商品マスタNo").Value = defaultShouhin.MasterNo
            MRowSheet.MRows(mRow)("商品コード").Value = defaultShouhin.Code
            MRowSheet.MRows(mRow)("商品税区分").Value = defaultShouhin.ZeiKubun
            MRowSheet.MRows(mRow)("消費税率区分").Value = defaultShouhin.TaxRateKubun

            Dim taxRate As Tuple(Of Decimal, Boolean) = CDenpyouCommon.GetTaxRate(Denpyou.Tokuisaki.ZeiKubun, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.SeikyuDate, Denpyou.aryRate, MRowSheet.MRows(mRow)("消費税率区分").Value)
            MRowSheet.MRows(mRow)("消費税率").Value = taxRate.Item1
            MRowSheet.MRows(mRow)("軽減税率").Value = taxRate.Item2
            MRowSheet.MRows(mRow)("消費税率").Note = Nothing
            MRowSheet.MRows(mRow)("軽減税率").Note = Nothing

            ChangeTaxRate(mRow)
        End If
    End Sub

    '消費税率マスタより、明細シートの消費税率コンボボックスを設定（消費税マスタの税率と0%を設定）
    '（シートのテンプレートを設定し直すと、他の値が消えることがあるためセルに設定）
    '（テンプレートの列をコンボボックスにしておくと、セルに設定しているにもかかわらず、何故か他行のセルのitemsも追加削除されてしまうため、テンプレートの列はTextにしている）
    '（基本的には消費税率マスタの税率＋0%のみだが、既存データで使用している%がSetFormで追加される）
    Private Sub SetComboItemTaxRate()
        For mRow As Integer = 0 To MRowSheet.MaxMRows - 1
            SetComboItemTaxRate(mRow)
            If MRowSheet.MRows(mRow)("消費税率").Value Is Nothing Then
                MRowSheet.MRows(mRow)("消費税率").Value = CDec(0)
            End If
        Next
    End Sub
    Private Sub SetComboItemTaxRate(ByVal mRow As Integer)  'シート1行分の設定のみ行う
        '消費税マスタの税率（Denpyou.aryRate）を一意にして、消費税率ゼロが無ければ追加（非課税で使用）
        Dim lstRate As New List(Of Decimal)
        For i As Integer = 0 To Denpyou.aryRate.Length - 1
            If lstRate.Contains(Denpyou.aryRate(i)) = False Then
                lstRate.Add(Denpyou.aryRate(i))
            End If
        Next
        If lstRate.Contains(0) = False Then
            lstRate.Add(0)  '0%を追加
        End If

        Dim cmbEditor As New GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor()
        cmbEditor.Items.Clear()
        For i As Integer = 0 To lstRate.Count - 1
            cmbEditor.Items.Add(New GrapeCity.Win.ElTabelle.Editors.ComboItem(0, Nothing, lstRate(i).ToString("#0%").PadLeft(3), "", lstRate(i)))
        Next
        cmbEditor.Editable = False  '手入力不可（選択のみ）
        cmbEditor.ItemHeight = ROWHEIGHT * 0.85  '１行の高さ
        cmbEditor.ValueAsIndex = False  'IndexとValueの値が違う場合、Valueの値を使用
        cmbEditor.ShowDropDown = GrapeCity.Win.ElTabelle.Editors.Visibility.ShowOnFocus
        MRowSheet.MRows(mRow)("消費税率").Editor = cmbEditor
    End Sub

    '売上区分マスタNoから売上区分マスタを取得し、売上区分情報をセットする
    Private Sub SetUriageKubunInfo(ByRef connection As SqlConnection, ByVal uriageKubunNo As Integer)
        Dim CUriageKubun As New CUriageKubun()
        Dim drUriageKubun As DataRow = CUriageKubun.GetMaster(uriageKubunNo, connection)
        If drUriageKubun IsNot Nothing Then
            Denpyou.UriageKubun.Code = drUriageKubun("コード").ToString
            Denpyou.UriageKubun.Zougen = drUriageKubun("増減")
            Denpyou.UriageKubun.Kubun = drUriageKubun("区分")
        Else
            MessageBox.Show("売上区分マスタに該当データがありません。" & vbCrLf & "（売上区分マスタNo=" & uriageKubunNo & "）", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Denpyou.UriageKubun.MasterNo = 0
            Denpyou.UriageKubun.Code = ""
            Denpyou.UriageKubun.Zougen = 1
            Denpyou.UriageKubun.Kubun = 0
        End If
    End Sub

    '商品、倉庫、納品日から、在庫数を得る
    Private Function GetZaikoSu(ByVal MasterNo As Integer) As Decimal
        Dim CShouhin As New HanbaikanriDialog.CShouhin()
        Dim zaikoSu As Decimal = 0
        If CShouhin.Zaiko(MasterNo, Denpyou.Souko.MasterNo, Denpyou.NouhinDate, zaikoSu) Then
            Return zaikoSu
        Else
            Return 0
        End If
    End Function

    '税区分/端数/掛率/消費税率の変更に伴い、明細行の再計算を行なう
    Private Sub ReCalcMeisai()
        Dim CShouhin As New HanbaikanriDialog.CShouhin()
        For mRow As Integer = 0 To MRowSheet.MaxMRows - 1
            '商品マスタNoをキーに、商品マスタのレコードを得る
            Dim drShouhin As DataRow = Nothing
            If GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value) > 0 Then
                drShouhin = CShouhin.GetMaster(GetInt(MRowSheet.MRows(mRow)("商品マスタNo").Value))
            End If

            ReCalcMeisai(mRow, drShouhin, Denpyou.Tokuisaki.SeikyuSaki)

            If TypeOf MRowSheet.MRows(mRow)("消費税率").Editor IsNot GrapeCity.Win.ElTabelle.Editors.SuperiorComboEditor Then
                SetComboItemTaxRate(mRow)  '消費税率コンボボックスの設定
                MRowSheet.MRows(mRow)("消費税率").Value = CDec(0)
            End If
        Next
    End Sub
    Private Sub ReCalcMeisai(ByVal mRow As Integer, ByVal drShouhin As DataRow, ByVal SeikyuSakiMasterNo As Integer)
        Dim orgTaxRate As Decimal = MRowSheet.MRows(mRow)("消費税率").Value  '変更前の消費税率
        SetComboItemTaxRate(mRow)  '明細シートの消費税率コンボボックスを、新しい消費税率で設定

        If DataInSheet(mRow, MRowSheet) = False Then
            MRowSheet.MRows(mRow)("消費税率").Value = CDec(0)
            Exit Sub
        End If

        If drShouhin IsNot Nothing Then
            '商品マスタありの時
            '現在の商品マスタと消費税マスタから消費税率を再設定
            Dim taxRate As Tuple(Of Decimal, Boolean) = CDenpyouCommon.GetTaxRate(Denpyou.Tokuisaki.ZeiKubun, drShouhin("税区分"), Denpyou.SeikyuDate, Denpyou.aryRate, GetByte(drShouhin("消費税率区分"), enTaxRateKubun.税率1))
            MRowSheet.MRows(mRow)("消費税率").Value = taxRate.Item1
            MRowSheet.MRows(mRow)("軽減税率").Value = taxRate.Item2
            If MRowSheet.MRows(mRow)("消費税率").Value Is Nothing Then
                MRowSheet.MRows(mRow)("消費税率").Value = CDec(0)
            End If

            If MRowSheet.MRows(mRow)("商品単価").Value <> 0 Then
                '商品単価が入力済の時
                '  変更前の税区分で単価を得る
                Dim shouhintanka As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetUriageTanka(drShouhin("マスタNo"), drShouhin("標準単価"), SeikyuSakiMasterNo, MRowSheet.MRows(mRow)("商品税区分").Value, oldZeikubun, oldKakeritu, oldHasuu, GetByte(drJisha("売掛単価少数桁数")), orgTaxRate, oldAryRate(MRowSheet.MRows(mRow)("消費税率区分").Value - 1))

                '　商品単価の再計算
                If MRowSheet.MRows(mRow)("商品単価").Value = shouhintanka.Kingaku Then
                    '単価の変更をしていないので、マスタから持って来直して再計算
                    ReCalcTanka_Master(mRow, drShouhin)

                Else
                    '単価の変更入力がされているので、入力された単価から再計算
                    ReCalcMeisai_TankaIn(mRow)
                End If

            Else
                '商品単価が未入力で、金額が入力済の時
                If MRowSheet.MRows(mRow)("金額").Value <> 0 Then
                    '入力された金額から再計算
                    ReCalcMeisai_KingakuIn(mRow)
                End If
            End If

            '原価単価の再計算（変更入力前の税区分/端数で計算）
            Dim genkatanka As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetGenkaTanka(drShouhin("仕入単価"), MRowSheet.MRows(mRow)("商品税区分").Value, oldZeikubun, oldHasuu, GetByte(drJisha("買掛単価少数桁数")), orgTaxRate, oldAryRate(MRowSheet.MRows(mRow)("消費税率区分").Value - 1))
            If MRowSheet.MRows(mRow)("原価単価").Value = genkatanka.Kingaku Then
                '原価単価の変更をしていないので、マスタから持って来直して再計算
                ReCalcGenka_Master(mRow, drShouhin)

            Else
                '原価単価の変更入力がされているので、入力された原価単価から再計算
                ReCalcMeisai_GenkaIn(mRow)
            End If

        Else
            '商品マスタなしの時
            MRowSheet.MRows(mRow)("消費税率").Value = orgTaxRate
            If MRowSheet.MRows(mRow)("消費税率").Value Is Nothing Then
                MRowSheet.MRows(mRow)("消費税率").Value = CDec(0)
            End If

            ReCalcMeisai_NotShouhin(mRow)
        End If
    End Sub

    '得意先別単価、商品単価を、マスタから再セット
    Private Sub ReCalcTanka_Master(ByVal mRow As Integer, ByVal drShouhin As DataRow)
        '得意先別単価、商品単価セット
        SetTanka(mRow, drShouhin)
        '金額セット
        SetKingaku(mRow, MRowSheet.MRows(mRow)("商品単価").Value, MRowSheet.MRows(mRow)("数量").Value)
    End Sub

    '原価単価、税抜原価を、マスタから再セット
    Private Sub ReCalcGenka_Master(ByVal mRow As Integer, ByVal drShouhin As DataRow)
        '原価単価セット
        SetGenka(mRow, drShouhin)
        '税抜原価セット
        MRowSheet.MRows(mRow)("税抜原価").Value = CDenpyouCommon.GetCalcGenkaKingaku(MRowSheet.MRows(mRow)("税抜原価単価").Value, MRowSheet.MRows(mRow)("数量").Value, Denpyou.Tokuisaki.Hasuu)
    End Sub

    '商品単価、金額を、入力した単価から再計算してセット
    Private Sub ReCalcMeisai_TankaIn(ByVal mRow As Integer)
        '商品単価のセット（手入力した単価から税抜/税込単価を再計算）
        Dim tanka As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetRecalcKingakuIn(MRowSheet.MRows(mRow)("税抜商品単価").Value, MRowSheet.MRows(mRow)("税込商品単価").Value, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.Hasuu, GetByte(drJisha("売掛単価少数桁数")), MRowSheet.MRows(mRow)("消費税率").Value)
        MRowSheet.MRows(mRow)("商品単価").Value = tanka.Kingaku
        MRowSheet.MRows(mRow)("税抜商品単価").Value = tanka.ZeinukiGaku
        MRowSheet.MRows(mRow)("税込商品単価").Value = tanka.ZeikomiGaku

        '金額セット
        SetKingaku(mRow, MRowSheet.MRows(mRow)("商品単価").Value, MRowSheet.MRows(mRow)("数量").Value)
    End Sub

    '金額を、入力した金額から再計算してセット（単価未入力の時）
    Private Sub ReCalcMeisai_KingakuIn(ByVal mRow As Integer)
        '金額のセット（手入力した金額から税抜/税込金額を再計算）
        Dim kingaku As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetRecalcKingakuIn(MRowSheet.MRows(mRow)("税抜金額").Value, MRowSheet.MRows(mRow)("税込金額").Value, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.Hasuu, 0, MRowSheet.MRows(mRow)("消費税率").Value, Denpyou.Tokuisaki.ShouhizeiKeisan)
        MRowSheet.MRows(mRow)("金額").Value = kingaku.Kingaku
        MRowSheet.MRows(mRow)("税抜金額").Value = kingaku.ZeinukiGaku
        MRowSheet.MRows(mRow)("税込金額").Value = kingaku.ZeikomiGaku
        MRowSheet.MRows(mRow)("消費税").Value = kingaku.ShouhizeiGaku  '（明細毎以外は、小数点以下4桁まで保持）
    End Sub

    '原価単価、税抜原価を、入力した原価単価から再計算してセット
    Private Sub ReCalcMeisai_GenkaIn(ByVal mRow As Integer)
        If Denpyou.SiireDenpyouNo = 0 OrElse DirectCast(oldAryRate, IStructuralEquatable).Equals(Denpyou.aryRate, StructuralComparisons.StructuralEqualityComparer) = False Then
            '仕入参照でない時、または消費税率が変わった時
            '　原価単価のセット（手入力した原価単価から税抜/税込原価単価を再計算）
            Dim tanka As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetRecalcKingakuIn(MRowSheet.MRows(mRow)("税抜原価単価").Value, MRowSheet.MRows(mRow)("税込原価単価").Value, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.Hasuu, GetByte(drJisha("買掛単価少数桁数")), MRowSheet.MRows(mRow)("消費税率").Value)
            MRowSheet.MRows(mRow)("原価単価").Value = tanka.Kingaku
            MRowSheet.MRows(mRow)("税抜原価単価").Value = tanka.ZeinukiGaku
            MRowSheet.MRows(mRow)("税込原価単価").Value = tanka.ZeikomiGaku

        Else
            '仕入参照で消費税率変更なしの時、表示のみ切り替え
            '  表示する原価単価のセット
            If Denpyou.Tokuisaki.ZeiKubun = enZeikubun.外税 Then
                MRowSheet.MRows(mRow)("原価単価").Value = MRowSheet.MRows(mRow)("税抜原価単価").Value
            Else
                MRowSheet.MRows(mRow)("原価単価").Value = MRowSheet.MRows(mRow)("税込原価単価").Value
            End If
        End If

        '税抜原価セット
        MRowSheet.MRows(mRow)("税抜原価").Value = CDenpyouCommon.GetCalcGenkaKingaku(MRowSheet.MRows(mRow)("税抜原価単価").Value, MRowSheet.MRows(mRow)("数量").Value, Denpyou.Tokuisaki.Hasuu)
    End Sub

    '商品コード未入力の明細の時、手入力した単価 or 金額、原価単価から再計算してセット
    Private Sub ReCalcMeisai_NotShouhin(ByVal mRow As Integer)
        MRowSheet.MRows(mRow)("商品税区分").Value = Denpyou.Tokuisaki.ZeiKubun

        If MRowSheet.MRows(mRow)("商品単価").Value <> 0 Then
            '商品単価が入力済の時、商品単価、金額を入力した単価から再計算
            ReCalcMeisai_TankaIn(mRow)
        Else
            '商品単価が未入力
            If MRowSheet.MRows(mRow)("金額").Value <> 0 Then
                '金額が入力済の時、金額から再計算
                ReCalcMeisai_KingakuIn(mRow)
            End If
        End If

        '原価単価をセット
        If MRowSheet.MRows(mRow)("原価単価").Value <> 0 Then
            '原価単価が入力済の時、単価から再計算
            ReCalcMeisai_GenkaIn(mRow)
        End If
    End Sub

    '請求書発行済の日付なら、確認ダイアログを表示する
    Private Function CheckSeikyuShoMsg(ByVal UpdateType As String) As Boolean
        Dim datSime As Object = Nothing
        If CheckSeikyuShoHakkou(datSime) Then  '請求書発行済かどうか
            If MessageBox.Show(DirectCast(datSime, Date).ToString("yyyy/MM/dd") & "締の請求書を発行済みですが、" & vbCrLf _
                               & Denpyou.SeikyuDate.ToString("yyyy/MM/dd") & "の伝票を" & UpdateType & "してもよろしいですか？",
                               Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) _
              = DialogResult.No Then
                Return False  '登録しない
            End If
        End If
        Return True  '登録する
    End Function

    '請求書発行済の日付かどうかをチェック
    Private Function CheckSeikyuShoHakkou(Optional ByRef datSime As Object = Nothing) As Boolean
        Dim sSQL As String
        sSQL = "SELECT TOP 1 締日 FROM 請求書 " _
             & "WHERE 請求先マスタNo = " & Denpyou.Tokuisaki.SeikyuSaki _
             & " ORDER BY 締日 DESC"
        datSime = CDBCommon.SQLExecuteScalar(CSingleton.CSetting.Connect, sSQL) '単一データを取得
        If datSime IsNot Nothing AndAlso datSime >= Denpyou.SeikyuDate Then
            Return True  '発行済（請求書の最新締日を返す）
        Else
            Return False  '未発行
        End If
    End Function

    '在庫のチェックを行う
    '　商品在庫マスタに、該当の在庫データが登録されているかチェック（ワーニング）
    '　最小在庫数、在庫数のセットも行う。最小在庫数のチェックも行う
    '  納品日（請求日）、倉庫、商品が変わった時チェック。GetRecord後もチェック
    Private Sub CheckZaiko(ByVal MinZaikoCheck As Boolean)
        '明細の全ての行をチェック
        For mRow As Integer = 0 To MRowSheet.MaxMRows - 1
            CheckZaiko(mRow, MinZaikoCheck)
        Next
    End Sub
    '　明細の指定の行をチェック
    Private Sub CheckZaiko(ByVal mRow As Integer, ByVal MinZaikoCheck As Boolean)
        MRowSheet.MRows(mRow).ErrorText = ""

        If MRowSheet.MRows(mRow)("在庫管理有効").Value = False Then    '在庫管理有効でない時はチェックしない
            Exit Sub
        End If

        '商品在庫マスタに、該当の在庫データが登録されているかチェック（ワーニング）
        Dim dMinZaikoSu As Decimal = 0
        CheckZaikoMaster(dMinZaikoSu, mRow, MRowSheet.MRows(mRow)("商品マスタNo").Value, Denpyou.Souko.MasterNo, Denpyou.NouhinDate)
        MRowSheet.MRows(mRow)("最小在庫数").Value = dMinZaikoSu

        '今日の伝票なら在庫を取得する（新規だけでなく検索時も数量変更時はチェック）
        If Denpyou.NouhinDate = Date.Today Then
            MRowSheet.MRows(mRow)("在庫数").Value = GetZaikoSu(MRowSheet.MRows(mRow)("商品マスタNo").Value)

            If MinZaikoCheck Then    '最小在庫数のチェックを行う時
                CheckMinZaiko(mRow, MRowSheet.MRows(mRow)("数量").Value)    '最小在庫数のチェック
            End If
        End If
    End Sub

    '商品在庫マスタに、該当商品の在庫データが登録されているかチェック（ワーニング）
    Private Sub CheckZaikoMaster(ByRef minZaikoSu As Decimal, ByVal mRow As Integer, ByVal shouhinNo As Integer, ByVal soukoNo As Integer, ByVal nouhinDate As Date)
        '在庫チェックを行い、最小在庫数を得る（minZaikoSuに最小在庫数がセットされる）
        '  商品在庫マスタに登録されていない時、運用開始日 > 納品日の締日の時、ワーニング
        Dim res As String = CDenpyouCommon.CheckZaikoMaster(shouhinNo, soukoNo, nouhinDate, "納品日", drJisha("締日"), drJisha("和暦"), minZaikoSu)
        If res = "" Then
            MRowSheet.MRows(mRow).ErrorText = ""
        Else
            'MRowSheet.MRows(mRow).ErrorIcon = Nothing  '通常のアイコンにする（赤に！）
            MRowSheet.MRows(mRow).ErrorIcon = My.Resources.StatusInvalid_16x
            MRowSheet.MRows(mRow).ErrorText = res
        End If
    End Sub

    '最小在庫数のチェックを行う
    Private Sub CheckMinZaiko(ByVal mRow As Integer, ByVal Suryou As Decimal)
        If MRowSheet.MRows(mRow).ErrorText <> "" Then
            '既に商品在庫マスタのエラーなので、チェックしない
            Exit Sub
        End If
        MRowSheet.MRows(mRow).ErrorText = ""

        If Suryou <= 0 _
          OrElse MRowSheet.MRows(mRow)("商品マスタNo").Value = 0 _
          OrElse drJisha("在庫切れMSG") = False _
          OrElse MRowSheet.MRows(mRow)("在庫数チェック").Value = False _
          OrElse Denpyou.UriageKubun.Kubun = enUriKubunKubun.Nebiki _
          OrElse Denpyou.UriageKubun.Zougen < 0 Then
            '  売上区分が値引の時はチェックしない（値引、値引取消時）
            '  売上区分の増減がマイナスの時はチェックしない（売上取消、返品時）
            Exit Sub
        End If

        Dim NewZaikoSu As Decimal = MRowSheet.MRows(mRow)("在庫数").Value - Suryou
        Dim MinZaikoSu As Decimal = MRowSheet.MRows(mRow)("最小在庫数").Value

        MRowSheet.MRows(mRow).ErrorIcon = New Icon(My.Resources.StatusWarning, 16, 16)
        If NewZaikoSu < 0 Then
            MRowSheet.MRows(mRow).ErrorText = "商品が足りません"
        ElseIf NewZaikoSu < MinZaikoSu Then
            MRowSheet.MRows(mRow).ErrorText = "商品の在庫数が最小在庫数を下回ります"
        End If
    End Sub

    '消費税率変更時、金額の再計算を行なう
    Private Sub WhenChangeRate()
        If Denpyou.Tokuisaki.ZeiKubun <> enZeikubun.非課税 Then '非課税以外
            '明細行の再計算
            ReCalcMeisai()
            '合計金額の再計算
            SetGoukei()
        Else
            SetComboItemTaxRate()  '明細シートの消費税率コンボボックスを、新しい消費税率で設定
        End If
    End Sub

    '商品単価、原価単価が最新の単価と同じかどうかチェックし、違う時はワーニングを表示する
    Private Sub CheckNewTanka()
        For mRow As Integer = 0 To MRowSheet.MaxMRows - 1
            If MRowSheet.MRows(mRow)("商品マスタNo").Value = 0 OrElse MRowSheet.MRows(mRow)("商品マスタNo").Value = defaultShouhin.MasterNo Then
                Continue For  '商品未入力行の時、商品がデフォルト商品の時はチェックしない
            End If
            'If DataInSheet(mRow, MRowSheet) = False OrElse MRowSheet.MRows(mRow)("商品マスタNo").Value = 0 Then
            '    Continue For  '未入力行の時
            'End If

            '商品単価が最新かどうかチェック
            Dim CShouhin As New HanbaikanriDialog.CShouhin()
            Dim drShouhin As DataRow = CShouhin.GetMaster(MRowSheet.MRows(mRow)("商品マスタNo").Value)
            If drShouhin IsNot Nothing Then  '商品マスタあり
                '得意先別単価or商品単価を得る
                Dim shouhintanka As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetUriageTanka(drShouhin("マスタNo"), drShouhin("標準単価"), Denpyou.Tokuisaki.SeikyuSaki, MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.Kakeritu, Denpyou.Tokuisaki.Hasuu, GetByte(drJisha("売掛単価少数桁数")), MRowSheet.MRows(mRow)("消費税率").Value, Denpyou.aryRate(MRowSheet.MRows(mRow)("消費税率区分").Value - 1))

                '商品単価が最新と違う場合はセルノートを表示
                If MRowSheet.MRows(mRow)("商品単価").Value <> shouhintanka.Kingaku Then
                    Dim format As String = "#,0"
                    If GetByte(drJisha("売掛単価少数桁数")) > 0 Then
                        format &= "." & New String("0", GetByte(drJisha("売掛単価少数桁数")))
                    End If
                    Dim msg As String = "登録されている商品単価は「" & shouhintanka.Kingaku.ToString(format) & "」です"
                    CFormCommon.SetSelNote(MRowSheet.MRows(mRow)("商品単価"), msg)
                End If

                '原価単価が最新かどうかチェック
                CheckNewGenkaTanka(mRow, drShouhin)
            End If
        Next
    End Sub

    '原価単価が最新の仕入単価と同じかどうかチェックし、違う時はワーニングを表示する
    Private Sub CheckNewGenkaTanka()
        '売上区分=値引時は、原価はチェックしない
        If Denpyou.UriageKubun.Kubun = enUriKubunKubun.Nebiki Then
            Exit Sub
        End If

        For mRow As Integer = 0 To MRowSheet.MaxMRows - 1
            If MRowSheet.MRows(mRow)("商品マスタNo").Value = 0 OrElse MRowSheet.MRows(mRow)("商品マスタNo").Value = defaultShouhin.MasterNo Then
                Continue For  '商品未入力行の時、商品がデフォルト商品の時はチェックしない
            End If
            'If DataInSheet(iRow, MRowSheet) = False OrElse MRowSheet.MRows(iRow)("商品マスタNo").Value = 0 Then
            '    Continue For  '未入力行の時
            'End If

            Dim CShouhin As New HanbaikanriDialog.CShouhin()
            Dim drShouhin As DataRow = CShouhin.GetMaster(MRowSheet.MRows(mRow)("商品マスタNo").Value)
            If drShouhin IsNot Nothing Then
                CheckNewGenkaTanka(mRow, drShouhin)
            End If
        Next
    End Sub
    '原価単価が最新の仕入単価と同じかどうかチェックし、違う時はワーニングを表示する
    Private Sub CheckNewGenkaTanka(ByVal mRow As Integer, ByVal drShouhin As DataRow)
        '売上区分=値引時は、原価はチェックしない
        If Denpyou.UriageKubun.Kubun = enUriKubunKubun.Nebiki Then
            Exit Sub
        End If

        '原価単価を得る
        Dim genkatanka As CDenpyouCommon.strctZeinukiZeikomi = CDenpyouCommon.GetGenkaTanka(drShouhin("仕入単価"), MRowSheet.MRows(mRow)("商品税区分").Value, Denpyou.Tokuisaki.ZeiKubun, Denpyou.Tokuisaki.Hasuu, GetByte(drJisha("買掛単価少数桁数")), MRowSheet.MRows(mRow)("消費税率").Value, Denpyou.aryRate(MRowSheet.MRows(mRow)("消費税率区分").Value - 1))

        '商品単価が最新と違う場合はセルノートを表示
        If MRowSheet.MRows(mRow)("原価単価").Value <> genkatanka.Kingaku Then
            Dim format As String = "#,0"
            If GetByte(drJisha("買掛単価少数桁数")) > 0 Then
                format &= "." & New String("0", GetByte(drJisha("買掛単価少数桁数")))
            End If
            Dim msg As String = "登録されている原価単価は「" & genkatanka.Kingaku.ToString(format) & "」です"
            CFormCommon.SetSelNote(MRowSheet.MRows(mRow)("原価単価"), msg)
        End If
    End Sub

    '納品日をチェックし、エラーアイコンを表示（前々月の自社締日以前、本日の1ヵ月後以降はワーニング）
    Private Sub CheckNouhinDate()
        If drJisha("伝票日付チェック") = False Then Exit Sub
        CDenpyouCommon.CheckDateSetError(datNouhinDate, checkMinDate, checkMaxDate, ErrorProvider1, "納品日")
    End Sub

    '請求日をチェックし、エラーアイコンを表示（前々月の自社締日以前、本日の1ヵ月後以降はワーニング）
    Private Sub CheckSeikyuDate()
        If drJisha("伝票日付チェック") = False Then Exit Sub
        CDenpyouCommon.CheckDateSetError(datSeikyuDate, checkMinDate, checkMaxDate, ErrorProvider1, lblSeikyuDate.Text)
    End Sub

    '納品伝票更新の確認を行い、Yesなら納品伝票を登録する
    Private Function UpdateCaution() As Boolean
        If isChanged = False Then Return True

        Dim res As DialogResult
        res = MessageBox.Show("この納品伝票を登録しますか？", Me.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        Select Case res
            Case DialogResult.Yes
                If UpdateRecord() Then
                    Return True
                Else
                    Return False
                End If
            Case DialogResult.No
                Return True
            Case DialogResult.Cancel
                Return False
        End Select
        Return True
    End Function

    'コード部分のフォーカス移動の制御
    Private Sub ControlFocusCode(ByVal sender As Object)
        If sender Is edtTokuiCode AndAlso edtNounyuuCode.TabStop Then
            '得意先にカーソルがある時、納入先TabStopがTrueなら、納入先にフォーカス移動する
            edtNounyuuCode.Select()

        ElseIf (sender Is edtTokuiCode OrElse sender Is edtNounyuuCode OrElse sender Is cmbNounyuuKeisho) AndAlso edtSoukoCode.TabStop Then
            '得意先/納入先にカーソルがある時、倉庫TabStopがTrueなら、倉庫にフォーカス移動する
            edtSoukoCode.Select()

        ElseIf (sender Is edtTokuiCode OrElse sender Is edtNounyuuCode OrElse sender Is cmbNounyuuKeisho OrElse sender Is edtSoukoCode) AndAlso edtTantouCode.TabStop Then
            '得意先/納入先/倉庫にカーソルがある時、担当者TabStopがTrueなら、担当者にフォーカス移動する
            edtTantouCode.Select()

        Else
            '上記以外の時、日付にフォーカス移動する
            If drJisha("日付選択") = False Then
                datSeikyuDate.Select()
            Else
                datNouhinDate.Select()
            End If
        End If
    End Sub

    'メニューボタンの表示/非表示　設定
    '　isNewInputtable：新規伝票入力が可能な時はTrue、isChanged：伝票を変更した時True
    Private Sub ChangeButton(ByVal isNewInputtable As Boolean, ByVal isChanged As Boolean)
        Dim isEnabled As Boolean
        If Denpyou.TableNo = 0 AndAlso isChanged = False Then
            isEnabled = False  '新規伝票でまだ未入力の時
        Else
            isEnabled = True
        End If

        toolBtnNextNew.Enabled = isEnabled And isNewInputtable  '次伝票
        mnuNextNew.Enabled = isEnabled And isNewInputtable      'ファイル→同じ得意先で新規の伝票

        toolBtnDeleteRow.Enabled = isEnabled                    '行削除
        mnuDeleteRow.Enabled = isEnabled                        '編集→行削除
        toolBtnInsertRow.Enabled = isEnabled                    '行挿入
        mnuInsertRow.Enabled = isEnabled                        '編集→行挿入
        toolBtnDelete.Enabled = isEnabled                       '伝票削除
        mnuDelete.Enabled = isEnabled                           'ファイル→削除
        toolBtnPrint.Enabled = isEnabled                        '印刷
        mnuPrintMenu.Enabled = isEnabled                        '納品伝票印刷
        toolBtnPreview.Enabled = isEnabled                      'プレビュー
        toolBtnRyoushuSho.Enabled = isEnabled                   '領収書  *信和*
        toolBtnCopyNew.Enabled = isEnabled                      'コピー
        mnuCopyNew.Enabled = isEnabled                          '編集→コピー
        toolBtnTankaRireki.Enabled = isEnabled                  '単価履歴
        mnuTankaRireki.Enabled = isEnabled                      '検索→単価履歴
        toolBtnExpandMeisai.Enabled = isEnabled                 '行拡縮
        mnuExpandMeisai.Enabled = isEnabled                     '編集→行拡縮
        toolBtnExport.Enabled = isEnabled                       'エクスポート
        mnuExport.Enabled = isEnabled                           'ファイル→エクスポート

        toolBtnUpdate.Enabled = isChanged                       '登録
        mnuUpdate.Enabled = isChanged                           'ファイル→登録

        toolBtnNew.Enabled = isNewInputtable                    '新規伝票
        mnuNew.Enabled = isNewInputtable                        'ファイル→全てクリア
        toolBtnSearchCopy.Enabled = isNewInputtable             '複写入力
        mnuSearchCopy.Enabled = isNewInputtable                 '検索→複写入力
        toolBtnMitumori.Enabled = isNewInputtable               '見積参照
        mnuMitumori.Enabled = isNewInputtable                   '検索→見積参照
        toolBtnJutyu.Enabled = isNewInputtable                  '受注参照
        mnuJutyu.Enabled = isNewInputtable                      '検索→受注参照
        edtTokuiCode.Enabled = isNewInputtable                  '得意先コード
        btnSearchTokui.Enabled = isNewInputtable                '得意先検索

        If Denpyou.Tokuisaki.MasterNo = 0 Then
            toolBtnSiire.Enabled = False                        '仕入参照
            mnuSiire.Enabled = False                            '仕入参照
        Else
            toolBtnSiire.Enabled = isNewInputtable              '仕入参照
            mnuSiire.Enabled = isNewInputtable                  '仕入参照
        End If

        '日の出は、登録ボタンを青/赤に
        If My.Settings.EndUserName = "株式会社　日の出" Then
            If toolBtnUpdate.Enabled Then
                toolBtnUpdate.BackColor = Color.FromArgb(242, 0, 0)  '赤
            Else
                toolBtnUpdate.BackColor = Color.LightSkyBlue  '青
            End If
        End If
    End Sub

    'シートを、プラスのみ入力可 or マイナスも入力可に設定する
    '  引数：plus=True:プラスにする時、False:増減を掛ける時
    Private Sub SetSheetPlus(ByVal plus As Boolean)
        For mRow As Integer = 0 To MRowSheet.MaxMRows - 1
            SetSheetPlus(plus, mRow)
        Next
    End Sub
    Private Sub SetSheetPlus(ByVal plus As Boolean, ByVal mRow As Integer)
        If plus AndAlso Denpyou.UriageKubun.Code = CON_UriageCode Then  '売上の時は、マイナス入力も可とする
            Exit Sub
        End If

        '設定するシートの項目名を指定
        Dim itemList As New List(Of String) From {"セット数", "数量", "原価単価", "商品単価", "金額"}

        Dim numEditor As New GrapeCity.Win.ElTabelle.Editors.NumberEditor()
        For Each item As String In itemList
            numEditor = New GrapeCity.Win.ElTabelle.Editors.NumberEditor()
            numEditor = MRowSheet.MRows(mRow)(item).Editor
            If plus Then
                numEditor.MinValue = 0
            Else
                numEditor.MinValue = numEditor.MaxValue * -1
            End If
            MRowSheet.MRows(mRow)(item).Editor = numEditor
        Next
    End Sub

    'DBに連結しているシートのデータを、プラスのみ or 増減によりプラス/マイナスに変更する
    '  引数：plus=True:プラスにする時、False:増減を掛ける時
    Private Sub SetDBSheetPlus(ByVal plus As Boolean, ByVal mRow As Integer)
        If plus Then
            MRowSheet.MRows(mRow)("セット数").Value = Math.Abs(MRowSheet.MRows(mRow)("セット数").Value)
            MRowSheet.MRows(mRow)("数量").Value = Math.Abs(MRowSheet.MRows(mRow)("数量").Value)
            MRowSheet.MRows(mRow)("税抜金額").Value = Math.Abs(MRowSheet.MRows(mRow)("税抜金額").Value)
            MRowSheet.MRows(mRow)("税込金額").Value = Math.Abs(MRowSheet.MRows(mRow)("税込金額").Value)
            MRowSheet.MRows(mRow)("税抜原価").Value = Math.Abs(MRowSheet.MRows(mRow)("税抜原価").Value)
            MRowSheet.MRows(mRow)("消費税").Value = Math.Abs(MRowSheet.MRows(mRow)("消費税").Value)
        Else
            MRowSheet.MRows(mRow)("セット数").Value = MRowSheet.MRows(mRow)("セット数").Value * Denpyou.UriageKubun.Zougen
            MRowSheet.MRows(mRow)("数量").Value = MRowSheet.MRows(mRow)("数量").Value * Denpyou.UriageKubun.Zougen
            MRowSheet.MRows(mRow)("税抜金額").Value = MRowSheet.MRows(mRow)("税抜金額").Value * Denpyou.UriageKubun.Zougen
            MRowSheet.MRows(mRow)("税込金額").Value = MRowSheet.MRows(mRow)("税込金額").Value * Denpyou.UriageKubun.Zougen
            MRowSheet.MRows(mRow)("税抜原価").Value = MRowSheet.MRows(mRow)("税抜原価").Value * Denpyou.UriageKubun.Zougen
            MRowSheet.MRows(mRow)("消費税").Value = MRowSheet.MRows(mRow)("消費税").Value * Denpyou.UriageKubun.Zougen
        End If
    End Sub

    '明細シートにデータが入力されているかどうかを調べ、入力済の最大行数を返す
    '  （CDenpyouCommon.GetCalcGoukeiでも使用するためPublic）
    Public Function DataInSheet(ByVal MSheet As GrapeCity.Win.ElTabelle.MultiRowSheet) As Integer
        Dim maxRow As Integer = 0
        For mRow As Integer = (MSheet.MaxMRows - 1) To 0 Step -1
            If DataInSheet(mRow, MSheet) Then  'データが入力済の行の時
                maxRow = mRow + 1  '入力済明細の行数
                Exit For
            End If
        Next
        Return maxRow  '入力済明細の行数を返す（未入力ならゼロが返る）
    End Function
    '明細シートの指定の行に、データが入力されているかどうかを調べる
    Public Function DataInSheet(ByVal mRow As Integer, ByVal MSheet As GrapeCity.Win.ElTabelle.MultiRowSheet) As Boolean
        If MSheet.MRows(mRow)("商品コード").Value <> "" _
          OrElse MSheet.MRows(mRow)("商品名称").Value <> "" _
          OrElse MSheet.MRows(mRow)("商品名称カナ").Value <> "" _
          OrElse MSheet.MRows(mRow)("数量").Value <> 0 _
          OrElse MSheet.MRows(mRow)("単位IN").Text <> "" _
          OrElse MSheet.MRows(mRow)("金額").Value <> 0 _
          OrElse MSheet.MRows(mRow)("備考").Value <> "" Then
            Return True  'データ入力済
        Else
            Return False  'データ未入力
        End If
    End Function

    '新規入力・変更入力した時、更新状態とし、登録ボタン等を表示する
    Private Sub UpdateFlagOn()
        '得意先未入力時は変更しない
        If Denpyou.Tokuisaki.MasterNo <= 0 Then Exit Sub

        isChanged = True
        ChangeButton(isNewInputtable, True)
    End Sub

    '検索画面が開いていたら閉じる
    '　（自身の検索画面は閉じないようにするには、ownFormに自身のインスタンスを指定する）
    Private Sub CloseFormListIfOpend(ByRef ownForm As Form)
        Dim searchFormList As New List(Of Form) From {frmNouhinList, frmNouhinListCopy, frmMitumoriList, frmJutyuList, frmSiireList, frmJutyuListMeisai}
        For Each frm As Form In searchFormList
            If frm Is ownForm Then Continue For  '自分は除く
            If frm IsNot Nothing AndAlso frm.IsDisposed = False Then
                frm.Close()
            End If
        Next
    End Sub

    'シートを再描画しない
    Private Sub SheetRedrawOFF()
        MRowSheet.Redraw = False
        sheetGoukei.Redraw = False
        'MRowSheet.AutoCalculate = False  '数式の自動再計算を無効にする
    End Sub

    'シートを再描画する
    Private Sub SheetRedrawON()
        'MRowSheet.AutoCalculate = True  '数式の自動再計算を有効にする
        'MRowSheet.Calculate()           '合計行を再計算
        sheetGoukei.Redraw = True
        MRowSheet.Redraw = True
    End Sub

    Private Sub SetCursorWait()
        Me.Cursor = Cursors.WaitCursor
        MRowSheet.CrossCursor = Cursors.WaitCursor
        sheetGoukei.CrossCursor = Cursors.WaitCursor
    End Sub
    Private Sub SetCursorDefault()
        sheetGoukei.CrossCursor = Nothing
        MRowSheet.CrossCursor = Nothing
        Me.Cursor = Cursors.Default
    End Sub
End Class