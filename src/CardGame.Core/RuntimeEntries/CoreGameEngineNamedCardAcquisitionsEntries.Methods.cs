#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedCardAcquisitionsEntries
{
    internal void AssertNamedCardAcquisition(ProgramSkillFrame f) => _resolve().AssertNamedCardAcquisition(f);
    internal SkillProgramStepOutcome BeginNamedCardAcquisition(ProgramSkillFrame supplied, SkillProgramEffect e) => _resolve().BeginNamedCardAcquisition(supplied, e);
    internal Card? DeckNamedCard(CardKind kind) => _resolve().DeckNamedCard(kind);
    internal IReadOnlyList<PromptChoice> NamedCardAcquisitionChoices(ProgramSkillFrame f) => _resolve().NamedCardAcquisitionChoices(f);
    internal IEnumerable<(Card Card, CardLocation From)> PublicNamedCardCandidates(CardKind kind) => _resolve().PublicNamedCardCandidates(kind);
    internal void PublishNamedCardAcquisition(ProgramSkillFrame f) => _resolve().PublishNamedCardAcquisition(f);
    internal void ResolveNamedCardAcquisitionChoice(PromptChoice selected) => _resolve().ResolveNamedCardAcquisitionChoice(selected);
    internal bool ResumeNamedCardAcquisition(long id) => _resolve().ResumeNamedCardAcquisition(id);
    internal bool ReturnNamedCardAcquisitionMovement(ProgramSkillFrame f) => _resolve().ReturnNamedCardAcquisitionMovement(f);
    internal bool ValidNamedCardAcquisition(ProgramSkillFrame f) => _resolve().ValidNamedCardAcquisition(f);
}
