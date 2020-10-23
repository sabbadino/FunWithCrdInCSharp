using System;
using KubeOps.TestOperator.Entities;

namespace warehouseapi.Dtos
{
    public class GetProductCommand
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }

    }
    public class ProductAvailabilityReadDto : ProductAvailabilityDto
    {
        public DateTime? CreationTimeStamp { get; set; }
    }

    public class ProductAvailabilityDto : ProductAvailabilitySpec
    {
        public string Id { get; set; }

    }

    
  
}
