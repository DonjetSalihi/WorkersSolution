using System.Threading.Tasks;
using WorkersSolution.Models;

namespace WorkersSolution.Services
{
    public interface IRepository<T>
    {
        Task<ApiResponse> GetPictureByWorkerIdAsync(string workerId);
        Task<ApiResponse> GetAllWorkersAsync();
        Task<ApiResponse> GetLocationsByWorkerIdAsync(string workerId);
    }
}
