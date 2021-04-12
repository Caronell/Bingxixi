function Trans()
{
    $(document).ready(function ()
    {
        $("#trans").fadeOut(1500);
    });

    $(document).ready(function ()
    {
        $("#mainbox").animate({ margin: "10% 10% 0 65%"}, 1200);
    });

}

function ChangePanel(obj)
{
    obj.style.backgroundColor = "rgba(247,244,237,1)";
    obj.style.borderBottom = "none";
    if (obj.id == "regstbtn")
    {
        document.getElementById("UserLoginPanel_Shade").style.display = "none";
        $(document).ready(function ()
        {
            $("#mainbox").animate({ height: "500px", width: "35%", margin: "5% 10% 0 62%" }, 500);
            $("#UserLoginPanel").animate({ height: "468px" }, 500, function ()
            {
                document.getElementById("UserLoginPanel").style.display = "none";
                document.getElementById("RegisterPanel").style.display = "block";
                document.getElementById("RegisterPanel_Shade").style.display = "block";
            });
        });
        document.getElementById("loginbtn").style.backgroundColor = "transparent";
        document.getElementById("loginbtn").style.borderBottom = "solid 1px darkgray";
        ValidatorEnable(RequiredFieldValidator1, true);
        document.getElementById("RequiredFieldValidator1").style.visibility = "hidden";
        ValidatorEnable(RequiredFieldValidator2, true);
        document.getElementById("RequiredFieldValidator2").style.visibility = "hidden";
        ValidatorEnable(RequiredFieldValidator3, true);
        document.getElementById("RequiredFieldValidator3").style.visibility = "hidden";
        ValidatorEnable(RequiredFieldValidator4, true);
        document.getElementById("RequiredFieldValidator4").style.visibility = "hidden";

    }
    else
    {
        document.getElementById("RegisterPanel_Shade").style.display = "none";
        $(document).ready(function ()
        {
            $("#mainbox").animate({ height: "332px", width: "30%", margin: "10% 10% 0 65%" }, 500);
            $("#UserLoginPanel").animate({ height: "300px" }, 500, function ()
            {
                document.getElementById("RegisterPanel").style.display = "none";
                document.getElementById("UserLoginPanel").style.display = "block";
                document.getElementById("UserLoginPanel_Shade").style.display = "block";
            });
        });
        document.getElementById("regstbtn").style.backgroundColor = "transparent";
        document.getElementById("regstbtn").style.borderBottom = "solid 1px darkgray";
        ValidatorEnable(RequiredFieldValidator1, false);
        ValidatorEnable(RequiredFieldValidator2, false);
        ValidatorEnable(RequiredFieldValidator3, false);
        ValidatorEnable(RequiredFieldValidator4, false);
    }
}

function activeVisible(index)
{
    document.getElementById("RequiredFieldValidator" + index).style.visibility = "visible";
}

function recordValidatorState()
{
    document.getElementById("passwordState").value = document.getElementById("PwdValidator").style.display;
    document.getElementById("phonenumState").value = document.getElementById("PhoneNumValidator").style.display;
    document.getElementById("IDnumState").value = document.getElementById("IDnumberValidator").style.display;
}