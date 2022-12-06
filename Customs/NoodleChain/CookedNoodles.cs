using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using UnityEngine;

namespace PastaPalace.Customs.NoodleChain
{
    internal class CookedNoodles : CustomItem
    {
        public override string UniqueNameID => "CookedNoodles";
        public override GameObject Prefab => ((Item)GDOUtils.GetExistingGDO(Mod.TomatoID)).Prefab;
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.StackableFood;
        public override void OnRegister(GameDataObject gameDataObject)
        {
            //Material[] materials = { MaterialUtils.GetExistingMaterial("Bread - Inside") };
            // MaterialUtils.ApplyMaterial(Prefab, "pasta.blend", materials);
        }
    }
}
