using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using ThoriumMod.Items.BasicAccessories;
using InfernalEclipseWeaponsDLC.Core.NewFolder;
using CalamityMod.Items;

namespace InfernalEclipseWeaponsDLC.Content.Items.Accessories.Melee.Flails
{
    public class DoubleFlail : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => WeaponConfig.Instance.FlailCores;

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<InfernalWeaponsPlayer>().DoubleFlailAcc = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<IronFlailCore>(2)
                .AddIngredient<CryonicBar>(10)
                .AddIngredient<AshesofCalamity>(10)
                .AddIngredient<UnholyCore>(5)
                .AddIngredient<EssenceofEleum>(2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}