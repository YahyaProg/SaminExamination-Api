namespace SaminExamination.Helper
{
    public static class Pagination
    {
        public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source, int page, int PageSize)
        {
            return source.Skip((page - 1) * PageSize).Take(PageSize);
        }
    }
}
