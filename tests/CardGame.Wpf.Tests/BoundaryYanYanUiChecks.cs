using CardGame.Wpf.Persistence;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;

internal static class BoundaryYanYanUiChecks
{
    public static void StructuredPolarityOccurrenceAndPersistentQuotaRailCold(string output)
    {
        const string skill = "boundary:juzhan-mobile";
        var scene = BoundaryYanYanMobileChecks.CreateScene(singleHuman: true);
        using var model = ShiFifthBatchUiChecks.Model(scene.Game, scene.Registry);
        var game = Program.Engine(model);
        var chip = model.HumanSkillCards.Single(item => item.ContentId == skill);
        Program.Assert(chip.ProgressText.Contains("阴", StringComparison.Ordinal) && chip.ProgressText.Contains("0次", StringComparison.Ordinal) &&
                chip.ProgressText.Contains("杀+0", StringComparison.Ordinal) && !chip.StateText.Contains("觉醒", StringComparison.Ordinal),
            "The actual Shu edition starts with Yin, its current state occurrence count and persistent Slash quota instead of an awakening counter.");
        var action = game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.Slash && item.TargetSeats.Count == 1);
        var prompt = game.CreateSnapshot(0).PendingDecision!;
        ShiFifthBatchUiChecks.Accept(game, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats,
            game.Revision, prompt.PromptId, action.PlayedCardKind, action.TargetCardId)
            { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        ShiFifthBatchUiChecks.Reach(game, () => game.CreateSnapshot(0).PendingDecision?.SkillPrompt?.SkillId == skill);
        ShiFifthBatchUiChecks.Refresh(model);
        LegacySaveFixtures.Save(model); LegacySaveFixtures.Load(model); game = Program.Engine(model);
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        Program.Render(host.Root, 1120, 740, null);
        chip = model.HumanSkillCards.Single(item => item.ContentId == skill);
        var runtime = game.CreateSnapshot(0).Players[0].SkillRuntimeStates!.Single(item => item.SkillId == skill);
        Program.Assert(!model.HasSaveError && runtime.Polarity == SkillPolarity.Yin &&
                runtime.PublicRuleStates!.Single(item => item.StateId == "juzhan-state-count").Value == 1 &&
                chip.ProgressText.Contains("1次", StringComparison.Ordinal) && chip.ProgressText.Contains("杀+0", StringComparison.Ordinal),
            "A real accepted Slash updates the visible state counter, and the actual save/load UI retains that occurrence without granting Chengshi.");
        var decline = model.DockSkillChoices.Single(item => item.Parameters.GetValueOrDefault("program-action") == "skip");
        var button = Program.Find<Button>(host.Root).Single(item => item.IsVisible && item.IsEnabled &&
            ReferenceEquals(item.Command, model.DockSkillChoiceCommand) && ReferenceEquals(item.CommandParameter, decline));
        host.Capture(output, "boundary-yan-yan-mobile-state-counter-cold", 1120);
        var before = game.AcceptedCommands.Count;
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
        ShiFifthBatchUiChecks.Reach(game, () => game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard });
        ShiFifthBatchUiChecks.Refresh(model);
        Program.Assert(game.AcceptedCommands.Skip(before).OfType<AnswerPromptCommand>().Count(item => item.Choice == decline.Id) == 1 &&
                model.HumanSkillCards.Single(item => item.ContentId == skill).ProgressText.Contains("阴 1次", StringComparison.Ordinal),
            "The real decline button leaves the original polarity and current-state count in place for a later qualifying Slash.");
    }
}
