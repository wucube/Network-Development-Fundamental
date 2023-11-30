using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServerSyncExercise2
{
    class ClientSocket
    {
        private static int CLIENT_BEGIN_ID = 1;
        public int clientID;
        public Socket socket;

        public ClientSocket(Socket socket)
        {
            this.clientID = CLIENT_BEGIN_ID;
            this.socket = socket;
            ++CLIENT_BEGIN_ID;
        }

        /// <summary>
        /// 是否是连接状态
        /// </summary>
        public bool Connected => this.socket.Connected;

        //我们应该封装一些方法
        //关闭
        public void Close()
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket = null;
            }
        }
        //发送
        public void Send(BaseMsg info)
        {
            if (socket != null)
            {
                try
                {
                    socket.Send(info.Writing());
                }
                catch (Exception e)
                {
                    Console.WriteLine("发消息出错" + e.Message);
                    Close();
                }
            }

        }
        //接收
        public void Receive()
        {
            if (socket == null)
                return;
            try
            {
                if (socket.Available > 0)
                {
                    byte[] result = new byte[1024 * 5];
                    int receiveNum = socket.Receive(result);
                    //收到数据后 先读取4个字节 转为ID 才知道用哪一个类型去处理反序列化
                    int msgID = BitConverter.ToInt32(result, 0);
                    BaseMsg msg = null;
                    switch (msgID)
                    {
                        case 1001:
                            msg = new PlayerMsg();
                            msg.Reading(result, 4);
                            break;
                    }
                    if (msg == null)
                        return;
                    ThreadPool.QueueUserWorkItem(MsgHandle, msg);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("收消息出错" + e.Message);
                Close();
            }
        }

        private void MsgHandle(object obj)
        {
            BaseMsg msg = obj as BaseMsg;
            if (msg is PlayerMsg)
            {
                PlayerMsg playerMsg = msg as PlayerMsg;
                Console.WriteLine(playerMsg.playerID);
                Console.WriteLine(playerMsg.playerData.name);
                Console.WriteLine(playerMsg.playerData.lev);
                Console.WriteLine(playerMsg.playerData.atk);
            }
        }

    }
}
