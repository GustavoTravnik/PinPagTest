namespace BankServices.Dto
{
    public class PaginatedRespondeDto<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Items { get; set; }

        public PaginatedRespondeDto(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
