using System.Collections.Generic;
using System;
using System.Text;

public class PacketBuffer : IDisposable
{

        List<byte> _bufferList;
        byte[] _readBuffer;
        int _readPos;
        bool _buffUpdate = false;

        public PacketBuffer()
        {
            _bufferList = new List<byte>();
            _readPos = 0;
        }
        public int GetReadPos()
        {
            return _readPos;
        }
        public byte[] ToArray()
        {
            return _bufferList.ToArray();
        }
        public int Count()
        {
            return _bufferList.Count;
        }
        public int Length()
        {
            return Count() - _readPos;
        }
        public void Clear()
        {
            _bufferList.Clear();
            _readPos = 0;
        }

        // Write Data (Converts data into bytes then adds them to the bufer list
        public void WriteBytes(byte[] input)
        {
            _bufferList.AddRange(input);
            _buffUpdate = true;

        }
        public void WriteByte(byte input)
        {
            _bufferList.Add(input);
            _buffUpdate = true;

        }
        public void WriteInteger(int input)
        {
            _bufferList.AddRange(BitConverter.GetBytes(input));
            _buffUpdate = true;
        }
        public void WriteFloat(float input)
        {
            _bufferList.AddRange(BitConverter.GetBytes(input));
            _buffUpdate = true;
        }
        public void WriteString(string input)
        {
            // Writes length of string to keep a record of the string's length
            _bufferList.AddRange(BitConverter.GetBytes(input.Length));

            _bufferList.AddRange(Encoding.ASCII.GetBytes(input));
            _buffUpdate = true;
        }

        // Read Data (Reads bytes from the buffer list
        public int ReadInteger(bool peek = true)
        {
            if (_bufferList.Count > _readPos)
            {
                if (_buffUpdate)
                {
                    _readBuffer = _bufferList.ToArray();
                    _buffUpdate = false;
                }

                int value = BitConverter.ToInt32(_readBuffer, _readPos);
                if (peek & _bufferList.Count > _readPos)
                {
                    _readPos += 4;
                }
                return value;
            }
            else
            {
                throw new Exception("Buffer is past its Limit!");
            }
        }
        public float ReadFloat(bool peek = true)
        {
            if (_bufferList.Count > _readPos)
            {
                if (_buffUpdate)
                {
                    _readBuffer = _bufferList.ToArray();
                    _buffUpdate = false;
                }

                float value = BitConverter.ToSingle(_readBuffer, _readPos);
                if (peek & _bufferList.Count > _readPos)
                {
                    _readPos += 4;
                }
                return value;
            }
            else
            {
                throw new Exception("Buffer is past its Limit!");
            }
        }
        public byte ReadByte(bool peek = true)
        {
            if (_bufferList.Count > _readPos)
            {
                if (_buffUpdate)
                {
                    _readBuffer = _bufferList.ToArray();
                    _buffUpdate = false;
                }

                byte value = _readBuffer[_readPos];
                if (peek & _bufferList.Count > _readPos)
                {
                    _readPos += 1;
                }
                return value;
            }
            else
            {
                throw new Exception("Buffer is past its Limit!");
            }
        }
        public byte[] ReadBytes(int length, bool peek = true)
        {

            if (_buffUpdate)
            {
                _readBuffer = _bufferList.ToArray();
                _buffUpdate = false;
            }

            byte[] value = _bufferList.GetRange(_readPos, length).ToArray();
            if (peek & _bufferList.Count > _readPos)
            {
                _readPos += length;
            }
            return value;


        }
        public string ReadString(bool peek = true)
        {
            int length = ReadInteger(true);
            if (_buffUpdate)
            {
                _readBuffer = _bufferList.ToArray();
                _buffUpdate = false;
            }

            string value = Encoding.ASCII.GetString(_readBuffer, _readPos, length);
            if (peek & _bufferList.Count > _readPos)
            {
                _readPos += length;
            }
            return value;

        }

        // IDisposable
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bufferList.Clear();
                }
                _readPos = 0;
            }
            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    
}
