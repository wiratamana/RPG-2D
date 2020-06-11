using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

/*
 * // TODO : ■Unityで作成するスマホアプリとLampプログラム間でユーザー情報を保存・取り出しを行うプログラムを作成」
　　・ユーザー情報は「機器ID、ユーザ名、性別、生年月日」
　　・Unityで作成するスマホアプリは、iOS・AndroidどちらでもOK
　　・スマホアプリで入力されたユーザー情報を、LAMPプログラムに送信し、LAMPサーバーのMySQLに保存する
　　・スマホアプリで、LAMPサーバーのMySQLに保存されたユーザー方法を取得し表示する
　　・LAMPサーバーでWEBでヒ表示できる運営画面を作成し、保存されているユーザー情報が一覧表示できる
　　　WEBプログラムを作成する
 */

public static class Client2
{ 
    public static string ID { get; private set; }

    private const int BUFFER_SIZE = 4096;
    private static TcpClient socket;
    private static NetworkStream stream;
    private static byte[] receivedBuffer;

    private static Dictionary<ServerPackets, Action<Packet>> responses;

    public static void ConnectToServer()
    {
        const string ip = "192.168.0.14";
        const int port = 26950;

        socket = new TcpClient()
        {
            ReceiveBufferSize = BUFFER_SIZE,
            SendBufferSize = BUFFER_SIZE
        };

        responses = new Dictionary<ServerPackets, Action<Packet>>();
        responses.Add(ServerPackets.PingResponse, ResponsePing);
        responses.Add(ServerPackets.SendFreeID, ResponseID);

        receivedBuffer = new byte[BUFFER_SIZE];
        socket.BeginConnect(ip, port, new AsyncCallback(OnConnectedToTheServer), null);
    }

    private static void OnConnectedToTheServer(IAsyncResult result)
    {
        socket.EndConnect(result);

        if (socket.Connected == false)
        {
            UnityEngine.Debug.Log("Failed to connect");
            return;
        }

        UnityEngine.Debug.Log("Success to connect");
        stream = socket.GetStream();

        stream.BeginRead(receivedBuffer, 0, BUFFER_SIZE, OnReceivedDataFromServer, null);
    }

    private static void OnReceivedDataFromServer(IAsyncResult result)
    {
        try
        {
            int byteLength = stream.EndRead(result);
            if (byteLength <= 0)
            {
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(receivedBuffer, data, byteLength);

            HandleData(data);
            stream.BeginRead(receivedBuffer, 0, BUFFER_SIZE, OnReceivedDataFromServer, null);
        }
        catch (System.Exception e)
        {

        }
    }

    private static void HandleData(byte[] data)
    {
        ThreadManager.ExecuteOnMainThread(() =>
        {
            using (var packet = new Packet(data))
            {
                var type = packet.ReadInt();
                responses[(ServerPackets)type].Invoke(packet);
            }            
        });
    }

    public static void SendData(Packet packet)
    {
        try
        {
            if (socket != null)
            {
                stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
        }
        catch (Exception e)
        {
            // TODO : Disconnect
        }
    }

    public static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    private static void ResponsePing(Packet packet)
    {
        sw.Stop();
        
        var message = packet.ReadString();
        UnityEngine.Debug.Log($"Response from server {sw.ElapsedMilliseconds}ms.");
    }

    private static void ResponseID(Packet packet)
    {
        var id = packet.ReadString();
        ID = id;
        UnityEngine.Debug.Log($"ID from server : {id}");
    }
}
