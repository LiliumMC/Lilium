using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPlib.Protocol
{
    class Protocol
    {
        public const int MC18Version = 47;
        public const int MC19Version = 107;
        public const int MC191Version = 108;
        public const int MC110Version = 210;
        public const int MC111Version = 315;
        public const int MC17w13aVersion = 318;
        public const int MC112pre5Version = 332;
        public const int MC17w31aVersion = 336;
        public const int MC17w45aVersion = 343;
        public const int MC17w46aVersion = 345;
        public const int MC17w47aVersion = 346;
        public const int MC18w01aVersion = 352;
        public const int MC18w06aVersion = 357;
        public const int MC113pre4Version = 386;
        public const int MC113pre7Version = 389;
        public const int MC113Version = 393;

        public enum PacketIncomingType
        {
            KeepAlive,
            JoinGame,
            ChatMessage,
            TimeUpdate,
            EntityEquipment,
            SpawnPosition,
            UpdateHealth,
            Respawn,
            PlayerPositionAndLook,
            HeldItemChange,
            UseBed,
            Animation,
            SpawnPlayer,
            ChunkData,
            MultiBlockChange,
            BlockChange,
            MapChunkBulk,
            UnloadChunk,
            PlayerListUpdate,
            TabCompleteResult,
            PluginMessage,
            KickPacket,
            NetworkCompressionTreshold,
            ResourcePackSend,
            UnknownPacket
        }
        public enum PacketOutgoingType
        {
            KeepAlive,
            ResourcePackStatus,
            ChatMessage,
            ClientStatus,
            ClientSettings,
            PluginMessage,
            TabComplete,
            PlayerPosition,
            PlayerPositionAndLook,
            TeleportConfirm,
            Unknown
        }
    }
}
