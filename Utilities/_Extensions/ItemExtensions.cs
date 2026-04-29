using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Utilities._Extensions
{
    public static class ItemExtensions
    {
        public static void SetResearchCost(this ModItem modItem, int amt)
        {
            modItem.Item.ResearchUnlockCount = amt;
        }
    }
}
