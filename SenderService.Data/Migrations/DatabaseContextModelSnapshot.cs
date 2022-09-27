﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NTB.SenderService.Data;

namespace NTB.SenderService.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NTB.SenderService.Data.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AttachesRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("MessageId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Recipient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.HasIndex("TypeId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("NTB.SenderService.Data.MessageStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MessageStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Поставлено в очередь"
                        },
                        new
                        {
                            Id = 2,
                            Name = "В процессе отправки"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Передано провайдеру"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Доставлено получателю"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Ошибка доставки"
                        });
                });

            modelBuilder.Entity("NTB.SenderService.Data.MessageType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("Attachable")
                        .HasColumnType("bit");

                    b.Property<int>("MaxLenght")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("Subjectable")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MessageType");

                    b.HasData(
                        new
                        {
                            Id = 4,
                            Attachable = true,
                            MaxLenght = 4096,
                            Name = "telegram",
                            Subjectable = false
                        });
                });

            modelBuilder.Entity("NTB.SenderService.Data.Message", b =>
                {
                    b.HasOne("NTB.SenderService.Data.MessageStatus", "Status")
                        .WithMany("Messages")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NTB.SenderService.Data.MessageType", "Type")
                        .WithMany("Messages")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("NTB.SenderService.Data.MessageStatus", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("NTB.SenderService.Data.MessageType", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
