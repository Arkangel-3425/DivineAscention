using CalamityMod;
using CalamityMod.DataStructures;
using CalamityMod.Dusts;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Melee;
using InfernalEclipseWeaponsDLC.Core.NewFolder;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.MeleePro.Void
{
    public class GauntletofAnnihilationPunches : PhosphorescentGauntletPunches
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.DamageType = ModLoader.TryGetMod("SOTS", out Mod sots) ? sots.Find<DamageClass>("VoidMelee") : ModContent.GetInstance<TrueMeleeDamageClass>();
        }

        // reused from Calamity Mod
        internal void ReelBack()
        {
            // 17APR2024: Ozzatron: Phosphorescent Gauntlet gives iframes when striking enemies in a similar manner to a bonk dash.
            // This is a fixed and intentionally very low number of iframes, and is not boosted by Cross Necklace.
            Owner.GiveUniversalIFrames(PhosphorescentGauntlet.OnHitIFrames);

            // Create some visual effects.
            if (!Main.dedServ)
            {
                Vector2 topLeft = Projectile.Center + Projectile.velocity.RotatedBy(-MathHelper.PiOver2) * 40f;
                Vector2 top = Projectile.Center + Projectile.velocity * 70f;
                Vector2 topRight = Projectile.Center + Projectile.velocity.RotatedBy(MathHelper.PiOver2) * 40f;
                foreach (Vector2 spawnPosition in new BezierCurve(topLeft, top, topRight).GetPoints(50))
                {
                    Dust sulphurousAcid = Dust.NewDustPerfect(spawnPosition + Projectile.velocity * 16f, (int)CalamityDusts.PurpleCosmilite);
                    sulphurousAcid.velocity = Projectile.velocity * 4f;
                    sulphurousAcid.noGravity = true;
                    sulphurousAcid.scale = 1.2f;
                }
            }
            if (Main.myPlayer != Projectile.owner)
                return;

            // Reel back after collision.
            Owner.velocity = Vector2.Reflect(Owner.velocity.SafeNormalize(Vector2.Zero), Projectile.velocity.SafeNormalize(Vector2.Zero)) * Owner.velocity.Length();

            Owner.GetModPlayer<InfernalWeaponsPlayer>().annihilationBonusShotTimeLeft = 3;

            // Create on-hit tile dust.
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width + 16, Projectile.height + 16);

            Projectile.Kill();
        }

        internal static void GenerateDustOnOwnerHand(Player player)
        {
            if (Main.dedServ)
                return;

            Vector2 handOffset = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
                handOffset.X = player.bodyFrame.Width - handOffset.X;
            if (player.gravDir != 1f)
                handOffset.Y = player.bodyFrame.Height - handOffset.Y;

            handOffset -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - player.height) / 2f;
            Vector2 rotatedHandPosition = player.RotatedRelativePoint(player.position + handOffset, true);
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(player.Center, 0, 0, (int)CalamityDusts.PurpleCosmilite, 0f, 0f, 150, default, 1.3f);
                dust.position = rotatedHandPosition;
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.velocity += player.velocity;
                if (Main.rand.NextBool())
                {
                    dust.position += Utils.RandomVector2(Main.rand, -4f, 4f);
                    dust.scale += Main.rand.NextFloat();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ReelBack();
        }
    }
}
