using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static class SlashUsePrefixChecks
{
    public static void DeclarationPrefixesKeepActorAndKindCutoffs()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new FireContent());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, registry);
        var reader = game.GetRuntimeModule<CoreGameEngineSlashUseOrdinalsRuntime>();

        // These are explicit journal inputs to the real reader, not injected
        // live uses or fabricated accepted CardActionContext instances.
        var current = Declaration(101, 1001, actor: 0, provider: 2);
        var other = Declaration(102, 1002, actor: 2, provider: 2);
        var response = Declaration(101, 1003, actor: 0, provider: 0,
            actionType: CardActionType.Response);
        IGameEvent[] history =
        [
            Declaration(100, 1000, actor: 1, provider: 1),
            current, other, response,
            Replace(101, previous: 0, actor: 1, provider: 2),
            Replace(102, previous: 2, actor: 1, provider: 2),
            Declaration(103, 1004, actor: 1, provider: 1)
        ];
        Require(Count(history, 1, 4, actor: 0) == 1 && Count(history, 1, 4, actor: 1) == 0 &&
                Count(history, 1, 4, actor: 2) == 1,
            "A prefix excludes earlier and later declarations and actor replacements; a played Slash response sharing its incoming origin is not another Use.");
        Require(Count(history, 1, 5, actor: 1) == 1 && Count(history, 1, 5, actor: 0) == 0,
            "An in-prefix actor change moves only the original Use, preserving its provider instead of following the response's provider.");
        Require(Count(history, 1, 4, actor: 1, active: current) == 1 &&
                Count(history, 1, 4, actor: 0, active: current) == 0,
            "Only the current origin follows actor changes beyond the cutoff; another origin's later change cannot enter the count.");

        var legacy = Declaration(201, null, actor: 0, provider: 0, instruction: 0);
        var nextInstruction = legacy with { ProgramInstructionIndex = 1 };
        Require(Count([legacy, nextInstruction], 0, 2, actor: 0) == 2,
            "Action-null program declarations keep separate original instruction identities without acquiring fake Actions.");
        Reject(() => Count([current, other with { CardActionId = current.CardActionId }], 0, 2, 0),
            "unique effective declarations and providers");
        Reject(() => Count([legacy, legacy], 0, 2, 0), "unique effective declarations and providers");
        Reject(() => Count([current with { RoundProviderSeat = null }], 0, 1, 0),
            "unique effective declarations and providers");
        Reject(() => Count([current with { RoundProviderSeat = game.PlayerCount }], 0, 1, 0),
            "unique effective declarations and providers");
        Reject(() => Count([current, Replace(101, previous: 0, actor: 1, provider: 0)], 0, 2, 1),
            "native actor-replacement chain");

        CheckFireReturn();

        int Count(IReadOnlyList<IGameEvent> facts, int start, int end, int actor,
            CurrentTurnCardUseKindsRecordedEvent? active = null) =>
            reader.CountSlashUsePrefix(facts, start, end, actor, elemental: false, current: active);

        void CheckFireReturn()
        {
            var program = registry.GetSkill("boundary:lihuo").Program!;
            var binding = program.Triggers.Single(trigger => trigger.Effects.Any(effect =>
                effect.Op == SkillProgramEffectOp.OfferCurrentSlashFireAndExtraTarget));
            var source = new CardConversionSource(program.Id, binding.Id, 0, "fixture:slash-prefix-lihuo");
            var original = Declaration(301, 3001, actor: 0, provider: 0);
            var sameOriginResponse = Declaration(301, 3002, actor: 0, provider: 0,
                actionType: CardActionType.Response);
            var completed = new ProgramBindingResolvedEvent(401, source.SkillId, source.BindingId,
                source.SkillInstanceId, source.OwnerSeat, binding.Window, Activated: true, Completed: true);
            IGameEvent[] fireHistory =
            [
                original,
                sameOriginResponse,
                new CardUseDeclaredEvent(original.OriginFrameId, 1, CardKind.Slash, original.ActorSeat),
                new ProgramBindingStartedEvent(401, source.SkillId, source.BindingId,
                    source.SkillInstanceId, source.OwnerSeat, binding.Window),
                new ProgramCurrentSlashFireChangedEvent(401, original.OriginFrameId,
                    original.CardActionId!.Value, source, CardKind.Slash, CardKind.FireSlash, null),
                completed
            ];
            Require(reader.SlashUseKindAtPrefix(original, fireHistory, 4, null) == CardKind.Slash &&
                    reader.SlashUseKindAtPrefix(original, fireHistory, 5, null) == CardKind.FireSlash,
                "Historical kind changes honor the cutoff while their matching completed producer return may occur later in the journal.");
            Require(reader.SlashUseKindAtPrefix(original, fireHistory, 4, original) == CardKind.FireSlash &&
                    reader.SlashUseKindAtPrefix(sameOriginResponse, fireHistory, 4, original) == CardKind.Slash &&
                    reader.CountSlashUsePrefix(fireHistory, 0, 4, 0, elemental: true, current: original) == 1,
                "The current declaration may observe its later committed fire conversion, but a distinct response sharing its origin cannot borrow that exception.");
            Reject(() => reader.SlashUseKindAtPrefix(original, [.. fireHistory, completed], 5, null),
                "exact native producer return");
        }
    }

    private static CurrentTurnCardUseKindsRecordedEvent Declaration(long origin, long? action,
        int actor, int? provider, CardActionType actionType = CardActionType.Use, int? instruction = null) =>
        new(1, 0, actor, origin, action, 0, 1, RoundActionType: actionType, RoundProviderSeat: provider)
        { EffectiveCardKind = CardKind.Slash, ProgramInstructionIndex = instruction };

    private static ProgramCardUseActorReplacedEvent Replace(long origin, int previous, int actor, int provider) =>
        new(501, "fixture:slash-prefix-role", previous, origin, previous, actor, provider);

    private static void Reject(Action action, string expected)
    {
        try { action(); }
        catch (InvalidOperationException error) when (error.Message.Contains(expected, StringComparison.Ordinal)) { return; }
        throw new InvalidOperationException("The Slash prefix accepted malformed journal input: " + expected);
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class FireContent : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-slash-use-prefix", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) => CharacterFixtureContent.Register(builder, "boundary:cheng-pu");
    }
}
