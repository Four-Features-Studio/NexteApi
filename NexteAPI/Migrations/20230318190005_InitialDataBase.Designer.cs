﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NexteServer.Efcore;

#nullable disable

namespace NexteAPI.Migrations
{
    [DbContext(typeof(NexteDbContext))]
    [Migration("20230318190005_InitialDataBase")]
    partial class InitialDataBase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NexteAPI.EFCore.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("AccessToken")
                        .HasColumnType("longtext")
                        .HasColumnName("accessToken");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext")
                        .HasColumnName("avatar");

                    b.Property<bool>("MultiplayerRealms")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("multiplayerRealms");

                    b.Property<bool>("MultiplayerServer")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("multiplayerServer");

                    b.Property<bool>("OnlineChat")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("onlineChat");

                    b.Property<string>("Password")
                        .HasColumnType("longtext")
                        .HasColumnName("password");

                    b.Property<string>("ServerId")
                        .HasColumnType("longtext")
                        .HasColumnName("serverId");

                    b.Property<bool>("Telemetry")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("telemetry");

                    b.Property<string>("Username")
                        .HasColumnType("longtext")
                        .HasColumnName("username");

                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}