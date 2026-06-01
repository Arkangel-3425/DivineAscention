using System;
using System.Collections.Generic;
using System.Linq;
using CalamityMod.Enums;
using CalamityMod.Graphics.Metaballs;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace InfernalEclipseWeaponsDLC.Core.Graphics.Metaballs
{
    public class TerraMetaball : Metaball
    {
        public class Particle
        {
            public float Size;
            public Vector2 Velocity;
            public Vector2 Center;
            public Texture2D TextureToUse = null;
            public float rotation = 0f;
            public Vector2 Scale = Vector2.One;

            public float SizeScaling = 0.85f;
            public int CurrentFrame = 0;
            public int MaxFrames = 1;

            public int ShrinkDelay = 45;
            public int TimeAlive = 0;

            public Particle(Vector2 center, Vector2 velocity, float size)
            {
                Center = center;
                Velocity = velocity;
                Size = size;
            }

            public void Update()
            {
                TimeAlive++;
                Center += Velocity;
                Velocity *= 0.96f;

                if (ShrinkDelay < TimeAlive)
                    Size *= SizeScaling;
            }
        }

        public override bool FixedToScreen => false;

        public static List<Particle> Particles { get; private set; } = new();

        public override bool AnythingToDraw => Particles.Any();

        public static Asset<Texture2D> LayerAsset { get; private set; }

        public override IEnumerable<Texture2D> Layers
        {
            get
            {
                yield return LayerAsset.Value;
            }
        }

        public override void Load()
        {
            if (Main.dedServ)
                return;

            LayerAsset = ModContent.Request<Texture2D>("CalamityMod/Graphics/Metaballs/BloodLayer", AssetRequestMode.ImmediateLoad);
        }

        public override GeneralDrawLayer DrawLayer => GeneralDrawLayer.BeforeProjectiles;

        public override Color EdgeColor => new Color(0, 255, 140);

        public override void Update()
        {
            for (int i = 0; i < Particles.Count; i++)
                Particles[i].Update();

            Particles.RemoveAll(p => p.Size <= 2f);
        }

        public static Particle SpawnParticle(Vector2 position, Vector2 velocity, float size)
        {
            Particle particle = new(position, velocity, size);
            Particles.Add(particle);
            return particle;
        }

        public override Vector2 CalculateManualOffsetForLayer(int layerIndex)
        {
            return Vector2.UnitX * Main.GlobalTimeWrappedHourly * 0.0005f;
        }

        public override void DrawInstances()
        {
            Texture2D tex = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/BasicCircle").Value;

            foreach (Particle particle in Particles)
            {
                Texture2D texture2d = particle.TextureToUse ?? tex;
                Vector2 drawPosition = particle.Center - Main.screenPosition;

                Rectangle frame = texture2d.Frame(1, particle.MaxFrames, 0, particle.CurrentFrame);
                Vector2 origin = frame.Size() * 0.5f;

                Vector2 scale = particle.Scale * particle.Size / texture2d.Width;

                float colorInterpolant = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 5f + particle.TimeAlive * 0.08f) * 0.5f + 0.5f);

                Color terraBladeColor = Color.Lerp(new Color(0, 255, 120), new Color(80, 255, 255),colorInterpolant);

                Main.spriteBatch.Draw( texture2d, drawPosition, frame, terraBladeColor, particle.rotation, origin, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
