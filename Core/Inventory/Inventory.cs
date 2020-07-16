using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace SP.Core.Inventory
{
    public class Inventory
    {
        public Inventory(Vector2Int inventorySize)
        {
            Slots = new InventorySlot[inventorySize.x, inventorySize.y];
        }

        public InventorySlot[,] Slots { get; }
        
        public class PlaceObjectResult
        {
            public bool Success;
            [CanBeNull] public InventoryObject ConflictedObject;

            public PlaceObjectResult(bool success, InventoryObject conflictedObject)
            {
                Success = success;
                ConflictedObject = conflictedObject;
            }
            
            public PlaceObjectResult()
            {
                Success = false;
                ConflictedObject = null;
            }
        }
        
        public PlaceObjectResult TryPlaceObject(Vector2Int index, InventoryObject obj)
        {
            var objSize = obj.Size;
            
            if (!CanBePlaced(index, objSize)) return new PlaceObjectResult();
            
            var conflictedSlots = GetNotEmptySlots(index, objSize);
            var conflictedSlotsCount = conflictedSlots.Count;

            if (conflictedSlotsCount > 1) return new PlaceObjectResult();

            var conflictedSlot = conflictedSlots.FirstOrDefault();
            if (conflictedSlot != null && conflictedSlotsCount == 1)
                RemoveConflictObject(conflictedSlot);

            PlaceObjectToSlot(index, obj);

            return new PlaceObjectResult(true, conflictedSlot?.Object);
        }

        private bool CanBePlaced(Vector2Int index, Vector2Int objSize)
        {
            return Slots.GetLength(0) - index.x >= objSize.x && Slots.Rank - index.x >= objSize.x;
        }
        
        private List<InventorySlot> GetNotEmptySlots(Vector2Int index, Vector2Int objSize)
        {
            var list = new List<InventorySlot>();
            for (var l = index.x; l < index.x + objSize.x; l++)
            for (var r = index.y; r < index.y + objSize.y; r++)
            {
                if (Slots[r, l].IsOwner)
                    list.Add(Slots[r, l]);
                else if (Slots[r, l].IsSupport && !list.Exists(slot => slot.Index == Slots[r, l].OwnerSlot.Index))
                    list.Add(Slots[r, l].OwnerSlot);
            }

            return list;
        }

        private void RemoveConflictObject(InventorySlot ownerSlot)
        {
            var index = ownerSlot.Index;
            
            if (ownerSlot.Object == null)
            {
                Debug.LogError($"[Inventory] Try remove conflicted object, but slot don't have item. Index: {index}");
                return;
            }
            
            var objSize = ownerSlot.Object.Size;
            
            Slots[index.x, index.y] = new InventorySlot(index, this);
            
            if (objSize.x == 1 && objSize.y == 1) return;
            
            for (var l = index.x + 1; l < index.x + objSize.x; l++)
            for (var r = index.y + 1; r < index.y + objSize.y; r++)
                Slots[r, l] = new InventorySlot(new Vector2Int(l,r), this);
        }

        private void PlaceObjectToSlot(Vector2Int index, InventoryObject obj)
        {
            Slots[index.x, index.y] = new InventorySlot(obj, index, this);

            var objSize = obj.Size;
            if (objSize.x == 1 && objSize.y == 1) return;
            
            for (var l = index.x + 1; l < index.x + objSize.x; l++)
            for (var r = index.y + 1; r < index.y + objSize.y; r++)
                Slots[r, l] = new InventorySlot(index, new Vector2Int(l,r), this);
        }
    }
}
