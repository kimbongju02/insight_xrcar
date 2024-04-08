using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class Packet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    [SerializeField]
    public struct DataPacket
    {
        public int dataLen;
        public byte[] data;
    }
}
