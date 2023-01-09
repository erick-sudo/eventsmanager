namespace ThAmCo.Catering.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public List<MenuFoodItem> MenuItems { get; set; }
    }
}
