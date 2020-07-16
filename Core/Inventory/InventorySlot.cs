using System;
using JetBrains.Annotations;
using UnityEngine;

namespace SP.Core.Inventory
{
    [Serializable]
    public class InventorySlot
    {
        public InventorySlot(Vector2Int index, Inventory inventory)
        {
            _currentRole = SlotRole.Empty;
            _inventory = inventory;
            Index = index;
        }
        
        public InventorySlot(InventoryObject obj, Vector2Int index, Inventory inventory)
        {
            _currentRole = SlotRole.ItemOwner;
            _inventory = inventory;
            Index = index;
            Object = obj;
        }
        
        public InventorySlot(Vector2Int ownerIndex, Vector2Int index, Inventory inventory)
        {
            _currentRole = SlotRole.ItemSupport;
            _ownerIndex = ownerIndex;
            _inventory = inventory;
            Index = index;
        }
        
        public bool IsEmpty => _currentRole == SlotRole.Empty;
        public bool IsOwner => _currentRole == SlotRole.ItemOwner;
        public bool IsSupport => _currentRole == SlotRole.ItemSupport;

        public InventorySlot OwnerSlot => _inventory.Slots[_ownerIndex.x, _ownerIndex.y];
        public Vector2Int Index { get; }
        [CanBeNull] public InventoryObject Object { get; }

        private Inventory _inventory;
        private SlotRole _currentRole;
        private Vector2Int _ownerIndex;

        private enum SlotRole
        {
            Empty, ItemOwner, ItemSupport
        }
    }
}