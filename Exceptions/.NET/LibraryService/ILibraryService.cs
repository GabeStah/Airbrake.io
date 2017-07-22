using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace LibraryService
{
    [DataContract]
    public class InvalidBookFault
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
        [FaultContract(typeof(InvalidBookFault))]
        [OperationContract]
        [WebGet]
        bool ReserveBook(string title, string author);
    }
}
