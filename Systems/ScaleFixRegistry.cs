using System.Collections.Generic;
using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Systems
{
    public class ScaleFixRegistry : ModSystem
    {
        public static HashSet<int> FixedSwingProjectiles = new HashSet<int>();

        public override void PostSetupContent()
        {
            // Register Calamity's Murasama slash
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity) && calamity.TryFind<ModProjectile>("MurasamaSlash", out ModProjectile murasamaProj))
                FixedSwingProjectiles.Add(murasamaProj.Type);

            if (calamity.TryFind<ModProjectile>("EarthHoldout", out ModProjectile earthProj))
                FixedSwingProjectiles.Add(earthProj.Type);

            //base cal bug (tested/verified) hitbox is scuffed. size maxing makes discrepancy larger maybe fix? no other tested weapons have this issue.
            //if (calamity.TryFind<ModProjectile>("DragonRageStaff", out ModProjectile dragonrageProj)) 
            //FixedSwingProjectiles.Add(dragonrageProj.Type);

            if (calamity.TryFind<ModProjectile>("HolyColliderHoldout", out ModProjectile holycolliderProj))
                FixedSwingProjectiles.Add(holycolliderProj.Type);

            if (ModLoader.TryGetMod("InfernalEclipseAPI", out Mod InfernalEclipseAPI) && InfernalEclipseAPI.TryFind<ModProjectile>("ChickenWingHoldout", out ModProjectile ChickenWingProj))
                FixedSwingProjectiles.Add(ChickenWingProj.Type);

            // Add other mod projectiles here as you find them
        }

        public override void Unload()
        {
            FixedSwingProjectiles.Clear();
        }
    }
}