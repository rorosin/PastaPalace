using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;

namespace PastaPalace.Customs.PastaProcess
{
    internal class PastaWhite : CustomDish
    {
        public override string UniqueNameID => "Pasta -- White Sauce";
        public override DishType Type => DishType.Base;
        public override DishCustomerChange CustomerMultiplier => DishCustomerChange.SmallDecrease;
        public override CardType CardType => CardType.Default;
        public override Unlock.RewardLevel ExpReward => Unlock.RewardLevel.Medium;
        public override UnlockGroup UnlockGroup => UnlockGroup.Dish;
        public override List<Dish.MenuItem> ResultingMenuItems => new List<Dish.MenuItem>
        {
            new Dish.MenuItem
            {
                Item = (Item)GDOUtils.GetExistingGDO(Mod.PlatedPastaID)
            }
        };
        public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks => new HashSet<Dish.IngredientUnlock>
        {
            new Dish.IngredientUnlock
            {
                Ingredient = (Item)GDOUtils.GetExistingGDO(Mod.ServedWhiteSauceID),
                MenuItem = (ItemGroup)GDOUtils.GetExistingGDO(Mod.PlatedPastaID)
            }
        };
        public override HashSet<Item> MinimumIngredients => new HashSet<Item>
        {
            (Item)GDOUtils.GetExistingGDO(Mod.FlourID),
            (Item)GDOUtils.GetExistingGDO(Mod.EggID),
            (Item)GDOUtils.GetExistingGDO(Mod.CheeseID),
            (Item)GDOUtils.GetExistingGDO(Mod.OnionID),
            (Item)GDOUtils.GetExistingGDO(Mod.WaterID)
        };
        public override HashSet<Process> RequiredProcesses => new HashSet<Process>
        {
            (Process)GDOUtils.GetExistingGDO(Mod.CookID),
            (Process)GDOUtils.GetExistingGDO(Mod.ChopID)
        };
        public override HashSet<Dish> PrerequisiteDishesEditor => new HashSet<Dish>
        {
            (Dish)GDOUtils.GetExistingGDO(Mod.PastaBaseID)
        };
        public override List<Unlock> HardcodedRequirements => new List<Unlock>
        {
            (Unlock)GDOUtils.GetExistingGDO(Mod.PastaBaseID)
        };
    }
}
