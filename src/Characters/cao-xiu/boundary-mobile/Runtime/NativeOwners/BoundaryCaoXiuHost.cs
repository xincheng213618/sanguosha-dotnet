#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoXiuHost(Func<RuntimeEntries.CharacterCaoXiuBoundaryMobileEntries> getCharacterCaoXiuBoundaryMobileRuntime) : IBoundaryCaoXiuHost
{
    public SkillProgramStepOutcome BeginQingxi(ProgramSkillFrame frame) => getCharacterCaoXiuBoundaryMobileRuntime().BeginBoundaryCaoXiuQingxi(frame);
}
