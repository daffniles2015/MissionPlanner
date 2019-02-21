﻿using System;

namespace UAVCAN
{
    /// <summary>
    /// https://uavcan.org/Specification/4._CAN_bus_transport_layer/
    /// </summary>
    public class CANFrame
    {
        private byte[] packet_data;

        public enum FrameType
        {
            anonymous,
            service,
            message
        }

        public CANFrame(byte[] packet_data)
        {
            this.packet_data = packet_data;
        }

        public FrameType TransferType
        {
            get
            {
                if (SourceNode == 0)
                    return FrameType.anonymous;
                if (IsServiceMsg)
                    return FrameType.service;
                return FrameType.message;
            }
        }

        // message frame
        //0-127
        public byte SourceNode
        {
            get { return (byte)(packet_data[0] & 0x7f); }
        }
        public bool IsServiceMsg
        {
            get { return (packet_data[0] & 0x80) > 0; }
        }
        // 0 - 65535    anon 0-3
        public UInt16 MsgTypeID
        {
            get { return BitConverter.ToUInt16(packet_data, 1); }
        }
        // 0-31 high-low
        public byte Priority
        {
            get { return (byte)(packet_data[3] & 0x1f); }
        }

        // anon frame
        public UInt16 AnonDiscriminator {
            get { return BitConverter.ToUInt16(packet_data, 1); }
        }

        // service frame
        //0-127
        public byte SvcDestinationNode { get { return (byte)(packet_data[1] & 0x7f); } }
        public bool SvcIsRequest { get { return (packet_data[1] & 0x80) > 0; } }
        //0-255
        public byte SvcTypeID { get { return (byte)(packet_data[2]); } }
    }
}