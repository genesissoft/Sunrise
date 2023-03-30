Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Text
Imports log4net

Public Class Util
    Public Sub WritErrorLog(ByVal className As String, ByVal functionName As String, ByVal msg As String, Optional ByVal stateVars As String = "", Optional ByVal ex As Exception = Nothing)
        Dim logger As ILog = log4net.LogManager.GetLogger(GetType(Util))
        logger.[Error]([String].Format("{0} : {1}METHOD={2}&MSG={3}&{4}{5},Error Trace:{6}", className, "end", functionName, msg, stateVars, _
         "end2", ex.ToString()))
    End Sub

    Public Sub WritInfoLog(ByVal className As String, ByVal functionName As String, ByVal msg As String, Optional ByVal stateVars As String = "")
        Dim logger As ILog = log4net.LogManager.GetLogger(GetType(Util))
        logger.Info([String].Format("{0} : {1}METHOD={2}&MSG={3}&{4}{5}", className, "end", functionName, msg, stateVars, _
         "end2"))
    End Sub
End Class
