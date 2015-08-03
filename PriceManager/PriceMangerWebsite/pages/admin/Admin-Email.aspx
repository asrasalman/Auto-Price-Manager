<%@ Page Title="" Language="C#" MasterPageFile="~/master/Main.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="Admin-Email.aspx.cs" Inherits="pages_admin_Admin_Email" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rt-containerInner">
        <div class="rt-grid-12 ">
            <div class="rt-block">
                <div class='emailContainer'>
                    <div class="componentheading">
                        <h2>
                            Send Email To Users</h2>
                    </div>
                    <p>
                        You can select 1 or more active or in-active users of Parcel Solutions and send
                        them a customized email.
                    </p>
                    <asp:Label ID="lblMessage" runat="server" CssClass="emailMessage" Visible="false"></asp:Label>
                    <div>
                        <div class='userListEmail'>
                            <div class='listHeading'>
                                Active Users</div>
                            <asp:CheckBoxList ID="chklActiveUsers" runat="server">
                            </asp:CheckBoxList>
                        </div>
                        <div class='userListEmail'>
                            <div class='listHeading'>
                                In-Active Users</div>
                            <asp:CheckBoxList ID="chklInactiveUsers" runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <h3>
                        Email Body
                     
                    </h3>
                    <br />
                    <p>
                        Subject:
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="txtSubject inputbox" Width="500px"></asp:TextBox>
                    </p>
                    <p>
                        <textarea id="txtEmailBody" name="txtEmailBody" rows="15" cols="80" style="width: 80%"
                            class="tinymce" runat="server">
                        </textarea>
                    </p>
                    
                    <p>
                        <asp:Button ID="btnSendEmail" runat="server" OnClientClick="return CheckEmailSend()"
                            CssClass="button" Text="Send Email" OnClick="btnSendEmail_Click" />
                    </p>
                </div>
            </div>
        </div>
    </div>
    <script src="/Scripts/lib/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/lib/jquery-ui-1.8.22.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            SetEditor();
        });

        function SetEditor() {
            $('textarea.tinymce').tinymce({
                // Location of TinyMCE script
                script_url: '/scripts/tiny_mce/tiny_mce.js',

                // General options
                theme: "advanced",
                plugins: "style,emotions,preview,paste,visualchars",

                // Theme options
                theme_advanced_buttons1: ",bold,italic,underline,|,bullist,numlist,|,outdent,indent,|,forecolor,backcolor,|,justifyleft,justifycenter,justifyright,justifyfull,fontselect,fontsizeselect,emotions,advimage,media",
                theme_advanced_buttons2: "",
                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left",
                theme_advanced_resizing: false,

                // Example content CSS (should be your site CSS)
                content_css: "/includes/css/style.css",

                // Drop lists for link/image/media/template dialogs
                template_external_list_url: "lists/template_list.js",
                external_link_list_url: "lists/link_list.js",
                external_image_list_url: "lists/image_list.js",
                media_external_list_url: "lists/media_list.js",

                // Replace values for the template plugin
                template_replace_values: {
                    username: "Some User",
                    staffid: "991234"
                }
            });
        }

        function CheckEmailSend() {
            // check recepient count.

            var checked = $('.userListEmail').find('input[type="checkbox"]:checked').length;
            if (checked == 0) {
                alert('Please select atleast 1 user to send the email');
                return false;
            }

            if ($('.txtSubject').val() == '') {
                alert('Email subject cannot be empty.');
                return false;
            }

            if ($('textarea.tinymce').val() == '') {
                alert('Email body cannot be empty.');
                return false;
            }

            if (confirm('Are you sure you want to send this email to the selected users?')) {

            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
