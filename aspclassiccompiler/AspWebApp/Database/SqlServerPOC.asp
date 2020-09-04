<% @ LANGUAGE=VBScript	%>
<% Option Explicit		%>
<HTML>

<%

	Dim oConn		
	Dim oRs		
	Dim strConn



	' Create ADO Connection Object.  Use OLEDB Source with 
	' default sa account and password

	Set oConn = Server.CreateObject("ADODB.Connection")
	strConn="Provider=SQLOLEDB;User ID=sa;Initial Catalog=pubs;Data Source="& Request.ServerVariables("SERVER_NAME")
    oConn.Open strConn
	Set oRs = oConn.Execute("SELECT top 1 * FROM authors")

%>

		<TABLE border = 1>
		<%  
			Do while (Not oRs.eof) %>

				<tr>
					<% For Index=0 to (oRs.fields.count-1) %>
						<TD VAlign=top><%= oRs(Index)%></TD>
					<% Next %>
				</tr>
            
				<% oRs.MoveNext 
			Loop 
		%>


		</TABLE>
		<%   
			oRs.close
			oConn.close 
		%>
dsafds
