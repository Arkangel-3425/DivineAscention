using CalamityMod.Graphics.Metaballs;
using CalamityMod.Particles;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using InfernalEclipseWeaponsDLC.Core.Graphics.Metaballs;
using System;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.MeleePro
{
    public class TerraOrb : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public NPC target = null;

        public int spawnCooldown = 60; //how many frames must pass before it homes into the nearest target

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 60 * 5 * Projectile.MaxUpdates;
            Projectile.friendly = true;

            spawnCooldown *= Projectile.MaxUpdates;
        }
        public override void AI()
        {
            bool finalUpdate = Projectile.FinalExtraUpdate();

            var p = TerraMetaball.SpawnParticle(Projectile.Center + Projectile.velocity, Main.rand.NextVector2Circular(-0.5f, -0.5f), Projectile.width);
            p.SizeScaling = 0.75f;
            p.ShrinkDelay = 1;

            float maxDistanceSq = 200f * 200f;

            if (spawnCooldown > 0)
            {
                PassiveBehavior();
                spawnCooldown--;
                return;
            }

            if (target == null)
            {
                PassiveBehavior();

                target = Projectile.FindTargetWithinRange(maxDistanceSq);
            }
            else Home();
        }

        public void PassiveBehavior()
        {
            Projectile.velocity *= 0.99f;
        }

        public void Home()
        {
            Vector2 targetVector = target.Center - Projectile.Center;
            float playerDist = targetVector.Length();

            Projectile.velocity = (Projectile.velocity * 5 + (targetVector.SafeNormalize(Vector2.Zero) * 5)) / 6f;
        }

        public override bool? CanDamage() => spawnCooldown <= 0;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            int healed = Math.Max(1, (int)((player.statLifeMax2 - player.statLife) * 0.03f));
            player.statLife += healed;
            player.HealEffect(healed);

            Particle ring = new CustomPulse(target.Center, Vector2.Zero, new Color(0, 255, 120) * 0.75f, "CalamityMod/Particles/DustyCircleHardEdge", Vector2.One, 0, 0.01f, 0.05f, 20);
            GeneralParticleHandler.SpawnParticle(ring);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
