<%@ Page Language="VB" %>

<%
    Session.Clear()
    Session.RemoveAll()
    Session.Abandon()
    Session("UserId") = ""
    Session("UserName") = ""
    Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1))
    Response.Cache.SetCacheability(HttpCacheability.NoCache)
    Response.Cache.SetNoStore()
    Response.Redirect("Login.aspx?logout=true", False)
%>

<div style="text-align: center; vertical-align: middle; padding-top: 100px;">
    <div style="font-family: Calibri; font-size: 2em; color: green;">
        You have successfully logged out from eInstadeal portal.<br />
        <a href="SelectYear.aspx" target="_self" style="font-size: 0.8em !important; text-decoration: none;">Login Again</a>
    </div>
</div>
