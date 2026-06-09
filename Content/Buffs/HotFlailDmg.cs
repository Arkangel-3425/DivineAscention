using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Content.Buffs
{
    public class HotFlailDmg : ModBuff
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
            player.GetDamage(DamageClass.Generic) += 0.10f;
            //player.GetCritChance(DamageClass.Generic) += 10;
            //player.GetAttackSpeed(DamageClass.Generic) += 0.1f;
        }
    }
}