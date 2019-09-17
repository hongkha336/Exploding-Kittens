using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoChat
{
    public partial class Form1 : Form
    {
        int port = 5000;
        NetworkStream ns;
        TcpClient client;
        IPAddress ip;



        public Form1()
        {
            InitializeComponent();

        }

        SeverSocket SEVER;

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        public object DeserializeData(byte[] theByteArray)
        {
            MemoryStream ms = new MemoryStream(theByteArray);
            BinaryFormatter bf1 = new BinaryFormatter();
            ms.Position = 0;
            return bf1.Deserialize(ms);
        }
        public byte[] SerializeData(Object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();
            bf1.Serialize(ms, o);
            return ms.ToArray();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            myObject obj = new myObject();
            obj.Id = textBox2.Text;
            obj.Str = textBox1.Text;

            byte[] buffer = ObjectToByteArray(obj);
            ns.Write(buffer, 0, buffer.Length);





        }


        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }


        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            joinSever();
        }






        private void button3_Click(object sender, EventArgs e)
        {

        }

        #region clients
        public void joinSever()
        {
            ip = IPAddress.Parse("192.168.2.105");

            client = new TcpClient();
            client.Connect(ip, port);
            //Console.WriteLine("client connected!!");

            ns = client.GetStream();
            Thread thread = new Thread(o => ReceiveData((TcpClient)o));

            thread.Start(client);



            //client.Client.Shutdown(SocketShutdown.Send);
            //thread.Join();
            //ns.Close();
            //client.Close();

        }

        public void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[6000];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
               myObject obj = (myObject)ByteArrayToObject(receivedBytes);
                richTextBox1.Text += '\n' + (obj.Id +" : " + obj.Str);
            }
        }


#endregion

    } }