using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace PastaPalace.Customs.PastaProcess
{
    internal class PlatedPasta : CustomItemGroup
    {
        public override string UniqueNameID => "Plated Pasta";
        public override GameObject Prefab => throw new System.NotImplementedException();
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;
        public override List<ItemGroup.ItemSet> Sets => new List<ItemGroup.ItemSet>
        {
            new ItemGroup.ItemSet
            {
                Max = 1,
                Min = 1,
                Items = new List<Item>
                {
                    (Item)GDOUtils.GetExistingGDO(Mod.CookedNoodlesID);
                }
            },
            new ItemGroup.ItemSet
            {
                Max = 1,
                Min = 1,
                RequiresUnlock = true,
                Items = new List<Item>
                {
                    (Item)GDOUtils.GetExistingGDO(Mod.ServedRedSauceID),
                    (Item)GDOUtils.GetExistingGDO(Mod.ServedWhiteSauceID)
                }
            },
            new ItemGroup.ItemSet
            {
                Max = 1,
                Min = 0,
                Items = new List<Item>
                {
                    (Item)GDOUtils.GetExistingGDO(Mod.BroccoliChoppedContainerCookedID),
                    (Item)GDOUtils.GetExistingGDO(Mod.MeatChoppedContainerCookedID)
                }
            },
            new ItemGroup.ItemSet
            {
                Max = 1,
                Min = 1,
                Items = new List<Item>
                {
                    (Item)GDOUtils.GetExistingGDO(Mod.PlateID)
                }
            }
        };
    }
}
