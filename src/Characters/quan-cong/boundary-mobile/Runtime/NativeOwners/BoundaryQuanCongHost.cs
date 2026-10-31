#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryQuanCongHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterQuanCongBoundaryMobileRuntime>> getCharacterQuanCongBoundaryMobileRuntime) : IBoundaryQuanCongProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterQuanCongBoundaryMobileRuntime().Value.BeginBoundaryYaoming(frame);
}
