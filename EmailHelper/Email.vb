Imports System.Net
Imports System.Net.Mail

Public Class Email
#Region " Private Members "
    Private _host As String = String.Empty
    Private _port As Integer = 0
    Private _username As String = String.Empty
    Private _password As String = String.Empty
    Private _to As String = String.Empty
    Private _from As String = String.Empty
    Private _subject As String = String.Empty
    Private _body As String = String.Empty
#End Region

#Region " Public Properties "
    Public Property Host() As String
        Get
            Return _host
        End Get
        Set(ByVal value As String)
            _host = value
        End Set
    End Property

    Public Property Port() As Integer
        Get
            Return _port
        End Get
        Set(ByVal value As Integer)
            _port = value
        End Set
    End Property

    Public Property Username() As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public Property [To]() As String
        Get
            Return _to
        End Get
        Set(ByVal value As String)
            _to = value
        End Set
    End Property

    Public Property From() As String
        Get
            Return _from
        End Get
        Set(ByVal value As String)
            _from = value
        End Set
    End Property

    Public Property Subject() As String
        Get
            Return _subject
        End Get
        Set(ByVal value As String)
            _subject = value
        End Set
    End Property

    Public Property Body() As String
        Get
            Return _body
        End Get
        Set(ByVal value As String)
            _body = value
        End Set
    End Property

#End Region

#Region " Public Methods "
    Public Function Send() As Boolean
        Dim result As Boolean = True
        Try
            Dim smtp As New SmtpClient(_host, _port)
            Dim credentials As New NetworkCredential(_username, _password)
            smtp.Credentials = credentials
            smtp.EnableSsl = True

            Dim message As New MailMessage(_from, _to, _subject, _body)

            smtp.Send(message)
        Catch ex As Exception
            result = False
        End Try
        Return result

    End Function
#End Region
End Class
