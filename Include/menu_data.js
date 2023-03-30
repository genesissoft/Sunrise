fixMozillaZIndex=true; //Fixes Z-Index problem  with Mozilla browsers but causes odd scrolling problem, toggle to see if it helps
_menuCloseDelay=500;
_menuOpenDelay=150;
_subOffsetTop=0;
_subOffsetLeft=-0;




with(menuStyle=new mm_style()){
bordercolor="#999999";
borderstyle="solid";
borderwidth=0;
itemwidth="2%";
fontfamily="Verdana, Tahoma, Arial";
fontsize="8";
fontstyle="normal";
headerbgcolor="#ffffff";
headercolor="#000000";
offbgcolor="#eeeeee";
offcolor="#000000";
onbgcolor="#AAB08A";
oncolor="#000099";
align="center"

//outfilter="randomdissolve(duration=0.3)";
//overfilter="Fade(duration=0.2);Alpha(opacity=90);Shadow(color=#777777', Direction=135, Strength=3)";
padding="4";
//pagebgcolor="#82B6D7";
pagecolor="black";
separatorcolor="#999999";
separatorsize=0;
}

with(submenuStyle=new mm_style()){
bordercolor="#296488";
borderstyle="solid";
borderwidth=0;
fontfamily="Verdana, Tahoma, Arial";
fontsize="10";
fontstyle="normal";
headerbgcolor="#ffffff";
headercolor="#000000";
offbgcolor="#eeeeee";
offcolor="#000000";
onbgcolor="#C4CB9E";
oncolor="#000099";
outfilter="randomdissolve(duration=0.3)";
overfilter="Fade(duration=0.2);Alpha(opacity=100);Shadow(color=#777777', Direction=135, Strength=3)";
padding=6;
pagebgcolor="#C4CB9E";
pagecolor="black";
separatorcolor="#999999";
separatorsize=1;
width=200
align="left"
subimage="../Images/arrow.gif";
    subimagepadding=2;
    menualign="center"
//subimage="http://www.networkinstruments.com/menus/arrow.gif";
//subimagepadding=2;
}

/*
with(milonic=new menuname("System")){
overflow="scroll";
style=submenuStyle;
    aI("text=User Master ;url=ViewUserMaster.aspx;");
    aI("text=User Type Master;url=ViewUserTypeMaster.aspx;");
}

with(milonic=new menuname("Maintenance Master")){
style=submenuStyle;
aI("text=Customer Type Master;url=ViewCustomerTypeMaster.aspx;");
aI("text=Customer Master;url=ViewCustomerMaster.aspx;");
//aI("text=Customer Approvals;url=CustomerApprovals.aspx;");
aI("text=Security Master;url=ViewSecurityMaster.aspx");
aI("text=Security Type Master;url=ViewSecurityTypeMaster.aspx;");
aI("text=Branch Master;url=ViewBranchmaster.aspx");
}

with(milonic=new menuname("Transaction")){
style=submenuStyle;
aI("text=Quote Entry;url=PurchaseList.aspx;");
}

with(milonic=new menuname("Reports")){
style=submenuStyle;
aI("text=Purchase And Sell List;url=PurchaseList.aspx;");
//aI("text=Security List;url=gigabit_options.html;");
//aI("text=List of Purchase Quotes;url=Obs11_NewFeatures_B.pdf;");
//aI("text=List of Selling Quotes;url=Obs11_NewFeatures_B.pdf;");
//aI("text=Fax;url=Obs11_NewFeatures_B.pdf;");
}
drawMenus();*/

