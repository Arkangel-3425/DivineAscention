using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Content.Buffs
{
    public class ColdFlailDefense : ModBuff
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
            //player.statDefense += 15;
            player.endurance += 0.1f;
            //player.DefenseEffectiveness *= 1.05f;
        }
    }
}