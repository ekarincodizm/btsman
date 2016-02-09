// JScript File

function setVal(obj, v)
{
  document.getElementById(obj).value = v;
}

function doSubmit(strUrl)
{
  document.getElementById("form1").submit();
}

function checkEmptyText(obj)
{
    if (document.getElementById(obj).value=='') {
		alert('Please fill in required data.')
		return false;
	}
    return true;
}

function checkNumber() {
var e_k=event.keyCode
//if (((e_k < 48) || (e_k > 57)) && e_k != 46 ) {
if (e_k != 13 && (e_k < 48) || (e_k > 57)) {
event.returnValue = false;
alert("Please enter numerical value");
}
}

function ajaxPost(strURL,qStr,respDiv) {
    ajaxPost(strURL, qStr, true);
}

function ajaxPost(strURL,qStr,respDiv,async) {
    var xmlHttpReq = false;
    var self = this;
    // Mozilla/Safari
    if (window.XMLHttpRequest) {
        self.xmlHttpReq = new XMLHttpRequest();
    }
    // IE
    else if (window.ActiveXObject) {
        self.xmlHttpReq = new ActiveXObject("Microsoft.XMLHTTP");
    }
    self.xmlHttpReq.open('POST', strURL, async);
    self.xmlHttpReq.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    self.xmlHttpReq.onreadystatechange = function() { ajaxCallBack(self.xmlHttpReq, respDiv); }
    self.xmlHttpReq.send(qStr);
}

function ajaxCallBack(xmlHttpReq, respDiv) {

    if ((xmlHttpReq.readyState == 4) && (respDiv != null)) {
        ajaxWriteResp(respDiv, xmlHttpReq.responseText);
    }

}

function ajaxWriteResp(respDiv, str){
    document.getElementById(respDiv).innerHTML = str;
}
/*
function mouseX(evt) {
    if (evt.pageX) return evt.pageX;
    else if (evt.clientX)
       return evt.clientX + (document.documentElement.scrollLeft ?
       document.documentElement.scrollLeft :
       document.body.scrollLeft);
    else return null;
}

function mouseY(evt) {
    if (evt.pageY) return evt.pageY;
    else if (evt.clientY)
       return evt.clientY + (document.documentElement.scrollTop ?
       document.documentElement.scrollTop :
       document.body.scrollTop);
    else return null;
}
*/
function mouseX(evt) {return evt.clientX ? evt.clientX + (document.documentElement.scrollLeft || document.body.scrollLeft) : evt.pageX;}
function mouseY(evt) {return evt.clientY ? evt.clientY + (document.documentElement.scrollTop || document.body.scrollTop) : evt.pageY;}

function mouseMove(e){
    /*
    x=(document.all)?event.x:e.pageX;
    y=(document.all)?event.y:e.pageY;
    x=mouseX(e);
    y=mouseY(e);
    alert(x+","+y);
    */
   if (IE) { // grab the x-y pos.s if browser is IE
    x = event.clientX + document.documentElement.scrollLeft
    y = event.clientY + document.documentElement.scrollTop
  } else {  // grab the x-y pos.s if browser is NS
    x = e.pageX
    y = e.pageY
  }  
  // catch possible negative values in NS4
  if (x < 0){x = 0}
  if (y < 0){y = 0}  
    
//    eval('document.getElementById(\''+layerName+'\').style.top='+(y+offsetY));
//    eval('document.getElementById(\''+layerName+'\').style.left='+(x+offsetX));

}

function isInt(x) {
    var y = parseInt(x);
    if (isNaN(y)) return false;
    return x == y && x.toString() == y.toString();
}


// modal popup
function closePopup()  
{  
    document.getElementById("divSignin").style.display = "none";
    document.getElementById("divSelectBranch").style.display = "none";
 /*      objDiv = document.getElementById("divg");  
       objDiv.style.display = "none";   */
       return false;  
}  

function showPopup()  
{  
    try {
                document.getElementById("divSignin").style.display = "block"; 
                document.getElementById("divSelectBranch").style.display = "block";
        /*
                objDiv = document.getElementById("divg");  
                objDiv.style.display = "block";   
                objDiv.style.width = document.body.scrollWidth;  
                objDiv.style.height= document.body.scrollHeight;           
                fnSetDivSigninLeft("divSignin");       */
    }
    catch (e) {
        alert(e);
    }
    return false
}  

function fnSetDivSigninLeft(oElement)  
{  
   var DivWidth = parseInt(document.getElementById(oElement).offsetWidth,10)  
   var DivHeight = parseInt(document.getElementById(oElement).offsetHeight,10)  
//   document.getElementById(oElement).style.left = (document.body.offsetWidth / 2) - (DivWidth / 2)+200;
//   document.getElementById(oElement).style.top = 400; // (document.body.offsetHeight / 2) -  ( DivHeight / 2);  

  return false;
}  

// end modal popup