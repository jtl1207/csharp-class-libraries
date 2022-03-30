using Google.Authenticator;
using QRCoder;
using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace 手机令牌
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            xtraTabControl1.SelectedTabPageIndex = 1;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Pic_load();
            Threading_Load();
            DbStart DbStart = new DbStart();
        }
        private void Threading_Load()
        {
            System.Timers.Timer t_systime = new System.Timers.Timer(1000);
            t_systime.Elapsed += new System.Timers.ElapsedEventHandler(time);
            t_systime.AutoReset = true;
            t_systime.Enabled = true;
            Thread t_queue = new Thread(new ThreadStart(Update_db));
            t_queue.IsBackground = true;
            t_queue.Start();
        }
        private void time(object source, System.Timers.ElapsedEventArgs e)
        {
            systime.Text = DateTime.Now.ToString();
        }
        private void Update_db()
        {
            bool Account = true;
            while (true)
            {
                if (Dbupdate.Account && Account)
                {
                    Thread.Sleep(500);
                    this.accountTableAdapter.Fill(this.dataDataSet.Account);
                    Dbupdate.Account_done += 1;
                    Account = false;
                }
                else
                {
                    Account = true;
                }
                Thread.Sleep(1);
            }
        }
        public void Pic_load()
        {
            pictureBox1.Image = Code("http://s.downpp.com/apk9/googlesfyzq_5.10_2265.com.apk");
            pictureBox1.Show();
            pictureBox1.Refresh();
        }
        public Image Code(string strCodeUrl)
        {
            /// <summary>
            /// 显示二维码
            /// </summary>
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qrGenerator.CreateQrCode(strCodeUrl, QRCodeGenerator.ECCLevel.Q);
            QRCode qrcode = new QRCode(qRCodeData);
            Bitmap codeImg = qrcode.GetGraphic(3, Color.Black, Color.White, null, 0, 0, true);
            /* GetGraphic方法参数说明
                 public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon = null, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true)
             *   int pixelsPerModule:生成二维码图片的像素大小 ，我这里设置的是5 
             *  Color darkColor：暗色   一般设置为Color.Black 黑色
             *  Color lightColor:亮色   一般设置为Color.White  白色
             *  Bitmap icon :二维码 水印图标 例如：Bitmap icon = new Bitmap(context.Server.MapPath("~/images/zs.png")); 默认为NULL ，加上这个二维码中间会显示一个图标
             *  int iconSizePercent： 水印图标的大小比例 ，可根据自己的喜好设置 
             *  int iconBorderWidth： 水印图标的边框
             *  bool drawQuietZones:静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true
            */
            Image img = Image.FromHbitmap(codeImg.GetHbitmap());
            return img;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DbRange DbRange = new DbRange();
            DbRange.Delete("Account");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == 13))
            {
                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsPunctuation(e.KeyChar) || Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == 13))
            {
                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                button1.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Registration)
            {
                string user = textBox1.Text;
                string password = textBox2.Text;
                string key = "登录" + User + DateTime.Now.ToString();
                DbRange DbRange = new DbRange();
                DbReturn DbReturn = new DbReturn();

                DbRange.Search_DataTable("Account", "user", user, key);
                DataTable datatable = DbReturn.Return_datatable(key);

                if (datatable.Rows.Count==1)
                {
                    string password_2 = datatable.Rows[0][2].ToString();
                    string Key_2 = datatable.Rows[0][3].ToString();

                    if (password_2 == password)
                    {
                        if (Key_2 != "")
                        {
                            Form2 form2 = new Form2(Key_2);
                            if (form2.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }
                        }
                        MessageBox.Show("登录成功", "提示");
                        return;
                    }
                }
                MessageBox.Show("用户名或密码错误", "提示");
            }
            else
            {
                MessageBox.Show("手机令牌未验证", "提示");
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == 13))
            {
                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                this.textBox3.Focus();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsPunctuation(e.KeyChar) || Char.IsLetter(e.KeyChar) || Char.IsNumber(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == 13))
            {
                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                checkBox1.Focus();
            }
        }

        private void checkBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button2.PerformClick();
            }
        }

        static bool Registration = false;
        static string User, Password, Key;

        private void button7_Click(object sender, EventArgs e)
        {
            Registration = false;
            xtraTabControl1.SelectedTabPageIndex = 1;
            MessageBox.Show("注册取消", "提示");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Registration)
            {
                MessageBox.Show("手机令牌未验证", "提示");
                return;
            }
            User = textBox4.Text;
            Password = textBox3.Text;
            string key = "注册" + User + DateTime.Now.ToString();
            DbRange DbRange = new DbRange();
            DbReturn DbReturn = new DbReturn();

            DateTime beforDT = System.DateTime.Now;
            
            DbRange.Search_DataTable("Account", "user", User, key);
            bool stop = DbReturn.Return_datatable_bool(key);

            if (stop)
            {
                DateTime afterDT = System.DateTime.Now;
                TimeSpan ts = afterDT.Subtract(beforDT);
                MessageBox.Show("用户已存在\n共"+ts.TotalMilliseconds+"ms", "提示");
            }
            else
            {
                key = "注册" + User + DateTime.Now.ToString();
                string lists, values;
                if (checkBox1.Checked)
                {
                    Registration = true;

                    Key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

                    TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
                    var setupCode = tfA.GenerateSetupCode("手机令牌", User, Key, false, 3);

                    string qrCodeImageUrl = setupCode.QrCodeSetupImageUrl;
                    string manualEntrySetupCode = setupCode.ManualEntryKey;

                    label14.Text = User;
                    label13.Text = manualEntrySetupCode;

                    var base64 = qrCodeImageUrl.Replace("data:image/png;base64,", "");
                    byte[] bytes = Convert.FromBase64String(base64);
                    MemoryStream memStream = new MemoryStream(bytes);
                    pictureBox2.Image = Image.FromStream(memStream);
                    pictureBox2.Show();
                    pictureBox2.Refresh();

                    xtraTabControl1.SelectedTabPageIndex = 0;
                }
                else
                {
                    lists = "[user], [password]";
                    values = $"'{User}', '{Password}'";
                    DbRange.Add_Int("Account", lists, values, key);
                    int Return = DbReturn.Return_int(key);

                    DateTime afterDT = System.DateTime.Now;
                    TimeSpan ts = afterDT.Subtract(beforDT);
                    if (Return == 1)
                    {
                        MessageBox.Show("用户注册成功\n共" + ts.TotalMilliseconds + "ms", "提示");
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            bool result = tfa.ValidateTwoFactorPIN(Key, textBox6.Text);

            if (result)
            {
                string lists = "[user], [password], authenticator";
                string values = $"'{User}', '{Password}', '{Key}'";

                DateTime beforDT = System.DateTime.Now;
                string key = "注册" + User + DateTime.Now.ToString();
                DbRange DbRange = new DbRange();
                DbReturn DbReturn = new DbReturn();
                DbRange.Add_Int("Account", lists, values, key);
                int Return = DbReturn.Return_int(key);

                DateTime afterDT = System.DateTime.Now;
                TimeSpan ts = afterDT.Subtract(beforDT);
                if (Return == 1)
                {
                    MessageBox.Show("用户注册成功\n共" + ts.TotalMilliseconds + "ms", "提示");
                    xtraTabControl1.SelectedTabPageIndex = 1;
                    Registration = false;
                }
            }
            else
            {
                MessageBox.Show("手机令牌错误", "提示");
            }
        }
    }
}
