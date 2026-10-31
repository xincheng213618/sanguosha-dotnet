#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoPiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoPiBoundaryMobileRuntime>> getCharacterCaoPiBoundaryMobileRuntime) : IBoundaryCaoPiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e) => e.Op == SkillProgramEffectOp.ChooseBoundaryCaoPiXingshang ? getCharacterCaoPiBoundaryMobileRuntime().Value.BeginBoundaryXingshang(f, e) : getCharacterCaoPiBoundaryMobileRuntime().Value.BeginBoundaryFangzhu(f, e);
}
