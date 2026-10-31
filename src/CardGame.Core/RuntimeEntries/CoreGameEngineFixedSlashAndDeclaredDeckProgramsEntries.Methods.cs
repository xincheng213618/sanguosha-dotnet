#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedSlashAndDeclaredDeckProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDeclaredDeckCriterion(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginDeclaredDeckCriterion(frame);
    internal void BeginFixedTargetFactionSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginFixedTargetFactionSlash(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOwnerSlashAgainstTurnOwner(global::CardGame.Core.ProgramSkillFrame frame, bool ignoreDistance) => _resolve().BeginOwnerSlashAgainstTurnOwner(frame, ignoreDistance);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DeclaredDeckCriterionChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DeclaredDeckCriterionChoices(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DeclaredDeckRecipientChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DeclaredDeckRecipientChoices(frame);
    internal void FinishDeclaredDeckCriterion(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishDeclaredDeckCriterion(frame);
    internal global::System.Collections.Generic.Dictionary<string, string> FixedSlashAndDeclaredDeckParameters(global::CardGame.Core.ProgramSkillFrame frame, string action) => _resolve().FixedSlashAndDeclaredDeckParameters(frame, action);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FixedTargetSlashChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FixedTargetSlashChoices(frame);
    internal bool IsFixedTargetFactionSlashLegal(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardKind kind, int? effectiveRank = default, bool allowPotentialRank = true) => _resolve().IsFixedTargetFactionSlashLegal(frame, kind, effectiveRank, allowPotentialRank);
    internal void PublishFixedSlashAndDeclaredDeckPrompt(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string text, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().PublishFixedSlashAndDeclaredDeckPrompt(frame, choices, text, targets);
    internal void ResolveFixedSlashAndDeclaredDeckChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFixedSlashAndDeclaredDeckChoice(selected);
    internal void ResolveFixedTargetSlashChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFixedTargetSlashChoice(frame, selected);
    internal void ResumeDeclaredDeckCriterion(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResumeDeclaredDeckCriterion(frame);
    internal global::CardGame.Core.PromptChoice SelectAiFixedSlashAndDeclaredDeck(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiFixedSlashAndDeclaredDeck(decision, frame);
    internal bool TryResumeFixedSlashAndDeclaredDeck(long frameId) => _resolve().TryResumeFixedSlashAndDeclaredDeck(frameId);
    internal bool ValidateFixedTargetFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidateFixedTargetFactionSlashParent(pending, frame);
}
