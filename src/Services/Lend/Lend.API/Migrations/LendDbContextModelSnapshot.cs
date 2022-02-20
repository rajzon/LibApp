﻿// <auto-generated />
using System;
using Lend.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lend.API.Migrations
{
    [DbContext(typeof(LendDbContext))]
    partial class LendDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lend.API.Domain.LendedBasket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("LendedBaskets");
                });

            modelBuilder.Entity("Lend.API.Domain.LendedStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BookEan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LendedBasketId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LendedBasketId");

                    b.ToTable("LendedStock");
                });

            modelBuilder.Entity("Lend.API.Domain.SimpleBooleanRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RuleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RuleValue")
                        .HasColumnType("bit");

                    b.Property<int>("RuleValueType")
                        .HasColumnType("int");

                    b.Property<string>("StrategyType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SimpleBooleanRules");
                });

            modelBuilder.Entity("Lend.API.Domain.SimpleIntRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RuleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RuleValue")
                        .HasColumnType("int");

                    b.Property<int>("RuleValueType")
                        .HasColumnType("int");

                    b.Property<string>("StrategyType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SimpleIntRules");
                });

            modelBuilder.Entity("Lend.API.Domain.LendedStock", b =>
                {
                    b.HasOne("Lend.API.Domain.LendedBasket", null)
                        .WithMany("Stocks")
                        .HasForeignKey("LendedBasketId");
                });

            modelBuilder.Entity("Lend.API.Domain.LendedBasket", b =>
                {
                    b.Navigation("Stocks");
                });
#pragma warning restore 612, 618
        }
    }
}