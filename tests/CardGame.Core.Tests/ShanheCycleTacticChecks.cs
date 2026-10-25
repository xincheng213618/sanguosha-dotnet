using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheCycleTacticChecks
{
    private const string Owner = "fixture:cycle-tactic-owner", Enemy = "fixture:cycle-tactic-enemy";
    private const string Driver = "fixture:cycle-tactic-driver", Quiet = "fixture:cycle-tactic-quiet";
    private const string Deck = "fixture:cycle-tactic-deck";
    private static readonly string[] Tactics =
    [
        StandardShanheCycleTacticsPackage.ShoudaoQinlaiSkillId,
        StandardShanheCycleTacticsPackage.ShoudaoQinlai2SkillId,
        StandardShanheCycleTacticsPackage.ShoudaoQinlai3SkillId
    ];

    public static void ActualFifthSixthSeventhUsesDrawAndResetBeyondPlayPhase()
    {
        var s = Scene.Start();
        var response = s.V.Hand.Single(card => card.Kind == CardKind.Slash).Id;
        s.Accept(new UseProgramSkillCommand(0, Driver, "foreign-duel", [], [1], s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });
        s.Answer(s.P!.Choices.Single(choice => choice.Cards.SequenceEqual([response])));
        s.Play();
        Need(s.Events<CardActionAcceptedEvent>().Any(e => e.Action is
                { ActorSeat: 0, Type: CardActionType.Response, EffectiveKind: CardKind.Slash }) &&
            s.Events<CurrentTurnCardUseKindsRecordedEvent>().All(e => e.ActorSeat != 0) &&
            Tactics.All(skill => s.ProgramDraws(skill) == 0),
            "The opponent's real Duel and the owner's paid ordinary Slash response consume no owner use slot and grant no cycle reward.");

        for (var ordinal = 1; ordinal <= 4; ordinal++)
        {
            s.Equip();
            Need(Tactics.All(skill => s.ProgramDraws(skill) == 0), "The first four actual owner uses grant no early cycle draw.");
        }
        var firstTurn = s.G.State.TurnNumber;
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Driver && s.P.Choices.Any(choice =>
            choice.Parameters.GetValueOrDefault("program-action") == "select-target"));
        Need(s.G.State.Phase != TurnPhase.Play, "The fifth real equipment use must be prepared in the actual turn-ending window, outside the play phase.");
        s.Answer(s.P!.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-target" &&
            choice.Targets.SequenceEqual([0])));
        s.Until(() => s.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        var endingChoice = s.P!.Choices.First(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards");
        var endingCard = endingChoice.Cards.Single();
        s.Answer(endingChoice);
        s.Until(() => s.G.State.TurnNumber > firstTurn && s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(s.ProgramDraws(Tactics[1]) == 1 && s.ProgramDraws(Tactics[0]) == 0 && s.ProgramDraws(Tactics[2]) == 0 &&
            s.Events<CurrentTurnCardUseKindsRecordedEvent>().Where(e => e.TurnNumber == firstTurn && e.ActorSeat == 0)
                .Select(e => e.ActualTurnActorUseOrdinal).SequenceEqual(Enumerable.Range(1, 5).Select(value => (int?)value)) &&
            s.G.CardMovements.Count(move => move.CardId == endingCard && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(move => move.CardId == endingCard && move.From == CardLocation.Processing &&
                move.To == CardLocation.Equipment(0)) == 1,
            "The same fifth-use reward draws one real card after the paid turn-ending equipment resolves, then the next actual turn begins.");

        var nextTurn = s.G.State.TurnNumber;
        for (var ordinal = 1; ordinal <= 8; ordinal++)
        {
            var hand = s.V.HandCount;
            s.Equip();
            var drawn = ordinal is 5 or 7 ? 1 : ordinal == 6 ? 2 : 0;
            Need(s.V.HandCount == hand - 1 + drawn &&
                s.ProgramDraws(Tactics[0]) == (ordinal >= 7 ? 1 : 0) &&
                s.ProgramDraws(Tactics[1]) == 1 + (ordinal >= 5 ? 1 : 0) &&
                s.ProgramDraws(Tactics[2]) == (ordinal >= 6 ? 2 : 0),
                $"The next actual turn's completed use {ordinal} must draw only the source-defined 5th=1, 6th=2 and 7th=1 cards, with no reward on the 8th use.");
        }
        Need(s.Events<CurrentTurnCardUseKindsRecordedEvent>().Where(e => e.TurnNumber == nextTurn && e.ActorSeat == 0)
                .Select(e => e.ActualTurnActorUseOrdinal).SequenceEqual(Enumerable.Range(1, 8).Select(value => (int?)value)) &&
            s.Events<ProgramCardTriggerResolvedEvent>().Count(e => e.SkillId == Tactics[0] && e.Activated) == 1 &&
            s.Events<ProgramCardTriggerResolvedEvent>().Count(e => e.SkillId == Tactics[1] && e.Activated) == 2 &&
            s.Events<ProgramCardTriggerResolvedEvent>().Count(e => e.SkillId == Tactics[2] && e.Activated) == 1,
            "Each tier activates only on its owner's exact completed-use ordinal, and a real turn boundary resets all three without a new counter.");
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public IEnumerable<T> Events<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public int ProgramDraws(string skillId) => G.CardMovements.Count(move => move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program." + skillId + ".Draw");
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual cycle-tactic command."); }
        public void Answer(PromptChoice choice) => Accept(new AnswerPromptCommand(P!.PlayerSeat, P.PromptId, choice.Id, G.Revision));
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Equip()
        {
            var id = V.Hand.First(card => card.Kind == CardKind.Crossbow).Id;
            var action = G.GetHumanLegalActions().Single(action => action.CardId == id);
            Accept(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (done()) return;
                Need(G.State.Status != EngineStatus.Completed, "The small cycle-tactic battle ended before its expected real phase.");
                if (P is { PlayerSeat: 0 } prompt)
                {
                    var finish = prompt.Choices.SingleOrDefault(choice =>
                        choice.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards");
                    Need(finish is not null, "Unexpected cycle-tactic prompt: " + JsonSerializer.Serialize(prompt));
                    Answer(finish!);
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded actual cycle-tactic boundary: " + JsonSerializer.Serialize(P));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheCycleTacticsPackage(), new Fixture());
            var cards = Enumerable.Range(0, 9).Select(_ => new ShanheStartingCard(CardKind.Crossbow))
                .Append(new(CardKind.Slash)).ToArray();
            var setup = new ShanheBattleSetup(83, 1, "fixture:cycle-tactics", Owner, 12, 0, 0, 0, 0, [], cards, [],
                [new(Enemy) { StartingHand = [] }], [], new string('c', 64)) { TacticSkillIds = Tactics };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = true,
                AdvanceAfterHumanCommands = false, MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-cycle-tactics", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"foreign-duel","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"usesPerPhase":2,
                     "effects":[{"op":"useSelectedActorDuel","target":"owner"}]}],
                   "triggers":[{"id":"fifth-outside-play","window":"turnEnding","subject":"owner","usageScope":"game","usageLimit":1,"optional":false,
                     "effects":[{"op":"selectTarget","target":"owner","targetKind":"anyLiving"},
                       {"op":"selectOwnedCards","target":"owner","minimumCards":1,"maximumCards":1,"zones":["hand"],"cardKinds":["crossbow"],"resultBind":"ending-material"},
                       {"op":"revealBoundCards","target":"owner","sourceBind":"ending-material"},
                       {"op":"useBoundCardByTarget","target":"selectedTarget","sourceBind":"ending-material"}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "真实用牌驱动", description = "普通响应与结束阶段实体装备。" })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "过牌战法使用者", "supporter", Driver, "qun", 12));
            builder.AddGeneral(new(Enemy, "安静守军", "supporter", Quiet, "qun", 12));
            builder.AddDeck(new(Deck, "固定实体连弩", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "过牌战法实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
