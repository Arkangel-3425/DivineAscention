using System;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.FlailPro
{
    public class BlackHoleFlailCorePro : FlailCoreProBase
    {
        public override void SetDefaults()
        {
            Entity.width = 50;
            Entity.height = 42;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = ProjAIStyleID.GroundProjectile;
            AIType = ProjectileID.SpikyBall;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Asphalt, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.8f, 0, default, 1f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextFloat(1) < 0.05)
                target.AddBuff(ModContent.BuffType<WitherDebuff>(), 300, false);
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 250)
            {
                CalamityUtils.HomeInOnNPC(Projectile, Projectile.tileCollide, 250f, 10f, 15f);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            //Enemy sucktion code down
            float projX = Projectile.Center.X;
            float projY = Projectile.Center.Y;
            float homeRange = 300f;
            float homingSpeed = 0.1f;
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1) && !npc.boss)
                {
                    float npcX = npc.position.X + (npc.width / 2);
                    float npcY = npc.position.Y + (npc.height / 2);
                    float targetDist = Math.Abs(Projectile.position.X + (Projectile.width / 2) - npcX) + Math.Abs(Projectile.position.Y + (Projectile.height / 2) - npcY);
                    if (targetDist < homeRange)
                    {
                        if (npc.position.X < projX)
                        {
                            npc.velocity.X += homingSpeed;
                        }
                        else
                        {
                            npc.velocity.X -= homingSpeed;
                        }
                        if (npc.position.Y < projY)
                        {
                            npc.velocity.Y += homingSpeed;
                        }
                        else
                        {
                            npc.velocity.Y -= homingSpeed;
                        }
                    }
                }
            }
        }
    }
}