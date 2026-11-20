using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class AnniversaryLiuLiDehuaRoundChecks
{
    private const string Hero = "anniversary:liu-li", Dehua = "anniversary:dehua";
    private const string Peer = "fixture:dehua-round-peer", Deck = "fixture:dehua-round-deck";

    internal static void FirstRoundUsesNativeDamageNameBeforeActualTurnCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(43, 1, "dehua-round", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Spade, 7)], [], [new(Peer) { StartingHand = [] }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
        }, content);

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        void Reach(Func<PendingDecision, bool> ready)
        {
            for (var step = 0; step < 80 && game.State.Winner == Winner.None; step++)
            {
                if (Prompt() is { } prompt)
                {
                    Require(ready(prompt), $"Unexpected Dehua choice {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
                    return;
                }
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException($"Missing Dehua boundary at turn {game.State.TurnNumber}, phase {game.State.Phase}.");
        }

        Accept(game, new StartGameCommand());
        Reach(prompt => prompt.SkillPrompt?.SkillId == Dehua);
        var round = Facts<RoundStartedEvent>().Single();
        var prelude = game.ResolutionStack.OfType<BeiMiHuTurnPreludeFrame>().Single();
        var window = game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(frame => frame.Window == SkillProgramTriggerWindow.RoundStarting);
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Dehua);
        var started = Facts<AnniversaryLiuLiStartedEvent>().Single();
        Require(round is { RoundNumber: 1, ActorSeat: 0 } && prelude is { RoundAdvanced: true, Stage: 0, Proof.UpcomingTurnNumber: 1, Proof.UpcomingOwnerSeat: 0 } &&
            prelude.WindowId == window.Id && window.BeforeTurnPreludeProof == prelude.Proof &&
            window.Continuation == ProgramLifecycleContinuation.BeiMiHuPreludeRound &&
            window is { CandidateIndex: 0, Candidates: [{ OwnerSeat: 0, SkillId: Dehua, BindingId: "round-damage-name" }] } &&
            parent is { OwnerSeat: 0, TriggerId: "round-damage-name", AnniversaryLiuLi.Stage: AnniversaryLiuLiStage.UseChoice } &&
            parent.WindowContext is { BeforeTurnPreludeProof: { } proof } context && context.ParentFrameId == window.Id && proof == prelude.Proof &&
            started.FrameId == parent.Id && started.ParentId == window.Id && started.Round == round.RoundNumber && Facts<TurnStartedEvent>().Length == 0,
            "The first round publishes Liu Li's original Dehua choice on its native prelude before the actual turn starts.");

        var promptId = Prompt()!.PromptId;
        var frames = game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())).ToArray();
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Prompt()?.PromptId == promptId && frames.SequenceEqual(game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType()))),
            "Cold replay preserves the round prelude, its frozen candidate and the same published Dehua choice.");
        var fireSlash = "【" + CardCatalog.Get(CardKind.FireSlash).DisplayName + "】";
        var choice = Prompt()!.Choices.Single(item => item.Targets.SequenceEqual([1]) && item.Label.Contains(fireSlash, StringComparison.Ordinal));
        var peerHp = game.CreateSnapshot(0).Players[1].Hp;
        Accept(game, new AnswerPromptCommand(0, promptId, choice.Id, game.Revision));
        Reach(prompt => prompt.Kind == DecisionKind.PlayCard);

        var issued = Facts<AnniversaryLiuLiNativeIssuedEvent>().Single();
        var chosen = Facts<AnniversaryLiuLiNameChosenEvent>().Single();
        var finished = game.Events.Single(entry => entry.Payload is CardUseFinishedEvent fact && fact.ResolutionId == issued.UseId);
        var actualStart = game.Events.Single(entry => entry.Payload is TurnStartedEvent { TurnNumber: 1, ActorSeat: 0 });
        Require(issued.FrameId == parent.Id && issued.Kind == CardKind.FireSlash && chosen.FrameId == parent.Id && chosen.UseId == issued.UseId &&
            chosen.ActionId == issued.ActionId && chosen.Source == started.Source && chosen.Name == CardKind.Slash &&
            Facts<CardUseDeclaredEvent>().Single(fact => fact.ResolutionId == issued.UseId) is { SourceSeat: 0, CardKind: CardKind.FireSlash, CardId: 0 } &&
            finished.Payload is CardUseFinishedEvent { CardKind: CardKind.FireSlash, CardId: 0 } && finished.Sequence < actualStart.Sequence &&
            Facts<AnniversaryLiuLiStartedEvent>().SequenceEqual([started]) && game.CreateSnapshot(0).Players[1].Hp == peerHp - 1,
            "The same Liu Li frame uses and finishes one native Fire Slash before the actual turn and records the common Slash name.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-dehua-round", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "德化固定牌堆", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "德化真实轮首", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
