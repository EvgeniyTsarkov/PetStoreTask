using System.Collections.Generic;

namespace PetStoreTask.Business
{
    public class PetFactory
    {
        private readonly Pet _testPet = new Pet()
        {
            Category = new Category()
            {
                Id = 123456,
                Name = "Test Project Pets"
            },
            Name = "Katy Purry",
            Tags = new List<Tag>()
            {
                new Tag()
                {
                    Id = 9876542,
                    Name = "exceptional test pets"
                }
            },
            Status = Status.Available
        };

        public Pet GetTestPet() => _testPet;
    }
}
