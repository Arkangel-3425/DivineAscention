using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using InfernalEclipseWeaponsDLC.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.OtherPro
{
    public class AbsoluteTVRemoteFastForwardPro : ModProjectile
    {
        public override string Texture => "InfernalEclipseWeaponsDLC/Content/Projectiles/OtherPro/AbsoluteTVRemoteAura";

        public override void SetDefaults()
        {
            Projectile.width = 172;
            Projectile.height = 172;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;

            Projectile.tileCollide = false;

            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // Stick to player
            Projectile.Center = player.Center;
            Projectile.velocity = Vector2.Zero;

            float maxRadius = 200f;

            foreach (Projectile proj in Main.projectile)
            {
                if (!proj.active || proj.whoAmI == Projectile.whoAmI)
                    continue;

                var global = proj.GetGlobalProjectile<TimeSpeedGlobalProjectile>();

                global.affectedByTimeField = true;
                global.fieldCenter = player.Center;
                global.fieldRadius = maxRadius;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D aura = ModContent.Request<Texture2D>(Texture).Value;

            Texture2D overlay = ModContent.Request<Texture2D>(
                "InfernalEclipseWeaponsDLC/Content/Projectiles/OtherPro/AbsoluteTVRemoteFastForwardIcon"
            ).Value;

            Vector2 basePos = Projectile.Center - Main.screenPosition;

            Vector2 auraOrigin = aura.Size() / 2f;
            Vector2 overlayOrigin = overlay.Size() / 2f;

            // --- AURA (your current effect) ---
            Vector2 jitter = Main.rand.NextVector2Circular(2f, 2f);
            float flicker = 0.85f + Main.rand.NextFloat(0.15f);

            Main.EntitySpriteDraw(
                aura,
                basePos + jitter,
                null,
                Color.White * flicker,
                0f,
                auraOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            // --- HOVERING SPRITE ---
            float time = Main.GlobalTimeWrappedHourly;

            // smooth up/down motion
            float hoverOffset = (float)Math.Sin(time * 3f) * 2f;

            // slight rotation for life
            float rotation = (float)Math.Sin(time * 2f) * 0.05f;

            // slight flicker to match aura
            float overlayFlicker = 0.9f + Main.rand.NextFloat(0.1f);

            Vector2 overlayPos = basePos + new Vector2(0f, -160f + hoverOffset);

            Main.EntitySpriteDraw(
                overlay,
                overlayPos,
                null,
                Color.White * overlayFlicker,
                rotation,
                overlayOrigin,
                Projectile.scale * 0.8f,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }

    public class TimeSpeedGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool affectedByTimeField;
        public Vector2 fieldCenter;
        public float fieldRadius;

        public override void PostAI(Projectile projectile)
        {
            if (!affectedByTimeField)
                return;

            float distance = Vector2.Distance(projectile.Center, fieldCenter);

            if (distance > fieldRadius)
                return;

            float strength = 1f - (distance / fieldRadius);
            strength = MathHelper.Clamp(strength, 0f, 1f);

            // 1x at edge → up to 2.5x speed at center
            float speedMultiplier = MathHelper.Lerp(1f, 1.2f, strength);

            projectile.velocity *= speedMultiplier;
        }
    }
}
