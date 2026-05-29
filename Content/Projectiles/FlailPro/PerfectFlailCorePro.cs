using CalamityMod.Buffs.DamageOverTime;
using InfernalEclipseWeaponsDLC.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.FlailPro
{
    public class PerfectFlailCorePro : FlailCoreProBase
    {
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.8f, 0, default, 1f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 240, false);
            if (Main.rand.NextFloat(1) < 0.1)
                Main.player[Projectile.owner].AddBuff(ModContent.BuffType<PerfectFlailBuff>(), 600);
        }
    }
}