using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace PastaPalace.Customs.WhiteSauceChain
{
    internal class CookedWhiteSauce : CustomItem
    {
        public override string UniqueNameID => "cookedWhiteSauce";
        public override GameObject Prefab => ((Item)GDOUtils.GetExistingGDO(Mod.TomatoID)).Prefab;
        public override Item DisposesTo => (Item)GDOUtils.GetExistingGDO(Mod.PotID);
        public override int SplitCount => 4;
        public override bool PreventExplicitSplit => true;
        public override Item SplitSubItem => (Item)GDOUtils.GetExistingGDO(Mod.ServedWhiteSauceID);
        public override List<Item> SplitDepletedItems => new List<Item>
        {
            (Item)GDOUtils.GetExistingGDO(Mod.PotID)
        };
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;
    }
}
