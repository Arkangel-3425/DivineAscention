using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using ThoriumMod;
using CalamityMod.Buffs.DamageOverTime;
using System;
using Terraria.Graphics.Shaders;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.HealerPro.Scythes
{
    public class PlanetShaperFireball : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.BallofFire}";

        private const int TrailLength = 12;
        private Vector2[] trailPositions = new Vector2[TrailLength];
        private bool trailInitialized = false;

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.ArmorPenetration = 125;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 28;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            if (!trailInitialized)
            {
                for (int i = 0; i < TrailLength; i++)
                    trailPositions[i] = Projectile.Center;
                trailInitialized = true;
            }

            for (int i = TrailLength - 1; i > 0; i--)
                trailPositions[i] = trailPositions[i - 1];
            trailPositions[0] = Projectile.Center;
            //Projectile.rotation += MathHelper.Pi;

            if (Projectile.ai[1] == 0.0 && Main.myPlayer == Projectile.owner)
            {
                float num = 960f;
                for (int index = 0; index < Main.maxNPCs; ++index)
                {
                    if (Main.npc[index].CanBeChasedBy(this, false) && (double)Main.npc[index].Distance(Main.MouseWorld) < (double)num)
                    {
                        num = (Main.npc[index]).Distance(Main.MouseWorld);
                        Projectile.ai[1] = index + 1;
                    }
                }
                if (Projectile.ai[1] > 0.0)
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
            }
            else if (Projectile.ai[1] > 0.0)
            {
                if (!Main.npc[(int)Projectile.ai[1] - 1].CanBeChasedBy(this, false))
                    Projectile.ai[0] = Projectile.ai[1] = 0.0f;
                else
                    Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 120f, 1f))) * ((Vector2)Projectile.velocity).Length();
            }
            int num1 = 6;
            if (ModLoader.TryGetMod("CatalystMod", out Mod mod))
                num1 = mod.Find<ModDust>("MonoDust2").Type;
            if (Projectile.timeLeft % Projectile.MaxUpdates == 0)
            {
                for (int index = 0; index < 2; ++index)
                {
                    if (!Utils.NextBool(Main.rand, 3))
                    {
                        float num2 = index * 0.5f;
                        Dust dust1 = Dust.NewDustPerfect(Projectile.Center + Utils.NextVector2Circular(Main.rand, Projectile.width / 2, Projectile.height / 2) + Projectile.velocity * num2, num1, new Vector2?(), 0, new Color(), 1f);
                        dust1.noGravity = true;
                        Dust dust2 = dust1;
                        dust2.velocity *= 0.1f;
                        dust1.scale = 0.9f;
                        dust1.fadeIn = 0.1f;
                        dust1.alpha = 100;
                        dust1.color = !Utils.NextBool(Main.rand, 3) ? new Color(byte.MaxValue, 233, 2, 50) : new Color(220, 95, 210, 50);
                        if (Projectile.oldVelocity != Vector2.Zero)
                        {
                            Dust dust3 = dust1;
                            dust3.velocity = dust3.velocity - Vector2.Normalize(Projectile.oldVelocity);
                        }
                    }
                }
            }
            Projectile.rotation += Projectile.velocity.X < 0.0 ? -1f : 1f;

            // Deep orange embers
            if (Main.rand.NextBool(2))
            {
                Dust fire = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(6f, 6f),
                    DustID.Torch,
                    -Projectile.velocity * 0.15f + Main.rand.NextVector2Circular(1.5f, 1.5f),
                    80, default, Main.rand.NextFloat(1.0f, 1.8f));
                fire.noGravity = true;
            }

            // Dark smoke trail
            if (Main.rand.NextBool(3))
            {
                Dust smoke = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(4f, 4f),
                    DustID.Smoke,
                    -Projectile.velocity * 0.1f + new Vector2(0, Main.rand.NextFloat(-1f, -0.3f)),
                    120, new Color(50, 25, 10), Main.rand.NextFloat(1.2f, 2f));
                smoke.noGravity = true;
            }

            // Falling sparks
            if (Main.rand.NextBool(5))
            {
                Dust ember = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Torch,
                    -Projectile.velocity * 0.05f + Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.5f, 2f),
                    0, default, Main.rand.NextFloat(0.6f, 1.2f));
                ember.noGravity = false;
            }

            // Deep warm lighting
            Lighting.AddLight(Projectile.Center, 0.8f, 0.3f, 0.05f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.ai[0] = 0.0f;
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
            }
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 240, false);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 6f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, vel, 0, default, Main.rand.NextFloat(1f, 1.8f));
                d.noGravity = true;
            }

            for (int i = 0; i < 5; i++)
            {
                Dust smoke = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke,
                    Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 3f),
                    120, new Color(50, 25, 10), Main.rand.NextFloat(1.5f, 2.5f));
                smoke.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor)
        {
            if (!trailInitialized)
                return false;

            SpriteBatch sb = Main.spriteBatch;
            Texture2D vortexNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
            Vector2 vortexOrigin = vortexNoise.Size() * 0.5f;
            float time = Main.GlobalTimeWrappedHourly;

            // === Trail vortexes ===
            Main.spriteBatch.EnterShaderRegion();

            for (int i = TrailLength - 1; i >= 1; i--)
            {
                Vector2 pos = trailPositions[i];
                if (pos == trailPositions[0] && i > 1)
                    continue;

                float t = i / (float)TrailLength;
                float alpha = (1f - t);
                alpha *= alpha;

                float scale = MathHelper.Lerp(0.15f, 0.06f, t);
                float spinSpeed = MathHelper.Lerp(1.5f, 0.6f, t);

                GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(alpha * 0.5f);
                GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(220, 10, 5));
                GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(255, 60, 15));
                GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

                for (int j = 0; j < 3; j++)
                {
                    float angle = MathHelper.TwoPi * j / 3f - time * MathHelper.TwoPi * spinSpeed;
                    Color drawColor = Color.White * alpha * 0.7f;
                    drawColor.A = 0;
                    Vector2 drawPosition = pos - Main.screenPosition + angle.ToRotationVector2() * 2f;

                    Main.EntitySpriteDraw(vortexNoise, drawPosition, null, drawColor,
                        angle + MathHelper.PiOver2, vortexOrigin,
                        scale, SpriteEffects.None, 0);
                }
            }

            Main.spriteBatch.ExitShaderRegion();

            // === Head vortex ===
            Main.spriteBatch.EnterShaderRegion();

            float pulse = 1f + (float)Math.Sin(time * 10f) * 0.12f;

            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(0.8f);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(220, 20, 5));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(255, 70, 20));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f - time * MathHelper.TwoPi * 1.4f;
                Color drawColor = Color.White * 0.85f;
                drawColor.A = 0;
                Vector2 drawPosition = Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * 3f;

                Main.EntitySpriteDraw(vortexNoise, drawPosition, null, drawColor,
                    angle + MathHelper.PiOver2, vortexOrigin,
                    0.25f * pulse, SpriteEffects.None, 0);
            }

            Main.spriteBatch.ExitShaderRegion();

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            bool flag = false;
            for (int index = 0; index < Projectile.oldPos.Length; ++index)
                flag |= ((Rectangle)targetHitbox).Intersects(new Rectangle((int)Projectile.oldPos[index].X, (int)Projectile.oldPos[index].Y, projHitbox.Width, projHitbox.Height));
            return new bool?(((Rectangle)projHitbox).Intersects(targetHitbox) | flag);
        }
    }
}
