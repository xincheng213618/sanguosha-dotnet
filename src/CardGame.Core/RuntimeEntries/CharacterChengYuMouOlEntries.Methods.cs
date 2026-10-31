#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengYuMouOlEntries
{
    internal void ApplyMouLidanTargets(ProgramSkillFrame f, MouLidanSelection selection) => _resolve().ApplyMouLidanTargets(f, selection);
    internal void BindMouDanchiUse(CardUseDeclaredEvent declared) => _resolve().BindMouDanchiUse(declared);
    internal bool CanMouDanchiSlash(int actor, int target) => _resolve().CanMouDanchiSlash(actor, target);
    internal void CollectMouChengYuIssued(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectMouChengYuIssued(action, window, result);
    internal void ConsumeMouLidanCounterspell(CardActionContext action) => _resolve().ConsumeMouLidanCounterspell(action);
    internal bool FinishMouDanchiSlash(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouDanchiSlash(attack, out paused);
    internal bool HasMouLidanTargetTail(CardUseFrame use) => _resolve().HasMouLidanTargetTail(use);
    internal bool IsIssuedMouChengYu(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedMouChengYu(c, context);
    internal bool IsMouChengYuTrueUse(CardActionContext action) => _resolve().IsMouChengYuTrueUse(action);
    internal bool IsMouDanchiDrawTwo(long id) => _resolve().IsMouDanchiDrawTwo(id);
    internal bool IsMouDanchiSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().IsMouDanchiSettlementBatch(f, batch);
    internal bool IsMouDanchiUse(long id) => _resolve().IsMouDanchiUse(id);
    internal bool IsMouLidanAppliedTargetFact(CardUseFrame use, MouLidanTargetsAppliedEvent fact, IReadOnlyList<int> precedingTargets, scoped out IReadOnlyList<int> resultTargets) => _resolve().IsMouLidanAppliedTargetFact(use, fact, precedingTargets, out resultTargets);
    internal bool IsMouLidanTargetSummary(CardUseFrame use, ProgramCardUseTargetAddedEvent summary) => _resolve().IsMouLidanTargetSummary(use, summary);
    internal bool IssueMouDanchiReward(ProgramSkillFrame supplied, CardKind kind, int target) => _resolve().IssueMouDanchiReward(supplied, kind, target);
    internal CardActionContext MouChengYuAction(ProgramSkillFrame f) => _resolve().MouChengYuAction(f);
    internal MouDanchiDamage? MouChengYuDamage(int owner, ProgramSkillWindowContext? c) => _resolve().MouChengYuDamage(owner, c);
    internal bool MouChengYuDrawTwoTargets(ProgramTargetCardWindowQuery query) => _resolve().MouChengYuDrawTwoTargets(query);
    internal bool MouChengYuFinalizedTrick(ProgramTargetCardWindowQuery query) => _resolve().MouChengYuFinalizedTrick(query);
    internal bool MouChengYuParentMatches(ProgramSkillFrame f) => _resolve().MouChengYuParentMatches(f);
    internal bool MouDanchiSuspensionMatches(ProgramSkillFrame f) => _resolve().MouDanchiSuspensionMatches(f);
    internal bool ObserveMouChengYuResponse(CardActionContext action) => _resolve().ObserveMouChengYuResponse(action);
    internal void ObserveMouChengYuUse(IGameEvent fact) => _resolve().ObserveMouChengYuUse(fact);
    internal CardActionContext PrepareMouDanchiAction(CardActionContext action) => _resolve().PrepareMouDanchiAction(action);
    internal bool QualifyMouChengYu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().QualifyMouChengYu(candidate, trigger, context);
    internal Card ReadMouDanchiAppearance(long id, CardAppearanceReference appearance) => _resolve().ReadMouDanchiAppearance(id, appearance);
    internal void RestoreMouDanchiAttack(ProgramSkillFrame f) => _resolve().RestoreMouDanchiAttack(f);
    internal bool ReturnMouDanchiAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnMouDanchiAttack(completion);
    internal void ReturnMouDanchiUse(CardUseFrame use) => _resolve().ReturnMouDanchiUse(use);
    internal void SuspendMouDanchiAttack(long frameId) => _resolve().SuspendMouDanchiAttack(frameId);
    internal void ValidateMouDanchiNativeUse(CardUseFrame use) => _resolve().ValidateMouDanchiNativeUse(use);
    internal bool WindowSettlement_IsMouDanchiSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsMouDanchiSettlementBatch(frame, batch);
    internal bool HasMouChengYu { get => _resolve().HasMouChengYu; }
    internal MouDanchiRuntime MouDanchi { get => _resolve().MouDanchi; }
    internal MouLidanRuntime MouLidan { get => _resolve().MouLidan; }
}
