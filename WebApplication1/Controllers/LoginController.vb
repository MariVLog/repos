Imports System.Web.Mvc

Namespace Controllers
    Public Class LoginController
        Inherits Controller

        ' GET: Login
        Function Index() As ActionResult
            Return View()
        End Function
    End Class
End Namespace