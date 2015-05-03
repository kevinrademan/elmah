$(function () {

    var getSelectedErrors = function () {

        var errors = [];
        $("[name=delete-errors]").each(function () {
            var $chkBox = $(this);
            if ($chkBox.prop("checked")) {
                errors.push($(this).val());
            }
        });
        return errors;
    };

    $("#removeErrors").on("click", function () {
        if (confirm("Are you sure you wish to delete the errors?")) {
            var request = {
                command: "delete-errors",
                body: getSelectedErrors()
            }

            $.ajax({
                type: 'POST',
                url: '/elmah.axd/ajax',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(request),
                success: function () {
                    location.reload();
                }
            });
        }
    });
});
