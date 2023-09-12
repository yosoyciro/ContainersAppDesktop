using System.Linq.Expressions;
using ContainersDesktop.Infraestructura.Contracts;

namespace ContainersDesktop.Logica.Specification;
public class BaseSpecification<T> : ISpecification<T>
{
    public BaseSpecification()
    {
    }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>> Criteria
    {
        get; private set;
    }
    protected void SetCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

    protected void AgregarIncludes(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }    
}
