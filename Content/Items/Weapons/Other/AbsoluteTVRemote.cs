using CalamityMod.Items;
using CalamityMod;
using CalamityMod.Projectiles.Magic;
using InfernalEclipseWeaponsDLC.Utilities;
using InfernalEclipseWeaponsDLC.Content.Projectiles.HealerPro.BarrenGarden;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.CustomRecipes;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using InfernalEclipseWeaponsDLC.Content.Projectiles.MagicPro.GrandAmplifier;
using CalamityMod.NPCs.CalClone;
using InfernalEclipseWeaponsDLC.Content.Projectiles.OtherPro;

namespace InfernalEclipseWeaponsDLC.Content.Items.Weapons.Other
{
    public class AbsoluteTVRemote : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 30;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item43;
            Item.shoot = ModContent.ProjectileType<AbsoluteTVRemotePausePro>();
            Item.shootSpeed = 16f;
        }

        private bool usingPause = false;

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            // Always allow using right-click
            if (player.altFunctionUse == 2)
            {
                // Right-click: no shooting projectile
                Item.useStyle = ItemUseStyleID.Swing;
                Item.shoot = ModContent.ProjectileType<AbsoluteTVRemotePausePro>();
            }
            else
            {
                // Left-click: normal shooting
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.shoot = ModContent.ProjectileType<AbsoluteTVRemotePausePro>();
            }

            return true;
        }

        public override void HoldItem(Player player)
        {
            if (player.whoAmI != Main.myPlayer)
                return;

            // Prevent triggering during normal item usage
            if (player.itemAnimation > 0)
                return;

            if (InfernalEclipseWeaponsDLC.AbsoluteTVRemotePause.JustPressed)
            {
                ActivateThirdAbility(player);
            }
        }

        private void ActivateThirdAbility(Player player)
        {
            if (player.itemAnimation > 0)
                return;

            usingPause = true;

            player.controlUseItem = true;
            player.ItemCheck();

            usingPause = false;
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2) // right-click
            {
                // spawn invisible projectile to do the effect
                if (Main.myPlayer == player.whoAmI)
                {
                    Projectile.NewProjectile(
                        player.GetSource_ItemUse(Item),
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<AbsoluteTVRemotePausePro>(),
                        Item.damage,
                        0f,
                        player.whoAmI
                    );
                }

                return true; // counts as using the item
            }

            return base.UseItem(player); // left-click normal
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
    Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Right click still handled normally
            if (player.altFunctionUse == 2)
                return false;

            int projType = type;

            if (usingPause)
            {
                projType = ModContent.ProjectileType<AbsoluteTVRemotePausePro>();
            }

            Projectile.NewProjectile(
                source,
                position,
                velocity,
                projType,
                damage,
                knockback,
                player.whoAmI
            );

            return false; // we handled spawning ourselves
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string key = InfernalEclipseWeaponsDLC.AbsoluteTVRemotePause.GetAssignedKeys().Count > 0
                ? InfernalEclipseWeaponsDLC.AbsoluteTVRemotePause.GetAssignedKeys()[0]
                : "Unbound";

            foreach (var line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name.StartsWith("Tooltip"))
                {
                    line.Text = string.Format(line.Text, key);
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WulfrumMetalScrap>(5)
                .AddIngredient<EnergyCore>(5)
                .AddIngredient<DubiousPlating>(5)
                .AddIngredient<MysteriousCircuitry>(5)
                .AddIngredient(ItemID.MartianConduitPlating, 5)
                .AddIngredient<InfectedArmorPlating>(5)
                .AddIngredient<MiracleMatter>()
                .AddIngredient<ShadowspecBar>(5)
                .AddIngredient<Rock>()
                .AddTile(ModContent.TileType<DraedonsForge>())
                .Register();
        }
    }
}