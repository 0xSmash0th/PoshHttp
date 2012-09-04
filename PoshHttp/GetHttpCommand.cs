using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Net;

namespace PoshHttp
{
    [Cmdlet(VerbsCommon.Get, "Http", DefaultParameterSetName = "Addresses")]
    public class GetHttpCommand : Cmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Addresses", Mandatory = true)]
        [Parameter(Position = 0, ParameterSetName = "Address", Mandatory = true)]
        [Parameter(Position = 0, ParameterSetName = "Href", Mandatory = true)]
        [Parameter(Position = 0, ParameterSetName = "Url", Mandatory = true)]
        public Uri[] Addresses { get; set; }

        protected override void ProcessRecord()
        {
            List<string> content = new List<string>();

            foreach (Uri address in Addresses)
            {
                try
                {
                    WebRequest request = HttpWebRequest.Create(address);
                    using (WebResponse response = request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        content.Add(streamReader.ReadToEnd());
                    }
                }
                catch (Exception exception)
                {
                    WriteError(new ErrorRecord(exception, "HttpError", ErrorCategory.ReadError, address));
                }
            }

            WriteObject(content, true);
        }
    }
}
