#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedCardAcquisitionsEntries
{
    internal void AssertNamedCardAcquisition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertNamedCardAcquisition(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginNamedCardAcquisition(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect e) => _resolve().BeginNamedCardAcquisition(supplied, e);
    internal global::CardGame.Core.Card? DeckNamedCard(global::CardGame.Core.CardKind kind) => _resolve().DeckNamedCard(kind);
    internal bool IsNamedCardAcquisitionMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsNamedCardAcquisitionMovement(f, e, pending);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NamedCardAcquisitionChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NamedCardAcquisitionChoices(f);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation From)> PublicNamedCardCandidates(global::CardGame.Core.CardKind kind) => _resolve().PublicNamedCardCandidates(kind);
    internal void PublishNamedCardAcquisition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishNamedCardAcquisition(f);
    internal void ResolveNamedCardAcquisitionChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveNamedCardAcquisitionChoice(selected);
    internal bool ResumeNamedCardAcquisition(long id) => _resolve().ResumeNamedCardAcquisition(id);
    internal bool ReturnNamedCardAcquisitionMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnNamedCardAcquisitionMovement(f);
    internal bool ValidNamedCardAcquisition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidNamedCardAcquisition(f);
}
