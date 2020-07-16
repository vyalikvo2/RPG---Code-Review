using SP.Utils.Attributes;
using UnityEngine;

namespace SP.Core.Inventory
{
    public class InventoryObjectSize
    {
        public Vector2Int Size;
        
        public InventoryObjectSize(Vector2Int size)
        {
            Size = size;
        }
    }
    
    public enum InventoryObjectName // string value as localization key
    {
        [StringValue("cigars")] Cigars, 
        [StringValue("lighter")] Lighter
    }
}