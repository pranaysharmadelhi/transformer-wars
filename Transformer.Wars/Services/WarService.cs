using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Transformer.Wars.Controllers;
using Transformer.Wars.Interfaces;
using Transformer.Wars.Models.DB;
using Transformer.Wars.Models.DTO;
using Transformer.Wars.Exceptions;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Transformer.Wars.Models;

namespace Transformer.Wars.Services
{
    public class WarService : IWarService
    {
        private ILogger _logger;
        private TransformerWarsContext _dbcontext;

        private const string OPTIMUS = "Optimus";
        private const string PREDAKING = "Predaking";
        private static Random random = new Random();


        public WarService(TransformerWarsContext dbcontext, ILogger<WarService> logger)
        {
            _logger = logger;
            _dbcontext = dbcontext;
        }

        public async Task<List<Models.DB.Transformer>> GetTransformers(Models.AllegianceTypes allegianceType)
        {
            _logger.LogInformation("WarService.GetTransformers");
            _logger.LogInformation("Get all registered transformers for {allegianceType}", allegianceType);
            return await _dbcontext.Transformers.Where(x => (int)allegianceType == 0 || ((int)allegianceType > 0 && x.AllegianceTypeId == (int)allegianceType)).OrderBy(x => x.Name).ToListAsync();

        }

        public async Task<Models.DB.Transformer> GetTransformer(int id)
        {

            _logger.LogInformation("WarService.GetTransformer");
            _logger.LogInformation("Retrieving the details of a registered Transformer {id}", id);
            var dbTransformer = await _dbcontext.Transformers.FindAsync(id);
            if (dbTransformer == null)
            {
                throw new TransformerWarsException("Unable to find Transformer#" + id);
            }
            return (dbTransformer);

        }

        public async Task<Models.DB.Transformer> AddTransformer(TransformerRequest transformer) {

            _logger.LogInformation("WarService.AddTransformer");
            _logger.LogInformation("Insert transformer {transformer}", transformer.Name);
            var transformerWithSameName = _dbcontext.Transformers.Where(x => x.Name == transformer.Name);
            if (transformerWithSameName.Count() > 0)
            {
                throw new TransformerWarsException("Duplicate name found in Transformer#" + transformerWithSameName.FirstOrDefault().TransformerId);
            }
            var transformerWithSameRank = _dbcontext.Transformers.Where(x => x.Rank == transformer.Rank && x.AllegianceTypeId == (int)transformer.AllegianceType);
            if (transformerWithSameRank.Count() > 0)
            {
                throw new TransformerWarsException("Duplicate rank in Transformer#" + transformerWithSameRank.FirstOrDefault().TransformerId);
            }
            var dbTransformer = transformer.GetDBObject();
            _dbcontext.Transformers.Add(dbTransformer);
            await _dbcontext.SaveChangesAsync();
            return (dbTransformer);
        }


        public async Task<Models.DB.Transformer> UpdateTransformer(int id, TransformerRequest transformer) {
            _logger.LogInformation("WarService.UpdateTransformer");
            _logger.LogInformation("Update transformer {id}", id);
            var dbTransformer = await _dbcontext.Transformers.FindAsync(id);
            if (dbTransformer == null)
            {
                throw new TransformerWarsException("Unable to find Transformer#" + id);
            }
            var transformerWithSameName = _dbcontext.Transformers.Where(x => x.Name == transformer.Name && x.TransformerId!= dbTransformer.TransformerId);
            if (transformerWithSameName.Count() > 0)
            {
                throw new TransformerWarsException("Duplicate name found in Transformer#" + transformerWithSameName.FirstOrDefault().TransformerId);
            }
            var transformerWithSameRank = _dbcontext.Transformers.Where(x => x.Rank == transformer.Rank && x.AllegianceTypeId == (int)transformer.AllegianceType && x.TransformerId != dbTransformer.TransformerId);
            if (transformerWithSameRank.Count() > 0)
            {
                throw new TransformerWarsException("Duplicate rank in Transformer#" + transformerWithSameRank.FirstOrDefault().TransformerId);
            }
            dbTransformer.AllegianceTypeId = (int)transformer.AllegianceType;
            dbTransformer.Courage = transformer.Courage;
            dbTransformer.Endurance = transformer.Endurance;
            dbTransformer.Firepower = transformer.Firepower;
            dbTransformer.Intelligence = transformer.Intelligence;
            dbTransformer.Name = transformer.Name;
            dbTransformer.Rank = transformer.Rank;
            dbTransformer.Skill = transformer.Skill;
            dbTransformer.Speed = transformer.Speed;
            dbTransformer.Strength = transformer.Strength;
            await _dbcontext.SaveChangesAsync();
            return (dbTransformer);
        }

        public async Task DeleteTransformer(int id)
        {
            _logger.LogInformation("WarService.DeleteTransformer");
            _logger.LogInformation("Delete transformer {id}", id);
            var dbTransformer = await _dbcontext.Transformers.FindAsync(id);
            _dbcontext.Transformers.Remove(dbTransformer);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<int> GetScore(int id)
        {
            var transformer = await GetTransformer(id);

            String connection = _dbcontext.Database.GetDbConnection().ConnectionString;
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetScore", conn);
                cmd.Parameters.Add(new SqlParameter("@TransformerId", id));
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return Convert.ToInt32(dt.Rows[0][0]);
                }
            }
        }

        public async Task TacticalNuke()
        {
            await _dbcontext.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE Transformers");
        }

        public async Task<Models.DTO.WarsResponse> RunWar()
        {
            WarsResponse warsResponse = new WarsResponse();
            var optimus = await _dbcontext.Transformers.Where(x => x.Name == OPTIMUS).FirstOrDefaultAsync();
            var predaking = await _dbcontext.Transformers.Where(x => x.Name == PREDAKING).FirstOrDefaultAsync();
            if (optimus != null && predaking != null && optimus.Allegiance != predaking.Allegiance)
            {
                warsResponse.Battles.Add("Optimus is on one team and Predaking is on another team. War ended.");
                _logger.LogInformation("Optimus is on one team and Predaking is on another team. War ended.");
                return warsResponse;
            }

            List<Models.DB.Transformer> autoBots = await _dbcontext.Transformers.Where(x => x.AllegianceTypeId == (int)AllegianceTypes.Autobot).OrderByDescending(x => x.Rank).ToListAsync();
            List<Models.DB.Transformer> decepticon = await _dbcontext.Transformers.Where(x => x.AllegianceTypeId == (int)AllegianceTypes.Decepticon).OrderByDescending(x => x.Rank).ToListAsync();

            Parallel.For(0, Math.Max(autoBots.Count, decepticon.Count), i => {
            
                if (i >= autoBots.Count)
                {
                    _logger.LogInformation("No more Autobots left to fight. Add decepticon to survivor");
                    warsResponse.Battles.Add(String.Format("Batle#{0}: No more Autobots left to fight. Add decepticon#{1} to survivor", i, decepticon[i].TransformerId));

                    warsResponse.Decepticon.Survivors.Add(decepticon[i]);
                }

                else if (i >= decepticon.Count)
                {
                    _logger.LogInformation("No more Decepticons left to fight. Add autoBot to survivor");
                    warsResponse.Battles.Add(String.Format("Batle#{0}: No more Decepticons left to fight. Add autoBot#{1} to survivor", i, autoBots[i].TransformerId));
                    warsResponse.Autobots.Survivors.Add(autoBots[i]);
                }
                else
                {
                    _logger.LogInformation("Battle#{i}: {autobot} vs {decepticon}", i, autoBots[i], decepticon[i]);

                    AllegianceTypes winningAllegianceTypes = GetWinner(autoBots[i], decepticon[i]);
                    warsResponse.Battles.Add(String.Format("Batle#{0}: autoBot#{1} vs decepticon#{2}. {3} won.", i, autoBots[i].TransformerId, decepticon[i].TransformerId, winningAllegianceTypes.ToString()));

                    if (winningAllegianceTypes == AllegianceTypes.Autobot)
                    {
                        warsResponse.Autobots.Victors.Add(autoBots[i]);
                        warsResponse.Decepticon.Losers.Add(decepticon[i]);
                    }
                    else
                    {
                        warsResponse.Decepticon.Victors.Add(decepticon[i]);
                        warsResponse.Autobots.Losers.Add(autoBots[i]);
                    }
                }
            });





            return warsResponse;
        }

        public AllegianceTypes GetWinner(Models.DB.Transformer autobot, Models.DB.Transformer decepticon)
        {
            _logger.LogInformation("WarService.GetWinner {autobot} vs {decepticon}", autobot.Name, decepticon.Name);

            if (autobot.Allegiance == decepticon.Allegiance)
            {
                throw new Exception("Invalid Battle");
            }

            if (autobot.Name.Equals(OPTIMUS, StringComparison.InvariantCultureIgnoreCase) || autobot.Name.Equals(PREDAKING, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("autobot won by name: "+ autobot.Name);

                return AllegianceTypes.Autobot;
            }
            if (decepticon.Name.Equals(OPTIMUS, StringComparison.OrdinalIgnoreCase) || decepticon.Name.Equals(PREDAKING, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("decepticon won by name: " + decepticon.Name);

                return AllegianceTypes.Decepticon;
            }

            if (autobot.Strength - decepticon.Strength >= 3 && decepticon.Courage < 5)
            {
                _logger.LogInformation("autobot won, decepticon ran away");

                return AllegianceTypes.Autobot;

            }

            if (decepticon.Strength - autobot.Strength >= 3 && autobot.Courage < 5)
            {
                _logger.LogInformation("decepticon won, autobot ran away");

                return AllegianceTypes.Decepticon;

            }

            if (Math.Abs(autobot.Skill - decepticon.Skill) >= 5)
            {
                if (autobot.Skill > decepticon.Skill)
                {
                    _logger.LogInformation("autobot won by skill");

                    return AllegianceTypes.Autobot;
                }
                else
                {
                    _logger.LogInformation("decepticon won by skill");
                    return AllegianceTypes.Decepticon;
                }
            }


            if (autobot.Score == decepticon.Score)
            {
                _logger.LogInformation("Its a tie. Choose randomly");
                int n1 = random.Next(1, 10);
                int n2 = random.Next(1, 10);
                return n1 > n2 ? AllegianceTypes.Autobot : AllegianceTypes.Decepticon;
            }

            if (autobot.Score > decepticon.Score)
            {
                _logger.LogInformation("autobot won by Score");

                return AllegianceTypes.Autobot;
            }
            else
            {
                _logger.LogInformation("decepticon won by Score");

                return AllegianceTypes.Decepticon;
            }

        }

    }
}
