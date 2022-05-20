using System;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS.Model;

namespace AWSLambdaNew
{
    public interface ISqsService: IDisposable
    {
        public List<string> SqsAttributesToRetrieve { get; set; }
        public Task<DeleteMessageResponse> DeleteMessage();
        public Task<ChangeMessageVisibilityResponse> ChangeMessageVisibility(int previousDeliveries, int visibilityTimeoutIncrement = 5, int newVisibilityTimeout = 0);
        public void SetupClientProperties(SQSEvent.SQSMessage message);
    }
}
