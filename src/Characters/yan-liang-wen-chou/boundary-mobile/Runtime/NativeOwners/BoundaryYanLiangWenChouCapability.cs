#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryYanLiangWenChouCapability(Func<RuntimeEntries.CharacterYanLiangWenChouBoundaryMobileEntries> getCharacterYanLiangWenChouBoundaryMobileRuntime) : IBoundaryYanLiangWenChouHost
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame f) => getCharacterYanLiangWenChouBoundaryMobileRuntime().BeginBoundaryYanLiangWenChouClaim(f);
}
