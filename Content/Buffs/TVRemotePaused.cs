using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Content.Buffs
{
    public class TVRemotePaused : ModBuff
    {
        public override string Texture => "InfernalEclipseWeaponsDLC/Assets/Textures/Empty";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }

    public class PausedGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private Vector2 storedVelocity;

        public override void AI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<TVRemotePaused>()))
            {
                // Store velocity once
                if (storedVelocity == Vector2.Zero)
                    storedVelocity = npc.velocity;

                // Freeze movement
                npc.velocity = Vector2.Zero;

                // Stop AI
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.knockBackResist = 0f;

                // Optional: stop rotation/animation
                npc.rotation = 0f;

                return;
            }
            else
            {
                storedVelocity = Vector2.Zero;
            }
        }
    }
}
