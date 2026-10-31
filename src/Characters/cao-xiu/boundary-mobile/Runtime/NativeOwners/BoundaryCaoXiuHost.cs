#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoXiuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoXiuBoundaryMobileEntries> getCharacterCaoXiuBoundaryMobileRuntime) : IBoundaryCaoXiuHost
{
    public SkillProgramStepOutcome BeginQingxi(ProgramSkillFrame frame) => getCharacterCaoXiuBoundaryMobileRuntime().BeginBoundaryCaoXiuQingxi(frame);
}
