using System.Collections.Generic;

namespace EventBus.Messages.Commands
{
    public class CheckStocksExistance
    {
        public List<int> StocksIds { get; set; }
    }
}