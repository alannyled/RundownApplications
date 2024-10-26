using RundownDbService.Models;

namespace RundownDbService.BLL.Interfaces
{
    public interface IItemDetailService
    {
        ItemDetail? GetModel(string type);
        Task<RundownItem> CreateItemDetailAsync(Guid rundownId, RundownItem existingItem);
    }
}
