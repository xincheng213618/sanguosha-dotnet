#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineQimeiProgramsEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.QimeiPairGrant> ActiveQimeiGrants() => _resolve().ActiveQimeiGrants();
    internal global::CardGame.Core.SkillProgramStepOutcome ArmQimeiPair(global::CardGame.Core.ProgramSkillFrame input) => _resolve().ArmQimeiPair(input);
    internal bool CanRunQimeiEquality(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunQimeiEquality(c, context);
    internal void CaptureQimeiHpQualification(global::CardGame.Core.HpChangeContext change) => _resolve().CaptureQimeiHpQualification(change);
    internal void CaptureQimeiMovementQualification(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureQimeiMovementQualification(batch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectQimeiCandidates(long nativeId, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().CollectQimeiCandidates(nativeId, window);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectQimeiHpCandidates(global::CardGame.Core.HpChangeContext c) => _resolve().CollectQimeiHpCandidates(c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectQimeiMovementCandidates(global::CardGame.Core.CardMovementBatchContext b) => _resolve().CollectQimeiMovementCandidates(b);
    internal void CollectWindow_CollectQimeiMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectQimeiMovementCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureQimeiMovementQualification(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureQimeiMovementQualification(batch, completed);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateQimeiHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateQimeiHpContext(change, c);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateQimeiMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateQimeiMovementContext(window, c);
    internal bool ExcludeOrdinaryMovement_IsQimeiEqualityTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsQimeiEqualityTrigger(candidate, trigger);
    internal bool HasQimeiHealthSubject(int seat) => _resolve().HasQimeiHealthSubject(seat);
    internal bool IsQimeiEqualityTrigger(global::CardGame.Core.SkillProgramTrigger t) => _resolve().IsQimeiEqualityTrigger(t);
    internal void ObserveQimeiExpiry(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveQimeiExpiry(payload);
    internal bool? ProveQimeiHealth(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveQimeiHealth(trigger, candidate, context);
    internal bool? ProveQimeiMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveQimeiMovement(trigger, candidate, context);
    internal bool QimeiPaidThisTurn(global::CardGame.Core.QimeiPairGrant grant, global::CardGame.Core.QimeiAttribute attribute) => _resolve().QimeiPaidThisTurn(grant, attribute);
    internal global::CardGame.Core.QimeiEqualityReceipt? QimeiQualification(global::CardGame.Core.ProgramTriggerCandidate c, long id) => _resolve().QimeiQualification(c, id);
    internal global::CardGame.Core.CardConversionSource QimeiSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QimeiSource(f);
    internal void RequireQimeiDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireQimeiDraw(f);
    internal bool ResumeQimeiDraw(long id) => _resolve().ResumeQimeiDraw(id);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleQimeiPairEquality(global::CardGame.Core.ProgramSkillFrame input) => _resolve().SettleQimeiPairEquality(input);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateQimeiMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateQimeiMovementContext(frame, candidate);
    internal bool ValidQimeiDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidQimeiDraw(f);
    internal bool HasQimeiCapability { get => _resolve().HasQimeiCapability; }
}
