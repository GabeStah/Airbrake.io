using System.Runtime.Serialization;
using System.ServiceModel;

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
        [OperationContract]
        [FaultContract(typeof(InvalidBookFault))]
        bool ReserveBook(string title, string author);
    }
}
