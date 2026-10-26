using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheIncomingReductionChecks
{
    private const string Owner = "fixture:houshi-owner", Driver = "fixture:houshi-driver", Quiet = "fixture:houshi-quiet";
    private const string EarlierPrevention = "fixture:houshi-earlier-prevention", Deck = "fixture:houshi-deck";
    private const string Skill = StandardShanheIncomingReductionPackage.Houshi2SkillId;
    private static string Peer(int seat) => "fixture:houshi-peer-" + seat;

    public static void RoundBudgetCountsZeroedSegmentsAndResetsAfterThree()
    {
        var s = Scene.Start(earlierPrevention: true);
        s.Use("incoming-four", [1]);
        Need(s.Reductions().Length == 0 && s.Usages(0).Length == 0 && s.V(0).Hp == 30 &&
            s.E<ProgramDamagePreventedEvent>() is [{ SkillId: EarlierPrevention }],
            "An earlier native prevention ends the damage window before Houshi begins, so its round allowance stays unused.");

        s.Use("source-less");
        Need(s.Reductions() is [{ OwnerSeat: 0, TargetSeat: 0, AmountBefore: 1, AmountAfter: 0 }] &&
            s.Usages(0) is [{ Scope: SkillUsageScope.Round, Count: 1 }] && s.E<DamageAppliedEvent>().Length == 0,
            "The first real incoming segment is source-less, reduces to zero and still consumes exactly one round use.");
        s.Use("incoming-three", [1]); s.Use("incoming-four", [1]); s.Use("incoming-three", [1]);
        Need(s.Reductions().Select(e => (e.AmountBefore, e.AmountAfter)).SequenceEqual([(1, 0), (3, 1), (4, 2)]) &&
            s.E<DamageAppliedEvent>().Select(e => (e.SourceSeat, e.TargetSeat, e.Amount)).SequenceEqual([(1, 0, 1), (1, 0, 2), (1, 0, 3)]) &&
            s.Usages(0).Length == 3 && s.Usages(0).All(e => e is { Scope: SkillUsageScope.Round, Count: 1 }) && s.V(0).Hp == 24,
            "The next two multi-point segments each consume one use and subtract two; the fourth segment in that round pays its full three damage.");

        s.NextOwnPlay();
        Need(s.E<RoundStartedEvent>().Last().RoundNumber == 2 && s.V(0).Hp == 24,
            "Normal native turns reach the next round without fixture damage or direct ledger changes.");
        s.Use("incoming-three", [1]);
        Need(s.Reductions().Last() is { OwnerSeat: 0, TargetSeat: 0, AmountBefore: 3, AmountAfter: 1 } &&
            s.Usages(0).Length == 4 && s.Usages(0).Last() is { Scope: SkillUsageScope.Round, Count: 1 } && s.V(0).Hp == 23,
            "The first incoming segment in round two receives a fresh native allowance and subtracts two again.");
        var reductions = s.Reductions().Length; var uses = s.Usages(0).Length; s.Use("lose");
        Need(s.V(0).Hp == 22 && s.Reductions().Length == reductions && s.Usages(0).Length == uses,
            "Direct HP loss remains outside the incoming-damage window and cannot spend a reduction use.");
    }

    public static void ElementalChainRecipientsReduceWithIndependentOwnerBudgets()
    {
        var s = Scene.Start(earlierPrevention: false);
        var chainA = s.Card([1, 2]); var chainB = s.Card([3]);
        Need(new[] { 1, 2, 3 }.All(seat => s.V(seat).IsChained) && !s.V(0).IsChained,
            "Two real paid Iron Chains link an unprotected origin and two separate production-tactic holders.");
        s.Use("fire-four", [1]);
        Need(s.E<DamageAppliedEvent>().Select(e => (e.SourceSeat, e.TargetSeat, e.Amount, e.Nature))
                .SequenceEqual([(0, 1, 4, DamageNature.Fire), (0, 2, 2, DamageNature.Fire)]) &&
            s.Reductions().Select(e => (e.OwnerSeat, e.TargetSeat, e.AmountBefore, e.AmountAfter))
                .SequenceEqual([(2, 2, 4, 2), (3, 3, 2, 0)]) &&
            s.E<ChainedDamagePropagatedEvent>().Length == 2 &&
            s.Usages(2) is [{ Scope: SkillUsageScope.Round, Count: 1 }] &&
            s.Usages(3) is [{ Scope: SkillUsageScope.Round, Count: 1 }] && s.Usages(0).Length == 0,
            "Both later native chain recipients receive one reduction: the original four becomes two, then zero under the existing propagated-segment contract; each target spends its own use and the source spends none.",
            new { Damage = s.E<DamageAppliedEvent>(), Reductions = s.Reductions(), Propagation = s.E<ChainedDamagePropagatedEvent>(),
                OwnerUses = s.Usages(0), FirstRecipientUses = s.Usages(2), LastRecipientUses = s.Usages(3) });
        foreach (var card in new[] { chainA, chainB })
            Need(s.G.CardMovements.Count(move => move.CardId == card && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing) == 1,
                "The original physical chain card is paid once through the native card movement path.");

        s.Use("hit-three", [2]); s.Use("hit-three", [3]);
        Need(s.Usages(2).Length == 2 && s.Usages(3).Length == 2 &&
            s.Usages(2).Concat(s.Usages(3)).All(e => e is { Scope: SkillUsageScope.Round, Count: 1 }) && s.Usages(0).Length == 0 &&
            s.Reductions().TakeLast(2).Select(e => (e.TargetSeat, e.AmountBefore, e.AmountAfter))
                .SequenceEqual([(2, 3, 1), (3, 3, 1)]),
            "Later independent incoming segments continue each recipient's own round counter without borrowing the other holder's or damage source's budget.");
        s.Use("source-less");
        Need(s.Usages(0) is [{ Scope: SkillUsageScope.Round, Count: 1 }] && s.V(0).Hp == 30 &&
            s.Reductions().Last() is { OwnerSeat: 0, TargetSeat: 0, AmountBefore: 1, AmountAfter: 0 },
            "The original damage source still owns its untouched first incoming reduction after the entire elemental chain.");
    }

    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(message + (details is null ? "" : " " + JsonSerializer.Serialize(details))); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public ProgramDamageReducedEvent[] Reductions() => E<ProgramDamageReducedEvent>().Where(e => e.SkillId == Skill).ToArray();
        public SkillUsageConsumedEvent[] Usages(int owner) => E<SkillUsageConsumedEvent>()
            .Where(e => e.SkillId == Skill && e.SkillOwnerSeat == owner).ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Houshi command."); }
        private void Play()
        {
            for (var step = 0; step < 280; step++)
            {
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(G.State.Winner == Winner.None, "The bounded Houshi battle ended before its expected boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native Houshi Play boundary.");
        }
        public void Use(string activation, int[]? targets = null)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId)); Play(); }
        public void NextOwnPlay()
        { Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId)); Play(); }
        public int Card(int[] targets)
        {
            var card = V(0).Hand.First(c => c.Kind == CardKind.IronChain);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play(); return card.Id;
        }
        public static Scene Start(bool earlierPrevention)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheIncomingReductionPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture:houshi", Owner, 30, 0, 0, 0, 0,
                earlierPrevention ? [EarlierPrevention] : [],
                [new(CardKind.IronChain, Suit.Club, 10), new(CardKind.IronChain, Suit.Spade, 11)], [],
                Enumerable.Range(1, 3).Select(seat => new ShanheCombatantDefinition(Peer(seat)) { StartingHand = [] }).ToArray(), [], new string('b', 64))
                { TacticSkillIds = [Skill] };
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-houshi", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"incoming-three","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":3}]},
                    {"id":"incoming-four","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":4}]},
                    {"id":"hit-three","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":3}]},
                    {"id":"fire-four","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":4,"nature":"fire"}]},
                    {"id":"source-less","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"receiveOwnerDamage","target":"owner","amount":1}]},
                    {"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{EarlierPrevention}}","revision":1,"triggers":[{"id":"prevent-earlier","window":"beforeDamageApplied","subject":"damageTarget","optional":false,"priority":100,"usageScope":"game","usageLimit":1,"effects":[{"op":"preventCurrentDamage","target":"owner"}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet, EarlierPrevention }.ToDictionary(id => id, _ => new { name = "厚实真实结算驱动", description = "原生伤害段与回合推进" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "厚实持有者", "supporter", Driver, "qun", 30));
            builder.AddGeneral(new(Peer(1), "未持有厚实", "supporter", Quiet, "wei", 30));
            builder.AddGeneral(new(Peer(2), "厚实传导接收者甲", "supporter", Quiet, "wei", 30, [Skill]));
            builder.AddGeneral(new(Peer(3), "厚实传导接收者乙", "supporter", Quiet, "wei", 30, [Skill]));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "厚实真实伤害段", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
