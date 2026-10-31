#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuanJiongOlEntries
{
    internal bool AdvanceSaoguSubtree() => _resolve().AdvanceSaoguSubtree();
    internal bool AllowsDuanJiongEnding(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().AllowsDuanJiongEnding(c, t, context);
    internal bool AllowsDuanJiongSaogu(CharacterState owner) => _resolve().AllowsDuanJiongSaogu(owner);
    internal void AnswerSaogu(PromptChoice choice) => _resolve().AnswerSaogu(choice);
    internal IReadOnlyList<Card> AppendDuanJiongSaoguDiscardCards(CharacterState actor, IReadOnlyList<Card> held) => _resolve().AppendDuanJiongSaoguDiscardCards(actor, held);
    internal void AssertSaoguNativeUse(CardUseFrame use) => _resolve().AssertSaoguNativeUse(use);
    internal SkillProgramStepOutcome BeginDuanJiongSaogu(ProgramSkillFrame supplied) => _resolve().BeginDuanJiongSaogu(supplied);
    internal void BindDuanJiongSaoguNativeUse(CardUseFrame use) => _resolve().BindDuanJiongSaoguNativeUse(use);
    internal void CaptureDuanJiongSaoguBatch(CardMovementBatchContext batch) => _resolve().CaptureDuanJiongSaoguBatch(batch);
    internal PromptChoice ChooseSaoguAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseSaoguAi(p, f);
    internal CardMovementBatchContext CompleteMovementBatchCaptureDuanJiongSaoguBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureDuanJiongSaoguBatch(batch, completed);
    internal bool ContinuePaidSaogu(ProgramSkillFrame f) => _resolve().ContinuePaidSaogu(f);
    internal void ContinueSaogu(long id) => _resolve().ContinueSaogu(id);
    internal bool DrainSaoguChildren(long id) => _resolve().DrainSaoguChildren(id);
    internal bool DuanJiongSaoguSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => _resolve().DuanJiongSaoguSlashQuotaBeforeLimit(query);
    internal Card? FindDuanJiongSaoguDiscardCard(CharacterState actor, int id) => _resolve().FindDuanJiongSaoguDiscardCard(actor, id);
    internal void FinishSaogu(ProgramSkillFrame f, string reason) => _resolve().FinishSaogu(f, reason);
    internal bool HasIssuedDuanJiongSaoguUnlimitedUse(long id, int actor) => _resolve().HasIssuedDuanJiongSaoguUnlimitedUse(id, actor);
    internal bool IsDuanJiongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsDuanJiongRecoveryParent(f, recovery);
    internal bool IsDuanJiongSaoguDiscardQuery(CharacterState actor, int? id) => _resolve().IsDuanJiongSaoguDiscardQuery(actor, id);
    internal bool IsDuanJiongSaoguSelectedDiscard(int actor, int id) => _resolve().IsDuanJiongSaoguSelectedDiscard(actor, id);
    internal bool IsSaoguPaidDestination(ProgramSkillFrame frame, CardMovementRecord movement) => _resolve().IsSaoguPaidDestination(frame, movement);
    internal void IssueSaoguUse(ProgramSkillFrame f, PromptChoice choice) => _resolve().IssueSaoguUse(f, choice);
    internal void PaySaoguDiscard(ProgramSkillFrame f, bool fee) => _resolve().PaySaoguDiscard(f, fee);
    internal void PaySaoguDraw(ProgramSkillFrame f) => _resolve().PaySaoguDraw(f);
    internal void PrepareSaoguDiscard(ProgramSkillFrame f) => _resolve().PrepareSaoguDiscard(f);
    internal void PublishSaogu(ProgramSkillFrame f) => _resolve().PublishSaogu(f);
    internal bool ResumeSaogu(long id) => _resolve().ResumeSaogu(id);
    internal bool ReturnSaoguNativeAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnSaoguNativeAttack(completion);
    internal void ReturnSaoguNativeUse(CardUseFrame use) => _resolve().ReturnSaoguNativeUse(use);
    internal Dictionary<string, string> SaoguArgs(string branch) => _resolve().SaoguArgs(branch);
    internal IReadOnlyList<PromptChoice> SaoguChoices(ProgramSkillFrame f) => _resolve().SaoguChoices(f);
    internal ProgramSkillFrame? SaoguDiscardQueryOwner(int actor) => _resolve().SaoguDiscardQueryOwner(actor);
    internal IReadOnlyList<DuanJiongSaoguCard> SaoguDiscardable(int actor, int forbidden, string skill, string instance) => _resolve().SaoguDiscardable(actor, forbidden, skill, instance);
    internal int SaoguDiscardedSuitMask(int actor, ActualDiscardRecoveryPhaseKey phase) => _resolve().SaoguDiscardedSuitMask(actor, phase);
    internal bool SaoguFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SaoguFirstChild(f, child);
    internal IReadOnlyList<LegalAction> SaoguNativeActions(ProgramSkillFrame f) => _resolve().SaoguNativeActions(f);
    internal ProgramSkillFrame? SaoguObserverRoot() => _resolve().SaoguObserverRoot();
    internal bool SaoguUnspentDiscard(ProgramSkillFrame f, int id) => _resolve().SaoguUnspentDiscard(f, id);
    internal void SettleSaoguNativeUse(DuanJiongSaoguUseOrigin o) => _resolve().SettleSaoguNativeUse(o);
    internal bool ValidSaogu(ProgramSkillFrame f) => _resolve().ValidSaogu(f);
    internal bool HasDuanJiongSaoguCapability { get => _resolve().HasDuanJiongSaoguCapability; }
}
