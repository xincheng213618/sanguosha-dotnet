#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYaDanOlEntries
{
    internal bool AdvanceYaDanObserver() => _resolve().AdvanceYaDanObserver();
    internal void AnswerQingyaNativeUse(ProgramSkillFrame f, PromptChoice choice) => _resolve().AnswerQingyaNativeUse(f, choice);
    internal void AnswerYaDan(PromptChoice choice) => _resolve().AnswerYaDan(choice);
    internal IReadOnlyList<Card> AppendQingyaDiscardCards(CharacterState actor, IReadOnlyList<Card> held) => _resolve().AppendQingyaDiscardCards(actor, held);
    internal SkillProgramStepOutcome BeginYaDan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginYaDan(supplied, op);
    internal void BindQingyaNativeUse(CardUseFrame use) => _resolve().BindQingyaNativeUse(use);
    internal PromptChoice ChooseYaDanAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseYaDanAi(p, f);
    internal IReadOnlyList<ActualPhaseEndItem> CollectQingyaPhaseEndCandidates(ActualDiscardRecoveryPhaseKey phase) => _resolve().CollectQingyaPhaseEndCandidates(phase);
    internal bool ContinueIssuedQingya(ProgramSkillFrame f) => _resolve().ContinueIssuedQingya(f);
    internal bool ContinuePaidYaDan(ProgramSkillFrame f) => _resolve().ContinuePaidYaDan(f);
    internal void ContinueYaDan(long id) => _resolve().ContinueYaDan(id);
    internal void ExpireQingyaActualTurnAlcohol(IGameEvent payload) => _resolve().ExpireQingyaActualTurnAlcohol(payload);
    internal Card? FindQingyaDiscardCard(CharacterState actor, int id) => _resolve().FindQingyaDiscardCard(actor, id);
    internal void FinishYaDan(ProgramSkillFrame f) => _resolve().FinishYaDan(f);
    internal bool IsIssuedQingyaCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedQingyaCandidate(c, context);
    internal bool IsIssuedQingyaUse(long id) => _resolve().IsIssuedQingyaUse(id);
    internal bool IsQingyaDiscardQuery(CharacterState actor, int? id) => _resolve().IsQingyaDiscardQuery(actor, id);
    internal bool IsQingyaSelectedDiscard(int actor, int id) => _resolve().IsQingyaSelectedDiscard(actor, id);
    internal void NextQingyaHand(ProgramSkillFrame f) => _resolve().NextQingyaHand(f);
    internal void PublishYaDan(ProgramSkillFrame f) => _resolve().PublishYaDan(f);
    internal IReadOnlyList<QingyaArc> QingyaArcs(int owner, int target, IReadOnlyList<int> living) => _resolve().QingyaArcs(owner, target, living);
    internal ProgramSkillFrame? QingyaDiscardQueryOwner(int actor) => _resolve().QingyaDiscardQueryOwner(actor);
    internal bool QingyaEntityStillAvailable(QingyaDiscardEntity entity) => _resolve().QingyaEntityStillAvailable(entity);
    internal bool QingyaGrantDue(QingyaGrantIssuedEvent grant, ActualDiscardRecoveryPhaseKey phase) => _resolve().QingyaGrantDue(grant, phase);
    internal IReadOnlyList<QingyaDiscardEntity> QingyaGrantedEntities(QingyaGrantIssuedEvent grant) => _resolve().QingyaGrantedEntities(grant);
    internal IReadOnlyList<QingyaHandMaterial> QingyaHandMaterials(int owner, int target) => _resolve().QingyaHandMaterials(owner, target);
    internal IReadOnlyList<LegalAction> QingyaNativeActions(ProgramSkillFrame f) => _resolve().QingyaNativeActions(f);
    internal CardUseFrame? QingyaQualifiedUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().QingyaQualifiedUse(c, context);
    internal bool QingyaUnspentEntity(ProgramSkillFrame f, int id) => _resolve().QingyaUnspentEntity(f, id);
    internal bool QualifyQingya(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyQingya(c, t, context);
    internal bool ResumeYaDan(long id) => _resolve().ResumeYaDan(id);
    internal bool ReturnQingyaAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnQingyaAttack(completion);
    internal void ReturnQingyaUse(CardUseFrame use) => _resolve().ReturnQingyaUse(use);
    internal bool ReturnYaDanMovement(ProgramSkillFrame f) => _resolve().ReturnYaDanMovement(f);
    internal void SettleQingyaNative(QingyaNativeOrigin origin) => _resolve().SettleQingyaNative(origin);
    internal bool ValidQingyaGrant(QingyaGrantIssuedEvent g) => _resolve().ValidQingyaGrant(g);
    internal void ValidateQingyaUse(CardUseFrame use) => _resolve().ValidateQingyaUse(use);
    internal IReadOnlyList<PromptChoice> YaDanChoices(ProgramSkillFrame f) => _resolve().YaDanChoices(f);
    internal bool YaDanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().YaDanFirstChild(f, child);
    internal ProgramSkillFrame? YaDanObserverRoot() => _resolve().YaDanObserverRoot();
    internal bool HasYaDanCapability { get => _resolve().HasYaDanCapability; }
}
