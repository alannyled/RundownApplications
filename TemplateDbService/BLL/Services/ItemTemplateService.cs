﻿using TemplateDbService.BLL.Interfaces;
using TemplateDbService.DAL.Interfaces;
using TemplateDbService.DAL.Repositories;
using TemplateDbService.Models;

namespace TemplateDbService.Services
{
    public class ItemTemplateService : IItemTemplateService
    {
        private readonly IItemTemplateRepository _repository;

        public ItemTemplateService(IItemTemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StoryTemplate>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<StoryTemplate> GetByIdAsync(Guid uuid) => await _repository.GetByIdAsync(uuid);

        public async Task CreateAsync(StoryTemplate itemTemplate)
        {
            itemTemplate.UUID = Guid.NewGuid();
            await _repository.CreateAsync(itemTemplate);
        }

        public async Task UpdateAsync(Guid uuid, StoryTemplate itemTemplate)
        {
            await _repository.UpdateAsync(uuid, itemTemplate);
        }

        public async Task DeleteAsync(Guid uuid) => await _repository.DeleteAsync(uuid);
    }
}


