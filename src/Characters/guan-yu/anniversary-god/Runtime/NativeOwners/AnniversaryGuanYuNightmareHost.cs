#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryGuanYuNightmareHost(Func<RuntimeEntries.CharacterGuanYuAnniversaryGodEntries> getCharacterGuanYuAnniversaryGodRuntime) : IAnniversaryGuanYuNightmareHost
{
    public void RecordNightmare(ProgramSkillFrame frame) => getCharacterGuanYuAnniversaryGodRuntime().RecordAnniversaryGuanYuNightmare(frame);
}
