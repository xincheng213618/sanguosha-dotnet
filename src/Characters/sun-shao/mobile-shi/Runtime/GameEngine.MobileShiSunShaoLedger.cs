namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasShiSsCapability =>
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiSsGanjue);

    // The strike is a cardless virtual use whose declared use id exempts every
    // distance stage during resolution. It is deliberately not marked against
    // the play-phase Slash count: 敢决's use never counts toward the limit.
    private bool IsShiSsGanjueStrikeUse(long useFrameId) =>
        ProgramEventHistory<ShiSsGanjueIssuedEvent>().Any(entry => entry.UseId == useFrameId);

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 68400)]
    private bool ShiSsGanjueUseDistance(ProgramCardDistanceQuery query) =>
        query.CardUseFrameId is { } id && IsShiSsGanjueStrikeUse(id);

    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 68400)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 68400)]
    private bool ShiSsGanjueSlashDistance(ProgramSlashDistanceQuery query) =>
        query.ExistingUseFrameId is { } id && IsShiSsGanjueStrikeUse(id);

    // 敢决's rider: while one of its strikes is the pending attack, a target
    // holding no hand card of the strike's suit cannot respond to it.
    private bool ShiSsGanjueCannotRespond(int sourceSeat, int responderSeat, CardKind? incomingKind,
        IReadOnlyList<int> targetSeats)
    {
        if (!HasShiSsCapability || _winner != Winner.None || incomingKind is not { } kind || !IsSlashCard(kind) ||
            ActiveCardAttack is not { } attack || attack.ResolutionId == 0 ||
            attack.SourceSeat != sourceSeat || attack.TargetSeat != responderSeat)
            return false;
        var issued = ProgramEventHistory<ShiSsGanjueIssuedEvent>()
            .LastOrDefault(entry => entry.UseId == attack.ResolutionId && entry.TargetSeat == responderSeat);
        if (issued is null) return false;
        return !GetHand(_players[responderSeat]).Any(card => card.Suit == issued.StrikeSuit);
    }
}
