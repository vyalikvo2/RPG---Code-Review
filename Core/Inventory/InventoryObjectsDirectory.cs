using System.Collections.Generic;
using UnityEngine;

namespace SP.Core.Inventory
{
    public abstract class InventoryObjectsDirectory
    {
        public static readonly Dictionary<int, InventoryObject> Objects = new Dictionary<int, InventoryObject>()
        {
            { (int)InventoryObjectName.Cigars, new InventoryObject(InventoryObjectName.Cigars, new Vector2Int(1, 1))},
            { (int)InventoryObjectName.Lighter, new InventoryObject(InventoryObjectName.Lighter, new Vector2Int(1, 1))},
        };
    }
}