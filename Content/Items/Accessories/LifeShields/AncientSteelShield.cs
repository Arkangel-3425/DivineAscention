using CalamityMod.Items;
using InfernalEclipseWeaponsDLC.Core.Players;
using SOTS.Items.AbandonedVillage;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace InfernalEclipseWeaponsDLC.Content.Items.Accessories.LifeShields
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    [AutoloadEquip(EquipType.Shield)]
    public class AncientSteelShield : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => WeaponConfig.Instance.LifeShields;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;
            Item.rare = ItemRarityID.Blue;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.accessory = true;
            Item.defense = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ThoriumPlayer>().MetalShieldMax += 30;
            player.GetModPlayer<SOTSCurseVoidEffects>().ancientShieldEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AncientSteelBar>(10)
                .AddIngredient<CharredWood>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
