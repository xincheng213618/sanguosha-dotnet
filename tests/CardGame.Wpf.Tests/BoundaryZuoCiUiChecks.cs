using CardGame.Wpf.Persistence;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;
using CardGame.Wpf.ViewModels;
using static BoundaryZuoCiScenario;

internal static class BoundaryZuoCiUiChecks
{
    public static void ActualPrivateExchangeBackAndSelectedDraftCold(string output)
    {
        var scene = CreateScene();
        using var model = ShiFifthBatchUiChecks.Model(scene.Game, scene.Registry);
        var game = Program.Engine(model);
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        var before = Library(game);
        var hidden = before.GeneralIds!.Where(id => id != before.RevealedGeneralId).Order(StringComparer.Ordinal).ToArray();
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.ShanheS5AvatarChoice is not null);
        Program.Assert(before.Count == 3 && hidden.Length == 2 && before.DeclaredSkillId == Borrowed,
            "The real mobile general begins with three original avatars and its actual borrowed skill.");
        Click(model, host, choice => Action(choice, "exchange"));
        Click(model, host, choice => Action(choice, "select", hidden[0]));
        Click(model, host, choice => Action(choice, "back"));
        Program.Assert(Library(game).GeneralIds!.SequenceEqual(before.GeneralIds!) &&
                game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == frame.Id).ShanheS5AvatarChoice is
                    { Stage: ShanheS5AvatarChoiceStage.Mode, SelectedGeneralIds.Count: 0 } &&
                !game.Events.Any(item => item.Payload is ShanheS5AvatarsExchangedEvent),
            "The actual Back button retires the unpaid selection and keeps all three original avatars.");
        Click(model, host, choice => Action(choice, "exchange"));
        foreach (var id in hidden) Click(model, host, choice => Action(choice, "select", id));
        LegacySaveFixtures.Save(model);
        LegacySaveFixtures.Load(model);
        game = Program.Engine(model);
        Program.Assert(!model.HasSaveError && Library(game).GeneralIds!.SequenceEqual(before.GeneralIds!) &&
                game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.Id == frame.Id).ShanheS5AvatarChoice!.SelectedGeneralIds.SequenceEqual(hidden) &&
                model.GeneralLibraries.Single(item => item.OwnerSeat == 0).PrivateAvatars.Count == 3 &&
                game.CreateSnapshot(1).Players[0].PrivateGeneralLibraries!.Single().GeneralIds is null,
            "Actual save/load restores the same two unpaid selections on the original frame and keeps private faces local.");
        host.Capture(output, "boundary-zuo-ci-mobile-private-exchange-draft-cold", 1120);
        Click(model, host, choice => Action(choice, "finish"));
        Play(game);
        ShiFifthBatchUiChecks.Refresh(model);
        var after = Library(game);
        var exchanged = game.Events.Select(item => item.Payload).OfType<ShanheS5AvatarsExchangedEvent>().Single();
        Program.Assert(exchanged.ProgramFrameId == frame.Id && exchanged.RemovedCount == 2 && exchanged.GainedCount == 2 &&
                after.Source == before.Source && after.Count == 3 && after.RevealedGeneralId == before.RevealedGeneralId &&
                after.DeclaredSkillId == before.DeclaredSkillId && after.GeneralIds!.Distinct().Count() == 3 &&
                !after.GeneralIds!.Intersect(hidden).Any() && model.GeneralLibraries.Single(item => item.OwnerSeat == 0).PrivateAvatars.Count == 3,
            "The real confirmation exchanges the two selected originals exactly once while preserving the displayed avatar and its native skill.");
        host.Capture(output, "boundary-zuo-ci-mobile-private-exchange-completed", 1120);
    }

    private static bool Action(PromptChoice choice, string action, string? id = null) =>
        choice.Parameters.GetValueOrDefault("program-action") == "shanhe-s5-avatar" &&
        choice.Parameters.GetValueOrDefault("action") == action &&
        (id is null || choice.Parameters.GetValueOrDefault("general-id") == id);

    private static void Click(MainViewModel model, NativeResponseStatusChecks.NativeHost host, Func<PromptChoice, bool> predicate)
    {
        ShiFifthBatchUiChecks.Refresh(model);
        Program.Render(host.Root, 1120, 740, null);
        var choice = model.SkillChoices.Single(predicate);
        var button = Program.Find<Button>(host.Root).Single(item => item.IsVisible && item.IsEnabled &&
            (ReferenceEquals(item.Command, model.SelectSkillChoiceCommand) || ReferenceEquals(item.Command, model.DockSkillChoiceCommand)) &&
            ReferenceEquals(item.CommandParameter, choice));
        Program.Assert(button.IsLoaded && button.ActualWidth > 0 && button.ActualHeight > 0,
            "The original avatar choice must be rendered and executable before clicking.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    }
}
