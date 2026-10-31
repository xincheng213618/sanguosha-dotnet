#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryCaoPiHost(Func<RuntimeEntries.CharacterCaoPiBoundaryMobileEntries> getCharacterCaoPiBoundaryMobileRuntime) : IBoundaryCaoPiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e) => e.Op == SkillProgramEffectOp.ChooseBoundaryCaoPiXingshang ? getCharacterCaoPiBoundaryMobileRuntime().BeginBoundaryXingshang(f, e) : getCharacterCaoPiBoundaryMobileRuntime().BeginBoundaryFangzhu(f, e);
}
