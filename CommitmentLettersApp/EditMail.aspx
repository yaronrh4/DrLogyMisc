<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMail.aspx.cs" Inherits="CommitmentLettersApp.EditMail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>עריכת תבניות מייל</title>
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
            var subjectSelect = $("#editSubjectModal").find("#subjectfile");
            var curSub = ($(e).parent().parent().attr("subjectfile"));
            var currSubjecs = [];

            $(e).parent().parent().parent().children().each(function () {
                var sub = $(this).attr("subjectfile");
                if (sub != null && sub != curSub)
                    currSubjecs.push(sub);
            });

            //clear options
            subjectSelect.children().remove().end();
            for (var i = 0; i < subjectsFile.length; i++) {
                //add the subject if it doesn't exist in the letter
                if (currSubjecs.indexOf(subjectsFile[i]) < 0)
                    subjectSelect.append($('<option>', {
                        value: subjectsFile[i],
                        text: subjectsDB[i]
                    }));
            }

            $("#editSubjectModal").find("#stsubidx").val($(e).parent().parent().attr("stsubidx"));
            $("#editSubjectModal").find("#subjectidx").val($(e).parent().parent().attr("subjectidx"));
            $("#editSubjectModal").find("#subjectfile").val($(e).parent().parent().attr("subjectfile"));
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
            tinyMCE.init({
                selector: '#editorcontent',
                plugins: [
                    'autolink link lists charmap hr anchor',
                    'searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime nonbreaking',
                    'table contextmenu directionality template paste textcolor'
                ],

                directionality: "rtl",
                menubar: "edit insert view format table tools",
                language: 'he_IL',
                setup: function (ed) {
                    ed.on('init', function (e) {
                        //ed.execCommand('mceFullScreen');
                    });
                }

            });

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
                                    <span id="alertokmessage"></span>
                                  <button title="סגירה" type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                  </button>

                                </div>
                             </div>

                            

                            <div class="col-lg-2">
                                <asp:DropDownList runat="server" ID="drpType" OnSelectedIndexChanged="drpType_SelectedIndexChanged" AutoPostBack="true" >
                                </asp:DropDownList>
                            </div>

                            <div class="col-lg-6">
                                <h2>עריכת תבניות מייל</h2>
                            </div>

                        </div>
                    </div>
                    <div class="row text-right">
                        <div class="col-lg-12">
                            <label>נושא</label>
                            <asp:TextBox runat="server" id="txtSubject" required2="required2"  CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-12">
                            <div style="height:500px">
                                <textarea runat="server" id="editorcontent" style="height:480px;">&lt;div&gt;היי |שם|,&lt;/div&gt;&#xA;&lt;div&gt;ברוכ/ה&amp;nbsp;הבא/ה&amp;nbsp;לד&quot;ר לוגי !&lt;/div&gt;&#xA;&lt;div&gt;בהתאם להודעה שקיבלנו מביטוח לאומי, מעדכנת&amp;nbsp; גם כאן בצורה מסודרת :&lt;/div&gt;&#xA;&lt;div&gt;&amp;nbsp;&lt;/div&gt;&#xA;&lt;div&gt;אושרה לך זכאות להנגשות בין התאריכים&amp;nbsp;&lt;span style=&quot;color: #000000;&quot;&gt;|התחלה| ו&lt;/span&gt;עד ה-|סוף|.&lt;/div&gt;&#xA;&lt;div&gt;&amp;nbsp;&lt;/div&gt;&#xA;&lt;div&gt;|הנגשות|&lt;br&gt;&lt;br&gt;&lt;strong&gt;בסמסטר זה, רכזת בשם |רכז| תהיה אחראית על השיבוץ שלך ותיתן לך מענה.&lt;/strong&gt;&lt;/div&gt;&#xA;&lt;div&gt;&lt;strong&gt;&lt;br&gt;&lt;/strong&gt;&lt;strong&gt;המס שלה&amp;nbsp; הוא -|טלפון רכז|&lt;/strong&gt;&lt;/div&gt;&#xA;&lt;div&gt;&amp;nbsp;&lt;/div&gt;&#xA;&lt;div&gt;אנחנו רוצים להתחיל ולעבוד כדי למצוא לך תלמיד מתאים בהקדם, נשמח שתעביר/י לרכזת המטפלת סילבוסים בקורסים בהם את/ה זקוק/ה לסיוע&amp;nbsp; .&amp;nbsp;&lt;br&gt;|הערות|&lt;br&gt;&lt;br&gt;&lt;/div&gt;&#xA;&lt;div&gt;לתשומת&amp;nbsp;&lt;img class=&quot;an1&quot; src=&quot;https://fonts.gstatic.com/s/e/notoemoji/15.0/2764/32.png&quot; alt=&quot;❤&quot; loading=&quot;lazy&quot; data-emoji=&quot;❤&quot; aria-label=&quot;❤&quot;&gt;:&amp;nbsp;&lt;/div&gt;&#xA;&lt;div&gt;*אין לחרוג מתקופת הזכאות הכוללת וממכסת השעות החודשית.&amp;nbsp;&lt;/div&gt;&#xA;&lt;div&gt;*אין להעביר שעות מחודש לחודש...&lt;/div&gt;&#xA;&lt;div&gt;* אין לבצע מעל 5 שעות של שיעורי עזר ליום .&amp;nbsp;&lt;/div&gt;&#xA;&lt;div&gt;&amp;nbsp;&lt;/div&gt;&#xA;&lt;div&gt;&#xA;&lt;div&gt;&lt;strong&gt;&lt;span style=&quot;color: #20124d;&quot;&gt;מדיניות ביטולים:&lt;/span&gt;&lt;/strong&gt;&lt;br&gt;&lt;img class=&quot;an1&quot; src=&quot;https://fonts.gstatic.com/s/e/notoemoji/15.0/25aa_fe0f/32.png&quot; alt=&quot;▪️&quot; loading=&quot;lazy&quot; data-emoji=&quot;▪️&quot; aria-label=&quot;▪️&quot;&gt;&amp;nbsp;ניתן לבטל שיעור עד 24 שעות לפני תחילת השיעור, במידה והשיעור בוטל פחות מ24 שעות לפני מועד התחלתו יחשבו השעות המבוטלות כשיעור לכל דבר, ועל הסטודנט לחתום לתלמיד כאילו השיעור בוצע.&lt;/div&gt;&#xA;&lt;div&gt;&lt;img class=&quot;an1&quot; src=&quot;https://fonts.gstatic.com/s/e/notoemoji/15.0/25aa_fe0f/32.png&quot; alt=&quot;▪️&quot; loading=&quot;lazy&quot; data-emoji=&quot;▪️&quot; aria-label=&quot;▪️&quot;&gt;&amp;nbsp;נא להקפיד לחתום לתלמיד בסוף כל שיעור על הקישור שישלח אליכם ולא להמתין לסוף החודש.&lt;/div&gt;&#xA;&lt;div&gt;&amp;nbsp;&lt;/div&gt;&#xA;&lt;div&gt;&lt;strong&gt;&lt;span style=&quot;font-family: arial, sans-serif;&quot;&gt;בברכה,&lt;br&gt;אוריין עובדיה&lt;br&gt;ד&quot;ר לוגי השכלה בע&quot;מ&lt;/span&gt;&lt;/strong&gt;&lt;/div&gt;&#xA;&lt;/div&gt;</textarea>
                            </div>
                        </div>
                    </div>


                    <div class="table-title">
                        <div class="row">
                            <div class="col-lg-6">
                                <asp:LinkButton ToolTip="שמירה" runat="server" ID="btnUpdate" OnClick="btnUpdate_Click"> <div class="btn btn-success"><i class="material-icons">&#xE161;</i> <span>שמירה</span></div></asp:LinkButton>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>

</body>
</html>
