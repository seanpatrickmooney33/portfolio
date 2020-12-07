using EngineServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Prototype.Data;

namespace EngineServer.Message
{
    public abstract class MessageProcessor : IMessageProcessor
    {
        protected IRedisBuffer RedisBuffer;

        protected JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { MaxDepth = 1000, IgnoreNullValues = true, };


        public MessageProcessor(IRedisBuffer r)
        {
            this.RedisBuffer = r;
        }

        private String ConvertNewLine(String FileContent) 
        { 
            if(FileContent.Contains(System.Environment.NewLine) == false)
            {
                FileContent = FileContent.Replace("\n", System.Environment.NewLine);
            }
            return FileContent;
        }

        public void DoWork(String FileContent, String FileName)
        {
            FileContent = this.ConvertNewLine(FileContent);
            this.Validate(FileContent);
            this.Parse(FileContent, EnumHelper<ElectionTypeEnum>.GetByAbreviation(FileName.Substring(3, 2).ToUpper()));
            this.Process();
        }

        protected abstract void Validate(string FileContent);
        protected abstract void Parse(string FileContent, ElectionTypeEnum overrideFromFileName);
        protected abstract void Process();
    }
}
