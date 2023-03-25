using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopM4_Utility.BrainTree
{
    public interface IBrainTreeBridge
    {
        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateWay();
    }
}
