﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockDelivery.API.Data;

namespace StockDelivery.API.Migrations
{
    [DbContext(typeof(DeliveryStockDbContext))]
    [Migration("20211217134403_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StockDelivery.API.Domain.ActiveDelivery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsAllDeliveryItemsScanned")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAnyDeliveryItemsScanned")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ActiveDeliveries");
                });

            modelBuilder.Entity("StockDelivery.API.Domain.BookStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("_code")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BookStocks");
                });

            modelBuilder.Entity("StockDelivery.API.Domain.CancelledDelivery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ActiveDeliveryId")
                        .HasColumnType("int");

                    b.Property<string>("CancellationReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("CancelledDeliveries");
                });

            modelBuilder.Entity("StockDelivery.API.Domain.CompletedDelivery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ActiveDeliveryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("CompletedDeliveries");
                });

            modelBuilder.Entity("StockDelivery.API.Domain.ActiveDelivery", b =>
                {
                    b.OwnsMany("StockDelivery.API.Domain.ActiveDeliveryItem", "_items", b1 =>
                        {
                            b1.Property<int>("ActiveDeliveryId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("BookId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("CreationDate")
                                .HasColumnType("datetime2");

                            b1.Property<bool>("IsAllScanned")
                                .HasColumnType("bit");

                            b1.Property<bool>("IsScanned")
                                .HasColumnType("bit");

                            b1.Property<short>("ItemsCount")
                                .HasColumnType("smallint");

                            b1.Property<DateTime>("ModificationDate")
                                .HasColumnType("datetime2");

                            b1.Property<short>("ScannedCount")
                                .HasColumnType("smallint");

                            b1.Property<string>("_code")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ActiveDeliveryId", "Id");

                            b1.ToTable("ActiveDeliveryItem");

                            b1.WithOwner()
                                .HasForeignKey("ActiveDeliveryId");

                            b1.OwnsOne("StockDelivery.API.Domain.ValueObjects.BookEan13", "BookEan", b2 =>
                                {
                                    b2.Property<int>("ActiveDeliveryItemActiveDeliveryId")
                                        .HasColumnType("int");

                                    b2.Property<int>("ActiveDeliveryItemId")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int")
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.Property<string>("Code")
                                        .HasMaxLength(13)
                                        .HasColumnType("nvarchar(13)");

                                    b2.HasKey("ActiveDeliveryItemActiveDeliveryId", "ActiveDeliveryItemId");

                                    b2.HasIndex("Code")
                                        .IsUnique()
                                        .HasFilter("[BookEan_Code] IS NOT NULL");

                                    b2.ToTable("ActiveDeliveryItem");

                                    b2.WithOwner()
                                        .HasForeignKey("ActiveDeliveryItemActiveDeliveryId", "ActiveDeliveryItemId");
                                });

                            b1.Navigation("BookEan");
                        });

                    b.Navigation("_items");
                });

            modelBuilder.Entity("StockDelivery.API.Domain.BookStock", b =>
                {
                    b.OwnsOne("StockDelivery.API.Domain.ValueObjects.BookEan13ForStock", "BookEan13", b1 =>
                        {
                            b1.Property<int>("BookStockId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Code")
                                .HasMaxLength(13)
                                .HasColumnType("nvarchar(13)");

                            b1.HasKey("BookStockId");

                            b1.ToTable("BookStocks");

                            b1.WithOwner()
                                .HasForeignKey("BookStockId");
                        });

                    b.Navigation("BookEan13");
                });

            modelBuilder.Entity("StockDelivery.API.Domain.CancelledDelivery", b =>
                {
                    b.OwnsMany("StockDelivery.API.Domain.CancelledDeliveryItem", "_items", b1 =>
                        {
                            b1.Property<int>("CancelledDeliveryId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("BookId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("CreationDate")
                                .HasColumnType("datetime2");

                            b1.Property<short>("ItemsCount")
                                .HasColumnType("smallint");

                            b1.Property<DateTime>("ModificationDate")
                                .HasColumnType("datetime2");

                            b1.Property<string>("_code")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CancelledDeliveryId", "Id");

                            b1.ToTable("CancelledDeliveryItem");

                            b1.WithOwner()
                                .HasForeignKey("CancelledDeliveryId");

                            b1.OwnsOne("StockDelivery.API.Domain.ValueObjects.BookEan13", "BookEan", b2 =>
                                {
                                    b2.Property<int>("CancelledDeliveryItemCancelledDeliveryId")
                                        .HasColumnType("int");

                                    b2.Property<int>("CancelledDeliveryItemId")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int")
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.Property<string>("Code")
                                        .HasMaxLength(13)
                                        .HasColumnType("nvarchar(13)");

                                    b2.HasKey("CancelledDeliveryItemCancelledDeliveryId", "CancelledDeliveryItemId");

                                    b2.HasIndex("Code")
                                        .IsUnique()
                                        .HasFilter("[BookEan_Code] IS NOT NULL");

                                    b2.ToTable("CancelledDeliveryItem");

                                    b2.WithOwner()
                                        .HasForeignKey("CancelledDeliveryItemCancelledDeliveryId", "CancelledDeliveryItemId");
                                });

                            b1.Navigation("BookEan");
                        });

                    b.Navigation("_items");
                });

            modelBuilder.Entity("StockDelivery.API.Domain.CompletedDelivery", b =>
                {
                    b.OwnsMany("StockDelivery.API.Domain.CompletedDeliveryItem", "_items", b1 =>
                        {
                            b1.Property<int>("CompletedDeliveryId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("BookId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("CreationDate")
                                .HasColumnType("datetime2");

                            b1.Property<short>("ItemsCount")
                                .HasColumnType("smallint");

                            b1.Property<DateTime>("ModificationDate")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Stocks")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("_code")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CompletedDeliveryId", "Id");

                            b1.ToTable("CompletedDeliveryItem");

                            b1.WithOwner()
                                .HasForeignKey("CompletedDeliveryId");

                            b1.OwnsOne("StockDelivery.API.Domain.ValueObjects.BookEan13", "BookEan", b2 =>
                                {
                                    b2.Property<int>("CompletedDeliveryItemCompletedDeliveryId")
                                        .HasColumnType("int");

                                    b2.Property<int>("CompletedDeliveryItemId")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int")
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.Property<string>("Code")
                                        .HasMaxLength(13)
                                        .HasColumnType("nvarchar(13)");

                                    b2.HasKey("CompletedDeliveryItemCompletedDeliveryId", "CompletedDeliveryItemId");

                                    b2.HasIndex("Code")
                                        .IsUnique()
                                        .HasFilter("[BookEan_Code] IS NOT NULL");

                                    b2.ToTable("CompletedDeliveryItem");

                                    b2.WithOwner()
                                        .HasForeignKey("CompletedDeliveryItemCompletedDeliveryId", "CompletedDeliveryItemId");
                                });

                            b1.Navigation("BookEan");
                        });

                    b.Navigation("_items");
                });
#pragma warning restore 612, 618
        }
    }
}
