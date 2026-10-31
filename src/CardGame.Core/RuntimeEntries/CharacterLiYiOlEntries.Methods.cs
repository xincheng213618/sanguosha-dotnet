#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiYiOlEntries
{
    internal void AddLiYiAttackRangeBaseTerms(ProgramAttackRangeQuery query, List<RuleQueryBaseTerm> terms) => _resolve().AddLiYiAttackRangeBaseTerms(query, terms);
    internal bool AdvanceLiYiChildren() => _resolve().AdvanceLiYiChildren();
    internal void AnswerChanshuang(PromptChoice selected) => _resolve().AnswerChanshuang(selected);
    internal SkillProgramStepOutcome BeginChanshuang(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginChanshuang(supplied, op);
    internal void BeginChanshuangFactionRequest(ProgramSkillFrame f, int actor, int target) => _resolve().BeginChanshuangFactionRequest(f, actor, target);
    internal void BindChanshuangSlashUse(CardUseFrame supplied) => _resolve().BindChanshuangSlashUse(supplied);
    internal bool CanOfferChanshuangEnding(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferChanshuangEnding(c, t, context);
    internal IGameEvent[] ChanshuangActualTurnHistory() => _resolve().ChanshuangActualTurnHistory();
    internal IReadOnlyList<PromptChoice> ChanshuangChoices(ProgramSkillFrame f) => _resolve().ChanshuangChoices(f);
    internal int ChanshuangChooser(ProgramSkillFrame f) => _resolve().ChanshuangChooser(f);
    internal bool ChanshuangSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => _resolve().ChanshuangSlashQuotaBeforeLimit(query);
    internal CardConversionSource ChanshuangSource(ProgramSkillFrame f) => _resolve().ChanshuangSource(f);
    internal PromptChoice ChooseChanshuangActiveAi(PendingDecision p) => _resolve().ChooseChanshuangActiveAi(p);
    internal PromptChoice ChooseChanshuangAi(PendingDecision p) => _resolve().ChooseChanshuangAi(p);
    internal PromptChoice ChooseChanshuangEndingAi(PendingDecision p) => _resolve().ChooseChanshuangEndingAi(p);
    internal void CompleteChanshuangItem(ProgramSkillFrame f, bool actual) => _resolve().CompleteChanshuangItem(f, actual);
    internal void ContinueChanshuang(long id) => _resolve().ContinueChanshuang(id);
    internal bool ContinueIssuedLiYi(ProgramSkillFrame f) => _resolve().ContinueIssuedLiYi(f);
    internal int CurrentChanshuangMask(int actor) => _resolve().CurrentChanshuangMask(actor);
    internal bool DrainLiYi(ProgramSkillFrame f) => _resolve().DrainLiYi(f);
    internal void FinishChanshuang(ProgramSkillFrame f) => _resolve().FinishChanshuang(f);
    internal bool FinishChanshuangSuppliedSlashFailure(ProgramSkillFrame f) => _resolve().FinishChanshuangSuppliedSlashFailure(f);
    internal bool HasIssuedChanshuangSlash(long? useId, int actor) => _resolve().HasIssuedChanshuangSlash(useId, actor);
    internal bool HasIssuedChanshuangSlashDebitExemption(long useId, int actorSeat) => _resolve().HasIssuedChanshuangSlashDebitExemption(useId, actorSeat);
    internal bool HasLiYiVirtualStoneAxe(CharacterState player, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().HasLiYiVirtualStoneAxe(player, excludedEquipmentId, excludedEquipmentSlot);
    internal bool IsChanshuangFactionOrigin(ProgramSkillFrame f, int actor, int target, string resultBind) => _resolve().IsChanshuangFactionOrigin(f, actor, target, resultBind);
    internal bool IsChanshuangNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().IsChanshuangNativeSettlementBatch(f, batch);
    internal bool IsChanshuangRemovedMaterial(long useId, int cardId) => _resolve().IsChanshuangRemovedMaterial(useId, cardId);
    internal bool IsChanshuangSlashQuery(CharacterState actor) => _resolve().IsChanshuangSlashQuery(actor);
    internal bool IsChanshuangSlashRequest(ProgramSkillFrame f) => _resolve().IsChanshuangSlashRequest(f);
    internal bool IsIssuedChanshuangUse(CardUseFrame use) => _resolve().IsIssuedChanshuangUse(use);
    internal bool IsLiYiPaidMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsLiYiPaidMovement(f, m);
    internal bool IsLiYiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsLiYiRecoveryParent(f, recovery);
    internal bool LiYiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LiYiFirstChild(f, child);
    internal ProgramSkillFrame? LiYiObserverRoot() => _resolve().LiYiObserverRoot();
    internal void PayChanshuangCards(ProgramSkillFrame f) => _resolve().PayChanshuangCards(f);
    internal void PrepareChanshuangSuppliedSlash(ProgramSkillFrame supplied, int actorSeat, int providerSeat, int targetSeat, CardKind kind, IReadOnlyList<Card> materials, CardConversionSource? conversionSource, IReadOnlyList<CardConversionSource>? additionalConversionSources = default) => _resolve().PrepareChanshuangSuppliedSlash(supplied, actorSeat, providerSeat, targetSeat, kind, materials, conversionSource, additionalConversionSources);
    internal void PublishChanshuang(ProgramSkillFrame supplied) => _resolve().PublishChanshuang(supplied);
    internal void RecordChanshuangSlashPayment(CardMovedEvent e) => _resolve().RecordChanshuangSlashPayment(e);
    internal bool ResumeChanshuangSuppliedSlash(ProgramSkillFrame supplied) => _resolve().ResumeChanshuangSuppliedSlash(supplied);
    internal bool ResumeLiYi(long id) => _resolve().ResumeLiYi(id);
    internal bool ReturnChanshuangAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnChanshuangAttack(completion);
    internal void ReturnChanshuangNative(CardUseFrame use) => _resolve().ReturnChanshuangNative(use);
    internal bool ReturnLiYiMovement(ProgramSkillFrame f) => _resolve().ReturnLiYiMovement(f);
    internal void SettleChanshuangNative(ChanshuangNativeOrigin o) => _resolve().SettleChanshuangNative(o);
    internal bool TryGetChanshuangSlashRequest(ProgramSkillFrame f, scoped out int actorSeat, scoped out IReadOnlyList<int> targets) => _resolve().TryGetChanshuangSlashRequest(f, out actorSeat, out targets);
    internal bool ValidateChanshuangFactionSlashParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f) => _resolve().ValidateChanshuangFactionSlashParent(pending, f);
    internal void ValidateChanshuangNative(CardUseFrame use) => _resolve().ValidateChanshuangNative(use);
    internal bool WindowSettlement_IsChanshuangNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsChanshuangNativeSettlementBatch(frame, batch);
    internal bool HasLiYiCapability { get => _resolve().HasLiYiCapability; }
}
