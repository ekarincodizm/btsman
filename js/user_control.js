// JScript File
var Sx=10;                 // THE "X" POSITION OF SLIDER
var Sy=20;                 // THE "Y" POSITION OF SLIDER
var Sh=110;                // SLIDER HEIGHT
var Sbg='#ffedcf';         // BACKGROUND COLOR OF SLIDER AREA
var burl='img/b1.jpg'/*tpa=http://www.codetukyang.com/java/images2/textbutton.gif*/;     // URL OF BUTTON GRAPHIC
var bw=3;                 // SLIDER GRAPHIC WIDTH
var bh=3;                 // SLIDER GRAPHIC HEIGHT
var bw2=21;                 // BUTTON GRAPHIC WIDTH
var bh2=20;                 // BUTTON GRAPHIC HEIGHT

// BELOW IS THE ARRAY CONTAINING SUB-ARRAYS OF EACH LINKS ATTRIBUTES.
// FORMAT: [ "hyperlink" , "HTML code" , "target frame/page" ]
// ADD A SUBARRAY FOR EACH LINK. DON'T FORGET THE "," AFTER EACH EXCEPT THE LAST ONE.

var SL=[ 
["http://www.textemo.cjb.net/" ,  "TEXT EMOTION" , "" ],
["http://www.geocities.com/thaifreshmilk" ,  "WEB THAI FRESH MILK" , "" ],
["http://www.nongkhaiweb.com/" ,  "NONGKHAIWEB" , "" ]
]

/****************** DO NOT EDIT BEYOND THIS POINT ****************/

var w3c=(document.getElementById)?true:false;
var ns4=(document.layers)?true:false;
var ie5=(document.all && w3c)?true:false;
var ie4=(document.all && !w3c)?true:false;
var ns6=(w3c && navigator.appName.indexOf("Netscape")>=0)?true:false;
var isDrag=false;
var sliderButton;
var yo=0;
var lids=new Array();
lids.spc=Sh/SL.length+5;
lids.yc=new Array();

var t='';
t+=(ns4)?'<layer top='+Sy+' left='+Sx+' height='+Sh+' width='+(bw+2)+' bgcolor=black>':'<div style="position:absolute; top:'+Sy+'px; left:'+Sx+'px; height:'+Sh+'px; width:'+(bw+2)+'px; background-color:black;">';
//t+='<table cellpadding=0 cellspacing=0 border=1 height='+Sh+' width='+(bw+2)+' '+((ns4)?'bordercolor="black"' : 'style="border-style:solid; border-color:black"')+'><tr><td></td></tr></table>';
t+=(ns4)?'<layer  height='+(Sh-2)+' width=100 left=1 top=1 bgcolor="'+Sbg+'"></layer>':'<div style="position:absolute; height:'+(Sh-2)+'px; width:'+bw+'px; top:1px; left:1px; background-color:'+Sbg+'"></div>';
t+=(ns4)?'<layer top='+(bh/2)+' left='+(bw/2+1)+' height='+(Sh-bh)+' width=1 bgcolor=black></layer>':'<div style="position:absolute; top:'+(bh/2)+'px; left:'+(bw/2+1)+'px; height:'+(Sh-bh)+'px; width:1px; background-color:black"></div>';
t+=(ns4)?'<layer name="sliderB" top=1 left=1 height='+bh+' width='+bw+'>':'<div id="sliderB" style="position:absolute; top:1px; left:-7px; height:'+bh+'px; width:'+bw+'px; cursor:hand">';
t+='<img src="'+burl+'" width='+bw2+' height='+bh2+'>';
t+=(ns4)?'</layer></layer>':'</div></div>';
for(i=0;i<SL.length;i++){
lids.yc[i]=5+Sy+(i*lids.spc);
t+=(ns4)?'<layer name="Lnk_'+i+'" top='+lids.yc[i]+' left='+(bw+6+Sx)+' visibility="hidden">':'<div id="Lnk_'+i+'" style="position:absolute; top:'+lids.yc[i]+'px; left:'+(bw+6+Sx)+'px; visibility:hidden">';
t+='<a href="'+SL[i][0]+'" target="'+SL[i][2]+'">'+SL[i][1]+'</a>';
t+=(ns4)?'</layer>':'</div>';
}
document.write(t);

function checklinks(){
var y;
for(i=0;i<lids.length;i++){
y=(ns4)?sliderButton.top:parseInt(sliderButton.style.top);
y2=lids.yc[i]-Sy;
if((y>y2-bh) && (y<y2+bh)) (ns4)?lids[i].visibility="show":lids[i].style.visibility="visible";
else (ns4)?lids[i].visibility="hide":lids[i].style.visibility="hidden";
}}

function getid(id){
if(ns4) return findlayer(id,document);
else if(ie4)return document.all[id];
else return document.getElementById(id);
}

// FUNCTION TO FIND NESTED LAYERS IN NS4 BY MIKE HALL
function findlayer(name,doc){
var i,layer;
for(i=0;i<doc.layers.length;i++){
layer=doc.layers[i];
if(layer.name==name)return layer;
if(layer.document.layers.length>0)if((layer=findlayer(name,layer.document))!=null)return layer;
}
return null;
}

function trackMouse(e){
if(isDrag){
var y=(ie4||ie5)?event.clientY+document.body.scrollTop:e.pageY;
y=Math.max(1,Math.min(y-yo,Sh-bh-1));
if(ns4)sliderButton.top=y;
else sliderButton.style.top=y+'px';
}
// ADD OTHER MOUSEMOVE EVENT HANDLER COMMAND(S) HERE...

return false;
}

function dragInit(e){
isDrag=true;
var ty=(ns4)? sliderButton.top : parseInt(sliderButton.style.top);
yo=((ie4||ie5)?event.clientY+document.body.scrollTop:e.pageY)-ty;
return false;
}


function SBinit(){
sliderButton=getid('sliderB');
for(i=0;i<SL.length;i++)lids[i]=getid('Lnk_'+i);
if(ns4){
sliderButton.captureEvents(Event.MOUSEDOWN | Event.MOUSEUP);
document.captureEvents(Event.MOUSEMOVE);
}
sliderButton.onmousedown=dragInit;
if(ns6)document.onmouseup=function(){
isDrag=false;
// IF ANOTHER SCRIPT NEEDS TO CAPTURE THE MOUSEUP EVENT, ADD THE COMMAND(S) HERE...

}
else sliderButton.onmouseup=new Function("isDrag=false");
// IF ANOTHER SCRIPT NEEDS TO CAPTURE THE MOUSEMOVE EVENT, SEE THE trackmouse() FUNCTION. 
document.onmousemove=trackMouse;
setInterval('checklinks()',100);
}

window.onresize=function(){
if(ns4)setTimeout('history.go(0)',300);
}



