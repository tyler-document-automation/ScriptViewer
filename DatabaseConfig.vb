Imports System.Configuration

Public Module DatabaseConfig

    Public ReadOnly Property ConnectionString As String
        Get
            Dim connectionName As String

#If DEBUG Then
            connectionName = "ScriptViewerTest"
#Else
            connectionName = "ScriptViewerProd"
#End If

            Dim setting = ConfigurationManager.ConnectionStrings(connectionName)

            If setting Is Nothing Then
                Throw New ConfigurationErrorsException(
                    "Connection string '" &
                    connectionName &
                    "' was not found."
                )
            End If

            Return setting.ConnectionString
        End Get
    End Property

    Public ReadOnly Property EnvironmentName As String
        Get
#If DEBUG Then
            Return "TEST"
#Else
            Return "PRODUCTION"
#End If
        End Get
    End Property

End Module