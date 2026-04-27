using System;
using CalamityMod.Enums;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.Buffs.DamageOverTime;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.HealerPro.Scythes
{
    public class PlanetShaperStar : ModProjectile
    {
        public override string Texture => "InfernalEclipseWeaponsDLC/Assets/Textures/Sparkle";

        public override string GlowTexture => "Terraria/Images/Extra_98";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 50;
            Projectile.timeLeft = 240;
            Projectile.extraUpdates = 2;
            Projectile.alpha = (int)byte.MaxValue;
            Projectile.ArmorPenetration = 75;
        }

        public override void AI()
        {
            float radians = MathHelper.ToRadians(Projectile.ai[1] * 4f);
            if ((double)radians < 0.0)
                radians += 6.28318548f;
            else if ((double)radians > 6.2831854820251465)
                radians -= 6.28318548f;
            Color rgb = Main.hslToRgb(Math.Abs(radians / 6.28318548f) % 1f, 1f, 0.7f, byte.MaxValue);
            rgb.G = (byte)(0.5 * rgb.G);
            Lighting.AddLight(Projectile.Center, rgb.ToVector3());
            if ((double)++Projectile.localAI[2] > ProjectileID.Sets.TrailCacheLength[Type])
            {
                GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, rgb * 0.9f, 15, (float)((double)Utils.NextFloat(Main.rand, 0.4f, 0.7f) * Projectile.scale * 0.800000011920929), 0.8f, Utils.NextFloat(Main.rand, -0.03f, 0.03f), true, 0.01f, true, false), false, new GeneralDrawLayer?());
                Projectile.localAI[2] -= Projectile.MaxUpdates;
            }
            if (Projectile.timeLeft < 51)
                Projectile.alpha += 5;
            else if (Projectile.alpha > 0)
                Projectile.alpha -= 15;
            if (Projectile.velocity.Length() < (double)Projectile.ai[0])
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * Projectile.ai[0];
            Projectile projectile = Projectile;
            projectile.position += Utils.ToRotationVector2(Utils.ToRotation(Projectile.velocity) + Utils.RotatedBy(Vector2.UnitY, (double)MathHelper.ToRadians(Projectile.ai[1] * 4f), new Vector2()).X * 0.7853982f * Projectile.ai[2]) * Projectile.velocity.Length();
            if ((double)++Projectile.ai[1] < 90.0)
                return;
            this.Projectile.ai[1] = 0.0f;
        }

        private bool HomeInOnTarget(Projectile projectile, float maxVelocity, float velocityWeight = 0.04761905f)
        {
            NPC closest = null;
            float sqrMaxDistance = 1000f * 1000f;

            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                if (target.CanBeChasedBy())
                {
                    float sqrDistance = Vector2.DistanceSquared(target.Center, Projectile.Center);
                    if (sqrDistance < sqrMaxDistance)
                    {
                        sqrMaxDistance = sqrDistance;
                        closest = target;

                        Vector2 vector2_1 = closest.Center - projectile.Center;
                        Vector2 vector2_2 = vector2_1 * maxVelocity / ((Vector2)vector2_1).Length();
                        Projectile projectile1 = projectile;
                        projectile1.velocity *= 1f - velocityWeight;
                        Projectile projectile2 = projectile;
                        projectile2.velocity += vector2_2 * velocityWeight;

                        return true;
                    }
                }
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ElementalMix>(), 120);
            if (ModLoader.TryGetMod("CatalystMod", out Mod mod))
                return;
            target.AddBuff(mod.Find<ModBuff>("AstralBlight").Type, 120);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad).Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>(GlowTexture, AssetRequestMode.ImmediateLoad).Value;

            float opacity = MathHelper.Lerp(0.5f, 0f, Projectile.alpha / 255f);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPosition = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;

                float hueRotation = MathHelper.ToRadians(Projectile.ai[1] * 4f + i * 12f);
                hueRotation = MathHelper.WrapAngle(hueRotation);
                if (hueRotation < 0f)
                    hueRotation += MathHelper.TwoPi;

                Color trailColor = Main.hslToRgb(hueRotation / MathHelper.TwoPi, 1f, 0.66f) * opacity;
                trailColor.G = (byte)(trailColor.G * 0.5f);
                trailColor.A = 0;

                if (i == 0)
                {
                    Main.EntitySpriteDraw(
                        texture,
                        drawPosition,
                        null,
                        trailColor * 2.5f,
                        Main.GlobalTimeWrappedHourly * MathHelper.Pi,
                        texture.Size() * 0.5f,
                        Projectile.scale,
                        SpriteEffects.None,
                        0f
                    );

                    Main.EntitySpriteDraw(
                        texture,
                        drawPosition,
                        null,
                        new Color(255, 255, 255, 0),
                        Main.GlobalTimeWrappedHourly * MathHelper.Pi,
                        texture.Size() * 0.5f,
                        Projectile.scale * 0.6f,
                        SpriteEffects.None,
                        0f
                    );
                }
                else
                {
                    Vector2 trailDirection = Projectile.oldPos[i] - Projectile.oldPos[i - 1];

                    Main.EntitySpriteDraw(
                        glowTexture,
                        drawPosition,
                        null,
                        trailColor,
                        trailDirection.ToRotation() + MathHelper.PiOver2,
                        glowTexture.Size() * 0.5f,
                        new Vector2(0.8f, Projectile.scale),
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool flag = false;
            for (int index = 0; index < Projectile.oldPos.Length; ++index)
                flag |= ((Rectangle)targetHitbox).Intersects(new Rectangle((int)Projectile.oldPos[index].X, (int)Projectile.oldPos[index].Y, projHitbox.Width, projHitbox.Height));
            return new bool?(((Rectangle)projHitbox).Intersects(targetHitbox) | flag);
        }

        public override bool ShouldUpdatePosition() => false;
    }
}
