using Biker_Keeper_Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data
{
    public partial class BaseContext: DbContext
    {
        public BaseContext() { }
        public BaseContext(DbContextOptions<BaseContext> options) : base(options) { }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<Park> Park { get; set; }
        public virtual DbSet<Park_Transaction> Park_Transaction { get; set; }
        public virtual DbSet<Parking_Card> Parking_Card { get; set; }
        public virtual DbSet<ParkKindVehicle> ParkKindVehicle { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RolesPages> RolesPages { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersRoles> UsersRoles { get; set; }
        public virtual DbSet<VehicleType> VehicleType { get; set; }
    }
}
