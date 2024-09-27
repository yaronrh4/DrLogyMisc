<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CommitmentLettersApp.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>מכתבי התחייבות</title>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Roboto|Varela+Round" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script src="lib/jquery/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js"></script>

    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js"></script>

    <link href="lib/bootstrap-datepicker/css/bootstrap-datepicker.standalone.min.css" rel="stylesheet" />
    <script src="lib/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
    <script src="lib/bootstrap-datepicker/locales/bootstrap-datepicker.he.min.js"></script>
    <%--https://bootstrap-datepicker.readthedocs.io/en/latest/--%>
    <script src="lib/tinymce/tinymce.min.js"></script>
    <script src="lib/jquery-validation/dist/jquery.validate.min.js"></script>
    <script src="lib/jquery-validation/dist/additional-methods.js"></script>
    <script src="lib/jquery-validation/dist/localization/messages_he.min.js"></script>

    <style>
        body {
            direction: rtl;
            color: #566787;
            background: #f5f5f5;
            font-family: 'Varela Round', sans-serif;
            font-size: 13px;
        }

        .table-responsive {
            margin: 30px 0;
        }

        .table-wrapper {
            background: #fff;
            padding: 20px 25px;
            border-radius: 3px;
            min-width: 1000px;
            box-shadow: 0 1px 1px rgba(0,0,0,.05);
        }

        .table-title {
            padding-bottom: 15px;
            background: #435d7d;
            color: #fff;
            padding: 16px 30px;
            min-width: 100%;
            margin: -20px -25px 10px;
            border-radius: 3px 3px 0 0;
        }

            .table-title h2 {
                margin: 5px 0 0;
                font-size: 24px;
            }

            .table-title .btn-group {
                float: right;
            }

            .table-title .btn {
                color: #fff;
                float: right;
                font-size: 13px;
                border: none;
                min-width: 50px;
                border-radius: 2px;
                border: none;
                outline: none !important;
                margin-left: 10px;
            }

                .table-title .btn i {
                    float: left;
                    font-size: 21px;
                    margin-right: 5px;
                }

                .table-title .btn span {
                    float: left;
                    margin-top: 2px;
                }

        table.table tr th, table.table tr td {
            border-color: #e9e9e9;
            padding: 12px 15px;
            vertical-align: middle;
        }

            table.table tr th:first-child {
                width: 60px;
            }

            table.table tr th:last-child {
                width: 100px;
            }

        table.table-striped tbody tr:nth-of-type(odd) {
            background-color: #fcfcfc;
        }

        table.table-striped.table-hover tbody tr:hover {
            background: #f5f5f5;
        }

        table.table th i {
            font-size: 13px;
            margin: 0 5px;
            cursor: pointer;
        }

        table.table td:last-child i {
            opacity: 0.9;
            font-size: 22px;
            margin: 0 5px;
        }

        table.table td a {
            font-weight: bold;
            color: #566787;
            display: inline-block;
            text-decoration: none;
            outline: none !important;
        }

            table.table td a:hover {
                color: #2196F3;
            }

            table.table td a.edit {
                color: #FFC107;
            }

            table.table td a.delete {
                color: #F44336;
            }

        table.table td i {
            font-size: 19px;
        }

        table.table .avatar {
            border-radius: 50%;
            vertical-align: middle;
            margin-right: 10px;
        }

        .pagination {
            float: right;
            margin: 0 0 5px;
        }

            .pagination li a {
                border: none;
                font-size: 13px;
                min-width: 30px;
                min-height: 30px;
                color: #999;
                margin: 0 2px;
                line-height: 30px;
                border-radius: 2px !important;
                text-align: center;
                padding: 0 6px;
            }

                .pagination li a:hover {
                    color: #666;
                }

            .pagination li.active a, .pagination li.active a.page-link {
                background: #03A9F4;
            }

                .pagination li.active a:hover {
                    background: #0397d6;
                }

            .pagination li.disabled i {
                color: #ccc;
            }

            .pagination li i {
                font-size: 16px;
                padding-top: 6px
            }

        .hint-text {
            float: left;
            margin-top: 10px;
            font-size: 13px;
        }
        /* Custom checkbox */
        .custom-checkbox {
            position: relative;
        }

            .custom-checkbox input[type="checkbox"] {
                opacity: 0;
                position: absolute;
                margin: 5px 0 0 3px;
                z-index: 9;
            }

            .custom-checkbox label:before {
                width: 18px;
                height: 18px;
            }

            .custom-checkbox label:before {
                content: '';
                margin-right: 10px;
                display: inline-block;
                vertical-align: text-top;
                background: white;
                border: 1px solid #bbb;
                border-radius: 2px;
                box-sizing: border-box;
                z-index: 2;
            }

            .custom-checkbox input[type="checkbox"]:checked + label:after {
                content: '';
                position: absolute;
                left: 6px;
                top: 3px;
                width: 6px;
                height: 11px;
                border: solid #000;
                border-width: 0 3px 3px 0;
                transform: inherit;
                z-index: 3;
                transform: rotateZ(45deg);
            }

            .custom-checkbox input[type="checkbox"]:checked + label:before {
                border-color: #03A9F4;
                background: #03A9F4;
            }

            .custom-checkbox input[type="checkbox"]:checked + label:after {
                border-color: #fff;
            }

            .custom-checkbox input[type="checkbox"]:disabled + label:before {
                color: #b8b8b8;
                cursor: auto;
                box-shadow: none;
                background: #ddd;
            }
        /* Modal styles */
        .modal .modal-dialog {
            max-width: 400px;
        }

        .modal .modal-header, .modal .modal-body, .modal .modal-footer {
            padding: 20px 30px;
            direction: rtl;
            text-align: right;
        }

        .modal .modal-content {
            border-radius: 3px;
            font-size: 14px;
        }

        .modal .modal-footer {
            background: #ecf0f1;
            border-radius: 0 0 3px 3px;
        }

        .modal .modal-title {
            display: inline-block;
        }

        .modal .form-control {
            border-radius: 2px;
            box-shadow: none;
            border-color: #dddddd;
        }

        .modal textarea.form-control {
            resize: vertical;
        }

        .modal .btn {
            border-radius: 2px;
            min-width: 100px;
        }

        .modal form label {
            font-weight: normal;
        }
    </style>
    <script>
        var subjectsFile = '<%= string.Join(",", lettersPDF.Options.Subjects.Select(t => t.NameInFile).ToArray()).Replace ("'", "\\'") %>'.split(',');
        var subjectsDB = '<%= string.Join(",", lettersPDF.Options.Subjects.Select(t => t.NameInDB).ToArray()).Replace ("'", "\\'") %>'.split(',');
        function downloadTemplate ()
        {
            var selFile = $("#drpFiles").val();
            window.location.href = "ImportTemplates\\"+ selFile;
        }
        function ddmmyyyyToDate(value) {
            var dateObject = null;

            if (value != null) {
                var dateParts = value.split("/");
                if (dateParts.length == 3) {
                    // month is 0-based, that's why we need dataParts[1] - 1
                    dateObject = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
                }
            }
            return dateObject;
        }

        jQuery.validator.addMethod("greaterThanDate",
            function (value, element, params) {
                var date1 = ddmmyyyyToDate(value);
                var date2 = ddmmyyyyToDate($(params).val())
      

                return date1 > date2;
            }, 'הערך חייב להיות גדול מתאריך התחלה.');



        function confirmModal(title, msg, action) {
            $("#confirmModal").find("#confirmtitle").html(title);
            $("#confirmModal").find("#confirmmessage").html(msg);
            $("#confirmaction").val(action);
        }

        function addStudent(e) {
            $("#editStudentModal").find(".modal-title").html("הוספת תלמיד");
            $("#stid").val("-1");
            $("#stidx").val("-1");
            $("#editStudentModal").find("#firstname").val("");
            $("#editStudentModal").find("#lastname").val("");
            $("#editStudentModal").find("#idnum").val("");
            $("#editStudentModal").find("#phone").val("");
            $("#editStudentModal").find("#email").val("");
            $("#editStudentModal").find("#branch").val("");
            $("#editStudentModal").find("#coordinatorname").val($("#defcoordinator").val());
            $("#editStudentModal").find("#socialworker").val("");
            $("#editStudentModal").find("#comments").val("");

            $("#currfirstname").text("");
            $("#currlastname").text("");
            $("#currphone").text("");
            $("#curremail").text("");
            $("#currbranch").text("");
            $("#currcoordinatorname").text("");
            $("#currsocialworker").text("");


            const date = new Date();

            let day = date.getDate();
            let month = date.getMonth() + 1;
            let year = date.getFullYear();

            // This arrangement can be altered based on how we want the date's format to appear.
            let currentDate = `${day}/${month}/${year}`;
            console.log(currentDate); // "17-6-2022"
            $("#createdate").val(currentDate);
            $("#isnewstudent").prop('checked', true);


            $("#createdate").each(function () {
                $(this).datepicker('setDate', $(this).val());
            });


        }

        function copyStudentDataFromPdf() {
            $("#editStudentModal").find("#firstname").val($("#currfirstname").text());
            $("#editStudentModal").find("#lastname").val($("#currlastname").text());
            $("#editStudentModal").find("#phone").val($("#currphone").text());
            $("#editStudentModal").find("#email").val($("#curremail").text());
            $("#editStudentModal").find("#branch").val($("#currbranch").text());
            $("#editStudentModal").find("#coordinatorname").val($("#currcoordinatorname").text());
            $("#editStudentModal").find("#socialworker").val($("#currsocialworker").text());
        }

        function editStudent(e) {
            var stid = $(e).parent().parent().attr("stid");
            var edited = $(e).parent().parent().attr("edited");
            $("#editStudentModal").find(".modal-title").html("עריכת תלמיד");

            $("#editStudentModal").find("#stid").val(stid);
            $("#stidx").val($(e).parent().parent().attr("stidx"));
            $("#editStudentModal").find("#firstname").val($(e).parent().parent().attr("firstname"));
            $("#editStudentModal").find("#lastname").val($(e).parent().parent().attr("lastname"));
            $("#editStudentModal").find("#idnum").val($(e).parent().parent().attr("idnum"));
            $("#editStudentModal").find("#phone").val($(e).parent().parent().attr("phone"));
            $("#editStudentModal").find("#email").val($(e).parent().parent().attr("email"));
            $("#editStudentModal").find("#branch").val($(e).parent().parent().attr("branch"));
            $("#editStudentModal").find("#coordinatorname").val($(e).parent().parent().attr("coordinatorname"));
            $("#editStudentModal").find("#socialworker").val($(e).parent().parent().attr("socialworker"));
            $("#editStudentModal").find("#createdate").val($(e).parent().parent().attr("createdate"));
            $("#editStudentModal").find("#isnewstudent").prop("checked", $(e).parent().parent().attr("isnewstudent") == 1);
            $("#editStudentModal").find("#comments").val($(e).parent().parent().attr("comments"));

            $("#currfirstname").text($(e).parent().parent().attr("currfirstname"));
            $("#currlastname").text($(e).parent().parent().attr("currlastname"));
            $("#currphone").text($(e).parent().parent().attr("currphone"));
            $("#curremail").text($(e).parent().parent().attr("curremail"));
            $("#currbranch").text($(e).parent().parent().attr("currbranch"));
            $("#currcoordinatorname").text($(e).parent().parent().attr("currcoordinatorname"));
            $("#currsocialworker").text($(e).parent().parent().attr("currsocialworker"));

            $("#createdate").each(function () {
                $(this).datepicker('setDate', $(this).val());
            });

            if (stid == 0 && edited != "1")
                copyStudentDataFromPdf();
        }
        3
        function addSubject(e) {

            $("#editSubjectModal").find(".modal-title").html("הוספת הנגשה");
            var subjectSelect = $("#editSubjectModal").find("#subjectfile");
            var currSub = "";// ($(e).parent().parent().attr("subjectfile"));
            var currSubjects = [];
            var stidx = $(e).parent().parent().attr("stidx");
            subjectSelect.find(".modal-title").html("הוספת הנגשה");
            $(e).parent().parent().parent().children("[stsubidx='" + stidx + "']").each(function () {
                var sub = $(this).attr("subjectfile");
                if (sub != null && sub != currSub)
                    currSubjects.push(sub);
            });

            if (currSubjects.length >= subjectsFile.length) {
                showAlert("לתלמיד יש את כל ההנגשות הזמינות", "alert-danger");
                return;
            }


            //clear options
            subjectSelect.children().remove().end();

            var val = "";
            for (var i = 0; i < subjectsFile.length; i++) {
                //add the subject if it doesn't exist in the letter
                if (currSubjects.indexOf(subjectsFile[i]) < 0) {
                    if (val == "")
                        val = subjectsFile[i];
                    subjectSelect.append($('<option>', {
                        value: subjectsFile[i],
                        text: subjectsDB[i],
                    }));
                }
            }

            $("#editSubjectModal").find("#subjectfile").on("change", function () {
                $("#subjectname").val($(this).val());
            });

            $("#stsubidx").val(stidx);
            $("#subjectidx").val("-1"); //new

            //select the first subject
            $("#editSubjectModal").find("#subjectfile").prop('selectedIndex', 0);
            $("#subjectname").val($("#subjectfile").val());

            $("#editSubjectModal").find("#subjectfile").prop("disabled", false);



            $("#editSubjectModal").find("#hours").val("");
            $("#editSubjectModal").find("#startdate").val($(e).parent().parent().attr("startdate"));
            $("#editSubjectModal").find("#enddate").val($(e).parent().parent().attr("enddate"));

            $("#currhours").text("");
            $("#currstartdate").text("");
            $("#currenddate").text("");

            $(".datepicker").each(function () {
                $(this).datepicker('setDate', $(this).val());
            });

            $('#editSubjectModal').modal('show');

            return false;

        }


        function editSubject(e) {
            $("#editSubjectModal").find(".modal-title").html("עריכת הנגשה");
            var subjectSelect = $("#editSubjectModal").find("#subjectfile");
            var currSub = ($(e).parent().parent().attr("subjectfile"));
            var i = subjectsFile.indexOf(currSub);

            subjectSelect.append($('<option>', {
                value: subjectsFile[i],
                text: subjectsDB[i]
            }));

            $("#stsubidx").val($(e).parent().parent().attr("stsubidx"));

            $("#subjectidx").val($(e).parent().parent().attr("subjectidx"));
            $("#editSubjectModal").find("#subjectfile").val($(e).parent().parent().attr("subjectfile"));
            $("#editSubjectModal").find("#subjectfile").prop("disabled", "disabled");
            $("#editSubjectModal").find("#hours").val($(e).parent().parent().attr("hours"));
            $("#editSubjectModal").find("#startdate").val($(e).parent().parent().attr("startdate"));
            $("#editSubjectModal").find("#enddate").val($(e).parent().parent().attr("enddate"));

            $("#currhours").text($(e).parent().parent().attr("currhours"));
            $("#currstartdate").text($(e).parent().parent().attr("currstartdate"));
            $("#currenddate").text($(e).parent().parent().attr("currenddate"));


            $(".datepicker").each(function () {
                $(this).datepicker('setDate', $(this).val());
            });

        }

        function showAlert(msg, cssClass) {
            $('#alertmessage').parent().removeClass();
            $('#alertmessage').parent().addClass(cssClass);
            $('#alertmessage').html(msg);
            $('#alert').removeClass("d-none");
        }


        $(function () {
            $("#frm1").validate({
                rules: {
                    fuPdfs: {
                        required: true
                    },
                    firstname: {
                        required: true,
                        minlength: 2,
                        maxlength: 50
                    },
                    lastname: {
                        required: true,
                        minlength: 2,
                        maxlength: 50
                    },
                    idnum: {
                        required: true,
                        digits: true,
                        minlength: 2,
                        maxlength: 9
                    },
                    Loadidnum: {
                        required: true,
                        digits: true,
                        minlength: 2,
                        maxlength: 9
                    },
                    phone: {
                        required: true,
                        pattern: "^\\+?(972|0)(\\-)?0?(([23489]{1}[\\-]?\\d{7})\\-?|([71,72,73,74,75,76,77]{2}\\-?\\d{7})|[5]{1}?\\d{1}\\-?\\d{7})$"
                    },
                    email: {
                        required: true,
                        email: true
                    },
                    socialworker: {
                        required: true,
                        minlength: 2,
                        maxlength: 50
                    },
                    branch: {
                        required: true,
                        minlength: 2,
                        maxlength: 50
                    },
                    coordinatorname: {
                        required: true
                    },
                    hours: {
                        number: true,
                    },
                    startdate: {
                        required: true
                    },
                    enddate: {
                        required: true,
                        greaterThanDate: "#startdate"
                    },
                    Loadstartdate: {
                        required: true
                    },
                    Loadenddate: {
                        required: true,
                        greaterThanDate: "#Loadstartdate"
                    }

                },
                messages: {
                    phone: "הפורמט של מספר הטלפון שגוי",
                    enddate: "התאריך חייב להיות גדול מתאריך ההתחלה"
                }
            });

            let q = $("#datachanged").val();
            $("#btnSave").prop('disabled', (q != "1"));
            $("#btnSendMails").prop('disabled', $(".subjectdatarow").length == 0);
            $("#btnExportToExcel").prop('disabled', $(".subjectdatarow").length == 0);

            q = $("#allowchangeproject").val();
            $("#drpProjects").prop('disabled', (q != "1"));
            $("#btnSetProject").prop('disabled', (q != "1"));

            $("#btnSave").on("click", function () {
                confirmModal(
                    "שמירה",
                    "האם אתה בטוח שברצונך לעדכן בבסיס הנתונים את פרטי התלמידים וההנגשות ששונו?",
                    "save"
                );
                return true;
            });

            $("#btnSendMails").on("click", function () {
                var url = "SendMail.aspx";
                window.open(url, '_blank');
            });


            if ($("#successhidden").val() != "") {
                showAlert($("#successhidden").val(), "alert-success");
                $("#successhidden").val("");
            }

            if ($("#errorhidden").val() != "") {
                showAlert($("#errorhidden").val(), "alert-danger");
                $("#errorhidden").val("");
            }

            $('.datepicker').datepicker({
                //language: "he",
                format: "dd/mm/yyyy",
                autoclose: true
                //    orientation : "left"
            });

            // Activate tooltip
            $('[data-toggle="tooltip"]').tooltip();
        });


    </script>
</head>
<body>
    <form runat="server" id="frm1">
        <input runat="server" type="hidden" id="successhidden" value="" />
        <input runat="server" type="hidden" id="errorhidden" value="" />
        <input runat="server" type="hidden" id="confirmaction" value="" />
        <input runat="server" type="hidden" id="datachanged" value="" />
        <input runat="server" type="hidden" id="defcoordinator" value="" />
        <input runat="server" type="hidden" id="allowchangeproject" value="1" />

        <input runat="server" id="stid" type="hidden" />
        <input runat="server" id="stidx" type="hidden" />
        <input runat="server" id="stsubidx" type="hidden" />
        <input runat="server" id="subjectidx" type="hidden" />
        <input runat="server" id="subjectname" type="hidden" />

        <div class="container-xl">
            <div class="table-responsive">
                <div class="table-wrapper">
                    <div class="table-title">
                        <div class="row">
                            <div class="col-lg-12 text-right">
                                <div class="alert d-none" id="alert">
                                    <div>
                                        <span id="alertmessage">גיא פניני מת</span>
                                        <button title="סגירה" type="button" class="close" data-dismiss="alert" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                </div>
                            </div>


                            <div class="col-lg-3">
                                <h2>עדכון התחייבויות</h2>
                            </div>
                            <div class="col-lg-9">

                                <button type="button" title="טעינת קבצי PDF" <%= AllowLoadPDF() %> class="btn btn-success" data-toggle="modal" data-target="#addPdfsModal"><i class="material-icons">&#xE147;</i> <span>טעינת קבצי PDF</span></button>
                                <button type="button" title="טעינת תלמיד" class="btn btn-success" data-toggle="modal" data-target="#loadStudentModal"><i class="material-icons">&#xE7FD;</i> <span>טעינת תלמיד</span></button>
                                <button onclick="addStudent(this)" title="הוספת תלמיד" type="button" class="btn btn-success" data-toggle="modal" data-target="#editStudentModal"><i class="material-icons" data-toggle="tooltip" title="הוספת תלמיד">&#xE7FE;</i> <span>הוספת תלמיד</span></button>
                                <button type="button" title="יבוא מאקסל" class="btn btn-success" data-toggle="modal" data-target="#excelImportModal"><i class="material-icons">&#xEAF3;</i> <span>יבוא מאקסל</span></button>

                                <asp:LinkButton ToolTip="ניקוי" runat="server" OnClick="btnClear_Click" ID="btnClear" CssClass="btn btn-success"><i class="material-icons">&#xE14C;</i> <span>ניקוי</span></asp:LinkButton>
                                <%--                                <a href="#confirmModal" class="btn btn-danger" data-toggle="modal"><i class="material-icons">&#xE15C;</i> <span>Delete</span></a>--%>
                            </div>
                        </div>
                    </div>
                    <table class="table table-striped table-hover">
                        <thead>
                            <tr>
                                <th>
                                    <span class="custom-checkbox">
                                        <%--                                        <input type="checkbox" id="selectAll" disabled="disabled" />
                                        <label for="selectAll"></label>--%>
                                    </span>
                                </th>
                                <th>סטטוס</th>
                                <th>זיהוי תלמיד</th>
                                <th>שם פרטי</th>
                                <th>שם משפחה</th>
                                <th>ת.ז</th>
                                <th>אימייל</th>
                                <th>התחלה</th>
                                <th>סיום</th>
                                <th>הנגשה</th>
                                <th>שעות</th>
                                <th>הערות</th>
                                <th>פעולות</th>
                            </tr>
                        </thead>
                        <tbody>

                            <asp:Repeater runat="server" ID="rep1">

                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="studentdatarow" stidx="<%# Container.ItemIndex %>" stid="<%#Eval2("Id")%>" firstname="<%#AttrEval("CurrFirstName")%>" lastname="<%#AttrEval("CurrLastName")%>" idnum="<%#AttrEval("IdNum")%>" phone="<%#AttrEval("CurrPhone")%>" email="<%#AttrEval("CurrEmail")%>" socialworker="<%#AttrEval("CurrSocialWorker")%>" branch="<%#AttrEval("CurrBranch")%>" coordinatorname="<%#AttrEval("CoordinatorName")%>" startdate="<%#Eval2("StartDate" , "{0:dd/MM/yyyy}")%>" enddate="<%#Eval2("EndDate", "{0:dd/MM/yyyy}") %>" currfirstname="<%#StudentAttrEval("FirstName")%>" currlastname="<%#StudentAttrEval("LastName")%>" currphone="<%#StudentAttrEval("Phone")%>" curremail="<%#StudentAttrEval("Email")%>" currbranch="<%#StudentAttrEval("Branch")%>" currcoordinatorname="<%#StudentAttrEval("CoordinatorName")%>" currsocialworker="<%#StudentAttrEval("SocialWorker")%>" createdate="<%#Eval2("CreateDate" , "{0:dd/MM/yyyy}")%>" isnewstudent="<%#Eval2("IsNewStudent").ToString() == "" || !((bool)Eval2("IsNewStudent")) ? "0" : "1"%>" comments="<%#AttrEval("Comments")%>" edited="<%#(bool)Eval2("Edited") ? 1 : 0%>">
                                        <td></td>
                                        <td></td>
                                        <td><%#Eval2("Id") %></td>
                                        <td><%#Eval2("FirstName")%><br />
                                            <span class="small text-info">(<%#Eval2("CurrFirstName")%>)</span>
                                        </td>
                                        <td><%#Eval2("LastName")%><br />
                                            <span class="small text-info">(<%#Eval2("CurrLastName")%>)</span>
                                        </td>
                                        <td><%#Eval2("IdNum") %></td>
                                        <td><%#Eval2("Email")%><br />
                                            <span class="small text-info">(<%#Eval2("CurrEmail")%>)</span>
                                        </td>
                                        <td><%#Eval2("StartDate", "{0:dd/MM/yyyy}")%></td>
                                        <td><%#Eval2("EndDate", "{0:dd/MM/yyyy}")%></td>
                                        <td></td>
                                        <td></td>
                                        <td><%#Eval2("Comments")%>
                                        </td>

                                        <td>
                                            <a onclick="editStudent(this)" href="#editStudentModal" class="edit" data-toggle="modal"><i class="material-icons" data-toggle="tooltip" title="עריכת תלמיד">&#xE254;</i></a>
                                            <a onclick="addSubject(this)" class="edit" data-toggle="modal"><i class="material-icons" data-toggle="tooltip" title="הוסף הנגשה">&#xE145;</i></a>
                                            <%--                                            <a href="#confirmModal" class="delete" data-toggle="modal"><i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i></a>--%>
                                        </td>
                                    </tr>
                                    <asp:Repeater runat="server" DataSource='<%# Eval2("Subjects") %>'>
                                        <ItemTemplate>
                                            <tr class="subjectdatarow" stsubidx="<%#((RepeaterItem)(Container.Parent.Parent)).ItemIndex %>" subjectidx="<%# Container.ItemIndex %>" subjectfile="<%#Eval2("SubjectInFile")%>" hours="<%#Eval2("Hours")%>" startdate="<%#DataBinder.Eval(Container.Parent.Parent, "DataItem.StartDate" , "{0:dd/MM/yyyy}") %>" enddate="<%#DataBinder.Eval(Container.Parent.Parent, "DataItem.EndDate" , "{0:dd/MM/yyyy}") %>" currhours="<%#Eval2("CurrHours")%>" currstartdate="<%#Eval2("CurrStartDate", "{0:dd/MM/yyyy}") %>" currenddate="<%#Eval2("CurrEndDate", "{0:dd/MM/yyyy}") %>" />
                                            <td>
                                                <%--                                    <span class="custom-checkbox">--%>
                                                <%--                                        <label for="checkbox1"></label>--%>
                                                <%--                                    </span>--%>
                                            </td>
                                            <td><%#GetEnumDescription (Eval2("Status") as Enum)%></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td><span class="small text-info">(<%#Eval2("CurrStartDate", "{0:dd/MM/yyyy}")%>)</span></td>
                                            <td><span class="small text-info">(<%#Eval2("CurrEndDate", "{0:dd/MM/yyyy}")%>)</span></td>
                                            <td><%#Eval2("SubjectInFile")%></td>
                                            <td><%#Eval2("Hours")%>
                                                <span class="small text-info">(<%#Eval2("CurrHours")%>)</span>
                                            </td>
                                            <td></td>
                                            <td>
                                                <a title="עריכת הנגשה" onclick="editSubject(this)" href="#editSubjectModal" class="edit" data-toggle="modal"><i class="material-icons" data-toggle="tooltip">&#xE254;</i></a>
                                                <%--                                                <a href="#confirmModal" class="delete" data-toggle="modal"><i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i></a>--%>
                                            </td>
                                            </tr>
                                        </ItemTemplate>
                                        <%--      <SeparatorTemplate>,</SeparatorTemplate>--%>
                                    </asp:Repeater>

                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div class="table-wrapper">

                    <div class="table-title">
                        <div class="row">
                            <div class="col-lg-6">
                                <button id="btnSave" title="שמירה" disabled="disabled" type="button" class="btn btn-success" data-toggle="modal" data-target="#confirmModal"><i class="material-icons">&#xF233;</i> <span>שמירה</span></></button>
                                <button id="btnSendMails" title="שליחת מיילים" disabled="disabled" type="button" class="btn btn-success"><i class="material-icons">&#xE159;</i> <span>שליחת מיילים</span></button>
                                <asp:LinkButton ToolTip="יצוא לאקסל" runat="server" OnClick="btnExportToExcel_Click" ID="btnExportToExcel" CssClass="btn btn-success"><i class="material-icons">&#xE159;</i> <span>יצוא לאקסל</span></asp:LinkButton>
                            </div>
                            <div class="col-lg-5">
                                <a title="עריכת תבניות מייל" href="EditMail.aspx" target="_blank" class="btn btn-success"><i class="material-icons">&#xE161;</i> <span>עריכת תבניות מייל</span></a>
                            </div>
                            <div class="col-lg-1">
                                <a title="התנתקות" href="logout.aspx" target="_blank" class="btn btn-success"><i class="material-icons">&#xE9BA;</i> <span></span></a>
                            </div>
                        </div>

                    </div>
                                        <div class="row">
                            <div class="col-lg-6 text-right">פרוייקט
                                <asp:DropDownList runat="server" ClientIDMode="Static" ID="drpProjects" AutoPostBack="true" OnSelectedIndexChanged="drpProjects_SelectedIndexChanged" ></asp:DropDownList>
                            <asp:Button runat="server" ClientIDMode="Static"  ID="btnSetProject" ToolTip="שמור כברירת מחדל" CssClass="" Text="שמור כברירת מחדל" OnClick="btnSetProject_Click" />

                            </div>
                    </div>

                </div>

            </div>

            <div id="loadStudentModal" dir="rtl" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <%--                <form id="frm2" runat="server">--%>
                        <div class="modal-header">
                            <div>
                                <button title="סגירה" type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">טעינת תלמיד</h4>
                            </div>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-2">
                                    <label>
                                        ת.ז
                                    <br />
                                        <br />
                                    </label>
                                </div>

                                <div class="col-lg-10">
                                    <input runat="server" id="Loadidnum" type="text" class="form-control" />
                                </div>
                                <div class="col-lg-2">
                                    <label>תאריך התחלה</label>
                                </div>
                                <div class="col-lg-10">
                                    <input runat="server" id="Loadstartdate" class="form-control datepicker" />
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        תאריך סיום<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-md-10">
                                    <input runat="server" id="Loadenddate" class="form-control datepicker" />
                                </div>
                                <div class="col-lg-2">
                                    <label>
                                        הנגשות
                             <br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-lg-10">

                                    <asp:CheckBoxList ID="chklstSubjects" runat="server" chk1="">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ToolTip="טעינה" runat="server" ID="btnLoadStudent" OnClick="btnLoadStudent_Click" CssClass="btn btn-success" Text="טעינה" />
                                <input title="ביטול" type="button" class="btn btn-default" data-dismiss="modal" value="ביטול" />
                            </div>

                            <%--                </form>--%>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Add PDF HTML -->
            <div id="addPdfsModal" dir="rtl" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <%--                <form id="frm2" runat="server">--%>
                        <div class="modal-header">
                            <div>
                                <button title="סגירה" type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">טעינת קבצים</h4>
                            </div>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <label>בחירת קבצים</label>
                                <asp:FileUpload ID="fuPdfs" runat="server" accept="application/pdf" CssClass="form-control" AllowMultiple="true" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button title="הוספה" runat="server" ID="btnAddPdf" OnClick="btnAddPdf_Click" CssClass="btn btn-success" Text="הוספה" />
                            <input type="button" title="ביטול" class="btn btn-default" data-dismiss="modal" value="ביטול" />
                        </div>

                        <%--                </form>--%>
                    </div>
                </div>
            </div>

            <!-- Excel Impoty HTML -->
            <div id="excelImportModal" dir="rtl" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <%--                <form id="frm2" runat="server">--%>
                        <div class="modal-header">
                            <div>
                                <button title="סגירה" type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title">יבוא מאקסל</h4>
                            </div>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <label>בחירת קובץ</label>
                                <asp:FileUpload ID="fuExcel" runat="server" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel" CssClass="form-control" AllowMultiple="false" />
                            </div>

                        </div>
                        <div class="modal-footer">
                            <asp:Button title="הוספה" runat="server" ID="btnImportFromExcel" OnClick="btnImportFromExcel_Click" CssClass="btn btn-success" Text="יבוא" />
                            <input type="button" title="ביטול" class="btn btn-default" data-dismiss="modal" value="ביטול" />
                        
                            <div class="form-group">
                                <label>הורדת קובץ תבנית</label>
                                <asp:DropDownList runat="server" ID="drpFiles" ClientIDMode="Static">
                                </asp:DropDownList>
                                <input type="button" title="הורד" value="הורד" onclick="downloadTemplate()" />
                            </div>
                        </div>

                        <%--                </form>--%>
                    </div>
                </div>
            </div>


            <!-- Confirm Modal HTML -->
            <div id="confirmModal" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <%--                <form id="frm1">--%>
                        <div class="modal-header">
                            <h4 class="modal-title" id="confirmtitle"></h4>
                            <button type="button" title="סגירה" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        </div>
                        <div class="modal-body">
                            <p id="confirmmessage"></p>
                            <p class="text-warning"><small>הפעולה אינה הפיכה</small></p>
                        </div>
                        <div class="modal-footer">
                            <asp:Button title="אישור" runat="server" ID="btnConfirm" CssClass="btn btn-danger" Text="אישור" OnClick="btnConfirm_Click" />
                            <input titlt="ביטול" type="button" class="btn btn-default" data-dismiss="modal" value="ביטול" />
                        </div>
                        <%--                </form>--%>
                    </div>
                </div>
            </div>

            <!-- Edit Student Modal HTML -->
            <div id="editStudentModal" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">עריכת תלמיד</h4>
                            <button title="סגירה" type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-2">
                                    <label>שם פרטי</label>
                                </div>
                                <div class="col-lg-10">
                                    <input runat="server" id="firstname" type="text" class="form-control" />
                                    <span class="small text-info" id="currfirstname"></span>

                                </div>
                                <div class="col-lg-2">
                                    <label>שם משפחה</label>
                                </div>
                                <div class="col-lg-10">
                                    <input runat="server" id="lastname" type="text" class="form-control" />
                                    <span class="small text-info" id="currlastname"></span>
                                </div>
                                <div class="col-lg-2">
                                    <label>
                                        ת.ז
                                    <br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-lg-10">
                                    <input runat="server" id="idnum" type="text" class="form-control" />
                                    <span class="small text-info" id="curridnum"></span>
                                </div>
                                <div class="col-lg-2">
                                    <label>
                                        טלפון<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-lg-10">
                                    <input runat="server" id="phone" type="text" class="form-control" />
                                    <span class="small text-info" id="currphone"></span>
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        מייל<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-md-10">
                                    <input runat="server" id="email" type="text" class="form-control" />
                                    <span class="small text-info" id="curremail"></span>
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        עו"ס<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-md-10">
                                    <input runat="server" id="socialworker" type="text" class="form-control" />
                                    <span class="small text-info" id="currsocialworker"></span>
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        סניף<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-md-10">
                                    <input runat="server" id="branch" type="text" class="form-control" />
                                    <span class="small text-info" id="currbranch"></span>
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        רכז תלמיד<br />
                                    </label>
                                </div>
                                <div class="col-md-10">
                                    <asp:DropDownList runat="server" ID="coordinatorname" CssClass="form-control" />
                                    <span class="small text-info" id="currcoordinatorname"></span>
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        תאריך קליטה<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-md-10">
                                    <input runat="server" id="createdate" class="form-control datepicker" />
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        הערות<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-md-10">
                                    <textarea runat="server" id="comments" class="form-control" rows="3" />
                                </div>

                                <div class="col-md-2">
                                    <label>
                                        תלמיד חדש<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-md-10">
                                    <asp:CheckBox runat="server" ID="isnewstudent" class="form-control" />
                                </div>

                            </div>

                        </div>
                        <div class="modal-footer">
                            <input title="ביטול" type="button" class="btn btn-default" data-dismiss="modal" value="ביטול" />
                            <asp:Button ToolTip="שמירה" runat="server" ID="btnSaveStudent" OnClick="btnSaveStudent_Click" CssClass="btn btn-info" Text="שמירה" />
                        </div>

                    </div>
                </div>
            </div>

            <!-- Edit Subject Modal HTML -->
            <div id="editSubjectModal" class="modal fade">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <%--                <form id="Form1" runat="server">--%>
                        <div class="modal-header">
                            <h4 class="modal-title">עריכת הנגשה</h4>
                            <button title="סגירה" type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-2">
                                    <label>
                                        הנגשה<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-lg-10">
                                    <select id="subjectfile" class="form-control">
                                    </select>
                                </div>
                                <div class="col-lg-2">
                                    <label>
                                        שעות<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-lg-10">
                                    <input runat="server" id="hours" type="number" step="1" min="1" class="form-control" />
                                    <span class="small text-info" id="currhours"></span>

                                </div>
                                <div class="col-lg-2">
                                    <label>תאריך התחלה</label>
                                </div>
                                <div class="col-lg-10">
                                    <input runat="server" id="startdate" class="form-control datepicker" />
                                    <span class="small text-info" id="currstartdate"></span>
                                </div>
                                <div class="col-md-2">
                                    <label>
                                        תאריך סיום<br />
                                        <br />
                                    </label>
                                </div>
                                <div class="col-md-10">
                                    <input runat="server" id="enddate" class="form-control datepicker" />
                                    <span class="small text-info" id="currenddate"></span>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <input title="סגירה" type="button" class="btn btn-default" data-dismiss="modal" value="ביטול" />
                            <asp:Button ToolTip="שמירה" runat="server" ID="btnSaveSubject" OnClick="btnSaveSubject_Click" CssClass="btn btn-info" Text="שמירה" />
                        </div>
                        <%--                </form>--%>
                    </div>
                </div>
            </div>
        </div>

    </form>

</body>
</html>

<!-- Session Timeout:<%= Session.Timeout %> <%=DateTime.Now.ToString() %>-->
