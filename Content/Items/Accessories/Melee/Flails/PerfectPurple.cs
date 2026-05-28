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
            Item.value = Item.sellPrice(0, 0, 15, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PerfectPurplePlayer>().PerfectPurple = true;
        }
        public override void AddRecipes()
        {
            Mod Calamity = ModLoader.GetMod("CalamityMod");
            Mod Thorium = ModLoader.GetMod("ThoriumMod");
            Mod MyCut = ModLoader.GetMod("MyCut");
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(
                Thorium.Find<ModItem>("IronFlailCore").Type, 1)
                .AddIngredient(Calamity.Find<ModItem>("CosmoiliteBar").Type, 15)
                .AddIngredient(MyCut.Find<ModItem>("DoubleFlail").Type, 1)
                .AddTile(Calamity.Find<ModTile>("CosmicAnvil").Type)
                .Register();
        }
    }
    internal class PerfectPurplePlayer : ModPlayer
    {
        public bool PerfectPurple;
        public override void ResetEffects()
        {
            PerfectPurple = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            bool isFlail = false;
            if (proj.ModProjectile is FlailProBase || proj.ModProjectile is BaseMaceFlailProjectile || proj.ModProjectile is CalamityMod.Projectiles.Melee.DragonPowFlail || proj.aiStyle == ProjAIStyleID.Flail)
                {
                    isFlail = true;
                }
            Vector2 vector = proj.velocity * 0.5f;
            if (vector == Vector2.Zero)
            {
                vector = Main.MouseWorld - Player.Center;
                vector.Normalize();
                vector *= 6f;
            }
            if (PerfectPurple && Utils.NextBool(Main.rand, 4) && isFlail == true)
            {
                SoundEngine.PlaySound(SoundID.Item1, proj.Center);
                Projectile.NewProjectile(proj.GetSource_OnHit(target), proj.Center, vector, ModContent.ProjectileType<PerfectFlailCorePro>(), (int)(proj.damage * 1.5), proj.knockBack, proj.owner);
            }
        }
    }
}