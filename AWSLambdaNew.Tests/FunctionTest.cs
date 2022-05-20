using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.SQSEvents;

namespace AWSLambdaNew.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestSQSEventLambdaFunction()
    {
        var sqsEvent = new SQSEvent
        {
            Records = new List<SQSEvent.SQSMessage>
            {
                new SQSEvent.SQSMessage
                {
                    Body = "foobar",
                    AwsRegion= "us-region-1",
                    EventSourceArn="arn:any:sqs:us-region-1:123456789012:MyQueue",
                    MessageId="AnyId",
                    ReceiptHandle="ReceiptHandle",
                    Attributes=new Dictionary<string, string>()
                    {
                        {"SenderId", "SenderId" }
                    }
                }
            }
        };

        var logger = new TestLambdaLogger();
        var context = new TestLambdaContext
        {
            Logger = logger
        };

        var function = new Function();

        _ = await Assert.ThrowsAsync<Amazon.Runtime.AmazonServiceException>(async () =>
        {
            await function.FunctionHandler(sqsEvent, context);
        });        
    }
}