using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using InfernalEclipseWeaponsDLC.Systems;

namespace InfernalEclipseWeaponsDLC.Common.GlobalItems
{
    public class MeleeScaleGlobalProjectile : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return ScaleFixRegistry.FixedSwingProjectiles.Contains(entity.type);
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse itemSource && itemSource.Entity is Player player)
            {
                float totalScale = player.GetAdjustedItemScale(itemSource.Item);

                int oldWidth = projectile.width;
                int oldHeight = projectile.height;

                projectile.width = (int)(projectile.width * totalScale);
                projectile.height = (int)(projectile.height * totalScale);

                projectile.position.X -= (projectile.width - oldWidth) / 2f;
                projectile.position.Y -= (projectile.height - oldHeight) / 2f;
            }
        }

        public override void PostAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            // Ensures the player is active and actually holding a weapon
            if (player.active && !player.dead && player.HeldItem != null && !player.HeldItem.IsAir)
            {
                projectile.scale = player.GetAdjustedItemScale(player.HeldItem);
            }
        }
    }
}