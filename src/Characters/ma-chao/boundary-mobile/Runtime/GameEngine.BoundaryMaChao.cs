namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryMaChaoMobileTieqi = "boundary:tieqi-mobile";

    private bool IsBoundaryMaChaoUnrespondable(long frameId) =>
        ActiveCardAttack is { } attack && attack.ResolutionId == frameId &&
        IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) &&
        GetCardAttackState(frameId).JudgmentSuitDodgeRestriction is { } restriction &&
        restriction.TargetSeat == attack.TargetSeat &&
        ProgramEventHistory<ProgramSlashSuitDiscardResolvedEvent>().Any(receipt =>
            receipt.ProgramFrameId == restriction.ProgramFrameId && receipt.SkillId == BoundaryMaChaoMobileTieqi &&
            receipt.OwnerSeat == attack.CardUserSeat && receipt.CardUseFrameId == frameId &&
            receipt.TargetSeat == attack.TargetSeat && !receipt.PaymentCompleted);
}
