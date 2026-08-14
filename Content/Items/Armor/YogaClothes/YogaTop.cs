using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Players;

namespace WgMod.Content.Items.Armor.YogaClothes;

[AutoloadEquip(EquipType.Body)]

[Credit(ProjectRole.Programmer, Contributor.alphas0)]
[Credit(ProjectRole.Artist, Contributor.alphas0)]

public class YogaTop : ModItem
{
    WgStat _movePenalty = new(0.99f, 0.92f);

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 28;
        Item.value = Item.sellPrice(silver: 20);
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
            .AddIngredient(ItemID.Silk, 10)
            .AddTile(TileID.Loom)
            .Register();
    }
}
