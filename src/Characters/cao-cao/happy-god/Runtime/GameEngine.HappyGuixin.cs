namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability] private sealed class HappyGuixinCapability(GameEngine e) : IHappyGuixinHost
    {
        public IProgramSeatAcquisitionPolicy Policy { get; } = new HappyGuixinPolicy(e);
        public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => e.SeatAcquisitions.Begin(f, Policy);
    }
    private sealed class HappyGuixinPolicy(GameEngine e) : IProgramSeatAcquisitionPolicy
    {
        public ProgramSeatAcquisitionMode Mode => ProgramSeatAcquisitionMode.AllRegionsConditionalTurnover;
        public string SkillId => "happy:guixin";
        public SkillProgramEffectOp Operation => SkillProgramEffectOp.ResolveHappyShenCaoCaoGuixin;
        public string Reason => "skill-program.happy:guixin.obtain";
        public string InvalidReceiptMessage => "Happy Guixin lost its original damage point, private random selection, actual paid gain or conditional turnover.";
        public void PublishPriority(ProgramSkillFrame f) => throw new InvalidOperationException("Happy Guixin has no priority-area choice.");
        public bool ShouldTurnOver(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r, bool before) => ProgramRandomSeatAcquisitionRuntime.ActualObtained(f) > 4 && !before;
        public IGameEvent Started(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r) => new HappyGuixinStartedEvent(f.Id, r.Source, r.GameplayHash, r.DamageWindowId, r.DamageFrameId, r.OccurrenceIndex, r.DamageAmount, string.Join(",", r.OtherSeats));
        public IGameEvent Skipped(ProgramSkillFrame f, int cursor, int target) => new HappyGuixinSeatSkippedEvent(f.Id, cursor, target);
        public IGameEvent Paid(ProgramSkillFrame f, ProgramSeatAcquisitionPayment p) => new HappyGuixinMovementPaidEvent(f.Id, p.Cursor, p.TargetSeat, p.BatchId, p.Before, p.After);
        public IGameEvent Turned(ProgramSkillFrame f, int obtained, bool before, bool after) => new HappyGuixinTurnoverIssuedEvent(f.Id, obtained, before, after);
        public IGameEvent Completed(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r, bool cancelled) => new HappyGuixinCompletedEvent(f.Id, r.Payments.Count, ProgramRandomSeatAcquisitionRuntime.ActualObtained(f), r.TurnoverIssued, cancelled);
        public bool ValidStarted(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r) => e.ProgramEventHistory<HappyGuixinStartedEvent>().Where(x => x.FrameId == f.Id).ToArray() is [var fact] && Equals(fact, Started(f, r));
        public bool ValidPriority(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r) => true;
        public bool ValidSkipped(ProgramSkillFrame f, int cursor, int target) => e.ProgramEventHistory<HappyGuixinSeatSkippedEvent>().Count(x => Equals(x, Skipped(f, cursor, target))) == 1;
        public bool HasSkipped(ProgramSkillFrame f, int cursor) => e.ProgramEventHistory<HappyGuixinSeatSkippedEvent>().Any(x => x.FrameId == f.Id && x.Cursor == cursor);
        public bool ValidPaid(ProgramSkillFrame f, ProgramSeatAcquisitionPayment p) => e.ProgramEventHistory<HappyGuixinMovementPaidEvent>().Where(x => x.FrameId == f.Id && x.Cursor == p.Cursor).ToArray() is [var fact] && Equals(fact, Paid(f, p));
        public bool ValidTurned(ProgramSkillFrame f, ProgramSeatAcquisitionReceipt r) => e.ProgramEventHistory<HappyGuixinTurnoverIssuedEvent>().Where(x => x.FrameId == f.Id).ToArray() is [var fact] && Equals(fact, Turned(f, ProgramRandomSeatAcquisitionRuntime.ActualObtained(f), false, r.FaceDownAfter!.Value));
        public bool NoTurnoverEvent(ProgramSkillFrame f) => !e.ProgramEventHistory<HappyGuixinTurnoverIssuedEvent>().Any(x => x.FrameId == f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 41400)] private bool ResumeHappyGuixin(long id) => SeatAcquisitions.Resume(id, ProgramCapabilities.Get<IHappyGuixinHost>().Policy);
    [ProgramContinuation(ProgramContinuationKind.Movement, 41400)] private bool ReturnHappyGuixinMovement(ProgramSkillFrame f) =>
        f.SeatAcquisition?.Mode == ProgramSeatAcquisitionMode.AllRegionsConditionalTurnover && ResumeHappyGuixin(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41400)] private bool RetainHappyGuixinPaid(ProgramSkillFrame f) => SeatAcquisitions.RetainPaid(f, ProgramCapabilities.Get<IHappyGuixinHost>().Policy);
}
