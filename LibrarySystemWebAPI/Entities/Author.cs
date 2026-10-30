namespace LibrarySystemWebAPI.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
