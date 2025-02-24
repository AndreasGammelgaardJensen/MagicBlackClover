﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Data
{
    [DbContext(typeof(DataContext))]
    [Migration("20250224075914_test3")]
    partial class test3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicScannerLib.Models.Database.CardDatabaseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Cmc")
                        .HasColumnType("float");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Layout")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManaCost")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MultiverseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Power")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rarity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Set")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Toughness")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.CollectionCardsDatabaseModel", b =>
                {
                    b.Property<Guid>("CollectionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CollectionId", "CardId");

                    b.HasIndex("CardId");

                    b.ToTable("CollectionCards");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.CollectionDatabaseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.ForeignNameDatabaseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MultiverseId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.ToTable("ForeignNames");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.LegalityDatabaseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LegalityStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.ToTable("Legalities");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.PrintingDatabaseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Set")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.ToTable("Printings");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.ScanDatabaseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CollectionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.ToTable("Scans");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.ScanItemsDatabaseModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CardName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ScanDatabaseModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("ScanDatabaseModelId");

                    b.ToTable("ScanItems");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.CollectionCardsDatabaseModel", b =>
                {
                    b.HasOne("MagicScannerLib.Models.Database.CardDatabaseModel", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MagicScannerLib.Models.Database.CollectionDatabaseModel", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Collection");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.ForeignNameDatabaseModel", b =>
                {
                    b.HasOne("MagicScannerLib.Models.Database.CardDatabaseModel", "Card")
                        .WithMany("ForeignNames")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.LegalityDatabaseModel", b =>
                {
                    b.HasOne("MagicScannerLib.Models.Database.CardDatabaseModel", "Card")
                        .WithMany("Legalities")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.PrintingDatabaseModel", b =>
                {
                    b.HasOne("MagicScannerLib.Models.Database.CardDatabaseModel", "Card")
                        .WithMany("Printings")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.ScanDatabaseModel", b =>
                {
                    b.HasOne("MagicScannerLib.Models.Database.CollectionDatabaseModel", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collection");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.ScanItemsDatabaseModel", b =>
                {
                    b.HasOne("MagicScannerLib.Models.Database.CardDatabaseModel", "Card")
                        .WithMany()
                        .HasForeignKey("CardId");

                    b.HasOne("MagicScannerLib.Models.Database.ScanDatabaseModel", null)
                        .WithMany("ScanItems")
                        .HasForeignKey("ScanDatabaseModelId");

                    b.Navigation("Card");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.CardDatabaseModel", b =>
                {
                    b.Navigation("ForeignNames");

                    b.Navigation("Legalities");

                    b.Navigation("Printings");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.ScanDatabaseModel", b =>
                {
                    b.Navigation("ScanItems");
                });
#pragma warning restore 612, 618
        }
    }
}
