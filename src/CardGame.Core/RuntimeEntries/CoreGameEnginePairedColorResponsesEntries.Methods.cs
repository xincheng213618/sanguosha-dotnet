#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedColorResponsesEntries
{
    internal PairedColorResponseLinkedEvent? BuildPairedColorResponseLink(ProgramCardTriggerWindowFrame window) => _resolve().BuildPairedColorResponseLink(window);
    internal bool CanContinuePairedColorDisposition(ProgramSkillFrame f) => _resolve().CanContinuePairedColorDisposition(f);
    internal bool CanOfferPairedColorDisposition(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferPairedColorDisposition(candidate, trigger, context);
    internal void CapturePairedColorResponseLink(ProgramCardTriggerWindowFrame window) => _resolve().CapturePairedColorResponseLink(window);
    internal void CompletePairedColorDisposition(ProgramSkillFrame f, bool paid) => _resolve().CompletePairedColorDisposition(f, paid);
    internal bool DrainPairedColorDispositionChildren(ProgramSkillFrame f) => _resolve().DrainPairedColorDispositionChildren(f);
    internal bool ExactPairedColorDispositionParent(ProgramSkillFrame f, scoped out ProgramCardTriggerWindowFrame window, scoped out PairedColorResponseLinkedEvent pair, scoped out int counterpart) => _resolve().ExactPairedColorDispositionParent(f, out window, out pair, out counterpart);
    internal PairedColorResponseLinkedEvent? ExactPairedColorResponseLink(ProgramCardTriggerWindowFrame window) => _resolve().ExactPairedColorResponseLink(window);
    internal SkillProgramStepOutcome OfferPairedColorCardDisposition(ProgramSkillFrame supplied, string stateId) => _resolve().OfferPairedColorCardDisposition(supplied, stateId);
    internal IReadOnlyList<PromptChoice> PairedColorDispositionChoices(ProgramSkillFrame f) => _resolve().PairedColorDispositionChoices(f);
    internal IEnumerable<Card> PairedColorLegalCards(int owner, int counterpart, bool obtain) => _resolve().PairedColorLegalCards(owner, counterpart, obtain);
    internal bool PairedColorResponseFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().PairedColorResponseFinalizedAfterCategoryTargetWindow(query);
    internal void PublishPairedColorDisposition(ProgramSkillFrame f) => _resolve().PublishPairedColorDisposition(f);
    internal void ResolvePairedColorDisposition(PromptChoice selected) => _resolve().ResolvePairedColorDisposition(selected);
    internal bool ResumePairedColorDisposition(long frameId) => _resolve().ResumePairedColorDisposition(frameId);
    internal bool ReturnPairedColorDispositionMovement(ProgramSkillFrame f) => _resolve().ReturnPairedColorDispositionMovement(f);
    internal PromptChoice SelectAiPairedColorDisposition(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiPairedColorDisposition(decision, f);
    internal bool TryGetPairedColorResponse(ProgramSkillWindowContext context, scoped out ProgramCardTriggerWindowFrame window, scoped out PairedColorResponseLinkedEvent pair, scoped out int counterpart) => _resolve().TryGetPairedColorResponse(context, out window, out pair, out counterpart);
    internal long PairedColorSequence { get => _resolve().PairedColorSequence; }
}
