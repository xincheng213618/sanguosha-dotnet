#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianChouOlEntries
{
    internal bool AdvanceTianChouSubtree() => _resolve().AdvanceTianChouSubtree();
    internal global::CardGame.Core.PromptChoice AiTianChou(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiTianChou(p, f);
    internal void AnswerTianChou(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerTianChou(choice);
    internal void AssertTianChou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertTianChou(f);
    internal void AssertTianChouNativeUses() => _resolve().AssertTianChouNativeUses();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginTianChou(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginTianChou(supplied);
    internal void BeginTianChouShandaoTrick(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.TianChouShandaoUseOrigin origin) => _resolve().BeginTianChouShandaoTrick(frame, origin);
    internal bool CanActivateTianChou(global::CardGame.Core.CharacterState owner) => _resolve().CanActivateTianChou(owner);
    internal void CompleteTianChouShandaoTrick(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.TianChouShandaoUseOrigin origin) => _resolve().CompleteTianChouShandaoTrick(f, origin);
    internal bool ContinuePaidTianChou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidTianChou(f);
    internal void ContinueShandaoAfterPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShandaoAfterPublicReveal(f);
    internal void ContinueTianChou(long id) => _resolve().ContinueTianChou(id);
    internal bool DrainTianChou(long id) => _resolve().DrainTianChou(id);
    internal void FinishTianChou(global::CardGame.Core.ProgramSkillFrame f, string reason) => _resolve().FinishTianChou(f, reason);
    internal bool HasTianChouBeforeDamageObserver(long id) => _resolve().HasTianChouBeforeDamageObserver(id);
    internal bool HasTianChouDamageObserver(long id) => _resolve().HasTianChouDamageObserver(id);
    internal bool IsTianChouDying() => _resolve().IsTianChouDying();
    internal bool IsTianChouRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsTianChouRecoveryParent(f, recovery);
    internal bool IsTianChouShandaoAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsTianChouShandaoAttackConsistent(attack, processing);
    internal bool IsTianChouShandaoUse(long id) => _resolve().IsTianChouShandaoUse(id);
    internal bool IsTianChouTopMovement(global::CardGame.Core.TianChouShandaoTop t, global::CardGame.Core.CardMovementRecord m) => _resolve().IsTianChouTopMovement(t, m);
    internal bool MatchesTianChouShandao(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.TianChouShandaoUseOrigin o) => _resolve().MatchesTianChouShandao(f, o);
    internal bool MatchesTianChouShandaoUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => _resolve().MatchesTianChouShandaoUseAction(id, action, cardId);
    internal void PublishTianChou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishTianChou(f);
    internal bool ResumeTianChou(long id) => _resolve().ResumeTianChou(id);
    internal void ReturnTianChouShandaoUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnTianChouShandaoUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TianChouChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TianChouChoices(f);
    internal bool TianChouFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().TianChouFirstChild(f, child);
    internal bool TianChouHasCards(int seat) => _resolve().TianChouHasCards(seat);
    internal global::CardGame.Core.ProgramSkillFrame? TianChouObserverRoot() => _resolve().TianChouObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<int> TianChouShandaoTrickTargets(int actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> candidates) => _resolve().TianChouShandaoTrickTargets(actor, kind, candidates);
    internal bool ValidTianChou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidTianChou(f);
    internal void ValidateTianChouShandaoUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateTianChouShandaoUse(use);
}
