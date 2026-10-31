#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineQimeiProgramsEntries
{
    internal IEnumerable<QimeiPairGrant> ActiveQimeiGrants() => _resolve().ActiveQimeiGrants();
    internal SkillProgramStepOutcome ArmQimeiPair(ProgramSkillFrame input) => _resolve().ArmQimeiPair(input);
    internal bool CanRunQimeiEquality(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunQimeiEquality(c, context);
    internal void CaptureQimeiHpQualification(HpChangeContext change) => _resolve().CaptureQimeiHpQualification(change);
    internal void CaptureQimeiMovementQualification(CardMovementBatchContext batch) => _resolve().CaptureQimeiMovementQualification(batch);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectQimeiCandidates(long nativeId, SkillProgramTriggerWindow window) => _resolve().CollectQimeiCandidates(nativeId, window);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectQimeiHpCandidates(HpChangeContext c) => _resolve().CollectQimeiHpCandidates(c);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectQimeiMovementCandidates(CardMovementBatchContext b) => _resolve().CollectQimeiMovementCandidates(b);
    internal void CollectWindow_CollectQimeiMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectQimeiMovementCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureQimeiMovementQualification(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureQimeiMovementQualification(batch, completed);
    internal ProgramSkillWindowContext CreateQimeiHpContext(HpChangeContext change, ProgramTriggerCandidate c) => _resolve().CreateQimeiHpContext(change, c);
    internal ProgramSkillWindowContext CreateQimeiMovementContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate c) => _resolve().CreateQimeiMovementContext(window, c);
    internal bool ExcludeOrdinaryMovement_IsQimeiEqualityTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsQimeiEqualityTrigger(candidate, trigger);
    internal bool HasQimeiHealthSubject(int seat) => _resolve().HasQimeiHealthSubject(seat);
    internal bool IsQimeiEqualityTrigger(SkillProgramTrigger t) => _resolve().IsQimeiEqualityTrigger(t);
    internal void ObserveQimeiExpiry(IGameEvent payload) => _resolve().ObserveQimeiExpiry(payload);
    internal bool? ProveQimeiHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveQimeiHealth(trigger, candidate, context);
    internal bool? ProveQimeiMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveQimeiMovement(trigger, candidate, context);
    internal bool QimeiPaidThisTurn(QimeiPairGrant grant, QimeiAttribute attribute) => _resolve().QimeiPaidThisTurn(grant, attribute);
    internal QimeiEqualityReceipt? QimeiQualification(ProgramTriggerCandidate c, long id) => _resolve().QimeiQualification(c, id);
    internal CardConversionSource QimeiSource(ProgramSkillFrame f) => _resolve().QimeiSource(f);
    internal void RequireQimeiDraw(ProgramSkillFrame f) => _resolve().RequireQimeiDraw(f);
    internal bool ResumeQimeiDraw(long id) => _resolve().ResumeQimeiDraw(id);
    internal SkillProgramStepOutcome SettleQimeiPairEquality(ProgramSkillFrame input) => _resolve().SettleQimeiPairEquality(input);
    internal ProgramSkillWindowContext? TryCreateQimeiMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateQimeiMovementContext(frame, candidate);
    internal bool ValidQimeiDraw(ProgramSkillFrame f) => _resolve().ValidQimeiDraw(f);
    internal bool HasQimeiCapability { get => _resolve().HasQimeiCapability; }
}
