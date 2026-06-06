using CalamityMod.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Content.Buffs
{
    public class LimbBurn : ModBuff
    {
        public static DebuffData debuffData = new DebuffData()
        {
            EnemyLostRegen = 100f,
            HeatDebuffScaling = 1,
            MinimumDamageTickSize = 6,
            MultiplierDamageTickSize = 0,
            DrawAboveNPC = true
        };
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
            BuffDatasets.DebuffDataset[Type] = debuffData;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.runAcceleration *= 0.975f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity *= 0.925f;
        }
    }
}