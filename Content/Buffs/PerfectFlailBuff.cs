using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Content.Buffs
{
    public class PerfectFlailBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.30f;
            player.GetCritChance(DamageClass.Generic) += 20;
            player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
            player.statDefense += 30;
            player.endurance += 0.20f;
            player.DefenseEffectiveness *= 1.1f;
            player.moveSpeed += 0.3f;
            player.jumpSpeedBoost += 0.2f;
            player.runAcceleration += 0.1f;
        }
    }
}