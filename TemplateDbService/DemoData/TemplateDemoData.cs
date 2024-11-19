using TemplateDbService.Models;

namespace TemplateDbService.DemoData
{
    public static class TemplateDemoData
    {
        public static List<RundownTemplate> TemplateData()
        {
            DateTime now = DateTime.Now;
            var Rundown1Uuid = Guid.NewGuid();
            var Rundown2Uuid = Guid.NewGuid();
            var Rundown3Uuid = Guid.NewGuid();
            var Rundown4Uuid = Guid.NewGuid();
            var Rundown5Uuid = Guid.NewGuid();
            var Item1Uuid = Guid.NewGuid();
            var Item2Uuid = Guid.NewGuid();
            var Item3Uuid = Guid.NewGuid();
            var Item4Uuid = Guid.NewGuid();
            var Item5Uuid = Guid.NewGuid();

            var templates = new List<RundownTemplate>
                {
                    new() { UUID = Guid.NewGuid() ,Name = "TVA 1830", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] },                    
                    new() { UUID = Guid.NewGuid() ,Name = "21 Søndag", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] },
                    new() { UUID = Guid.NewGuid() ,Name = "Time Nyheder", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] },
                    new() { UUID = Guid.NewGuid() ,Name = "Deadline", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] },
                    new() { UUID = Guid.NewGuid() ,Name = "Krigens døgn", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] }
                };
            return templates;
        }
    }
    }
