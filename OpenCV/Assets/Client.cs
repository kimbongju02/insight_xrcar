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

public class Client : MonoBehaviour
{
    Socket client;

    [SerializeField] string ip;
    [SerializeField] int port;

    byte[] payloadSize = new byte[4];
    int bytesReceived;

    [SerializeField] RawImage rawImage;
    [SerializeField] Texture2D bmp;

    byte[] messageData;
    [SerializeField] byte[] test;

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
        ReceiveData(); 
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

            ConvertTexture2D(messageData);
        }
        catch (SocketException ex)
        {
            //Console.WriteLine("Socket error: " + ex.Message);
            Debug.Log("Socket error : " + ex.Message);
            client.Close(); // Ensure proper socket closure
        }
    }

    void ConvertTexture2D(byte[] imageData)
    {
        try
        {
            /*
            Mat mat = DecodeImage(imageData);
            Debug.Log(mat.Size());
            Texture2D texture = MatToTexture(mat);
            */

            /*
            Texture2D bmp;
            bmp = new Texture2D(720, 480);
            IntPtr _buff = Marshal.AllocHGlobal(720 * 480 * 4);
            bmp.LoadRawTextureData(imageData, _buff);
            bmp.Apply();

            rawImage.texture = bmp;
            */
            //stringData = Convert.ToBase64String(imageData);
            //datas = Convert.FromBase64String(stringData);
            test = new byte[]
            {
                0x30, 0x32, 0x32, 0x32, 0xe7, 0x30, 0xaa, 0x7f, 0x32, 0x32, 0x32, 0x32, 0xf9, 0x40, 0xbc, 0x7f,
                0x03, 0x03, 0x03, 0x03, 0xf6, 0x30, 0x02, 0x05, 0x03, 0x03, 0x03, 0x03, 0xf4, 0x30, 0x03, 0x06,
                0x32, 0x32, 0x32, 0x32, 0xf7, 0x40, 0xaa, 0x7f, 0x32, 0xf2, 0x02, 0xa8, 0xe7, 0x30, 0xff, 0xff,
                0x03, 0x03, 0x03, 0xff, 0xe6, 0x40, 0x00, 0x0f, 0x00, 0xff, 0x00, 0xaa, 0xe9, 0x40, 0x9f, 0xff,
                0x5b, 0x03, 0x03, 0x03, 0xca, 0x6a, 0x0f, 0x30, 0x03, 0x03, 0x03, 0xff, 0xca, 0x68, 0x0f, 0x30,
                0xaa, 0x94, 0x90, 0x40, 0xba, 0x5b, 0xaf, 0x68, 0x40, 0x00, 0x00, 0xff, 0xca, 0x58, 0x0f, 0x20,
                0x00, 0x00, 0x00, 0xff, 0xe6, 0x40, 0x01, 0x2c, 0x00, 0xff, 0x00, 0xaa, 0xdb, 0x41, 0xff, 0xff,
                0x00, 0x00, 0x00, 0xff, 0xe8, 0x40, 0x01, 0x1c, 0x00, 0xff, 0x00, 0xaa, 0xbb, 0x40, 0xff, 0xff,
            };

            bmp = new Texture2D(720, 480, TextureFormat.RGBA64, false);

            bmp.hideFlags = HideFlags.HideAndDontSave;
            bmp.filterMode = FilterMode.Point;
            bmp.LoadImage(test);

            //Vector2 pivot = new Vector2(0.5f, 0.5f);
            //UnityEngine.Rect tRect = new UnityEngine.Rect(0, 0, bmp.width, bmp.height);
            //Sprite newSprite = Sprite.Create(bmp, tRect, pivot);

            //image.overrideSprite = newSprite;

            bmp.Apply();
            rawImage.texture = bmp;
        }
        catch (Exception ex)
        {
            Debug.Log("변환 실패 " + ex);
        }
    }
}