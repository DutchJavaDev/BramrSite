using BramrSite.Models;
using System.Threading.Tasks;

namespace BramrSite.Classes.Interfaces
{
    public interface IApiDesignConnection
    {
        Task<ChangeModel> GetOneFromDB(int ID);
        Task AddToDB(int ID, ChangeModel CurrentChange);
        Task DeleteAllFromDB();
        Task DeleteAmountFromDB(int ID);
    }
}
