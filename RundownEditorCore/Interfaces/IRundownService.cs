﻿using RundownEditorCore.DTO;

namespace RundownEditorCore.Interfaces
{
    public interface IRundownService
    {
        Task<List<RundownDTO>> GetActiveRundowsAsync();
    }
}