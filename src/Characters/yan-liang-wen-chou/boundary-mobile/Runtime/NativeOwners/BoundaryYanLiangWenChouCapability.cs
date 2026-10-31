#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryYanLiangWenChouCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanLiangWenChouBoundaryMobileEntries> getCharacterYanLiangWenChouBoundaryMobileRuntime) : IBoundaryYanLiangWenChouHost
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame f) => getCharacterYanLiangWenChouBoundaryMobileRuntime().BeginBoundaryYanLiangWenChouClaim(f);
}
