#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuJinDingOlEntries
{
    internal bool AdvanceHuJinDingSubtree() => _resolve().AdvanceHuJinDingSubtree();
    internal PromptChoice AiQingyuan(PendingDecision prompt, ProgramSkillFrame f) => _resolve().AiQingyuan(prompt, f);
    internal void AnswerQingyuanPartner(PromptChoice choice) => _resolve().AnswerQingyuanPartner(choice);
    internal SkillProgramStepOutcome BeginQingyuan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginQingyuan(supplied, op);
    internal bool CanOfferHuJinDing(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferHuJinDing(c, t, context);
    internal void CaptureQingyuanGainBatch(CardMovementBatchContext batch) => _resolve().CaptureQingyuanGainBatch(batch);
    internal IEnumerable<ProgramTriggerCandidate> CollectQingyuanGainCandidates(CardMovementBatchContext batch) => _resolve().CollectQingyuanGainCandidates(batch);
    internal void CollectWindow_CollectQingyuanGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectQingyuanGainCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureQingyuanGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureQingyuanGainBatch(batch, completed);
    internal bool ContinuePaidHuJinDing(ProgramSkillFrame f) => _resolve().ContinuePaidHuJinDing(f);
    internal ProgramSkillWindowContext CreateQingyuanGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate c) => _resolve().CreateQingyuanGainContext(parent, c);
    internal bool DrainHuJinDingChildren(long id) => _resolve().DrainHuJinDingChildren(id);
    internal bool ExcludeOrdinaryMovement_IsQingyuanGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsQingyuanGainTrigger(candidate, trigger);
    internal void FinishQingyuan(ProgramSkillFrame f) => _resolve().FinishQingyuan(f);
    internal bool HasZhongshenCardMark(int cardId) => _resolve().HasZhongshenCardMark(cardId);
    internal bool HuJinDingFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HuJinDingFirstChild(f, child);
    internal ProgramSkillFrame? HuJinDingObserverRoot() => _resolve().HuJinDingObserverRoot();
    internal bool IsHuJinDingRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsHuJinDingRecoveryParent(f, recovery);
    internal bool IsLiveQingyuanPartner(QingyuanPartnerSelectedEvent p) => _resolve().IsLiveQingyuanPartner(p);
    internal bool MatchesZhongshenRoundGain(CharacterState owner, Card card, SkillProgramViewAs rule) => _resolve().MatchesZhongshenRoundGain(owner, card, rule);
    internal void ObserveHuJinDingFacts(IGameEvent fact) => _resolve().ObserveHuJinDingFacts(fact);
    internal void PayQingyuanTake(ProgramSkillFrame f) => _resolve().PayQingyuanTake(f);
    internal void ProjectZhongshenCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectZhongshenCardSkillMark(cardId, ref projection);
    internal bool? ProveQingyuanMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveQingyuanMovement(trigger, candidate, context);
    internal void PublishQingyuanPartner(ProgramSkillFrame f) => _resolve().PublishQingyuanPartner(f);
    internal QingyuanGainQualifiedEvent? QingyuanGainQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().QingyuanGainQualification(c, context);
    internal IReadOnlyList<int> QingyuanNewPartnerChoices(int owner, string skill, string instance) => _resolve().QingyuanNewPartnerChoices(owner, skill, instance);
    internal IReadOnlyList<PromptChoice> QingyuanPartnerChoices(ProgramSkillFrame f) => _resolve().QingyuanPartnerChoices(f);
    internal IEnumerable<QingyuanPartnerSelectedEvent> QingyuanPartners(int owner, string skill, string instance) => _resolve().QingyuanPartners(owner, skill, instance);
    internal int[] QingyuanTakeHolders(int owner, string skill, string instance) => _resolve().QingyuanTakeHolders(owner, skill, instance);
    internal bool ResumeQingyuan(long id) => _resolve().ResumeQingyuan(id);
    internal bool ReturnHuJinDingMovement(ProgramSkillFrame f) => _resolve().ReturnHuJinDingMovement(f);
    internal void SynchronizeQingyuanPublicMarkers() => _resolve().SynchronizeQingyuanPublicMarkers();
    internal ProgramSkillWindowContext? TryCreateQingyuanGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateQingyuanGainContext(frame, candidate);
    internal bool HasQingyuanCapability { get => _resolve().HasQingyuanCapability; }
    internal bool HasZhongshenCapability { get => _resolve().HasZhongshenCapability; }
}
