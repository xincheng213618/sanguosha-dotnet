#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoAngMouEntries
{
    internal void BindMouCaNativeUse(CardUseFrame use) => _resolve().BindMouCaNativeUse(use);
    internal bool CanRunMouCaEquipment(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunMouCaEquipment(c, context);
    internal IEnumerable<ProgramTriggerCandidate> CollectMouCaEquipmentCandidates(CardMovementBatchContext batch) => _resolve().CollectMouCaEquipmentCandidates(batch);
    internal void CollectWindow_CollectMouCaEquipmentCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouCaEquipmentCandidates(batch, result);
    internal ProgramSkillWindowContext CreateMouCaEquipmentContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateMouCaEquipmentContext(frame, candidate);
    internal bool ExcludeOrdinaryMovement_IsMouCaFengmin(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsMouCaFengmin(candidate, trigger);
    internal bool IsMouCaExactCardQuery(CharacterState actor, int? selected) => _resolve().IsMouCaExactCardQuery(actor, selected);
    internal void IssueMouCaUse(ProgramSkillFrame f) => _resolve().IssueMouCaUse(f);
    internal bool MouCaAdvanceChildren() => _resolve().MouCaAdvanceChildren();
    internal bool MouCaDying() => _resolve().MouCaDying();
    internal bool MouCaEdge(int i) => _resolve().MouCaEdge(i);
    internal IReadOnlyList<int> MouCaExpectedTargets(MouCaNativeUse n) => _resolve().MouCaExpectedTargets(n);
    internal bool MouCaFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouCaFirstChild(f, child);
    internal bool MouCaMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouCaMovement(f, effect, p);
    internal ProgramSkillFrame? MouCaObserverRoot() => _resolve().MouCaObserverRoot();
    internal IReadOnlyList<MouCaPair> MouCaPairs(ProgramSkillFrame f) => _resolve().MouCaPairs(f);
    internal bool MouCaParent(ProgramSkillFrame f) => _resolve().MouCaParent(f);
    internal IReadOnlyList<LegalAction> MouCaPlans(ProgramSkillFrame f, int actor, int id) => _resolve().MouCaPlans(f, actor, id);
    internal Card? MouCaQueryOriginalCard(CharacterState actor, int? id) => _resolve().MouCaQueryOriginalCard(actor, id);
    internal bool MouCaRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouCaRecoveryParent(f, recovery);
    internal bool MouCaSuffix(int n) => _resolve().MouCaSuffix(n);
    internal bool MouCaUseRidesOn(CardUseFrame u, ProgramSkillFrame f) => _resolve().MouCaUseRidesOn(u, f);
    internal void PrepareMouCaDamageSource(GameEngine.IDamageAttempt attempt) => _resolve().PrepareMouCaDamageSource(attempt);
    internal bool? ProveMouCaMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveMouCaMovement(trigger, candidate, context);
    internal bool ResumeMouCaUse() => _resolve().ResumeMouCaUse();
    internal bool ReturnMouCaUse(CardUseFrame u) => _resolve().ReturnMouCaUse(u);
    internal ProgramSkillWindowContext? TryCreateMouCaEquipmentContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateMouCaEquipmentContext(frame, candidate);
    internal void ValidateMouCaUse(CardUseFrame u) => _resolve().ValidateMouCaUse(u);
    internal MouCaoAngRuntime MouCaoAng { get => _resolve().MouCaoAng; }
}
