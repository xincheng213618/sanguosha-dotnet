#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryZhangZhaoMobileHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangZhaoZhangHongBoundaryMobileRuntime>> getCharacterZhangZhaoZhangHongBoundaryMobileRuntime) : IBoundaryZhangZhaoMobileHost
{
    public SkillProgramStepOutcome Restore(ProgramSkillFrame f) => getCharacterZhangZhaoZhangHongBoundaryMobileRuntime().Value.BeginBoundaryZhangZhaoPhase(f);
}
