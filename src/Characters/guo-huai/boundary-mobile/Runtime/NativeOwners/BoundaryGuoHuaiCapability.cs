#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryGuoHuaiCapability(Func<RuntimeEntries.CharacterGuoHuaiBoundaryMobileEntries> getCharacterGuoHuaiBoundaryMobileRuntime) : IBoundaryGuoHuaiHost
{
    public void Prepare(ProgramSkillFrame frame) => getCharacterGuoHuaiBoundaryMobileRuntime().PrepareBoundaryGuoHuaiJingce(frame);
}
