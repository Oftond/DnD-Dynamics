using System;
using System.Collections.Generic;
using System.Linq;

namespace DnD_Dynamics.Models
{
    [Serializable]
    public class SimpleItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public int Weight { get; set; } = 0;
    }

    [Serializable]
    public class Inventory
    {
        public List<SimpleItem> Items { get; set; } = new List<SimpleItem>();
        public int Gold { get; set; } = 0;
        public int Silver { get; set; } = 0;
        public int Copper { get; set; } = 0;

        [NonSerialized]
        private CharacterData _character;

        public Inventory() { }

        public Inventory(CharacterData character)
        {
            _character = character;
        }

        public void SetCharacter(CharacterData character)
        {
            _character = character;
        }

        public void AddItem(SimpleItem item)
        {
            var existingItem = Items.Find(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void AddItem(string name, string description, int quantity = 1, int weight = 0)
        {
            var item = new SimpleItem
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                Quantity = quantity,
                Weight = weight
            };
            AddItem(item);
        }

        public void RemoveItem(string itemId, int quantity = 1)
        {
            var item = Items.Find(i => i.Id == itemId);
            if (item != null)
            {
                if (item.Quantity > quantity)
                {
                    item.Quantity -= quantity;
                }
                else
                {
                    Items.Remove(item);
                }
            }
        }

        public SimpleItem GetItem(string itemId)
        {
            return Items.Find(i => i.Id == itemId);
        }

        public List<SimpleItem> GetItemsByName(string name)
        {
            return Items.FindAll(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public void AddGold(int amount)
        {
            Gold += amount;
        }

        public void AddSilver(int amount)
        {
            Silver += amount;
        }

        public void AddCopper(int amount)
        {
            Copper += amount;
        }

        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }

        public bool SpendSilver(int amount)
        {
            if (Silver >= amount)
            {
                Silver -= amount;
                return true;
            }
            return false;
        }

        public bool SpendCopper(int amount)
        {
            if (Copper >= amount)
            {
                Copper -= amount;
                return true;
            }
            return false;
        }

        public int GetTotalGoldValue()
        {
            return Gold + (Silver / 10) + (Copper / 100);
        }

        public int TotalWeight => CalculateTotalWeight();
        public int CarryingCapacity => CalculateCarryingCapacity();

        private int CalculateTotalWeight()
        {
            int totalWeight = 0;
            foreach (var item in Items)
            {
                totalWeight += item.Weight * item.Quantity;
            }
            return totalWeight;
        }

        private int CalculateCarryingCapacity() => (_character?.BaseStats.Strength ?? 10) * 15;

        public bool IsOverEncumbered() => TotalWeight > CarryingCapacity;
        public bool CanCarryMore() => TotalWeight < CarryingCapacity;
        public int GetAvailableCapacity() => Math.Max(0, CarryingCapacity - TotalWeight);
        public float GetWeightPercentage() => (float)TotalWeight / CarryingCapacity;

        public SerializableInventory ToSerializable()
        {
            return new SerializableInventory
            {
                Items = Items.Select(i => new SerializableInventoryItem
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    Weight = i.Weight
                }).ToList(),
                Gold = Gold,
                Silver = Silver,
                Copper = Copper
            };
        }

        public static Inventory FromSerializable(SerializableInventory serializable, CharacterData character)
        {
            var inventory = new Inventory(character);
            inventory.Gold = serializable.Gold;
            inventory.Silver = serializable.Silver;
            inventory.Copper = serializable.Copper;

            foreach (var serializableItem in serializable.Items)
            {
                inventory.AddItem(new SimpleItem
                {
                    Id = serializableItem.Id,
                    Name = serializableItem.Name,
                    Description = serializableItem.Description,
                    Quantity = serializableItem.Quantity,
                    Weight = serializableItem.Weight
                });
            }

            return inventory;
        }
    }

    [Serializable]
    public class SerializableInventory
    {
        public List<SerializableInventoryItem> Items { get; set; } = new List<SerializableInventoryItem>();
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Copper { get; set; }
    }

    [Serializable]
    public class SerializableInventoryItem
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public int Weight { get; set; } = 0;
    }
}