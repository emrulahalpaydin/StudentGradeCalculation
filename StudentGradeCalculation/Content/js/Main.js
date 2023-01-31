$('#registerButton').on('click', function (e) {
    var NameSurname = $("input[name='NameSurname']");
    var StudentNumber = $("input[name='StudentNumber']");
    var Password = $("input[name='Password']");
    var PasswordConfirmation = $("input[name='PasswordConfirmation']");
    var antiforgeryInput = $('input[name="__RequestVerificationToken"]');
    $.ajax({
        url: "/Administration/Register",
        type: "post",
        data: {
            NameSurname: $(NameSurname).val(),
            StudentNumber: $(StudentNumber).val(),
            Password: $(Password).val(),
            PasswordConfirmation: $(PasswordConfirmation).val(),
            __RequestVerificationToken: $(antiforgeryInput).val()
        },
        success: function (resp) {
            if (!resp.hasError) {
                toastr.success(resp.message);
                setTimeout(function () { window.location.href = "/Administration/Login" }, 2500);
            }
            else {
                toastr.error(resp.message);
            }
        }
        });
    e.preventDefault();
    e.stopPropagation();
    return false;
});

$('#loginButton').on('click', function (e) {
    var StudentNumber = $("input[name='StudentNumber']");
    var Password = $("input[name='Password']");
    var returnInput = $("input[name='ReturnUrl']");
    var antiforgeryInput = $('input[name="__RequestVerificationToken"]');
    $.ajax({
        url: "/Administration/Login",
        type: "post",
        data: {
            StudentNumber: $(StudentNumber).val(),
            Password: $(Password).val(),
            ReturnUrl: $(returnInput).val(),
            __RequestVerificationToken: $(antiforgeryInput).val()
        },
        success: function (resp) {
            if (!resp.hasError) {
                if ($(returnInput).val() != "/" && $(returnInput).val().length > 1) {
                    window.location.href = $(returnInput).val();
                }
                else
                    window.location.href = "/Home/Index";
            } else {
                swal("", resp.message, "error");
            }
        }
    })
    e.preventDefault();
    e.stopPropagation();
    return false;
});
$("#ChangePasswordButton").on('click', function () {
    var model = {};
    var oldPass = $("input[name='OldPassword']").val();
    var newPass = $("input[name='NewPassword']").val();
    var newPassConfirm = $("input[name='NewPasswordConfirmation']").val();
    if (oldPass != null && oldPass != undefined && oldPass != "") {
        if ((newPass != null && newPass != undefined && newPass != "") && (newPassConfirm != null && newPassConfirm != undefined != "")) {
            model.OldPassword = oldPass;
            model.NewPassword = newPass;
            model.NewPasswordConfirmation = newPassConfirm;
            $.ajax({
                url: "/Administration/ChangePassword",
                type: "post",
                data: { "model": model },
                success: function (resp) {
                    if (!resp.hasError) {
                        toastr.success(resp.message);
                        setTimeout(function () { window.location.href = "/Administration/Index" }, 2500)
                    }
                    else {
                        toastr.error(resp.message);
                        return false;
                    }
                }
            });
        }
        else {
            toastr.error("Lütfen yeni şifre alanlarını doldurunuz.");
            return false;
        }
    }
    else {
        toastr.error("Lütfen eski şifrenizi giriniz.");
        return false;
    }
});