using System.IO;
using System.Threading.Tasks;

namespace NetBlade.Data.Storage
{
    public class FileSystemStorage : IStorage
    {
        private readonly string _directory;

        public FileSystemStorage(string directory)
        {
            this._directory = directory;
            if (!Directory.Exists(this._directory))
            {
                Directory.CreateDirectory(this._directory);
            }
        }

        public virtual void Add(string fileName, byte[] file)
        {
            using FileStream stream = File.Create(Path.Combine(this._directory, fileName));
            stream.Write(file, 0, file.Length);
        }

        public virtual async Task AddAsync(string fileName, byte[] file)
        {
            using FileStream stream = File.Create(Path.Combine(this._directory, fileName));
            await stream.WriteAsync(file, 0, file.Length);
        }

        public virtual void Delete(string fileName)
        {
            File.Delete(Path.Combine(this._directory, fileName));
        }

        public virtual bool Exists(string fileName)
        {
            return File.Exists(Path.Combine(this._directory, fileName));
        }

        public virtual FileStream GetFileStream(string fileName, FileMode mode)
        {
            return File.Open(Path.Combine(this._directory, fileName), mode);
        }

        public virtual byte[] ReadAllBytes(string fileName)
        {
            return File.ReadAllBytes(Path.Combine(this._directory, fileName));
        }

        public virtual async Task<byte[]> ReadAllBytesAsync(string fileName)
        {
            return await File.ReadAllBytesAsync(Path.Combine(this._directory, fileName));
        }

        public virtual void Update(string fileName, byte[] file)
        {
            string fullFileName = Path.Combine(this._directory, fileName);

            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }

            this.Add(fileName, file);
        }

        public virtual async Task UpdateAsync(string fileName, byte[] file)
        {
            string fullFileName = Path.Combine(this._directory, fileName);

            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }

            await this.AddAsync(fileName, file);
        }
    }
}
