using UnityEngine;

namespace SP.Core.Inventory
{
    public class InventoryObject
    {
        public InventoryObjectName Name;
        public Vector2Int Size;
        
        public InventoryObject(InventoryObjectName name, Vector2Int size)
        {
            Name = name;
            Size = size;
        }
        
    }
}