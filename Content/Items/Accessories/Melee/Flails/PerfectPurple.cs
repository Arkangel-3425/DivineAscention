using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ThoriumMod.Items.BasicAccessories;
using InfernalEclipseWeaponsDLC.Core.NewFolder;
using CalamityMod.Items;
using CalamityMod.Rarities;

namespace InfernalEclipseWeaponsDLC.Content.Items.Accessories.Melee.Flails
{
    public class PerfectPurple : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => WeaponConfig.Instance.FlailCores;

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<InfernalWeaponsPlayer>().PerfectPurple = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DoubleFlail>())
                .AddIngredient(ModContent.ItemType<IronFlailCore>())
                .AddIngredient(ModContent.ItemType<CosmiliteBar>(), 15)
                .AddTile(ModContent.TileType<CosmicAnvil>())
                .Register();
        }
    }
}