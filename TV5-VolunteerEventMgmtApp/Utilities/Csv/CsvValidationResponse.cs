namespace TV5_VolunteerEventMgmtApp.Utilities.Csv
{
    public class CsvValidationResponse
    {
        public List<string> Errors { get; set; } // List of errors that the ( singer is too old, no first/last given )
        public int LineOfRecord { get; set; } // line of the record in the file
        public bool IsValid { get; set; } // is this record's values valid
    }
}
