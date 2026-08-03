using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Configs;
using WgMod.Common.Players;
using WgMod.Content.Items;

namespace WgMod.Common.GlobalItems;

public class WgItem : GlobalItem
{
    public override bool CanUseItem(Item item, Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return true;
        if (!WgServerConfig.Instance.DisableFatBuffs && wg.Weight.GetStage() >= Weight.BlobStage)
        {
            bool allow = item.type == ModContent.ItemType<WeightManipulator>(); // Is dev object
            allow |= item.shoot != ProjectileID.None && ProjectileID.Sets.SingleGrappleHook[item.shoot]; // Is grappling hook
            allow |= item.mountType != -1; // Is mount
            if (!allow)
                player.PlayDroppedItemAnimation(30);
            return allow;
        }
        if (WgMod._buffTable.TryGetValue(item.buffType, out GainOptions gain) && gain.IsInstant)
        {
            if (wg.Stomach + gain.TotalGain > WgPlayer.StomachCapacity)
                return false;
        }
        return true;
    }

    public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;
        position.Y += wg._addedGfxOffY;
    }
}
