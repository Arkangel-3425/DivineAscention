using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod;
using Terraria.ID;
using InfernalEclipseWeaponsDLC.Utilities;
using ThoriumMod.Buffs.Healer;
using System.Collections.Generic;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.ArmorPro
{
    public class EclipsePulseLight : ModProjectile
    {
        public override string Texture => "CalamityMod/Particles/SmallBloomRing";

        public float LifetimeCompletion => 1f - Projectile.timeLeft / 60f;

        private HashSet<int> healedPlayers = new HashSet<int>();

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = (DamageClass)(object)ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
            Projectile.timeLeft = 60;
            Projectile.scale = 0.001f;
        }

        public override void AI()
        {
            Projectile.ai[0]++;

            // Initial random rotation
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.rotation = Utils.NextFloat(Main.rand, MathHelper.TwoPi);
                Projectile.localAI[0] = Utils.ToDirectionInt(Utils.NextBool(Main.rand));
                Projectile.netUpdate = true;
            }

            Projectile.Opacity = (1f - (float)Math.Pow(LifetimeCompletion, 1.56)) * 0.4f;
            Projectile.scale = MathHelper.Lerp(0.1f, 15f, LifetimeCompletion);
            Projectile.rotation += Projectile.localAI[0] * 0.012f;

            HealNearbyAllies();
        }

        private void HealNearbyAllies()
        {
            Player owner = Main.player[Projectile.owner];

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];

                if (target.active && !target.dead && target.whoAmI != owner.whoAmI)
                {
                    if (owner.team != 0 && owner.team != target.team)
                        continue;

                    if (healedPlayers.Contains(target.whoAmI))
                        continue;

                    if (CalamityUtils.CircularHitboxCollision(Projectile.Center, Projectile.scale * 48f, target.Hitbox))
                    {
                        int healAmount = 0 + (int)owner.GetModPlayer<ThoriumPlayer>().healBonus;

                        HealTeammateThorium(owner, target, baseHeal: 0);

                        healedPlayers.Add(target.whoAmI);
                    }
                }
            }
        }

        private void HealTeammateThorium(Player healer, Player target, int baseHeal)
        {
            if (healer.whoAmI != Main.myPlayer) return;
            if (healer == target) return;
            if (healer.team == 0 || healer.team != target.team) return;

            if (baseHeal <= 0 && healer.GetModPlayer<ThoriumPlayer>().healBonus <= 0)
                return; // Nothing to heal

            HealerHelper.HealPlayer(
                healer,
                target,
                healAmount: baseHeal,
                recoveryTime: 60,
                healEffects: true,
                extraEffects: p => p.AddBuff(ModContent.BuffType<Cured>(), 30, true, false)
            );
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Orange * Projectile.Opacity;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color drawColor = Projectile.GetAlpha(lightColor) * 0.33f;

            for (int i = 0; i < 8; i++)
            {
                float rotation = Projectile.rotation;
                Vector2 drawOffset = Utils.ToRotationVector2((float)Math.PI * 2f * i / 8f) * Projectile.scale;
                Vector2 drawPosition = Projectile.Center - Main.screenPosition + drawOffset;

                if (i % 2 == 1) rotation *= -1f;

                Main.EntitySpriteDraw(
                    texture, drawPosition, null, drawColor,
                    rotation, Utils.Size(texture) * 0.5f,
                    Projectile.scale, SpriteEffects.None, 0f
                );
            }
            return false;
        }

        public override bool? CanHitNPC(NPC target) => !target.CountsAsACritter && !target.friendly && target.chaseable;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // Scale damage based on remaining lifetime
            // LifetimeCompletion goes 0 - 1, we want damage high - low, so invert it
            //float damageScale = 1f - LifetimeCompletion; // linear fade
            //modifiers.SourceDamage *= damageScale;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return CalamityUtils.CircularHitboxCollision(Projectile.Center, Projectile.scale * 48f, targetHitbox);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;
    }
}
