#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProvenanceVirtualAlcoholEntries
{
    internal void AssertProvenanceAlcohol(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertProvenanceAlcohol(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProvenanceVirtualAlcohol(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginProvenanceVirtualAlcohol(input);
    internal bool CanStartProvenanceAlcohol(global::CardGame.Core.CharacterState owner) => _resolve().CanStartProvenanceAlcohol(owner);
    internal bool IsExactProvenanceVirtualBasicDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramVirtualBasicDraft draft) => _resolve().IsExactProvenanceVirtualBasicDraft(f, draft);
    internal bool ResumeProvenanceAlcohol(long id) => _resolve().ResumeProvenanceAlcohol(id);
}
