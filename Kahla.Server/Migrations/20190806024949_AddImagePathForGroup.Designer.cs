﻿// <auto-generated />
using Kahla.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Kahla.Server.Migrations
{
    [DbContext(typeof(KahlaDbContext))]
    [Migration("20190806024949_AddImagePathForGroup")]
    partial class AddImagePathForGroup
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Kahla.Server.Models.At", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MessageId");

                    b.Property<string>("TargetUserId");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("TargetUserId");

                    b.ToTable("Ats");
                });

            modelBuilder.Entity("Kahla.Server.Models.Conversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AESKey");

                    b.Property<DateTime>("ConversationCreateTime");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("MaxLiveSeconds");

                    b.HasKey("Id");

                    b.ToTable("Conversations");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Conversation");
                });

            modelBuilder.Entity("Kahla.Server.Models.Device", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime");

                    b.Property<string>("IPAddress");

                    b.Property<string>("Name");

                    b.Property<string>("PushAuth");

                    b.Property<string>("PushEndpoint");

                    b.Property<string>("PushP256DH");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Kahla.Server.Models.KahlaUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime>("AccountCreateTime");

                    b.Property<string>("Bio");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("ConnectKey");

                    b.Property<int>("CurrentChannel");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("EnableEmailNotification");

                    b.Property<int>("HeadImgFileKey");

                    b.Property<string>("IconFilePath");

                    b.Property<DateTime>("LastEmailHimTime");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<bool>("MakeEmailPublic");

                    b.Property<string>("NickName");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PreferedLanguage");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Sex");

                    b.Property<int>("ThemeId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Kahla.Server.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<int>("ConversationId");

                    b.Property<bool>("Read");

                    b.Property<DateTime>("SendTime");

                    b.Property<string>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Kahla.Server.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Reason");

                    b.Property<int>("Status");

                    b.Property<string>("TargetId");

                    b.Property<string>("TriggerId");

                    b.HasKey("Id");

                    b.HasIndex("TargetId");

                    b.HasIndex("TriggerId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Kahla.Server.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Completed");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("CreatorId");

                    b.Property<string>("TargetId");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("TargetId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Kahla.Server.Models.UserGroupRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId");

                    b.Property<DateTime>("JoinTime");

                    b.Property<bool>("Muted");

                    b.Property<DateTime>("ReadTimeStamp");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroupRelations");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Kahla.Server.Models.GroupConversation", b =>
                {
                    b.HasBaseType("Kahla.Server.Models.Conversation");

                    b.Property<int>("GroupImageKey");

                    b.Property<string>("GroupImagePath");

                    b.Property<string>("GroupName");

                    b.Property<string>("JoinPassword");

                    b.Property<string>("OwnerId");

                    b.HasIndex("OwnerId");

                    b.HasDiscriminator().HasValue("GroupConversation");
                });

            modelBuilder.Entity("Kahla.Server.Models.PrivateConversation", b =>
                {
                    b.HasBaseType("Kahla.Server.Models.Conversation");

                    b.Property<string>("RequesterId");

                    b.Property<string>("TargetId");

                    b.HasIndex("RequesterId");

                    b.HasIndex("TargetId");

                    b.HasDiscriminator().HasValue("PrivateConversation");
                });

            modelBuilder.Entity("Kahla.Server.Models.At", b =>
                {
                    b.HasOne("Kahla.Server.Models.Message", "Message")
                        .WithMany("Ats")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Kahla.Server.Models.KahlaUser", "TargetUser")
                        .WithMany("ByAts")
                        .HasForeignKey("TargetUserId");
                });

            modelBuilder.Entity("Kahla.Server.Models.Device", b =>
                {
                    b.HasOne("Kahla.Server.Models.KahlaUser", "KahlaUser")
                        .WithMany("HisDevices")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Kahla.Server.Models.Message", b =>
                {
                    b.HasOne("Kahla.Server.Models.Conversation", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Kahla.Server.Models.KahlaUser", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("Kahla.Server.Models.Report", b =>
                {
                    b.HasOne("Kahla.Server.Models.KahlaUser", "Target")
                        .WithMany("ByReported")
                        .HasForeignKey("TargetId");

                    b.HasOne("Kahla.Server.Models.KahlaUser", "Trigger")
                        .WithMany("Reported")
                        .HasForeignKey("TriggerId");
                });

            modelBuilder.Entity("Kahla.Server.Models.Request", b =>
                {
                    b.HasOne("Kahla.Server.Models.KahlaUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.HasOne("Kahla.Server.Models.KahlaUser", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId");
                });

            modelBuilder.Entity("Kahla.Server.Models.UserGroupRelation", b =>
                {
                    b.HasOne("Kahla.Server.Models.GroupConversation", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Kahla.Server.Models.KahlaUser", "User")
                        .WithMany("GroupsJoined")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Kahla.Server.Models.KahlaUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Kahla.Server.Models.KahlaUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Kahla.Server.Models.KahlaUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Kahla.Server.Models.KahlaUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Kahla.Server.Models.GroupConversation", b =>
                {
                    b.HasOne("Kahla.Server.Models.KahlaUser", "Owner")
                        .WithMany("GroupsOwned")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("Kahla.Server.Models.PrivateConversation", b =>
                {
                    b.HasOne("Kahla.Server.Models.KahlaUser", "RequestUser")
                        .WithMany("Friends")
                        .HasForeignKey("RequesterId");

                    b.HasOne("Kahla.Server.Models.KahlaUser", "TargetUser")
                        .WithMany("OfFriends")
                        .HasForeignKey("TargetId");
                });
#pragma warning restore 612, 618
        }
    }
}
