using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using InfernalEclipseWeaponsDLC.Core.NewFolder;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Terraria.ID;

namespace InfernalEclipseWeaponsDLC.Content.Items.Accessories.Melee.Flails
{
    public class HolyFlail : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => WeaponConfig.Instance.FlailCores;

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<InfernalWeaponsPlayer>().holyFlail = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DoubleFlail>())
                .AddIngredient(ModContent.ItemType<ShadowspecBar>(), 10)
                .AddIngredient(ModContent.ItemType<DarkPlasma>(), 15)
                .AddTile(ModContent.TileType<DraedonsForge>())
                .Register();
        }
    }
}