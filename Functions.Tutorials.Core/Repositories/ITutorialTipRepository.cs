using Functions.Tutorials.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.Tutorials.Core.Repositories
{
    public interface ITutorialTipRepository
    {
        Task<List<TutorialTip>> GetAll();
    }
}
