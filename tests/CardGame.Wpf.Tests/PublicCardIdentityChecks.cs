using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CardGame.Core;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Presentation;
using CardGame.Wpf.ViewModels;
using static Program;

internal static class PublicCardIdentityChecks
{
    public static void MultiMaterialResponses()
    {
        using var vm = new MainViewModel(false, 721019, false, new MemorySaveStore());
        var view = Engine(vm).CreateSnapshot(0);
        foreach (var kind in new[] { CardKind.Dodge, CardKind.Nullification })
        {
            long sequence = 0;
            EventEnvelope Event(IGameEvent payload) => new(new(++sequence), null, sequence, 1, "multi-response", payload);
            var events = new List<EventEnvelope>();
            foreach (var id in new[] { 41, 42, 43 })
                events.Add(Event(new CardRespondedEvent(id, 1, 0, kind)
                    { PublicCard = new(id, CardKind.Crossbow, Suit.Heart, id - 40, "诸葛连弩", (id - 40).ToString()) }));
            events.Add(Event(new ProgramViewAsConvertedEvent(9, "anniversary:boqia", "boqia-" + kind, 1, [41, 42, 43], kind, false, [0])));
            if (kind == CardKind.Nullification)
                events.Add(Event(new NullificationRespondedEvent(9, 50, CardKind.Snatch, 1, 41, true, 1)));
            var play = BattleCueProjector.ProjectCardPlays(events, view).Single();
            Assert(play.CardKind == kind && play.PublicCard is null && play.PublicCardId is null && play.Detail!.Contains("3张牌转化为一张"),
                "Three paid materials must display one virtual response without an arbitrary material's rank or suit.");
            Assert(BattleCueProjector.Project(events, view).Count(cue => cue.CardKind == kind && cue.Kind == BattleCueKind.Response) == 1,
                "A multi-material response must also have only one flying card face.");
            vm.RecentPlays.Clear();
            typeof(MainViewModel).GetMethod("RecordPublicPlays", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .Invoke(vm, [events, view]);
            Assert(vm.RecentPlays.Count == 1 && vm.RecentPlays[0].PublicCard is null,
                "The persistent table row must retain exactly the one virtual response.");
            var ordinary = events.Take(3).ToArray();
            Assert(BattleCueProjector.ProjectCardPlays(ordinary, view).Count == 3,
                "Independent ordinary responses without a conversion fact must not be combined.");
            events.AddRange(ordinary.Select(item => item with { Sequence = ++sequence }));
            Assert(BattleCueProjector.ProjectCardPlays(events, view).Count == 4,
                "A later response using the same card IDs must remain independent of the earlier converted action.");
        }
    }
}
