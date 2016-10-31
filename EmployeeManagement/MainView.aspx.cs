using System;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeManagement
{
    public partial class MainView : System.Web.UI.Page
    {
        //定数
        #region
        //入力項目表示用
        public const String ADD_TITLE = "登録情報";
        public const String UP_TITLE = "更新情報";
        public const String DEL_TITLE = "削除情報";

        public const String EMP_NUMBER = "社員番号";
        public const String EMP_NAME = "社員名";
        public const String EMP_OLD = "年齢";
        public const String EMP_BRITHDAY = "生年月日";
        public const String EMP_JOIN = "入社年月日";
        public const String EMP_CON = "勤続年月";
        public const String EMP_SKILL = "得意スキル";
        public const String EMP_SKILL_2nd = "得意スキル2";
        public const String EMP_SKILL_3rd = "得意スキル3";
        public const String EMP_RETIREMENT = "退職年月日";
        public const String EMP_ENGINEER_COUNT = "技術者数：";
        public const String EMP_RETIREE_COUNT = "退職者数:";

        public const String EMP_ID = "T00000";
        public const String DEFAULT_SKILL = "未設定";
        public const String TRUE_TEXT = "退職者表示";
        public const String FALSE_TEXT = "正社員表示";

        public const String ERROR_LABEL = "※入力必須";
        public const String ERROR_TEXT = "※入力内容に誤りがあります";
        public const String DATE_ERROR = "※日付を入力してください";
        public const String FUTURE_ERROR = "※未来は入力出来ません";
        public const String PAST_BRITHDAY_ERROR = "※1900/01/01から入力可";
        public const String PAST_JOIN_ERROR = "※2005/04/01から入力可";
        public const String MIN_BRITHDAY_DATE = "1900/01/01";
        public const String MIN_JOIN_DATE = "2005/04/01";

        //退職者表示フラグ初期設定
        public const Boolean DEL_FLG = false;
        #endregion

        //変数
        #region
        DateTime dt;             //日付型の宣言
        String JsScript;         //クライアント側で実行するjavascript    

        String number;           //社員番号
        String name;             //名前
        int age;                 //年齢
        DateTime brithday;       //生年月日
        DateTime join;           //入社年月日
        String continuous;       //勤続年月
        String skill;            //得意スキル
        String skill_2nd;        //得意スキル2
        String skill_3rd;        //得意スキル3
        String termination_date; //退職日

        int count;               //社員数

        Boolean del_flg = false;    //退職者表示フラグ
        Boolean error_flg = false;  //入力エラーフラグ
        Boolean update_flg = false; //更新フラグ
        #endregion

        //インスタンス生成
        #region
        Hashtable select_hash = new Hashtable();    //選択された社員データ格納用
        MethodClass mc = new MethodClass();         //表示項目自動生成用クラス
        #endregion

        //画面ロード時
        protected void Page_Load(object sender, EventArgs e)
        {
            //ポストバック
            #region
            if (IsPostBack == true && (Boolean)ViewState["del_flg"] == true)
            {
                //退職者表示時
                Emp_Count_Label.Text = EMP_RETIREE_COUNT;
                return;
            }

            if (IsPostBack == true)
            {   
                //正社員表示時
                return;
            }
            #endregion
            //表示ラベル初期化
            #region
            //エラー大項目初期化
            Add_error_Label.Text = "";
            Up_error_Label.Text = "";
            Del_error_Label.Text = "";

            //更新情報エラー表示初期化
            Up_Brithday_Error.Text = "";
            Up_Join_Error.Text = "";
            #endregion
            //大項目表示
            #region
            Add_Label.Text = ADD_TITLE;
            Up_Label.Text = UP_TITLE;
            Del_Label.Text = DEL_TITLE;
            #endregion
            //入力項目表示
            #region
            //追加
            Add_Name_Label.Text = EMP_NAME;
            Add_Brithday_Label.Text = EMP_BRITHDAY;
            Add_Join_Label.Text = EMP_JOIN;
            Add_Skill_Label.Text = EMP_SKILL;
            Add_Skill_Label2.Text = EMP_SKILL_2nd;
            Add_Skill_Label3.Text = EMP_SKILL_3rd;

            //更新
            Up_Name_Label.Text = EMP_NAME;
            Up_Brithday_Label.Text = EMP_BRITHDAY;
            Up_Join_Label.Text = EMP_JOIN;
            Up_Skill_Label.Text = EMP_SKILL;
            Up_Skill_Label2.Text = EMP_SKILL_2nd;
            Up_Skill_Label3.Text = EMP_SKILL_3rd;

            //削除
            Del_Number_Label.Text = EMP_NUMBER;
            Del_Retirement_Label.Text = EMP_RETIREMENT;

            //技術者数
            Emp_Count_Label.Text = EMP_ENGINEER_COUNT;

            //更新機能
            Up_Name_Text.Enabled = false;
            Up_Brithday_Text.Enabled = false;
            Up_Join_Text.Enabled = false;
            Up_Skill_Text.Enabled = false;
            Up_Skill_Text2.Enabled = false;
            Up_Skill_Text3.Enabled = false;
            Up_Button.Enabled = false;

            //削除機能
            Del_Number_Text.Enabled = false;
            Del_Retirement_Text.Enabled = false;
            Del_Button.Enabled = false;

            #endregion
            //表に表示
            selectSQL();

            ViewState["del_flg"] = del_flg;
            ViewState["update_flg"] = update_flg;
        }

        //追加処理
        #region
        //追加ボタンクリック
        protected void Add_Button_Click(object sender, EventArgs e)
        {
            //表示初期化
            #region
            clearError();

            //更新項目
            Up_Name_Text.Text = "";
            Up_Brithday_Text.Text = "";
            Up_Join_Text.Text = "";
            Up_Skill_Text.Text = "";
            Up_Skill_Text2.Text = "";
            Up_Skill_Text3.Text = "";

            //削除項目
            Del_Number_Text.Text = "";

            #endregion

            try
            {
                //入力された値の整合性チェック
                #region
                //社員名
                if(Add_Name_Text.Text == null || Add_Name_Text.Text == "")
                {
                    Add_Name_Error.Text = ERROR_LABEL;
                    error_flg = true;
                }
                #endregion
                //生年月日
                #region
                if (Add_Brithday_Text.Text == null || Add_Brithday_Text.Text == "")
                {
                    //未入力の場合
                    Add_Brithday_Error.Text = ERROR_LABEL;
                    error_flg = true;
                }
                else if (DateTime.TryParse(Add_Brithday_Text.Text, out dt) == false)
                {
                    //入力された生年月日が日付ではなかった時
                    Add_Brithday_Error.Text = DATE_ERROR;
                    error_flg = true;
                }
                else if(DateTime.Parse(Add_Brithday_Text.Text) > DateTime.Now)
                {
                    //未来を入力
                    Add_Brithday_Error.Text = FUTURE_ERROR;
                    error_flg = true;
                }
                //閾値のチェック
                else if (DateTime.Parse(Add_Brithday_Text.Text) < DateTime.Parse(MIN_BRITHDAY_DATE))
                {
                    //過去の閾値を下回ったとき
                    Add_Brithday_Error.Text = PAST_BRITHDAY_ERROR;
                    error_flg = true;
                }

                #endregion
                //入社年月日が入力されているか？
                #region
                if (Add_Join_Text.Text == null || Add_Join_Text.Text == "")
                {
                    Add_Join_Error.Text = ERROR_LABEL;
                    error_flg = true;
                }
                else if (DateTime.TryParse(Add_Join_Text.Text, out dt) == false)
                {
                    //入力された生年月日が日付ではなかった時
                    Add_Join_Error.Text = DATE_ERROR;
                    error_flg = true;
                }

                else if (DateTime.Parse(Add_Join_Text.Text) < DateTime.Parse(MIN_JOIN_DATE))
                {
                    //過去の閾値を下回ったとき
                    Add_Join_Error.Text = PAST_JOIN_ERROR;
                    error_flg = true;
                }

                #endregion
                //結果判定
                #region
                if (error_flg == true)
                {
                    //入力内容に不備がある場合
                    Add_error_Label.Text = ERROR_TEXT;
                    return;
                }
                #endregion
                //確認ダイアログを出す
                #region
                JsScript = "if (confirm('この情報を追加しますか？')){ document.getElementById('" + Add.ClientID + "').click(); }";
                ClientScript.RegisterStartupScript(this.GetType(), "updateScript", JsScript, true);
                #endregion
                //入力された値を変数に入れる
            }
            catch
            {
                Add_error_Label.Text = ERROR_TEXT;
                return;
            }

        }
        //追加
        protected void Add_Click(object sender, EventArgs e)
        {
            try
            {
                //入力内容を変数に格納
                #region
                //ID初期化
                number = EMP_ID;
                
                brithday = DateTime.Parse(Add_Brithday_Text.Text);

                join = DateTime.Parse(Add_Join_Text.Text);

                
                if (Add_Skill_Text.Text == null || Add_Skill_Text.Text == "")
                {
                    //スキル未入力の場合
                    skill = "未設定";
                }
                else
                {
                    skill = Add_Skill_Text.Text;
                }

                
                if (Add_Skill_Text2.Text == null || Add_Skill_Text2.Text == "")
                {
                    //スキル2未入力の場合
                    skill_2nd = "未設定";
                }
                else
                {
                    skill_2nd = Add_Skill_Text2.Text;
                }

                
                if (Add_Skill_Text3.Text == null || Add_Skill_Text3.Text == "")
                {
                    //スキル3未入力の場合
                    skill_3rd = "未設定";
                }
                else
                {
                    skill_3rd = Add_Skill_Text3.Text;
                }
                #endregion
                //生年月日から年齢算出
                age = mc.getAGE(brithday);
                //入社年月日から勤続年月算出
                continuous = mc.getCON(join);
                //社員ID生成
                number = mc.getID(number);

                //社員追加
                insertSQL();

                Response.Redirect(Request.Url.OriginalString);
            }
            catch
            {
                return;
            }
            
        }
        #endregion

        //更新処理
        #region
        //更新ボタンクリック:入力チェック
        protected void Up_Button_Click(object sender, EventArgs e)
        {
            try
            {
                //表示初期化
                #region
                clearError();

                //追加項目
                Add_Name_Text.Text = "";
                Add_Brithday_Text.Text = "";
                Add_Join_Text.Text = "";
                Add_Skill_Text.Text = "";
                Add_Skill_Text2.Text = "";
                Add_Skill_Text3.Text = "";

                //削除項目
                Del_Number_Text.Text = "";

                #endregion
                //入力された値の整合性チェック
                #region
                //生年月日が入力されているか？
                if (DateTime.TryParse(Up_Brithday_Text.Text, out dt) == false)
                {
                    //入力された生年月日が日付ではなかった時
                    Up_Brithday_Error.Text = DATE_ERROR;
                    error_flg = true;
                }

                //入社年月日が入力されているか？
                if (DateTime.TryParse(Up_Join_Text.Text, out dt) == false)
                {
                    //入力された生年月日が日付ではなかった時
                    Up_Join_Error.Text = DATE_ERROR;
                    error_flg = true;
                }
                #endregion
                //閾値のチェック
                #region
                //生年月日
                if (DateTime.Parse(Up_Brithday_Text.Text) > DateTime.Now)
                {
                    //未来を入力していた場合
                    Up_Brithday_Error.Text = FUTURE_ERROR;
                    error_flg = true;
                }
                else if (DateTime.Parse(Up_Brithday_Text.Text) < DateTime.Parse(MIN_BRITHDAY_DATE))
                {
                    //過去の閾値を下回ったとき
                    Up_Brithday_Error.Text = PAST_BRITHDAY_ERROR;
                    error_flg = true;
                }

                //入社年月日
               if (DateTime.Parse(Up_Join_Text.Text) < DateTime.Parse(MIN_JOIN_DATE))
                {
                    //過去の閾値を下回ったとき
                    Up_Join_Error.Text = PAST_JOIN_ERROR;
                    error_flg = true;
                }
                #endregion
                //結果判定
                #region
                if (error_flg == true)
                {
                    Up_error_Label.Text = ERROR_TEXT;
                    return;
                }
                #endregion
                //確認ダイアログを出す
                #region
                JsScript = "if (confirm('更新しますか？')){ document.getElementById('" + UPDATE.ClientID + "').click(); }";
                ClientScript.RegisterStartupScript(this.GetType(), "updateScript", JsScript, true);
                #endregion
            }
            catch
            {
                return;
            }
        }
        //更新確認ダイアログOKクリック:DB更新＋更新データ出力
        protected void UPDATE_Click(object sender, EventArgs e)
        {
            try
            {
                //社員名取得
                #region
                if (Up_Name_Text.Text == null || Up_Name_Text.Text == "")
                {
                    //名前未入力の場合
                    name = Emp_Grid.SelectedRow.Cells[2].Text;
                }
                else
                {
                    name = Up_Name_Text.Text;
                }
                #endregion
                //生年月日取得
                #region
                if (Up_Brithday_Text.Text == null || Up_Brithday_Text.Text == "")
                {
                    //生年月日未入力の場合
                    brithday = DateTime.Parse(Emp_Grid.SelectedRow.Cells[4].Text);
                }
                else
                {
                    brithday = DateTime.Parse(Up_Brithday_Text.Text);
                }
                #endregion
                //入社年月日取得
                #region
                if (Up_Join_Text.Text == null || Up_Join_Text.Text == "")
                {
                    //入社年月が未入力の場合
                    join = DateTime.Parse(Emp_Grid.SelectedRow.Cells[5].Text);
                }
                else
                {
                    join = DateTime.Parse(Up_Join_Text.Text);
                }
                #endregion
                //得意スキル取得
                #region
                if (Up_Skill_Text.Text == null || Up_Skill_Text.Text == "")
                {
                    //スキル未入力の場合
                    skill = DEFAULT_SKILL;
                }
                else
                {
                    skill = Up_Skill_Text.Text;
                }


                if (Up_Skill_Text2.Text == null || Up_Skill_Text2.Text == "")
                {
                    //スキル2未入力の場合
                    skill_2nd = DEFAULT_SKILL;
                }
                else
                {
                    skill_2nd = Up_Skill_Text2.Text;
                }


                if (Up_Skill_Text3.Text == null || Up_Skill_Text3.Text == "")
                {
                    //スキル3未入力の場合
                    skill_3rd = DEFAULT_SKILL;
                }
                else
                {
                    skill_3rd = Up_Skill_Text3.Text;
                }
                #endregion
                //生年月日から年齢算出
                age = mc.getAGE(brithday);
                //入社年月日から勤続年月算出
                continuous = mc.getCON(join);
                //更新処理
                updateSQL();
                //更新データ表示
                selectSQL();
            }
            catch
            {
                return;
            }             
        }
        #endregion

        //削除処理
        #region
        //削除ボタンクリック
        protected void Del_Button_Click(object sender, EventArgs e)
        {
            try
            {
                //表示初期化
                #region
                //エラー表示
                clearError();

                //追加項目
                Add_Name_Text.Text = "";
                Add_Brithday_Text.Text = "";
                Add_Join_Text.Text = "";
                Add_Skill_Text.Text = "";
                Add_Skill_Text2.Text = "";
                Add_Skill_Text3.Text = "";

                //更新項目
                Up_Name_Text.Text = "";
                Up_Brithday_Text.Text = "";
                Up_Join_Text.Text = "";
                Up_Skill_Text.Text = "";
                Up_Skill_Text2.Text = "";
                Up_Skill_Text3.Text = "";

                #endregion
                //入力された値の整合性チェック
                #region
                //退職年月日が正常に入力されているか？
                if (Del_Retirement_Text.Text == null || Del_Retirement_Text.Text == "")
                {
                    //退職年月日が入力されていなかった時
                    Del_Retirement_Error.Text = ERROR_LABEL;
                    error_flg = true;
                }
                else if (DateTime.TryParse(Del_Retirement_Text.Text, out dt) == false)
                {
                    //入力された生年月日が日付ではなかった時
                    Del_Retirement_Error.Text = DATE_ERROR;
                    error_flg = true;
                }
                //閾値のチェック
                else if (DateTime.Parse(Del_Retirement_Text.Text) < DateTime.Parse(MIN_BRITHDAY_DATE))
                {
                    //過去の閾値を下回ったとき
                    Del_Retirement_Error.Text = PAST_BRITHDAY_ERROR;
                    error_flg = true;
                }

                #endregion
                //結果判定
                #region
                if (error_flg == true)
                {
                    Del_error_Label.Text = ERROR_TEXT;
                    return;
                }
                #endregion
                //確認ダイアログを出す
                #region
                JsScript = "if (confirm('この情報を削除しますか？')){ document.getElementById('" + DELETE_button.ClientID + "').click(); }";
                ClientScript.RegisterStartupScript(this.GetType(), "updateScript", JsScript, true);
                #endregion               
            }
            catch
            {               
                return;
            }
        }
        //削除
        protected void DELETE_button_Click(object sender, EventArgs e)
        {
            select_hash = (Hashtable)ViewState["select_hash"];

            number = (String)select_hash["NUMBER"];
            termination_date = Del_Retirement_Text.Text;

            //社員削除用SQL
            SqlConnection con = new SqlConnection(@"Data Source=(local);Initial Catalog=EmpDB;Integrated Security=SSPI");
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Empmst SET DEL_FLG = 1, TERMINATION_DATE = @TERMINATION_DATE WHERE EMP_NUMBER = @EMP_NUMBER;", con);
            cmd.Parameters.Add(new SqlParameter("@TERMINATION_DATE", termination_date));
            cmd.Parameters.Add(new SqlParameter("@EMP_NUMBER", number));
            SqlDataReader dr = cmd.ExecuteReader();
            Emp_Grid.DataSource = dr;
            Emp_Grid.DataBind();
            con.Close();

            Response.Redirect(Request.Url.OriginalString);
        }
        #endregion

        //選択ボタンクリック
        protected void Emp_Grid_SelectedIndexChanged(object sender, EventArgs e)
        {

            //入力項目初期表示
            #region
            clearError();

            //更新情報
            Up_Name_Text.Text = Emp_Grid.SelectedRow.Cells[2].Text;
            Up_Brithday_Text.Text = Emp_Grid.SelectedRow.Cells[4].Text;
            Up_Join_Text.Text = Emp_Grid.SelectedRow.Cells[5].Text;
            Up_Skill_Text.Text = Emp_Grid.SelectedRow.Cells[8].Text;
            Up_Skill_Text2.Text = Emp_Grid.SelectedRow.Cells[9].Text;
            Up_Skill_Text3.Text = Emp_Grid.SelectedRow.Cells[10].Text;

            //削除情報
            Del_Number_Text.Text = Emp_Grid.SelectedRow.Cells[1].Text;
            #endregion

            //選択された社員情報を格納
            #region
            select_hash.Add("NUMBER", Emp_Grid.SelectedRow.Cells[1].Text);
            select_hash.Add("NAME", Emp_Grid.SelectedRow.Cells[2].Text);
            select_hash.Add("AGE", Emp_Grid.SelectedRow.Cells[3].Text);
            select_hash.Add("BIRTHDAY", Emp_Grid.SelectedRow.Cells[4].Text);
            select_hash.Add("JOIN", Emp_Grid.SelectedRow.Cells[5].Text);
            select_hash.Add("CONTINUE", Emp_Grid.SelectedRow.Cells[6].Text);
            select_hash.Add("SKILL", Emp_Grid.SelectedRow.Cells[8].Text);
            select_hash.Add("SKILL_2nd", Emp_Grid.SelectedRow.Cells[9].Text);
            select_hash.Add("SKILL_3rd", Emp_Grid.SelectedRow.Cells[10].Text);
            #endregion

            //入力項目解除
            #region
            //更新機能
            Up_Name_Text.Enabled = true;
            Up_Brithday_Text.Enabled = true;
            Up_Join_Text.Enabled = true;
            Up_Skill_Text.Enabled = true;
            Up_Skill_Text2.Enabled = true;
            Up_Skill_Text3.Enabled = true;
            Up_Button.Enabled = true;

            //削除機能
            Del_Retirement_Text.Enabled = true;
            Del_Button.Enabled = true;
            #endregion

            ViewState["select_hash"] = select_hash;

        }

        //退職者表示ボタンクリック
        protected void Del_Show_Button_Click(object sender, EventArgs e)
        {
            del_flg = (Boolean)ViewState["del_flg"];

            Emp_Grid.SelectedIndex = -1;

            //更新情報初期化
            #region
            Up_Name_Text.Text = "";
            Up_Brithday_Text.Text = "";
            Up_Join_Text.Text = "";
            Up_Skill_Text.Text = "";
            Up_Skill_Text2.Text = "";
            Up_Skill_Text3.Text = "";
            #endregion

            //表示内容判定
            if (del_flg == true)
            {
                
                Del_Show_Button.Text = TRUE_TEXT;
                del_flg = false;
                ViewState["del_flg"] = del_flg;

                //入力フォームおよび情報変更ボタン使用可処理
                cahngeInput(del_flg);

                //正社員表示用SQL
                selectSQL();

                return;
            }

            del_flg = true;
            ViewState["del_flg"] = del_flg;
            Del_Show_Button.Text = FALSE_TEXT;

            //退職者表示用SQL
            retiree_selectSQL();

            //入力フォームおよび情報変更ボタン使用不可処理
            cahngeInput(del_flg);

        }

        //正社員検索SQL
        public void selectSQL()
        {
            //正社員表示用SQL
            SqlConnection con = new SqlConnection(@"Data Source=(local);Initial Catalog=EmpDB;Integrated Security=SSPI");
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT EMP_NUMBER,NAME,AGE,BIRTHDAY,JOIN_YEAR,CON_YEAR,TERMINATION_DATE,SKILL,SKILL_2nd,SKILL_3rd FROM Empmst WHERE DEL_FLG = 0;", con);
            SqlDataReader dr = cmd.ExecuteReader();
            Emp_Grid.DataSource = dr;
            Emp_Grid.DataBind();
            con.Close();

            //技術者の人数表示
            Emp_Count_Label.Text = EMP_ENGINEER_COUNT;
            count = Emp_Grid.Rows.Count;
            Emp_Count.Text = count.ToString() + "人";
        }

        //退職者追加SQL
        public void retiree_selectSQL()
        {
            //退職者表示用SQL
            SqlConnection con = new SqlConnection(@"Data Source=(local);Initial Catalog=EmpDB;Integrated Security=SSPI");
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT EMP_NUMBER,NAME,AGE,BIRTHDAY,JOIN_YEAR,CON_YEAR,TERMINATION_DATE,SKILL,SKILL_2nd,SKILL_3rd FROM Empmst WHERE DEL_FLG = 1;", con);
            SqlDataReader dr = cmd.ExecuteReader();
            Emp_Grid.DataSource = dr;
            Emp_Grid.DataBind();
            con.Close();

            //退職者の人数表示
            Emp_Count_Label.Text = EMP_RETIREE_COUNT;
            count = Emp_Grid.Rows.Count;
            Emp_Count.Text = count.ToString() + "人";

        }

        //update文
        public void updateSQL()
        {
            select_hash = (Hashtable)ViewState["select_hash"];

            number = (String)select_hash["NUMBER"];

            //社員情報更新SQL
            SqlConnection con = new SqlConnection(@"Data Source=(local);Initial Catalog=EmpDB;Integrated Security=SSPI");
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Empmst SET NAME = @NAME, AGE = @AGE, BIRTHDAY = @BIRTHDAY, JOIN_YEAR = @JOIN_YEAR, CON_YEAR = @CON_YEAR, SKILL = @SKILL, SKILL_2nd = @SKILL_2nd, SKILL_3rd = @SKILL_3rd  WHERE EMP_NUMBER = @EMP_NUMBER;", con);
            cmd.Parameters.Add(new SqlParameter("@EMP_NUMBER", number));
            cmd.Parameters.Add(new SqlParameter("@NAME", name));
            cmd.Parameters.Add(new SqlParameter("@AGE", age));
            cmd.Parameters.Add(new SqlParameter("@BIRTHDAY", brithday));
            cmd.Parameters.Add(new SqlParameter("@JOIN_YEAR", join));
            cmd.Parameters.Add(new SqlParameter("@CON_YEAR", continuous));
            cmd.Parameters.Add(new SqlParameter("@SKILL", skill));
            cmd.Parameters.Add(new SqlParameter("@SKILL_2nd", skill_2nd));
            cmd.Parameters.Add(new SqlParameter("@SKILL_3rd", skill_3rd));
            SqlDataReader dr = cmd.ExecuteReader();
            Emp_Grid.DataSource = dr;
            con.Close();
        }

        //insert文
        public void insertSQL()
        {
            //社員情報追加SQL
            SqlConnection con = new SqlConnection(@"Data Source=(local);Initial Catalog=EmpDB;Integrated Security=SSPI");
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Empmst(EMP_NUMBER,NAME,AGE,BIRTHDAY,JOIN_YEAR,CON_YEAR,SKILL,SKILL_2nd,SKILL_3rd,DEL_FLG) VALUES(@EMP_NUMBER,@NAME,@AGE,@BIRTHDAY,@JOIN_YEAR,@CON_YEAR,@SKILL,@SKILL_2nd,@SKILL_3rd,@DEL_FLG);", con);
            cmd.Parameters.Add(new SqlParameter("@EMP_NUMBER", number));
            cmd.Parameters.Add(new SqlParameter("@NAME", name));
            cmd.Parameters.Add(new SqlParameter("@AGE", age));
            cmd.Parameters.Add(new SqlParameter("@BIRTHDAY", brithday));
            cmd.Parameters.Add(new SqlParameter("@JOIN_YEAR", join));
            cmd.Parameters.Add(new SqlParameter("@CON_YEAR", continuous));
            cmd.Parameters.Add(new SqlParameter("@SKILL", skill));
            cmd.Parameters.Add(new SqlParameter("@SKILL_2nd", skill_2nd));
            cmd.Parameters.Add(new SqlParameter("@SKILL_3rd", skill_3rd));
            cmd.Parameters.Add(new SqlParameter("@DEL_FLG", DEL_FLG));
            SqlDataReader dr = cmd.ExecuteReader();
            Emp_Grid.DataSource = dr;
            Emp_Grid.DataBind();
            con.Close();
        }

        /// <summary>
        /// 入力フォーム切り替え
        /// </summary>
        /// <param name="input_flg">正社員表示か退職者表示か判別</param>
        public void cahngeInput(Boolean input_flg)
        {
            if (input_flg == false)
            {
                //正社員表示の時
                #region
                //追加機能
                Add_Name_Text.Enabled = true;
                Add_Brithday_Text.Enabled = true;
                Add_Join_Text.Enabled = true;
                Add_Skill_Text.Enabled = true;
                Add_Skill_Text2.Enabled = true;
                Add_Skill_Text3.Enabled = true;
                Add_Button.Enabled = true;

                //更新機能
                Up_Name_Text.Enabled = false;
                Up_Brithday_Text.Enabled = false;
                Up_Join_Text.Enabled = false;
                Up_Skill_Text.Enabled = false;
                Up_Skill_Text2.Enabled = false;
                Up_Skill_Text3.Enabled = false;
                Up_Button.Enabled = false;

                //削除機能
                Del_Button.Enabled = true;
                #endregion

            }
            else
            {
                //退職者表示の時
                #region
                //追加機能
                Add_Name_Text.Enabled = false;
                Add_Brithday_Text.Enabled = false;
                Add_Join_Text.Enabled = false;
                Add_Skill_Text.Enabled = false;
                Add_Skill_Text2.Enabled = false;
                Add_Skill_Text3.Enabled = false;
                Add_Button.Enabled = false;

                //更新機能
                Up_Name_Text.Enabled = false;
                Up_Brithday_Text.Enabled = false;
                Up_Join_Text.Enabled = false;
                Up_Skill_Text.Enabled = false;
                Up_Skill_Text2.Enabled = false;
                Up_Skill_Text3.Enabled = false;
                Up_Button.Enabled = false;

                //削除機能
                Del_Number_Text.Enabled = false;
                Del_Button.Enabled = false;
                #endregion
            }
        }

        /// <summary>
        /// 選択行表示切替
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Emp_Grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ViewState["del_flg"] = del_flg;

          　//選択行および勤続年数、退職年月日の表示切替
            if (del_flg == true)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = true;
            }
            else
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[6].Visible = true;
                e.Row.Cells[7].Visible = false;
            }
        }

        /// <summary>
        /// 表示初期化
        /// </summary>
        public void clearError()
        {
            //各項目のエラー文を一度初期化
            Add_error_Label.Text = "";
            Add_Name_Error.Text = "";
            Add_Brithday_Error.Text = "";
            Add_Join_Error.Text = "";

            Up_error_Label.Text = "";
            Up_Brithday_Error.Text = "";
            Up_Join_Error.Text = "";

            Del_error_Label.Text = "";
            Del_Retirement_Error.Text = "";
        }
    }
}
