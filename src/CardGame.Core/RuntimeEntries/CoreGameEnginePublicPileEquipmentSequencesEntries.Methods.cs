#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileEquipmentSequencesEntries
{
    internal bool CanUseSequenceEquipment(ProgramSkillFrame f, int seat, Card card) => _resolve().CanUseSequenceEquipment(f, seat, card);
    internal void ContinueProgramAfterPileEquipmentUse(long completedUseId) => _resolve().ContinueProgramAfterPileEquipmentUse(completedUseId);
    internal IReadOnlyList<PromptChoice> DomainChoices(ProgramSkillFrame f) => _resolve().DomainChoices(f);
    internal IEnumerable<Card> DomainOwnedCards(ProgramSkillFrame f, int seat) => _resolve().DomainOwnedCards(f, seat);
    internal PublicPersistentPileSource? DomainReferencedPile(ProgramSkillFrame f, ProgramDomainCrossingDraft d) => _resolve().DomainReferencedPile(f, d);
    internal SkillProgramStepOutcome ExecuteGameDomain(SkillProgramEffect e, ProgramSkillFrame f) => _resolve().ExecuteGameDomain(e, f);
    internal void FinishGameDomain(ProgramSkillFrame f) => _resolve().FinishGameDomain(f);
    internal bool PileEquipmentFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().PileEquipmentFrameRidesOn(ride, beneath);
    internal void PublishDomainChoice(ProgramSkillFrame f) => _resolve().PublishDomainChoice(f);
    internal void ResolveGameDomainChoice(PromptChoice choice) => _resolve().ResolveGameDomainChoice(choice);
    internal bool ResumeGameDomain(long id) => _resolve().ResumeGameDomain(id);
    internal PromptChoice SelectAiDomainChoice(PendingDecision pending, ProgramSkillFrame f) => _resolve().SelectAiDomainChoice(pending, f);
    internal (string? Instance, CardLocation? Location) SelectDomainReference(ProgramSkillFrame f, string skill) => _resolve().SelectDomainReference(f, skill);
    internal PublicPersistentPileSource? SequencePile(ProgramSkillFrame f) => _resolve().SequencePile(f);
}
