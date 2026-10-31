#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAppliedDamageReflectionEntries
{
    internal SkillProgramStepOutcome BeginAppliedDamageReflection(ProgramSkillFrame frame) => _resolve().BeginAppliedDamageReflection(frame);
}
