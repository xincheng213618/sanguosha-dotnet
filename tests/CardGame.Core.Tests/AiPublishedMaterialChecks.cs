using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AiPublishedMaterialChecks
{
    private const string Hero = "fixture:ai-public-material-actor", Holder = "fixture:ai-public-material-holder",
        Peer = "fixture:ai-public-material-peer", Quiet = "fixture:ai-public-material-quiet", Deck = "fixture:ai-public-material-deck",
        CardlessWatch = "fixture:ai-cardless-use-watch";

    public static void RoundOnceCardlessSlashScoresWithoutPhysicalMaterialAndReturnsCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new Fixture());
        var setup = new ShanheBattleSetup(41, 1, "ai-round-cardless", Hero, 8, 0, 0, 0, 0, ["ol:weijing", CardlessWatch],
            [new(CardKind.Peach, Suit.Heart, 4)], [],
            [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }],
            [new(Peer, Hp: 8) { StartingHand = [] }], new string('c', 64));
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            AiPolicyVersion = AiDecisionPolicy.CurrentVersion, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
        Accept(game, new StartGameCommand()); Play(game);
        var before = Boundary(game);
        var inventory = game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order().ToArray();
        var hand = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).ToArray();
        var actions = game.GetHumanLegalActions();
        Need(actions.Any(action => action.RoundOnceCardlessBasicUse is not null) &&
            actions.Where(action => action.Kind == LegalActionKind.Slash).All(action => action.CardId == 0),
            "The real acquired Weijing publishes a zero-material Slash despite the actor owning only one Peach, with no physical Slash to borrow or fabricate.");
        LegalAction? chosen = null;
        foreach (var version in new[] { AiDecisionPolicy.RebelCoordinationVersion, AiDecisionPolicy.WushuangStrategyVersion })
        {
            var decision = new SimpleAiBrain(0, 113, version).ChoosePlay(game.CreateSnapshot(0), actions, 1,
                (action, target) => game.PredictPublicActionDamage(0, action, target));
            Need(decision.Action is { Kind: LegalActionKind.Slash, CardId: 0, RoundOnceCardlessBasicUse: not null } &&
                decision.Action.ConversionSource?.SkillId == "ol:weijing" && decision.Action.TargetSeats.SequenceEqual([3]) &&
                decision.Thought.Candidates.All(candidate => double.IsFinite(candidate.Score)) && Boundary(game) == before,
                "Both recorded policies select the genuine public zero-material enemy attack without touching entities, quota, engine RNG or history.");
            chosen = decision.Action;
        }
        game = Cold(game, registry);
        var actionToPlay = game.GetHumanLegalActions().Single(action => action.ConversionSource == chosen!.ConversionSource && action.TargetSeats.SequenceEqual(chosen.TargetSeats));
        Accept(game, new PlayCardCommand(0, 0, actionToPlay.TargetSeats, game.Revision, game.CreateSnapshot(0).PendingDecision!.PromptId,
            actionToPlay.PlayedCardKind) { ConversionSource = actionToPlay.ConversionSource });
        for (var step = 0; step < 80 && game.CreateSnapshot(0).PendingDecision?.SkillPrompt?.SkillId != CardlessWatch; step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        Need(game.CreateSnapshot(0).PendingDecision?.SkillPrompt?.SkillId == CardlessWatch,
            "The real cardless attack reaches its committed-use child after native round-quota payment.");
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.RoundOnceCardlessBasicUse is not null);
        Need(use is { CardId: 0, PhysicalCardIds.Count: 0, Action.PhysicalCards.Count: 0 } &&
            use.Action.ConversionChain.Single() == actionToPlay.ConversionSource &&
            game.Events.Select(e => e.Payload).OfType<RoundOnceCardlessBasicQuotaPaidEvent>().Count() == 1,
            "The actual native Use carries its issued source and once-only round quota, with zero physical payment entities.");
        game = Cold(game, registry);
        var pending = game.CreateSnapshot(0).PendingDecision!;
        Accept(game, new AnswerPromptCommand(0, pending.PromptId,
            pending.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision));
        Play(game); game = Cold(game, registry);
        Need(game.CreateSnapshot(0).Players[3].Hp == 7 && game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).SequenceEqual(hand) &&
            game.CreateCardZoneDiagnostics().Select(card => card.CardId).Order().SequenceEqual(inventory) &&
            game.Events.Select(e => e.Payload).OfType<RoundOnceCardlessBasicQuotaPaidEvent>().Count() == 1 &&
            game.Events.Select(e => e.Payload).OfType<CardUseFinishedEvent>().Count(done => done.ResolutionId == use.Id) == 1 &&
            game.GetHumanLegalActions().All(action => action.RoundOnceCardlessBasicUse is null),
            "Cold native return deals the real damage once, consumes the original shared round quota once and retains the full physical inventory and the actor's Peach.");
    }

    public static void ForeignXiansiMaterialsUsePublishedOwnerPileAndPayNativeSlashCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new Fixture());
        var cards = Enumerable.Range(0, 4).Select(index => new ShanheStartingCard(CardKind.Peach, Suit.Heart, index + 2)).ToArray();
        var setup = new ShanheBattleSetup(37, 1, "ai-public-material", Hero, 8, 0, 0, 0, 0, [], cards, [],
            [new(Peer, Hp: 8) { StartingHand = cards }, new(Holder, Hp: 8) { StartingHand = [] }],
            [new(Peer, Hp: 8) { StartingHand = cards }], new string('b', 64));
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            AiPolicyVersion = AiDecisionPolicy.CurrentVersion, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
        Accept(game, new StartGameCommand()); Play(game);
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, game.CreateSnapshot(0).PendingDecision!.PromptId));
        Play(game);
        var view = game.CreateSnapshot(0);
        var action = game.GetHumanLegalActions().Single(a => a.ProgramSkillId == "classic:xiansi");
        var owner = action.ProgramSkillOwnerSeat!.Value;
        var publicPile = view.Players.Single(p => p.Seat == owner).AuthorityCards!;
        Need(owner != 0 && publicPile.Count == 2 && view.Players.Single(p => p.Seat == owner).Hand.Count == 0 &&
            action.SelectableCardIds.Order().SequenceEqual(publicPile.Select(card => card.Id).Order()),
            "The genuine other owner's Xiansi collects two physical entities and publishes exactly their public faces as this action's payment.");
        var brain = new SimpleAiBrain(0, 113, AiDecisionPolicy.CurrentVersion);
        var state = Boundary(game);
        var selected = brain.ChooseActiveSkillCards(view, action);
        brain.ChoosePlay(view, game.GetHumanLegalActions(), 1);
        Need(selected.Count == 2 && selected.Distinct().Count() == 2 && selected.All(action.SelectableCardIds.Contains) && Boundary(game) == state,
            "Scoring and preparing the published foreign payment use the supplied seat view without throwing, consuming cards or inventing hidden material.");
        game = Cold(game, registry);
        action = game.GetHumanLegalActions().Single(a => a.ProgramSkillId == "classic:xiansi");
        Need(new SimpleAiBrain(0, 113, AiDecisionPolicy.CurrentVersion).ChooseActiveSkillCards(game.CreateSnapshot(0), action).SequenceEqual(selected),
            "Cold reconstruction preserves the exact deterministic foreign-material selection.");
        var ownHand = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).ToArray();
        var wrong = game.Submit(new UseProgramSkillCommand(0, action.ProgramSkillId!, action.ProgramActivationId!, [ownHand[0], selected[0]],
            [owner], game.Revision, game.CreateSnapshot(0).PendingDecision!.PromptId) { SkillOwnerSeat = owner });
        Need(!wrong.Accepted && Boundary(game) == state, "A real own-hand substitute still fails the original published foreign-pile payment guard without mutation.");
        var hp = game.CreateSnapshot(0).Players[owner].Hp;
        Accept(game, new UseProgramSkillCommand(0, action.ProgramSkillId!, action.ProgramActivationId!, selected, [owner],
            game.Revision, game.CreateSnapshot(0).PendingDecision!.PromptId) { SkillOwnerSeat = owner });
        game = Cold(game, registry); Play(game);
        var use = game.Events.Select(e => e.Payload).OfType<CardUseDeclaredEvent>().Single(e => e.SourceSeat == 0 && e.CardKind == CardKind.Slash);
        Need(game.CreateSnapshot(0).Players[owner].Hp == hp - 1 &&
            game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).SequenceEqual(ownHand) &&
            selected.All(id => game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Authority(owner) &&
                move.To == CardLocation.DiscardPile && move.Reason.Value == "program.public-pile.slash-payment") == 1) &&
            game.Events.Select(e => e.Payload).OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.ResolutionId) == 1,
            "The accepted native conversion pays each exact external authority entity once, retains the actor's hand, deals its real Slash and completes once after cold recovery.");
        Cold(game, registry);
    }

    private static string Boundary(GameEngine game) => JsonSerializer.Serialize(new {
        Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray(),
        Frames = game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())).ToArray(),
        Events = game.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
        game.CardMovements, Commands = CommandJson.Serialize(game.AcceptedCommands) });
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var before = Boundary(game);
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(before == Boundary(restored), "Cold recovery retains the original public source, paid entities, native frame and command journal.");
        return restored;
    }
    private static void Play(GameEngine game)
    {
        for (var step = 0; step < 160; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded real public-material fixture missed its actor's play boundary.");
    }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected public-material command."); }
    private static void Need(bool valid, string why) { if (!valid) throw new InvalidOperationException(why); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:ai-public-material", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] {
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "no-play", window = "afterNormalDraw", subject = "owner", optional = false,
                    effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } },
                new { id = CardlessWatch, revision = 1, triggers = new[] { new { id = "paid-native-use", window = "cardUseCommitted", ownerRelation = "actor",
                    cardKinds = new[] { "slash" }, optional = false, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "paid",
                        options = new[] { new { id = "continue" } } } } } } } } });
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Quiet] = new { name = "安静旁座", description = "真实跳过出牌以保留公开支付候选。" },
                    [CardlessWatch] = new { name = "零材料原生使用子窗", description = "真实轮额度付款后暂停原生杀，验证冷恢复及父链。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, "Native fixture") { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Hero, "支付行动者", "", "standard:none", "shu", 8));
            builder.AddGeneral(new(Holder, "公开逆的持有者", "", "classic:xiansi", "wei", 8, AdditionalSkillIds: [Quiet]));
            builder.AddGeneral(new(Peer, "安静旁座", "", Quiet, "shu", 8));
            builder.AddDeck(new(Deck, "原生实体库存", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(index => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "公开外区材料AI", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Holder, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
