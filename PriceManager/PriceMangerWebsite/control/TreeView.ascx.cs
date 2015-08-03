using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Collections;

public partial class TreeView : System.Web.UI.UserControl
{
    private string nameColumn = "Text";
    private string idColumn = "Id";
    private string parentIdColumn = "ParentId";
    private string navigateUrlColumn = "NavigateUrl";
    private string checkedColumn = "Checked";
    private string custom1Column = "Custom1";
    private string custom2Column = "Custom2";
    private string custom3Column = "Custom3";
    private bool enableDragDrop = false;
    private bool enableCheckBox = false;
    private bool enableContextMenu = false;
    private bool adminMode = false;
    protected string theme = "default";

    public TreeViewTheme Theme
    {
        get
        {
            if (theme == "default")
                return TreeViewTheme.Default;
            else if (theme == "apple")
                return TreeViewTheme.Apple;
            else if (theme == "classic")
                return TreeViewTheme.Classic;
            else
                return TreeViewTheme.Default;
        }
        set
        {
            if (value == TreeViewTheme.Default)
                theme = "default";
            else if (value == TreeViewTheme.Apple)
                theme = "apple";
            else if (value == TreeViewTheme.Classic)
                theme = "classic";
        }
    }
    public string CheckedColumn
    {
        get { return checkedColumn; }
        set { checkedColumn = value; }
    }
    public string NameColumn
    {
        get { return nameColumn; }
        set { nameColumn = value; }
    }
    public string IdColumn
    {
        get { return idColumn; }
        set { idColumn = value; }
    }
    public string ParentIdColumn
    {
        get { return parentIdColumn; }
        set { parentIdColumn = value; }
    }
    public string NavigateUrlColumn
    {
        get { return navigateUrlColumn; }
        set { navigateUrlColumn = value; }
    }
    public string Custom1Column
    {
        get { return custom1Column; }
        set { custom1Column = value; }
    }
    public string Custom2Column
    {
        get { return custom2Column; }
        set { custom2Column = value; }
    }
    public string Custom3Column
    {
        get { return custom3Column; }
        set { custom3Column = value; }
    }
    public bool EnableDragDrop
    {
        get { return enableDragDrop; }
        set { enableDragDrop = value; }
    }
    public bool EnableContextMenu
    {
        get { return enableContextMenu; }
        set { enableContextMenu = value; }
    }
    public bool EnableCheckBox
    {
        get { return enableCheckBox; }
        set { enableCheckBox = value; }
    }
    public bool AdminMode
    {
        get { return adminMode; }
        set { adminMode = value; }
    }
    public string TreeXML = string.Empty;
    public event EventHandler<CustomTreeNodeEventArgs> NodeDrop;
    public event EventHandler<CustomTreeNodeEventArgs> NodeAdd;
    public event EventHandler<CustomTreeNodeEventArgs> NodeUpdate;
    public event EventHandler<CustomTreeNodeEventArgs> NodeDelete;
    public List<TreeNode> TreeNodes = new List<TreeNode>();

    protected override void OnLoad(EventArgs e)
    {
        // Code used to trigger javascript function before page post, so we can make the tree information saved in hidden field.
        base.OnLoad(e);
        ScriptManager.RegisterOnSubmitStatement(Page, Page.GetType(), "SubmitHandlerKey" + demo1.ClientID, "OnSubmitCustom" + demo1.ClientID + "()");
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            // get the tree information sent by hidden field, and update the TreeNodes List
            string tree = Request.Form[hfTree.ClientID.Replace('_', '$')]; // controls values are not available in Page_Init, so Request object is used
            if (tree != string.Empty)
                UpdateTreeClass(tree);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Code used to persist information in treeview during postbacks.
        if (IsPostBack)
        {
            string treeXML = Request.Form[hfTreeXML.ClientID.Replace('_', '$')];
            if (treeXML != string.Empty)
                TreeXML = treeXML.Replace('{', '<').Replace('}', '>').Replace("<ins*>", "");
        }
    }

    public void SetDataSource(DataSet dSet)
    {
        dSet = SetAdminMode(dSet);
        string xml = dSet.GetXml(); // convert dataset to xml
        xml = ReplaceColumnNames(xml);
        TreeXML = ExecuteXSLTransformation(xml); // convert xml to required html format, UL and LI format. TreeXML is being written in HTML using server tags.
        SetTreeNodes(dSet); // creates the TreeNodes list for server side use
    }

    private DataSet SetAdminMode(DataSet dSet)
    {
        if (adminMode == true)
        {
            if (dSet.Tables[0].Columns.Contains(navigateUrlColumn))
                dSet.Tables[0].Columns.Remove(navigateUrlColumn);
        }
        return dSet;
    }

    private string ReplaceColumnNames(string xml)
    {
        xml = xml.Replace(nameColumn + ">", "Text>");
        xml = xml.Replace(idColumn + ">", "Id>");
        xml = xml.Replace(navigateUrlColumn + ">", "NavigateUrl>");
        xml = xml.Replace(parentIdColumn + ">", "ParentId>");
        xml = xml.Replace(nameColumn + ">", "Text>");
        xml = xml.Replace(checkedColumn + ">", "Checked>");
        xml = xml.Replace(custom1Column + ">", "Custom1>");
        xml = xml.Replace(custom2Column + ">", "Custom2>");
        xml = xml.Replace(custom3Column + ">", "Custom3>");
        return xml;
    }

    private void SetTreeNodes(DataSet dSet)
    {
        foreach (DataRow dRow in dSet.Tables[0].Rows)
        {
            // create tree nodes based on dataset rows
            TreeNode treeNode = new TreeNode();

            treeNode.Id = dRow[idColumn] == null ? string.Empty : dRow[idColumn].ToString();
            treeNode.Name = dRow[nameColumn] == null ? string.Empty : dRow[nameColumn].ToString();
            treeNode.ParentId = dRow[parentIdColumn] == null ? string.Empty : dRow[parentIdColumn].ToString();
            treeNode.IsChecked = (dRow[checkedColumn] == null || dRow[checkedColumn].ToString() == string.Empty) ? false : Convert.ToBoolean(dRow[checkedColumn].ToString());
            if (adminMode == false)
                treeNode.NavigateUrl = dRow[navigateUrlColumn].ToString() == string.Empty ? string.Empty : dRow[navigateUrlColumn].ToString();
            else
            {
                treeNode.NavigateUrl = "#";
            }
            if (dSet.Tables[0].Columns.Contains(custom1Column))
                treeNode.Custom1 = dRow[custom1Column] == null ? string.Empty : dRow[custom1Column].ToString();
            if (dSet.Tables[0].Columns.Contains(custom2Column))
                treeNode.Custom2 = dRow[custom2Column] == null ? string.Empty : dRow[custom2Column].ToString();
            if (dSet.Tables[0].Columns.Contains(custom3Column))
                treeNode.Custom3 = dRow[custom3Column] == null ? string.Empty : dRow[custom3Column].ToString();

            TreeNodes.Add(treeNode);
        }
    }

    protected void btnOnDrop_Click(object sender, EventArgs e)
    {
        CustomTreeNodeEventArgs tEventArgs = new CustomTreeNodeEventArgs();
        tEventArgs.Position = hf1.Value == string.Empty ? 0 : int.Parse(hf1.Value);
        tEventArgs.ParentId = hf2.Value;
        NodeDrop(sender, tEventArgs);
    }

    protected void btnOnAdd_Click(object sender, EventArgs e)
    {
        CustomTreeNodeEventArgs tEventArgs = new CustomTreeNodeEventArgs();
        tEventArgs.Name = hf1.Value;
        tEventArgs.Position = int.Parse(hf2.Value);
        tEventArgs.ParentId = hf3.Value;
        NodeAdd(sender, tEventArgs);
    }

    protected void btnOnDelete_Click(object sender, EventArgs e)
    {
        CustomTreeNodeEventArgs tEventArgs = new CustomTreeNodeEventArgs();
        tEventArgs.Id = hf1.Value;
        NodeDelete(sender, tEventArgs);
    }

    protected void btnOnUpdate_Click(object sender, EventArgs e)
    {
        CustomTreeNodeEventArgs tEventArgs = new CustomTreeNodeEventArgs();
        tEventArgs.Name = hf1.Value;
        tEventArgs.Id = hf2.Value;
        NodeUpdate(sender, tEventArgs);
    }

    public string GetPluginText()
    {
        // this method is used for jstree plugins activation, based on the user control properties.
        string text = string.Empty;
        if (enableDragDrop == true)
            text += ", 'dnd'";
        if (enableCheckBox == true)
            text += ",'checkbox' ";
        if (enableContextMenu == true)
            text += ",'crrm', 'contextmenu'";

        return text;

    }

    public string ExecuteXSLTransformation(string xml)
    {
        string HtmlTags, XsltPath;
        MemoryStream DataStream = default(MemoryStream);
        StreamReader streamReader = default(StreamReader);


        //Path of XSLT file
        XsltPath = HttpContext.Current.Server.MapPath("~/control/TreeXML.xslt");

        //Encode all Xml format string to bytes
        byte[] bytes = Encoding.ASCII.GetBytes(xml);
        DataStream = new MemoryStream(bytes);

        //Create Xmlreader from memory stream
        XmlReader reader = XmlReader.Create(DataStream);

        // Load the XML
        XPathDocument document = new XPathDocument(reader);
        XslCompiledTransform XsltFormat = new XslCompiledTransform();

        // Load the style sheet.
        XsltFormat.Load(XsltPath);
        DataStream = new MemoryStream();
        XmlTextWriter writer = new XmlTextWriter(DataStream, Encoding.ASCII);

        //Apply transformation from xml format to html format and save it in xmltextwriter
        XsltFormat.Transform(document, writer);
        streamReader = new StreamReader(DataStream);
        DataStream.Position = 0;
        HtmlTags = streamReader.ReadToEnd();
        //Release the resources
        streamReader.Close();
        DataStream.Close();
        return HtmlTags;
    }

    private void UpdateTreeClass(string tree)
    {
        // this method splits the received string from client side, and updates the TreeNodes List for server side use.
        if (tree != null && tree != string.Empty)
        {
            string[] trees = tree.Split(new string[] { "|,|" }, StringSplitOptions.None);

            for (int i = 0; i < trees.Length; i++)
            {
                string[] attr = trees[i].Split(new string[] { "|:|" }, StringSplitOptions.None);

                TreeNode treeNode = new TreeNode();
                treeNode.Id = attr[0];
                treeNode.Name = attr[1];
                treeNode.ParentId = attr[2];
                treeNode.NavigateUrl = attr[3];
                treeNode.IsChecked = Convert.ToBoolean(attr[4]);
                treeNode.Custom1 = attr[5];
                treeNode.Custom2 = attr[6];
                treeNode.Custom3 = attr[7];

                TreeNodes.Add(treeNode);
            }
        }
    }
}

// class used to create tree node values passed to the event handler
public class CustomTreeNodeEventArgs : EventArgs
{
    private string name = string.Empty;
    private string id = string.Empty;
    private int position = 0;
    private string parentId = string.Empty;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Id
    {
        get { return id; }
        set { id = value; }
    }
    public int Position
    {
        get { return position; }
        set { position = value; }
    }
    public string ParentId
    {
        get { return parentId; }
        set { parentId = value; }
    }
}

// class used to contain tree nodes information.
[Serializable]
public class TreeNode
{
    private string name = string.Empty;
    private string id = string.Empty;
    private int position = 0;
    private string parentId = string.Empty;
    private string navigateUrl = string.Empty;
    private bool isChecked = false;
    private string custom1 = string.Empty;
    private string custom2 = string.Empty;
    private string custom3 = string.Empty;

    public bool IsChecked
    {
        get { return isChecked; }
        set { isChecked = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Id
    {
        get { return id; }
        set { id = value; }
    }
    public int Position
    {
        get { return position; }
        set { position = value; }
    }
    public string NavigateUrl
    {
        get { return navigateUrl; }
        set { navigateUrl = value; }
    }
    public string ParentId
    {
        get { return parentId; }
        set { parentId = value; }
    }
    public string Custom1
    {
        get { return custom1; }
        set { custom1 = value; }
    }
    public string Custom2
    {
        get { return custom2; }
        set { custom2 = value; }
    }
    public string Custom3
    {
        get { return custom3; }
        set { custom3 = value; }
    }
}

public enum TreeViewTheme
{
    Default = 1,
    Apple = 2,
    Classic = 3
}