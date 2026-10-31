#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMengHuoBoundaryMobileRuntime
{
    internal static bool IsMobileMengHuoZaiqi(ProgramSkillFrame frame) => frame.SkillId == "boundary:zaiqi-mobile";
    internal bool ValidMobileOrOlRedDiscardRecoveryWindow(ProgramSkillFrame frame) => frame.OwnerSeat == _coreActualTurnProgressCurrentSeatAccess._currentSeat && frame.WindowContext?.Window == (IsMobileMengHuoZaiqi(frame) ? SkillProgramTriggerWindow.DiscardPhaseEnded : SkillProgramTriggerWindow.TurnEnding);
    internal bool CanMobileOrOlRedDiscardRecoverySelect(ProgramSkillFrame frame, int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && (seat != frame.OwnerSeat || IsMobileMengHuoZaiqi(frame));
}
