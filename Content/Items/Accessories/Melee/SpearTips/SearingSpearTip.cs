using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using InfernalEclipseWeaponsDLC.Core.NewFolder;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Items.BasicAccessories;
using Terraria.Localization;
using Terraria.DataStructures;

namespace InfernalEclipseWeaponsDLC.Content.Items.Accessories.Melee.SpearTips
{
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class SearingSpearTip : ThoriumItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.HasMod("ThoriumMod");
        }

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            accDamage = Language.GetTextValue("Mods.InfernalEclipseWeaponsDLC.ItemTooltip.SearingSpearTip");
            Item.width = 36;
            Item.height = 36;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            accessoryType = AccessoryType.SpearTip;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<InfernalWeaponsPlayer>().spearSearing = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MoltenSpearTip>()
                .AddIngredient<ScoriaBar>(6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
