﻿using Hovedopgave.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Hovedopgave.Migrations
{
    [DbContext(typeof(HovedopgaveContext))]
    partial class HovedopgaveContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Hovedopgave.Models.NotificationSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("EmailNotificationsEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("Frequency")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("NotificationSetting");
                });

            modelBuilder.Entity("Hovedopgave.Models.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("LocationX")
                        .HasColumnType("float");

                    b.Property<double>("LocationY")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Station");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            LocationX = 1.0,
                            LocationY = 2.0,
                            Name = "Station 1",
                            Notes = "This is station 1"
                        },
                        new
                        {
                            Id = 2,
                            LocationX = 3.0,
                            LocationY = 4.0,
                            Name = "Station 2",
                            Notes = "This is station 2"
                        });
                });

            modelBuilder.Entity("Hovedopgave.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Ticket");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2024, 12, 15, 23, 0, 14, 345, DateTimeKind.Local).AddTicks(9816),
                            Description = "Ticket 1",
                            IsFinished = false,
                            LastUpdated = new DateTime(2024, 12, 15, 23, 0, 14, 345, DateTimeKind.Local).AddTicks(9878),
                            Priority = 1
                        });
                });

            modelBuilder.Entity("Hovedopgave.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Initials")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@admin.dk",
                            FullName = "Admin Adminsen",
                            Initials = "AA",
                            Password = "admin123",
                            Role = "Admin",
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Email = "user@user.dk",
                            FullName = "User Useren",
                            Initials = "UU",
                            Password = "password123",
                            Role = "User",
                            Username = "user1"
                        },
                        new
                        {
                            Id = 3,
                            Email = "felt@felt.dk",
                            FullName = "Felt Feltsen",
                            Initials = "FF",
                            Password = "password123",
                            Role = "Felt",
                            Username = "felt1"
                        },
                        new
                        {
                            Id = 4,
                            Email = "kontor@kontor.dk",
                            FullName = "Kontor Kontorsen",
                            Initials = "KK",
                            Password = "password123",
                            Role = "Kontor",
                            Username = "kontor1"
                        });
                });

            modelBuilder.Entity("TicketUser", b =>
                {
                    b.Property<int>("TicketsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("TicketsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("TicketUser", (string)null);
                });

            modelBuilder.Entity("Hovedopgave.Models.NotificationSetting", b =>
                {
                    b.HasOne("Hovedopgave.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TicketUser", b =>
                {
                    b.HasOne("Hovedopgave.Models.Ticket", null)
                        .WithMany()
                        .HasForeignKey("TicketsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hovedopgave.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
