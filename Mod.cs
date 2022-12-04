using BepInEx;
using KitchenData;
using KitchenLib;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace PastaPalace
{
    [BepInProcess("PlateUp.exe")]
    [BepInPlugin("pastapalace", "Pasta Palace", "0.0.1")]
    public class Mod : BaseMod
    {
        public Mod() : base(">=1.0.0 <=1.0.5", Assembly.GetCallingAssembly()) { }
        public static AssetBundle bundle;

        public static int RawNoodlesID;
        public static int RawNoodlePotID;
        public static int CookedNoodlePotID;
        public static int BurntNoodlePotID;
        public static int CookedNoodlesID;
        public static int UncookedRedSauceID;
        public static int CookedRedSauceID;
        public static int ServedRedSauceID;
        public static int UncookedWhiteSauceID;
        public static int CookedWhiteSauceID;
        public static int ServedWhiteSauceID;
        public static int PlatedPastaID;
        public static int PastaBaseID;
        public static int PastaWhiteID;
        void Start()
        {
            bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "pastapalacetextures"));
            UnityEngine.Debug.Log("Loaded Textures.");

            RawNoodlesID = AddGameDataObject<RawNoodles>().ID;
            RawNoodlePotID = AddGameDataObject<RawNoodlePot>().ID;
            CookedNoodlePotID = AddGameDataObject<CookedNoodlePot>().ID;
            BurntNoodlePotID = AddGameDataObject<BurntNoodlePot>().ID;
            CookedNoodlesID = AddGameDataObject<CookedNoodles>().ID;
            UnityEngine.Debug.Log("Loaded Noodles.");

            UncookedRedSauceID = AddGameDataObject<UncookedRedSauce>().ID;
            CookedRedSauceID = AddGameDataObject<CookedRedSauce>().ID;
            ServedRedSauceID = AddGameDataObject<ServedRedSauce>().ID;
            UnityEngine.Debug.Log("Loaded Red Sauce.");

            UncookedWhiteSauceID = AddGameDataObject<UncookedWhiteSauce>().ID;
            CookedWhiteSauceID = AddGameDataObject<CookedWhiteSauce>().ID;
            ServedWhiteSauceID = AddGameDataObject<ServedWhiteSauce>().ID;
            UnityEngine.Debug.Log("Loaded White Sauce.");

            PlatedPastaID = AddGameDataObject<PlatedPasta>().ID;
            PastaBaseID = AddGameDataObject<PastaBase>().ID;
            PastaWhiteID = AddGameDataObject<PastaWhite>().ID;
            UnityEngine.Debug.Log("Loaded Dish.");
        }
    }

    /*
     *  James' Pasta Palace introduces the delicious flavors of Italy to PlateUp!
     *      Pasta is a special challenge, impossible to fully automate and requiring the cook 
     *  to carefully balance multiple pots of simmering noodles, tasty toppings and 
     *  bubbling sauce to please ever-choosier customers.
     *  
     *  The Pasta Main Dish currently comes with:
     *  Two Sauces (Red and White)
     *  Two Toppings (Broccoli and Meat) (WIP)
     *  
     *  To Come:
     *  More Sauces and Toppings!
     */

    /* 
     * Noodle Chain:
     * Mix flour with cracked egg. 
     * Add water and raw noodle to pot.
     * Cook and portion onto plate. Serves 6.
    */
    public class RawNoodles : CustomItemGroup
    {
        public ItemGroup.ItemSet ingredients;
        public override string UniqueNameID { get { return "rawNoodles"; } }

        public override GameObject Prefab => (GameObject)Mod.bundle.LoadAsset("pasta");

        Material[] materials = { MaterialUtils.GetExistingMaterial("Bread - Inside") };
        public override void OnRegister(GameDataObject gameDataObject)
        {
            // MaterialUtils.ApplyMaterial(Prefab, "pasta.blend", materials);
            ingredients.Max = 2;
            ingredients.Min = 2;
            ingredients.IsMandatory = true;
            ingredients.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Flour)); // Flour
            ingredients.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.EggCracked)); // Cracked Egg
            DerivedSets.Add(ingredients);

        }
    }

    public class RawNoodlePot : CustomItemGroup
    {
        public ItemGroup.ItemSet ingredients;

        public ItemGroup.ItemSet vessel;

        public Item.ItemProcess cookNoodle;

        public override string UniqueNameID { get { return "rawNoodlePot"; } }

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            ingredients.Max = 2;
            ingredients.Min = 2;
            ingredients.IsMandatory = true;
            ingredients.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Water));
            ingredients.Items.Add((Item)GDOUtils.GetExistingGDO(Mod.RawNoodlesID));
            DerivedSets.Add(ingredients);

            vessel.Max = 1;
            vessel.Min = 1;
            vessel.IsMandatory = true;
            vessel.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot));
            DerivedSets.Add(vessel);

            cookNoodle.Duration = 10;
            cookNoodle.Process = (Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Cook);
            cookNoodle.Result = (Item)GDOUtils.GetExistingGDO(Mod.CookedNoodlePotID);
            DerivedProcesses.Add(cookNoodle);
        }
    }

    public class CookedNoodlePot : CustomItem
    {
        public Item.ItemProcess burnNoodle;
        public override string UniqueNameID { get { return "CookedNoodlePot"; } }

        public override GameObject Prefab => ((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot)).Prefab;

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public int splitCount = 6;

        public override Item SplitSubItem { get { return (Item)GDOUtils.GetExistingGDO(Mod.CookedNoodlesID); } }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            SplitDepletedItems.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot));
            burnNoodle.Duration = 30;
            burnNoodle.Process = (Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Cook);
            burnNoodle.Result = (Item)GDOUtils.GetExistingGDO(Mod.BurntNoodlePotID);
            DerivedProcesses.Add(burnNoodle);
        }
    }

    public class BurntNoodlePot : CustomItem
    {
        public override string UniqueNameID { get { return "BurntNoodlePot"; } }

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public int splitCount = 6;

        public override void OnRegister(GameDataObject gameDataObject)
        {
        }
    }
    public class CookedNoodles : CustomItem
    {
        public override string UniqueNameID { get { return "cookedNoodles"; } }

        public override GameObject Prefab => (GameObject)Mod.bundle.LoadAsset("pasta");

        Material[] materials = { MaterialUtils.GetExistingMaterial("Bread - Inside") };

        public override void OnRegister(GameDataObject gameDataObject)
        {
           // MaterialUtils.ApplyMaterial(Prefab, "pasta.blend", materials);
        }
    }

    /*
     * Red Sauce Chain
     * Add Onion, Tomato Sauce to pot and cook. 
     * Pour onto cooked noodles. Serves 4.
     */

    public class UncookedRedSauce : CustomItemGroup
    {
        public ItemGroup.ItemSet ingredients;

        public ItemGroup.ItemSet vessel;

        public Item.ItemProcess cookSauce;

        public override string UniqueNameID { get { return "rawRedSauce"; } }

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            ingredients.Max = 2;
            ingredients.Min = 2;
            ingredients.IsMandatory = true;
            ingredients.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Onion));
            ingredients.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.TomatoSauce));
            DerivedSets.Add(ingredients);

            vessel.Max = 1;
            vessel.Min = 1;
            vessel.IsMandatory = true;
            vessel.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot));
            DerivedSets.Add(vessel);

            cookSauce.Duration = 6;
            cookSauce.Process = (Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Cook);
            cookSauce.Result = (Item)GDOUtils.GetExistingGDO(Mod.CookedRedSauceID);
            DerivedProcesses.Add(cookSauce);
        }
    }

    public class CookedRedSauce : CustomItem
    {
        public override string UniqueNameID { get { return "cooedRedSauce"; } }

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public int splitCount = 4;

        public bool preventExplicitSplit = true;

        public override Item SplitSubItem { get { return (Item)GDOUtils.GetExistingGDO(Mod.ServedRedSauceID); } }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            SplitDepletedItems.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot));
        }
    }

    public class ServedRedSauce : CustomItem
    {
        public override string UniqueNameID { get { return "ServedRedSauce"; } }

        public override GameObject Prefab => (GameObject)Mod.bundle.LoadAsset("sauce");

        Material[] materials = { MaterialUtils.GetExistingMaterial("Tomato Flesh") };

        public override void OnRegister(GameDataObject gameDataObject)
        {
           // MaterialUtils.ApplyMaterial(Prefab, "sauce.blend", materials);
        }
    }

    /*
     * White Sauce Chain
     * Add Onion, Cheese to pot and cook. 
     * Pour onto cooked noodles. Serves 4.
     */

    public class UncookedWhiteSauce : CustomItemGroup
    {
        public ItemGroup.ItemSet ingredients;

        public ItemGroup.ItemSet vessel;

        public Item.ItemProcess cookSauce;

        public override string UniqueNameID { get { return "rawWhiteSauce"; } }

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            ingredients.Max = 2;
            ingredients.Min = 2;
            ingredients.IsMandatory = true;
            ingredients.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Onion));
            ingredients.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Cheese));
            DerivedSets.Add(ingredients);

            vessel.Max = 1;
            vessel.Min = 1;
            vessel.IsMandatory = true;
            vessel.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot));
            DerivedSets.Add(vessel);

            cookSauce.Duration = 8;
            cookSauce.Process = (Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Cook);
            cookSauce.Result = (Item)GDOUtils.GetExistingGDO(Mod.CookedWhiteSauceID);
            DerivedProcesses.Add(cookSauce);
        }
    }

    public class CookedWhiteSauce : CustomItem
    {
        public override string UniqueNameID { get { return "cookedWhiteSauce"; } }

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public int splitCount = 4;

        public bool preventExplicitSplit = true;

        public override Item SplitSubItem { get { return (Item)GDOUtils.GetExistingGDO(Mod.ServedWhiteSauceID); } }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            SplitDepletedItems.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot));
        }
    }

    public class ServedWhiteSauce : CustomItem
    {
        public override string UniqueNameID { get { return "ServedWhiteSauce"; } }
        public override GameObject Prefab => (GameObject)Mod.bundle.LoadAsset("sauce");

        Material[] materials = { MaterialUtils.GetExistingMaterial("Bread - Inside") };

        public override void OnRegister(GameDataObject gameDataObject)
        {
           // MaterialUtils.ApplyMaterial(Prefab, "sauce.blend", materials);
        }
    }

    /*
     * Pasta Process:
     * Add Cooked Noodles
     * Add Red or White Sauce
     * (Optional) Add topping
     * Serve
     */
    public class PlatedPasta : CustomItemGroup
    {
        public ItemGroup.ItemSet noodles;

        public ItemGroup.ItemSet sauce;

        public ItemGroup.ItemSet toppings;

        public ItemGroup.ItemSet plate;

        public override string UniqueNameID { get { return "Plated Pasta"; } }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            noodles.Max = 1;
            noodles.Min = 1;
            noodles.IsMandatory = true;
            noodles.Items.Add((Item)GDOUtils.GetExistingGDO(Mod.CookedNoodlesID));
            DerivedSets.Add(noodles);

            sauce.Max = 1;
            sauce.Min = 1;
            sauce.IsMandatory = true;
            sauce.Items.Add((Item)GDOUtils.GetExistingGDO(Mod.ServedRedSauceID));
            sauce.Items.Add((Item)GDOUtils.GetExistingGDO(Mod.ServedWhiteSauceID));
            sauce.RequiresUnlock = true;
            DerivedSets.Add(sauce);

            /*  toppings.Max = 1;
              toppings.Min = 0;
              toppings.IsMandatory = true;
              toppings.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.BroccoliChoppedContainerCooked));
              toppings.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.MeatChoppedContainerCooked));
              DerivedSets.Add(toppings); */

            plate.Max = 1;
            plate.Min = 1;
            plate.IsMandatory = true;
            plate.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Plate));
            DerivedSets.Add(plate);
        }
    }
    public class PastaBase : CustomDish
    {
        public override string UniqueNameID { get { return "Pasta -- Base"; } }

        new DishType Type = DishType.Base;

        public Dish.MenuItem pasta;

        public Dish.IngredientUnlock redSauce;

        public override DishCustomerChange CustomerMultiplier { get { return DishCustomerChange.SmallDecrease; } }

        public override CardType CardType { get { return CardType.Default; } }

        public override Unlock.RewardLevel ExpReward { get { return Unlock.RewardLevel.Small; } }

        public override UnlockGroup UnlockGroup { get { return UnlockGroup.Dish; } }

        public override void OnRegister(GameDataObject gameDataObject)
        {

            pasta.Item = (Item)GDOUtils.GetExistingGDO(Mod.PlatedPastaID);
            pasta.Phase = MenuPhase.Main;
            pasta.Weight = 1;
            UnlocksMenuItems.Add(pasta);

            redSauce.Ingredient = (Item)GDOUtils.GetExistingGDO(Mod.ServedRedSauceID);
            redSauce.MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(Mod.PlatedPastaID);
            UnlocksIngredients.Add(redSauce);

            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Flour));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Egg));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Tomato));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Onion));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Water));

            RequiredProcesses.Add((Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Chop));
            RequiredProcesses.Add((Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Cook));

        }
    }

    public class PastaWhite : CustomDish
    {
        public override string UniqueNameID { get { return "Pasta -- White Sauce"; } }

        new DishType Type = DishType.Extra;

        public Dish.MenuItem pasta;

        public Dish.IngredientUnlock whiteSauce;

        public override DishCustomerChange CustomerMultiplier { get { return DishCustomerChange.SmallDecrease; } }

        public override CardType CardType { get { return CardType.Default; } }

        public override Unlock.RewardLevel ExpReward { get { return Unlock.RewardLevel.Medium; } }

        public override UnlockGroup UnlockGroup { get { return UnlockGroup.Dish; } }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            pasta.Item = (Item)GDOUtils.GetExistingGDO(Mod.PlatedPastaID);
            pasta.Phase = MenuPhase.Main;
            pasta.Weight = 1;
            UnlocksMenuItems.Add(pasta);

            whiteSauce.Ingredient = (Item)GDOUtils.GetExistingGDO(Mod.ServedWhiteSauceID);
            whiteSauce.MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(Mod.PlatedPastaID);
            UnlocksIngredients.Add(whiteSauce);

            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Flour));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Egg));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Cheese));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Onion));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Water));

            RequiredProcesses.Add((Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Chop));
            RequiredProcesses.Add((Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Cook));

            PrerequisiteDishes.Add((Dish)GDOUtils.GetExistingGDO(Mod.PastaBaseID));

            Requires.Add((Unlock)GDOUtils.GetExistingGDO(Mod.PastaBaseID));
        }
    }
}
