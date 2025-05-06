function report(obj1, obj2) {

    var table, tbody, i, rowLen, row, j, colLen, cell, nWidth;

    //if (obj1 != "") {
    //    alert(obj1);
    //    table = document.getElementById(obj1);
    //    //alert(table);
    //    tbody = table.tBodies[0];
    //    var myWindow = window.open("", "Report", "width=800, height=500,left=100,top=100,resizable=yes,scrollbars=yes,toolbar=yes,status=yes");

    //    myWindow.document.write("<html><head><title>Report</title>");
    //    myWindow.document.write("<style type='text/css'>");
    //    myWindow.document.write("table { width: 100%; table-layout: fixed; word-wrap: break-word; border: 1px solid gray; }");
    //    myWindow.document.write("</style>");
    //    myWindow.document.write("</head>");
    //    myWindow.document.write("</body>");
    //    myWindow.document.write("<table border='1' cellspacing='0' cellpadding='2' width='100%'>");

    //    for (i = 0, rowLen = tbody.rows.length; i < rowLen; i++) {
    //        myWindow.document.write("<tr>");
    //        row = tbody.rows[i];
    //        for (j = 0, colLen = row.cells.length; j < colLen; j++) {
    //            cell = row.cells[j];
    //            myWindow.document.write("<td>" + cell.innerHTML + "</td>");
    //        }
    //        myWindow.document.write("</tr>");
    //    }
    //}
    //alert(obj1)
    var myWindow = window.open("", "Report", "width=800, height=500");
    if (obj2 != "") {

        table = document.getElementById(obj2);
        tbody = table.tBodies[0];

        myWindow.document.write("<html><head><title>Report</title>");
        myWindow.document.write("<style type='text/css'>");
        myWindow.document.write("table { width: 100%; table-layout: fixed; word-wrap: break-word; border: 1px solid gray; }");
        myWindow.document.write("</style>");
        myWindow.document.write("</head>");
        //alert(obj1)
        if (obj1 != "gvHeader") {
            myWindow.document.write(obj1);
        }
        myWindow.document.write("<table border='1' cellspacing='0' cellpadding='2' width='100%'>");
        for (i = 0, rowLen = tbody.rows.length; i < rowLen; i++) {
            myWindow.document.write("<tr>");
            row = tbody.rows[i];
            for (j = 0, colLen = row.cells.length; j < colLen; j++) {
                cell = row.cells[j];
                myWindow.document.write("<td>" + cell.innerHTML + "</td>");
            }
            myWindow.document.write("</tr>");
        }
    }

    myWindow.document.write("</table>");
    myWindow.document.write("</body>");
    myWindow.print();
}

function reportFormat2(tcontent) {

    //tcontent = "<table><tbody><tr><td colspan=\'2\'>Company</td></tr><tr><td colspan=\'2\'>Address</td></tr><tr><td>Quatation No:</td><td>QuatationNo</td></tr><tr><td>Date :</td><td>qDate</td></tr></tbody></table><table border=\'1px\' cellpadding=\'3px\' cellspacing='0px'><tr><td>Material</td><td>Qty</td><td>Price</td><td>TaxAmount</td><td>Total</td></tr><tr><td>Material</td><td>Qty</td><td>Price</td><td>TaxAmount</td><td>Total</td></tr><tr><td>Material</td><td>Qty</td><td>Price</td><td>TaxAmount</td><td>Total</td></tr><tr><td>Material</td><td>Qty</td><td>Price</td><td>TaxAmount</td><td>Total</td></tr></table><table><tbody><tr><td>Terms and Conditions :</td><td>qTerms</td></tr></tbody></table>";
    //alert(tcontent);
    var myWindow = window.open("", "Report", "width=800, height=500,left=100,top=100,resizable=yes,scrollbars=yes,toolbar=yes,status=yes");
    //myWindow.document.write('<html><head><title>Print</title><link rel="stylesheet" type="text/css" href="Styles/Print.css"></head><body>');
    myWindow.document.write('<html><head><style type = "text/css"> @media print { #page-end {page-break-after: always;} tr{page-break-inside: avoid;}}</style><title>Print</title><link rel="stylesheet" type="text/css" href="/Styles/Print.css"></head>');
    myWindow.document.write('<page size="A4">');
    myWindow.document.write(tcontent);
    myWindow.document.write('</page>');
    myWindow.document.write('</html>');
    //myWindow.document.write('</body></html>');
    myWindow.print();

}