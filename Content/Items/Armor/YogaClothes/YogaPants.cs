using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Items.Armor.YogaClothes;

[AutoloadEquip(EquipType.Legs)]

[Credit(ProjectRole.Programmer, Contributor.alphas0)]
[Credit(ProjectRole.Artist, Contributor.alphas0)]

public class YogaPants : ModItem
{
    WgStat _movePenalty = new(0.99f, 0.92f);

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 30;
        Item.value = Item.sellPrice(silver: 30);
        Item.rare = ItemRarityID.Blue;
        Item.defense = 0;
    }

    public override void UpdateEquip(Player player)
    {
        if (!player.TryGetModPlayer(out WgPlayer wg))
            return;
        float immobility = wg.Weight.ClampedImmobility;

        wg.MovementPenalty *= float.Lerp(1f, 0.92f, immobility);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Silk, 15)
            .AddTile(TileID.Loom)
            .Register();
    }
}
