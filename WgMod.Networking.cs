using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;
using WgMod.Common.Players;
using WgMod.Common.Systems;

namespace WgMod;

partial class WgMod
{
    public enum MessageType : byte
    {
        Invalid = 0,
        WgPlayerSync,
        WgPlayerGurgle,
        WgPlayerCombatWeightText,
        MannequinSetStage
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        MessageType type = (MessageType)reader.ReadByte();
        switch (type)
        {
            case MessageType.WgPlayerSync:
                WgPlayer player = Main.player[reader.ReadByte()].Wg();
                player.ReceivePlayerSync(reader);
                if (Main.netMode == NetmodeID.Server) // Forward the changes to the other clients
                    player.SyncPlayer(-1, whoAmI, false);
                break;
            case MessageType.WgPlayerGurgle:
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = this.GetPacket(type);
                    packet.Write(reader.ReadByte());
                    packet.Send();
                }
                else
                    Main.player[reader.ReadByte()].Wg().Gurgle(false);
                break;
            case MessageType.WgPlayerCombatWeightText:
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = this.GetPacket(type);
                    packet.Write(reader.ReadByte());
                    packet.Write(reader.ReadSingle());
                    packet.Send();
                }
                else
                    Main.player[reader.ReadByte()].Wg().CombatWeightText(reader.ReadSingle(), false);
                break;
            case MessageType.MannequinSetStage:
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = this.GetPacket(type);
                    packet.Write(reader.ReadInt32());
                    packet.Write(reader.ReadByte());
                    packet.Send();
                }
                else
                    WgMannequinSystem.SetStage((TEDisplayDoll)TileEntity.ByID[reader.ReadInt32()], reader.ReadByte(), false);
                break;
            default:
                Logger.WarnFormat("WgMod: Unknown Message type: {0}", type);
                break;
        }
    }
}
