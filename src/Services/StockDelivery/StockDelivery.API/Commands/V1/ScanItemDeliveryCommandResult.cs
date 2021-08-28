using System.Collections.Generic;
using StockDelivery.API.Commands.V1.Common;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Commands.V1
{
    public class ScanItemDeliveryCommandResult : BaseCommandResult
    {
        public bool ScanMode { get; init; }
        public ActiveDeliveryScanInfoDto Result { get; init; }
        
        public ScanItemDeliveryCommandResult(bool succeeded, bool scanMode, ActiveDeliveryScanInfoDto result) 
            : base(succeeded)
        {
            ScanMode = scanMode;
            Result = result;
        }

        public ScanItemDeliveryCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default) 
            : base(succeeded, errors)
        {
        }
    }
}