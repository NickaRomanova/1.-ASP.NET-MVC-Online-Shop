namespace Shop_v2.Context
{ 
    using System.Data.Entity; 
    using Shop_v2.Models; 

    public class Shop_version_2_Context : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Producer> Producers { get; set; }

        public DbSet<Category> Categories { get; set; } 

        public DbSet<Cart> Carts { get; set; } 
          
        public DbSet<ShoppingCartModel> ShoppingCartModels { get; set; } 

        public DbSet<HistoryOrder> HistoryOrder { get; set; }

        public Shop_version_2_Context()
            : base("name=Shop_version_2_Context")
        {
        } 
    }
}