using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumMod;
using ThoriumMod.Projectiles;
using InfernalEclipseWeaponsDLC.Content.Items.Accessories.Melee.Flails;
using InfernalEclipseWeaponsDLC.Content.Projectiles.FlailPro;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ThoriumMod.Items.BasicAccessories;
using InfernalEclipseWeaponsDLC.Core.NewFolder;
using CalamityMod.Items;
using CalamityMod.Rarities;

namespace InfernalEclipseWeaponsDLC.Content.Items.Accessories.Melee.Flails
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class PerfectPurple : ModItem
    {
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
            Recipe recipe = CreateRecipe()
                .AddIngredient(ModContent.ItemType<IronFlailCore>())
                .AddIngredient(ModContent.ItemType<CosmiliteBar>(), 15)
                .AddIngredient(ModContent.ItemType<DoubleFlail>())
                .AddTile(ModContent.TileType<CosmicAnvil>())
                .Register();
        }
    }
}