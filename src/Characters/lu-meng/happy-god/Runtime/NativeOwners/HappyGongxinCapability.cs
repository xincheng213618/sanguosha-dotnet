#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyGongxinCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuMengHappyGodRuntime>> getCharacterLuMengHappyGodRuntime) : IHappyGongxinHost
{
    public void Bind(ProgramSkillFrame f) => getCharacterLuMengHappyGodRuntime().Value.BindHappyGongxin(f);
}
