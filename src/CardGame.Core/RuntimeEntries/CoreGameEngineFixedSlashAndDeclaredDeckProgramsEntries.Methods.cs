#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedSlashAndDeclaredDeckProgramsEntries
{
    internal SkillProgramStepOutcome BeginDeclaredDeckCriterion(ProgramSkillFrame frame) => _resolve().BeginDeclaredDeckCriterion(frame);
    internal void BeginFixedTargetFactionSlash(ProgramSkillFrame frame) => _resolve().BeginFixedTargetFactionSlash(frame);
    internal SkillProgramStepOutcome BeginOwnerSlashAgainstTurnOwner(ProgramSkillFrame frame, bool ignoreDistance) => _resolve().BeginOwnerSlashAgainstTurnOwner(frame, ignoreDistance);
    internal IReadOnlyList<PromptChoice> DeclaredDeckCriterionChoices(ProgramSkillFrame frame) => _resolve().DeclaredDeckCriterionChoices(frame);
    internal IReadOnlyList<PromptChoice> DeclaredDeckRecipientChoices(ProgramSkillFrame frame) => _resolve().DeclaredDeckRecipientChoices(frame);
    internal void FinishDeclaredDeckCriterion(ProgramSkillFrame frame) => _resolve().FinishDeclaredDeckCriterion(frame);
    internal Dictionary<string, string> FixedSlashAndDeclaredDeckParameters(ProgramSkillFrame frame, string action) => _resolve().FixedSlashAndDeclaredDeckParameters(frame, action);
    internal IReadOnlyList<PromptChoice> FixedTargetSlashChoices(ProgramSkillFrame frame) => _resolve().FixedTargetSlashChoices(frame);
    internal bool IsFixedTargetFactionSlashLegal(ProgramSkillFrame frame, CardKind kind, int? effectiveRank = default, bool allowPotentialRank = true) => _resolve().IsFixedTargetFactionSlashLegal(frame, kind, effectiveRank, allowPotentialRank);
    internal void PublishFixedSlashAndDeclaredDeckPrompt(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string text, IReadOnlyList<int> targets) => _resolve().PublishFixedSlashAndDeclaredDeckPrompt(frame, choices, text, targets);
    internal void ResolveFixedSlashAndDeclaredDeckChoice(PromptChoice selected) => _resolve().ResolveFixedSlashAndDeclaredDeckChoice(selected);
    internal void ResolveFixedTargetSlashChoice(ProgramSkillFrame frame, PromptChoice selected) => _resolve().ResolveFixedTargetSlashChoice(frame, selected);
    internal void ResumeDeclaredDeckCriterion(ProgramSkillFrame frame) => _resolve().ResumeDeclaredDeckCriterion(frame);
    internal PromptChoice SelectAiFixedSlashAndDeclaredDeck(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiFixedSlashAndDeclaredDeck(decision, frame);
    internal bool TryResumeFixedSlashAndDeclaredDeck(long frameId) => _resolve().TryResumeFixedSlashAndDeclaredDeck(frameId);
    internal bool ValidateFixedTargetFactionSlashParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame frame) => _resolve().ValidateFixedTargetFactionSlashParent(pending, frame);
}
