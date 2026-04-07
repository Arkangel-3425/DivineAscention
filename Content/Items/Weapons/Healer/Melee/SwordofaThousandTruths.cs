using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using ThoriumMod;
using CalamityMod.Buffs.DamageOverTime;
using InfernalEclipseWeaponsDLC.Content.Projectiles.HealerPro.ExecutionersSword;
using CalamityMod.Items;
using Terraria.DataStructures;
using ThoriumMod.Items;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Critters;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;

namespace InfernalEclipseWeaponsDLC.Content.Items.Weapons.Healer.Melee
{
    public class SwordofaThousandTruths : ThoriumItem
    {
        public override void SetDefaults()
        {
            Item.damage = 1000;
            Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            healType = HealType.Ally;
            healAmount = 0;
            healDisplay = true;
            isHealer = true;

            Item.width = 78;
            Item.height = 78;

            Item.useTime = 14;
            Item.useAnimation = 14;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = false;

            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<SwordofaThousandTruthsSlashPro>();
            Item.shootSpeed = 10f;

            Item.noMelee = false;
            Item.noUseGraphic = false;

            Item.scale = 1.25f;

            Item.channel = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2) // right click
            {
                // prevent multiple swords
                if (player.ownedProjectileCounts[ModContent.ProjectileType<SwordofaThousandTruthsHoldPro>()] > 0)
                    return false;

                // Suppress melee swing + graphic
                Item.channel = true;
                Item.noMelee = true;
                Item.noUseGraphic = true;
            }
            else
            {
                // Left click restores normal behavior
                //Item.channel = false;
                Item.noMelee = true;
                Item.noUseGraphic = false;
            }

            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position,
                                   Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.myPlayer != player.whoAmI)
                return false;

            if (player.altFunctionUse == 2)
            {
                // spawn HOLD projectile instead
                Projectile.NewProjectile(
                    source,
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<SwordofaThousandTruthsHoldPro>(),
                    damage,
                    knockback,
                    player.whoAmI
                );

                return false;
            }
            else // left-click slash
            {
                int proj = Projectile.NewProjectile(
                    source,
                    position,
                    velocity,
                    ModContent.ProjectileType<SwordofaThousandTruthsSlashPro>(),
                    damage,
                    knockback,
                    player.whoAmI
                );
                NetMessage.SendData(MessageID.SyncProjectile, number: proj);

                return false; // suppress extra default projectiles
            }
        }

        // Longer use time for right-click
        public override float UseTimeMultiplier(Player player)
        {
            if (player.altFunctionUse == 2)
                return 1.5f; // 50% slower
            return 1f;
        }

        public override float UseAnimationMultiplier(Player player)
        {
            if (player.altFunctionUse == 2)
                return 1.5f;
            return 1f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", $"{Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.DedTo", Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Dedicated.Pudding"))}\n{Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Donor")}");
                line5.OverrideColor = new Color(196, 35, 44);
                tooltips.Add(line5);
            }
            else
            {
                TooltipLine line5 = new(Mod, "DedicatedItem", Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.Donor"));
                line5.OverrideColor = new Color(196, 35, 44);
                tooltips.Add(line5);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<ExecutionersSword>()
                .AddIngredient<PiggyItem>(7)
                .AddIngredient<AscendantSpiritEssence>(3)
                .AddTile(ModContent.TileType<CosmicAnvil>())
                .Register();
        }
    }
}
