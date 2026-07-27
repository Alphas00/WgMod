using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace WgMod.Common.Players;

public partial class WgPlayer
{
    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        ModPacket packet = Mod.GetPacket(WgMod.MessageType.WgPlayerSync);
        packet.Write((byte)Player.whoAmI);
        packet.Write(Weight.Mass);
        packet.Send(toWho, fromWho);
    }

    public void ReceivePlayerSync(BinaryReader reader)
    {
        SetWeightForced(new Weight(reader.ReadSingle()));
    }

    public override void CopyClientState(ModPlayer targetCopy)
    {
        WgPlayer clone = (WgPlayer)targetCopy;
        clone.SetWeightForced(Weight, false);
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        WgPlayer clone = (WgPlayer)clientPlayer;
        if (Weight != clone.Weight)
            SyncPlayer(-1, Main.myPlayer, false);
    }

    public void Gurgle(bool network)
    {
        if (Main.netMode == NetmodeID.SinglePlayer || !network)
        {
            SoundEngine.PlaySound(WgSounds.Gurgle, Player.Center);
            return;
        }
        ModPacket packet = Mod.GetPacket(WgMod.MessageType.WgPlayerGurgle);
        packet.Write((byte)Player.whoAmI);
        packet.Send();
    }

    public void CombatWeightText(float amount, bool network)
    {
        if (Main.netMode == NetmodeID.SinglePlayer || !network)
        {
            CombatText.NewText(Player.getRect(), Color.Yellow, amount + " kg");
            return;
        }
        ModPacket packet = Mod.GetPacket(WgMod.MessageType.WgPlayerCombatWeightText);
        packet.Write((byte)Player.whoAmI);
        packet.Write(amount);
        packet.Send();
    }

    // Saving
    public override void LoadData(TagCompound tag)
    {
        if (tag.TryGet("Weight", out float w))
        {
            if (float.IsNaN(w) || !float.IsFinite(w))
                w = Weight.Base.Mass;
            SetWeightForced(new Weight(w), false);
        }
        else
            SetWeightForced(Weight.Base, false);
    }

    public override void SaveData(TagCompound tag)
    {
        tag["Weight"] = Weight.Mass.Value;
    }
}
