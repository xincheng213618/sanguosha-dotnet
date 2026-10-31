#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuJinDingOlEntries
{
    internal bool AdvanceHuJinDingSubtree() => _resolve().AdvanceHuJinDingSubtree();
    internal global::CardGame.Core.PromptChoice AiQingyuan(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiQingyuan(prompt, f);
    internal void AnswerQingyuanPartner(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerQingyuanPartner(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginQingyuan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginQingyuan(supplied, op);
    internal bool CanOfferHuJinDing(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferHuJinDing(c, t, context);
    internal void CaptureQingyuanGainBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureQingyuanGainBatch(batch);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectQingyuanGainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectQingyuanGainCandidates(batch);
    internal void CollectWindow_CollectQingyuanGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectQingyuanGainCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureQingyuanGainBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureQingyuanGainBatch(batch, completed);
    internal bool ContinuePaidHuJinDing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidHuJinDing(f);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateQingyuanGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame parent, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateQingyuanGainContext(parent, c);
    internal bool DrainHuJinDingChildren(long id) => _resolve().DrainHuJinDingChildren(id);
    internal bool ExcludeOrdinaryMovement_IsQingyuanGainTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsQingyuanGainTrigger(candidate, trigger);
    internal void FinishQingyuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishQingyuan(f);
    internal bool HasHuJinDingBeforeDamageObserver(long id) => _resolve().HasHuJinDingBeforeDamageObserver(id);
    internal bool HasHuJinDingDamageObserver(long id) => _resolve().HasHuJinDingDamageObserver(id);
    internal bool HasZhongshenCardMark(int cardId) => _resolve().HasZhongshenCardMark(cardId);
    internal bool HuJinDingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HuJinDingFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? HuJinDingObserverRoot() => _resolve().HuJinDingObserverRoot();
    internal bool IsHuJinDingMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsHuJinDingMovement(f, effect, pending);
    internal bool IsHuJinDingRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsHuJinDingRecoveryParent(f, recovery);
    internal bool IsLiveQingyuanPartner(global::CardGame.Core.QingyuanPartnerSelectedEvent p) => _resolve().IsLiveQingyuanPartner(p);
    internal bool MatchesZhongshenRoundGain(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.SkillProgramViewAs rule) => _resolve().MatchesZhongshenRoundGain(owner, card, rule);
    internal void ObserveHuJinDingFacts(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveHuJinDingFacts(fact);
    internal void PayQingyuanTake(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayQingyuanTake(f);
    internal void ProjectZhongshenCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectZhongshenCardSkillMark(cardId, ref projection);
    internal bool? ProveQingyuanMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveQingyuanMovement(trigger, candidate, context);
    internal void PublishQingyuanPartner(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishQingyuanPartner(f);
    internal global::CardGame.Core.QingyuanGainQualifiedEvent? QingyuanGainQualification(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QingyuanGainQualification(c, context);
    internal global::System.Collections.Generic.IReadOnlyList<int> QingyuanNewPartnerChoices(int owner, string skill, string instance) => _resolve().QingyuanNewPartnerChoices(owner, skill, instance);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QingyuanPartnerChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QingyuanPartnerChoices(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.QingyuanPartnerSelectedEvent> QingyuanPartners(int owner, string skill, string instance) => _resolve().QingyuanPartners(owner, skill, instance);
    internal int[] QingyuanTakeHolders(int owner, string skill, string instance) => _resolve().QingyuanTakeHolders(owner, skill, instance);
    internal bool ResumeQingyuan(long id) => _resolve().ResumeQingyuan(id);
    internal bool ReturnHuJinDingMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHuJinDingMovement(f);
    internal void SynchronizeQingyuanPublicMarkers() => _resolve().SynchronizeQingyuanPublicMarkers();
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateQingyuanGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateQingyuanGainContext(frame, candidate);
    internal bool HasQingyuanCapability { get => _resolve().HasQingyuanCapability; }
    internal bool HasZhongshenCapability { get => _resolve().HasZhongshenCapability; }
}
