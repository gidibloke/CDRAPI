using CDRAPI.Data;
using CDRAPI.Interfaces;
using Domain.LookupModels;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Globalization;
using System.Security.Cryptography.Xml;

namespace CDRAPI.Services
{
    public class UploadCallRecordsService : IUploadCallRecords
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public IList<Currency> Currencies { get; set; } = new List<Currency>();

        public UploadCallRecordsService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<int> UploadRecords()
        {
            if (Currencies.Count == 0)
            {
                Currencies = await _context.Currencies.ToListAsync();
            }
            //var csvFileFolder = Path.Combine(_env.WebRootPath, "wwwroot");
            var files = Directory.GetFiles(_env.WebRootPath);
            if (files.Length == 0)
            {
                return 0;
            }
            foreach (var file in files)
            {
                await UploadInBatches(file);
            }
            return files.Length;
        }


        private async Task UploadInBatches(string filePath)
        {
            //Uploading in batches to make sure the application scales well for CSV file of any size. 
            //In production, the server should be able to load so much more record in memory
            //StreamReader allows you to efficiently read and process the contents of a file without loading the entire file into memory
            
            var batchSize = 1000;
            using var reader = new StreamReader(filePath);
            reader.ReadLine();
            while(!reader.EndOfStream)
            {
                var rows = new List<string>();
                for(int i = 0; i < batchSize && !reader.EndOfStream; i++) 
                {
                    string row = reader.ReadLine();
                    rows.Add(row);
                }
                await InsertRowsAsync(rows);
            }

        }

        private async Task InsertRowsAsync(List<string> rows)
        {
            var callrecords = new List<CallRecord>();
            foreach (var row in rows)
            {
                //I have assumed the CSV file is flawless. In production, i can add a try,catch block here to handle any parsing error from the ParseStringToRecord method. This ensures other rows are inserted.
                //Catch block might perform some sort of logging for review after the upload process.
                var record = ParseStringToRecord(row);
                callrecords.Add(record);
            }
            //if row is processed in batches of more than 1000, other libaries for bulk inserts should be considered
            //an example is EFCore.BulkExtensions
            _context.AddRange(callrecords);
            await _context.SaveChangesAsync();
        }

        private CallRecord ParseStringToRecord(string record)
        {
            string[] cols = record.Split(',');
            var callrecord = new CallRecord
            {
                CallerId = cols[0],
                Recipient = cols[1],
                CallDate = DateTime.ParseExact(cols[2], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                EndTime = TimeSpan.ParseExact(cols[3], "hh\\:mm\\:ss", CultureInfo.InvariantCulture),
                Duration = int.Parse(cols[4]),
                Cost = decimal.Parse(cols[5]),
                Reference = cols[6],
                Currency = Currencies.Where(x => x.Description == cols[7]).Single().Id
            };

            return callrecord;
        }
    }
}
