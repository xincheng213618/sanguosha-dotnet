#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedDefenseAndPublicDraftEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLowHandPublicDraft(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginLowHandPublicDraft(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginNamedTargetDefense(global::CardGame.Core.ProgramSkillFrame frame, string ledgerId, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> names) => _resolve().BeginNamedTargetDefense(frame, ledgerId, names);
    internal void ContinuePublicHandDraft(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinuePublicHandDraft(frame);
    internal void FinishNamedDefense(global::CardGame.Core.ProgramSkillFrame frame, bool nullify) => _resolve().FinishNamedDefense(frame, nullify);
    internal global::CardGame.Core.CardUseFrame NamedDefenseUse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().NamedDefenseUse(frame);
    internal int[] PublicHandDraftStartingSeats(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublicHandDraftStartingSeats(frame);
    internal void PublishNamedDefensePayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishNamedDefensePayment(frame);
    internal void PublishNamedDefenseTake(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishNamedDefenseTake(frame);
    internal void PublishNamedDraftPrompt(global::CardGame.Core.ProgramSkillFrame frame, int chooser, string text, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, bool privacy) => _resolve().PublishNamedDraftPrompt(frame, chooser, text, choices, privacy);
    internal void PublishPublicDraftSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishPublicDraftSelection(frame);
    internal void ResolveNamedDefenseAndDraftChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveNamedDefenseAndDraftChoice(choice);
    internal void ReturnPublicHandDraftReveal(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnPublicHandDraftReveal(frame);
    internal bool TryResumeNamedDefenseAndPublicDraft(long frameId) => _resolve().TryResumeNamedDefenseAndPublicDraft(frameId);
}
