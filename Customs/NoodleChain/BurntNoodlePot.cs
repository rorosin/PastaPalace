using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace PastaPalace.Customs.NoodleChain
{
    internal class BurntNoodlePot : CustomItem
    {
        public override string UniqueNameID => "BurntNoodlePot";
        public override GameObject Prefab => ((Item)GDOUtils.GetExistingGDO(Mod.TomatoID)).Prefab;
        public override Item DisposesTo => (Item)GDOUtils.GetExistingGDO(Mod.PotID);
        public override int SplitCount => 6;
        public override List<Item> SplitDepletedItems => new List<Item>
        {
            (Item)GDOUtils.GetExistingGDO(Mod.PotID)
        };
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;
    }
}
