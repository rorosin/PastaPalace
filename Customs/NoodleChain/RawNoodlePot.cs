using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using UnityEngine;

namespace PastaPalace.Customs.NoodleChain
{
    internal class RawNoodlePot : CustomItemGroup
    {
        public override string UniqueNameID => "RawNoodlePot";
        public override GameObject Prefab => ((Item)GDOUtils.GetExistingGDO(Mod.TomatoID)).Prefab;
        public override Item DisposesTo => (Item)GDOUtils.GetExistingGDO(Mod.PotID);
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;
    }
}
