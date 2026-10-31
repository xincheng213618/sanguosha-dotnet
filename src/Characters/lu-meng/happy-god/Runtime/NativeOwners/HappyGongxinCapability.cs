#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyGongxinCapability(Func<RuntimeEntries.CharacterLuMengHappyGodEntries> getCharacterLuMengHappyGodRuntime) : IHappyGongxinHost
{
    public void Bind(ProgramSkillFrame f) => getCharacterLuMengHappyGodRuntime().BindHappyGongxin(f);
}
