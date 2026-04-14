using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using InfernalEclipseWeaponsDLC.Content.Buffs;

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
            if (Main.myPlayer != Projectile.owner)
                return;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.CanBeChasedBy())
                    continue;

                if (Projectile.Hitbox.Intersects(npc.Hitbox))
                {
                    npc.AddBuff(ModContent.BuffType<TVRemotePaused>(), 600);

                    Projectile.Kill();
                    break;
                }
            }
        }
    }
}
