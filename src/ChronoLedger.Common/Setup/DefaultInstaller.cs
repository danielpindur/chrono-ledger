using System.Collections.ObjectModel;
using System.Reflection;
using ChronoLedger.Common.Database;
using ChronoLedger.Common.Facades;
using ChronoLedger.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ChronoLedger.Common.Setup;

public abstract class DefaultInstaller : Collection<ServiceDescriptor>
{
    private readonly Type[] _currentAssemblyTypes;

    protected DefaultInstaller()
    {
        var currentAssembly = Assembly.GetCallingAssembly();
        _currentAssemblyTypes = currentAssembly.GetTypes();
        
        // TODO: Change to some sensible logging
        Console.WriteLine($"Initializing default installer in {currentAssembly}");
        
        InstallCommonComponents();
    }

    private void InstallCommonComponents()
    {
        AddDbConnectionFactory();
        AddRepositories();
        AddFacades();
    }

    private void AddFacades()
    {
        var facadeInterfaces = _currentAssemblyTypes
            .Where(t => t.IsInterface && typeof(IFacade).IsAssignableFrom(t))
            .ToList();

        foreach (var facadeInterface in facadeInterfaces)
        {
            var implementation = _currentAssemblyTypes.SingleOrDefault(t =>
                t is { IsClass: true, IsAbstract: false } &&
                facadeInterface.IsAssignableFrom(t));

            if (implementation is not null)
            {
                Console.WriteLine($"Found facade {implementation.FullName} for type {facadeInterface.FullName}");
                
                Add(ServiceDescriptor.Transient(facadeInterface, implementation));
            }
        }
    }
    
    private void AddRepositories()
    {
        var repositoryInterfaces = _currentAssemblyTypes
            .Where(t => t.IsInterface && typeof(IRepository).IsAssignableFrom(t))
            .ToList();

        foreach (var repositoryInterface in repositoryInterfaces)
        {
            var implementation = _currentAssemblyTypes.SingleOrDefault(t =>
                t is { IsClass: true, IsAbstract: false } &&
                repositoryInterface.IsAssignableFrom(t));

            if (implementation is not null)
            {
                Console.WriteLine($"Found facade {implementation.FullName} for type {repositoryInterface.FullName}");
                
                Add(ServiceDescriptor.Transient(repositoryInterface, implementation));
            }
        }
    }
    
    private void AddDbConnectionFactory()
    {
        var dbConnectionFactoryInterface = typeof(IDbConnectionFactory);
        var dbConnectionFactory = _currentAssemblyTypes.SingleOrDefault(t =>
            t is { IsClass: true, IsAbstract: false } &&
            dbConnectionFactoryInterface.IsAssignableFrom(t));

        if (dbConnectionFactory is null)
        {
            return;
        }
        
        Console.WriteLine($"Found {dbConnectionFactoryInterface.FullName} implementation {dbConnectionFactory.FullName}");
        Console.WriteLine("Installing DB context");
        
        Add(ServiceDescriptor.Singleton(dbConnectionFactoryInterface, dbConnectionFactory));
        Add(ServiceDescriptor.Scoped<IDatabaseContext, DatabaseContext>());
    }
}