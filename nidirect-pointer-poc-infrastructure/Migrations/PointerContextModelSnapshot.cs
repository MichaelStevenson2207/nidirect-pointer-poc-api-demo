﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using nidirect_pointer_poc_infrastructure.Data;

#nullable disable

namespace nidirect_pointer_poc_infrastructure.Migrations
{
    [DbContext(typeof(PointerContext))]
    partial class PointerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("nidirect_pointer_poc_core.Entities.Pointer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AltThorfareName1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Archived_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Blpu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BuildingName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BuildingNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BuildingStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Classification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Commencement_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("County")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Creation_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocalCouncil")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Locality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostTown")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postcode")
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("PrimaryThorfare")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondaryThorfare")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubBuildingName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TempCoords")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Town")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TownLand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Udprn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UniqueBuildingId")
                        .HasColumnType("int");

                    b.Property<int?>("Uprn")
                        .HasColumnType("int");

                    b.Property<int?>("Usrn")
                        .HasColumnType("int");

                    b.Property<int?>("XCor")
                        .HasColumnType("int");

                    b.Property<int?>("Ycor")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Pointer");
                });
#pragma warning restore 612, 618
        }
    }
}
