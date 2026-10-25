using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDamageDrawTacticChecks
{
    private const string Owner = "fixture:damage-draw-owner", Driver = "fixture:damage-draw-driver";
    private const string Quiet = "fixture:damage-draw-quiet", Deck = "fixture:damage-draw-deck";
    private const string Cuixue = StandardShanheCampaignCatalog.CurrentCuixueSkillId;
    private const string Xialu = StandardShanheCampaignCatalog.CurrentXialuXiangfengSkillId;
    private const string Zhongji = StandardShanheCampaignCatalog.CurrentZhongjijiSkillId;
    private const string Ceding = StandardShanheCampaignCatalog.CurrentCedingTianxiaSkillId;
    private const string Ceding2 = StandardShanheCampaignCatalog.CurrentCedingTianxia2SkillId;

    public static void SlashTrickPhaseAllowancesAndDuelRecoveryUseNativeDamage()
    {
        var s = Scene.Start([Cuixue, Xialu, Ceding, Ceding2], cards: true);
        var firstHand = s.V().HandCount;
        s.Card(CardKind.Slash, 2);
        s.Card(CardKind.FireSlash, 2);
        Need(s.DrawCount(Cuixue) == 1 && s.V().HandCount == firstHand - 1 &&
            s.DrawCount(Ceding) == 0 && s.DrawCount(Ceding2) == 0,
            "The first actual Slash draws one, elemental Slash shares its round quota, and neither spends a trick-damage phase allowance.");
        s.Card(CardKind.Duel, 2);
        Need(s.DrawCount(Ceding) == 1 && s.DrawCount(Ceding2) == 2 &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 2 && e.Amount == 1 && e.SourceCardUseResolutionId is not null) == 3,
            "A physical Duel leaves Hand, actually damages its target, then pays the two independent locked trick-draw tactics once.");
        s.Card(CardKind.Duel, 2);
        Need(s.DrawCount(Ceding) == 1 && s.DrawCount(Ceding2) == 2,
            "A second actual damaging trick in the same Play phase cannot repeat either tactic.");

        var hp = s.V().Hp;
        s.Use("duel", [3, 0]);
        Need(s.V().Hp == hp &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 3 && e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            s.E<RecoveryAppliedEvent>().Count(e => e.TargetSeat == 0 && e.Amount == 1) == 1,
            "The actual incoming native Duel segment loses one HP before Xialu really recovers one; recovery does not manufacture another damage event.");
        s.NextOwnPlay();
        var zeroUse = s.ActualDuel(2);
        Need(s.DrawCount(Ceding) == 2 && s.DrawCount(Ceding2) == 4 && s.DrawCount(Cuixue) == 1 &&
            s.E<SkillUsageConsumedEvent>().Count(e => e.SkillId == Ceding && e.Scope == SkillUsageScope.Phase) == 2 &&
            s.E<SkillUsageConsumedEvent>().Count(e => e.SkillId == Ceding2 && e.Scope == SkillUsageScope.Phase) == 2 &&
            zeroUse is { CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0,
                Action: { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0 } } &&
            s.E<CardUseDeclaredEvent>().Count(e => e.ResolutionId == zeroUse.Id && e.CardId == 0 &&
                e.CardKind == CardKind.Duel && e.SourceSeat == 0) == 1 &&
            s.E<DamageAppliedEvent>().Count(e => e.SourceCardUseResolutionId == zeroUse.Id &&
                e.SourceSeat == 0 && e.TargetSeat == 2 && e.Amount == 1) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == zeroUse.Id) == 1,
            "A later real own Play phase refreshes both quotas, and an actual zero-entity Duel Use qualifies without confusing pure skill damage with a card Use.");
    }

    public static void HeavyHitUsesSingleAppliedDamageAndPublicEnemyRelation()
    {
        var s = Scene.Start([Zhongji], cards: false);
        var initialHand = s.V().HandCount;
        s.Use("two", [2]);
        s.Use("three", [1]);
        Need(s.DrawCount(Zhongji) == 0 && s.V().HandCount == initialHand,
            "Two actual enemy damage is below threshold, and three actual damage to a publicly allied seat is not enemy damage.");
        s.Use("three", [2]);
        Need(s.DrawCount(Zhongji) == 1 && s.V().HandCount == initialHand + 1,
            "One genuine three-damage enemy segment pays one native card draw, without accumulating earlier segments.");
        s.Use("four", [2]);
        Need(s.DrawCount(Zhongji) == 2,
            "One four-damage enemy segment pays only one draw rather than drawing for every damage point.");
        s.Use("four", [3]);
        Need(!s.G.CreateSnapshot(0).Players[3].IsAlive && s.DrawCount(Zhongji) == 3 &&
            s.E<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).Select(e => e.Amount).SequenceEqual([2, 3, 3, 4, 4]) &&
            s.E<DamageAppliedEvent>().All(e => e.SourceCardUseResolutionId is null),
            "A real lethal enemy segment still pays the draw; cardless native damage qualifies by its true amount and public team rather than a fake card identity.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }
    private static string Peer(int seat) => "fixture:damage-draw-peer-" + seat;

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V() => G.CreateSnapshot(0).Players[0];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int DrawCount(string skillId) => G.CardMovements.Count(move => move.To == CardLocation.Hand(0) &&
            move.Reason.Value == "skill-program." + skillId + ".Draw");
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native damage-draw tactic command."); }
        private void Until(Func<bool> done)
        {
            for (var step = 0; step < 300; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The small damage-draw battle ended before its expected boundary.");
                if (P is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Kind is DecisionKind.RescueDying or DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification
                        ? prompt.Choices.First(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die")
                        : prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native damage-draw tactic boundary.");
        }
        private void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Card(CardKind kind, int target)
        {
            var card = V().Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.TargetSeats.SequenceEqual([target]));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        public void Use(string activation, int[] targets)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets, G.Revision, P!.PromptId)); Play(); }
        public CardUseFrame ActualDuel(int target)
        {
            Accept(new UseProgramSkillCommand(0, Driver, "actual-owner-duel", [], [target], G.Revision, P!.PromptId));
            Until(() => G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.CardId == 0 && f.CardKind == CardKind.Duel));
            var use = G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == 0 && f.CardKind == CardKind.Duel);
            Play();
            return use;
        }
        public void NextOwnPlay()
        {
            var turn = G.State.TurnNumber;
            Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
            Until(() => G.State.TurnNumber > turn && P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        }
        public static Scene Start(string[] tactics, bool cards)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheDamageDrawTacticsPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:damage-draw-tactics", Owner, 10, 0, 0, 0, 4,
                [],
                cards ? [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.FireSlash, Suit.Spade, 8),
                    new(CardKind.Duel, Suit.Spade, 1), new(CardKind.Duel, Suit.Club, 1), new(CardKind.Duel, Suit.Diamond, 1)] : [], [],
                [new(Peer(2)) { StartingHand = [] }, new(Peer(3), Hp: 4) { StartingHand = [] }],
                [new(Peer(1)) { StartingHand = [] }], new string('d', 64))
                { TacticSkillIds = tactics };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-damage-draw-tactics", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"fixture-range","query":"attackRange","operation":"add","value":10,"priority":0}],"activations":[
                    {"id":"two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":2}]},
                    {"id":"three","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":3}]},
                    {"id":"four","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":4}]},
                    {"id":"actual-owner-duel","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"usesPerPhaseExpression":"currentRound","effects":[{"op":"useSelectedActorDuel","target":"owner","participantMode":"ownerToSelectedTarget"}]},
                    {"id":"duel","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"startVirtualDuel","target":"owner"}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "原生伤害补牌驱动", description = "真实牌移动、伤害、决斗和阶段" })
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "补牌战法使用者", "supporter", Driver, "qun", 10));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new(Peer(seat), "固定队伍角色", "supporter", Quiet, "wei", 14));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "伤害补牌战法实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
