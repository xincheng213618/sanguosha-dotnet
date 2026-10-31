#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiaoHuaMobileHost(Func<RuntimeEntries.CharacterLiaoHuaBoundaryMobileEntries> getCharacterLiaoHuaBoundaryMobileRuntime) : IBoundaryLiaoHuaMobileHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterLiaoHuaBoundaryMobileRuntime().BeginBoundaryLiaoHuaDiscardSlash(frame);
}
