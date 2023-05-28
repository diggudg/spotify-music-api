﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpotifyApi.DbContext;

#nullable disable

namespace spotify_api.Migrations
{
    [DbContext(typeof(SpotifyContext))]
    [Migration("20230517214917_Updated_user_table")]
    partial class Updated_user_table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-preview.4.23259.3");

            modelBuilder.Entity("SpotifyApi.Model.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Href")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("Label")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uri")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("SpotifyApi.Model.Artist", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int?>("AlbumId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Href")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uri")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("SpotifyApi.Model.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Height")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Width")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Images");

                    b.HasAnnotation("Relational:JsonPropertyName", "images");
                });

            modelBuilder.Entity("SpotifyApi.Model.SpotifyToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccessToken")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "access_token");

                    b.Property<int>("ExpiresIn")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Scope")
                        .HasColumnType("TEXT");

                    b.Property<string>("TokenType")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SpotifyToken");
                });

            modelBuilder.Entity("SpotifyApi.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "display_name");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalUrls")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "external_urls");

                    b.Property<string>("Href")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "href");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uri")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SpotifyApi.Model.Artist", b =>
                {
                    b.HasOne("SpotifyApi.Model.Album", null)
                        .WithMany("Artist")
                        .HasForeignKey("AlbumId");
                });

            modelBuilder.Entity("SpotifyApi.Model.Image", b =>
                {
                    b.HasOne("SpotifyApi.Model.User", null)
                        .WithMany("Image")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SpotifyApi.Model.Album", b =>
                {
                    b.Navigation("Artist");
                });

            modelBuilder.Entity("SpotifyApi.Model.User", b =>
                {
                    b.Navigation("Image");
                });
#pragma warning restore 612, 618
        }
    }
}
