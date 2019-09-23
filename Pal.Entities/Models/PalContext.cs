
using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Entities.Models
{
    public partial class PalContext : DataContext
    {
        static PalContext()
        {
            Database.SetInitializer<PalContext>(null);
        }

        public PalContext()
            : base("Name=PalContext")
        {
        }

      //  public DbSet<Register> tmpRegister { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public override int SaveChanges(string userid)
        {


            if (ConfigurationManager.AppSettings["AuditLog"] != null && ConfigurationManager.AppSettings["AuditLog"].ToUpper() == "TRUE")
            {
                // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
                foreach (var ent in this.ChangeTracker.Entries()
                    .Where(p => p.State == EntityState.Added
                    || p.State == EntityState.Deleted
                    || p.State == EntityState.Modified))
                {
                    // For each changed record, get the audit record entries and add them
                    foreach (AuditLog x in GetAuditRecordsForChange(ent, userid))
                    {
                        x.ObjectState = ObjectState.Added;
                        base.Set<AuditLog>().Add(x);
                    }
                }
            }

            return base.SaveChanges();


        }

        private List<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userId)
        {
            List<AuditLog> result = new List<AuditLog>();

            DateTime changeTime = DateTime.UtcNow;

            // Get the Table() attribute, if one exists
            TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)

            var keyNames = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).FirstOrDefault();

            if (dbEntry.State == EntityState.Added)
            {
                // For Inserts, just add the whole record
                // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                result.Add(new AuditLog()
                {
                    AuditLogID = Guid.NewGuid(),
                    userid = userId,
                    eventdateutc = changeTime,
                    eventtype = "A", // Added
                    tablename = tableName,
                    recordid = dbEntry.CurrentValues.GetValue<object>(keyNames.Name).ToString(),  // Again, adjust this if you have a multi-column key
                    columnname = "*ALL",    // Or make it nullable, whatever you want
                    newvalue = dbEntry.CurrentValues.ToObject().ToString()
                }
                    );
            }
            else if (dbEntry.State == EntityState.Deleted)
            {
                // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                result.Add(new AuditLog()
                {
                    AuditLogID = Guid.NewGuid(),
                    userid = userId,
                    eventdateutc = changeTime,
                    eventtype = "D", // Deleted
                    tablename = tableName,
                    recordid = dbEntry.OriginalValues.GetValue<object>(keyNames.Name).ToString(),
                    columnname = "*ALL",
                    newvalue = dbEntry.OriginalValues.ToObject().ToString()
                }
                    );
            }
            else if (dbEntry.State == EntityState.Modified)
            {
                foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                {
                    var originalValue = dbEntry.GetDatabaseValues().GetValue<object>(propertyName);
                    // For updates, we only want to capture the columns that actually changed
                    if (!object.Equals(originalValue, dbEntry.CurrentValues.GetValue<object>(propertyName)))
                    {

                        result.Add(new AuditLog()
                        {
                            AuditLogID = Guid.NewGuid(),
                            userid = userId,
                            eventdateutc = changeTime,
                            eventtype = "M",    // Modified
                            tablename = tableName,
                            recordid = dbEntry.OriginalValues.GetValue<object>(keyNames.Name).ToString(),
                            columnname = propertyName,
                            originalvalue = originalValue == null ? null : originalValue.ToString(),
                            newvalue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                        }
                            );
                    }
                }
            }
            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities

            return result;
        }

        //public DbSet<Login> Logins { get; set; }
      //  public DbSet<Register> tmpRegister { get; set; }
        public DbSet<tblLists> tblLists { get; set; }
		public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<JobworkParty> JobworkParty { get; set; }
        public DbSet<Jobwork> Jobwork { get; set; }
        public DbSet<Outwards> Outwards { get; set; }
        public DbSet<OutwardsDetails> OutwardsDetails { get; set; }
    }
}
