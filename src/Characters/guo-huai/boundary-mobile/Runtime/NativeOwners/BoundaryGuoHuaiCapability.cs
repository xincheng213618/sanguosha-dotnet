#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryGuoHuaiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoHuaiBoundaryMobileEntries> getCharacterGuoHuaiBoundaryMobileRuntime) : IBoundaryGuoHuaiHost
{
    public void Prepare(ProgramSkillFrame frame) => getCharacterGuoHuaiBoundaryMobileRuntime().PrepareBoundaryGuoHuaiJingce(frame);
}
