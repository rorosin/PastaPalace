using BepInEx;
using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Customs;
using KitchenLib.Event;
using KitchenLib.References;
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

        internal static Item Flour => GetExistingGDO<Item>(ItemReferences.Flour);
        internal static Item EggCracked => GetExistingGDO<Item>(ItemReferences.EggCracked);
        internal static Item Water => GetExistingGDO<Item>(ItemReferences.Water);
        internal static Item Pot => GetExistingGDO<Item>(ItemReferences.Pot);
        internal static Item Onion => GetExistingGDO<Item>(ItemReferences.Onion);
        internal static Item TomatoSauce => GetExistingGDO<Item>(ItemReferences.TomatoSauce);
        internal static Item Cheese => GetExistingGDO<Item>(ItemReferences.Cheese);
        internal static Item BroccoliChoppedContainerCooked => GetExistingGDO<Item>(ItemReferences.BroccoliChoppedContainerCooked);
        internal static Item MeatChoppedContainerCooked => GetExistingGDO<Item>(ItemReferences.MeatChoppedContainerCooked);
        internal static Item Plate => GetExistingGDO<Item>(ItemReferences.Plate);
        internal static Item Egg => GetExistingGDO<Item>(ItemReferences.Egg);
        internal static Item Tomato => GetExistingGDO<Item>(ItemReferences.Tomato);

        internal static Process Cook => GetExistingGDO<Process>(ProcessReferences.Cook);
        internal static Process Chop => GetExistingGDO<Process>(ProcessReferences.Chop);

        internal static int RawNoodlesID;
        internal static int RawNoodlePotID;
        internal static int CookedNoodlesID;
        internal static int CookedNoodlePotID;
        internal static int BurntNoodlePotID;
        internal static ItemGroup RawNoodles;
        internal static ItemGroup RawNoodlePot;
        internal static Item CookedNoodles;
        internal static Item CookedNoodlePot;
        internal static Item BurntNoodlePot;

        internal static int UncookedRedSauceID;
        internal static int ServedRedSauceID;
        internal static int CookedRedSauceID;
        internal static ItemGroup UncookedRedSauce;
        internal static Item ServedRedSauce;
        internal static Item CookedRedSauce;

        internal static int UncookedWhiteSauceID;
        internal static int ServedWhiteSauceID;
        internal static int CookedWhiteSauceID;
        internal static ItemGroup UncookedWhiteSauce;
        internal static Item ServedWhiteSauce;
        internal static Item CookedWhiteSauce;

        internal static int PlatedPastaID;
        internal static int PastaBaseID;
        internal static int PastaWhiteID;
        internal static ItemGroup PlatedPasta;
        internal static Dish PastaBase;
        internal static Dish PastaWhite;

        internal static GameData gamedata;
        internal static AssetBundle bundle;

        public Mod() : base($">={MOD_VERSION}", Assembly.GetCallingAssembly())
        {
            bundle = AssetBundle.LoadFromFile(Path.Combine(new string[] { Directory.GetParent(Application.dataPath).FullName, "BepInEx", "plugins", "PastaPalace", "assets", "pastapalacetextures" }));
            //bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "pastapalacetextures"));
            Debug.Log($"Loaded Textures: {bundle != null}");

            Events.BuildGameDataEvent += OnBuildGameDataEvent;
        }

        void Start()
        {
            /* 
            * Noodle Chain:
            * Mix flour with cracked egg. 
            * Add water and raw noodle to pot.
            * Cook and portion onto plate. Serves 6.
            */
            RawNoodlesID = AddGameDataObject<RawNoodles>().ID;
            CookedNoodlesID = AddGameDataObject<CookedNoodles>().ID;
            BurntNoodlePotID = AddGameDataObject<BurntNoodlePot>().ID;
            RawNoodlePotID = AddGameDataObject<RawNoodlePot>().ID;
            CookedNoodlePotID = AddGameDataObject<CookedNoodlePot>().ID;
            Debug.Log("Loaded Noodles.");

            /*
            * Red Sauce Chain
            * Add Onion, Tomato Sauce to pot and cook. 
            * Pour onto cooked noodles. Serves 4.
            */
            UncookedRedSauceID = AddGameDataObject<UncookedRedSauce>().ID;
            CookedRedSauceID = AddGameDataObject<CookedRedSauce>().ID;
            ServedRedSauceID = AddGameDataObject<ServedRedSauce>().ID;
            Debug.Log("Loaded Red Sauce.");

            /*
            * White Sauce Chain
            * Add Onion, Cheese to pot and cook. 
            * Pour onto cooked noodles. Serves 4.
            */
            UncookedWhiteSauceID = AddGameDataObject<UncookedWhiteSauce>().ID;
            CookedWhiteSauceID = AddGameDataObject<CookedWhiteSauce>().ID;
            ServedWhiteSauceID = AddGameDataObject<ServedWhiteSauce>().ID;
            Debug.Log("Loaded White Sauce.");

            /*
            * Pasta Process:
            * Add Cooked Noodles
            * Add Red or White Sauce
            * (Optional) Add topping
            * Serve
            */
            PlatedPastaID = AddGameDataObject<PlatedPasta>().ID;
            PastaBaseID = AddGameDataObject<PastaBase>().ID;
            PastaWhiteID = AddGameDataObject<PastaWhite>().ID;
            Debug.Log("Loaded Dish.");
        }

        private void OnBuildGameDataEvent(object sender, BuildGameDataEventArgs e)
        {
            gamedata = e.gamedata;

            RawNoodles = GetModdedGDO<ItemGroup>(RawNoodlesID);
            CookedNoodles = GetModdedGDO<Item>(CookedNoodlesID);
            BurntNoodlePot = GetModdedGDO<Item>(BurntNoodlePotID);
            RawNoodlePot = GetModdedGDO<ItemGroup>(RawNoodlePotID);
            CookedNoodlePot = GetModdedGDO<Item>(CookedNoodlePotID);

            UncookedRedSauce = GetModdedGDO<ItemGroup>(UncookedRedSauceID);
            CookedRedSauce = GetModdedGDO<Item>(CookedRedSauceID);
            ServedRedSauce = GetModdedGDO<Item>(ServedRedSauceID);

            UncookedWhiteSauce = GetModdedGDO<ItemGroup>(UncookedWhiteSauceID);
            CookedWhiteSauce = GetModdedGDO<Item>(CookedWhiteSauceID);
            ServedWhiteSauce = GetModdedGDO<Item>(ServedWhiteSauceID);

            PlatedPasta = GetModdedGDO<ItemGroup>(PlatedPastaID);
            PastaBase = GetModdedGDO<Dish>(PastaBaseID);
            PastaWhite = GetModdedGDO<Dish>(PastaWhiteID);
        }

        internal static T GetModdedGDO<T>(int id) where T: GameDataObject
        {
            CustomGDO.GetGameDataObject(id).Convert(Mod.gamedata, out GameDataObject gdo);
            return (T)gdo;
        }

        private static T GetExistingGDO<T>(int id) where T: GameDataObject
        {
            return (T)GDOUtils.GetExistingGDO(id);
        }
    }
}
