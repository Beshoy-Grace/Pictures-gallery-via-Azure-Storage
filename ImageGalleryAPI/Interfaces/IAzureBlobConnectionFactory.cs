using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImageGalleryAPI.Interfaces
{
    public interface IAzureBlobConnectionFactory
    {
         Task<CloudBlobContainer> GetBlobContainer();
    }
}