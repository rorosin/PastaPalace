using BepInEx;
using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Customs;
using KitchenLib.Event;
using KitchenLib.Reference;
using KitchenLib.Utils;
using PastaPalace.Customs.NoodleChain;
using PastaPalace.Customs.PastaProcess;
using PastaPalace.Customs.RedSauceChain;
using PastaPalace.Customs.WhiteSauceChain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PastaPalace
{
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

    [BepInProcess("PlateUp.exe")]
    [BepInPlugin(MOD_ID, MOD_NAME, MOD_VERSION)]
    public class Mod : BaseMod
    {
        public const string MOD_ID = "pastapalace";
        public const string MOD_NAME = "Pasta Palace";
        public const string MOD_VERSION = "0.0.1";

        internal static int FlourID = KitchenLib.Reference.ItemReference.Flour;
        internal static int EggCrackedID = KitchenLib.Reference.ItemReference.EggCracked;
        internal static int WaterID = KitchenLib.Reference.ItemReference.Water;
        internal static int PotID = KitchenLib.Reference.ItemReference.Pot;
        internal static int OnionID = KitchenLib.Reference.ItemReference.Onion;
        internal static int TomatoSauceID = KitchenLib.Reference.ItemReference.TomatoSauce;
        internal static int CheeseID = KitchenLib.Reference.ItemReference.Cheese;
        internal static int BroccoliChoppedContainerCookedID = KitchenLib.Reference.ItemReference.BroccoliChoppedContainerCooked;
        internal static int MeatChoppedContainerCookedID = KitchenLib.Reference.ItemReference.MeatChoppedContainerCooked;
        internal static int PlateID = KitchenLib.Reference.ItemReference.Plate;
        internal static int EggID = KitchenLib.Reference.ItemReference.Egg;
        internal static int TomatoID = KitchenLib.Reference.ItemReference.Tomato;

        internal static int CookID = ProcessReference.Cook;
        internal static int ChopID = ProcessReference.Chop;

        internal static CustomItem CookedNoodles;

        internal static int UncookedRedSauceID;
        internal static int CookedRedSauceID;
        internal static int ServedRedSauceID;

        internal static int UncookedWhiteSauceID;
        internal static int CookedWhiteSauceID;
        internal static int ServedWhiteSauceID;

        internal static int PlatedPastaID;
        internal static int PastaBaseID;
        internal static int PastaWhiteID;

        public static AssetBundle bundle;

        public Mod() : base($">={MOD_VERSION}", Assembly.GetCallingAssembly())
        {
            bundle = AssetBundle.LoadFromFile(Path.Combine(new string[] { Directory.GetParent(Application.dataPath).FullName, "BepInEx", "plugins", "PastaPalace", "assets", "pastapalacetextures" }));
            //bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "pastapalacetextures"));
            Debug.Log($"Loaded Textures: {bundle != null}");

            Events.AfterGDORegisterEvent += AfterGDORegisterEvent;
        }

        void Awake()
        {
            /* 
            * Noodle Chain:
            * Mix flour with cracked egg. 
            * Add water and raw noodle to pot.
            * Cook and portion onto plate. Serves 6.
            */
            AddGameDataObject<RawNoodles>();
            AddGameDataObject<CookedNoodles>();
            AddGameDataObject<RawNoodlePot>();
            AddGameDataObject<CookedNoodlePot>();
            AddGameDataObject<BurntNoodlePot>();
            Debug.Log("Loaded Noodles.");


            /*
            * Red Sauce Chain
            * Add Onion, Tomato Sauce to pot and cook. 
            * Pour onto cooked noodles. Serves 4.
            */
            //UncookedRedSauceID = AddGameDataObject<UncookedRedSauce>().ID;
            //CookedRedSauceID = AddGameDataObject<CookedRedSauce>().ID;
            //ServedRedSauceID = AddGameDataObject<ServedRedSauce>().ID;
            //Debug.Log("Loaded Red Sauce.");

            /*
            * White Sauce Chain
            * Add Onion, Cheese to pot and cook. 
            * Pour onto cooked noodles. Serves 4.
            */
            //UncookedWhiteSauceID = AddGameDataObject<UncookedWhiteSauce>().ID;
            //CookedWhiteSauceID = AddGameDataObject<CookedWhiteSauce>().ID;
            //ServedWhiteSauceID = AddGameDataObject<ServedWhiteSauce>().ID;
            //Debug.Log("Loaded White Sauce.");

            /*
            * Pasta Process:
            * Add Cooked Noodles
            * Add Red or White Sauce
            * (Optional) Add topping
            * Serve
             */
            //PlatedPastaID = AddGameDataObject<PlatedPasta>().ID;
            //PastaBaseID = AddGameDataObject<PastaBase>().ID;
            //PastaWhiteID = AddGameDataObject<PastaWhite>().ID;
            //Debug.Log("Loaded Dish.");
        }

        private void AfterGDORegisterEvent(object sender, AfterGDORegisterEventArgs e)
        {
            List<GameDataObject> gameDataObjects = e.GameDataObjects;

            ItemGroup RawNoodlePot = (ItemGroup)gameDataObjects.FirstOrDefault(a => a.ID == GetHash(MOD_NAME, "RawNoodlePot"));
            Item RawNoodle = (Item)gameDataObjects.FirstOrDefault(a => a.ID == GetHash(Mod.MOD_NAME, "RawNoodles"));
            Item CookedNoodlePot = (Item)gameDataObjects.FirstOrDefault(a => a.ID == GetHash(Mod.MOD_NAME, "CookedNoodlePot"));
            Item CookedNoodles = (Item)gameDataObjects.FirstOrDefault(a => a.ID == GetHash(Mod.MOD_NAME, "CookedNoodles"));
            Item BurntNoodlePot = (Item)gameDataObjects.FirstOrDefault(a => a.ID == GetHash(Mod.MOD_NAME, "BurntNoodlePot"));

            Debug.Log($"RawNoodlePot:{RawNoodlePot.ID == GetHash(MOD_NAME, "RawNoodlePot")}");
            Debug.Log($"RawNoodle:{RawNoodle == null}");
            Debug.Log($"CookedNoodlePot:{CookedNoodlePot == null}");
            Debug.Log($"CookedNoodles:{CookedNoodles == null}");
            Debug.Log($"BurntNoodlePot:{BurntNoodlePot == null}");

            FieldInfo sets = ReflectionUtils.GetField<ItemGroup>("Sets");
            FieldInfo processes = ReflectionUtils.GetField<Item>("Processes");

            sets.SetValue(RawNoodlePot, new List<ItemGroup.ItemSet>
            {
                new ItemGroup.ItemSet
                {
                    Max = 2,
                    Min = 2,
                    Items = new List<Item>
                    {
                        (Item)GDOUtils.GetExistingGDO(Mod.WaterID),
                        RawNoodle
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
            });

            /*
            processes.SetValue(RawNoodlePot, new List<Item.ItemProcess>
            {
                new Item.ItemProcess
                {
                    Duration = 10,
                    Process = (Process)GDOUtils.GetExistingGDO(Mod.CookID),
                    Result = CookedNoodlePot
                }
            });

            CookedNoodlePot.SplitSubItem = CookedNoodles;
            processes.SetValue(CookedNoodlePot, new List<Item.ItemProcess>
            {
                new Item.ItemProcess
                {
                    Duration = 30,
                    Process = (Process)GDOUtils.GetExistingGDO(Mod.CookID),
                    Result = BurntNoodlePot
                }
            });
            */
        }

        internal static int GetHash(string ModName, string UniqueNameID)
        {
            return StringUtils.GetInt32HashCode($"{ModName}:{UniqueNameID}");
        }
    }
}
