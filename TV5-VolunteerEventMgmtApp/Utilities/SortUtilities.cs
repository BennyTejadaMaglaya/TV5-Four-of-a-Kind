namespace TV5_VolunteerEventMgmtApp.Utilities
{
    public static class SortUtilities
    {
        public static void SwapSortDirection(ref string sortField, ref string sortDirection, string[] sortOptions, string? actionButton)
        {
            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted!
            {
                //page = 1;//Reset page to start

                if (sortOptions.Contains(actionButton))//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }
        }
    }
}
