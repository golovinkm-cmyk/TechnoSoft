using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Statistics
{
    public record StatusStatisticItem
    {
        public required string Status { get; set; }
        public required int Count { get; set; }
    }
}
