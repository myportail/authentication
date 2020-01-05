using System.Threading.Tasks;

namespace authInit.Services
{
    public interface IDatabaseUpdater
    {
        Task UpdateDb();
    }
}