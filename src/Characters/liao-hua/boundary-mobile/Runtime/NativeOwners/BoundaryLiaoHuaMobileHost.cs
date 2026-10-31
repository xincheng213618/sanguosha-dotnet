#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryLiaoHuaMobileHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiaoHuaBoundaryMobileRuntime>> getCharacterLiaoHuaBoundaryMobileRuntime) : IBoundaryLiaoHuaMobileHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp op) => getCharacterLiaoHuaBoundaryMobileRuntime().Value.BeginBoundaryLiaoHuaDiscardSlash(frame);
}
