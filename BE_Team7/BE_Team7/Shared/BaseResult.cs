using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace BE_Team7.Shared
{
    public abstract class BaseResult
    {
        private JToken rawJson;

        //
        // Summary:
        //     Gets or sets hTTP status code.
        public HttpStatusCode StatusCode { get; set; }

        //
        // Summary:
        //     Gets raw JSON as received from the server.   
        public JToken JsonObj
        {
            get
            {
                return rawJson;
            }
            internal set
            {
                rawJson = value;
                SetValues(value);
            }
        }

        //
        // Summary:
        //     Gets or sets description of server-side error (if one has occurred).
        [DataMember(Name = "error")]

        //
        // Summary:
        //     Gets or sets current limit of API requests until CloudinaryDotNet.Actions.BaseResult.Reset.
        public long Limit { get; set; }

        //
        // Summary:
        //     Gets or sets remaining amount of requests until CloudinaryDotNet.Actions.BaseResult.Reset.
        public long Remaining { get; set; }

        //
        // Summary:
        //     Gets or sets time of next reset of limits.
        public DateTime Reset { get; set; }

        //
        // Summary:
        //     Populates additional token fields.
        //
        // Parameters:
        //   source:
        //     JSON token received from the server.
        internal virtual void SetValues(JToken source)
        {
        }
    }
}