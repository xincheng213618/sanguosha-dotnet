#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryGuanYuNightmareHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuanYuAnniversaryGodEntries> getCharacterGuanYuAnniversaryGodRuntime) : IAnniversaryGuanYuNightmareHost
{
    public void RecordNightmare(ProgramSkillFrame frame) => getCharacterGuanYuAnniversaryGodRuntime().RecordAnniversaryGuanYuNightmare(frame);
}
