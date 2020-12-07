﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenCredentialsPublisher.PublishingService.Data;

namespace OpenCredentialsPublisher.PublishingService.Api.Data.Migrations.Application.OcpDb
{
    [DbContext(typeof(OcpDbContext))]
    [Migration("20201127163105_AddProcessingState")]
    partial class AddProcessingState
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OpenCredentialsPublisher.PublishingService.Data.AccessKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateTimestamp")
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<bool>("Expired")
                        .HasColumnType("bit");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("RequestId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("AccessKey","dbo");
                });

            modelBuilder.Entity("OpenCredentialsPublisher.PublishingService.Data.ClrPublishLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset(7)");

                    b.HasKey("Id");

                    b.ToTable("ClrPublishLog","dbo");
                });

            modelBuilder.Entity("OpenCredentialsPublisher.PublishingService.Data.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContainerId")
                        .HasColumnType("varchar(256)");

                    b.Property<DateTimeOffset>("CreateTimestamp")
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("RequestId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("File","dbo");
                });

            modelBuilder.Entity("OpenCredentialsPublisher.PublishingService.Data.PublishRequest", b =>
                {
                    b.Property<string>("RequestId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<bool?>("ContainsPdf")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("CreateTimestamp")
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:Clustered", true)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset?>("PackageSignedTimestamp")
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<string>("ProcessingState")
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("PublishState")
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("RequestIdentity")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("RequestId")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("PublishRequest","dbo");
                });

            modelBuilder.Entity("OpenCredentialsPublisher.PublishingService.Data.SigningKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateTimestamp")
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<bool>("Expired")
                        .HasColumnType("bit");

                    b.Property<string>("IssuerId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("KeyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("RequestId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("VaultKeyIdentifier")
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("SigningKey","dbo");
                });

            modelBuilder.Entity("OpenCredentialsPublisher.PublishingService.Data.AccessKey", b =>
                {
                    b.HasOne("OpenCredentialsPublisher.PublishingService.Data.PublishRequest", "PublishRequest")
                        .WithMany("AccessKeys")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OpenCredentialsPublisher.PublishingService.Data.File", b =>
                {
                    b.HasOne("OpenCredentialsPublisher.PublishingService.Data.PublishRequest", "PublishRequest")
                        .WithMany("Files")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OpenCredentialsPublisher.PublishingService.Data.SigningKey", b =>
                {
                    b.HasOne("OpenCredentialsPublisher.PublishingService.Data.PublishRequest", "PublishRequest")
                        .WithMany("SigningKeys")
                        .HasForeignKey("RequestId");
                });
#pragma warning restore 612, 618
        }
    }
}
