#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiaoHuaMobileHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiaoHuaBoundaryMobileEntries> getCharacterLiaoHuaBoundaryMobileRuntime) : IBoundaryLiaoHuaMobileHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterLiaoHuaBoundaryMobileRuntime().BeginBoundaryLiaoHuaDiscardSlash(frame);
}
