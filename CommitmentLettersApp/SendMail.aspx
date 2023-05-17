<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendMail.aspx.cs" Inherits="CommitmentLettersApp.SendMail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>שליחת מיילים</title>
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

        function confirmSaveStudents() {
            confirmModal(
                "שמירת תלמידים",
                "האם אתה בטוח שברצונך לעדכן בבסיס הנתונים את פרטי התלמידים שנערכו?",
                "savestudents"
            );
        }

        function confirmSaveSubjects() {
            confirmModal(
                "שמירת הנגשות",
                "האם אתה בטוח שברצונך לעדכן בבסיס הנתונים את השינויים בהנגשות?",
                "savesubjects"
            );
        }

        function confirmModal(title, msg, action) {
            $("#confirmModal").find("#confirmtitle").html(title);
            $("#confirmModal").find("#confirmmessage").html(msg);
            $("#confirmaction").val(action);
        }

        function editStudent(e) {
            $("#editStudentModal").find("#stid").val($(e).parent().parent().attr("stid"));
            $("#editStudentModal").find("#stidx").val($(e).parent().parent().attr("stidx"));
            $("#editStudentModal").find("#firstname").val($(e).parent().parent().attr("firstname"));
            $("#editStudentModal").find("#lastname").val($(e).parent().parent().attr("lastname"));
            $("#editStudentModal").find("#idnum").val($(e).parent().parent().attr("idnum"));
            $("#editStudentModal").find("#phone").val($(e).parent().parent().attr("phone"));
            $("#editStudentModal").find("#email").val($(e).parent().parent().attr("email"));
            $("#editStudentModal").find("#branch").val($(e).parent().parent().attr("branch"));
            $("#editStudentModal").find("#coordinatorname").val($(e).parent().parent().attr("coordinatorname"));
            $("#editStudentModal").find("#socialworker").val($(e).parent().parent().attr("socialworker"));

        }

        function editSubject(e) {
            var subjectSelect = $("#editSubjectModal").find("#subjectbtl");
            var curSub = ($(e).parent().parent().attr("subjectbtl"));
            var currSubjecs = [];

            $(e).parent().parent().parent().children().each(function () {
                var sub = $(this).attr("subjectbtl");
                if (sub != null && sub != curSub)
                    currSubjecs.push(sub);
            });

            //clear options
            subjectSelect.children().remove().end();
            for (var i = 0; i < subjectsBTL.length; i++) {
                //add the subject if it doesn't exist in the letter
                if (currSubjecs.indexOf(subjectsBTL[i]) < 0)
                    subjectSelect.append($('<option>', {
                        value: subjectsBTL[i],
                        text: subjectsDB[i]
                    }));
            }

            $("#editSubjectModal").find("#stsubidx").val($(e).parent().parent().attr("stsubidx"));
            $("#editSubjectModal").find("#subjectidx").val($(e).parent().parent().attr("subjectidx"));
            $("#editSubjectModal").find("#subjectbtl").val($(e).parent().parent().attr("subjectbtl"));
            $("#editSubjectModal").find("#subjectindb").val($(e).parent().parent().attr("subjectindb"));
            $("#editSubjectModal").find("#hours").val($(e).parent().parent().attr("hours"));
            $("#editSubjectModal").find("#startdate").val($(e).parent().parent().attr("startdate"));
            $("#editSubjectModal").find("#enddate").val($(e).parent().parent().attr("enddate"));

            $("#editSubjectModal").find("#coordinatorname").val($(e).parent().parent().attr("coordinatorname"));
            $("#editSubjectModal").find("#socialworker").val($(e).parent().parent().attr("socialworker"));

            $(".datepicker").each(function () {
                $(this).datepicker('setDate', $(this).val());
            });

        }

        function showAlert(msg) {
            $('#alertokmessage').html (msg);
            $('#alertok').removeClass("d-none");;
        }

        $(document).ready(function () {
            //$('#if1').attr("srcdoc", "<h1>zona</h1>");

            if ($("#successhidden").val() != "") {
                showAlert($("#successhidden").val());
                $("#successhidden").val("");
            }
            else {
            }

            //
            $('.datepicker').datepicker({
                //language: "he",
                format: "dd/mm/yyyy"
                //    orientation : "left"
            });

            // Activate tooltip
            $('[data-toggle="tooltip"]').tooltip();

            $("#selectAll").click(function () {
                alert("dd");
                var ck = this.checked;
                //alert($("").length);
                $("input[name$='chk1']").each(function () { this.checked = ck; })
            });

            $("input[name$='chk1']").click(function () {
                $("#selectAll").prop("checked", false);
            });

            // Select/Deselect checkboxes
            //var checkbox = $('table tbody input[type="checkbox"]');
            //$("#selectAll")
            //checkbox.click(function () {
            ////    if (!this.checked) {
            ////        $("#selectAll").prop("checked", false);
            ////    }
            //});
        });


    </script>
</head>
<body>
    <form runat="server" id="frm1">
        <input type="hidden" id="successhidden" value="" runat="server" />
        <input type="hidden" id="confirmaction" value="" runat="server" />
        
        <div class="container-xl">
            <div class="table-responsive">
                <div class="table-wrapper">
                    <div class="table-title">
                        <div class="row">
                            <div class="col-lg-12 text-right">
                                <div class="alert alert-success d-none" id="alertok">
                                    <span id="alertokmessage">גיא פניני מת</span>
                                  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                  </button>

                                </div>
                             </div>


                            <div class="col-lg-6">
                                <h2>שליחת מייל</h2>
                            </div>
                            <div class="col-lg-2">
                                מייל בדיקה<asp:TextBox runat="server" ID="txtTestEmail"></asp:TextBox>
                            </div>

                        </div>
                    </div>
                    <table class="table table-striped table-hover">
                        <thead>
                            <tr>
                                <th>
                                    <span class="custom-checkbox">
                                        <input type="checkbox" id="selectAll" />
                                        <label for="selectAll"></label>
                                    </span>
                                </th>
                                <th>נושא</th>
                                <th>הודעה</th>
                            </tr>
                        </thead>

                        <tbody>
                        <asp:Repeater id="rep1" runat="server">
                          <ItemTemplate>
                            <tr class="datarow">
                                <td width="20px">
                                    <asp:CheckBox runat="server" id="chk1" class="chk1"/>
                                </td>
                                <td width="20px"><%#Eval("MailSubject") %></td>
                                <td width="100%"><iframe runat="server" style="width:100%;height:200px" srcdoc='<%#Eval("MailBody") %>' ></iframe></td>
                                    </tr>
                        </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                    <div class="table-title">
                        <div class="row">
                            <div class="col-lg-6">
                                <asp:LinkButton runat="server" ID="btnSendAllMails" OnClick="btnSendAllMails_Click"> <div class="btn btn-success"><i class="material-icons">&#xE159;</i> <span>שלח כל המיילים</span></div></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnSendSelectedMails" OnClick="btnSendSelectedMails_Click"> <div class="btn btn-success"><i class="material-icons">&#xE159;</i> <span>שלח מיילים מסומנים</span></div></asp:LinkButton>
                            </div>
                        </div>
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
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">טען קבצים</h4>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label>בחר קבצים</label>
                            <asp:FileUpload ID="fuPdfs" runat="server" accept=".pdf" CssClass="form-control" required2="required2" AllowMultiple="true" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnAddPdf" OnClick="btnAddPdf_Click" CssClass="btn btn-success" Text="הוסף" />
                        <input type="button" class="btn btn-default" data-dismiss="modal" value="ביטול" />
                    </div>

                    <%--                </form>--%>
                </div>
            </div>
        </div>

        <!-- Edit Modal HTML -->
        <div id="addStudentModal" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <%--                <form id="frm2">--%>
                    <div class="modal-header">
                        <h4 class="modal-title">Add Student</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label>Name</label>
                            <input type="text" class="form-control" required2="required2" />
                        </div>
                        <div class="form-group">
                            <label>Email</label>
                            <input type="email" class="form-control" required2="required2" />
                        </div>
                        <div class="form-group">
                            <label>Address</label>
                            <textarea class="form-control" required2="required2" /></textarea>
                        </div>
                        <div class="form-group">
                            <label>Phone</label>
                            <input type="text" class="form-control" required2="required2" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="button" class="btn btn-default" data-dismiss="modal" value="ביטול">
                        <input type="submit" class="btn btn-success" value="Add2">
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
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <p id="confirmmessage"></p>
                        <p class="text-warning"><small>הפעולה אינה הפיכה</small></p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" id="btnConfirm" CssClass="btn btn-danger" Text="אישור" OnClick="btnConfirm_Click" />
                        <input type="button" class="btn btn-default" data-dismiss="modal" value="ביטול">
                    </div>
                    <%--                </form>--%>
                </div>
            </div>
        </div>

        <!-- Edit Student Modal HTML -->
        <div id="editStudentModal" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <%--                <form>--%>
                    <input runat="server" id="stid" type="hidden" />
                    <input runat="server" id="stidx" type="hidden" />
                    <div class="modal-header">
                        <h4 class="modal-title">עריכת מורה</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-2">
                                <label>שם פרטי</label>
                            </div>
                            <div class="col-lg-10">
                                <input runat="server" id="firstname" type="text" class="form-control" required2="required2" />
                            </div>
                            <div class="col-lg-2">
                                <label>שם משפחה</label>
                            </div>
                            <div class="col-lg-10">
                                <input runat="server" id="lastname" type="text" class="form-control" required2="required2" />
                            </div>
                            <div class="col-lg-2">
                                <label>ת.ז
                                    <br />
                                    <br />
                                </label>
                            </div>
                            <div class="col-lg-10">
                                <input runat="server" id="idnum" type="text" class="form-control" required2="required2" />
                            </div>
                            <div class="col-lg-2">
                                <label>טלפון<br />
                                    <br />
                                </label>
                            </div>
                            <div class="col-lg-10">
                                <input runat="server" id="phone" type="text" class="form-control" required2="required2" />
                            </div>
                            <div class="col-md-2">
                                <label>מייל<br />
                                    <br />
                                </label>
                            </div>
                            <div class="col-md-10">
                                <input runat="server" id="email" type="text" class="form-control" required2="required2" />
                            </div>
                            <div class="col-md-2">
                                <label>עו"ס<br />
                                    <br />
                                </label>
                            </div>
                            <div class="col-md-10">
                                <input runat="server" id="socialworker" type="text" class="form-control" required2="required2" />
                            </div>
                            <div class="col-md-2">
                                <label>סניף<br />
                                    <br />
                                </label>
                            </div>
                            <div class="col-md-10">
                                <input runat="server" id="branch" type="text" class="form-control" required2="required2" />
                            </div>
                            <div class="col-md-2">
                                <label>רכז<br />
                                    <br />
                                </label>
                            </div>
                            <div class="col-md-10">
                                <input runat="server" id="coordinatorname" type="text" class="form-control" required2="required2" />
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="button" class="btn btn-default" data-dismiss="modal" value="ביטול" />
                        <asp:button runat="server"  ID="btnSaveStudent" CssClass="btn btn-info" Text="3שמירה" OnClick="btnSaveStudent_Click"/>
                    </div>
                    <%--                </form>--%>
                </div>
            </div>
        </div>

        <!-- Edit Subject Modal HTML -->

        <!-- Edit Subject Modal HTML -->
        <div id="editSubjectModal" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <%--                <form id="Form1" runat="server">--%>
                    <input runat="server" id="stsubidx" type="hidden" />
                    <input runat="server" id="subjectidx" type="hidden" />
                    <div class="modal-header">
                        <h4 class="modal-title">עריכת הנגשה</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-2">
                                <label>הנגשה ב"ל</label>
                            </div>
                            <div class="col-lg-10">
                                <select runat="server" id="subjectbtl" class="form-control">
                                </select>
                            </div>
                            <div class="col-lg-2">
                                <label>הנגשה</label>
                            </div>
                            <div class="col-lg-10">
                                <input runat="server" id="subjectindb" type="text" class="form-control" disabled="disabled" />
                            </div>
                            <div class="col-lg-2">
                                <label>שעות<br />
                                    <br />
                                </label>
                            </div>
                            <div class="col-lg-10">
                                <input runat="server" id="hours" type="text" class="form-control" required2="required2" />
                            </div>
                            <div class="col-lg-2">
                                <label>תאריך התחלה</label>
                            </div>
                            <div class="col-lg-10">
                                <input runat="server" id="startdate" class="form-control datepicker" required2="required2" />
                            </div>
                            <div class="col-md-2">
                                <label>תאריך סיום<br />
                                    <br />
                                </label>
                            </div>
                            <div class="col-md-10">
                                <input runat="server" id="enddate" class="form-control datepicker" required2="required2" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="button" class="btn btn-default" data-dismiss="modal" value="ביטול" />
                        <asp:Button runat="server" ID="btnSaveSubject" OnClick="btnSaveSubject_Click" CssClass="btn btn-info" Text="שמירה2" />
                    </div>
                    <%--                </form>--%>
                </div>
            </div>
        </div>

    </form>

</body>
</html>
