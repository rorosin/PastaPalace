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
    [BepInPlugin("examplemod", "Example Mod", "1.0.0")]
    public class Mod : BaseMod
    {
        public Mod() : base(">=1.0.0 <=1.0.5", Assembly.GetCallingAssembly()) { }
        public static AssetBundle bundle;

        void OnStart()
        {
            bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "hexblue"));

            AddGameDataObject<RawNoodles>();
            AddGameDataObject<RawNoodlePot>();
            AddGameDataObject<CookedNoodlePot>();
            AddGameDataObject<BurntNoodlePot>();
            AddGameDataObject<CookedNoodles>();
            AddGameDataObject<UncookedRedSauce>();
            AddGameDataObject<CookedRedSauce>();
            AddGameDataObject<ServedRedSauce>();
            AddGameDataObject<UncookedWhiteSauce>();
            AddGameDataObject<CookedWhiteSauce>();
            AddGameDataObject<ServedWhiteSauce>();
            AddGameDataObject<PlatedPasta>();
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
            MaterialUtils.ApplyMaterial(Prefab, "", materials);
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
            ingredients.Items.Add((Item)GDOUtils.GetExistingGDO(new RawNoodles().ID));
            DerivedSets.Add(ingredients);

            vessel.Max = 1;
            vessel.Min = 1;
            vessel.IsMandatory = true;
            vessel.Items.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot));
            DerivedSets.Add(vessel);

            cookNoodle.Duration = 10;
            cookNoodle.Process = (Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Cook);
            cookNoodle.Result = (Item)GDOUtils.GetExistingGDO(new CookedNoodlePot().ID);
            DerivedProcesses.Add(cookNoodle);
        }
    }

    public class CookedNoodlePot : CustomItem
    {
        public Item.ItemProcess burnNoodle;
        public override string UniqueNameID { get { return "rawNoodles"; } }

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public int splitCount = 6;

        public override Item SplitSubItem { get { return (Item)GDOUtils.GetExistingGDO(new CookedNoodles().ID); } }

        public override void OnRegister(GameDataObject gameDataObject)
        {
            SplitDepletedItems.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot));
            burnNoodle.Duration = 30;
            burnNoodle.Process = (Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Cook);
            burnNoodle.Result = (Item)GDOUtils.GetExistingGDO(new BurntNoodlePot().ID);
            DerivedProcesses.Add(burnNoodle);
        }
    }

    public class BurntNoodlePot : CustomItem
    {
        public override string UniqueNameID { get { return "rawNoodles"; } }

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
            MaterialUtils.ApplyMaterial(Prefab, "", materials);
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
            cookSauce.Result = (Item)GDOUtils.GetExistingGDO(new CookedRedSauce().ID);
            DerivedProcesses.Add(cookSauce);
        }
    }

    public class CookedRedSauce : CustomItem
    {
        public override string UniqueNameID { get { return "cooedRedSauce"; } }

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public int splitCount = 4;

        public bool preventExplicitSplit = true;

        public override Item SplitSubItem { get { return (Item)GDOUtils.GetExistingGDO(new ServedRedSauce().ID); } }

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
            MaterialUtils.ApplyMaterial(Prefab, "", materials);
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
            cookSauce.Result = (Item)GDOUtils.GetExistingGDO(new CookedWhiteSauce().ID);
            DerivedProcesses.Add(cookSauce);
        }
    }

    public class CookedWhiteSauce : CustomItem
    {
        public override string UniqueNameID { get { return "cookedWhiteSauce"; } }

        public override Item DisposesTo { get { return (Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Pot); } }

        public int splitCount = 4;

        public bool preventExplicitSplit = true;

        public override Item SplitSubItem { get { return (Item)GDOUtils.GetExistingGDO(new ServedWhiteSauce().ID); } }

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
            MaterialUtils.ApplyMaterial(Prefab, "", materials);
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
            noodles.Items.Add((Item)GDOUtils.GetExistingGDO(new CookedNoodles().ID));
            DerivedSets.Add(noodles);

            sauce.Max = 1;
            sauce.Min = 1;
            sauce.IsMandatory = true;
            sauce.Items.Add((Item)GDOUtils.GetExistingGDO(new ServedRedSauce().ID));
            sauce.Items.Add((Item)GDOUtils.GetExistingGDO(new ServedWhiteSauce().ID));
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

            pasta.Item = (Item)GDOUtils.GetExistingGDO(new PlatedPasta().ID);
            pasta.Phase = MenuPhase.Main;
            pasta.Weight = 1;
            UnlocksMenuItems.Add(pasta);

            redSauce.Ingredient = (Item)GDOUtils.GetExistingGDO(new ServedRedSauce().ID);
            redSauce.MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(new PlatedPasta().ID);
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
            pasta.Item = (Item)GDOUtils.GetExistingGDO(new PlatedPasta().ID);
            pasta.Phase = MenuPhase.Main;
            pasta.Weight = 1;
            UnlocksMenuItems.Add(pasta);

            whiteSauce.Ingredient = (Item)GDOUtils.GetExistingGDO(new ServedWhiteSauce().ID);
            whiteSauce.MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(new PlatedPasta().ID);
            UnlocksIngredients.Add(whiteSauce);

            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Flour));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Egg));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Cheese));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Onion));
            MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(KitchenLib.Reference.ItemReference.Water));

            RequiredProcesses.Add((Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Chop));
            RequiredProcesses.Add((Process)GDOUtils.GetExistingGDO(KitchenLib.Reference.ProcessReference.Cook));

            PrerequisiteDishes.Add((Dish)GDOUtils.GetExistingGDO(new PastaBase().ID));

            Requires.Add((Unlock)GDOUtils.GetExistingGDO(new PastaBase().ID));
        }
    }
}
