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
    public class DoubleFlail : ModItem
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
            player.GetModPlayer<DoubleFlailPlayer>().DoubleFlailAcc = true;
        }
        public override void AddRecipes()
        {
            Mod Calamity = ModLoader.GetMod("CalamityMod");
            Mod Thorium = ModLoader.GetMod("ThoriumMod");
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(
                Thorium.Find<ModItem>("IronFlailCore").Type, 2)
                .AddIngredient(Calamity.Find<ModItem>("AshesofCalamity").Type, 10)
                .AddIngredient(Calamity.Find<ModItem>("UnholyCore").Type, 5)
                .AddIngredient(Calamity.Find<ModItem>("CryonicBar").Type, 10)
                .AddIngredient(Calamity.Find<ModItem>("EssenceofEleum").Type, 2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    internal class DoubleFlailPlayer : ModPlayer
    {
        public bool DoubleFlailAcc;
        public override void ResetEffects()
        {
            DoubleFlailAcc = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            bool isFlail = false;
            if (proj.ModProjectile is FlailProBase || proj.ModProjectile is BaseMaceFlailProjectile || proj.aiStyle == ProjAIStyleID.Flail)
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
            if (DoubleFlailAcc && Utils.NextBool(Main.rand, 6) && isFlail == true)
            {
                SoundEngine.PlaySound(SoundID.Item1, proj.Center);
                Projectile.NewProjectile(proj.GetSource_OnHit(target), proj.Center, vector, ModContent.ProjectileType<HotFlailCorePro>(), (int)(proj.damage * 0.75), proj.knockBack, proj.owner);
            }
            if (DoubleFlailAcc && Utils.NextBool(Main.rand, 6) && isFlail == true)
            {
                SoundEngine.PlaySound(SoundID.Item1, proj.Center);
                Projectile.NewProjectile(proj.GetSource_OnHit(target), proj.Center, vector, ModContent.ProjectileType<ColdFlailCorePro>(), (int)(proj.damage * 0.75), proj.knockBack, proj.owner);
            }
        }
    }
}