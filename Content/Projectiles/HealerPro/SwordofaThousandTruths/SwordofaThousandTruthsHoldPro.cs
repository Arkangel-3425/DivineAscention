using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using InfernalEclipseWeaponsDLC.Content.Buffs;
using ReLogic.Content;
using CalamityMod.Buffs.StatDebuffs;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.HealerPro.ExecutionersSword
{
    public class SwordofaThousandTruthsHoldPro : ModProjectile
    {
        private bool stuck = false;
        private int stuckTarget = -1;
        private Vector2 offsetFromNPC;
        public bool firedRight = true;

        public ref float State => ref Projectile.ai[0];
        public ref float Timer => ref Projectile.ai[1];
        public ref float StuckTimer => ref Projectile.ai[2];

        public override string Texture => "InfernalEclipseWeaponsDLC/Content/Items/Weapons/Healer/Melee/SwordofaThousandTruths";

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;

            Projectile.scale = 1.25f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.scale = player.GetAdjustedItemScale(player.HeldItem);

            if (!stuck)
            {
                if (State == 0f) // AIM
                {
                    if (!player.controlUseTile)
                    {
                        if (Timer >= 60f)
                        {
                            Fire(player);
                        }
                        else
                        {
                            Projectile.Kill();
                        }
                        return;
                    }

                    HandleAim(player);
                    HandleCharge();
                }
                else // FIRE
                {
                    DoFireBehavior();
                }
            }
            else
            {
                if (stuckTarget > -1 && Main.npc[stuckTarget].active && Main.npc[stuckTarget].dontTakeDamage == false)
                {
                    // Follow the NPC with a fixed offset
                    Projectile.Center = Main.npc[stuckTarget].Center + offsetFromNPC;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.tileCollide = false;

                    // --- Alternate projectile spawn logic ---
                    Projectile.ai[2]++; // timer
                    if (Projectile.ai[2] >= 10) // half second
                    {
                        Projectile.ai[2] = 0;

                        if (Projectile.owner == Main.myPlayer)
                        {
                            int projType;
                            Vector2 randDir;

                            // alternate between dark/light using ai[1]
                            if (Projectile.ai[1] == 0)
                            {
                                projType = ModContent.ProjectileType<ExecutionersSwordDarkEnergy>();
                                Projectile.ai[1] = 1;

                                randDir = Main.rand.NextVector2Unit() * Main.rand.NextFloat(10f, 18f);

                                SoundEngine.PlaySound(SoundID.Item103, Projectile.position);
                            }
                            else
                            {
                                projType = ModContent.ProjectileType<ExecutionersSwordLightEnergy>();
                                Projectile.ai[1] = 0;

                                randDir = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 10f);

                                SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                            }

                            Projectile.NewProjectile(
                                Projectile.GetSource_FromAI(),
                                Projectile.Center,
                                randDir,
                                projType,
                                Projectile.damage,
                                Projectile.knockBack,
                                Projectile.owner,
                                ai1: 1
                            );
                        }
                    }

                }
                else
                {
                    Projectile.Kill();
                }
            }
        }

        void DoFireBehavior()
        {
            Player player = Main.player[Projectile.owner];

            if (firedRight)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            }
                
            Lighting.AddLight(Projectile.Center, 0f, 0f, 0.8f);

            //Projectile.velocity *= 0.95f;
        }

        void HandleAim(Player player)
        {
            Vector2 dir = player.DirectionTo(Main.MouseWorld);
            player.ChangeDir(dir.X > 0f ? 1 : -1);

            float animationCompletion = MathHelper.Clamp(Timer / 60f, 0f, 1f);

            // Properly flipped pullback: use player.direction as a multiplier in the rotated vector
            float pullbackDistance = MathHelper.Lerp(10f, 0f, animationCompletion);
            Vector2 pullbackOffset = -dir.SafeNormalize(Vector2.Zero) * pullbackDistance;

            Projectile.rotation = dir.ToRotation() + MathHelper.PiOver4;

            float frontArmRotation = Projectile.rotation - MathHelper.PiOver4 - animationCompletion * player.direction * 0.74f;
            if (player.direction == 1)
                frontArmRotation += MathHelper.Pi;

            // Position the sword properly with pullback applied
            Projectile.Center = player.Center
                + (frontArmRotation + MathHelper.PiOver2).ToRotationVector2() * Projectile.scale * 16f
                + (dir * Projectile.scale * 40f) + pullbackOffset * Projectile.scale;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, frontArmRotation);

            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2);

            Projectile.spriteDirection = player.direction;

            if (player.direction == -1)
                Projectile.rotation += MathHelper.PiOver2;
        }

        void HandleCharge()
        {
            Timer++;

            Projectile.timeLeft = 180;

            if (Timer == 20f)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);

                for (int i = 0; i < 10; i++)
                {
                    Vector2 offset = Main.rand.NextVector2CircularEdge(Projectile.width / 2f, Projectile.height / 2f);
                    Vector2 spawnPos = Projectile.Center;
                    Vector2 vel = offset.SafeNormalize(Vector2.UnitY) * 6f;

                    Dust dust = Dust.NewDustPerfect(spawnPos, DustID.BlueTorch, vel, 150, Color.White, 1.5f);
                    dust.noGravity = true;
                }
            }
            if (Timer == 40f)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);

                for (int i = 0; i < 20; i++)
                {
                    Vector2 offset = Main.rand.NextVector2CircularEdge(Projectile.width / 2f, Projectile.height / 2f);
                    Vector2 spawnPos = Projectile.Center;
                    Vector2 vel = offset.SafeNormalize(Vector2.UnitY) * 6f;

                    Dust dust = Dust.NewDustPerfect(spawnPos, DustID.BlueTorch, vel, 150, Color.White, 1.5f);
                    dust.noGravity = true;
                }
            }
            if (Timer == 60f)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);

                for (int i = 0; i < 30; i++)
                {
                    Vector2 offset = Main.rand.NextVector2CircularEdge(Projectile.width / 2f, Projectile.height / 2f);
                    Vector2 spawnPos = Projectile.Center;
                    Vector2 vel = offset.SafeNormalize(Vector2.UnitY) * 6f;

                    Dust dust = Dust.NewDustPerfect(spawnPos, DustID.BlueTorch, vel, 150, Color.White, 1.5f);
                    dust.noGravity = true;
                }
            }
        }

        void Fire(Player player)
        {
            Vector2 dir = player.DirectionTo(Main.MouseWorld);

            float chargeMult = MathHelper.Clamp(Timer / 60f, 0.5f, 2f);

            Projectile.velocity = dir * (50f * chargeMult);
            Projectile.friendly = true;
            Projectile.tileCollide = true;

            State = 1f;
            Projectile.netUpdate = true;

            if (player.direction == 1)
            {
                firedRight = true;
            }
            else
            {
                firedRight = false;
            }

            SoundEngine.PlaySound(SoundID.Item1, player.position);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!stuck)
            {
                stuck = true;
                stuckTarget = target.whoAmI;
                Projectile.velocity = Vector2.Zero;
                Projectile.netUpdate = true;
                target.AddBuff(ModContent.BuffType<SwordStuckBuff>(), 720);
                Projectile.timeLeft = 720;
                target.AddBuff(ModContent.BuffType<Voidfrost>(), 720);

                Player owner = Main.player[Projectile.owner];
                Player healer = owner;

                if (healer.GetModPlayer<ThoriumPlayer>().healBonus >= 5)
                {
                    target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 720);
                    target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 720);
                    target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 720);
                    target.AddBuff(BuffID.BetsysCurse, 720);
                    target.AddBuff(BuffID.Ichor, 720);
                }

                SoundEngine.PlaySound(SoundID.Item71, Projectile.position);

                // Store the initial offset from the NPC's center
                offsetFromNPC = Projectile.Center - target.Center;

                for (int i = 0; i < 10; i++)
                {
                    Vector2 offset = Main.rand.NextVector2CircularEdge(target.width / 2f, target.height / 2f);
                    Vector2 spawnPos = target.Center + offset;
                    Vector2 vel = offset.SafeNormalize(Vector2.UnitY) * 6f;

                    Dust dust = Dust.NewDustPerfect(spawnPos, DustID.BlueTorch, vel, 150, Color.White, 1.5f);
                    dust.noGravity = true;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 offset = Main.rand.NextVector2CircularEdge(target.width / 2f, target.height / 2f);
                    Vector2 spawnPos = target.Center + offset;
                    Vector2 vel = offset.SafeNormalize(Vector2.UnitY) * 2f;

                    Dust dust = Dust.NewDustPerfect(spawnPos, DustID.BlueTorch, vel, 150, Color.White, 1.5f);
                    dust.noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            float chargeInterpolant = MathHelper.Clamp(Timer / 60f, 0f, 1f);
            Color baseColor = Projectile.GetAlpha(lightColor) * (1f);

            Texture2D spearTexture = ModContent.Request<Texture2D>(
                "InfernalEclipseWeaponsDLC/Content/Items/Weapons/Healer/Melee/SwordofaThousandTruths",
                AssetRequestMode.ImmediateLoad
            ).Value;

            Rectangle frame = Utils.Frame(spearTexture, 1, Main.projFrames[Projectile.type], 0, Projectile.frame, 0, 0);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = Utils.Size(frame) * 0.5f;

            /*
            // Backglow effect
            float backglowWidth = chargeInterpolant * 2f;
            if (backglowWidth > 0.5f)
            {
                Color backglowColor = Color.IndianRed;
                backglowColor = Color.Lerp(backglowColor, Color.Wheat, chargeInterpolant * 0.56f) * 0.4f;
                backglowColor.A = 20;

                // Decide glow rotation: rotate 90° if facing left and charging
                float glowRotation = Projectile.rotation;
                if (Projectile.spriteDirection == -1 && State == 0f)
                    glowRotation -= MathHelper.PiOver2; // rotate only the glow

                for (int i = 0; i < 10; i++)
                {
                    Vector2 offset = (MathHelper.TwoPi * i / 10f).ToRotationVector2() * backglowWidth;
                    Main.spriteBatch.Draw(spearTexture, drawPosition + offset, frame, backglowColor,
                        glowRotation, origin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            */

            // Determine sprite flipping based on player direction
            SpriteEffects direction = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.spriteBatch.Draw(spearTexture, drawPosition, frame, baseColor, Projectile.rotation, origin, Projectile.scale, direction, 0f);

            return false;
        }
    }
}
