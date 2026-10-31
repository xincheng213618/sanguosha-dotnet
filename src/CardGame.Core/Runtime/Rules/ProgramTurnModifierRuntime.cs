namespace CardGame.Core;
internal interface IProgramTurnQueries : IProgramSelectionQuery
{
    int TurnNumber { get; }

    int CurrentSeat { get; }

    int TurnOwnerSeat { get; }

    int TurnOwnerTurn { get; }
}

internal interface IProgramTurnModifiers : IProgramRuntimeOperations
{
    TurnRuleModifier Grant(ProgramSkillFrame frame, SkillRuleQuery query, SkillRuleOperation operation, int amount, int? affectedSeat = null, IReadOnlyList<CardKind>? cardKinds = null, SkillProgramDamageModifierExpiration expiration = SkillProgramDamageModifierExpiration.CurrentTurnEnd);
}

internal interface ITurnRuleModifierStore : IProgramRuntimeOperations
{
    TurnRuleModifier GrantRuleModifier(int turnNumber, int turnSeat, long parentFrameId, int effectIndex, CardUseEffectSource source, SkillRuleQuery query, SkillRuleOperation operation, int amount, IReadOnlyList<CardKind>? cardKinds, int? affectedSeat, SkillProgramDamageModifierExpiration expiration);
}

// The original turn-effect store owns grant keys and expiry. This runtime owns
// the active-instruction check and the grant/fact order shared by its callers.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramTurnModifierRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events, ISelectionParticipantQueries participants, IProgramTurnQueries turn, ITurnRuleModifierStore modifiers) : IProgramTurnModifiers
{
    public TurnRuleModifier Grant(ProgramSkillFrame frame, SkillRuleQuery query, SkillRuleOperation operation, int amount, int? affectedSeat = null, IReadOnlyList<CardKind>? cardKinds = null, SkillProgramDamageModifierExpiration expiration = SkillProgramDamageModifierExpiration.CurrentTurnEnd)
    {
        if (frames.Top is not ProgramSkillFrame current || current.Id != frame.Id || !participants.IsValidSeat(frame.OwnerSeat) || turn.TurnNumber <= 0 || (frame.TriggerId is null && string.IsNullOrWhiteSpace(frame.ActivationId)))
            throw new InvalidOperationException("A turn effect requires an active program frame in an ongoing turn.");
        var source = RuntimeSkills.RuntimeRules_Core_CardUseModules.CreateProgramTurnEffectSource(frame);
        var grant = modifiers.GrantRuleModifier(turn.TurnNumber, turn.TurnOwnerSeat, frame.Id, frame.InstructionIndex - 1, source, query, operation, amount, cardKinds, affectedSeat, expiration);
        events.AdvanceFact(new TurnRuleModifierGrantedEvent(grant));
        return grant;
    }
}

internal sealed partial class TurnCardUseEffectStore : ITurnRuleModifierStore
{
    TurnRuleModifier ITurnRuleModifierStore.GrantRuleModifier(int turnNumber, int turnSeat, long parentFrameId, int effectIndex, CardUseEffectSource source, SkillRuleQuery query, SkillRuleOperation operation, int amount, IReadOnlyList<CardKind>? cardKinds, int? affectedSeat, SkillProgramDamageModifierExpiration expiration) => GrantRuleModifier(turnNumber, turnSeat, parentFrameId, effectIndex, source, query, operation, amount, cardKinds, affectedSeat, expiration: expiration);
}
