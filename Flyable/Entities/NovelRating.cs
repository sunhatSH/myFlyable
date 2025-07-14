using System;

namespace Flyable.Entities
{
    public class NovelRating
    {
        public int Id { get; set; }
        public int NovelId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public DateTime RatedAt { get; set; }
    }
}