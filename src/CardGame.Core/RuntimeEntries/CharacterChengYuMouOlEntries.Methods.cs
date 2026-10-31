#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengYuMouOlEntries
{
    internal void ApplyMouLidanTargets(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.MouLidanSelection selection) => _resolve().ApplyMouLidanTargets(f, selection);
    internal void BindMouDanchiUse(global::CardGame.Core.CardUseDeclaredEvent declared) => _resolve().BindMouDanchiUse(declared);
    internal bool CanMouDanchiSlash(int actor, int target) => _resolve().CanMouDanchiSlash(actor, target);
    internal void CollectMouChengYuIssued(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectMouChengYuIssued(action, window, result);
    internal void ConsumeMouLidanCounterspell(global::CardGame.Core.CardActionContext action) => _resolve().ConsumeMouLidanCounterspell(action);
    internal bool FinishMouDanchiSlash(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouDanchiSlash(attack, out paused);
    internal bool HasMouDanchiSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().HasMouDanchiSuspendedCompletedParent(window);
    internal bool HasMouLidanTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasMouLidanTargetTail(use);
    internal bool IsIssuedMouChengYu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedMouChengYu(c, context);
    internal bool IsMouChengYuTrueUse(global::CardGame.Core.CardActionContext action) => _resolve().IsMouChengYuTrueUse(action);
    internal bool IsMouDanchiAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsMouDanchiAttackConsistent(attack, processing);
    internal bool IsMouDanchiDrawTwo(long id) => _resolve().IsMouDanchiDrawTwo(id);
    internal bool IsMouDanchiSettlementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsMouDanchiSettlementBatch(f, batch);
    internal bool IsMouDanchiUse(long id) => _resolve().IsMouDanchiUse(id);
    internal bool IsMouLidanAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.MouLidanTargetsAppliedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, scoped out global::System.Collections.Generic.IReadOnlyList<int> resultTargets) => _resolve().IsMouLidanAppliedTargetFact(use, fact, precedingTargets, out resultTargets);
    internal bool IsMouLidanTargetSummary(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramCardUseTargetAddedEvent summary) => _resolve().IsMouLidanTargetSummary(use, summary);
    internal bool IssueMouDanchiReward(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.CardKind kind, int target) => _resolve().IssueMouDanchiReward(supplied, kind, target);
    internal global::CardGame.Core.CardActionContext MouChengYuAction(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouChengYuAction(f);
    internal global::CardGame.Core.MouDanchiDamage? MouChengYuDamage(int owner, global::CardGame.Core.ProgramSkillWindowContext? c) => _resolve().MouChengYuDamage(owner, c);
    internal bool MouChengYuDrawTwoTargets(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().MouChengYuDrawTwoTargets(query);
    internal bool MouChengYuFinalizedTrick(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().MouChengYuFinalizedTrick(query);
    internal bool MouChengYuParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouChengYuParentMatches(f);
    internal global::System.Collections.Generic.IReadOnlyList<int>? MouDanchiAncestorProcessing(long id) => _resolve().MouDanchiAncestorProcessing(id);
    internal bool MouDanchiSuspensionMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDanchiSuspensionMatches(f);
    internal bool ObserveMouChengYuResponse(global::CardGame.Core.CardActionContext action) => _resolve().ObserveMouChengYuResponse(action);
    internal void ObserveMouChengYuUse(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveMouChengYuUse(fact);
    internal global::CardGame.Core.CardActionContext PrepareMouDanchiAction(global::CardGame.Core.CardActionContext action) => _resolve().PrepareMouDanchiAction(action);
    internal bool QualifyMouChengYu(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyMouChengYu(candidate, trigger, context);
    internal global::CardGame.Core.Card ReadMouDanchiAppearance(long id, global::CardGame.Core.CardAppearanceReference appearance) => _resolve().ReadMouDanchiAppearance(id, appearance);
    internal void RestoreMouDanchiAttack(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RestoreMouDanchiAttack(f);
    internal bool ReturnMouDanchiAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnMouDanchiAttack(completion);
    internal void ReturnMouDanchiUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouDanchiUse(use);
    internal void SuspendMouDanchiAttack(long frameId) => _resolve().SuspendMouDanchiAttack(frameId);
    internal void ValidateMouDanchiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateMouDanchiNativeUse(use);
    internal bool WindowSettlement_IsMouDanchiSettlementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().WindowSettlement_IsMouDanchiSettlementBatch(frame, batch);
    internal bool HasMouChengYu { get => _resolve().HasMouChengYu; }
    internal global::CardGame.Core.MouDanchiRuntime MouDanchi { get => _resolve().MouDanchi; }
    internal global::CardGame.Core.MouLidanRuntime MouLidan { get => _resolve().MouLidan; }
}
