// JScript File

function GetJsonCompatibleString(str)
{
    var regex = /\\/g;
    str = str.replace(regex, "\\\\");
    str= str.replace(new RegExp('"', "g"), '\\"');
    str= str.replace(new RegExp("'", "g"), "\\'");
    
    return str;
}

function myTrim(str) {
    return str.replace(/^\s+|\s+$/gm,'');
}

function AlertMessage(title,message,hieght,width,type)
{
    var strcolor;
      
    switch (type){
    case 'S':
        strcolor = "style='color:green;'";
        break;
    case 'D':
        strcolor = "style='color:#D9534F;'";
        break;
    case 'W':
        strcolor = "style='color:brown;'";
        break;
    default:
        strcolor = "style='color:black;'";
        break;
    }
   
    var str="<div title=" + title + " style='padding:5px;'><p " + strcolor + ">"+ message +"</p></div>";
   
    $(str).dialog({
        resizable: false,
        modal: true,
        height: hieght,
        width: width,
        buttons: {
            "Ok": function() { $(this).dialog("close"); }
        }
    });
    //return false;
}

function ConfirmDialog(title,message,hieght,width,OkFunction,CancelFunction)
{
    var str="<div title=" + title + " style='padding:5px;'><p>"+ message +"</p></div>";
    
    $(str).dialog({
        resizable: false,
        modal: true,
        height: hieght,
        width: width,
        buttons: {
            "Ok": function() {
                        if(OkFunction != null && OkFunction != undefined && OkFunction != ""){
                            OkFunction();
                        }
                        $(this).dialog("close"); 
                    },
            "Cancel":function() {
                        if(CancelFunction != null && CancelFunction != undefined && CancelFunction != ""){
                            CancelFunction();
                        }
                        $(this).dialog("close"); 
                    }   
        }
    });
    
    return false;
}
function FillComboIPO(Data,Combo,Type,StarterVal){
    var OptionHTML = '';
    if(Type == 1){
        OptionHTML = '<option value="" selected="Selected">' + StarterVal + '</option>'; 
    }
    for(var i = 0;i<Data.length;i++){
        OptionHTML += '<option value="' + Data[i].value + '">' + Data[i].text + '</option>'; 
    }
    $('#' + Combo).html(OptionHTML);    
}
function philterJSON(JSON,Value,ColName){ // ^_^ Philter....... Filter
    var Data = [];
    for(var i = 0;i<JSON.length;i++){
       if(JSON[i][ColName] == Value){
           Data.push(JSON[i]);
       } 
    }
    return Data;
}

function OnlyNumericKey(event) {
    var keycode = event.which || event.keyCode;
    //alert(keycode);
    if (keycode == 8 || keycode == 118 || keycode == 46 || (keycode >= 48 && keycode <= 57)) {
        return true;
    }
    else {
        return false;
    }
}

function OnlyStringKey(event) {
    var keycode = event.which || event.keyCode;
    //alert(keycode);
    if (keycode == 34 || keycode == 39) {
        return false;
    }
}