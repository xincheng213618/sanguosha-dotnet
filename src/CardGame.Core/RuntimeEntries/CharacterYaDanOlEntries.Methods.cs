#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYaDanOlEntries
{
    internal bool AdvanceYaDanObserver() => _resolve().AdvanceYaDanObserver();
    internal void AnswerQingyaNativeUse(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().AnswerQingyaNativeUse(f, choice);
    internal void AnswerYaDan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerYaDan(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> AppendQingyaDiscardCards(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> held) => _resolve().AppendQingyaDiscardCards(actor, held);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYaDan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginYaDan(supplied, op);
    internal void BindQingyaNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindQingyaNativeUse(use);
    internal global::CardGame.Core.PromptChoice ChooseYaDanAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseYaDanAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectQingyaPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().CollectQingyaPhaseEndCandidates(phase);
    internal bool ContinueIssuedQingya(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedQingya(f);
    internal bool ContinuePaidYaDan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidYaDan(f);
    internal void ContinueYaDan(long id) => _resolve().ContinueYaDan(id);
    internal void ExpireQingyaActualTurnAlcohol(global::CardGame.Core.IGameEvent payload) => _resolve().ExpireQingyaActualTurnAlcohol(payload);
    internal global::CardGame.Core.Card? FindQingyaDiscardCard(global::CardGame.Core.CharacterState actor, int id) => _resolve().FindQingyaDiscardCard(actor, id);
    internal void FinishYaDan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishYaDan(f);
    internal bool HasYaDanBeforeDamageObserver(long id) => _resolve().HasYaDanBeforeDamageObserver(id);
    internal bool HasYaDanDamageObserver(long id) => _resolve().HasYaDanDamageObserver(id);
    internal bool IsIssuedQingyaCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedQingyaCandidate(c, context);
    internal bool IsIssuedQingyaUse(long id) => _resolve().IsIssuedQingyaUse(id);
    internal bool IsQingyaDiscardQuery(global::CardGame.Core.CharacterState actor, int? id) => _resolve().IsQingyaDiscardQuery(actor, id);
    internal bool IsQingyaSelectedDiscard(int actor, int id) => _resolve().IsQingyaSelectedDiscard(actor, id);
    internal bool IsYaDanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsYaDanMovement(f, effect, pending);
    internal void NextQingyaHand(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NextQingyaHand(f);
    internal void PublishYaDan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishYaDan(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.QingyaArc> QingyaArcs(int owner, int target, global::System.Collections.Generic.IReadOnlyList<int> living) => _resolve().QingyaArcs(owner, target, living);
    internal global::CardGame.Core.ProgramSkillFrame? QingyaDiscardQueryOwner(int actor) => _resolve().QingyaDiscardQueryOwner(actor);
    internal bool QingyaEntityStillAvailable(global::CardGame.Core.QingyaDiscardEntity entity) => _resolve().QingyaEntityStillAvailable(entity);
    internal bool QingyaGrantDue(global::CardGame.Core.QingyaGrantIssuedEvent grant, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().QingyaGrantDue(grant, phase);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.QingyaDiscardEntity> QingyaGrantedEntities(global::CardGame.Core.QingyaGrantIssuedEvent grant) => _resolve().QingyaGrantedEntities(grant);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.QingyaHandMaterial> QingyaHandMaterials(int owner, int target) => _resolve().QingyaHandMaterials(owner, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> QingyaNativeActions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QingyaNativeActions(f);
    internal global::CardGame.Core.CardUseFrame? QingyaQualifiedUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QingyaQualifiedUse(c, context);
    internal bool QingyaUnspentEntity(global::CardGame.Core.ProgramSkillFrame f, int id) => _resolve().QingyaUnspentEntity(f, id);
    internal bool QualifyQingya(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyQingya(c, t, context);
    internal bool ResumeYaDan(long id) => _resolve().ResumeYaDan(id);
    internal bool ReturnQingyaAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnQingyaAttack(completion);
    internal void ReturnQingyaUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnQingyaUse(use);
    internal bool ReturnYaDanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnYaDanMovement(f);
    internal void SettleQingyaNative(global::CardGame.Core.QingyaNativeOrigin origin) => _resolve().SettleQingyaNative(origin);
    internal bool ValidQingyaGrant(global::CardGame.Core.QingyaGrantIssuedEvent g) => _resolve().ValidQingyaGrant(g);
    internal void ValidateQingyaUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateQingyaUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> YaDanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YaDanChoices(f);
    internal bool YaDanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().YaDanFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? YaDanObserverRoot() => _resolve().YaDanObserverRoot();
    internal bool HasYaDanCapability { get => _resolve().HasYaDanCapability; }
}
