using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class NativeAdditionalDiscardUiChecks
{
    public static void ShimouNativeResetClearsRecoveredAndLocalDraft(string output)
    {
        var scene = NativeAdditionalDiscardChecks.PrepareShimouReset();
        var originalFrame = scene.ShimouFrame!.Id;
        var original = scene.ShimouFrame.MouXunYuAnniversary!.Selected.Single();
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, scene.G.CreateCheckpoint()));
        using var model = new MainViewModel(false, 83, true, store, contentRegistry: scene.Registry)
            { IsMotionEnabled = false, IsMusicEnabled = false, IsSoundEnabled = false };
        LegacySaveFixtures.Load(model); scene.G = Program.Engine(model);
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        Program.Assert(!model.HasSaveError && model.IsOwnedCardSelectionPending &&
            scene.ShimouFrame?.MouXunYuAnniversary is { Stage: MouXyStage.Discard, Selected.Count: 1 } &&
            scene.P is { RequiredCardCount: 2, MinimumCardCount: 2 } && model.Hand.Single(c => c.Id == original).IsSelected,
            "Cold loading retains the one actual native Shimou pick and exposes exactly two remaining unpaid local selections.");
        var extra = scene.P!.Choices.Where(c => c.Parameters.GetValueOrDefault("owned-selection-role") == "pick")
            .Take(2).Select(c => c.Cards.Single()).ToArray();
        var commands = scene.G.AcceptedCommands.Count;
        var moves = scene.G.CardMovements.Count;
        foreach (var id in extra) model.SelectCardCommand.Execute(model.Hand.Single(c => c.Id == id));
        Program.Assert(extra.Length == 2 && model.CanConfirmSelected && model.Hand.Where(c => c.IsSelected).Select(c => c.Id)
            .ToHashSet().SetEquals(extra.Append(original)) && scene.G.AcceptedCommands.Count == commands && scene.G.CardMovements.Count == moves,
            "The recovered original pick and the new two-card local draft coexist without paying or sending either local singleton.");
        var reset = model.CenterSkillChoices.Single(c => c.Parameters.GetValueOrDefault("owned-selection-role") == "reset" &&
            c.Parameters.GetValueOrDefault("branch") == "clear");
        model.SelectSkillChoiceCommand.Execute(reset);
        Program.Assert(scene.G.AcceptedCommands.Count == commands + 1 && scene.G.AcceptedCommands.Last() is AnswerPromptCommand answer &&
            answer.Choice == reset.Id && scene.G.CardMovements.Count == moves && scene.ShimouFrame?.Id == originalFrame &&
            scene.ShimouFrame.MouXunYuAnniversary is { Stage: MouXyStage.Discard, Selected.Count: 0 } &&
            scene.P is { RequiredCardCount: 3, MinimumCardCount: 3 },
            "The real native reset answers only its current clear choice, empties the original native set and restores its unchanged frame's three-card requirement without payment.");
        Program.Assert(model.IsOwnedCardSelectionPending && model.Hand.All(c => !c.IsSelected) &&
            !model.HasSelection && !model.CanConfirmSelected,
            "A successful reset must also clear both local draft cards instead of retaining them across the same frame and stage.");
        model.SelectCardCommand.Execute(model.Hand.Single(c => c.Id == extra[0]));
        Program.Assert(model.Hand.Where(c => c.IsSelected).Select(c => c.Id).SequenceEqual([extra[0]]) &&
            !model.CanConfirmSelected && scene.G.AcceptedCommands.Count == commands + 1 && scene.G.CardMovements.Count == moves,
            "The first new pick after reset starts a fresh one-card local draft and cannot resurrect the second old local material.");

        LegacySaveFixtures.Load(model); scene.G = Program.Engine(model);
        Program.Assert(model.HasSelection && model.CanCancelAction && model.Hand.Single(c => c.Id == original).IsSelected &&
            scene.ShimouFrame?.MouXunYuAnniversary.Selected.Count == 1,
            "A recovered unpaid native pick alone must count as selection, enabling Cancel before another local card is chosen.");
        commands = scene.G.AcceptedCommands.Count; moves = scene.G.CardMovements.Count;
        model.CancelActionCommand.Execute(null);
        Program.Assert(scene.G.AcceptedCommands.Count == commands + 1 && scene.G.CardMovements.Count == moves &&
            scene.ShimouFrame?.Id == originalFrame && scene.ShimouFrame.MouXunYuAnniversary.Selected.Count == 0 &&
            model.Hand.All(c => !c.IsSelected) && !model.HasSelection && !model.CanConfirmSelected,
            "Cancel answers the original collector's native reset exactly once and clears the recovered physical pick without payment or advancing its stage.");

        LegacySaveFixtures.Load(model); scene.G = Program.Engine(model);
        foreach (var id in extra) model.SelectCardCommand.Execute(model.Hand.Single(c => c.Id == id));
        commands = scene.G.AcceptedCommands.Count; moves = scene.G.CardMovements.Count;
        model.ClearSelectionCommand.Execute(null);
        Program.Assert(scene.G.AcceptedCommands.Count == commands + 1 && scene.G.CardMovements.Count == moves &&
            scene.ShimouFrame?.Id == originalFrame && scene.ShimouFrame.MouXunYuAnniversary.Selected.Count == 0 &&
            model.Hand.All(c => !c.IsSelected) && !model.HasSelection && !model.CanConfirmSelected,
            "Escape's clear-selection command clears both the native recovered pick and both local draft cards using the same published reset, never paying a singleton.");
        Program.Render(host.Root, 1120, 740, Path.Combine(output, "shimou-native-reset-clears-local-draft.png"));
    }
}
