#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardEffectProgramsEntries
{
    internal void BeginActualDelayedJudgment(CharacterState current, Card card, CardKind kind) => _resolve().BeginActualDelayedJudgment(current, card, kind);
    internal bool BeginCardEffectPrograms(CardActionContext action, IReadOnlyList<int> designated, NullificationWindowFrame? ordinary, DelayedEffectContinuation? delayed, NativeTrickTargetEffectReturn? nativeTargetReturn = default, bool perTargetOnly = false) => _resolve().BeginCardEffectPrograms(action, designated, ordinary, delayed, nativeTargetReturn, perTargetOnly);
    internal bool CanRunCardEffectCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanRunCardEffectCandidate(candidate, context);
    internal void CompleteCardEffectCandidate(ProgramSkillWindowContext context) => _resolve().CompleteCardEffectCandidate(context);
    internal void ContinueCardEffectBeforeApply() => _resolve().ContinueCardEffectBeforeApply();
    internal void ContinueDelayedCardEffect(DelayedEffectContinuation delayed) => _resolve().ContinueDelayedCardEffect(delayed);
    internal bool TryBeginDelayedCardEffectPrograms(CharacterState owner, Card card, CardKind kind) => _resolve().TryBeginDelayedCardEffectPrograms(owner, card, kind);
    internal bool TryBeginOrdinaryCardEffectPrograms(NullificationWindowFrame pending) => _resolve().TryBeginOrdinaryCardEffectPrograms(pending);
    internal bool HasCardEffectBeforeApplyPrograms { get => _resolve().HasCardEffectBeforeApplyPrograms; }
}
