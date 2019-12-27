using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tectransit.Modles
{
    public partial class TECTRANSITDBContext : DbContext
    {
        public TECTRANSITDBContext()
        {
        }

        public TECTRANSITDBContext(DbContextOptions<TECTRANSITDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TDBanner> TDBanner { get; set; }
        public virtual DbSet<TDFaqD> TDFaqD { get; set; }
        public virtual DbSet<TDFaqH> TDFaqH { get; set; }
        public virtual DbSet<TDNews> TDNews { get; set; }
        public virtual DbSet<TNDeclarant> TNDeclarant { get; set; }
        public virtual DbSet<TNPackage> TNPackage { get; set; }
        public virtual DbSet<TNShippingH> TNShippingH { get; set; }
        public virtual DbSet<TSAccount> TSAccount { get; set; }
        public virtual DbSet<TSAcdeclarantmap> TSAcdeclarantmap { get; set; }
        public virtual DbSet<TSAclog> TSAclog { get; set; }
        public virtual DbSet<TSAcloginlog> TSAcloginlog { get; set; }
        public virtual DbSet<TSAcrankmap> TSAcrankmap { get; set; }
        public virtual DbSet<TSButton> TSButton { get; set; }
        public virtual DbSet<TSDeclarant> TSDeclarant { get; set; }
        public virtual DbSet<TSMenu> TSMenu { get; set; }
        public virtual DbSet<TSRank> TSRank { get; set; }
        public virtual DbSet<TSRole> TSRole { get; set; }
        public virtual DbSet<TSRolebuttonmap> TSRolebuttonmap { get; set; }
        public virtual DbSet<TSRolemenumap> TSRolemenumap { get; set; }
        public virtual DbSet<TSUser> TSUser { get; set; }
        public virtual DbSet<TSUserlog> TSUserlog { get; set; }
        public virtual DbSet<TSUserloginlog> TSUserloginlog { get; set; }
        public virtual DbSet<TSUserrolemap> TSUserrolemap { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=SiaWu-NB;Initial Catalog=TECTRANSITDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TDBanner>(entity =>
            {
                entity.ToTable("T_D_BANNER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Banseq)
                    .HasColumnName("BANSEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descr)
                    .HasColumnName("DESCR")
                    .HasMaxLength(100);

                entity.Property(e => e.Imgurl)
                    .HasColumnName("IMGURL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Istop).HasColumnName("ISTOP");

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(200);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TDFaqD>(entity =>
            {
                entity.ToTable("T_D_FAQ_D");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descr).HasColumnName("DESCR");

                entity.Property(e => e.Faqdseq)
                    .HasColumnName("FAQDSEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Istop).HasColumnName("ISTOP");

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(200);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TDFaqH>(entity =>
            {
                entity.ToTable("T_D_FAQ_H");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descr).HasColumnName("DESCR");

                entity.Property(e => e.Faqhseq)
                    .HasColumnName("FAQHSEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Istop).HasColumnName("ISTOP");

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(200);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TDNews>(entity =>
            {
                entity.ToTable("T_D_NEWS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descr).HasColumnName("DESCR");

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Istop).HasColumnName("ISTOP");

                entity.Property(e => e.Newsseq)
                    .HasColumnName("NEWSSEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(200);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TNDeclarant>(entity =>
            {
                entity.ToTable("T_N_DECLARANT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Declarantid).HasColumnName("DECLARANTID");

                entity.Property(e => e.ShippingidH).HasColumnName("SHIPPINGID_H");

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TNPackage>(entity =>
            {
                entity.ToTable("T_N_PACKAGE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Packname)
                    .HasColumnName("PACKNAME")
                    .HasMaxLength(500);

                entity.Property(e => e.Packtype)
                    .HasColumnName("PACKTYPE")
                    .HasMaxLength(100);

                entity.Property(e => e.Packurl)
                    .HasColumnName("PACKURL")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity)
                    .HasColumnName("QUANTITY")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingidH).HasColumnName("SHIPPINGID_H");

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("UNIT_PRICE")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TNShippingH>(entity =>
            {
                entity.ToTable("T_N_SHIPPING_H");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Accountid).HasColumnName("ACCOUNTID");

                entity.Property(e => e.Combinetype).HasColumnName("COMBINETYPE");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Exportdate)
                    .HasColumnName("EXPORTDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Importdate)
                    .HasColumnName("IMPORTDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Oldtrasferno)
                    .HasColumnName("OLDTRASFERNO")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PHeight)
                    .HasColumnName("P_HEIGHT")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PLength)
                    .HasColumnName("P_LENGTH")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PSource)
                    .HasColumnName("P_SOURCE")
                    .HasMaxLength(1000);

                entity.Property(e => e.PTrackingno)
                    .HasColumnName("P_TRACKINGNO")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PWeight)
                    .HasColumnName("P_WEIGHT")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PWidth)
                    .HasColumnName("P_WIDTH")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Paydate)
                    .HasColumnName("PAYDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Paystatus).HasColumnName("PAYSTATUS");

                entity.Property(e => e.Paytype)
                    .HasColumnName("PAYTYPE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Receiver)
                    .HasColumnName("RECEIVER")
                    .HasMaxLength(50);

                entity.Property(e => e.ReceiverAddr)
                    .HasColumnName("RECEIVER_ADDR")
                    .HasMaxLength(500);

                entity.Property(e => e.Remark1).HasColumnName("REMARK1");

                entity.Property(e => e.Remark2).HasColumnName("REMARK2");

                entity.Property(e => e.Remark3).HasColumnName("REMARK3");

                entity.Property(e => e.Status).HasColumnName("STATUS");

                entity.Property(e => e.Total)
                    .HasColumnName("TOTAL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Trackingdesc).HasColumnName("TRACKINGDESC");

                entity.Property(e => e.Trackingno)
                    .HasColumnName("TRACKINGNO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Trackingremark).HasColumnName("TRACKINGREMARK");

                entity.Property(e => e.Trackingtype)
                    .HasColumnName("TRACKINGTYPE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Trasferno)
                    .IsRequired()
                    .HasColumnName("TRASFERNO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TSAccount>(entity =>
            {
                entity.ToTable("T_S_ACCOUNT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Addr)
                    .HasColumnName("ADDR")
                    .HasMaxLength(300);

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdphotoB)
                    .HasColumnName("IDPHOTO_B")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdphotoF)
                    .HasColumnName("IDPHOTO_F")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Lastlogindate)
                    .HasColumnName("LASTLOGINDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Logincount).HasColumnName("LOGINCOUNT");

                entity.Property(e => e.Mobile)
                    .HasColumnName("MOBILE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Taxid)
                    .HasColumnName("TAXID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Usercode)
                    .IsRequired()
                    .HasColumnName("USERCODE")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Userdesc)
                    .HasColumnName("USERDESC")
                    .HasMaxLength(1000);

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(50);

                entity.Property(e => e.Userpassword)
                    .HasColumnName("USERPASSWORD")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Userseq)
                    .HasColumnName("USERSEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Warehouseno)
                    .HasColumnName("WAREHOUSENO")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TSAcdeclarantmap>(entity =>
            {
                entity.ToTable("T_S_ACDECLARANTMAP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Declarantid).HasColumnName("DECLARANTID");

                entity.Property(e => e.Usercode)
                    .HasColumnName("USERCODE")
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TSAclog>(entity =>
            {
                entity.ToTable("T_S_ACLOG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LogDate)
                    .HasColumnName("LOG_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Message).HasColumnName("MESSAGE");

                entity.Property(e => e.Position)
                    .HasColumnName("POSITION")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Target)
                    .HasColumnName("TARGET")
                    .HasMaxLength(100);

                entity.Property(e => e.Usercode)
                    .HasColumnName("USERCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TSAcloginlog>(entity =>
            {
                entity.ToTable("T_S_ACLOGINLOG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Hostip)
                    .HasColumnName("HOSTIP")
                    .HasMaxLength(100);

                entity.Property(e => e.Hostname)
                    .HasColumnName("HOSTNAME")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LoginDate)
                    .HasColumnName("LOGIN_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Usercode)
                    .HasColumnName("USERCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TSAcrankmap>(entity =>
            {
                entity.ToTable("T_S_ACRANKMAP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Rankid).HasColumnName("RANKID");

                entity.Property(e => e.Usercode)
                    .HasColumnName("USERCODE")
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TSButton>(entity =>
            {
                entity.ToTable("T_S_BUTTON");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Buttoncode)
                    .IsRequired()
                    .HasColumnName("BUTTONCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Buttondesc)
                    .HasColumnName("BUTTONDESC")
                    .HasMaxLength(100);

                entity.Property(e => e.Buttonevent)
                    .HasColumnName("BUTTONEVENT")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Buttonname)
                    .HasColumnName("BUTTONNAME")
                    .HasMaxLength(50);

                entity.Property(e => e.Buttonseq)
                    .HasColumnName("BUTTONSEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Buttontype)
                    .HasColumnName("BUTTONTYPE")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iconclass)
                    .HasColumnName("ICONCLASS")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Menucode)
                    .HasColumnName("MENUCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TSDeclarant>(entity =>
            {
                entity.ToTable("T_S_DECLARANT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Addr)
                    .HasColumnName("ADDR")
                    .HasMaxLength(200);

                entity.Property(e => e.Appointment)
                    .HasColumnName("APPOINTMENT")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdphotoB)
                    .HasColumnName("IDPHOTO_B")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.IdphotoF)
                    .HasColumnName("IDPHOTO_F")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasColumnName("MOBILE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Taxid)
                    .HasColumnName("TAXID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type).HasColumnName("TYPE");

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TSMenu>(entity =>
            {
                entity.ToTable("T_S_MENU");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iconurl)
                    .HasColumnName("ICONURL")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Isback).HasColumnName("ISBACK");

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Isvisible).HasColumnName("ISVISIBLE");

                entity.Property(e => e.Menucode)
                    .IsRequired()
                    .HasColumnName("MENUCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Menudesc)
                    .HasColumnName("MENUDESC")
                    .HasMaxLength(1000);

                entity.Property(e => e.Menuname)
                    .HasColumnName("MENUNAME")
                    .HasMaxLength(500);

                entity.Property(e => e.Menuseq)
                    .HasColumnName("MENUSEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Menuurl)
                    .HasColumnName("MENUURL")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Parentcode)
                    .IsRequired()
                    .HasColumnName("PARENTCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TSRank>(entity =>
            {
                entity.ToTable("T_S_RANK");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Rankcode)
                    .IsRequired()
                    .HasColumnName("RANKCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Rankdesc)
                    .HasColumnName("RANKDESC")
                    .HasMaxLength(1000);

                entity.Property(e => e.Rankname)
                    .HasColumnName("RANKNAME")
                    .HasMaxLength(50);

                entity.Property(e => e.Rankseq)
                    .HasColumnName("RANKSEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Ranktype).HasColumnName("RANKTYPE");

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TSRole>(entity =>
            {
                entity.ToTable("T_S_ROLE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Rolecode)
                    .IsRequired()
                    .HasColumnName("ROLECODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Roledesc)
                    .HasColumnName("ROLEDESC")
                    .HasMaxLength(1000);

                entity.Property(e => e.Rolename)
                    .HasColumnName("ROLENAME")
                    .HasMaxLength(50);

                entity.Property(e => e.Roleseq)
                    .HasColumnName("ROLESEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TSRolebuttonmap>(entity =>
            {
                entity.ToTable("T_S_ROLEBUTTONMAP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Buttonid).HasColumnName("BUTTONID");

                entity.Property(e => e.Rolecode)
                    .HasColumnName("ROLECODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TSRolemenumap>(entity =>
            {
                entity.ToTable("T_S_ROLEMENUMAP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Menucode)
                    .HasColumnName("MENUCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Rolecode)
                    .HasColumnName("ROLECODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TSUser>(entity =>
            {
                entity.ToTable("T_S_USER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Createby)
                    .HasColumnName("CREATEBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Credate)
                    .HasColumnName("CREDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Isenable).HasColumnName("ISENABLE");

                entity.Property(e => e.Isresetpw).HasColumnName("ISRESETPW");

                entity.Property(e => e.Lastlogindate)
                    .HasColumnName("LASTLOGINDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Logincount).HasColumnName("LOGINCOUNT");

                entity.Property(e => e.Updby)
                    .HasColumnName("UPDBY")
                    .HasMaxLength(300);

                entity.Property(e => e.Upddate)
                    .HasColumnName("UPDDATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Usercode)
                    .IsRequired()
                    .HasColumnName("USERCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Userdesc)
                    .HasColumnName("USERDESC")
                    .HasMaxLength(1000);

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(50);

                entity.Property(e => e.Userpassword)
                    .HasColumnName("USERPASSWORD")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Userseq)
                    .HasColumnName("USERSEQ")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TSUserlog>(entity =>
            {
                entity.ToTable("T_S_USERLOG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LogDate)
                    .HasColumnName("LOG_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Message).HasColumnName("MESSAGE");

                entity.Property(e => e.Position)
                    .HasColumnName("POSITION")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Target)
                    .HasColumnName("TARGET")
                    .HasMaxLength(100);

                entity.Property(e => e.Usercode)
                    .HasColumnName("USERCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TSUserloginlog>(entity =>
            {
                entity.ToTable("T_S_USERLOGINLOG");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Hostip)
                    .HasColumnName("HOSTIP")
                    .HasMaxLength(100);

                entity.Property(e => e.Hostname)
                    .HasColumnName("HOSTNAME")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LoginDate)
                    .HasColumnName("LOGIN_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Usercode)
                    .HasColumnName("USERCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("USERNAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TSUserrolemap>(entity =>
            {
                entity.ToTable("T_S_USERROLEMAP");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Rolecode)
                    .HasColumnName("ROLECODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Usercode)
                    .HasColumnName("USERCODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
