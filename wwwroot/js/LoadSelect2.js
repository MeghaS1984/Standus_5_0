$("document").ready(function () {

});

$('select').select2({
    width: '100%',
    height: '10px',
});


$('#ddlAccounts').select2({
    width: '50%',
    height: '10px',
    dropdownParent: $('#ModalForm')
})

$('#ddlEmployee').on('select2:select', function (e) {
    // Do something
    //alert('this is from main class');
    //var employeeId = $("#ddlEmployee").val();
    //$("#skillbox").load('@(Url.Action("index", "Skills", Nothing, Request.Url.Scheme))?Id=' + employeeId);
    employeeChanged()
});




