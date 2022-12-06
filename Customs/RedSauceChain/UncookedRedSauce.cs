using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace PastaPalace.Customs.RedSauceChain
{
    internal class UncookedRedSauce : CustomItemGroup
    {
        public override string UniqueNameID => "RawRedSauce";
        public override GameObject Prefab => ((Item)GDOUtils.GetExistingGDO(Mod.TomatoID)).Prefab;
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
                    (Item)GDOUtils.GetExistingGDO(Mod.TomatoSauceID)
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
                Duration = 6,
                Process = (Process)GDOUtils.GetExistingGDO(Mod.CookID),
                Result = (Item)GDOUtils.GetExistingGDO(Mod.CookedRedSauceID)
            }
        };
    }
}
