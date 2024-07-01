

namespace Infraesctructure.Data
{
    public static  class CallIsInQueuesSQL
    {


        private static string query=string.Empty;


        public static string Query(string queryParams="")
        {
            query = @"
                  SELECT
            Sc.SupportCallID,
            Sc.Number,
            CASE
                WHEN Sc.SupportCallType = 'I' THEN 'Incident'
                WHEN Sc.SupportCallType = 'R' THEN 'Request For Change'
                WHEN Sc.SupportCallType = 'S' THEN 'Service Request'
                ELSE 'NO DEFINIDO'
            END AS Types,
            Sc.Summary,
            Que.Name AS [Queue],
            Status.Name AS [Status],
            DATEADD(HH, -6, Sc.OpenDate) AS OpenDate,
            DATEADD(HH, -6, Scs.DueDateBySla) AS DueDate,
            -- Obtener la fecha inicial (StartDate)
            (SELECT MIN(DATEADD(HH, -6, StartDate))
             FROM SupportCallAssignToHistory AS SCATH
             WHERE SCATH.SupportCallID = Sc.SupportCallID) AS StartDate,
            -- Obtener la fecha final (EndDate)
            (SELECT MAX(DATEADD(HH, -6, StartDate))
             FROM SupportCallAssignToHistory AS SCATH
             WHERE SCATH.SupportCallID = Sc.SupportCallID) AS DateAssignedTo,
            Pri.Description AS [Priority],
            ISNULL(Ac.LabelText,'N/A') AS Attribute,
            ISNULL(Av.ValueShortText,'N/A') AS Value,
            -- Subconsulta para obtener el último Summary de SupportCallEvent
            (SELECT TOP 1 Summary
             FROM SupportCallEvent
             WHERE SupportCallID = Sc.SupportCallID
             ORDER BY LastChangeDate DESC) AS EventSummary
        FROM
            SupportCall AS Sc WITH (NOLOCK)
        LEFT JOIN
            Queue AS Que WITH (NOLOCK) ON Que.QueueID = Sc.AssignToQueueID
        LEFT JOIN
            SupportCallStatus AS Status WITH (NOLOCK) ON Status.SupportCallStatusID = Sc.StatusID
        LEFT JOIN
            SupportCallSLAStatus AS Scs ON Scs.SupportCallID = Sc.SupportCallID
        LEFT JOIN
            ValueListEntry AS Pri ON Pri.ValueListEntryID = SC.PriorityID
        LEFT JOIN
            AttributeValue AS Av ON Av.ParentID = Sc.SupportCallID
        LEFT JOIN
            AttributeBoundColumn AS Abc ON Abc.AttributeBoundColumnID = Av.AttributeBoundColumnID
        LEFT JOIN
            AttributeColumn AS Ac ON Ac.AttributeColumnID = Abc.AttributeColumnID
        WHERE
            Sc.Closed = 0
            AND Sc.AssignToQueueID IS NOT NULL
                     ";

            return query;
        }

        public static string QueuesQuey()
        {
            query = @"
             SELECT 
			QueueID,
			Name 
			FROM Queue
			WHERE NAME 
			In('Administrativo - Soporte Noreste',
			'Administrativo - Soporte Oficinas',
			'Administrativo - Soporte Sucursales',
			'Administrativo - Telecom',
			'Gerencia Administrativa')
                    ";

            return query;
        }

    }
}
