using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaNew;

public class Function
{
    private IServiceCollection _serviceCollection;    
    private static ServiceProvider _serviceProvider;

    public int previousDeliveries;

    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {
        ConfigureServices();
    }

    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach (var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        try
        {
            throw new Exception();
        }
        catch (Exception)
        {
            using var _sqsService = _serviceProvider.GetRequiredService<ISqsService>();
            _sqsService.SetupClientProperties(message);
            
            var previousDeliveries = GetPreviousDeliveries(message.Attributes);

            if (previousDeliveries > Constants.MAX_RETRIES)
            {
                await _sqsService.DeleteMessage();
            }
            else
            {
                await _sqsService.ChangeMessageVisibility(previousDeliveries);
            }

            throw;
        }       
    }    

    private int GetPreviousDeliveries(Dictionary<string,string> messageAttributes)
    {
        var previousDeliveriesStr = messageAttributes.FirstOrDefault(a => a.Key.Equals(Constants.APPROXIMATE_RECEIVE_COUNT)).Value;
        _ = int.TryParse(previousDeliveriesStr, out int previousDeliveries);
        return previousDeliveries;
    }

    /// <summary>
    /// Configure dependency injection 
    /// </summary>
    private void ConfigureServices()
    {
        _serviceCollection = new ServiceCollection();

        var proxyURL = Environment.GetEnvironmentVariable(Constants.WEB_PROXY_URL);

        _ = _serviceCollection.AddTransient<ISqsService>(x =>
        {
            return new SqsService(proxyURL);
        });

        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }  
}