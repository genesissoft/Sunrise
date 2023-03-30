
function Show(e,StrHTML)
{
    var div = document.getElementById("a_Tooltip");  
    if(div.style.display == "none")div.style.display="block";
    div.style.position = "absolute";   
    var offset = $(e).offset();
    
    div.style.left=offset.left + 20;
    div.style.top=offset.top - 60;
    div.focus();
    div.innerHTML=StrHTML
   
    
}
function Hide()
{
    var div = document.getElementById("a_Tooltip");
    div.innerHTML="";
    if(div.style.display=="block")div.style.display="none";
}