using System;
using System.IO;

namespace WebApplication1
{
    public interface ICvManager
    {
        public string Folder { get; }

        public Guid GenerateNewId();

        public void DeleteCv(Guid id);

        public string GetFilePath(Guid id);
    }
}
