namespace TV5_VolunteerEventMgmtApp.Utilities
{
    public class PercentageColorVM
    {
        public const string GreatColor = "#44ce1b";
        public const string GoodColor = "#bbdb44";
        public const string PoorColor = "#f2a134";
        public const string BadColor = "#e51f1f";

        public const string GreatClassName = "text-summary-great";
        public const string GoodClassName = "text-summary-good";
        public const string PoorClassName = "text-summary-poor";
        public const string BadClassName = "text-summary-bad";

        public string ClassName { get; set; }
        public double Value { get; set; }
        public PercentageColorVM(double summaryPercent) 
        {
            //Console.WriteLine(summaryPercent);
            if (summaryPercent >= 0.75)
            {
                Console.WriteLine("GT");
                ClassName = GreatClassName;
            }
            if(summaryPercent <0.75 && summaryPercent > 0.60)
            {
                ClassName = GoodClassName;
            }
             if(summaryPercent <= 0.60 && summaryPercent >= 0.50)
            {
                ClassName = PoorClassName;
            }

            if(summaryPercent <0.50) { ClassName = BadClassName; }
            Value= summaryPercent;
        }


    }
}
