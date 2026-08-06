using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Players;
using WgMod.Content.Buffs.Debuffs;

namespace WgMod.Content.Items.Consumables.Potions;

public abstract class WeightPotion : ModItem
{
    public abstract float WeightEffect { get; }

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.DrinkLiquid;
        Item.useAnimation = 17;
        Item.useTime = 17;
        Item.useTurn = true;
        Item.UseSound = SoundID.Item3;
        Item.maxStack = Item.CommonMaxStack;
        Item.consumable = true;
    }

    public override bool? UseItem(Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return false;

        float weightChange;
        if (player.HasBuff<MilkshakeSickness>())
            weightChange = WeightEffect * 0.1f;
        else
        {
            weightChange = WeightEffect;
            player.AddBuff(ModContent.BuffType<MilkshakeSickness>(), 1 * 60 * 30);
        }
        wg.AddWeight(weightChange);
        wg.CombatWeightText(weightChange, false);
        return true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        tooltips.FormatLines(MathF.Abs(WeightEffect));
    }
}
