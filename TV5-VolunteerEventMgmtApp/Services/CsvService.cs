﻿using CsvHelper;
using CsvHelper.TypeConversion;
using System.Globalization;
using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.Services
{
    public class CsvService
    {
        public IEnumerable<SingerCsvUpload> ReadSingerCsvFile(Stream fileStream)
        {
            try
            {
                using (var reader = new StreamReader(fileStream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<SingerCsvUpload>();
                    return records.ToList();
                }
            }
            catch (HeaderValidationException ex)
            {
                // Specific exception for header issues
                throw new ApplicationException("CSV file header is invalid.", ex);
            }
            catch (TypeConverterException ex)
            {
                // Specific exception for type conversion issues
                throw new ApplicationException("CSV file contains invalid data format.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // General exception for other issues
                throw new ApplicationException("Error reading CSV file", ex);
            }
        }
    }
}
