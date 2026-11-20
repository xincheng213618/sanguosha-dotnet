using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryLiuYongFengxiangChecks
{
    private const string Hero = "anniversary:liu-yong", Zhuning = "anniversary:zhuning", Fengxiang = "anniversary:fengxiang";
    private const string Driver = "fixture:fengxiang-native-damage", Watch = "fixture:fengxiang-draw-watch";
    private const string Peer = "fixture:fengxiang-peer", Deck = "fixture:fengxiang-deck";
    private const string DrawReason = "skill.anniversary.fengxiang.draw";

    internal static void XiLeaderChangeAndDamageDrawUseOriginalReceiptCold()
    {
        const int owner = 0, leader = 1;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(43, 1, "fengxiang-draw-and-leader", Hero, 3, 0, 0, 0, 0, [Driver, Watch],
            [new(CardKind.Slash, Suit.Club, 7), new(CardKind.Dodge, Suit.Heart, 2)], [],
            [new(Peer, Hp: 18) { StartingHand = [] }], [], new string('d', 64)) { ExtraSkillSlots = 2 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(new StartGameCommand());
        Reach(IsPlay);
        var xi = Player(owner).Hand.Single(card => card.Kind == CardKind.Dodge).Id;
        var slash = Player(owner).Hand.Single(card => card.Kind == CardKind.Slash).Id;
        Require(Player(leader) is { Hp: 18, MaxHp: 20, HandCount: 0 },
            "The actual neighboring enemy starts wounded and holds no alternate Dodge.");
        Submit(new UseProgramSkillCommand(owner, Zhuning, "gift-and-use", [], [leader], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Zhuning && Prompt()!.Choices.Any(choice => choice.Cards.SequenceEqual([xi])));
        Answer(choice => choice.Cards.SequenceEqual([xi]));
        Answer(choice => choice.Parameters.GetValueOrDefault("option") == "give");
        var firstDraw = FinishDraw("transition", cold: true);
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Zhuning && Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "finish"));
        var gift = Facts<AnniversaryLiuYongGiftPaidEvent>().Single();
        Require(Facts<AnniversaryLiuYongLeaderChangedEvent>() is [{ Previous: null, Current: leader } change] &&
            gift is { Recipient: leader, Count: 1 } && firstDraw.ParentId == change.BatchId &&
            Player(leader).Hand.Single() is { Id: var marked, SkillMarkLabel: "隙" } && marked == xi &&
            game.CardMovements.Count(move => move.CardId == xi && move.From == CardLocation.Hand(owner) &&
                move.To == CardLocation.Hand(leader) && move.Reason.Value == "skill.anniversary.zhuning.gift") == 1,
            "The original physical gift makes its receiver the unique Xi leader and its own Fengxiang Draw returns to the gift flow.");
        Answer(choice => choice.Parameters.GetValueOrDefault("option") == "finish");
        Reach(IsPlay);

        DamageOwner();
        Reach(IsPlay);
        var heal = Facts<AnniversaryLiuYongStartedEvent>().Single(fact => fact.Mode == "damage");
        Require(Player(owner).Hp == 2 && Player(leader).Hp == 19 &&
            Facts<RecoveryAppliedEvent>() is [{ SourceSeat: owner, TargetSeat: leader, Amount: 1 }] &&
            Facts<AnniversaryLiuYongDrawReturnedEvent>().All(fact => fact.FrameId != heal.FrameId),
            "The original damage with a unique Xi leader performs real recovery for the wounded leader.");

        var action = game.GetLegalActions(owner).Single(item => item.Kind == LegalActionKind.Slash && item.CardId == slash &&
            item.TargetSeats.SequenceEqual([leader]) && item.ConversionSource is null);
        Submit(new PlayCardCommand(owner, slash, action.TargetSeats, game.Revision, Prompt()!.PromptId, action.PlayedCardKind));
        FinishDraw("transition");
        Reach(IsPlay);
        Require(Facts<AnniversaryLiuYongLeaderChangedEvent>() is [{ Previous: null, Current: leader }, { Previous: leader, Current: null }] &&
            game.CardMovements.Count(move => move.CardId == xi && move.From == CardLocation.Hand(leader) && move.To == CardLocation.Processing) == 1 &&
            Facts<CardUseFinishedEvent>().Count(fact => fact.CardId == slash) == 1,
            "The enemy really plays the marked Dodge, ends its unique lead and returns through the original Slash.");

        DamageOwner();
        var fallback = FinishDraw("damage");
        Reach(IsPlay);
        Require(Player(owner).Hp == 1 && Player(leader).Hp == 19 && Facts<RecoveryAppliedEvent>().Length == 1 &&
            Facts<DamageAppliedEvent>().Count(fact => fact.TargetSeat == owner && fact.Amount == 1) == 2 &&
            Facts<AnniversaryLiuYongDrawReturnedEvent>().Length == 3 &&
            Facts<AnniversaryLiuYongDrawReturnedEvent>().Count(fact => fact.FrameId == fallback.FrameId) == 1,
            "With the unique lead gone, the next actual damage pays one original native Draw and no further recovery.");

        AnniversaryLiuYongStartedEvent FinishDraw(string mode, bool cold = false)
        {
            Reach(() => HasWatchOption("request"));
            var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Fengxiang);
            var attempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
            var started = Facts<AnniversaryLiuYongStartedEvent>().Single(fact => fact.FrameId == parent.Id);
            Require(started.Mode == mode && parent.AnniversaryLiuYong is { Stage: AnniversaryLiuYongStage.Drawing, Recipient: owner } &&
                attempt.Context is { RecipientSeat: owner, RequestedCount: 1 } && attempt.Context.ParentFrameId == parent.Id &&
                attempt.Context.Reason.Value == DrawReason && attempt.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation,
                "The original Fengxiang receipt owns one native card request for Liu Yong in the expected rule branch.");
            Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "request");
            Reach(() => HasWatchOption("gain"));

            var paid = ReadDraw(parent.Id);
            var physicalId = paid.ActualDrawIds.Single();
            var move = game.CardMovements.Single(record => record.CardId == physicalId && record.Reason.Value == DrawReason);
            var returned = Facts<AnniversaryLiuYongDrawReturnedEvent>().Single(fact => fact.FrameId == parent.Id);
            Require(paid.AttemptFrameId == attempt.Id && paid.Replacement is null &&
                move.Sequence > paid.SequenceBefore && move.Sequence <= paid.SequenceAfter &&
                move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner) &&
                returned == new AnniversaryLiuYongDrawReturnedEvent(parent.Id, attempt.Id, paid.SequenceBefore, paid.SequenceAfter, 1) &&
                game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(frame => frame.Batch.ParentFrameId == parent.Id && frame.Batch.Movements.Contains(move)),
                "The visible gain child belongs to the exact original physical draw and shared movement range.");
            if (cold)
            {
                var gainPrompt = Prompt()!.PromptId;
                game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
                var restored = ReadDraw(parent.Id);
                Require(Prompt()?.PromptId == gainPrompt && restored.AttemptFrameId == attempt.Id && restored.ActualDrawIds.SequenceEqual([physicalId]) &&
                    restored.SequenceBefore == paid.SequenceBefore && restored.SequenceAfter == paid.SequenceAfter,
                    "The paid transition Draw restores its same gain decision, original entity and shared interval cold.");
            }
            Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "gain");
            return started;
        }

        NativeDrawOutcome ReadDraw(long id) => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == id)
            .NativeOperationDrawReturns.Single(receipt => receipt.Operation == SkillProgramEffectOp.ResolveAnniversaryLiuYongFengxiang).Outcome;
        PendingDecision? Prompt() => game.CreateSnapshot(owner).PendingDecision;
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        T[] Facts<T>() => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        bool IsPlay() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: owner };
        bool HasWatchOption(string option) => Prompt()?.SkillPrompt?.SkillId == Watch &&
            Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == option);
        void DamageOwner() => Submit(new UseProgramSkillCommand(owner, Driver, "damage-owner", [], [], game.Revision, Prompt()!.PromptId));
        void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.Single(predicate);
            Submit(new AnswerPromptCommand(owner, prompt.PromptId, choice.Id, game.Revision));
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                Require(Prompt() is null && game.State.Winner == Winner.None, "The fixture reached a different public command boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Require(ready(), "The fixture did not reach the original Fengxiang or Zhuning boundary.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Require(result.Accepted, result.Error?.Message ?? "The native Fengxiang fixture command was rejected.");
        }
        void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}/{game.State.Phase}, " +
                    $"owner prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-fengxiang-draw", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"damage-owner","minCards":0,"maxCards":0,
                    "minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"owner","amount":1}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"observe-request","window":"nativeDrawStarting","subject":"owner","condition":{"kind":"phaseIsPlay"},
                      "optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"request"}]}]},
                    {"id":"observe-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch",
                      "movementReasons":["{{DrawReason}}"],"optional":false,"isSkillInvocation":false,
                      "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"gain"}]}]}
                  ]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{
                  "{{Driver}}":{"name":"原生自伤","description":"执行一条真实伤害。"},
                  "{{Watch}}":{"name":"封乡真实摸牌","description":"确认原生请求和原获得实体。","optionLabels":{"request":"确认请求","gain":"确认获得"} } } }
                """);
            foreach (var (id, program) in catalog.Programs)
            {
                var shown = catalog.Presentations[id];
                builder.AddSkill(new(id, shown.Name, shown.Description) { Program = program, ProgramPresentation = shown });
            }
            builder.AddGeneral(new(Peer, "固定有伤对手", "supporter", "standard:none", "wei", 20));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 0, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "封乡实体隙与伤害分支", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
