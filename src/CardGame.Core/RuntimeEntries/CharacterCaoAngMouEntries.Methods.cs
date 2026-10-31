#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoAngMouEntries
{
    internal void BindMouCaNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindMouCaNativeUse(use);
    internal bool CanRunMouCaEquipment(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunMouCaEquipment(c, context);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectMouCaEquipmentCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectMouCaEquipmentCandidates(batch);
    internal void CollectWindow_CollectMouCaEquipmentCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouCaEquipmentCandidates(batch, result);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateMouCaEquipmentContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateMouCaEquipmentContext(frame, candidate);
    internal bool ExcludeOrdinaryMovement_IsMouCaFengmin(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsMouCaFengmin(candidate, trigger);
    internal bool IsMouCaExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => _resolve().IsMouCaExactCardQuery(actor, selected);
    internal void IssueMouCaUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouCaUse(f);
    internal bool MouCaAdvanceChildren() => _resolve().MouCaAdvanceChildren();
    internal bool MouCaDying() => _resolve().MouCaDying();
    internal bool MouCaEdge(int i) => _resolve().MouCaEdge(i);
    internal global::System.Collections.Generic.IReadOnlyList<int> MouCaExpectedTargets(global::CardGame.Core.MouCaNativeUse n) => _resolve().MouCaExpectedTargets(n);
    internal bool MouCaFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouCaFirstChild(f, child);
    internal bool MouCaMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouCaMovement(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? MouCaObserverRoot() => _resolve().MouCaObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.MouCaPair> MouCaPairs(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouCaPairs(f);
    internal bool MouCaParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouCaParent(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> MouCaPlans(global::CardGame.Core.ProgramSkillFrame f, int actor, int id) => _resolve().MouCaPlans(f, actor, id);
    internal global::CardGame.Core.Card? MouCaQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id) => _resolve().MouCaQueryOriginalCard(actor, id);
    internal bool MouCaRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouCaRecoveryParent(f, recovery);
    internal bool MouCaSuffix(int n) => _resolve().MouCaSuffix(n);
    internal bool MouCaUseRidesOn(global::CardGame.Core.CardUseFrame u, global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouCaUseRidesOn(u, f);
    internal void PrepareMouCaDamageSource(global::CardGame.Core.GameEngine.IDamageAttempt attempt) => _resolve().PrepareMouCaDamageSource(attempt);
    internal bool? ProveMouCaMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveMouCaMovement(trigger, candidate, context);
    internal bool ResumeMouCaUse() => _resolve().ResumeMouCaUse();
    internal bool ReturnMouCaUse(global::CardGame.Core.CardUseFrame u) => _resolve().ReturnMouCaUse(u);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateMouCaEquipmentContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateMouCaEquipmentContext(frame, candidate);
    internal void ValidateMouCaUse(global::CardGame.Core.CardUseFrame u) => _resolve().ValidateMouCaUse(u);
    internal global::CardGame.Core.MouCaoAngRuntime MouCaoAng { get => _resolve().MouCaoAng; }
}
