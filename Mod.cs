using BepInEx;
using KitchenLib;
using KitchenLib.Reference;
using PastaPalace.Customs.NoodleChain;
using PastaPalace.Customs.PastaProcess;
using PastaPalace.Customs.RedSauceChain;
using PastaPalace.Customs.WhiteSauceChain;
using System.IO;
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

        internal static int FlourID = ItemReference.Flour;
        internal static int EggCrackedID = ItemReference.EggCracked;
        internal static int WaterID = ItemReference.Water;
        internal static int PotID = ItemReference.Pot;
        internal static int OnionID = ItemReference.Onion;
        internal static int TomatoSauceID = ItemReference.TomatoSauce;
        internal static int CheeseID = ItemReference.Cheese;
        internal static int BroccoliChoppedContainerCookedID = ItemReference.BroccoliChoppedContainerCooked;
        internal static int MeatChoppedContainerCookedID = ItemReference.MeatChoppedContainerCooked;
        internal static int PlateID = ItemReference.Plate;
        internal static int EggID = ItemReference.Egg;
        internal static int TomatoID = ItemReference.Tomato;

        internal static int CookID = ProcessReference.Cook;
        internal static int ChopID = ProcessReference.Chop;

        internal static int RawNoodlesID;
        internal static int RawNoodlePotID;
        internal static int CookedNoodlePotID;
        internal static int CookedNoodlesID;
        internal static int BurntNoodlePotID;

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
        }

        void Awake()
        {
            /* 
            * Noodle Chain:
            * Mix flour with cracked egg. 
            * Add water and raw noodle to pot.
            * Cook and portion onto plate. Serves 6.
            */
            RawNoodlesID = AddGameDataObject<RawNoodles>().ID;
            RawNoodlePotID = AddGameDataObject<RawNoodlePot>().ID;
            CookedNoodlePotID = AddGameDataObject<CookedNoodlePot>().ID;
            BurntNoodlePotID = AddGameDataObject<BurntNoodlePot>().ID;
            CookedNoodlesID = AddGameDataObject<CookedNoodles>().ID;
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
    }
}
