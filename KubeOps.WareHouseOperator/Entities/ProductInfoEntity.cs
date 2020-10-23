using k8s.Models;
using KubeOps.Operator.Entities;

namespace KubeOps.WareHouse.Entities
{
    public class ProductInfoSpec
    {
        /// <summary>
        /// This is a test for the contextual fetching of descriptions.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        public int MinimumQuantity{ get; set; } 
        public int MaximumQuantity { get; set; } 
    }

    public class ProductInfoStatus
    {
        public string Status { get; set; } = string.Empty;
    }

    [KubernetesEntity(Group = CrdGroup, ApiVersion = CrdApiVersion, Kind = CrdKind, PluralName = CrdPluralName)]
    public class ProductInfoEntity : CustomKubernetesEntity<ProductInfoSpec, ProductInfoStatus>
    {
        public const string CrdGroup = "experiments.warehouse";
        public const string CrdApiVersion = "v1";
        public const string CrdKind = "productinfoentity";
        public const string CrdPluralName = "productinfoentities";



    }



}
