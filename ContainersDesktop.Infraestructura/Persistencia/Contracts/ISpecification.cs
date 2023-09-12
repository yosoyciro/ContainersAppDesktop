using System.Linq.Expressions;

namespace ContainersDesktop.Infraestructura.Contracts;
public interface ISpecification<T>
{
    //Filtro
    Expression<Func<T, bool>> Criteria
    {
        get;
    }

    //Include de tablas relacionadas
    List<Expression<Func<T, object>>> Includes
    {
        get;
    }    
}
