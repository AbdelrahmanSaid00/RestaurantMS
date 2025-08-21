using System.Linq.Expressions;

namespace MexicoRestaurant.Models
{
    public class QueryOptions<T> where T : class // this is the class accept link query 
    {
        public Expression<Func<T, Object>> OrderBy { get; set; } = null!; // OrderBy is a function that takes an object of type T and returns an object to order by
        public Expression <Func<T , bool>> Where { get; set; } = null!; // Where is a function that takes an object of type T and returns a boolean to filter the results

        private string[] includes = Array.Empty<string>(); // includes is an array of strings that represent the navigation properties to include in the query
        public  string Includes
        {
            set => includes = value.Replace(" ", "").Split(','); 
        }
        public string[] GetIncludes() => includes; 
        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
    }
}