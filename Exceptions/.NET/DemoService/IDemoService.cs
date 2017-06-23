using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DemoService
{
    [ServiceContract]
    public interface IDemoService
    {
        [OperationContract]
        int Divide(int n1, int n2);
    }

    //TODO: Add data contract Fault Exception

}
