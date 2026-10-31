#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    internal GuanPhaseFocusEvent? GuanPhaseFocus(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<GuanPhaseFocusEvent>().LastOrDefault(e => e.OwnerSeat == owner && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseSerial == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner);
    internal int GuanFocusedSlashCount(GuanPhaseFocusEvent focus) => _coreProgramEventHistoryOperations.ProgramEventHistory<GuanFocusedSlashEvent>().Count(e => e.OwnerSeat == focus.OwnerSeat && e.TargetSeat == focus.TargetSeat && e.TurnNumber == focus.TurnNumber && e.PhaseSerial == focus.PhaseSerial);
    internal bool GuanFocusAllowsSlash(int owner, int target) => GuanPhaseFocus(owner)is { } focus && focus.TargetSeat == target && GuanFocusedSlashCount(focus) < 3;
    internal bool GuanFocusProhibitsSlash(int owner, int target, CardKind kind) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && GuanPhaseFocus(owner)is { } focus && focus.TargetSeat == target && GuanFocusedSlashCount(focus) >= 3;
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 210)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 30)]
    internal bool DistancePhysicalSlash210GuanFocusAllowsSlash(ProgramSlashDistanceQuery q) => GuanFocusAllowsSlash(q.Actor.Seat, q.Target.Seat);
}
