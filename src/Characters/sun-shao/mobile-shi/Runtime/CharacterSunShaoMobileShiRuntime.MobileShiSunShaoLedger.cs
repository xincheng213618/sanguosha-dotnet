#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunShaoMobileShiRuntime
{
    internal bool HasShiSsCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiSsGanjue);

    internal bool IsShiSsGanjueStrikeUse(long useFrameId) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiSsGanjueIssuedEvent>().Any(entry => entry.UseId == useFrameId);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 68400)]
    internal bool ShiSsGanjueUseDistance(ProgramCardDistanceQuery query) => query.CardUseFrameId is { } id && IsShiSsGanjueStrikeUse(id);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 68400)]
    internal bool ShiSsGanjueSlashDistance(ProgramSlashDistanceQuery query) => query.ExistingUseFrameId is { } id && IsShiSsGanjueStrikeUse(id);
    internal bool ShiSsGanjueCannotRespond(int sourceSeat, int responderSeat, CardKind? incomingKind, IReadOnlyList<int> targetSeats)
    {
        if (!HasShiSsCapability || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || incomingKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || attack.ResolutionId == 0 || attack.SourceSeat != sourceSeat || attack.TargetSeat != responderSeat)
            return false;
        var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiSsGanjueIssuedEvent>().LastOrDefault(entry => entry.UseId == attack.ResolutionId && entry.TargetSeat == responderSeat);
        if (issued is null)
            return false;
        return !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[responderSeat]).Any(card => card.Suit == issued.StrikeSuit);
    }
}
