using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace InfernalEclipseWeaponsDLC.Common.GlobalItems
{
    public class CalamitySizeRevert : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public bool sizeFixed = false;

        public override void PostReforge(Item item)
        {
            sizeFixed = false;
        }

        public override void UpdateInventory(Item item, Player player)
        {
            ApplyFix(item);
        }

        public override void HoldItem(Item item, Player player)
        {
            ApplyFix(item);
        }

        private void ApplyFix(Item item)
        {
            if (sizeFixed) return;

            if (!ModLoader.HasMod("CalamityMod")) return;

            if (item.prefix > 0 && item.damage > 0)
            {
                {
                    if (item.prefix >= 0 && item.prefix < Lang.prefix?.Length)
                    {
                        string prefixName = Lang.prefix[item.prefix].Value;
                        if (prefixName.Equals("Horrible", StringComparison.OrdinalIgnoreCase))
                        {
                            // Grab the unmodified base item to find its default scale
                            if (ContentSamples.ItemsByType.TryGetValue(item.type, out Item baseItem))
                            {
                                float baseScale = baseItem.scale;

                                if (baseScale > 0f)
                                {
                                    // Calculate the nerfed multiplier currently on the item
                                    float currentMultiplier = item.scale / baseScale;
                                    float currentBonus = currentMultiplier - 1f;

                                    if (Math.Abs(currentBonus) > 0.001f)
                                    {
                                        // Calamity reduced size bonuses to 66% (2/3rds) of their original value 
                                        float originalBonus = currentBonus * 1.5f;

                                        // Permanently overwrite the item's saved scale with the fixed value
                                        item.scale = baseScale * (1f + originalBonus);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            sizeFixed = true;
        }

        // Ensure our fixed status transfers correctly if the item is moved or cloned by the game
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            CalamitySizeRevert myClone = (CalamitySizeRevert)base.Clone(item, itemClone);
            myClone.sizeFixed = sizeFixed;
            return myClone;
        }
    }
}