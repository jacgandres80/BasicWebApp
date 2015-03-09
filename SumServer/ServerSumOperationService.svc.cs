using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SumServer
{
    public class ServerSumOperationService : IServerSumOperationService
    {
        public double SumNumbers(List<double> pNums)
        {
            double result = 0d; 
            result = pNums.Sum();
            return result;
        }
    }
}
