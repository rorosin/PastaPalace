﻿using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace PastaPalace.Customs.RedSauceChain
{
    internal class CookedRedSauce : CustomItem
    {
        public override string UniqueNameID => "CookedRedSauce";
        public override GameObject Prefab => throw new System.NotImplementedException();
        public override Item DisposesTo => (Item)GDOUtils.GetExistingGDO(Mod.PotID);
        public override int SplitCount => 6;
        public override bool PreventExplicitSplit => true;
        public override Item SplitSubItem => (Item)GDOUtils.GetExistingGDO(Mod.ServedRedSauceID);
        public override List<Item> SplitDepletedItems => new List<Item>
        {
            (Item)GDOUtils.GetExistingGDO(Mod.PotID)
        };
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;
    }
}