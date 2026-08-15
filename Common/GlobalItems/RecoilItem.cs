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

    /// <summary>
    /// This creates recoil by applying velocity to the player against the direction of their mouse.
    /// </summary>
    /// <param name="recoilStrength"> The power of the applied recoil. </param>
    /// <param name="recoilResistance"> How much the player can resist the applied recoil, must be a clamped float. </param>
    /// <param name="airTimeFactor"> 
    /// How much firing in the air repeatedly reduces the recoil, must be a clamped float. Intended to prevent janky permaflight. 
    /// <para> Defaults to 1f </para>
    /// </param>
    /// <param name="flipRecoil"> 
    /// Makes the recoil send towards the player's mouse instead.  
    /// <para> Defaults to false. </para>
    /// </param>
    public void RecoilStats(float recoilStrength, float recoilResistance, float airTimeFactor = 1f, bool flipRecoil = false)
    {
        Player player = Main.LocalPlayer;

        if (player.noKnockback)
            return;

        Vector2 mousePosition = Main.MouseWorld;
        float angle = Utils.AngleFrom(player.Center, mousePosition);
        Vector2 velocity = new(MathF.Cos(angle), MathF.Sin(angle));

        if (!CheckForSolidGround())
        {
            recoilStrength *= _airTime;

            if (_airTime > airTimeFactor)
                _airTime -= airTimeFactor;
            else
                _airTime = airTimeFactor;
        }

        int direction = 1;
        if (flipRecoil)
            direction = -1;

        player.velocity += velocity * recoilStrength * recoilResistance * direction;
    }

    public override void UpdateInventory(Item item, Player player)
    {
        if (CheckForSolidGround())
            _airTime = 1;
    }

    /// <summary> Returns true when the player is grounded. </summary>
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
