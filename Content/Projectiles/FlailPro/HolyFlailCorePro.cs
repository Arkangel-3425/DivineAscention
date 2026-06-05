using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.FlailPro
{
    public class HolyFlailCorePro : FlailCoreProBase
    {
        public override void SetDefaults()
        {
            Entity.width = 40;
            Entity.height = 40;
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
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Fireworks, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.8f, 0, default, 1f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 240, false);
            if (Main.rand.NextFloat(1) < 0.5)
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HolyFlailFire>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner);
        }
    }
}