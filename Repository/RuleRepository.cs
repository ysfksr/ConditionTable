using ConditionTable.Abstracts;
using ConditionTable.Entity;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ConditionTable.Repository
{
    public class RuleRepository : IRuleRepository
    {
        private readonly IConfiguration _configuration;

        public RuleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool AddRule(List<RuleDbModel> model)
        {
            try
            {
                var filePath =  _configuration["FileLocation"];

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true
                };


                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteHeader<RuleDbModel>();
                    csv.NextRecord();
                    foreach (var item in model)
                    {
                        csv.WriteRecord(item);
                        csv.NextRecord();
                    }
                }


                return true;
            }
            catch
            {
                return false;
            }

        }
        public List<RuleDbModel> GetAllRules()
        {
            try
            {
                var filePath = _configuration["FileLocation"];

                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<RuleDbModel>().ToList();
                return records.OrderBy(x => x.LowerBound).ToList();
            }
            catch
            {
                return null;
            }

        }
        public void TruncateTable()
        {
            string filePath = _configuration["FileLocation"];

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public bool IsDataExist()
        {
            try
            {
                var filePath = _configuration["FileLocation"];

                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<RuleDbModel>().ToList();

                return records.Any();
            }
            catch
            {
                return false;
            }
        }
    }
}
