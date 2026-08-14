using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Items.Armor.YogaClothes;

[AutoloadEquip(EquipType.Head)]

[Credit(ProjectRole.Programmer, Contributor.alphas0)]
[Credit(ProjectRole.Artist, Contributor.alphas0)]

public class YogaHeadband : ModItem
{
    public static int HeadSlot { get; private set; }
    public static LocalizedText SetBonusText { get; private set; }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;

        HeadSlot = Item.headSlot;
        SetBonusText = this.GetLocalization("SetBonus");
    }
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 16;
        Item.value = Item.sellPrice(silver: 10);
        Item.rare = ItemRarityID.Orange;
        Item.defense = 0;
    }

    public override void UpdateEquip(Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;
        float immobility = wg.Weight.ClampedImmobility;

        wg.MovementPenalty *= float.Lerp(1f, 0.92f, immobility);
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ModContent.ItemType<YogaTop>() && legs.type == ModContent.ItemType<YogaPants>();
    }

    public override void UpdateArmorSet(Player player)
    {
        if (!player.TryGetModPlayer(out YogaClothesPlayer ycp) || !player.TryGetModPlayer(out WgPlayer wg))
            return;

        ycp._active = true;

        player.setBonus = SetBonusText.Format();
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 5)
            .AddTile(TileID.Loom)
            .Register();
    }
}

public class YogaClothesPlayer : ModPlayer
{
    public bool _active = false;

    public override void ResetEffects()
    {
        _active = false;
    }

    public override void PostUpdateEquips()
    {
        if (_active)
        {
            if (!Player.TryGetModPlayer(out WgPlayer wg))
                return;
            if (Player.controlRight || Player.controlLeft || Player.controlJump)
            {
                wg.AddWeight(-0.15f);
            }
        }
    }
}
