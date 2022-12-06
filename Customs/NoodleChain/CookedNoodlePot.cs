using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace PastaPalace.Customs.NoodleChain
{
    internal class CookedNoodlePot : CustomItem
    {
        public override string UniqueNameID => "CookedNoodlePot";
        public override GameObject Prefab => ((Item)GDOUtils.GetExistingGDO(Mod.TomatoID)).Prefab;
        public override Item DisposesTo => (Item)GDOUtils.GetExistingGDO(Mod.PotID);
        public override int SplitCount => 6;
        //public override Item SplitSubItem => (Item)GDOUtils.GetExistingGDO(Mod.GetHash(Mod.MOD_NAME, "CookedNoodles"));
        public override List<Item> SplitDepletedItems => new List<Item>
        {
            (Item)GDOUtils.GetExistingGDO(Mod.PotID)
        };
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;
        /*
        public override List<Item.ItemProcess> Processes => new List<Item.ItemProcess>
        {
            new Item.ItemProcess
            {
                Duration = 30,
                Process = (Process)GDOUtils.GetExistingGDO(Mod.CookID),
                Result = (Item)GDOUtils.GetExistingGDO(Mod.GetHash(Mod.MOD_NAME, "BurntNoodlePot"))
            }
        };
        */
    }
}
