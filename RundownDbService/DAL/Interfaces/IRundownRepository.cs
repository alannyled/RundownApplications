﻿using RundownDbService.Models;

namespace RundownDbService.DAL.Interfaces
{
    public interface IRundownRepository
    {
        Task<List<Rundown>> GetAllAsync();
        Task<Rundown> GetByIdAsync(Guid uuid);
        Task CreateAsync(Rundown rundown);
        Task UpdateAsync(Guid uuid, Rundown rundown);
        Task DeleteAsync(Guid uuid);
    }
}