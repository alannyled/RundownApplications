using TemplateDbService.Models;

namespace TemplateDbService.DemoData
{
    public static class TemplateDemoData
    {
        public static List<RundownTemplate> TemplateData()
        {
            var templates = new List<RundownTemplate>
                {
                    new() { UUID = Guid.NewGuid() ,Name = "TVA 1830", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] },                    
                    new() { UUID = Guid.NewGuid() ,Name = "21 Søndag", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] },
                    new() { UUID = Guid.NewGuid() ,Name = "Time Nyheder", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] },
                    new() { UUID = Guid.NewGuid() ,Name = "Deadline", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] },
                    new() { UUID = Guid.NewGuid() ,Name = "Krigens døgn", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] },
                    new() { UUID = Guid.NewGuid() ,Name = "Tom skabelon", CreatedDate = DateTime.Now, ArchivedDate = null, Items = [] }
                };
            return templates;
        }
    }
    }
