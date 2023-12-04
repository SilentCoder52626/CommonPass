function SeralizeForm(form) {
    return form.serialize();
}

function ShowSuccessNotification(message) {
    toastr["success"](message)
}
function ShowInfoNotification(message) {
    toastr["info"](message)
}
function ShowErrorNotification(message) {
    toastr["error"](message)
}
function ShowWarningNotification(message) {
    toastr["warning"](message)
}

$(document).ready(function () {
    $('.drop-down').select2({
        placeholder: '--Select--'
    });


    $(document).on("click", "#ExportAccounts", function () {
        $.confirm({
            title: 'To continue your download!',
            content: '' +
                '<form action="" class="ExportForm">' +
                '<div class="form-group">' +
                '<label>Enter your Pin</label>' +
                '<input type="password" placeholder="Pin" class="Pin form-control" required autocomplete="off"/>' +
                '</div>' +
                '</form>',
            buttons: {
                formSubmit: {
                    text: 'Submit',
                    btnClass: 'btn-blue',
                    action: function () {
                        var pin = this.$content.find('.Pin').val();
                        if (!pin) {
                            $.alert('provide a valid PIN');
                            return false;
                        }
                        window.location.href = '/Pass/Accounts/ExportAccountsToExcel?Pin='+pin;


                    }
                },
                cancel: function () {
                    //close
                },
            },
            onContentReady: function () {
                // bind to events
                var jc = this;
                this.$content.find('form').on('submit', function (e) {
                    // if the user submits the form by pressing enter in the field.
                    e.preventDefault();
                    jc.$$formSubmit.trigger('click'); // reference the button and click it
                });
            },
            animateFromElement: false,
            escapeKey : true,
        });
    })
});