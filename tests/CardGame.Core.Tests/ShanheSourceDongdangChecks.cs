using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceDongdangChecks
{
    private const string Driver = "fixture:source-dongdang-opening", Deck = "fixture:source-dongdang-deck";

    public static void ActualOpeningEntryConsumesItsNativeInstanceAndStartsBossFirst()
    {
        foreach (var dongdang in new[] { false, true })
        {
            var stageId = dongdang ? StandardShanheSourceZhangYanPackage.ZhangYanHardId : StandardShanheSourceZhangYanPackage.ZhangYanMediumId;
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-dongdang-opening", "classic:guan-yu", 4,
                0, 0, 0, 0, [Driver], [], [], [new("classic:guan-yu", Hp: 1) { StartingHand = [] }], [], "fixture:source-dongdang")
                { BossTransition = new(0, stageId, 4, 4, 2) };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var definition = registry.GetSkill(ShanheSourceDongdangRules.SkillId);
            Need(definition is { Program: null, Tags: SkillTag.Locked, ExecutionForms: SkillExecutionForm.Trigger },
                "The source entry rule is a real registered native locked trigger, rather than an empty skill program.");
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry);
            Submit(game, new StartGameCommand());
            Reach(game, () => Facts<ShanheBossStageStartedEvent>(game).Length == 1);
            var stage = Facts<ShanheBossStageStartedEvent>(game).Single();
            var paid = Facts<NativeDrawInvocationRecordedEvent>(game).Single(fact => fact.OwnerSeat == 0 &&
                fact.GameStartingOrigin?.Source.SkillId == Driver && fact.GameStartingOrigin.InstructionIndex == 1);
            Need(stage is { BossSeat: 1, TurnNumber: 0, Hp: 4, MaxHp: 4 } && stage.GeneralId == stageId &&
                paid is { ActualCount: 1, TurnNumber: 0, GameStartingOrigin: { } } &&
                game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.From == CardLocation.DrawPile &&
                    move.Reason.Value == "skill-program." + Driver + ".Draw") >= 1,
                "A genuine paid opening draw is followed by native lethal opening damage and exactly one zero-turn stage replacement.");
            if (dongdang)
            {
                var source = Facts<ShanheSourceDongdangActivatedEvent>(game).Single();
                Need(source is { BossSeat: 1, InterruptedSeat: 0, TurnNumber: 0 } && source.GeneralId == stageId &&
                    source.PreviousGeneralId == "classic:guan-yu" && source.Source.SkillId == ShanheSourceDongdangRules.SkillId &&
                    source.Source.BindingId == ShanheSourceDongdangRules.BindingId && source.Source.OwnerSeat == 1 &&
                    !string.IsNullOrWhiteSpace(source.Source.SkillInstanceId) &&
                    Facts<ShanheBossResolutionCancelledEvent>(game).Any(fact => fact.FrameId == paid.GameStartingOrigin!.ProgramFrameId) &&
                    Facts<ProgramBindingResolvedEvent>(game).All(fact => fact.FrameId != paid.GameStartingOrigin!.ProgramFrameId || !fact.Completed),
                    "Only the actually enabled new-form Dongdang instance cancels the exact paid opening producer instead of letting its next draw execute.");
                Reach(game, () => game.State.CurrentSeat == 1 && game.State.Phase == TurnPhase.Play && game.ResolutionStack.Count == 0);
                Need(Facts<TurnStartedEvent>(game) is [{ ActorSeat: 1, TurnNumber: 1 }] &&
                    Facts<ShanheSourceDongdangActivatedEvent>(game).Length == 1 &&
                    game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.From == CardLocation.DrawPile &&
                        move.Reason.Value == "skill-program." + Driver + ".Draw") == 1 &&
                    game.CreateSnapshot(0).Players[0].HandCount == 1 && game.CreateSnapshot(1).Players[1].HandCount == 2,
                    "The boss really starts the first natural turn, the interrupted opening does not replay, and already paid cards stay with their owner while the second form receives its configured two entities once.");
            }
            else
            {
                Reach(game, () => game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                    game.ResolutionStack.Count == 0);
                Need(Facts<ShanheSourceDongdangActivatedEvent>(game).Length == 0 && Facts<ShanheBossResolutionCancelledEvent>(game).Length == 0 &&
                    Facts<TurnStartedEvent>(game) is [{ ActorSeat: 0, TurnNumber: 1 }] &&
                    Facts<ProgramBindingResolvedEvent>(game).Count(fact => fact.FrameId == paid.GameStartingOrigin!.ProgramFrameId && fact.Completed) == 1 &&
                    game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.From == CardLocation.DrawPile &&
                        move.Reason.Value == "skill-program." + Driver + ".Draw") == 2,
                    "A registered definition elsewhere cannot trigger Dongdang for a form that does not own it: the existing opening producer and player's first natural turn remain intact.");
            }
            Need(Facts<ShanheBossStageStartedEvent>(game).Length == 1 && Facts<ShanheBossTurnInterruptedEvent>(game).Length == 0 &&
                Facts<TurnEndedEvent>(game).All(fact => fact.TurnNumber > 0) && Facts<PlayerDiedEvent>(game).Length == 0 &&
                game.State.Winner == Winner.None,
                "Neither opening branch invents an ended actual turn zero or a first-form death.");
        }
    }

    private static void Reach(GameEngine game, Func<bool> stop)
    {
        for (var step = 0; step < 160; step++)
        {
            if (stop()) return;
            var prompt = game.CreateSnapshot(0).PendingDecision;
            if (prompt is { PlayerSeat: 0 } && prompt.Kind != DecisionKind.PlayCard)
            {
                var choice = prompt.Choices.SingleOrDefault(option => option.Targets.SequenceEqual([1]) && option.Cards.Count == 0);
                Need(choice is not null, "Unexpected opening Dongdang native boundary: " + JsonSerializer.Serialize(prompt));
                Submit(game, new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, game.Revision));
            }
            else Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Opening Dongdang did not reach its native boundary: " + JsonSerializer.Serialize(new
            { game.State.CurrentSeat, game.State.TurnNumber, game.State.Phase, Pending = game.CreateSnapshot(0).PendingDecision,
                Source = Facts<ShanheSourceDongdangActivatedEvent>(game), Stages = Facts<ShanheBossStageStartedEvent>(game) }));
    }
    private static void Submit(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Dongdang command."); }
    private static T[] Facts<T>(GameEngine game) => game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-dongdang", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"triggers":[{"id":"native-opening-lethal","window":"gameStarting","subject":"owner","optional":false,"effects":[
                    {"op":"draw","target":"owner","amount":1},
                    {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                    {"op":"damage","target":"selectedTarget","amount":20},
                    {"op":"draw","target":"owner","amount":1}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "真实开局致命伤害", description = "先摸牌，再选择目标造成致命伤害，随后继续摸牌。" } } }));
            builder.AddSkill(new(Driver, "真实开局致命伤害", "原生开局、已付摸牌与后续步骤。")
                { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddDeck(new(Deck, "固定真实闪牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
        }
    }
}
