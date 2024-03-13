using Newtonsoft.Json;

namespace ThucTapProject.Page
{
    public class pageResult<T>
    {
        public Pagination Pagination { get; set; }
        public IEnumerable<T> Data { get; set; }

        public pageResult(Pagination pagination, IEnumerable<T> data)
        {
            Pagination = pagination;
            Data = data;
        }

        public static IEnumerable<T> ToPageResult(Pagination pagination, IEnumerable<T> data)
        {
            pagination.TotalRecord = data.Count();
            pagination.PageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;
            data = data.Skip(pagination.PageSize*(pagination.PageNumber-1)).Take(pagination.PageSize);
            return data;
        }
    }
}
