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
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly)
                    continue;

                if (Projectile.Hitbox.Intersects(npc.Hitbox))
                {
                    npc.AddBuff(ModContent.BuffType<TVRemotePaused>(), 600);

                    Projectile.netUpdate = true;
                    Projectile.Kill();
                }
            }

            foreach (Player player in Main.player)
            {
                if (!player.active || player.dead)
                    continue;

                if (Projectile.Hitbox.Intersects(player.Hitbox))
                {
                    if (player.whoAmI == Projectile.owner)
                        continue;

                    player.AddBuff(ModContent.BuffType<TVRemotePlayerPaused>(), 600);

                    Projectile.netUpdate = true;
                    Projectile.Kill();
                }
            }
        }
    }
}
