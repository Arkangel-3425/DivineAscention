using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.MeleePro.Void
{
    public class CosmicPunch : ModProjectile
    {
        private int numHits = 0;
        private int numSpawns = 1;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = ModLoader.TryGetMod("SOTS", out Mod sots) ? sots.Find<DamageClass>("VoidMelee") : DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.width = 56;
            Projectile.height = 30;
            Projectile.timeLeft = 70;
            Projectile.penetrate = 6;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 40;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.extraUpdates = 1;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (numSpawns >= 10)
                modifiers.SetCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 3 * 60);

            if (numSpawns >= 10)
            {
                SoundEngine.PlaySound(new("CalamityMod/Sounds/NPCHit/OtherworldlyHit"), new Vector2?(target.position));
                target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 3 * 60);
            }

            numHits++;
            Projectile.ai[0] = 200f;
            Projectile.netUpdate = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            numHits = (int)Projectile.ai[1];
            numSpawns = (int)Projectile.ai[2];

            if (numSpawns >= 10)
            {
                Projectile.scale *= 3;
            }
        }

        public override bool PreAI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
            return true;
        }

        public override void AI()
        {
            if (numHits >= 6 && numSpawns <= 10)
                Projectile.Kill();

            Projectile.ai[0]++;

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.alpha += 3;

            int target = FindTarget_Basic(Projectile.Center, 640f);

            if (target != -1)
            {
                NPC npc = Main.npc[target];

                if (npc.active && Projectile.ai[0] < 200f)
                {
                    Vector2 direction = Projectile.SafeDirectionTo(npc.Center);

                    Projectile.velocity = Vector2.Lerp(
                        Projectile.velocity,
                        direction * (Projectile.velocity.Length() + 3f),
                        0.07f
                    );
                }
            }

            if (Projectile.ai[0] <= 6f)
                return;

            Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5f), 0, 0, ModContent.DustType<CosmiliteBarDust>());
            dust.velocity *= 0.05f;
            dust.velocity -= Projectile.velocity.SafeNormalize(Vector2.Zero);
            dust.scale = 1.3f;
            dust.noGravity = true;
            dust.fadeIn = 0.2f;
            dust.alpha = 100;

            Lighting.AddLight(Projectile.Center, 0.6f, 0.2f, 0.8f);
        }

        private static int FindTarget_Basic(Vector2 center, float minDistance = 2000f)
        {
            int targetBasic = -1;
            for (int index = 0; index < Main.maxNPCs; ++index)
            {
                if (Main.npc[index].CanBeChasedBy(null, false))
                {
                    Vector2 vector2 = center - Main.npc[index].Center;
                    float num = vector2.Length();
                    if ((double)num < (double)minDistance && (Collision.CanHitLine(center - new Vector2(16f, 16f), 32, 32, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height)))
                    {
                        targetBasic = index;
                        minDistance = num;
                    }
                }
            }
            return targetBasic;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5f), 0, 0, ModContent.DustType<CosmiliteBarDust>());

                dust.velocity *= 1.2f;
                dust.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 5f;

                dust.scale *= 2f;
                dust.noGravity = true;
                dust.fadeIn = 0.2f;

                dust.alpha = 100;
            }

            if (Main.myPlayer == Projectile.owner) 
            {
                if (numHits < 6 && numSpawns < 2)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, -Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI, ai1: numHits, ai2: numSpawns + 1);
                else if (numSpawns >= 10 && numSpawns < 12)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, -Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI, ai1: numHits, ai2: numSpawns + 1);
            }

            base.OnKill(timeLeft);
        }
    }
}
