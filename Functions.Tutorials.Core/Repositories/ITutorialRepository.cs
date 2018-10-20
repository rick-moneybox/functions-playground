using Functions.Tutorials.Core.Domain;
using System.Threading.Tasks;

namespace Functions.Tutorials.Core.Repositories
{
    public interface ITutorialRepository
    {
        Task<Tutorial> FindTutorial(string userId);

        Task<Tutorial> UpsertTutorial(Tutorial tutorial);
    }
}
