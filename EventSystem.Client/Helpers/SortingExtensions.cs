using System.Linq.Expressions;

namespace EventSystem.Client.Helpers
{
    public static class SortingExtensions
    {
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string sortExpression, SortDirection direction)
        {
            var param = Expression.Parameter(typeof(T));
            var sortLambda = Expression.Lambda<Func<T, object>>(
                Expression.Convert(
                    Expression.Property(param, sortExpression),
                    typeof(object)
                ),
                param
            );

            return direction == SortDirection.Ascending
                ? source.AsQueryable().OrderBy(sortLambda)
                : source.AsQueryable().OrderByDescending(sortLambda);
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

}
