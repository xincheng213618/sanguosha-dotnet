#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedDefenseAndPublicDraftEntries
{
    internal SkillProgramStepOutcome BeginLowHandPublicDraft(ProgramSkillFrame frame) => _resolve().BeginLowHandPublicDraft(frame);
    internal SkillProgramStepOutcome BeginNamedTargetDefense(ProgramSkillFrame frame, string ledgerId, IReadOnlyList<CardKind> names) => _resolve().BeginNamedTargetDefense(frame, ledgerId, names);
    internal void ContinuePublicHandDraft(ProgramSkillFrame frame) => _resolve().ContinuePublicHandDraft(frame);
    internal void FinishNamedDefense(ProgramSkillFrame frame, bool nullify) => _resolve().FinishNamedDefense(frame, nullify);
    internal CardUseFrame NamedDefenseUse(ProgramSkillFrame frame) => _resolve().NamedDefenseUse(frame);
    internal int[] PublicHandDraftStartingSeats(ProgramSkillFrame frame) => _resolve().PublicHandDraftStartingSeats(frame);
    internal void PublishNamedDefensePayment(ProgramSkillFrame frame) => _resolve().PublishNamedDefensePayment(frame);
    internal void PublishNamedDefenseTake(ProgramSkillFrame frame) => _resolve().PublishNamedDefenseTake(frame);
    internal void PublishNamedDraftPrompt(ProgramSkillFrame frame, int chooser, string text, IReadOnlyList<PromptChoice> choices, bool privacy) => _resolve().PublishNamedDraftPrompt(frame, chooser, text, choices, privacy);
    internal void PublishPublicDraftSelection(ProgramSkillFrame frame) => _resolve().PublishPublicDraftSelection(frame);
    internal void ResolveNamedDefenseAndDraftChoice(PromptChoice choice) => _resolve().ResolveNamedDefenseAndDraftChoice(choice);
    internal void ReturnPublicHandDraftReveal(ProgramSkillFrame frame) => _resolve().ReturnPublicHandDraftReveal(frame);
    internal bool TryResumeNamedDefenseAndPublicDraft(long frameId) => _resolve().TryResumeNamedDefenseAndPublicDraft(frameId);
}
