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
using CalamityMod.Cooldowns;
using Terraria.Localization;

namespace InfernalEclipseWeaponsDLC.Content.Items.Weapons.Other
{
    public class AbsoluteTVRemote : ModItem
    {
        public class RemotePauseCooldown : CooldownHandler
        {
            public static new string ID => "RemotePauseCooldown";
            public override bool ShouldDisplay => true;
            public override bool SavedWithPlayer => true;
            public override bool PersistsThroughDeath => true;
            public override LocalizedText DisplayName => Language.GetText("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Cooldowns.Pause");
            public override string Texture => "InfernalEclipseWeaponsDLC/Content/Projectiles/OtherPro/AbsoluteTVRemotePauseIcon";
            public override Color OutlineColor => Color.Gray;
            public override Color CooldownStartColor => Color.LightGray;
            public override Color CooldownEndColor => Color.DarkGray;

            public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Texture2D value2 = ModContent.Request<Texture2D>(OutlineTexture).Value;
                Texture2D value3 = ModContent.Request<Texture2D>(ChargeBarTexture).Value;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
                ApplyBarShaders(opacity);
                spriteBatch.Draw(value3, position, null, Color.White * opacity, 0f, value3.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                spriteBatch.Draw(value2, position, null, OutlineColor * opacity, 0f, value2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(value, position, null, Color.White * opacity, 0f, value.Size() * 0.5f, scale / 2, SpriteEffects.None, 0f);
            }

            public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Texture2D value2 = ModContent.Request<Texture2D>(OutlineTexture).Value;
                Texture2D value3 = ModContent.Request<Texture2D>(OverlayTexture).Value;
                Color outlineColor = OutlineColor;
                spriteBatch.Draw(value2, position, null, outlineColor * opacity, 0f, value2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(value, position, null, Color.White * opacity, 0f, value.Size() * 0.5f, scale / 2, SpriteEffects.None, 0f);
                int num = (int)Math.Ceiling((float)value3.Height * (1f - instance.Completion));
                spriteBatch.Draw(sourceRectangle: new Rectangle(0, num, value3.Width, value3.Height - num), texture: value3, position: position + Vector2.UnitY * num * scale, color: outlineColor * opacity * 0.9f, rotation: 0f, origin: value.Size() * 0.25f, scale: scale, effects: SpriteEffects.None, layerDepth: 0f);
            }
        }

        public class RemoteFastForwardCooldown : CooldownHandler
        {
            public static new string ID => "RemoteFastForwardCooldown";
            public override bool ShouldDisplay => true;
            public override bool SavedWithPlayer => true;
            public override bool PersistsThroughDeath => true;
            public override LocalizedText DisplayName => Language.GetText("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Cooldowns.FastForward");
            public override string Texture => "InfernalEclipseWeaponsDLC/Content/Projectiles/OtherPro/AbsoluteTVRemoteFastForwardIcon";
            public override Color OutlineColor => Color.Gray;
            public override Color CooldownStartColor => Color.LightGray;
            public override Color CooldownEndColor => Color.DarkGray;

            public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Texture2D value2 = ModContent.Request<Texture2D>(OutlineTexture).Value;
                Texture2D value3 = ModContent.Request<Texture2D>(ChargeBarTexture).Value;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
                ApplyBarShaders(opacity);
                spriteBatch.Draw(value3, position, null, Color.White * opacity, 0f, value3.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                spriteBatch.Draw(value2, position, null, OutlineColor * opacity, 0f, value2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(value, position, null, Color.White * opacity, 0f, value.Size() * 0.5f, scale / 2, SpriteEffects.None, 0f);
            }

            public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Texture2D value2 = ModContent.Request<Texture2D>(OutlineTexture).Value;
                Texture2D value3 = ModContent.Request<Texture2D>(OverlayTexture).Value;
                Color outlineColor = OutlineColor;
                spriteBatch.Draw(value2, position, null, outlineColor * opacity, 0f, value2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(value, position, null, Color.White * opacity, 0f, value.Size() * 0.5f, scale / 2, SpriteEffects.None, 0f);
                int num = (int)Math.Ceiling((float)value3.Height * (1f - instance.Completion));
                spriteBatch.Draw(sourceRectangle: new Rectangle(0, num, value3.Width, value3.Height - num), texture: value3, position: position + Vector2.UnitY * num * scale, color: outlineColor * opacity * 0.9f, rotation: 0f, origin: value.Size() * 0.25f, scale: scale, effects: SpriteEffects.None, layerDepth: 0f);
            }
        }

        public class RemoteSlowdownCooldown : CooldownHandler
        {
            public static new string ID => "RemoteSlowdownCooldown";
            public override bool ShouldDisplay => true;
            public override bool SavedWithPlayer => true;
            public override bool PersistsThroughDeath => true;
            public override LocalizedText DisplayName => Language.GetText("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Cooldowns.Slowdown");
            public override string Texture => "InfernalEclipseWeaponsDLC/Content/Projectiles/OtherPro/AbsoluteTVRemoteRewindIcon";
            public override Color OutlineColor => Color.Gray;
            public override Color CooldownStartColor => Color.LightGray;
            public override Color CooldownEndColor => Color.DarkGray;

            public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Texture2D value2 = ModContent.Request<Texture2D>(OutlineTexture).Value;
                Texture2D value3 = ModContent.Request<Texture2D>(ChargeBarTexture).Value;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
                ApplyBarShaders(opacity);
                spriteBatch.Draw(value3, position, null, Color.White * opacity, 0f, value3.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                spriteBatch.Draw(value2, position, null, OutlineColor * opacity, 0f, value2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(value, position, null, Color.White * opacity, 0f, value.Size() * 0.5f, scale / 2, SpriteEffects.None, 0f);
            }

            public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Texture2D value2 = ModContent.Request<Texture2D>(OutlineTexture).Value;
                Texture2D value3 = ModContent.Request<Texture2D>(OverlayTexture).Value;
                Color outlineColor = OutlineColor;
                spriteBatch.Draw(value2, position, null, outlineColor * opacity, 0f, value2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(value, position, null, Color.White * opacity, 0f, value.Size() * 0.5f, scale / 2, SpriteEffects.None, 0f);
                int num = (int)Math.Ceiling((float)value3.Height * (1f - instance.Completion));
                spriteBatch.Draw(sourceRectangle: new Rectangle(0, num, value3.Width, value3.Height - num), texture: value3, position: position + Vector2.UnitY * num * scale, color: outlineColor * opacity * 0.9f, rotation: 0f, origin: value.Size() * 0.25f, scale: scale, effects: SpriteEffects.None, layerDepth: 0f);
            }
        }

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
            if (!usingPause)
            {
                // Always allow using right-click
                if (player.altFunctionUse == 2)
                {
                    // Right-click: no shooting projectile
                    Item.useStyle = ItemUseStyleID.HoldUp;
                    Item.shoot = ModContent.ProjectileType<AbsoluteTVRemoteFastForwardPro>();

                    if (player.Calamity().cooldowns.ContainsKey(RemoteFastForwardCooldown.ID))
                        return false;
                    player.AddCooldown(RemoteFastForwardCooldown.ID, 7200);
                    //player.AddCooldown(RemoteFastForwardCooldown.ID, 180);
                }
                else
                {
                    // Left-click: normal shooting
                    Item.useStyle = ItemUseStyleID.HoldUp;
                    Item.shoot = ModContent.ProjectileType<AbsoluteTVRemoteRewindPro>();

                    if (player.Calamity().cooldowns.ContainsKey(RemoteSlowdownCooldown.ID))
                        return false;
                    player.AddCooldown(RemoteSlowdownCooldown.ID, 7200);
                    //player.AddCooldown(RemoteSlowdownCooldown.ID, 180);
                }
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

            if (player.Calamity().cooldowns.ContainsKey(RemotePauseCooldown.ID))
                return;
            player.AddCooldown(RemotePauseCooldown.ID, 18000);
            //player.AddCooldown(RemotePauseCooldown.ID, 180);

            Item.useStyle = ItemUseStyleID.Shoot;

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