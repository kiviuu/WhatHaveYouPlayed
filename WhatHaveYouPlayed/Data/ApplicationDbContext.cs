using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WhatHaveYouPlayed.Models;

namespace WhatHaveYouPlayed.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Producent> Producents { get; set; }
    public DbSet<ProgressState> ProgressStates { get; set; }
    public DbSet<GameData> GameDatas { get; set; }
    public DbSet<UserGameData> UsersGamesDatas { get; set; }
    public DbSet<UserMessage> UsersMessages { get; set; }


    public DbSet<ApplicationUser> AspNetUsers { get; set; }
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        
        //columns configurations
        //builder.Entity<BlogPost>().ToTable("BlogPosts");
        builder.Entity<BlogPost>().HasKey(bg => bg.PostId);
        builder.Entity<BlogPost>().Property(bg => bg.PostId).HasMaxLength(450);
        builder.Entity<BlogPost>().Property(p => p.Title).IsRequired().HasMaxLength(50);
        builder.Entity<BlogPost>().Property(p => p.PostDate).IsRequired();
        builder.Entity<BlogPost>().Property(p => p.Content).IsRequired().HasColumnType("text");
        builder.Entity<BlogPost>().Property(p => p.AuthorId).IsRequired().HasMaxLength(450);
        builder.Entity<BlogPost>().Property(p => p.PostImgSrc).HasMaxLength(450);
        //builder.Entity<BlogPost>().Property(prop => prop.Content).UseCollation("LATIN1_GENERAL_100_CI_AS_SC_UTF8");

        //builder.Entity<Producent>().ToTable("Producents");
        builder.Entity<Producent>().HasKey(prod => prod.ProdId);
        builder.Entity<Producent>().Property(p => p.CompanyName).IsRequired().HasMaxLength(50);

        //builder.Entity<ProgressState>().ToTable("ProgressStates");
        builder.Entity<ProgressState>().HasKey(prog => prog.ProgressId);
        builder.Entity<ProgressState>().Property(p => p.State).IsRequired().HasMaxLength(20);

        //builder.Entity<GameData>().ToTable("GameDatas");
        builder.Entity<GameData>().HasKey(gd => gd.GameId);
        builder.Entity<GameData>().Property(id => id.GameId).HasMaxLength(450);
        builder.Entity<GameData>().Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Entity<GameData>().Property(p => p.ProducentId).IsRequired();
        builder.Entity<GameData>().Property(p => p.ImgSrc).IsRequired().HasMaxLength(450);

        //builder.Entity<UserGameData>().ToTable("UsersGamesDatas");
        builder.Entity<UserGameData>().HasKey(usg => usg.DataId);
        builder.Entity<UserGameData>().Property(p => p.UserId).IsRequired();
        builder.Entity<UserGameData>().Property(p => p.GameId).IsRequired().HasMaxLength(450);
        builder.Entity<UserGameData>().Property(p => p.StateId).IsRequired().HasDefaultValue(1);
        builder.Entity<UserGameData>().Property(p => p.AddDate).IsRequired();
        //builder.Entity<UserGameData>().Property(p => p.IsComplet).HasDefaultValue(false);
        builder.Entity<UserGameData>().Property(p => p.UserId).IsRequired().HasMaxLength(450);


        builder.Entity<UserMessage>().HasKey(um => um.MessageId);
        builder.Entity<UserMessage>().Property(p => p.Topic).IsRequired().HasMaxLength(100);
        builder.Entity<UserMessage>().Property(p => p.UserId).IsRequired().HasMaxLength(450);
        builder.Entity<UserMessage>().Property(p => p.Content).IsRequired().HasColumnType("text");
        builder.Entity<UserMessage>().Property(p => p.CreateTime).IsRequired();



        //relations
        builder.Entity<GameData>()
            .HasOne<Producent>(producent => producent.Producent)
            .WithMany(gdata => gdata.GameData)
            .HasForeignKey(fg => fg.ProducentId);

        builder.Entity<UserGameData>()
            .HasOne<GameData>(gd => gd.Game)
            .WithMany(ugd => ugd.UserGameData)
            .HasForeignKey(fg => fg.GameId);

        builder.Entity<UserGameData>()
            .HasOne<ProgressState>(ps => ps.State)
            .WithMany(usg => usg.UserGameData)
            .HasForeignKey(fg => fg.StateId);



        builder.Entity<BlogPost>()
            .HasOne<ApplicationUser>(ap => ap.Author)
            .WithMany(bp => bp.BlogPosts)
            .HasForeignKey(fg => fg.AuthorId);

        builder.Entity<UserGameData>()
            .HasOne<ApplicationUser>(ap => ap.User)
            .WithMany(usg => usg.UserGameDatas)
            .HasForeignKey(fg => fg.UserId);

        builder.Entity<UserMessage>()
            .HasOne<ApplicationUser>(ap => ap.User)
            .WithMany(usg => usg.UserMessages)
            .HasForeignKey(fg => fg.UserId);
    }
}
