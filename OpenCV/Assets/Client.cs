using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using OpenCvSharp;
using System.Runtime.InteropServices;
using System.Text;
using System.Buffers.Text;
using System.IO;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;

public class Client : MonoBehaviour
{
    Socket client;

    [SerializeField] string ip;
    [SerializeField] int port;

    byte[] payloadSize = new byte[4];
    int bytesReceived;

    [SerializeField] RawImage rawImage;
    [SerializeField] Texture2D bmp;

    [SerializeField]
    byte[] messageData;
    [SerializeField]
    byte[] resultData;
    int index = 0;

    private void Awake()
    {
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    private void Start()
    {
        client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
        Debug.Log("socket connenct success");
    }

    private void Update()
    {
        if (index >= 1)
            return;
        ReceiveData();
        index++;
    }

    private void ReceiveData()
    {
        try
        {
            // Receive the size of the incoming message
            bytesReceived = client.Receive(payloadSize, 4, SocketFlags.None);
            if (bytesReceived == 0)
            {
                Debug.Log("Client disconnected");
                return;
            }

            // Get the actual message size
            int messageSize = BitConverter.ToInt32(payloadSize.Reverse().ToArray(), 0);
            //Console.WriteLine("Receive payloadSize Size : " + messageSize);
            Debug.Log(messageSize);

            // Receive the entire message data
            messageData = new byte[messageSize];
            int totalBytesReceived = 0;
            while (totalBytesReceived < messageSize)
            {
                int received = client.Receive(messageData, totalBytesReceived, messageSize - totalBytesReceived, SocketFlags.None);
                if (received == 0)
                {
                    //Console.WriteLine("Client disconnected.");
                    Debug.Log("Client disconnected");
                    break;
                }
                totalBytesReceived += received;
            }
            //Console.WriteLine("Receive messageData Size : " + messageData.Length);
            Debug.Log("totalBytesReceived : " + messageData.Length);

            //ConvertTexture2D(messageData);
            string path = "C:/Users/user/Desktop/abc.txt";
            //File.WriteAllBytes(path, messageData);
            resultData = File.ReadAllBytes(path);
        }
        catch (SocketException ex)
        {
            //Console.WriteLine("Socket error: " + ex.Message);
            Debug.Log("Socket error : " + ex.Message);
            client.Close(); // Ensure proper socket closure
        }
    }

    //void ConvertTexture2D(byte[] imageData)
    //{
    //    try
    //    {
    //        Mat src = Cv2.ImDecode(imageData, ImreadModes.Color);
    //        //int size = src.Cols * src.Rows * src.ElemSize();
    //        //resultData = new byte[size];

    //        // Mat 데이터를 byte array로 복사
    //        //Marshal.Copy(src.Data, resultData, 0, size);
    //        bool isEncode = Cv2.ImEncode(".png", src, out resultData);

    //        bmp = new Texture2D(720, 480, TextureFormat.RGBA64, false);

    //        //bmp.hideFlags = HideFlags.HideAndDontSave;
    //        //bmp.filterMode = FilterMode.Point;
    //        bmp.LoadRawTextureData(resultData);

    //        //Vector2 pivot = new Vector2(0.5f, 0.5f);
    //        //UnityEngine.Rect tRect = new UnityEngine.Rect(0, 0, bmp.width, bmp.height);
    //        //Sprite newSprite = Sprite.Create(bmp, tRect, pivot);

    //        //image.overrideSprite = newSprite;

    //        bmp.Apply();
    //        rawImage.texture = bmp;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.Log("변환 실패 " + ex);
    //    }
    //}

    void ConvertTexture2D(byte[] imageData)
    {
        try
        {
            Texture2D tex = new Texture2D(720, 720, TextureFormat.ETC2_RGBA8, false);

            tex.LoadRawTextureData(imageData);
            tex.Apply();

            rawImage.texture = tex;
        }
        catch (Exception ex)
        {
            Debug.Log("변환 실패 " + ex);
        }
    }
}