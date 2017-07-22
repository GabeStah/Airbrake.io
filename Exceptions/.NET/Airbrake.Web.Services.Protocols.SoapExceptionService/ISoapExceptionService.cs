using System.Runtime.Serialization;
using System.ServiceModel;

namespace Airbrake.Web.Services.Protocols.SoapExceptionService
{
    [DataContract]
    public class SoapExceptionFault
    {
        [DataMember]
        public bool Result { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

    [ServiceContract]
    public interface ILibraryService
    {
        [OperationContract]
        [FaultContract(typeof(SoapExceptionFault))]
        bool ReserveBook(string title, string author);
    }
}
