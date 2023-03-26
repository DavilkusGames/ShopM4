using System;
using Braintree;

namespace ShopM4_Utility.BrainTree
{
	public interface IBrainTreeBridge
	{
		IBraintreeGateway CreateGateWay();
        IBraintreeGateway GetGateWay();
    }
}

