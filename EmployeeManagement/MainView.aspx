<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainView.aspx.cs" Inherits="EmployeeManagement.MainView" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>社員管理システム</title>
    <link rel="stylesheet" type="text/css" href="MainView.css"/>
</head>
<script type="text/javascript">
    //スクロール保持処理
    function setScroll() {
        document.getElementById("<%=Grid_Panel.ClientID%>").scrollTop = document.getElementById("<%=hidScroll.ClientID%>").value;
    }

    function keepScroll() {
        document.getElementById("<%=hidScroll.ClientID%>").value = document.getElementById("<%=Grid_Panel.ClientID%>").scrollTop;
    }

    //入力チェック
    function checkTxt()
    {
        var check_txt = String.fromCharCode(event.keyCode);
        //¿は「/」
        if (("0123456789\b\r¿&%'(").indexOf(check_txt, 0) < 0)
        {
            return false;
        }
        return true;
    }

</script>
<body onload="setScroll()">
    <form id="form1" runat="server" autocomplete="off">
    <input id="hidScroll" type="hidden" runat="server" value="0"/> 
    <div>
    
    </div>
        <asp:Panel ID="Main_Panel" CssClass="Panel" runat="server" Height="640px"  >
            <asp:Panel ID="Grid_Panel" runat="server" style="OVERFLOW: auto; Height:640px; Width:905px" onscroll="keepScroll()">
                <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" >
                </asp:ScriptManager>
                <asp:GridView ID="Emp_Grid" runat="server" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" OnSelectedIndexChanged="Emp_Grid_SelectedIndexChanged" AutoGenerateColumns="False" Width="887px" OnRowDataBound="Emp_Grid_RowDataBound">
                <Columns>
                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" />
                    <asp:BoundField HeaderText="社員番号" DataField="EMP_NUMBER" />
                    <asp:BoundField DataField="NAME" HeaderText="社員名" SortExpression="NAME" />
                    <asp:BoundField DataField="AGE" HeaderText="年齢" SortExpression="AGE" />
                    <asp:BoundField DataField="BIRTHDAY" DataFormatString="{0:yyyy/MM/dd}" HeaderText="生年月日" SortExpression="BIRTHDAY" />
                    <asp:BoundField DataField="JOIN_YEAR" DataFormatString="{0:yyyy/MM/dd}" HeaderText="入社年月日" SortExpression="JOIN_YEAR" />
                    <asp:BoundField DataField="CON_YEAR" HeaderText="勤続年数" SortExpression="CON_YEAR" />
                    <asp:BoundField DataField="TERMINATION_DATE" DataFormatString="{0:yyyy/MM/dd}" HeaderText="退職年月日" SortExpression="TERMINATION_DATE" />
                    <asp:BoundField DataField="SKILL" HeaderText="得意スキル" SortExpression="SKILL" />
                    <asp:BoundField DataField="SKILL_2nd" HeaderText="得意スキル２" SortExpression="SKILL_2nd" />
                    <asp:BoundField DataField="SKILL_3rd" HeaderText="得意スキル３" SortExpression="SKILL_3rd" />
                </Columns>
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <HeaderStyle BackColor="#003399" CssClass="Freezing" Font-Bold="True" ForeColor="#CCCCFF" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                <SortedDescendingHeaderStyle BackColor="#002876" />
            </asp:GridView>
            </asp:Panel>
            <br />
            <asp:Panel ID="Sub_Panel" CssClass="Panel" runat="server" Height="638px" Width="420px">
                <asp:Panel ID="Add_Panel" CssClass="Panel" runat="server" Height="207px">

                    <asp:Label ID="Add_Label" runat="server"></asp:Label>
                    <asp:Label ID="Add_error_Label" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="Add_Name_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Add_Name_Text" CssClass="Name_Text" runat="server" Width="130px"></asp:TextBox>
                    <asp:Label ID="Add_Name_Error" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="Add_Brithday_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Add_Brithday_Text" CssClass="Brithday_Text" runat="server" Width="130px"></asp:TextBox>
                    <asp:Label ID="Add_Brithday_Error" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="Add_Join_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Add_Join_Text" CssClass="Join_Text" runat="server" Width="130px"></asp:TextBox>
                    <asp:Label ID="Add_Join_Error" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="Add_Skill_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Add_Skill_Text"  CssClass="Skill_Text" runat="server" Width="130px"></asp:TextBox>
                    <br />
                    <asp:TextBox ID="Add_Skill_Text2" CssClass="Skill_Text Skill_left" runat="server" Width="130px"></asp:TextBox>
                    <br />
                    <asp:TextBox ID="Add_Skill_Text3" CssClass="Skill_Text Skill_left" runat="server" Width="130px"></asp:TextBox>
                    <br />
                    <asp:Button ID="Add_Button" runat="server" Height="38px" Text="追加" Width="98px" OnClick="Add_Button_Click" ValidationGroup="Add_Error"/> 
                    <asp:Button ID="Add" runat="server" CssClass="hide" OnClick="Add_Click" Text="Button" />
                </asp:Panel>
                <asp:Panel ID="Up_Panel" CssClass="Panel" runat="server" Height="207px">

                    <asp:Label ID="Up_Label" runat="server"></asp:Label>
                    <asp:Label ID="Up_error_Label" CssClass="Error_Label" runat="server" ForeColor="Red"></asp:Label>

                    <br />
                    <br />
                    <asp:Label ID="Up_Name_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Up_Name_Text" CssClass="Name_Text" runat="server" Width="130px"></asp:TextBox>
                    <br />
                    <asp:Label ID="Up_Brithday_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Up_Brithday_Text" CssClass="Brithday_Text" onkeyDown="return checkTxt()" runat="server" Width="130px"></asp:TextBox>
                    <asp:Label ID="Up_Brithday_Error" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="Up_Join_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Up_Join_Text" CssClass="Join_Text" onkeyDown="return checkTxt()" runat="server" Width="130px"></asp:TextBox>
                    <asp:Label ID="Up_Join_Error" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="Up_Skill_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Up_Skill_Text" CssClass="Skill_Text" runat="server" Width="130px"></asp:TextBox>
                    <br />
                    <asp:TextBox ID="Up_Skill_Text2" CssClass="Skill_Text Skill_left" runat="server" Width="130px"></asp:TextBox>
                    <br />
                    <asp:TextBox ID="Up_Skill_Text3" CssClass="Skill_Text Skill_left" runat="server" Width="130px"></asp:TextBox>
                    <br />
                    <asp:Button ID="Up_Button" runat="server" Height="38px" Text="更新" Width="98px" OnClick="Up_Button_Click" ClientIDMode="Inherit"/>
                    
                    <asp:Button ID="UPDATE" CssClass="hide" runat="server" Text="Button" OnClick="UPDATE_Click" />
                    
                </asp:Panel>
                <asp:Panel ID="Del_Panel" CssClass="Panel" runat="server" Height="207px">

                    <asp:Label ID="Del_Label" runat="server"></asp:Label>
                    <asp:Label ID="Del_error_Label" CssClass="Error_Label" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="Del_Number_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Del_Number_Text" CssClass="Number_Text" runat="server" Width="130px"></asp:TextBox>
                    <br />
                    <asp:Label ID="Del_Retirement_Label" runat="server"></asp:Label>
                    <asp:TextBox ID="Del_Retirement_Text" runat="server" Width="130px" onkeyDown="return checkTxt()"></asp:TextBox>
                    <asp:Label ID="Del_Retirement_Error" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="Del_Button" runat="server" Height="38px" Text="削除" Width="98px" OnClick="Del_Button_Click" />
                    <asp:Label ID="Emp_Count_Label" runat="server"></asp:Label>
                    <asp:Label ID="Emp_Count" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="Del_Show_Button" runat="server" Text="退職者表示" Height="38px" Width="98px" OnClick="Del_Show_Button_Click"/>
                    <asp:Button ID="DELETE_button" runat="server" CssClass="hide" OnClick="DELETE_button_Click" Text="Button" />
                    <br />
                    <br />
                </asp:Panel>

            </asp:Panel>
        </asp:Panel>
    </form>
</body>
</html>
