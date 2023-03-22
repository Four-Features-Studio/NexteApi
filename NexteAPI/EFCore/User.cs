using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexteAPI.EFCore
{
    public class User
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("avatar")]
        public string Avatar { get; set; }

        [Column("uuid")]
        public Guid? Uuid { get; set; }

        [Column("accessToken")]
        public string AccessToken { get; set; }

        [Column("serverId")]
        public string? ServerId { get; set; }

        [Column("skinUrl")]
        public string? SkinUrl { get; set; }

        [Column("cloakUrl")]
        public string? CloakUrl { get; set; }

        [Column("onlineChat")]
        public bool OnlineChat { get; set; } = true;

        [Column("multiplayerServer")]
        public bool MultiplayerServer { get; set; } = true;

        [Column("multiplayerRealms")]
        public bool MultiplayerRealms { get; set; } = true;

        [Column("telemetry")]
        public bool Telemetry { get; set; } = true;

    }
}
