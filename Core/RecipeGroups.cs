using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.BasicAccessories;

namespace InfernalEclipseWeaponsDLC.Core
{
    public class RecipeGroups : ModSystem
    {
        public static RecipeGroup Titanium;
        public static RecipeGroup IronShield;
        public override void Unload()
        {
            Titanium = null;
            IronShield = null;
        }

        public override void AddRecipeGroups()
        {
            string modName = Mod.Name;

            Titanium = new RecipeGroup(() => "Adamantite or Titanium Bar", new int[2]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup($"{modName}:TitaniumRecipeGroup", Titanium);

            IronShield = new RecipeGroup(() => "Iron or Lead Shield", new int[2]
            {
                ModContent.ItemType<IronShield>(),
                ModContent.ItemType<LeadShield>()
            });
            RecipeGroup.RegisterGroup($"{modName}:IronShieldRecipeGroup", IronShield);
        }
    }
}
