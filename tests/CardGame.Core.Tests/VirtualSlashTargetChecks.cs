using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static class VirtualSlashTargetChecks
{
    public static void FinalAppearanceAndCommonRestrictions()
    {
        var game = Start();
        var players = Field<IReadOnlyList<CharacterState>>(game, "_players");
        var actor = players[0];
        var target = players[1];
        var legality = game.GetRuntimeModule<CoreNativeTargetLegalityRuntime>();
        var events = game.Events.Count;
        var zones = JsonSerializer.Serialize(game.CreateCardZoneDiagnostics());
        Need(Can(CardKind.Slash) && Can(CardKind.FireSlash), "The real fixture must initially allow both Slash kinds.");

        WithPolicy("normal", () => Need(!Can(CardKind.Slash) && Can(CardKind.FireSlash),
            "A normal-Slash target prohibition must not be reused as the answer for Fire Slash."));
        WithPolicy("fire", () => Need(Can(CardKind.Slash) && !Can(CardKind.FireSlash),
            "The common target query must enforce the declared effective Slash kind."));
        var driver = actor.SkillGrants.Grants.Single(grant => grant.SkillId == "fixture:wj-driver");
        var source = new CardUseEffectSource(driver.SkillId, "target-check", actor.Seat, driver.SkillInstanceId);
        var turn = game.State.TurnNumber;
        var turnSeat = game.State.CurrentSeat;
        var effects = Field<TurnCardUseEffectStore>(game, "_turnCardUseEffects");
        var restriction = effects.GrantTargetRestriction(turn, turnSeat, 9001, 0, source, SkillProgramCardTargetRestriction.SelfOnly);
        try { Need(!Can(CardKind.Slash), "SelfOnly applies to a cardless Slash before declaration."); }
        finally { ((IList<TurnCardTargetRestriction>)effects.TargetRestrictions).Remove(restriction); }

        var directed = Field<List<DirectedTurnCardPolicy>>(game, "_directedTurnCardPolicies");
        var policy = new DirectedTurnCardPolicy(9002, turn, turnSeat, 9002, 0, source, actor.Seat, target.Seat,
            [CardKind.FireSlash], DirectedTurnCardPolicyEffect.ForbidTarget);
        directed.Add(policy);
        try { Need(Can(CardKind.Slash) && !Can(CardKind.FireSlash), "Directed policies use the actual actor, target and effective kind."); }
        finally { directed.Remove(policy); }

        var shields = Field<List<BeneficiarySuitShield>>(game, "_beneficiarySuitShields");
        WithShield(Suit.None, () => Need(Can(CardKind.Slash) && !Can(CardKind.Slash, Suit.None) && Can(CardKind.Slash, Suit.Spade),
            "Unknown suit stays unknown; an explicitly suitless declaration is a distinct known suit."));
        WithShield(Suit.Spade, () => Need(Can(CardKind.Slash) && !Can(CardKind.Slash, Suit.Spade) && Can(CardKind.Slash, Suit.None),
            "Beneficiary shielding must share the same known-suit target answer."));

        var physical = game.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == CardKind.Slash && card.Suit == Suit.Spade);
        WithShield(Suit.Spade, () => Need(legality.CanUseVirtualSlashTarget(actor, target, CardKind.Slash,
            physical.Suit, false, physical.Rank, [physical.Id], ignoreDistance: true,
            actualUnconvertedPhysical: true, countsTowardLimit: false),
            "Liubing's first physical Slash has a final Diamond appearance, so the printed Spade cannot prohibit its target."));
        WithShield(Suit.Diamond, () => Need(!legality.CanUseVirtualSlashTarget(actor, target, CardKind.Slash,
            physical.Suit, false, physical.Rank, [physical.Id], ignoreDistance: true,
            actualUnconvertedPhysical: true, countsTowardLimit: false),
            "Final appearance must be computed before the common target policy check."));
        Need(game.Events.Count == events && JsonSerializer.Serialize(game.CreateCardZoneDiagnostics()) == zones,
            "Legality queries must not issue a use, move cards or consume Liubing's first-use opportunity.");

        bool Can(CardKind kind, Suit? suit = null) => legality.CanUseVirtualSlashTarget(actor, target, kind,
            suit, physicalCardIds: [], ignoreDistance: true, countsTowardLimit: false);
        void WithPolicy(string name, Action check)
        {
            var id = "fixture:virtual-target-" + name;
            var grant = new SkillGrant(id, id, id, "acquired:virtual-target-check");
            target.SkillGrants.Grant(grant);
            try { check(); }
            finally { target.SkillGrants.RemoveGrant(grant.GrantId); }
        }
        void WithShield(Suit suit, Action check)
        {
            var shield = new BeneficiarySuitShield(9003, target.Seat, suit,
                new(driver.SkillId, "target-check", actor.Seat, driver.SkillInstanceId));
            shields.Add(shield);
            try { check(); }
            finally { shields.Remove(shield); }
        }
    }

    private static GameEngine Start()
    {
        var type = typeof(WuJingChecks).GetNestedType("Fixture", BindingFlags.NonPublic)!;
        var native = (IGameContentPackage)Activator.CreateInstance(type,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, ["black"], null)!;
        var registry = ContentRegistry.Build(new StandardContentPackage(), native, new Policies());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord,
            ModeId = "identity:classic-wu-jing-fixture", UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12
        }, registry);
        Accept(new StartGameCommand());
        Invoke("Reach", game, (Func<bool>)(() => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }));
        Accept(new SelectGeneralCommand(0, "fixture:wj-owner", game.Revision, game.PendingDecision!.PromptId));
        Invoke("Play", game);
        return game;

        void Accept(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "Shared target fixture command was rejected.");
        }
    }

    private static void Invoke(string name, params object[] args)
    {
        try { typeof(WuJingChecks).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic)!.Invoke(null, args); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        { ExceptionDispatchInfo.Capture(error.InnerException).Throw(); }
    }
    private static T Field<T>(GameEngine game, string name) =>
        (T)FixtureRuntimeAccess.Engine.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Policies : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-virtual-slash-targets", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var specs = new[] { ("normal", "slash"), ("fire", "fireSlash") };
            var skills = specs.Select(row => new
            {
                id = "fixture:virtual-target-" + row.Item1, revision = 1,
                cardPolicies = new[] { new Dictionary<string, object>
                {
                    ["id"] = "target", ["kind"] = "prohibitTarget",
                    ["cardKinds"] = new[] { row.Item2 }
                } }
            }).ToArray();
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = skills.ToDictionary(row => row.id, _ => new { name = "共同目标规则", description = "共享杀目标资格" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
        }
    }
}
