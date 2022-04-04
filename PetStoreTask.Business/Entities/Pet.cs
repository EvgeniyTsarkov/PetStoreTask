using System.Collections.Generic;

namespace PetStoreTask.Business
{
    public class Pet
    {
        public long Id { get; set; }

        public Category Category { get; set; }

        public string Name { get; set; }

        public List<string> PhotoUrls { get; set; }

        public List<Tag> Tags { get; set; }

        public Status Status { get; set; }
    }
}
