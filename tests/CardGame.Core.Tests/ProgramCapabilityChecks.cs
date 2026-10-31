using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ProgramCapabilityChecks
{
    public static void RegistrationAndIsolationPreserveBoundaries()
    {
        var content = StandardContentRegistry.Create();
        var firstGame = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, content);
        var secondGame = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, content);
        var types = typeof(GameEngine).Assembly.GetTypes();
        var production = new ProgramCapabilityCatalog(types);
        var reversed = new ProgramCapabilityCatalog(types.Reverse());
        Require(production.Registrations.Select(item => item.Implementation)
                .SequenceEqual(reversed.Registrations.Select(item => item.Implementation)),
            "Discovery order must not change the registered capability modules.");

        var hostType = global::FixtureRuntimeAccess.Engine.GetNestedType("ProgramSkillHost", BindingFlags.NonPublic)!;
        Require(hostType.GetInterfaces().OrderBy(type => type.FullName, StringComparer.Ordinal)
                .SequenceEqual(new[] { typeof(ISkillProgramExecutionHost), typeof(ISkillProgramEffectHost), typeof(IResumableProgramDrawHost) }
                    .OrderBy(type => type.FullName, StringComparer.Ordinal)),
            "The shared host keeps execution/effect contracts plus the optional resumable Draw adapter; character capabilities stay separate.");
        var constructor = hostType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Single();
        var firstHost = (ISkillProgramEffectHost)firstGame.ComposeNativeProgramSkillHost();
        var secondHost = (ISkillProgramEffectHost)secondGame.ComposeNativeProgramSkillHost();
        var registryField = typeof(GameEngine).GetField("_programCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!;
        Require(registryField.GetValue(firstGame) is ProgramCapabilityRegistry firstRegistry &&
                registryField.GetValue(secondGame) is ProgramCapabilityRegistry secondRegistry &&
                !ReferenceEquals(firstRegistry, secondRegistry),
            "Each constructor must validate registration and install its own capability registry.");
        var get = typeof(ISkillProgramEffectHost).GetMethod(nameof(ISkillProgramEffectHost.GetCapability))!;
        foreach (var registration in production.Registrations)
        {
            object? shared = null;
            foreach (var contract in registration.Contracts)
            {
                var method = get.MakeGenericMethod(contract);
                var capability = method.Invoke(firstHost, null);
                Require(capability?.GetType() == registration.Implementation &&
                        ReferenceEquals(capability, method.Invoke(firstHost, null)) &&
                        !ReferenceEquals(capability, method.Invoke(secondHost, null)),
                    $"Production capability '{contract.FullName}' must construct, resolve and cache per game.");
                if (shared is not null)
                    Require(ReferenceEquals(shared, capability), "One module must share its instance across its own contracts.");
                shared = capability;
            }
        }

        CheckNativeBindings(firstGame, secondGame, production);

        FixtureCapability.Constructions = 0;
        var catalog = new ProgramCapabilityCatalog([typeof(UnregisteredCapability), typeof(FixtureCapability)]);
        var registry = new ProgramCapabilityRegistry(firstGame, catalog);
        var another = new ProgramCapabilityRegistry(secondGame, catalog);
        Require(FixtureCapability.Constructions == 0, "Registration must not eagerly construct capability modules.");
        var first = registry.Get<IFixtureCapability>();
        Require(FixtureCapability.Constructions == 1 && ReferenceEquals(first.Owner, firstGame) &&
                ReferenceEquals(first, registry.Get<IFixtureCapability>()) &&
                ReferenceEquals(first, registry.Get<ISecondFixtureCapability>()),
            "All interfaces of a module must share one lazily constructed instance for their game.");
        var second = another.Get<IFixtureCapability>();
        Require(FixtureCapability.Constructions == 2 && !ReferenceEquals(first, second) && ReferenceEquals(second.Owner, secondGame),
            "Shared metadata must never retain another game's adapter or state.");
        Require(!registry.TryGet<IMissingCapability>(out var missing) && missing is null,
            "An unknown optional capability must decline without manufacturing an implementation.");
        Reject<NotSupportedException>(() => registry.Get<IMissingCapability>());
        Reject<ArgumentException>(() => registry.Get<object>());
        Reject<ArgumentNullException>(() => new ProgramCapabilityCatalog(null!));
        Reject<InvalidOperationException>(() => new ProgramCapabilityCatalog([null!]));
        Reject<InvalidOperationException>(() => new ProgramCapabilityCatalog([]));
        Reject<InvalidOperationException>(() => new ProgramCapabilityCatalog([typeof(UnregisteredCapability)]));
        Reject<InvalidOperationException>(() => new ProgramCapabilityCatalog([typeof(FixtureCapability), typeof(FixtureCapability)]));
        Reject<InvalidOperationException>(() => new ProgramCapabilityCatalog([typeof(FixtureCapability), typeof(DuplicateCapability)]));
        foreach (var invalid in new[]
                 {
                     typeof(UnsealedCapability), typeof(GenericCapability<>), typeof(GenericCapability<int>), typeof(MissingConstructorCapability),
                     typeof(ExtraConstructorCapability), typeof(EmptyCapability), typeof(SharedHostCapability)
                 })
            Reject<InvalidOperationException>(() => new ProgramCapabilityCatalog([invalid]));

        var throwing = new ProgramCapabilityRegistry(firstGame, new ProgramCapabilityCatalog([typeof(ThrowingCapability)]));
        try { throwing.Get<IFixtureCapability>(); throw new Exception("A constructor failure was swallowed."); }
        catch (InvalidOperationException error)
        {
            Require(ReferenceEquals(error, ThrowingCapability.Failure), "Constructor failures must retain the original exception.");
        }

        var frame = new ProgramSkillFrame(41, 0, "fixture:capability", "run", "hash", 0, [], []);
        var legacy = new LegacyHost(frame);
        ISkillProgramEffectHost legacyHost = legacy;
        Require(ReferenceEquals(legacyHost.GetCapability<IConversionPolarityProgramHost>(), legacy) &&
                legacyHost.TryGetCapability<IConversionPolarityProgramHost>(out var direct) && ReferenceEquals(direct, legacy),
            "Existing custom hosts that implement dedicated interfaces must keep working without a registry.");
        new CommitConversionPolarityHandler().Execute(
            new(SkillProgramEffectOp.CommitConversionPolarity, SkillProgramEffectTarget.Owner, 0,
                new(SkillProgramConditionKind.Always, 0, [])), frame, frame.OwnerSeat, legacyHost);
        Require(legacy.Commits == 1, "A migrated effect handler must invoke an existing custom host exactly once.");
        Require(!legacyHost.TryGetCapability<IMissingCapability>(out _), "Legacy hosts must decline unsupported optional capabilities.");
        Reject<NotSupportedException>(() => legacyHost.GetCapability<IMissingCapability>());
    }

    private static void CheckNativeBindings(GameEngine firstGame, GameEngine secondGame, ProgramCapabilityCatalog production)
    {
        var registryField = typeof(GameEngine).GetField("_programCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var ownerField = typeof(GameEngine).GetField("_cardZones", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var first = ((ProgramCapabilityRegistry)registryField.GetValue(firstGame)!).Get<IAtomicCardMovementZones>();
        var second = ((ProgramCapabilityRegistry)registryField.GetValue(secondGame)!).Get<IAtomicCardMovementZones>();
        Require(ReferenceEquals(first, ownerField.GetValue(firstGame)) &&
                ReferenceEquals(second, ownerField.GetValue(secondGame)) && !ReferenceEquals(first, second),
            "Native capabilities must expose each match's actual zone owner, without an engine adapter or duplicate store.");
        Require(!production.TryResolve(typeof(IAtomicCardMovementZones), out _),
            "The direct native owner must not also be registered as a legacy adapter.");

        var store = new CardZoneStore(2);
        var provider = new MatchCoreCapabilityProvider(store);
        Require(provider.Bindings is System.Collections.IList { IsReadOnly: true } &&
                provider.Bindings.Select(binding => binding.Contract).ToHashSet()
                    .SetEquals([typeof(IAtomicCardMovementZones), typeof(ICardZoneQueries), typeof(ICardDrawPileOrder)]) &&
                provider.Bindings.All(binding => ReferenceEquals(binding.Instance, store)),
            "Typed composition exposes the dedicated read, atomic and draw-pile order contracts of one zone owner and freezes its binding list.");
        var history = ((ProgramCapabilityRegistry)registryField.GetValue(firstGame)!).Get<IProgramEventHistoryQueries>();
        var secondHistory = ((ProgramCapabilityRegistry)registryField.GetValue(secondGame)!).Get<IProgramEventHistoryQueries>();
        var historyField = typeof(GameEngine).GetField("_programHistory", BindingFlags.Instance | BindingFlags.NonPublic)!;
        Require(ReferenceEquals(history, historyField.GetValue(firstGame)) &&
                ReferenceEquals(secondHistory, historyField.GetValue(secondGame)) && !ReferenceEquals(history, secondHistory),
            "History queries must reach each match's actual incremental index without an engine adapter or shared cache.");
        var factoryCalls = 0;
        var supplied = provider.Bindings.ToList();
        var registry = new ProgramCapabilityRegistry(production, _ => { factoryCalls++; return new object(); }, supplied);
        supplied.Clear();
        var zones = registry.Get<IAtomicCardMovementZones>();
        Require(ReferenceEquals(zones, store) && ReferenceEquals(zones, registry.Get(typeof(IAtomicCardMovementZones))) && factoryCalls == 0,
            "Native resolution must use the original owner, freeze supplied bindings and bypass adapter construction.");
        Require(!registry.TryGet<IMissingCapability>(out var missing) && missing is null,
            "Native bindings must not manufacture unknown optional capabilities.");
        Reject<NotSupportedException>(() => registry.Get<IMissingCapability>());

        store.LoadInitialDeck([
            new Card(1, CardKind.Slash, Suit.Spade, 7), new Card(2, CardKind.Dodge, Suit.Heart, 2),
            new Card(3, CardKind.Peach, Suit.Diamond, 3)]);
        var beforeInvalid = store.CreateDiagnostics();
        Reject<ArgumentOutOfRangeException>(() => zones.Move(1, CardLocation.DrawPile, CardLocation.Hand(99)));
        Require(beforeInvalid.SequenceEqual(store.CreateDiagnostics()), "The direct port retains invalid destination atomicity.");
        zones.Move(1, CardLocation.DrawPile, CardLocation.Hand(0));
        beforeInvalid = store.CreateDiagnostics();
        Reject<InvalidOperationException>(() => zones.MoveMany([2, 1], CardLocation.DrawPile, CardLocation.Hand(1)));
        Require(beforeInvalid.SequenceEqual(store.CreateDiagnostics()), "The direct port validates an entire batch before changing any zone.");
        var liveHand = zones.CardsAt(CardLocation.Hand(1));
        Require(zones.MoveMany([2, 3], CardLocation.DrawPile, CardLocation.Hand(1)).Select(card => card.Id).SequenceEqual([2, 3]) &&
                liveHand.Count == 2 && liveHand is not List<Card>,
            "Native moves retain entity order and the existing read-only live zone view.");
        Require(zones.MoveAll(CardLocation.Hand(1), CardLocation.DiscardPile).Select(card => card.Id).SequenceEqual([2, 3]) &&
                zones.MoveBatch([new(1, CardLocation.Hand(0), CardLocation.Hand(1)),
                    new(2, CardLocation.DiscardPile, CardLocation.Hand(0))]).Select(card => card.Id).SequenceEqual([1, 2]) &&
                zones.GetLocation(1) == CardLocation.Hand(1),
            "Every atomic zone primitive addresses the same state owner and preserves ordered physical identities.");
        store.AssertInvariants(3);

        var facts = new NativeFacts(5);
        var nativeCatalog = new ProgramCapabilityCatalog([typeof(NativeFactsAdapter)]);
        foreach (var invalid in new ProgramNativeCapabilityBinding[]
                 {
                     null!, new(null!, facts), new(typeof(object), facts), new(typeof(INativeGeneric<>), facts),
                     new(typeof(IProgramSelectionQuery), facts), new(typeof(IProgramRuntimeOperations), store),
                     new(typeof(ISkillProgramEffectHost), new object()), new(typeof(IFixtureCapability), facts),
                     new(typeof(INativeFacts), null!), new(typeof(INativeFacts), new object()), new(typeof(INativeFacts), firstGame)
                 })
            Reject<InvalidOperationException>(() => new ProgramCapabilityRegistry(production,
                _ => { factoryCalls++; return new object(); }, [invalid]));
        Reject<InvalidOperationException>(() => new ProgramCapabilityRegistry(production,
            _ => { factoryCalls++; return new object(); }, [new(typeof(INativeFacts), facts), new(typeof(INativeFacts), facts)]));
        Reject<InvalidOperationException>(() => new ProgramCapabilityRegistry(nativeCatalog,
            _ => { factoryCalls++; return new object(); }, [new(typeof(INativeFacts), facts)]));
        Require(factoryCalls == 0, "Invalid native bindings must fail during composition, before any adapter or rule executes.");

        var mismatchedFactoryCalls = 0;
        var mismatched = new ProgramCapabilityRegistry(nativeCatalog, _ => { mismatchedFactoryCalls++; return new object(); });
        Reject<InvalidOperationException>(() => mismatched.Get<INativeFacts>());
        Reject<InvalidOperationException>(() => mismatched.Get<INativeFacts>());
        Require(mismatchedFactoryCalls == 2,
            "A mismatched adapter factory must fail without caching success or leaving the dependency guard stuck.");
        var legacy = new ProgramCapabilityRegistry(firstGame, new ProgramCapabilityCatalog([typeof(FixtureCapability)]));
        Require(!legacy.TryGet<IAtomicCardMovementZones>(out _),
            "Isolated legacy catalogs retain their original scope and do not reach back into the engine for native owners.");
    }

    private interface INativeFacts : IProgramSelectionQuery { int Value { get; } }
    private interface INativeGeneric<T> : IProgramSelectionQuery { }
    private sealed record NativeFacts(int Value) : INativeFacts;
    [ProgramCapability]
    private sealed class NativeFactsAdapter : INativeFacts
    {
        internal NativeFactsAdapter(GameEngine engine) { }
        public int Value => 0;
    }

    private interface IFixtureCapability { GameEngine Owner { get; } }
    private interface ISecondFixtureCapability { }
    private interface IMissingCapability { }

    [ProgramCapability]
    private sealed class FixtureCapability : IFixtureCapability, ISecondFixtureCapability
    {
        internal static int Constructions;
        public GameEngine Owner { get; }
        internal FixtureCapability(GameEngine engine) { Owner = engine; Constructions++; }
    }

    private sealed class UnregisteredCapability : IFixtureCapability
    {
        public GameEngine Owner => throw new NotSupportedException();
    }

    [ProgramCapability]
    private sealed class DuplicateCapability(GameEngine engine) : IFixtureCapability
    {
        public GameEngine Owner => engine;
    }

    [ProgramCapability]
    private class UnsealedCapability(GameEngine engine) : IFixtureCapability
    {
        public GameEngine Owner => engine;
    }

    [ProgramCapability]
    private sealed class GenericCapability<T>(GameEngine engine) : IFixtureCapability
    {
        public GameEngine Owner => engine;
    }

    [ProgramCapability]
    private sealed class MissingConstructorCapability : IFixtureCapability
    {
        public GameEngine Owner => throw new NotSupportedException();
    }

    [ProgramCapability]
    private sealed class ExtraConstructorCapability : IFixtureCapability
    {
        internal ExtraConstructorCapability() { }
        internal ExtraConstructorCapability(GameEngine engine) { }
        public GameEngine Owner => throw new NotSupportedException();
    }

    [ProgramCapability]
    private sealed class EmptyCapability
    {
        internal EmptyCapability(GameEngine engine) { }
    }

    [ProgramCapability]
    private sealed class SharedHostCapability : SkillProgramExecutorChecks.FakeRuntime
    {
        internal SharedHostCapability(GameEngine engine)
            : base(null!, new(41, 0, "fixture:capability", "run", "hash", 0, [], [])) { }
    }

    [ProgramCapability]
    private sealed class ThrowingCapability : IFixtureCapability
    {
        internal static readonly InvalidOperationException Failure = new("Fixture constructor failure.");
        internal ThrowingCapability(GameEngine engine) => throw Failure;
        public GameEngine Owner => throw new NotSupportedException();
    }

    private sealed class LegacyHost(ProgramSkillFrame frame)
        : SkillProgramExecutorChecks.FakeRuntime(null!, frame), IConversionPolarityProgramHost
    {
        internal int Commits { get; private set; }
        public void CommitConversionPolarity(ProgramSkillFrame current)
        {
            Require(ReferenceEquals(current, Frame), "The legacy capability lost the caller's original frame.");
            Commits++;
        }
    }

    private static void Reject<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Expected {typeof(T).Name} for an invalid capability operation.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
