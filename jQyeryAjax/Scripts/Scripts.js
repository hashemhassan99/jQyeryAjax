$(function () {
    $('#loaderbody').addClass('hide');
});

function ShowImagePreivew(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imagePreview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#imgInp").change(function () {
    ShowImagePreivew(this);
});


//this function can be used for any form with file input or not
function jQueryAjaxPost (form) {
    $.validator.unobtrusive.parse(form);    
    if ($(form).valid()) {
        var ajaxconfig = {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                if (response.success) {
                    $("#firsttabe").html(response.html);
                    refreshAddNewTab($(form).attr('data-resetUrl'), true);
                    //success message
                    $.notify(response.message, "success");
                    if (typeof activatejQuerytable !== 'undefined' && $.isFunction(activatejQuerytable))
                        activatejQuerytable();
                }
                else {

                    //error message
                    $.notify(response.message, "error")
                }
            }

        }
        if ($(form).attr('enctype') == "multipart/form-data") {
            ajaxconfig["contentType"] = false;
            ajaxconfig["processData"] = false;
        }
        $.ajax(ajaxconfig);

    }
    return false;
}

//function to refresh the form and redierct to view all tab
function refreshAddNewTab(resetUrl, showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
            $("#secondtabe").html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Add New');//index 1 to Add new tab
            if (showViewTab)
                $('ul.nav.nav-tabs a:eq(0)').tab('show');//index 0 to View All tab  
        }
     });
}  
//function for edit 
function Edit(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $("#secondtabe").html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Edit');//index 1 to Add new tab
                $('ul.nav.nav-tabs a:eq(1)').tab('show');//index 0 to View All tab  
        }
    }); 
}
//function for to delet
function Delet(url) {
    if (confirm('Are you sure to delet this record?')) {
        $.ajax({
            type: 'POST',
            url: url,
            success: function (response) {
                if (response.success) {
                    $("#firsttabe").html(response.html);
                    $.notify(response.message, "warn");
                    if (typeof activatejQuerytable !== 'undefined' && $.isFunction(activatejQuerytable))
                        activatejQuerytable();
                } else {
                    $.notify(response.message, "error"); 
                }
            }
        }); 

    }
}
