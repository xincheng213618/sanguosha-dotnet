#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuanJiongOlEntries
{
    internal bool AdvanceSaoguSubtree() => _resolve().AdvanceSaoguSubtree();
    internal bool AllowsDuanJiongEnding(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AllowsDuanJiongEnding(c, t, context);
    internal bool AllowsDuanJiongSaogu(global::CardGame.Core.CharacterState owner) => _resolve().AllowsDuanJiongSaogu(owner);
    internal void AnswerSaogu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerSaogu(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> AppendDuanJiongSaoguDiscardCards(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> held) => _resolve().AppendDuanJiongSaoguDiscardCards(actor, held);
    internal void AssertSaogu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertSaogu(f);
    internal void AssertSaoguNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertSaoguNativeUse(use);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDuanJiongSaogu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginDuanJiongSaogu(supplied);
    internal void BindDuanJiongSaoguNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindDuanJiongSaoguNativeUse(use);
    internal void CaptureDuanJiongSaoguBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureDuanJiongSaoguBatch(batch);
    internal global::CardGame.Core.PromptChoice ChooseSaoguAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseSaoguAi(p, f);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureDuanJiongSaoguBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureDuanJiongSaoguBatch(batch, completed);
    internal bool ContinuePaidSaogu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidSaogu(f);
    internal void ContinueSaogu(long id) => _resolve().ContinueSaogu(id);
    internal bool DrainSaoguChildren(long id) => _resolve().DrainSaoguChildren(id);
    internal bool DuanJiongSaoguSlashQuotaBeforeLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().DuanJiongSaoguSlashQuotaBeforeLimit(query);
    internal global::CardGame.Core.Card? FindDuanJiongSaoguDiscardCard(global::CardGame.Core.CharacterState actor, int id) => _resolve().FindDuanJiongSaoguDiscardCard(actor, id);
    internal void FinishSaogu(global::CardGame.Core.ProgramSkillFrame f, string reason) => _resolve().FinishSaogu(f, reason);
    internal bool HasDuanJiongBeforeDamageObserver(long id) => _resolve().HasDuanJiongBeforeDamageObserver(id);
    internal bool HasDuanJiongDamageObserver(long id) => _resolve().HasDuanJiongDamageObserver(id);
    internal bool HasIssuedDuanJiongSaoguUnlimitedUse(long id, int actor) => _resolve().HasIssuedDuanJiongSaoguUnlimitedUse(id, actor);
    internal bool IsDuanJiongDying() => _resolve().IsDuanJiongDying();
    internal bool IsDuanJiongRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsDuanJiongRecoveryParent(f, recovery);
    internal bool IsDuanJiongSaoguDiscardQuery(global::CardGame.Core.CharacterState actor, int? id) => _resolve().IsDuanJiongSaoguDiscardQuery(actor, id);
    internal bool IsDuanJiongSaoguSelectedDiscard(int actor, int id) => _resolve().IsDuanJiongSaoguSelectedDiscard(actor, id);
    internal bool IsSaoguPaidDestination(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementRecord movement) => _resolve().IsSaoguPaidDestination(frame, movement);
    internal void IssueSaoguUse(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().IssueSaoguUse(f, choice);
    internal void PaySaoguDiscard(global::CardGame.Core.ProgramSkillFrame f, bool fee) => _resolve().PaySaoguDiscard(f, fee);
    internal void PaySaoguDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PaySaoguDraw(f);
    internal void PrepareSaoguDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrepareSaoguDiscard(f);
    internal void PublishSaogu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSaogu(f);
    internal bool ResumeSaogu(long id) => _resolve().ResumeSaogu(id);
    internal bool ReturnSaoguNativeAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnSaoguNativeAttack(completion);
    internal void ReturnSaoguNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnSaoguNativeUse(use);
    internal global::System.Collections.Generic.Dictionary<string, string> SaoguArgs(string branch) => _resolve().SaoguArgs(branch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SaoguChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SaoguChoices(f);
    internal global::CardGame.Core.ProgramSkillFrame? SaoguDiscardQueryOwner(int actor) => _resolve().SaoguDiscardQueryOwner(actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DuanJiongSaoguCard> SaoguDiscardable(int actor, int forbidden, string skill, string instance) => _resolve().SaoguDiscardable(actor, forbidden, skill, instance);
    internal int SaoguDiscardedSuitMask(int actor, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().SaoguDiscardedSuitMask(actor, phase);
    internal bool SaoguFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SaoguFirstChild(f, child);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> SaoguNativeActions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SaoguNativeActions(f);
    internal global::CardGame.Core.ProgramSkillFrame? SaoguObserverRoot() => _resolve().SaoguObserverRoot();
    internal bool SaoguUnspentDiscard(global::CardGame.Core.ProgramSkillFrame f, int id) => _resolve().SaoguUnspentDiscard(f, id);
    internal void SettleSaoguNativeUse(global::CardGame.Core.DuanJiongSaoguUseOrigin o) => _resolve().SettleSaoguNativeUse(o);
    internal bool ValidSaogu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSaogu(f);
    internal bool HasDuanJiongSaoguCapability { get => _resolve().HasDuanJiongSaoguCapability; }
}
