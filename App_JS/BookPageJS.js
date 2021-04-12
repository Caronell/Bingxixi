var t;

function preTimer()
{
    t = setTimeout(hideZoom, 100);
}

function stopTimer()
{
    clearTimeout(t);
}

function moveZoomPic(e)
{
    clearTimeout(t);
    var bigimg = document.getElementById("bigPic");
    var zoom = document.getElementById("zoom_pup");
    var oe = e || event;
    var left = zoom.offsetLeft + oe.offsetX - 80;
    var top = zoom.offsetTop + oe.offsetY - 80;
    if (left < 0)
        left = 0;
    else if (left > 160)
        left = 160;
    if (top < 0)
        top = 0;
    else if (top > 160)
        top = 160;
    zoom.style.left = left + "px";
    zoom.style.top = top + "px";
    bigimg.style.left = "-" + left * 2.5 + "px";
    bigimg.style.top = "-" + top * 2.5 + "px";
}

function showZoom(e)
{
    clearTimeout(t);
    document.getElementById("bigPicBox").style.display = "block";
    var zoom = document.getElementById("zoom_pup");
    if (zoom.style.display != "block")
    {
        var oe = e || event;
        var left = oe.offsetX - 80;
        if (left < 0)
            left = 0;
        if (left > 160)
            left = 160
        var top = oe.offsetY - 80;
        if (top < 0)
            top = 0;
        if (top > 160)
            top = 160
        zoom.style.left = left + "px";
        zoom.style.top = top + "px";
        zoom.style.display = "block";
    }
}

function hideZoom()
{
    document.getElementById("zoom_pup").style.display = "none";
    document.getElementById("bigPicBox").style.display = "none";
}

function addOneBook()
{
    var num = document.getElementById("buyNumber").value - 0;
    if (num + 1 <= document.getElementById("remainBookNum").value - 0)
    {
        num += 1;
        document.getElementById("buyNumber").value = num;
    }
}

function removeOneBook()
{
    var num = document.getElementById("buyNumber").value - 0;
    num -= 1;
    if (num < 1)
        num = 1;
    document.getElementById("buyNumber").value = num;
}

function checkzero()
{
    var num = document.getElementById("buyNumber").value - 0;
    if (num < 1)
    {
        document.getElementById("buyNumber").value = 1;
    }
    else if (num > document.getElementById("remainBookNum").value - 0)
    {
        num = document.getElementById("remainBookNum").value - 0;
        document.getElementById("buyNumber").value = num;
    }
}