using RundownDbService.Models;

namespace RundownDbService.DemoData
{
    public static class RundownDemoData
    {
        public static List<Rundown> RundownData()
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

            var rundowns = new List<Rundown>
                {
                    new() { UUID = Rundown1Uuid ,Name = "TVA 1830", ControlRoomId = Guid.Parse("ebf89c25-90c8-4a2e-bdca-91db0eb39c93"), BroadcastDate = now.AddDays(5), Items = [
                        new RundownItem { UUID = Item1Uuid, RundownId = Rundown1Uuid, Name = "Intro", Duration = TimeSpan.FromSeconds(12) , Order = 0, Details = [
                            new ItemDetailGraphic { UUID = Guid.NewGuid(), ItemId = Item1Uuid, Type= "Grafik", Title="Intro grafik", Duration = TimeSpan.FromSeconds(5), Order = 0, GraphicId = "Jingle + Wipe"}

                            ]},
                        new RundownItem { UUID = Item2Uuid, RundownId = Rundown1Uuid, Name = "Velkomst", Duration = TimeSpan.FromSeconds(15), Order = 1, Details = [
                            new ItemDetailTeleprompter { UUID = Guid.NewGuid(), ItemId = Item2Uuid, Type= "Teleprompter", Title="Velkommen", Duration = TimeSpan.FromSeconds(8), Order = 0, PrompterText = "Klokken er 1830, Velkommen til TV Avisen"},
                            new ItemDetailVideo { UUID = Guid.NewGuid(), ItemId = Item3Uuid, Type= "Video", Title="Kommuner skal spare", Duration = TimeSpan.FromSeconds(12), Order = 1, VideoPath = "Ingen video tilknyttet"},
                            new ItemDetailTeleprompter { UUID = Guid.NewGuid(), ItemId = Item4Uuid, Type= "Voiceover", Title="VO Kommuner", Duration = TimeSpan.FromSeconds(12), Order = 0, PrompterText = "Der skal spares millioner i kommunerne. Borgmester frygter at det vil påvirke årets julefrokost på rådhuset"}
                            ] },
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Vejr", Duration = TimeSpan.FromSeconds(113), Order = 2, Details = [] },
                        new RundownItem { UUID = Guid.NewGuid(), RundownId = Rundown1Uuid, Name = "Sport", Duration = TimeSpan.FromSeconds(198), Order = 3, Details = [] }
                    ] },
                    new() { UUID = Rundown2Uuid, Name = "21 Søndag", ControlRoomId = Guid.Parse("7b4c4fe6-2d9e-4276-949f-79cac408858c"), BroadcastDate = now, Items = [] },
                    new() { UUID = Rundown3Uuid, Name = "Nyheder 1200", ControlRoomId = Guid.Parse("ebf89c25-90c8-4a2e-bdca-91db0eb39c93"), BroadcastDate = now.AddDays(2), Items = [] },
                    new() { UUID = Rundown4Uuid, Name = "Deadline", ControlRoomId = Guid.Parse("7b4c4fe6-2d9e-4276-949f-79cac408858c"), BroadcastDate = now.AddDays(-5), ArchivedDate = now, Items = [] },
                    new() { UUID = Rundown5Uuid, Name = "Nyheder 0900", ControlRoomId = Guid.Parse("ebf89c25-90c8-4a2e-bdca-91db0eb39c93"), BroadcastDate = now.AddDays(-2), ArchivedDate = now, Items = [] }

                };

            return rundowns;

        }
    }
}
