using System;

namespace Flyable.Entities
{
    public class PostRating
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public DateTime RatedAt { get; set; }
    }
}