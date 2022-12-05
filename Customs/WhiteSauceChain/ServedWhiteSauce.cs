using KitchenData;
using KitchenLib.Customs;
using UnityEngine;

namespace PastaPalace.Customs.WhiteSauceChain
{
    internal class ServedWhiteSauce : CustomItem
    {
        public override string UniqueNameID => "ServedWhiteSauce";
        public override GameObject Prefab => (GameObject)Mod.bundle.LoadAsset("sauce");
        public override ItemCategory ItemCategory => ItemCategory.Generic;
        public override ItemStorage ItemStorageFlags => ItemStorage.None;

        public override void OnRegister(GameDataObject gameDataObject)
        {
            // Material[] materials = { MaterialUtils.GetExistingMaterial("Bread - Inside") };
            // MaterialUtils.ApplyMaterial(Prefab, "sauce.blend", materials);
        }
    }
}
