using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Transformer.Wars.Models.DB;
using Transformer.Wars.Models.DTO;

namespace Transformer.Wars.Interfaces
{
    public interface IWarService
    {
        Task<List<Models.DB.Transformer>> GetTransformers(Models.AllegianceTypes allegianceType);
        Task<Models.DB.Transformer> GetTransformer(int id);
        Task<int> GetScore(int id);
        Task<Models.DB.Transformer> AddTransformer(TransformerRequest transformer);
        Task<Models.DB.Transformer> UpdateTransformer(int id, TransformerRequest transformer);
        Task DeleteTransformer(int id);
        Task TacticalNuke();
        Task<Models.DTO.WarsResponse> RunWar();
    }
}
