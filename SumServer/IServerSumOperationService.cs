using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SumServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServerSumOperationService" in both code and config file together.
    [ServiceContract]
    public interface IServerSumOperationService
    {
        [OperationContract]
        double SumNumbers(List<double> pNums);
    }
    
}
