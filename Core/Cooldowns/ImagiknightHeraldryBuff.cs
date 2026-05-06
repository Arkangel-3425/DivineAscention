using CalamityMod;
using CalamityMod.Cooldowns;
using InfernalEclipseWeaponsDLC.Core.NewFolder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;

namespace InfernalEclipseWeaponsDLC.Core.Cooldowns
{
    public class ImagiknightHeraldryBuff : CooldownHandler
    {
        public float CompletionPercentage => Utils.GetLerpValue(20f, 0f, instance.timeLeft);
        private bool IsEmpty => instance.timeLeft >= 20;
        private float TextXOffset => instance.timeLeft <= 20 ? -11f : -18f;
        private Vector2 TextPosition => new(TextXOffset, 15);
        private Color TextColor => Color.White;
        private Color TextBorderColor => Color.Black;

        public static new string ID => "ImagiknightHeraldryBuff";
        public override bool CanTickDown => false;
        public override LocalizedText DisplayName => Language.GetOrRegister($"Mods.InfernalEclipseWeaponsDLC.UI.Cooldowns.{ID}");
        public override bool ShouldDisplay => instance.player.GetModPlayer<InfernalWeaponsPlayer>().imagiknightHeraldry || instance.player.GetModPlayer<InfernalWeaponsPlayer>().heraldyBuffFromOther > 0f;
        public override string Texture => "InfernalEclipseWeaponsDLC/Core/Cooldowns/" + ID;

        public override Color CooldownStartColor => IsEmpty ? Color.DimGray : Color.Lerp(Color.SlateGray, Color.BlueViolet, CompletionPercentage);
        public override Color CooldownEndColor => IsEmpty ? Color.DimGray : Color.Lerp(Color.SlateGray, Color.Blue, CompletionPercentage);

        private const int MaxBonusPercent = 20;
        private int CurrentBonusPercent => Utils.Clamp(MaxBonusPercent - instance.timeLeft, 0, MaxBonusPercent);

        public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            base.DrawExpanded(spriteBatch, position, opacity, scale);

            CalamityUtils.DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, $"+{CurrentBonusPercent}%", position + TextPosition, TextColor, TextBorderColor);
        }

        public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            base.DrawCompact(spriteBatch, position, opacity, scale);

            CalamityUtils.DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, $"+{CurrentBonusPercent}%", position + TextPosition, TextColor, TextBorderColor);
        }
    }
}
