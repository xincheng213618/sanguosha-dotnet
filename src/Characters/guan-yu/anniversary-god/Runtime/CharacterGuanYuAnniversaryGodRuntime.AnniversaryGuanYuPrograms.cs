#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuAnniversaryGodRuntime
{
    void IAnniversaryGuanYuNightmareHost.RecordNightmare(ProgramSkillFrame frame) => RecordAnniversaryGuanYuNightmare(frame);
    internal void RecordAnniversaryGuanYuNightmare(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.DamageAppliedBeforeDying, Amount: > 0 } context || _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger is not { } damage || damage.Id != context.ParentFrameId)
            throw new InvalidOperationException("Wuhun lost its actual applied-damage point.");
        var target = frame.TriggerId == "outgoing-nightmare" ? context.TargetSeat : context.SourceSeat;
        if (target is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat))
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(frame, seat, PlayerMarkerKind.Nightmare, 1);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40825, SkillProgramEffectOp.Draw)]
    internal bool AllowsAnniversaryGuanYuHeartSlashDraw(ProgramTriggerLegalityContext gate) => gate.SkillId != "anniversary-god:wushen" || gate.Context.Facts is { DamageCardSuit: Suit.Heart, DamageCardIsSlash: true, DamageSourceIsOwner: true } && _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger is { } damage && damage.Id == gate.Context.ParentFrameId && _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(damage).CardUserSeat == gate.OwnerSeat;
}
