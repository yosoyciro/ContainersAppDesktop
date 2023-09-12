using ContainersDesktop.Dominio.Models.Base;
using ContainersDesktop.Infraestructura.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ContainersDesktop.Infraestructura.Specification;
public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        //Filtros
        if (spec.Criteria != null)
        {
            inputQuery = inputQuery.Where(spec.Criteria);
        }
       
        //Includes
        inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));


        return inputQuery;
    }
}
