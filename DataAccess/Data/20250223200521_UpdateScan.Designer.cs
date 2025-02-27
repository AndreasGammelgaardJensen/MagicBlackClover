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
    [Migration("20250223200521_UpdateScan")]
    partial class UpdateScan
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

                    b.Property<string>("CardName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ScanDatabaseModelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ScanDatabaseModelId");

                    b.ToTable("ScanItems");
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
                    b.HasOne("MagicScannerLib.Models.Database.ScanDatabaseModel", null)
                        .WithMany("ScanItems")
                        .HasForeignKey("ScanDatabaseModelId");
                });

            modelBuilder.Entity("MagicScannerLib.Models.Database.ScanDatabaseModel", b =>
                {
                    b.Navigation("ScanItems");
                });
#pragma warning restore 612, 618
        }
    }
}
