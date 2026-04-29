using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using CalamityMod.BiomeManagers;
using CalamityMod;
using InfernalEclipseWeaponsDLC.Content.Items.Materials;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using InfernalEclipseWeaponsDLC.Content.Items.Weapons.Melee.Void;
using InfernalEclipseWeaponsDLC.Content.Projectiles.MeleePro.Void;
using Terraria.ID;
using InfernalEclipseWeaponsDLC.Content.Projectiles.BardPro;
using Terraria.Audio;

namespace InfernalEclipseWeaponsDLC.Core.NewFolder
{
    public class InfernalWeaponsPlayer : ModPlayer
    {
        public bool spearSearing;
        public bool spearArctic;
        public bool minionCrits;
        public bool godsPitch;

        const int shard2chance = 20;

        public int missileIndex = 10;
        public int CataclysmFistShotCount = 0;
        public int annihilationBonusShotTimeLeft = 0;
        public int annihilationBonusShotCooldown = 0;

        public override void ResetEffects()
        {
            spearSearing = false;
            spearArctic = false;
            minionCrits = false;
            godsPitch = false;

            if (annihilationBonusShotTimeLeft > 0)
                annihilationBonusShotTimeLeft--;

            if (annihilationBonusShotCooldown > 0)
                annihilationBonusShotCooldown--;
        }

        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            bool isSulfurCatch = Player.InModBiome<SulphurousSeaBiome>();
            bool inWater = !attempt.inLava && !attempt.inHoney;

            if (!isSulfurCatch || !inWater) return;

            bool goodEnoughLevel = attempt.fishingLevel >= 45;
            bool randomChanceSuccess = Main.rand.NextBool(shard2chance);

            if (!randomChanceSuccess || !goodEnoughLevel) return;

            itemDrop = ModContent.ItemType<DeepSeaDrawlShard2>();
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            object result = ModLoader.GetMod("ThoriumMod").Call("IsBardProjectile", proj);

            if (result is ValueTuple<bool, byte> valueTuple && valueTuple.Item1)
            {
                if (godsPitch)
                {
                    int metalPipe = ModContent.ProjectileType<MetalPipe>();
                    if (metalPipe != proj.type && Main.myPlayer == Player.whoAmI)
                    {
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.NewProjectile(proj.GetSource_OnHit(target), target.Center, new Vector2(target.position.X - target.oldPosition.X, -16f), metalPipe, (hit.Damage + damageDone) / 3, proj.knockBack, proj.owner, target.whoAmI, 0.0f, 0.0f), 0.0f, 0.0f, 0.0f, 0, 0, 0);
                    }
                }
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (proj.hostile)
                return;

            if (minionCrits && IsSummonDamage(proj))
            {
                if (Main.rand.Next(100) < ActualClassCrit(Player, DamageClass.Summon))
                    modifiers.SetCrit();
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (ModLoader.HasMod("SOTS"))
            {
                if (Player.controlUseItem && Player.HeldItem.type == Mod.Find<ModItem>("GauntletofAnnihilationVoid").Type)
                {
                    if (annihilationBonusShotTimeLeft > 0 && annihilationBonusShotCooldown == 0)
                    {
                        CombatText.NewText(Player.Hitbox, Color.Lerp(Color.Cyan, Color.Magenta, 0.5f), Main.rand.NextBool() ? "It's not over yet!" : "Did that hurt?", true);
                        SoundEngine.PlaySound(new("CalamityMod/Sounds/Custom/DoGFireball"), new Vector2?(Player.position));
                        Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.Center, Player.velocity, ModContent.ProjectileType<CosmicPunch>(), Player.HeldItem.damage * 30, Player.HeldItem.knockBack, Player.whoAmI, ai1: 6, ai2: 10);
                        annihilationBonusShotTimeLeft = 0;
                        annihilationBonusShotCooldown = 120;
                    }
                }
            }
            else
            {
                if (Player.controlUseItem && Player.HeldItem.type == ModContent.ItemType<GauntletofAnnihilation>())
                {
                    if (annihilationBonusShotTimeLeft > 0 && annihilationBonusShotCooldown == 0)
                    {
                        CombatText.NewText(Player.Hitbox, Color.Lerp(Color.Cyan, Color.Magenta, 0.5f), Main.rand.NextBool() ? "It's not over yet!" : "Did that hurt?", true);
                        SoundEngine.PlaySound(new("CalamityMod/Sounds/Custom/DoGFireball"), new Vector2?(Player.position));
                        Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.Center, Player.velocity, ModContent.ProjectileType<CosmicPunch>(), Player.HeldItem.damage * 25, Player.HeldItem.knockBack, Player.whoAmI, ai1: 6, ai2: 10);
                        annihilationBonusShotTimeLeft = 0;
                        annihilationBonusShotCooldown = 120;
                    }
                }
            }

            MiscEffects();
        }

        private void MiscEffects()
        {
            if (ModLoader.HasMod("SOTS"))
            {
                
                if (Player.HeldItem.type == Mod.Find<ModItem>("CataclysmicGauntletVoid").Type) //we have to do it this way since the item doesn't load without SOTS.
                    SupremeCataclysmFist.GenerateDustOnOwnerHand(Player);

                if (Player.HeldItem.type == Mod.Find<ModItem>("GauntletofAnnihilationVoid").Type)
                    GauntletofAnnihilationPunches.GenerateDustOnOwnerHand(Player);
            }
            else
            {
                if (Player.HeldItem.type == ModContent.ItemType<CataclysmicGauntlet>())
                {
                    SupremeCataclysmFist.GenerateDustOnOwnerHand(Player);
                }
            }
        }

        // thank you fargos
        public static bool IsSummonDamage(Projectile projectile, bool includeMinionShot = true, bool includeWhips = true)
        {
            if (!includeWhips && ProjectileID.Sets.IsAWhip[projectile.type])
                return false;

            if (!includeMinionShot && (ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type]))
                return false;

            return projectile.CountsAsClass(DamageClass.Summon) || projectile.minion || projectile.sentry || projectile.minionSlots > 0 || ProjectileID.Sets.MinionSacrificable[projectile.type]
                || (includeMinionShot && (ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type]))
                || (includeWhips && ProjectileID.Sets.IsAWhip[projectile.type]);
        }

        public float ActualClassCrit(Player player, DamageClass damageClass)
            => (damageClass == DamageClass.Summon || damageClass == DamageClass.SummonMeleeSpeed) && !(minionCrits)
            ? 0
            : player.GetTotalCritChance(damageClass);
    }
}
