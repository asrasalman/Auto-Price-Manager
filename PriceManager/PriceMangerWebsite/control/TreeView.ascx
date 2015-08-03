<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TreeView.ascx.cs" Inherits="TreeView" %>

<link href="/scripts/lib/treeview/_docs/!style.css" rel="stylesheet" type="text/css" />
<link href="/scripts/lib/treeview/themes/classic/style.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" src="/scripts/lib/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="/scripts/lib/jquery-ui-1.8.22.custom.min.js"></script>
<script src="/scripts/lib/treeview/jquery.jstree.js" type="text/javascript"></script>


<div id="demo1" class="demo" runat="server">
    <%= TreeXML %>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Button ID="btnOnDrop" runat="server" Text="Button" Style="display: none" CssClass="OnDropButton"
            OnClick="btnOnDrop_Click" />
        <asp:Button ID="btnOnAdd" runat="server" Text="Button" Style="display: none" CssClass="OnAddButton"
            OnClick="btnOnAdd_Click" />
        <asp:Button ID="btnOnDelete" runat="server" Text="Button" Style="display: none" CssClass="OnDeleteButton"
            OnClick="btnOnDelete_Click" />
        <asp:Button ID="btnOnUpdate" runat="server" Text="Button" Style="display: none" CssClass="OnUpdateButton"
            OnClick="btnOnUpdate_Click" />
        <asp:HiddenField ID="hf1" runat="server" />
        <asp:HiddenField ID="hf2" runat="server" />
        <asp:HiddenField ID="hf3" runat="server" />
        <asp:HiddenField ID="hfTree" runat="server" />
        <asp:HiddenField ID="hfTreeXML" runat="server" />
        <input id="hfAdminMode" type="hidden" value='<%= AdminMode %>' />
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">

      
        $("#<%= demo1.ClientID %>")
        .bind("move_node.jstree", function (e, data) { 
                $('#<%= hf1.ClientID %>').val(data.rslt.cp);
                $('#<%= hf2.ClientID %>').val(data.rslt.np.attr('value'));
                $('#<%= btnOnDrop.ClientID %>').click();
        })
        .bind("create.jstree", function (e,data) {
                $('#<%= hf1.ClientID %>').val(data.rslt.name); // node name 
                $('#<%= hf2.ClientID %>').val(data.rslt.position); // node position
                $('#<%= hf3.ClientID %>').val(data.rslt.parent.attr('id')); // node Parent Id
                $('#<%= btnOnAdd.ClientID %>').click();
		})
        .bind("delete_node.jstree", function (e,data) {
                $('#<%= hf1.ClientID %>').val(data.rslt.obj.attr('id')); // node Id to delete
                $('#<%= btnOnDelete.ClientID %>').click();
		})
        .bind("rename_node.jstree", function (e,data) {
                if(data.rslt.obj.attr('id') != '' && data.rslt.obj.attr('id') != null)
                {
			            $('#<%= hf1.ClientID %>').val(data.rslt.name); // node name 
                        $('#<%= hf2.ClientID %>').val(data.rslt.obj.attr('id')); // node Id
                        $('#<%= btnOnUpdate.ClientID %>').click();
                }
		})
       .bind("dblclick.jstree", function (e,data) {
           if($('#hfAdminMode').val() == 'True')
           {
                    var node = $(event.target).closest("li");
                    $("#<%= demo1.ClientID %>").jstree("rename",node);
		    }
        })
        .jstree({
            "themes" : {
			        "theme" : "<%= theme %>",
			        "dots" : true,
			        "icons" : false,
		        },
            "plugins": ['themes','html_data' <%= GetPluginText()  %>]
        });


    function OnSubmitCustom<%= demo1.ClientID %>()
    {
        var s = $('#<%= demo1.ClientID %>').find('li').map(
        function() 
        { 
            var id = $(this).attr('id'); 
            var text = $(this).children('a').text().trim();
            var parentId = $(this).attr('parentId'); 
            var link = $(this).attr('href'); 
            var checked = $(this).hasClass('jstree-checked');
            var custom1 = $(this).attr('custom1'); 
            var custom2 = $(this).attr('custom2'); 
            var custom3 = $(this).attr('custom3'); 

            return id + '|:|' + text + '|:|' + parentId + '|:|' + link + '|:|' + checked + '|:|' + custom1 + '|:|' + custom2 + '|:|' + custom3;
        }
        ).get().join('|,|');

        $('#<%= hfTree.ClientID %>').val(s);
        
        var s2 = $('#<%= demo1.ClientID %>').clone();
        $(s2).find('ins').remove();
        $('#<%= hfTreeXML.ClientID %>').val($(s2).html().replace(/</g,'{').replace(/>/g,'}'));
        $(s2).remove();
    }

    function pageLoad() {
         $('a[href=""]').attr('href','#');
    }

</script>
