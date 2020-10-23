using k8s.Models;
using KubeOps.Operator.Entities;
using KubeOps.Operator.Entities.Annotations;

namespace KubeOps.TestOperator.Entities
{
    public class ProductInfoSpec
    {
        /// <summary>
        /// This is a test for the contextual fetching of descriptions.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        public int MinimumQuantity{ get; set; } 
        public string MaximumQuantity { get; set; } 
    }

    public class ProductInfoStatus
    {
        public string Status { get; set; } = string.Empty;
    }

    [KubernetesEntity(Group = "experiments.warehouse", ApiVersion = "v1", Kind = "productInfoentity", PluralName = "testentities")]
    public class ProductInfoEntity : CustomKubernetesEntity<ProductInfoSpec, ProductInfoStatus>
    {
    }
}
