#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryYanLiangWenChouCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYanLiangWenChouBoundaryMobileRuntime>> getCharacterYanLiangWenChouBoundaryMobileRuntime) : IBoundaryYanLiangWenChouHost
{
    public SkillProgramStepOutcome Claim(ProgramSkillFrame f) => getCharacterYanLiangWenChouBoundaryMobileRuntime().Value.BeginBoundaryYanLiangWenChouClaim(f);
}
