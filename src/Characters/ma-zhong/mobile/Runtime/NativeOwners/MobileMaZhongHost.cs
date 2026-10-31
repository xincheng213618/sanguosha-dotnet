#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileMaZhongHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterMaZhongMobileRuntime>> getCharacterMaZhongMobileRuntime) : IMobileMaZhongHost
{
    public void Prepare(ProgramSkillFrame frame) => getCharacterMaZhongMobileRuntime().Value.PrepareMobileMaZhongFuman(frame);
}
