using System;
using System.Collections.Generic;
using k8s.Models;
using KubeOps.Operator.Entities;
using KubeOps.Operator.Entities.Annotations;

namespace KubeOps.TestOperator.Entities
{
    public class ProductAvailabilitySpec
    {
        /// <summary>
        /// This is a test for the contextual fetching of descriptions.
        /// </summary>
       
        public int AvailableQuantity{ get; set; }
        public DateTime? LastUpdateTimeStamp{ get; set; } 

    }

    public class ProductIAvailabilityStatus
    {
        public string Status { get; set; } = string.Empty;
    }

    [KubernetesEntity(Group = CrdGroup, ApiVersion = CrdApiVersion, Kind = CrdKind, PluralName = CrdPluralName)]
    public class ProductAvailabilityEntity : CustomKubernetesEntity<ProductAvailabilitySpec, ProductIAvailabilityStatus>
    {
        public ProductAvailabilityEntity()
        {
            this.Metadata.Annotations = new Dictionary<string, string>();
        }
        public const string CrdGroup = "experiments.warehouse";
        public const string CrdApiVersion = "v1";
        public const string CrdKind = "productavailabilityentity";
        public const string CrdPluralName = "productavailabilityentities";



    }



}
