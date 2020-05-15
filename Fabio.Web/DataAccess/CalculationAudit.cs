using System;
using Fabio.Web.Domain;

namespace Fabio.Web.DataAccess
{
    public class CalculationAudit
    {
        public DateTime Timestamp { get; set; }

        public CalculationType CalculationType { get; set; }

        public CalculationInput Input { get; set; }

        public CalculationResult Result { get; set; }
    }
}