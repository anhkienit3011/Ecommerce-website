namespace ThucTapProject.Page
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalRecord { get; set; }
        public int TotalPage
        {
            get
            {
                if (PageSize == 0) return 0;
                var total = TotalRecord / PageSize;
                if (TotalRecord % PageSize != 0)
                {
                    total++;
                }
                return total;   
            }
        }

        public Pagination()
        {
            PageSize = 20;
            PageNumber = 1;
        }

        public Pagination(int pageSize, int pageNumber, int totalRecord) {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalRecord = totalRecord;
        }
    }
}
