#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryGuanYuNightmareHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGuanYuAnniversaryGodRuntime>> getCharacterGuanYuAnniversaryGodRuntime) : IAnniversaryGuanYuNightmareHost
{
    public void RecordNightmare(ProgramSkillFrame frame) => getCharacterGuanYuAnniversaryGodRuntime().Value.RecordAnniversaryGuanYuNightmare(frame);
}
