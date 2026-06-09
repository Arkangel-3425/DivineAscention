using System;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items.Materials;
using InfernalEclipseWeaponsDLC.Content.Projectiles.MeleePro.DivineAxe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Items.HealerItems;

namespace InfernalEclipseWeaponsDLC.Content.Items.Weapons.Melee
{
    public class DivineAxe : ModItem
    {
        private int attackCombo = 1;

        public override void SetDefaults()
        {
            Item.damage = 1850;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.width = 112;
            Item.height = 122;
            Item.knockBack = 6f;
            Item.value = 1;
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.useTurn = true;

            Item.useTime = 60;
            Item.useAnimation = 60;

            Item.shoot = ModContent.ProjectileType<DivineAxeHoldout>();
            Item.shootSpeed = 1f;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            int axeHoldout = ModContent.ProjectileType<DivineAxeHoldout>();
            int spearHoldout = ModContent.ProjectileType<DivineAxeSpearHoldout>();

            if (player.ownedProjectileCounts[axeHoldout] > 0 ||
                player.ownedProjectileCounts[spearHoldout] > 0)
            {
                return false;
            }

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = true;
            Item.shootSpeed = 1f;

            if (player.altFunctionUse == 2)
            {
                // Skytide Dragoon-style right click.
                Item.useTime = 35;
                Item.useAnimation = 35;
                Item.shoot = spearHoldout;
                Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            }
            else
            {
                // Original Divine Axe left click.
                Item.useTime = 60;
                Item.useAnimation = 60;
                Item.shoot = axeHoldout;
                Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            }

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(
                    source,
                    position,
                    Vector2.Zero,
                    ModContent.ProjectileType<DivineAxeSpearHoldout>(),
                    damage,
                    knockback,
                    player.whoAmI
                );

                return false;
            }

            attackCombo = -attackCombo;

            SoundEngine.PlaySound(
                new SoundStyle(attackCombo == 1
                    ? "InfernalEclipseWeaponsDLC/Assets/Sounds/DemonSwordSwing1"
                    : "InfernalEclipseWeaponsDLC/Assets/Sounds/DemonSwordSwing2"),
                player.Center
            );

            Projectile.NewProjectile(
                source,
                position,
                velocity,
                ModContent.ProjectileType<DivineAxeHoldout>(),
                damage,
                knockback,
                player.whoAmI,
                attackCombo
            );

            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color lerpedColor = Color.Lerp(Color.White, new Color(30, 144, 255), (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.0) * 0.5 + 0.5));

            if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                TooltipLine dedicatedLine = new(Mod, "DedicatedItem", $"{Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.DedTo", Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Dedicated.Arkangel"))}\n{Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Playtester")}");
                dedicatedLine.OverrideColor = lerpedColor;
                tooltips.Add(dedicatedLine);
            }
            else
            {
                TooltipLine dedicatedLine = new(Mod, "DedicatedItem", Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Playtester"));
                dedicatedLine.OverrideColor = lerpedColor;
                tooltips.Add(dedicatedLine);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 12)
                .AddIngredient(ItemID.FragmentSolar, 4)
                .AddIngredient<CelestialFragment>(4)
                .AddIngredient<UnholyEssence>(9)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
