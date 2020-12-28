using System;
using System.IO;

namespace Telegram.Bot.Types.InputFiles
{
    public class NotifyingInputOnlineFile : InputOnlineFile
    {
        private readonly NotifyingStream _notifyingStream;

        public NotifyingInputOnlineFile(Stream content, long size)
            : this(content, size, default)
        { }

        public NotifyingInputOnlineFile(Stream content, long size, string fileName)
            : base(new NotifyingStream(content, size), fileName)
        {
            _notifyingStream = (NotifyingStream)base.Content;
        }

        public event EventHandler<UploadProgressEventArgs> OnProgressUpdated
        {
            add => _notifyingStream.OnProgressUpdated += value;
            remove => _notifyingStream.OnProgressUpdated -= value;
        }

        private class NotifyingStream : Stream
        {
            private readonly Stream _baseStream;
            private readonly int _length;
            private int _totalRead = 0;

            public NotifyingStream(Stream stream, long length)
            {
                _baseStream = stream;
                _length = (int)length;
            }

            public override bool CanSeek => false;
            public override bool CanRead => true;
            public override bool CanWrite => false;
            public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override long Length => throw new NotImplementedException();
            public override void Flush() => throw new NotImplementedException();
            public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();
            public override void SetLength(long value) => throw new NotImplementedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();

            public override int Read(byte[] buffer, int offset, int count)
            {
                int read = _baseStream.Read(buffer, offset, count);
                if (read > 0)
                {
                    _totalRead += read;
                    OnProgressUpdated?.Invoke(this, new UploadProgressEventArgs(_length, _totalRead));
                }
                return read;
            }

            public event EventHandler<UploadProgressEventArgs> OnProgressUpdated;
        }
    }

    public class UploadProgressEventArgs : EventArgs
    {
        public readonly int TotalSize;
        public readonly int Uploaded;
        public readonly float Progress;
        public readonly float ProgressPercentage;

        public UploadProgressEventArgs(int totalSize, int uploaded)
        {
            TotalSize = totalSize;
            Uploaded = uploaded;
            Progress = (float)uploaded / totalSize;
            ProgressPercentage = Progress * 100;
        }
    }
}

// author: https://github.com/MihaZupan
