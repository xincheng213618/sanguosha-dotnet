#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedColorResponsesEntries
{
    internal global::CardGame.Core.PairedColorResponseLinkedEvent? BuildPairedColorResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().BuildPairedColorResponseLink(window);
    internal bool CanContinuePairedColorDisposition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePairedColorDisposition(f);
    internal bool CanOfferPairedColorDisposition(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferPairedColorDisposition(candidate, trigger, context);
    internal void CapturePairedColorResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().CapturePairedColorResponseLink(window);
    internal void CompletePairedColorDisposition(global::CardGame.Core.ProgramSkillFrame f, bool paid) => _resolve().CompletePairedColorDisposition(f, paid);
    internal bool DrainPairedColorDispositionChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainPairedColorDispositionChildren(f);
    internal bool ExactPairedColorDispositionParent(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window, scoped out global::CardGame.Core.PairedColorResponseLinkedEvent pair, scoped out int counterpart) => _resolve().ExactPairedColorDispositionParent(f, out window, out pair, out counterpart);
    internal global::CardGame.Core.PairedColorResponseLinkedEvent? ExactPairedColorResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().ExactPairedColorResponseLink(window);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferPairedColorCardDisposition(global::CardGame.Core.ProgramSkillFrame supplied, string stateId) => _resolve().OfferPairedColorCardDisposition(supplied, stateId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PairedColorDispositionChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PairedColorDispositionChoices(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> PairedColorLegalCards(int owner, int counterpart, bool obtain) => _resolve().PairedColorLegalCards(owner, counterpart, obtain);
    internal bool PairedColorResponseFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().PairedColorResponseFinalizedAfterCategoryTargetWindow(query);
    internal void PublishPairedColorDisposition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPairedColorDisposition(f);
    internal void ResolvePairedColorDisposition(global::CardGame.Core.PromptChoice selected) => _resolve().ResolvePairedColorDisposition(selected);
    internal bool ResumePairedColorDisposition(long frameId) => _resolve().ResumePairedColorDisposition(frameId);
    internal bool ReturnPairedColorDispositionMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPairedColorDispositionMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiPairedColorDisposition(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiPairedColorDisposition(decision, f);
    internal bool TryGetPairedColorResponse(global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window, scoped out global::CardGame.Core.PairedColorResponseLinkedEvent pair, scoped out int counterpart) => _resolve().TryGetPairedColorResponse(context, out window, out pair, out counterpart);
    internal long PairedColorSequence { get => _resolve().PairedColorSequence; }
}
