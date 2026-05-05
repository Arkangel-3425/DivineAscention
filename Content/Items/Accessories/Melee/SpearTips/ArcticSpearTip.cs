using CalamityMod.Items.Materials;
using CalamityMod.Items;
using InfernalEclipseWeaponsDLC.Core.NewFolder;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items;

namespace InfernalEclipseWeaponsDLC.Content.Items.Accessories.Melee.SpearTips
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class ArcticSpearTip : ThoriumItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.HasMod("ThoriumMod");
        }

        public override void SetDefaults()
        {
            accDamage = Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.ArcticSpearTip");
            Item.width = 32;
            Item.height = 32;
            Item.value = CalamityGlobalItem.RarityLightPurpleBuyPrice;
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            accessoryType = AccessoryType.SpearTip;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<InfernalWeaponsPlayer>().spearArctic = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CrystalSpearTip>()
                .AddIngredient<CryonicBar>(6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
