using System.IO;
using System.Threading.Tasks;

namespace NetBlade.Data.Storage
{
    public interface IStorage
    {
        void Add(string fileName, byte[] file);

        Task AddAsync(string fileName, byte[] file);

        void Delete(string fileName);

        bool Exists(string fileName);

        FileStream GetFileStream(string fileName, FileMode mode);

        byte[] ReadAllBytes(string fileName);

        Task<byte[]> ReadAllBytesAsync(string fileName);

        void Update(string fileName, byte[] file);

        Task UpdateAsync(string fileName, byte[] file);
    }
}
