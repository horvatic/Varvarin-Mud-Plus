﻿using System;
using System.Net.WebSockets;

namespace Varvarin_Mud_Plus.Engine.UserComponent
{
    public class UserResult
    {
        private readonly WebSocketReceiveResult _receiveResult;
        private readonly byte[] _buffer;
        private bool IsConnectionLost = false;

        public UserResult(WebSocketReceiveResult receiveResult, byte[] buffer)
        {
            _receiveResult = receiveResult;
            _buffer = buffer;
        }

        public bool HasConnectionClosed()
        {
            return _receiveResult.CloseStatus.HasValue;
        }

        public UserCloseResult GetCloseResult()
        {
            return new UserCloseResult(_receiveResult);
        }

        public string GetMessage()
        {
            var message = System.Text.Encoding.Default.GetString(new ArraySegment<byte>(_buffer, 0, _receiveResult.Count).Array);
            return message;
        }

        public void ConntectionLostHasBeenLost()
        {
            IsConnectionLost = true;
        }

        public bool IsConntectionLost()
        {
            return IsConnectionLost;
        }
    }
}
