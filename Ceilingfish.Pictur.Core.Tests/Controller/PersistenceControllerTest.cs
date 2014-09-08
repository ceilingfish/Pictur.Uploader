using System.Data.Objects;
using Ceilingfish.Pictur.Core.Controller;
using NUnit.Framework;

namespace Ceilingfish.Pictur.Core.Tests.Controller
{
    [TestFixture]
    public class PersistenceControllerTest
    {
        [Test]
        public void LoadAllDirectories()
        {
            var controller = new PersistenceController("test.db");

            var dirs = controller.ManagedDirectories.Select("id", new ObjectParameter("id",1));
        }
    }
}
