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
using InfernalEclipseWeaponsDLC.Content.Projectiles.FlailPro;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Items.Materials;
using ThoriumMod.Items.BasicAccessories;
using InfernalEclipseWeaponsDLC.Core.NewFolder;
using CalamityMod.Items;

namespace InfernalEclipseWeaponsDLC.Content.Items.Accessories.Melee.Flails
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class DoubleFlail : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<InfernalWeaponsPlayer>().DoubleFlailAcc = true;
        }
        public override void AddRecipes()
        {
            Mod Calamity = ModLoader.GetMod("CalamityMod");
            Mod Thorium = ModLoader.GetMod("ThoriumMod");
            Recipe recipe = CreateRecipe()
                .AddIngredient(ModContent.ItemType<IronFlailCore>(), 2)
                .AddIngredient(ModContent.ItemType<AshesofCalamity>(), 10)
                .AddIngredient(ModContent.ItemType<UnholyCore>(), 5)
                .AddIngredient(ModContent.ItemType<CryonicBar>(), 10)
                .AddIngredient(ModContent.ItemType<EssenceofEleum>(), 2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}