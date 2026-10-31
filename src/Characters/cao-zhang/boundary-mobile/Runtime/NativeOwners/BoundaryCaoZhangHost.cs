#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoZhangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoZhangBoundaryMobileRuntime>> getCharacterCaoZhangBoundaryMobileRuntime) : IBoundaryCaoZhangHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterCaoZhangBoundaryMobileRuntime().Value.ExecuteBoundaryCaoZhang(f, e);
}
