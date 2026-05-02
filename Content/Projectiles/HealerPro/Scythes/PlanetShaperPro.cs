using System.IO;
using CalamityMod.Buffs.DamageOverTime;
using InfernalEclipseWeaponsDLC.Content.Items.Weapons.Healer.Melee.Scythes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.HealerPro.Scythes
{
    public class PlanetShaperPro : ScythePro
    {
        private bool shouldSpin;

        public override string Texture => "InfernalEclipseWeaponsDLC/Content/Items/Weapons/Healer/Melee/Scythes/PlanetShaper";

        public override void SafeSetDefaults()
        {
            Projectile.width = 75;
            Projectile.height = 98;
            Projectile.idStaticNPCHitCooldown = 4;
            Projectile.ArmorPenetration = 125;
            Projectile.alpha = byte.MaxValue;
            Projectile.manualDirectionChange = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.scale = player.GetAdjustedItemScale(player.HeldItem);
            if (Projectile.ai[1] <= 0.0 || player.dead)
            {
                Projectile.Kill();
                return false;
            }
            Projectile.timeLeft = (int)Projectile.ai[1];
            player.itemTime = (int)Projectile.ai[1];
            player.itemAnimation = (int)Projectile.ai[1];
            player.heldProj = Projectile.whoAmI;
            player.compositeFrontArm.enabled = true;
            if (Projectile.velocity.X != 0.0)
                player.ChangeDir(Projectile.velocity.X > 0.0 ? 1 : -1);
            float num1;
            float num2;
            if (Projectile.direction == -1)
            {
                if (Projectile.ai[1] / Projectile.ai[0] > 0.75)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        if (player.HeldItem.ModItem is PlanetShaper modItem)
                            shouldSpin = modItem.spin >= 3;
                        Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
                        NetMessage.SendData(MessageID.SyncProjectile);
                    }
                    float num3 = (float)((Projectile.ai[1] / Projectile.ai[0] - 0.75) * 4.0);
                    num1 = Utils.ToRotation(Projectile.velocity) + MathHelper.ToRadians(MathHelper.SmoothStep(135f, 80f, num3) * player.direction);
                    num2 = Utils.ToRotation(Projectile.velocity) + MathHelper.Lerp(1.57079637f, 0.7853982f, num3) * player.direction;
                }
                else if (shouldSpin)
                {
                    float num4 = MathHelper.SmoothStep(0.0f, 1f, Projectile.ai[1] / (Projectile.ai[0] * 0.75f));
                    num1 = MathHelper.Lerp(MathHelper.ToRadians(100f) * (float)-((Entity)player).direction, MathHelper.ToRadians(495f) * (float)((Entity)player).direction, num4) + Utils.ToRotation(Projectile.velocity);
                    num2 = MathHelper.Lerp(1.57079637f * (float)-((Entity)player).direction, 7.853982f * (float)((Entity)player).direction, num4) + Utils.ToRotation(Projectile.velocity);
                }
                else
                {
                    float num5 = Projectile.ai[1] / (Projectile.ai[0] * 0.75f);
                    for (int index = 0; index < 4; ++index)
                        num5 = MathHelper.SmoothStep(0.0f, 1f, num5);
                    num1 = MathHelper.Lerp(MathHelper.ToRadians(100f) * (float)-((Entity)player).direction, MathHelper.ToRadians(135f) * (float)((Entity)player).direction, num5) + Utils.ToRotation(Projectile.velocity);
                    num2 = MathHelper.Lerp(1.57079637f * (float)-((Entity)player).direction, 1.57079637f * (float)((Entity)player).direction, num5) + Utils.ToRotation(Projectile.velocity);
                }
                Projectile.spriteDirection = -player.direction;
            }
            else
            {
                if (Projectile.ai[1] / Projectile.ai[0] > 0.75)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        if (player.HeldItem.ModItem is PlanetShaper modItem)
                            shouldSpin = modItem.spin >= 3;
                        Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
                        NetMessage.SendData(MessageID.SyncProjectile);
                    }
                    float num6 = (float)((Projectile.ai[1] / Projectile.ai[0] - 0.75) * 4.0);
                    num1 = Utils.ToRotation(Projectile.velocity) - MathHelper.ToRadians(MathHelper.SmoothStep(135f, 80f, num6) * player.direction);
                    num2 = Utils.ToRotation(Projectile.velocity) - MathHelper.Lerp(1.57079637f, 0.7853982f, num6) * player.direction;
                }
                else if (shouldSpin)
                {
                    float num7 = MathHelper.SmoothStep(0.0f, 1f, Projectile.ai[1] / (Projectile.ai[0] * 0.75f));
                    num1 = MathHelper.Lerp(MathHelper.ToRadians(100f) * player.direction, MathHelper.ToRadians(495f) * -player.direction, num7) + Utils.ToRotation(Projectile.velocity);
                    num2 = MathHelper.Lerp(1.57079637f * player.direction, 7.853982f * -player.direction, num7) + Utils.ToRotation(Projectile.velocity);
                }
                else
                {
                    float num8 = Projectile.ai[1] / (Projectile.ai[0] * 0.75f);
                    for (int index = 0; index < 4; ++index)
                        num8 = MathHelper.SmoothStep(0.0f, 1f, num8);
                    num1 = MathHelper.Lerp(MathHelper.ToRadians(100f) * player.direction, MathHelper.ToRadians(135f) * -player.direction, num8) + Utils.ToRotation(Projectile.velocity);
                    num2 = MathHelper.Lerp(1.57079637f * player.direction, 1.57079637f * -player.direction, num8) + Utils.ToRotation(Projectile.velocity);
                }
                Projectile.spriteDirection = player.direction;
            }
            if (Projectile.localAI[2] == 0.0 && Projectile.ai[1] <= Projectile.ai[0] * 0.5)
            {
                bool flag = false;
                if (player.HeldItem.ModItem is PlanetShaper modItem)
                    flag = modItem.spin < 2 && !shouldSpin;
                if (Main.myPlayer == player.whoAmI)
                {
                    if (shouldSpin)
                    {
                        for (int index = 0; index < 6; ++index)
                        {
                            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center + (Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), (double)MathHelper.ToRadians(60f) * index, new Vector2()) * Projectile.height), Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), (double)MathHelper.ToRadians(60f) * index, new Vector2()) * 16f, ModContent.ProjectileType<PlanetShaperStar>(), Projectile.damage / 3, Projectile.knockBack, player.whoAmI, 0.0f, 0.0f, 0.0f), 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), Projectile.Center + (Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), (double)MathHelper.ToRadians(60f) * index, new Vector2()) * Projectile.height), Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), (double)MathHelper.ToRadians(60f) * index, new Vector2()) * 16f, ModContent.ProjectileType<PlanetShaperFireball>(), Projectile.damage / 3, Projectile.knockBack, player.whoAmI, 0.0f, 0.0f, 0.0f), 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        }
                    }
                    else if (flag)
                    {
                        for (int index = -1; index <= 1; ++index)
                        {
                            if (index != 0)
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem, null), Projectile.Center + Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), 0.39269909262657166 * index, new Vector2()) * Projectile.height, Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), 0.19634954631328583 * index, new Vector2()) * 16f, ModContent.ProjectileType<PlanetShaperFireball>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0.0f, 0.0f, 0.0f), 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            else
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem, null), Projectile.Center + Vector2.Normalize(Projectile.velocity * Projectile.height), Vector2.Normalize(Projectile.velocity) * 16f, ModContent.ProjectileType<PlanetShaperStar>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0.0f, 0.0f, 0.0f), 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        }
                    }
                    else
                    {
                        for (int index = -1; index <= 1; ++index)
                        {
                            if (index != 0)
                            {
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem, null), Projectile.Center + Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), 0.39269909262657166 * index, new Vector2()) * Projectile.height, Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), 0.19634954631328583 * index, new Vector2()) * 16f, ModContent.ProjectileType<PlanetShaperStar>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0.0f, 0.0f, 0.0f), 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem, null), Projectile.Center + Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), 0.39269909262657166 * index, new Vector2()) * Projectile.height, Utils.RotatedBy(Vector2.Normalize(Projectile.velocity), 0.19634954631328583 * index, new Vector2()) * 16f, ModContent.ProjectileType<PlanetShaperFireball>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0.0f, 0.0f, 0.0f), 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            }
                            else
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem, null), Projectile.Center + Vector2.Normalize(Projectile.velocity * Projectile.height), Vector2.Normalize(Projectile.velocity) * 16f, ModContent.ProjectileType<PlanetShaperFireball>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0.0f, 0.0f, 0.0f), 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        }
                    }
                }
                SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, new Vector2?(player.position));
                if (shouldSpin)
                    for (int index = 0; index < 6; ++index)
                    {
                        SoundEngine.PlaySound(new("CalamityMod/Sounds/Custom/DoGFireball"), new Vector2?(player.position));
                        SoundEngine.PlaySound(SoundID.Item105, new Vector2?(player.position));
                    }
                else
                {
                    SoundEngine.PlaySound(new("CalamityMod/Sounds/Custom/DoGFireball"), new Vector2?(player.position));
                    SoundEngine.PlaySound(SoundID.Item9, new Vector2?(player.position));
                }
                ++Projectile.localAI[2];
            }
            if (Projectile.alpha > 0)
                Projectile.alpha -= 17;
            player.compositeFrontArm.rotation = (float)((double)num2 - 1.5707963705062866 - (player.gravDir - 1.0) * 1.5707963705062866);
            Projectile.Center = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation);
            Projectile.rotation = num1;
            if (Projectile.ai[1] > 0.0)
                --Projectile.ai[1];
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;

            Texture2D texture = ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad).Value;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation + MathHelper.PiOver2 - MathHelper.ToRadians(Projectile.spriteDirection * 15f),
                new Vector2(texture.Width / 2f - Projectile.spriteDirection * 16f, texture.Height - 12f),
                Projectile.scale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f
            );

            if (Projectile.ai[1] / Projectile.ai[0] < 0.75f)
            {
                Player player = Main.player[Projectile.owner];

                float blurProgress = Projectile.ai[1] / (Projectile.ai[0] * 0.75f);

                if (shouldSpin)
                {
                    blurProgress = MathHelper.SmoothStep(0f, 1f, blurProgress);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                        blurProgress = MathHelper.SmoothStep(0f, 1f, blurProgress);
                }

                float blurOpacity = Vector2.UnitX.RotatedBy(blurProgress * MathHelper.Pi).Y - 0.3f;

                if (blurOpacity < 0f)
                    blurOpacity = 0f;

                SpriteEffects blurEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;

                Color blurColor = Color.Lerp(Color.Purple, Color.PaleVioletRed, blurOpacity) * 0.8f;
                blurColor.A = 0;

                Vector2 blurPosition =
                    player.MountedCenter -
                    new Vector2(4f, 2f) * player.Directions -
                    Main.screenPosition;

                Texture2D circularSmear = ModContent.Request<Texture2D>(
                    "CalamityMod/Particles/TrientCircularSmear",
                    AssetRequestMode.ImmediateLoad
                ).Value;

                Main.EntitySpriteDraw(
                    circularSmear,
                    blurPosition,
                    null,
                    blurColor * blurOpacity,
                    Projectile.rotation,
                    circularSmear.Size() / 2f,
                    Projectile.scale * 1.6f,
                    blurEffects,
                    0f
                );

                Texture2D slashSmear = ModContent.Request<Texture2D>(
                    "CalamityMod/Particles/SlashSmear",
                    AssetRequestMode.ImmediateLoad
                ).Value;

                Main.EntitySpriteDraw(
                    slashSmear,
                    blurPosition,
                    null,
                    blurColor * blurOpacity * 0.7f,
                    Projectile.rotation,
                    slashSmear.Size() / 2f,
                    Projectile.scale,
                    blurEffects,
                    0f
                );
            }

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Projectile.Center,
                Projectile.Center + Projectile.rotation.ToRotationVector2() * Projectile.height * Projectile.scale,
                Projectile.width * Projectile.scale * 0.5f,
                ref collisionPoint
            );
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(shouldSpin);
            writer.Write(Projectile.direction);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            shouldSpin = reader.ReadBoolean();
            Projectile.direction = reader.ReadInt32();
        }

        public override bool? CanDamage()
        {
            float progress = Projectile.ai[1] / Projectile.ai[0];

            if (progress >= 0.6f || progress <= 0.1f)
                return false;

            return null;
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 120, false);
            target.AddBuff(ModContent.BuffType<ElementalMix>(), 60);
            if (ModLoader.TryGetMod("CatalystMod", out Mod mod))
                return;
            target.AddBuff(mod.Find<ModBuff>("AstralBlight").Type, 60);
        }
    }
}
