using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Content.Items.Placeable;

namespace WgMod.Common.Systems;
public class CraftVanillaItemsSystem : ModSystem
{
    public override void AddRecipes()
    {
        Recipe.Create(ItemID.DartTrap)
                .AddIngredient(ModContent.ItemType<FatteningDartTrap>())
                .AddIngredient(ItemID.BottledWater)
                .Register();
        Recipe.Create(ItemID.DartTrap)
                .AddIngredient(ModContent.ItemType<FatteningDartTrap>())
                .AddCondition(Condition.NearWater)
                .Register();
    }
}
