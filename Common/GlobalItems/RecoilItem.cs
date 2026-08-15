using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WgMod.Common.GlobalItems;

public class RecoilItem : GlobalItem
{
    public override bool InstancePerEntity => true;


    public float _airTime = 1;

    public void RecoilStats(float recoilStrength, float recoilResistance, bool flipRecoil = false)
    {
        Player player = Main.LocalPlayer;

        Vector2 mousePosition = Main.MouseWorld;
        float angle = Utils.AngleFrom(player.Center, mousePosition);
        Vector2 velocity = new(MathF.Cos(angle), MathF.Sin(angle));

        if (!CheckForSolidGround())
        {
            recoilStrength *= _airTime;

            if (_airTime != 0)
                _airTime -= 0.1f;
        }
        else
            _airTime = 1;

        int direction = 1;
        if (flipRecoil)
            direction = -1;

        player.velocity += velocity * recoilStrength * recoilResistance * direction;
    }

    bool CheckForSolidGround()
    {
        Player player = Main.LocalPlayer;

        List<Point> tiles = Collision.GetTilesIn(player.Hitbox.BottomLeft() - new Vector2(-2, -2), player.Hitbox.BottomRight() + new Vector2(2, 6));
        bool hasSolidTile = false;
        foreach (var point in tiles)
        {
            Tile tile = Framing.GetTileSafely(point);
            if (tile.HasTile)
            {
                if (Main.tileSolid[tile.TileType])
                    hasSolidTile = true;
                if (Main.tileSolidTop[tile.TileType])
                    hasSolidTile = true;
            }
        }
        if (hasSolidTile)
            return true;
        else
            return false;
    }
}
