#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianChouOlEntries
{
    internal bool AdvanceTianChouSubtree() => _resolve().AdvanceTianChouSubtree();
    internal PromptChoice AiTianChou(PendingDecision p, ProgramSkillFrame f) => _resolve().AiTianChou(p, f);
    internal void AnswerTianChou(PromptChoice choice) => _resolve().AnswerTianChou(choice);
    internal SkillProgramStepOutcome BeginTianChou(ProgramSkillFrame supplied) => _resolve().BeginTianChou(supplied);
    internal void BeginTianChouShandaoTrick(ProgramSkillFrame frame, TianChouShandaoUseOrigin origin) => _resolve().BeginTianChouShandaoTrick(frame, origin);
    internal bool CanActivateTianChou(CharacterState owner) => _resolve().CanActivateTianChou(owner);
    internal void CompleteTianChouShandaoTrick(ProgramSkillFrame f, TianChouShandaoUseOrigin origin) => _resolve().CompleteTianChouShandaoTrick(f, origin);
    internal bool ContinuePaidTianChou(ProgramSkillFrame f) => _resolve().ContinuePaidTianChou(f);
    internal void ContinueShandaoAfterPublicReveal(ProgramSkillFrame f) => _resolve().ContinueShandaoAfterPublicReveal(f);
    internal void ContinueTianChou(long id) => _resolve().ContinueTianChou(id);
    internal bool DrainTianChou(long id) => _resolve().DrainTianChou(id);
    internal void FinishTianChou(ProgramSkillFrame f, string reason) => _resolve().FinishTianChou(f, reason);
    internal bool IsTianChouRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsTianChouRecoveryParent(f, recovery);
    internal bool IsTianChouShandaoUse(long id) => _resolve().IsTianChouShandaoUse(id);
    internal bool IsTianChouTopMovement(TianChouShandaoTop t, CardMovementRecord m) => _resolve().IsTianChouTopMovement(t, m);
    internal bool MatchesTianChouShandao(ProgramSkillFrame f, TianChouShandaoUseOrigin o) => _resolve().MatchesTianChouShandao(f, o);
    internal bool MatchesTianChouShandaoUseAction(long id, CardActionContext action, int cardId) => _resolve().MatchesTianChouShandaoUseAction(id, action, cardId);
    internal void PublishTianChou(ProgramSkillFrame f) => _resolve().PublishTianChou(f);
    internal bool ResumeTianChou(long id) => _resolve().ResumeTianChou(id);
    internal void ReturnTianChouShandaoUse(CardUseFrame use) => _resolve().ReturnTianChouShandaoUse(use);
    internal IReadOnlyList<PromptChoice> TianChouChoices(ProgramSkillFrame f) => _resolve().TianChouChoices(f);
    internal bool TianChouFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().TianChouFirstChild(f, child);
    internal bool TianChouHasCards(int seat) => _resolve().TianChouHasCards(seat);
    internal ProgramSkillFrame? TianChouObserverRoot() => _resolve().TianChouObserverRoot();
    internal IReadOnlyList<int> TianChouShandaoTrickTargets(int actor, CardKind kind, IReadOnlyList<int> candidates) => _resolve().TianChouShandaoTrickTargets(actor, kind, candidates);
    internal bool ValidTianChou(ProgramSkillFrame f) => _resolve().ValidTianChou(f);
    internal void ValidateTianChouShandaoUse(CardUseFrame use) => _resolve().ValidateTianChouShandaoUse(use);
}
