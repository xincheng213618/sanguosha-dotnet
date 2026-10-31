#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineChainedStateBasicConversionsEntries
{
    internal PendingDecision? AddChainedStateBasicChoices(PendingDecision? prompt) => _resolve().AddChainedStateBasicChoices(prompt);
    internal void BeginChainedStateBasic(CharacterState actor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets, PendingDecision original, GameEngine.ChainedStateBasicNeed need) => _resolve().BeginChainedStateBasic(actor, source, kind, targets, original, need);
    internal void BeginChainedStateBasicChild(long parentId, long childId) => _resolve().BeginChainedStateBasicChild(parentId, childId);
    internal bool CanPayChainedStateBasic(CharacterState actor, bool desired) => _resolve().CanPayChainedStateBasic(actor, desired);
    internal void CancelChainedStateBasic(ChainedStateBasicFrame frame) => _resolve().CancelChainedStateBasic(frame);
    internal GameEngine.ChainedStateBasicNeed? ChainedStateBasicContext(PendingDecision prompt, ResolutionFrame? suppliedTop = default) => _resolve().ChainedStateBasicContext(prompt, suppliedTop);
    internal IReadOnlyList<PromptChoice> ChainedStateBasicDodgeChoices(CharacterState actor) => _resolve().ChainedStateBasicDodgeChoices(actor);
    internal IReadOnlyList<PromptChoice> ChainedStateBasicForcedSlashChoices(CharacterState actor, CharacterState target, long originalId, bool qinglong) => _resolve().ChainedStateBasicForcedSlashChoices(actor, target, originalId, qinglong);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> ChainedStateBasicForcedSlashRules(CharacterState actor, CharacterState target, bool qinglong) => _resolve().ChainedStateBasicForcedSlashRules(actor, target, qinglong);
    internal IReadOnlyList<LegalAction> ChainedStateBasicNativePlayActions(CharacterState actor, CardConversionSource source, CardKind kind) => _resolve().ChainedStateBasicNativePlayActions(actor, source, kind);
    internal bool ChainedStateBasicNativeResponseLegal(CharacterState actor, GameEngine.ChainedStateBasicNeed need, CardKind kind, int? targetSeat) => _resolve().ChainedStateBasicNativeResponseLegal(actor, need, kind, targetSeat);
    internal bool ChainedStateBasicOriginalNeedCurrent(ChainedStateBasicFrame frame) => _resolve().ChainedStateBasicOriginalNeedCurrent(frame);
    internal IReadOnlyList<LegalAction> ChainedStateBasicPlayActions(CharacterState actor) => _resolve().ChainedStateBasicPlayActions(actor);
    internal IReadOnlyList<PromptChoice> ChainedStateBasicResponseChoices(PendingDecision prompt, GameEngine.ChainedStateBasicNeed need) => _resolve().ChainedStateBasicResponseChoices(prompt, need);
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> ChainedStateBasicRules(CharacterState actor, CardKind? output = default, bool dodge = false) => _resolve().ChainedStateBasicRules(actor, output, dodge);
    internal void ContinueChainedStateBasic(long id) => _resolve().ContinueChainedStateBasic(id);
    internal bool HasChainedStateBasicDodge(CharacterState actor) => _resolve().HasChainedStateBasicDodge(actor);
    internal bool HasChainedStateBasicProgramSlash(CharacterState actor, IEnumerable<int> targets) => _resolve().HasChainedStateBasicProgramSlash(actor, targets);
    internal bool IsChainedStateBasicAnswer(ChoiceId id) => _resolve().IsChainedStateBasicAnswer(id);
    internal void ResumeChainedStateBasicState(ProgramLifecycleTriggerWindowFrame child) => _resolve().ResumeChainedStateBasicState(child);
    internal CommandResult SubmitChainedStateBasicAnswer(int actorSeat, PendingDecision prompt, PromptChoice choice) => _resolve().SubmitChainedStateBasicAnswer(actorSeat, prompt, choice);
    internal CommandResult SubmitChainedStateBasicAnswer2(int actorSeat, PromptId promptId, ChoiceId choiceId) => _resolve().SubmitChainedStateBasicAnswer(actorSeat, promptId, choiceId);
    internal bool TryAdvanceChainedStateBasicAi() => _resolve().TryAdvanceChainedStateBasicAi();
    internal bool TryExecuteChainedStateBasicPlay(CharacterState actor, LegalAction action) => _resolve().TryExecuteChainedStateBasicPlay(actor, action);
    internal bool TrySubmitChainedStateBasicPlay(PlayCardCommand command, scoped out CommandResult result) => _resolve().TrySubmitChainedStateBasicPlay(command, out result);
    internal bool ValidChainedStateBasicPayment(ChainedStateBasicPayment p) => _resolve().ValidChainedStateBasicPayment(p);
}
