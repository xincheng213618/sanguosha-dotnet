#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryGuoHuaiCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGuoHuaiBoundaryMobileRuntime>> getCharacterGuoHuaiBoundaryMobileRuntime) : IBoundaryGuoHuaiHost
{
    public void Prepare(ProgramSkillFrame frame) => getCharacterGuoHuaiBoundaryMobileRuntime().Value.PrepareBoundaryGuoHuaiJingce(frame);
}
