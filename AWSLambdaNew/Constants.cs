using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSLambdaNew
{
    public class Constants
    {
        public const string SQS_URL = "SqsUrl";
        public const string WEB_PROXY_URL = "WebProxyURL";
        public const int MAX_RETRIES = 2;
        public const string VISIBILITY_TIMEOUT = "VisibilityTimeout";
        public const string SENDER_ID = "SenderId";
        public const string APPROXIMATE_RECEIVE_COUNT = "ApproximateReceiveCount";
    }
}
