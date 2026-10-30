namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 40700)] private void RequireOlGuixin(ProgramSkillFrame f)
    {
        if (f.SeatAcquisition?.Mode == ProgramSeatAcquisitionMode.PrioritizedRegion)
            SeatAcquisitions.Require(f, ProgramCapabilities.Get<IOlGuixinHost>().Policy);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 40700)]
    private bool OlGuixinMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        f.SeatAcquisition?.Mode == ProgramSeatAcquisitionMode.PrioritizedRegion &&
        SeatAcquisitions.MovementProof(f, effect, pending, ProgramCapabilities.Get<IOlGuixinHost>().Policy);
}
