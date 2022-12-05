using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace PastaPalace.Customs.WhiteSauceChain
{
    public class UncookedWhiteSauce : CustomItemGroup
    {
        public override string UniqueNameID => "RawWhiteSauce";
        public override GameObject Prefab => throw new System.NotImplementedException();
        public override Item DisposesTo => (Item)GDOUtils.GetExistingGDO(Mod.PotID);
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;
        public override List<ItemGroup.ItemSet> Sets => new List<ItemGroup.ItemSet>
        {
            new ItemGroup.ItemSet
            {
                Max = 2,
                Min = 2,
                Items = new List<Item>
                {
                    (Item)GDOUtils.GetExistingGDO(Mod.OnionID),
                    (Item)GDOUtils.GetExistingGDO(Mod.CheeseID)
                }
            },
            new ItemGroup.ItemSet
            {
                Max = 1,
                Min = 1,
                Items = new List<Item>
                {
                    (Item)GDOUtils.GetExistingGDO(Mod.PotID)
                }
            }
        };
        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>
        {
            new Item.ItemProcess
            {
                Duration = 8,
                Process = (Process)GDOUtils.GetExistingGDO(Mod.CookID),
                Result = (Item)GDOUtils.GetExistingGDO(Mod.CookedWhiteSauceID)
            }
        };
    }
}
