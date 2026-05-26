using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.MeleePro.DivineAxe
{
    public class DivineExplosion : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 5)
            {
                for (int i = 0; i < 8; i++)
                {
                    float angle = MathHelper.TwoPi / 8f * i;
                    Vector2 direction = angle.ToRotationVector2();
                    float maxSpeed = (i % 2 == 0) ? 20f : 10f;
                    for (int j = 0; j < 20; j++)
                    {
                        float speed = Main.rand.NextFloat(2f, maxSpeed);

                        Dust starDust = Dust.NewDustPerfect(
                            Projectile.Center,
                            DustID.SolarFlare,
                            direction * speed,
                            0,
                            new Color(255, 150, 0, 0),
                            Main.rand.NextFloat(1.5f, 3f)
                        );
                        starDust.noGravity = true;
                    }
                }

                for (int k = 0; k < 30; k++)
                {
                    Vector2 randomDir = Main.rand.NextVector2Circular(6f, 6f);
                    Dust coreDust = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.SolarFlare,
                        randomDir,
                        0,
                        new Color(255, 200, 50, 0),
                        Main.rand.NextFloat(2f, 4f)
                    );
                    coreDust.noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 300);
        }
    }
}
