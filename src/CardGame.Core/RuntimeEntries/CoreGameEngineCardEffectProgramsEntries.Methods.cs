#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardEffectProgramsEntries
{
    internal void AssertCardEffectProgramState() => _resolve().AssertCardEffectProgramState();
    internal void BeginActualDelayedJudgment(global::CardGame.Core.CharacterState current, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => _resolve().BeginActualDelayedJudgment(current, card, kind);
    internal bool BeginCardEffectPrograms(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<int> designated, global::CardGame.Core.NullificationWindowFrame? ordinary, global::CardGame.Core.DelayedEffectContinuation? delayed, global::CardGame.Core.NativeTrickTargetEffectReturn? nativeTargetReturn = default, bool perTargetOnly = false) => _resolve().BeginCardEffectPrograms(action, designated, ordinary, delayed, nativeTargetReturn, perTargetOnly);
    internal bool CanRunCardEffectCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunCardEffectCandidate(candidate, context);
    internal void CompleteCardEffectCandidate(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CompleteCardEffectCandidate(context);
    internal void ContinueCardEffectBeforeApply() => _resolve().ContinueCardEffectBeforeApply();
    internal void ContinueDelayedCardEffect(global::CardGame.Core.DelayedEffectContinuation delayed) => _resolve().ContinueDelayedCardEffect(delayed);
    internal bool TryBeginDelayedCardEffectPrograms(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => _resolve().TryBeginDelayedCardEffectPrograms(owner, card, kind);
    internal bool TryBeginOrdinaryCardEffectPrograms(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().TryBeginOrdinaryCardEffectPrograms(pending);
    internal bool HasCardEffectBeforeApplyPrograms { get => _resolve().HasCardEffectBeforeApplyPrograms; }
}
