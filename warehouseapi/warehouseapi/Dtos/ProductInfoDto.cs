using System;
using KubeOps.WareHouse.Entities;

namespace warehouseapi.Dtos
{
    public class ProductInfoReadDto : ProductInfoDto
    {
        public DateTime? CreationTimeStamp { get; set; }
    }

    public class ProductInfoDto : ProductInfoSpec
    {
        public string Id { get; set; }

    }


}
