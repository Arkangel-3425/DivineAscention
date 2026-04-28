using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;
using CalamityMod.Buffs.DamageOverTime;

namespace InfernalEclipseWeaponsDLC.Content.Projectiles.BardPro
{
    public class MetalPipe : BardProjectile
    {
        public override string Texture => "ThoriumRework/Items/ConcussiveInstrument";

        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.alpha = byte.MaxValue;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        public override void AI()
        {
            if (Projectile.alpha == byte.MaxValue)
                Projectile.velocity.X += float.Epsilon * Main.player[Projectile.owner].direction;
            Projectile.velocity.Y += 0.34f;
            Projectile projectile = Projectile;
            projectile.velocity *= 0.96f;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.rotation += Projectile.direction * 0.3f;

            if (Projectile.alpha > 0)
                Projectile.alpha -= 17;

            if (Projectile.ai[0] < 0.0)
                return;

            Rectangle hitbox = Projectile.Hitbox;
            if (hitbox.Intersects(Main.npc[(int)Projectile.ai[0]].Hitbox))
                return;
            Projectile.ai[0] = -1f;
        }

        public override void OnKill(int timeLeft)
        {
            ParticleOrchestrator.RequestParticleSpawn(false, 0, new ParticleOrchestraSettings()
            {
                PositionInWorld = Projectile.Center
            }, new int?(Projectile.owner));

            SoundEngine.PlaySound(new SoundStyle("InfernalEclipseWeaponsDLC/Assets/Sounds/MetalPipe") { Volume = 10f }, new Vector2?(Projectile.position));
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ash, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, Color.White, 1.2f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            SpriteEffects spriteEffects = Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle frame = new Rectangle(0, texture.Height / Main.projFrames[Type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Type]);
            Vector2 origin = new Vector2(texture.Width, texture.Height / Main.projFrames[Type]) * 0.5f;

            // Trail.
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPosition = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Color trailColor = lightColor * MathHelper.Lerp(0.2f, 0f, i / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPosition, frame, trailColor, Projectile.oldRot[i] - MathHelper.PiOver4 * Projectile.spriteDirection, origin, Projectile.scale, spriteEffects, 0f);
            }

            // Main projectile.
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation - MathHelper.PiOver4 * Projectile.spriteDirection, origin, Projectile.scale, spriteEffects, 0f);

            return false;
        }

        public override bool? CanHitNPC(NPC target) => new bool?(Projectile.ai[0] < 0.0 || target.whoAmI != (double)Projectile.ai[0]);

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AuricRebuke>(), 3 * 60);
        }
    }
}
