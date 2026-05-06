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
    public class AbsoluteTVRemotePausePro : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly)
                    continue;

                if (Projectile.Hitbox.Intersects(npc.Hitbox))
                {
                    npc.AddBuff(ModContent.BuffType<TVRemotePaused>(), 600);

                    Projectile.netUpdate = true;
                    Projectile.Kill();
                }
            }

            foreach (Player player in Main.player)
            {
                if (!player.active || player.dead)
                    continue;

                if (Projectile.Hitbox.Intersects(player.Hitbox))
                {
                    if (player.whoAmI == Projectile.owner)
                        continue;

                    player.AddBuff(ModContent.BuffType<TVRemotePlayerPaused>(), 600);

                    Projectile.netUpdate = true;
                    Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D aura = ModContent.Request<Texture2D>(Texture).Value;

            Texture2D overlay = ModContent.Request<Texture2D>(
                "InfernalEclipseWeaponsDLC/Content/Projectiles/OtherPro/AbsoluteTVRemotePausePro"
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

            return false;
        }
    }
}
