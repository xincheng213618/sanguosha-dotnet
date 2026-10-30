namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 41400)] private void RequireHappyGuixin(ProgramSkillFrame f)
    {
        if (f.SeatAcquisition?.Mode == ProgramSeatAcquisitionMode.AllRegionsConditionalTurnover)
            SeatAcquisitions.Require(f, ProgramCapabilities.Get<IHappyGuixinHost>().Policy);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 41400)]
    private bool HappyGuixinMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        f.SeatAcquisition?.Mode == ProgramSeatAcquisitionMode.AllRegionsConditionalTurnover &&
        SeatAcquisitions.MovementProof(f, effect, pending, ProgramCapabilities.Get<IHappyGuixinHost>().Policy);
}
