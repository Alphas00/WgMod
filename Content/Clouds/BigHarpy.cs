using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using WgMod.Content.EmoteBubbles;
using WgMod.Content.NPCs.TownNPCs.GroundedHarpy;

namespace WgMod.Content.Clouds;

public class BigHarpy : ModCloud, IUpdateCloud
{
	static int GetDir(Cloud cloud) => cloud.spriteDir == SpriteEffects.None ? -1 : 1;

	static void SetDir(Cloud cloud, int dir) => cloud.spriteDir = dir == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;


	public override float SpawnChance()
	{
		if (Main.dayTime)
			return 1f;
		else
			return 0;
	}

	public override void OnSpawn(Cloud cloud)
	{
		SetDir(cloud, Main.rand.NextBool() ? 1 : -1);
	}

	public override bool Draw(SpriteBatch spriteBatch, Cloud cloud, int cloudIndex, ref DrawData drawData)
	{
		const int frameCount = 4;
		const double frameDuration = 5.0;

		drawData.scale *= 0.5f;
		Texture2D texture = drawData.texture;
		drawData.position -= drawData.origin;
		drawData.sourceRect = texture.Frame(1, frameCount, 0, (int)(Main.timeForVisualEffects / frameDuration) % frameCount);
		drawData.origin = drawData.sourceRect.Value.Size() * 0.5f;
		drawData.position += drawData.origin;
		return true;
	}


	public void PostUpdate(Cloud cloud)
	{

	}

	public bool PreUpdate(Cloud cloud)
	{
		cloud.position.X += 0.5f * GetDir(cloud) + (Main.windSpeedCurrent * 0.5f);
		cloud.position.Y -= 0.1f;

		return false;
	}
}
