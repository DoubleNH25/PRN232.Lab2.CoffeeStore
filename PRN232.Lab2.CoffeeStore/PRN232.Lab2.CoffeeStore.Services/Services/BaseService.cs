using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;

namespace PRN232.Lab2.CoffeeStore.Services.Services
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Common methods that can be used across services can be added here
    }
}