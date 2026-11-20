using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;
using static BoundaryLiDianChecks;

internal static class AnniversaryQingrenDrawChecks
{
    private const string Hero = "anniversary:zhao-tong-zhao-guang", Yizan = "anniversary:yizan", Qingren = "anniversary:qingren";
    private const string Peer = "fixture:qingren-quiet-peer", Watch = "fixture:qingren-draw-watch", Deck = "fixture:qingren-draw-deck";
    private const string DrawReason = "skill-program.anniversary:qingren.draw";

    internal static void OneYizanUseReturnsOriginalQingrenDrawCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(47, 1, "qingren-draw", Hero, 4, 0, 0, 0, 0, [Watch],
            [new(CardKind.Dodge, Suit.Club, 3), new(CardKind.Dismantlement, Suit.Spade, 4)], [],
            [new(Peer) { StartingHand = [] }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, content);

        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        bool AtPlay() => Prompt()?.Kind == DecisionKind.PlayCard;
        void ContinueDraw(PendingDecision prompt) => Accept(game, new AnswerPromptCommand(0, prompt.PromptId,
            prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision));
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 80 && !ready() && game.State.Winner == Winner.None; step++)
            {
                if (Prompt() is not { } prompt)
                    Accept(game, new AdvanceOneStepCommand(game.Revision));
                else if (prompt.SkillPrompt?.SkillId == Watch)
                    ContinueDraw(prompt);
                else
                    throw new InvalidOperationException($"Unexpected Qingren choice {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
            }
            Require(ready(), $"Missing Qingren boundary at turn {game.State.TurnNumber}, phase {game.State.Phase}.");
        }

        Accept(game, new StartGameCommand());
        Reach(AtPlay);
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var materials = new[] { hand.Single(card => card.Kind == CardKind.Dodge).Id, hand.Single(card => card.Kind == CardKind.Dismantlement).Id };
        var option = game.GetHumanLegalActions().Single(action => action.ProgramSkillId == Yizan && action.PlayedCardKind == CardKind.Alcohol &&
            action.SelectableCardIds!.Order().SequenceEqual(materials.Order()));
        Accept(game, new UseProgramSkillCommand(0, Yizan, option.ProgramActivationId!, materials, [], game.Revision, Prompt()!.PromptId));
        Reach(AtPlay);
        var yizan = Facts<AnniversaryYizanInvokedEvent>().Single();
        var turn = Facts<TurnStartedEvent>().Single();
        Require(turn.ActorSeat == 0 && yizan.ActualTurn == turn.TurnNumber && yizan.MaterialCount == 2 &&
            yizan.Source is { SkillId: Yizan, OwnerSeat: 0 } && yizan.NativeUseFrameId is { } useId &&
            Facts<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == useId && fact.CardKind == CardKind.Alcohol) &&
            materials.All(id => game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location == CardLocation.DiscardPile),
            "One real Yizan Alcohol consumes both original entities on the owner's first actual turn.");

        Accept(game, new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Watch);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Qingren);
        var attempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        var issued = Facts<AnniversaryQingrenIssuedEvent>().Single();
        Require(root.AnniversaryQingren is { Count: 1, Stage: AnniversaryQingrenStage.Drawing } &&
            issued.FrameId == root.Id && issued.ActualTurn == turn.TurnNumber && issued.Count == 1 && issued.CutoffActionId >= yizan.ActionId &&
            game.ResolutionStack.OfType<TurnEndingBoundaryFrame>().Any(frame => frame.Id == issued.ParentId) &&
            attempt.Context.ParentFrameId == root.Id && attempt.Context is { RecipientSeat: 0, RequestedCount: 1 } && attempt.Context.Reason.Value == DrawReason &&
            game.GetRuntimeModule<CharacterZhaoTongZhaoGuangAnniversaryRuntime>().AnniversaryQingrenFirstChild(root, attempt) &&
            !game.CardMovements.Any(move => move.Reason.Value == DrawReason),
            "The actual ending owns the one-card native attempt before any physical draw.");

        var frames = game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())).ToArray();
        var promptId = Prompt()!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Prompt()?.PromptId == promptId && frames.SequenceEqual(game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType()))),
            "Cold replay preserves the original Qingren parent, attempt and private choice.");
        ContinueDraw(Prompt()!);
        Reach(() => Facts<TurnEndedEvent>().Any(fact => fact.TurnNumber == turn.TurnNumber && fact.ActorSeat == 0));
        var returned = Facts<AnniversaryQingrenDrawReturnedEvent>().Single();
        var move = game.CardMovements.Single(item => item.Reason.Value == DrawReason);
        Require(returned is { Requested: 1, Actual: 1 } && returned.FrameId == root.Id &&
            Facts<NativeDrawAttemptDecidedEvent>().Count(fact => fact.AttemptFrameId == attempt.Id && fact.ParentFrameId == root.Id && !fact.Replaced) == 1 &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Sequence > returned.Before && move.Sequence <= returned.After,
            "One card returns to the original Qingren parent and recipient before the actual turn finishes.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-qingren-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var program = SkillProgramCatalog.LoadCurrent($$"""
                {"skills":[{"id":"{{Watch}}","revision":1,"triggers":[{"id":"native-draw","window":"nativeDrawStarting",
                "subject":"owner","optional":false,"isSkillInvocation":false,
                "effects":[{"op":"chooseOption","target":"owner","resultBind":"draw-child","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Watch] = new { name = "青刃摸牌前继续", description = "停在原生摸牌子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                    }
                }));
            builder.AddSkill(new(Watch, program.Presentations[Watch].Name, program.Presentations[Watch].Description)
                { Program = program.Programs[Watch], ProgramPresentation = program.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "青刃固定牌堆", 0, 2, [new("standard:peach", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "青刃实际摸牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
