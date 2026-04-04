using CNCEmu.Models;
using System.Collections.Generic;
using System;

namespace CNCEmu.Interfaces.Repositories
{
    /// <summary>
    /// Interface for game repository.
    /// </summary>
    public interface IGameRepository
    {
        void Save(Game gameState);

        Game GetById(Guid id);

        void Delete(Guid id);

        IEnumerable<Game> GetAll();
    }
}
