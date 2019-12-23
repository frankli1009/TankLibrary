/* change the row to active state and refresh the image and description when clicked excluding first two column  */
$('#tanklist').on('click', '.clickable-row', function (event) {
    //console.log(event.target);
    //console.log(this);
    var e = event || window.event,
            elm = e.target || e.srcElement,
            allTDs = this.getElementsByTagName('td');

    while (elm.nodeName.toLowerCase() !== 'td' && elm !== this) {
        elm = elm.parentNode; // get the right element if it's not the td already
    }
    if (elm === allTDs[0] || elm === allTDs[1]) {
        return true;
    }

    $(this).addClass('active').siblings().removeClass('active');
    $('#tanklist').attr("data-fl-index", $(this).data("fl-index"));

    //console.log("before check visible of imganddesc_div");
    if (!$('#imganddesc_div').is(":visible")) {
        return true;
    }

    // refresh image and description when imganddesc_div is visible
    var url = $(this).data("fl-url");
    //alert(url);
    $.ajax({
        url: url,
        type: 'GET',
        cache: false,
        success: function (result) {
            //alert("success");
            $('#imganddesc_div').html(result);
        }
    });
    return true;
});

//due to no authorization on db and code first, add own form validation here
//valid name input to limit its length equal to or less than 40
function validateForm(event) {
    event = event || window.event || event.srcElement;
    var tankName = $('#Name').val();
    
    var $NameError = $('#NameError');
    if (tankName.length > 40) {
        $NameError.text("The maximum length of Name is 40.");
        $NameError.addClass("field_validation-error");
        $NameError.removeClass("field-validation-valid");
        $NameError.show();
        $("#Name").addClass("input-validation-error-custom");
        $("#Name").focus();
        event.preventDefault();
        return false;
    }
    //let DataAnnotations to check length equal to 0
    //else if (tankName.length === 0) {
    //    $NameError.text("Name field is required.");
    //    $NameError.addClass("field_validation-error");
    //    $NameError.removeClass("field-validation-valid");
    //    $NameError.show();
    //    $("#Name").addClass("input-validation-error-custom");
    //    event.preventDefault();
    //    return false;
    //}
    else {
        $("#Name").removeClass("input-validation-error-custom");
        $NameError.removeClass("field_validation-error");
        $NameError.addClass("field-validation-valid");
        $NameError.hide();
    }
}

$("#Name").on("change keydown paste input", function (e) {
    e = e || window.event || e.srcElement;
    switch (e.keyCode) {
        case 8: //backspace
        case 127: //delete
            return true;
            break;
        default:
            break;
    }
    console.log(e.keyCode);
    return validateForm(e);
});

//use tankimg to trigger and  get the input of the click event of file input element
$('#tankimg').on('click', function () {
    $('#uploadimgfile').click();
});

//refresh tankimg and set the value of the image path and filename hidden element after file input
$('#uploadimgfile').on('change', function () {
    var input = this;
    var sFilePathName = $(this).val();
    var sFileName = sFilePathName.substring(sFilePathName.lastIndexOf('\\') + 1);
    var sExt = sFileName.substring(sFileName.lastIndexOf('.'));
    sFileName = sFileName.substring(0, sFileName.lastIndexOf('.'));

    //create file name by current time and a random number to keep it unique
    var now = new Date();
    var iRandom = Math.ceil(Math.random() * 1000);
    sFileName += now.yyyymmddhhnnssms();
    sFileName += ('_' + iRandom);
    sFileName += sExt;

    if (sFilePathName.length > 0) {
        $('#ImagePath').attr('value', '/Content/data/');
        $('#ImageFile').attr('value', sFileName);
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#tankimg').attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);

    }

});

// Add a function to Date to return its date string in format "yyyymmddhhnnssttt"

Date.prototype.yyyymmddhhnnssms = function () {
    var mm = this.getMonth() + 1; // getMonth() is zero-based
    var dd = this.getDate();
    var hh = this.getHours();
    var nn = this.getMinutes();
    var ss = this.getSeconds();
    var ms = this.getMilliseconds();

    return [this.getFullYear(),
        (mm > 9 ? '' : '0') + mm,
        (dd > 9 ? '' : '0') + dd,
        (hh > 9 ? '' : '0') + hh,
        (nn > 9 ? '' : '0') + nn,
        (ss > 9 ? '' : '0') + ss,
        '' + ms,
    ].join('');
};

