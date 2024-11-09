using RundownDbService.Models;

namespace RundownDbService.BLL.Interfaces
{
    public interface IItemDetailService
    {
        ItemDetail? GetModel(string type);
        Task<Rundown> CreateItemDetailAsync(Rundown rundown, RundownItem existingItem);
    }
}
