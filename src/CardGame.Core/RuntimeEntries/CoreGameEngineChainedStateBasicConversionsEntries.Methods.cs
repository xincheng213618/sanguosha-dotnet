#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineChainedStateBasicConversionsEntries
{
    internal global::CardGame.Core.PendingDecision? AddChainedStateBasicChoices(global::CardGame.Core.PendingDecision? prompt) => _resolve().AddChainedStateBasicChoices(prompt);
    internal void BeginChainedStateBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.ChainedStateBasicNeed need) => _resolve().BeginChainedStateBasic(actor, source, kind, targets, original, need);
    internal void BeginChainedStateBasicChild(long parentId, long childId) => _resolve().BeginChainedStateBasicChild(parentId, childId);
    internal bool CanPayChainedStateBasic(global::CardGame.Core.CharacterState actor, bool desired) => _resolve().CanPayChainedStateBasic(actor, desired);
    internal void CancelChainedStateBasic(global::CardGame.Core.ChainedStateBasicFrame frame) => _resolve().CancelChainedStateBasic(frame);
    internal global::CardGame.Core.GameEngine.ChainedStateBasicNeed? ChainedStateBasicContext(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ResolutionFrame? suppliedTop = default) => _resolve().ChainedStateBasicContext(prompt, suppliedTop);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChainedStateBasicDodgeChoices(global::CardGame.Core.CharacterState actor) => _resolve().ChainedStateBasicDodgeChoices(actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChainedStateBasicForcedSlashChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, long originalId, bool qinglong) => _resolve().ChainedStateBasicForcedSlashChoices(actor, target, originalId, qinglong);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> ChainedStateBasicForcedSlashRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, bool qinglong) => _resolve().ChainedStateBasicForcedSlashRules(actor, target, qinglong);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> ChainedStateBasicNativePlayActions(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => _resolve().ChainedStateBasicNativePlayActions(actor, source, kind);
    internal bool ChainedStateBasicNativeResponseLegal(global::CardGame.Core.CharacterState actor, global::CardGame.Core.GameEngine.ChainedStateBasicNeed need, global::CardGame.Core.CardKind kind, int? targetSeat) => _resolve().ChainedStateBasicNativeResponseLegal(actor, need, kind, targetSeat);
    internal bool ChainedStateBasicOriginalNeedCurrent(global::CardGame.Core.ChainedStateBasicFrame frame) => _resolve().ChainedStateBasicOriginalNeedCurrent(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> ChainedStateBasicPlayActions(global::CardGame.Core.CharacterState actor) => _resolve().ChainedStateBasicPlayActions(actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChainedStateBasicResponseChoices(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.GameEngine.ChainedStateBasicNeed need) => _resolve().ChainedStateBasicResponseChoices(prompt, need);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> ChainedStateBasicRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind? output = default, bool dodge = false) => _resolve().ChainedStateBasicRules(actor, output, dodge);
    internal void ContinueChainedStateBasic(long id) => _resolve().ContinueChainedStateBasic(id);
    internal bool HasChainedStateBasicDodge(global::CardGame.Core.CharacterState actor) => _resolve().HasChainedStateBasicDodge(actor);
    internal bool HasChainedStateBasicProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => _resolve().HasChainedStateBasicProgramSlash(actor, targets);
    internal bool IsChainedStateBasicAnswer(global::CardGame.Core.ChoiceId id) => _resolve().IsChainedStateBasicAnswer(id);
    internal void ResumeChainedStateBasicState(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame child) => _resolve().ResumeChainedStateBasicState(child);
    internal global::CardGame.Core.CommandResult SubmitChainedStateBasicAnswer(int actorSeat, global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.PromptChoice choice) => _resolve().SubmitChainedStateBasicAnswer(actorSeat, prompt, choice);
    internal global::CardGame.Core.CommandResult SubmitChainedStateBasicAnswer2(int actorSeat, global::CardGame.Core.PromptId promptId, global::CardGame.Core.ChoiceId choiceId) => _resolve().SubmitChainedStateBasicAnswer(actorSeat, promptId, choiceId);
    internal bool TryAdvanceChainedStateBasicAi() => _resolve().TryAdvanceChainedStateBasicAi();
    internal bool TryExecuteChainedStateBasicPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().TryExecuteChainedStateBasicPlay(actor, action);
    internal bool TrySubmitChainedStateBasicPlay(global::CardGame.Core.PlayCardCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().TrySubmitChainedStateBasicPlay(command, out result);
    internal bool ValidChainedStateBasicPayment(global::CardGame.Core.ChainedStateBasicPayment p) => _resolve().ValidChainedStateBasicPayment(p);
}
