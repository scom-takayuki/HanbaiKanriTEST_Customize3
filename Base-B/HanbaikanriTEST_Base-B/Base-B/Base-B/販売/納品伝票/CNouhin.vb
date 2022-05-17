''' <summary>
''' 納品伝票呼び出しのコントロールクラス
''' 　　標準伝票か、指定伝票かを判断し、それぞれの伝票入力処理を呼ぶ
''' </summary>
''' <remarks>使用時は各会社用に分岐し、指定伝票の内容を修正して使用すること</remarks>
Public Class CNouhin
    'AAAA
    'プロパティ
    Private Shared mCNouhinInstance As CNouhin
    Private Shared mTokuisakiNo As Integer
    Private Shared mTableNo As Integer
    Private Shared mReference As Boolean
    Private Shared mJutyuNo As Integer
    Private Shared mMitumoriNo As Integer
    Private Shared mEdited As Boolean

    Private Enum enDenpyouID
        標準伝票 = 0
        指定伝票 = 1
    End Enum

    Private Shared iDenpyouID As Integer  '伝票の種類

    'CNouhinオブジェクトを保持するためのフィールド（frmNouhinからCNouhinをNewせずに使用するため）
    Public Shared Property CNouhinInstance() As CNouhin
        Get
            Return mCNouhinInstance
        End Get
        Set(ByVal Value As CNouhin)
            mCNouhinInstance = Value
        End Set
    End Property

    '得意先マスタNo
    Public Property TokuisakiNo() As Integer
        Get
            Return mTokuisakiNo
        End Get
        Set(ByVal Value As Integer)
            mTokuisakiNo = Value
        End Set
    End Property

    '納品伝票No
    Public Property TableNo() As Integer
        Get
            Return mTableNo
        End Get
        Set(ByVal Value As Integer)
            mTableNo = Value
        End Set
    End Property

    '複写入力かどうか（True:複写入力）
    Public Property Reference() As Boolean
        Get
            Return mReference
        End Get
        Set(ByVal Value As Boolean)
            mReference = Value
        End Set
    End Property

    '受注伝票No
    Public Property JutyuNo() As Integer
        Get
            Return mJutyuNo
        End Get
        Set(ByVal Value As Integer)
            mJutyuNo = Value
        End Set
    End Property

    '見積書No
    Public Property MitumoriNo() As Integer
        Get
            Return mMitumoriNo
        End Get
        Set(ByVal Value As Integer)
            mMitumoriNo = Value
        End Set
    End Property

    '修正画面表示後、修正したかどうか（True:修正、False:未修正）
    Public Property Edited() As Boolean
        Get
            Return mEdited
        End Get
        Set(ByVal Value As Boolean)
            mEdited = Value
        End Set
    End Property

    'コンストラクタ
    Public Sub New()
        CNouhin.CNouhinInstance = Me

        mTableNo = 0
        mTokuisakiNo = 0
        mReference = False
        mJutyuNo = 0
        mMitumoriNo = 0
        iDenpyouID = enDenpyouID.標準伝票
    End Sub

    '納品伝票入力画面の表示
    Public Sub NouhinDenpyouInput()
        Dim frmNouhin As New frmNouhin
        AddHandler frmNouhin.Closed, AddressOf frmNouhin_Closed  '納品伝票画面が閉じられた時のイベントハンドラを登録
        frmNouhin.DenpyouInput()  'モードレスで表示となる
    End Sub

    '納品伝票修正画面の表示
    Public Sub NouhinDenpyouEdit(ByVal TableNo As Integer)
        Dim frmNouhin As New frmNouhin
        '納品伝票修正画面
        Dim ret As Boolean
        ret = frmNouhin.Edit(TableNo)  'モーダルで表示となる
        If ret = False AndAlso frmNouhin.DialogResult = DialogResult.OK Then
            '指定伝票の時、指定伝票の修正画面を表示
            frmNouhin.Dispose()
            Select Case iDenpyouID
                Case enDenpyouID.指定伝票
                    '*** ここを各指定伝票用の処理に修正する ***
                    'Dim frmNouhinSitei As New frmNouhinSitei
                    'mEdited = frmNouhinSitei.Edit(TableNo)    '修正画面表示後、修正したかどうかの結果をEditedにセット
            End Select
        Else
            frmNouhin.Dispose()
            mEdited = ret    '修正画面表示後、修正したかどうかの結果をEditedにセット
        End If
    End Sub

    '納品伝票修正画面の表示（受注明細表から呼ぶ時）
    Public Sub NouhinDenpyouNounyu(ByVal TableNo As Integer)
        '納品伝票修正画面
        Dim frmNouhin As New frmNouhin
        Dim ret As Boolean
        ret = frmNouhin.Nounyu(TableNo)   'モーダルで表示となる
        If ret = False AndAlso frmNouhin.DialogResult = DialogResult.OK Then
            '指定伝票の時、指定伝票の修正画面を表示
            frmNouhin.Dispose()
            Select Case iDenpyouID
                Case enDenpyouID.指定伝票
                    '*** ここを各指定伝票用の処理に修正する ***
                    'Dim frmNouhinSitei As New frmNouhinSitei
                    'mEdited = frmNouhinSitei.Edit(TableNo)    '修正画面表示後、修正したかどうかの結果をEditedにセット
            End Select
        Else
            frmNouhin.Dispose()
            mEdited = ret    '修正画面表示後、修正したかどうかの結果をEditedにセット
        End If
    End Sub

    '指定伝票かどうかの判定
    '  戻り値　True:指定伝票、False:標準伝票
    Public Function ChkSiteiDenpyou(ByVal FormName As String) As Boolean
        '*** ここを各指定伝票用の処理に修正する ***
        'If FormName.IndexOf("指定伝票") >= 0 Then
        '    iDenpyouID = enDenpyouID.指定伝票
        '    Return True    '指定伝票
        'Else
        '    iDenpyouID = enDenpyouID.標準伝票
        '    Return False    '標準伝票
        'End If

        Return False    '標準伝票
    End Function

    '納品伝票画面を閉じた時に発生するイベント
    '  指定伝票画面を表示するために閉じられたかどうか、結果を取得し判断する
    '  （指定伝票表示時は、DialogResult.OKで閉じられる）
    Private Sub frmNouhin_Closed(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim frmNouhinResult As frmNouhin = CType(sender, frmNouhin)    '納品伝票を閉じた時の結果を取得

        '指定伝票の時、指定伝票画面を表示する
        If frmNouhinResult.DialogResult = DialogResult.OK Then
            Select Case iDenpyouID
                Case enDenpyouID.指定伝票
                    '*** ここを各指定伝票用の処理に修正する ***
                    'Dim frmNouhinSitei As New frmNouhinSitei
                    'AddHandler frmNouhinSitei.Closed, AddressOf frmNouhinSitei_Closed    '指定伝票登録画面が閉じられた時のイベントハンドラを登録
                    'If mTableNo = 0 AndAlso mJutyuNo = 0 AndAlso mMitumoriNo = 0 Then
                    '    frmNouhinSitei.DenpyouInput(mTokuisakiNo)    '得意先を指定し、指定伝票登録の新規入力画面を表示
                    'Else
                    '    frmNouhinSitei.FindEdit(mTableNo, mReference, mJutyuNo, mMitumoriNo)    '納品伝票No又は受注参照時の受注伝票No又は見積参照時の見積書Noを指定し、指定伝票登録の修正or複写or受注参照or見積参照画面を表示
                    'End If
            End Select
        End If
    End Sub

    '*** ここを各指定伝票用の処理に修正する。frmNouhinにFindEdit処理を入れる ***
    ''指定伝票画面を閉じた時に発生するイベント
    ''  納品伝票画面を表示するために閉じられたかどうか、結果を取得し判断する
    ''  （納品伝票表示時は、DialogResult.OKで閉じられる）
    'Private Sub frmNouhinSitei_Closed(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim frmNouhinSiteiResult As frmNouhinSitei = CType(sender, frmNouhinSitei)    '納品伝票を閉じた時の結果を取得

    '    '標準伝票の時、標準伝票画面を表示する
    '    If frmNouhinSiteiResult.DialogResult = Windows.Forms.DialogResult.OK Then
    '        Select Case iDenpyouID
    '            Case enDenpyouID.標準伝票
    '                Dim frmNouhin As New frmNouhin
    '                AddHandler frmNouhin.Closed, AddressOf frmNouhin_Closed    '納品伝票画面が閉じられた時のイベントハンドラを登録
    '                If mTableNo = 0 AndAlso mJutyuNo = 0 AndAlso mMitumoriNo = 0 Then
    '                    frmNouhin.DenpyouInput(mTokuisakiNo)    '得意先を指定し、納品伝票登録の新規入力画面を表示
    '                Else
    '                    frmNouhin.FindEdit(mTableNo, mReference, mJutyuNo, mMitumoriNo)    '納品伝票No又は受注参照時の受注伝票No又は見積参照時の見積書Noを指定し、納品伝票登録の修正or複写or受注参照or見積参照画面を表示
    '                End If
    '        End Select
    '    End If
    'End Sub
End Class