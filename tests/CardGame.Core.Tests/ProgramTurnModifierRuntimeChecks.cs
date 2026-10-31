using System.Reflection;
using CardGame.Core;

internal static class ProgramTurnModifierRuntimeChecks
{
    internal static void CheckNativeOwnershipAndGrantBoundaries(GameEngine game,
        ProgramCapabilityRegistry capabilities, ProgramCapabilityRegistry otherCapabilities)
    {
        var store = (TurnCardUseEffectStore)typeof(GameEngine)
            .GetField("_turnCardUseEffects", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        Require(ReferenceEquals(capabilities.Get<ITurnRuleModifierStore>(), store) &&
                !ReferenceEquals(store, otherCapabilities.Get<ITurnRuleModifierStore>()),
            "Turn grants must mutate the original match store and cannot share another match's store.");
        Require(ReferenceEquals(capabilities.Get<IProgramTurnModifiers>(), game.GetRuntimeModule<ProgramTurnModifierRuntime>()) &&
                ReferenceEquals(capabilities.Get<IProgramSkillAcquisitions>(), game.GetRuntimeModule<ProgramSkillChangeRuntime>()) &&
                ReferenceEquals(capabilities.Get<IProgramSkillGrantQueries>(), capabilities.Get<IProgramSkillChangeFacts>()),
            "Character consumers, shared mutations and native callers must resolve the same match-owned service instances.");

        var roles = capabilities.Get<IMatchRoleQueries>();
        var players = (IReadOnlyList<CharacterState>)global::FixtureRuntimeAccess.Engine
            .GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var first = players[0];
        var second = players.First(player => player.Role != first.Role);
        var firstRole = first.Role;
        var secondRole = second.Role;
        Require(!ReferenceEquals(roles, otherCapabilities.Get<IMatchRoleQueries>()),
            "Role queries must retain their own match's participants.");
        first.ExchangeIdentityWith(second);
        try
        {
            Require(roles.Role(first.Seat) == secondRole && roles.Role(second.Seat) == firstRole,
                "Runtime role reads must observe actual identity exchanges without a cached role snapshot.");
        }
        finally { first.ExchangeIdentityWith(second); }

        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var parent = frames.Top;
        var turn = new Turn { TurnNumber = 12, CurrentSeat = 1, TurnOwnerSeat = 1, TurnOwnerTurn = 12 };
        var localStore = new TurnCardUseEffectStore();
        var facts = new Events(localStore);
        var runtime = new ProgramTurnModifierRuntime(frames, facts,
            capabilities.Get<ISelectionParticipantQueries>(), turn, localStore);
        var frame = new ProgramSkillFrame(frames.NextId(), 0, "fixture:turn-modifier", "run", "hash", 2, [], []);
        frames.Push(frame);
        try
        {
            var grant = runtime.Grant(frame, SkillRuleQuery.HandLimit, SkillRuleOperation.Add, -1, affectedSeat: 1);
            Require(grant.TurnNumber == 12 && grant.TurnSeat == 1 && grant.ParentFrameId == frame.Id &&
                    grant.EffectIndex == 1 && grant.Source.OwnerSeat == 0 && grant.AffectedSeat == 1 &&
                    grant.Expiration == SkillProgramDamageModifierExpiration.CurrentTurnEnd &&
                    facts.Facts is [TurnRuleModifierGrantedEvent { Modifier: var issued }] && ReferenceEquals(issued, grant),
                "A grant must preserve its original owner, instruction, affected seat, turn and native expiry in the issued fact.");
            Require(ReferenceEquals(runtime.Grant(frame, SkillRuleQuery.HandLimit, SkillRuleOperation.Add, -1, 1), grant) &&
                    localStore.RuleModifiers.Count == 1,
                "Re-entering the same instruction must reuse the native grant key instead of applying its modifier twice.");
            var factCount = facts.Facts.Count;
            Reject(() => runtime.Grant(frame, SkillRuleQuery.HandLimit, SkillRuleOperation.Add, -2, 1));
            Require(localStore.RuleModifiers.Count == 1 && facts.Facts.Count == factCount,
                "A changed meaning for an issued key must fail before emitting a new fact.");

            var child = new ResponseWindowFrame(frames.NextId(), frame.Id, 0, 1, CardKind.Slash, RequiredCardKind: CardKind.Dodge);
            frames.Push(child);
            try { Reject(() => runtime.Grant(frame, SkillRuleQuery.HandLimit, SkillRuleOperation.Add, -1, 1)); }
            finally { frames.Pop(child.Id, child.Kind); }
            turn.TurnNumber = 0;
            Reject(() => runtime.Grant(frame, SkillRuleQuery.HandLimit, SkillRuleOperation.Add, -1, 1));
            Require(facts.Facts.Count == factCount && localStore.RuleModifiers.Count == 1,
                "Suspended frames and missing actual turns cannot grant modifiers or emit rule facts.");
        }
        finally { frames.Pop(frame.Id, frame.Kind); }
        Require(ReferenceEquals(frames.Top, parent), "The mechanism check must restore the original native frame owner.");
    }

    private sealed class Turn : IProgramTurnQueries
    {
        public int TurnNumber { get; set; }
        public int CurrentSeat { get; set; }
        public int TurnOwnerSeat { get; set; }
        public int TurnOwnerTurn { get; set; }
    }

    private sealed class Events(TurnCardUseEffectStore store) : IProgramRuntimeEvents
    {
        internal List<IGameEvent> Facts { get; } = [];
        public void AdvanceFact(IGameEvent fact)
        {
            Require(fact is TurnRuleModifierGrantedEvent issued && store.RuleModifiers.Any(grant => ReferenceEquals(grant, issued.Modifier)),
                "Rule advancement must see the original modifier already committed to its native store.");
            Facts.Add(fact);
        }
    }

    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new Exception("An invalid active-turn modifier grant was accepted.");
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new Exception(message); }
}
