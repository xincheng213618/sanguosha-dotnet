#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoZhangHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoZhangBoundaryMobileEntries> getCharacterCaoZhangBoundaryMobileRuntime) : IBoundaryCaoZhangHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterCaoZhangBoundaryMobileRuntime().ExecuteBoundaryCaoZhang(f, e);
}
