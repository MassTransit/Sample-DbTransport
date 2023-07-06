namespace Sample.Components;

using MassTransit;


public class DbEndpointAddressProvider :
    IEndpointAddressProvider
{
    readonly IEndpointNameFormatter _formatter;

    public DbEndpointAddressProvider(IEndpointNameFormatter formatter)
    {
        _formatter = formatter;
    }

    public Uri GetExecuteEndpoint<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
        where TArguments : class
    {
        return new Uri($"queue:{_formatter.ExecuteActivity<T, TArguments>()}");
    }
}