#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyGongxinCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuMengHappyGodEntries> getCharacterLuMengHappyGodRuntime) : IHappyGongxinHost
{
    public void Bind(ProgramSkillFrame f) => getCharacterLuMengHappyGodRuntime().BindHappyGongxin(f);
}
