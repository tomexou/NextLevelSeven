﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Streaming
{
    public class MLPStreamReader : HL7StreamReader
    {
        public MLPStreamReader(Stream baseStream) : base(baseStream)
        {
        }

        public override IMessage Read()
        {
            int vtByte = BaseStream.ReadByte();
            if (vtByte == -1)
            {
                return null;
            }
            if (vtByte != 0x0B)
            {
                throw new MLPStreamException(ErrorCode.HeaderByteIsIncorrect);
            }

            using (var mem = new MemoryStream())
            {
                unchecked
                {
                    while (true)
                    {
                        var buffer = BaseStream.ReadByte();
                        if (buffer == -1)
                        {
                            throw new MLPStreamException(ErrorCode.MlpDataEndedPrematurely);
                        }

                        if (buffer == 0x1C)
                        {
                            buffer = BaseStream.ReadByte();
                            if (buffer == 0x0D)
                            {
                                break;
                            }
                            if (buffer == -1)
                            {
                                throw new MLPStreamException(ErrorCode.MlpDataEndedPrematurely);
                            }
                        }
                        mem.WriteByte((byte)buffer);
                    }
                }

                return Decode(mem.ToArray());
            }
        }

        /// <summary>
        /// Read all messages in the stream. If empty, there were no more messages.
        /// </summary>
        /// <returns>Messages that were read.</returns>
        public override IEnumerable<IMessage> ReadAll()
        {
            var messages = new List<IMessage>();
            while (true)
            {
                var message = Read();
                if (message == null)
                {
                    return messages;
                }
                messages.Add(message);
            }
        }
    }
}
