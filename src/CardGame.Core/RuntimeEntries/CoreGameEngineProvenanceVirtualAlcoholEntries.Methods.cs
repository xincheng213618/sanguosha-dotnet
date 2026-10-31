#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProvenanceVirtualAlcoholEntries
{
    internal void AssertProvenanceAlcohol(ProgramSkillFrame f) => _resolve().AssertProvenanceAlcohol(f);
    internal SkillProgramStepOutcome BeginProvenanceVirtualAlcohol(ProgramSkillFrame input) => _resolve().BeginProvenanceVirtualAlcohol(input);
    internal bool CanStartProvenanceAlcohol(CharacterState owner) => _resolve().CanStartProvenanceAlcohol(owner);
    internal bool ResumeProvenanceAlcohol(long id) => _resolve().ResumeProvenanceAlcohol(id);
}
