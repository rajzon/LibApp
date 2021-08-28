using System.Collections.Generic;
using StockDelivery.API.Commands.V1.Common;

namespace StockDelivery.API.Commands.V1
{
    public class ScanItemDeliveryCommandResult : BaseCommandResult
    {
        public bool ScanMode { get; init; }
        
        public ScanItemDeliveryCommandResult(bool succeeded, bool scanMode) 
            : base(succeeded)
        {
            ScanMode = scanMode;
        }

        public ScanItemDeliveryCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default) 
            : base(succeeded, errors)
        {
        }
    }
}