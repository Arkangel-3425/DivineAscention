using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Content.Buffs
{
    public class GrandThunderWhipTag : ModBuff
    {
        public override string Texture => "InfernalEclipseWeaponsDLC/Assets/Textures/Empty";

        public override void Update(Player player, ref int buffIndex)
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }
}
