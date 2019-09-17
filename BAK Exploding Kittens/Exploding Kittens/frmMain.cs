using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exploding_Kittens
{
    public partial class frmMain : Form
    {
#region khaibao

        int yourID = -1;
        int port = 8888;
        NetworkStream ns;
        TcpClient client;
        IPAddress ip;
        Player PLAYER;

        static bool isinSever = false;

        static bool isStart = false;
        bool isCreateSever = false;

        public List<PictureBox> listMyCards;

#endregion
        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(bool isStartSever)
        {
            InitializeComponent();
            isCreateSever = isStartSever;
            
        }

        static GameBucket DATA;
         Card_Infor_ card_Infor;
        private void thoatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            card_Infor = new Card_Infor_();
            btnStart.Hide();
            btnDrawACard.Hide();
            if(isCreateSever)
            {
                DATA = new GameBucket();
                createSever();
                btnStart.Show();
               
                // cập nhật trạng thái là đang ở sever

            }
            
        }

        static bool sendBool = false;
        private void btnStart_Click(object sender, EventArgs e)
        {
            Console.WriteLine("START BUTTON WAS PRESSED");
           // btnStart.Hide();

            // gửi đi DATA đã thu thập


            // gửi đi thông tin rằng Sever đã bắt đầu chơi





            isinSever = true;
            isStart = true;



            //byte[] buffer = ObjectToByteArray("SET");
            //ns.Write(buffer, 0, buffer.Length);

            // Thread.Sleep(500);

            Console.WriteLine("Sent START MESSAGES to sever");

            byte[] buf = ObjectToByteArray(DATA);
            ns.Write(buf, 0, buf.Length);

        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            joinSever();

        }


       private void phanTichData()
        {

            // Tình trạng vừa mới nhận data;

            // Lấy ra bài của mình.

            Console.WriteLine("Phan tich data Your ID " + yourID);
            PLAYER = DATA.getPlayerWithID(yourID);
            Console.WriteLine("Da lay xong PLAYER");
            // Sau đó vẽ lại.
            label1.Text = DATA.PACK.getSize().ToString() ;
            label5.Text = "PLAYER " + (DATA.idPlayerTurn+1).ToString() + " (" + DATA.getPlayerWithID(DATA.idPlayerTurn + 1).getTurn() + ")";
            if (yourID - 1 == DATA.idPlayerTurn)
                btnDrawACard.Show();

            // Phân tích thêm cl gì đó. Nhưng trước tiên là vẽ trước đã.
        }
        #region clients
        private void joinSever()
        {
            ip = IPAddress.Parse("192.168.43.92");

            client = new TcpClient();
            client.Connect(ip, port);

            // gửi đi thông điệp "JOIN"
            ns = client.GetStream();
            Thread thread = new Thread(o => ReceiveData((TcpClient)o));
            thread.Start(client);

            // 
            Console.WriteLine("Sent JOIN MESSAGES to sever");
            byte[] buffer = ObjectToByteArray("JOIN");
            ns.Write(buffer, 0, buffer.Length);

           
        
        }


        public void changeNamelb()
        {
            Label.CheckForIllegalCrossThreadCalls = false;
            label6.Text = "PLAYER " + yourID.ToString();
        }
        
        public void ReceiveData(TcpClient client)
        {
            Console.WriteLine("Recieve DATA:");
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[6000];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                if (yourID == -1)
                {
                  
                  

                    try
                    {
                        yourID = Convert.ToInt32(ByteArrayToObject(receivedBytes));
                        changeNamelb();
                        Console.WriteLine("Recive ID Player which playerID equals " + yourID);
                        
                    }
                    catch {

                        Console.WriteLine("Nhan id that bai");
                        
                      
                    }


                }
                else
                {

                    try
                    {
                       
                        DATA = (GameBucket)ByteArrayToObject(receivedBytes);
                        Console.WriteLine("Recive OTHER infor");

                        btnStart.Visible = false;
                        btnDrawACard.Hide();
                        phanTichData();
                    }
                    catch
                    {

                    }
  

                }

            }
        }

        #endregion

        #region mahoa
        private static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }
        private static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }

        #endregion

        #region drawing
        private void drawmyCard()
        {
            // tạo mới
            Console.WriteLine("Draw new list card");
            flowMyCard.Controls.Clear();
            listMyCards = new List<PictureBox>();



            foreach (int id in PLAYER.getListCards())
            {
                DrawCardwithID(id);
            }
        }
        private void DrawCardwithID(int id)
        {
            PictureBox pic = new PictureBox();

            pic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pic.Location = new System.Drawing.Point(3, 3);
            pic.Name = id.ToString();
            pic.Size = new System.Drawing.Size(109, 159);
            pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pic.TabIndex = 1;
            pic.TabStop = false;

            pic.Image = Image.FromFile("SourseCard/"+id+".jpg");

            
            pic.MouseUp += Pic_MouseUp;
            listMyCards.Add(pic);
            flowMyCard.Controls.Add(pic);
        }

        private void Pic_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                right_click(sender);
            }
            else//left or middle click
            {
                left_click(sender);
            }
        }

        private void right_click(object sender)
        {
            PictureBox pic = (PictureBox)sender;
            int stt = -1;
            try
            {
                stt = Convert.ToInt32(pic.Name);
            }
            catch {
                try
                {
                    stt = Convert.ToInt32(pic.Name.Substring(4, pic.Name.Length - 4));
                }
                catch { }
            }

            // tới đây chắc chắn lấy được stt;

            lbDec.Text = card_Infor.getCardInforbyIdCard(stt);

        }

        private void left_click(object sender)
        {

            

            PictureBox pic = (PictureBox)sender;
            int stt = -1;
            try
            {
                stt = Convert.ToInt32(pic.Name);
            }
            catch { }

            if (stt != -1)
            {
                // Hình gốc

                lbDec.Text = card_Infor.getCardInforbyIdCard(stt);
                pic.Name = "fade" + pic.Name;
                pic.Image = Image.FromFile("choose.png");
                
            }
            else
            {
                lbDec.ResetText();
                pic.Name = pic.Name.Substring(4, pic.Name.Length - 4);
                pic.Image = Image.FromFile("SourseCard/" + pic.Name + ".jpg");
            }
        }
        private void btnDrawACard_Click(object sender, EventArgs e)
        {
            // rút một lá.
            int cardDraw = DATA.PACK.DRAW_A_CARD();
            // thêm card vào bộ bài
            PLAYER.addCard(cardDraw);
            // trả về data
            DATA.setPlayerWithID(PLAYER);


            // vẽ card ra màn hình
            drawmyCard();

            DATA.nextTurn();
            sendSomethingTosever(DATA);


        }

#endregion


        #region Sever

        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();
        static int count;
        static int temp;
        public void createSever()
        {
            Thread mySeverThread = new Thread(() => {
                count = 1;

                TcpListener ServerSocket = new TcpListener(IPAddress.Any, 8888);
                ServerSocket.Start();

                while (true)
                {
                    TcpClient client = ServerSocket.AcceptTcpClient();
                    lock (_lock) list_clients.Add(count, client);
                    temp = count;

                    Player p = new Player(count,"PLAYER");
                    DATA.setPlayer(p);

                    Console.WriteLine("Co nguoi than gia: "+temp);

                    Thread t = new Thread(handle_clients);
                    t.Start(count);
                    count++;
                }
            });

            mySeverThread.Start();
        }

        public static void handle_clients(object o)
        {
            int id = (int)o;
            TcpClient client;

            lock (_lock) client = list_clients[id];

            while (true)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[6000];
                int byte_count = stream.Read(buffer, 0, buffer.Length);

                if (byte_count == 0)
                {
                    break;
                }

                string s = "";
                try
                {
                
                    s = (string)ByteArrayToObject(buffer);
                }
                catch
                {
                }




                if (s.Equals(""))
                {
                    // Gửi data lên chắc chắn được boardcast
             
                    broadcast(buffer);
                    Console.WriteLine("Recieve REQUIRE while s equals SPACE mean: Sent EXTERNAL Files");
                }
                else
                {

                    if (s.Equals("START"))
                    {


                        broadcastDATA();
                        Console.WriteLine("Recieve START MESSAGES");
                        Console.WriteLine("\t Request Signal");

                    }
                    else
                    {
                        if (s.Equals("SET"))
                        {
                           //Chổ này sẽ không bao giờ được sử dụng
                            broadcast(ObjectToByteArray("true"));
                            Console.WriteLine("\t send START message TO the OTHER");
                            //Kết thúc chổ vô dụng
                        }
                        else
                        {
                            
                            Console.WriteLine("\t Sent temp which temp equals "+ temp);
                            broadcast(ObjectToByteArray(temp));
                        }
                    }

                }

            }
        }

            public static void broadcast(byte[] buffer)
        {


            lock (_lock)
            {
                foreach (TcpClient c in list_clients.Values)
                {
                    NetworkStream stream = c.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }
            }





        }


        public static void broadcastDATA()
        {
            try
            {
                byte[] buffer = ObjectToByteArray(DATA);

                lock (_lock)
                {
                    foreach (TcpClient c in list_clients.Values)
                    {
                        NetworkStream stream = c.GetStream();

                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
                Console.WriteLine("\t Sent DATA");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }




        }



        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
          
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }


        private void sendSomethingTosever(Object b)
        {
            
            byte[] buffer = ObjectToByteArray(b);
            ns.Write(buffer, 0, buffer.Length);
        }
    }
}
