using System;
using Braintree;
using Microsoft.Extensions.Options;

namespace ShopM4_Utility.BrainTree
{
    public class BrainTreeBridge : IBrainTreeBridge
    {
        public SettingsBrainTree SettingsBrainTree { get; set; }

        private IBraintreeGateway gateway;

        public BrainTreeBridge(IOptions<SettingsBrainTree> options)
        {
            SettingsBrainTree = options.Value;
        }

        public IBraintreeGateway CreateGateWay()
        {
            return new BraintreeGateway(SettingsBrainTree.Environment,
                SettingsBrainTree.MerchantId, SettingsBrainTree.PublicKey, SettingsBrainTree.PrivateKey);
        }

        public IBraintreeGateway GetGateWay()
        {
            if (gateway == null)
            {
                gateway = CreateGateWay();
            }
            return gateway;
        }
    }
}

