$(document).ready(function () {
    $('#btncomplaint').click(function () {
        var complaint = $('#txtcomplaint').val();
        var proof = $('#txtproof').val();
        var witness = $('#txtwitness').val();
        var tDate = new Date();
        var date = tDate.getFullYear() + '-' + (tDate.getMonth() + 1) + '-' + tDate.getDate();

        if (complaint == "" || proof == "" || witness == "") {
            alert('Please fill out all information');
        }
        else {
            $.post('../Home/fileComp', {
                complaint: complaint,
                proof: proof,
                witness: witness,
                date: date
            }, function (data) {
                if (data[0].mess == 1) {
                    alert('Successfully File a Complaint');
                    location.reload();
                }
                else {
                    alert('Failed to File a Complaint');
                }
            });
        }
    });
});
