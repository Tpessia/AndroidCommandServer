using System.Threading.Tasks;

namespace AndroidCommandServer.Services
{
    public interface ICommandHandler
    {
        Task<string> HandleCommand(string command);
    }
}