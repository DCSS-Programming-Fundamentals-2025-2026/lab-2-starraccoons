using CafeRush.Domain;

namespace CafeRush.Contracts
{
    public interface IRecipe
    {
        bool CanMake(Stock stock);
        void Consume(Stock stock);
    }
}
