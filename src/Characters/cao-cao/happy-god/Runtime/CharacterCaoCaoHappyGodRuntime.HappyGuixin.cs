#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoCaoHappyGodRuntime
{
    private static IProgramSeatAcquisitionPolicy Policy => new HappyGuixinPolicy();
    SkillProgramStepOutcome IHappyGuixinHost.Resolve(ProgramSkillFrame f) => acquisitions.Begin(f, Policy);

    private sealed class HappyGuixinPolicy : IProgramSeatAcquisitionPolicy
    {
        public ProgramSeatAcquisitionMode Mode => ProgramSeatAcquisitionMode.AllRegionsConditionalTurnover;
        public string SkillId => "happy:guixin";
        public string Reason => "skill-program.happy:guixin.obtain";
        public void PublishPriority(ProgramSkillFrame frame) =>
            throw new InvalidOperationException("An all-region acquisition does not choose a priority region.");
        public bool ShouldTurnOver(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt receipt, bool before) =>
            ProgramRandomSeatAcquisitionRuntime.ActualObtained(f) > 4 && !before;
        public IGameEvent Started(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r) =>
            new HappyGuixinStartedEvent(f.Id, r.Source, r.GameplayHash, r.DamageWindowId, r.DamageFrameId,
                r.OccurrenceIndex, r.DamageAmount, string.Join(",", r.OtherSeats));
        public IGameEvent Skipped(ProgramSkillFrame f, int cursor, int target) => new HappyGuixinSeatSkippedEvent(f.Id, cursor, target);
        public IGameEvent Paid(ProgramSkillFrame f, ProgramSeatAcquisitionPayment p) =>
            new HappyGuixinMovementPaidEvent(f.Id, p.Cursor, p.TargetSeat, p.BatchId!.Value, p.Before, p.After);
        public IGameEvent Turned(ProgramSkillFrame f, int obtained, bool before, bool after) =>
            new HappyGuixinTurnoverIssuedEvent(f.Id, obtained, before, after);
        public IGameEvent Completed(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r, bool cancelled) =>
            new HappyGuixinCompletedEvent(f.Id, ProgramRandomSeatAcquisitionRuntime.MovedSeats(r), ProgramRandomSeatAcquisitionRuntime.ActualObtained(f), r.TurnoverIssued, cancelled);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 41400)]
    internal bool ResumeHappyGuixin(long id) => acquisitions.Resume(id, Policy);
    [ProgramContinuation(ProgramContinuationKind.Movement, 41400)]
    internal bool ReturnHappyGuixinMovement(ProgramSkillFrame f) =>
        f.SeatAcquisition?.Mode == ProgramSeatAcquisitionMode.AllRegionsConditionalTurnover && ResumeHappyGuixin(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41400)]
    internal bool RetainHappyGuixinPaid(ProgramSkillFrame f) => acquisitions.RetainPaid(f, Policy);
}
