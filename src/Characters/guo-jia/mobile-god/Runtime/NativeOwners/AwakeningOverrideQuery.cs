#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AwakeningOverrideQuery(Func<RuntimeEntries.CharacterGuoJiaMobileGodEntries> getCharacterGuoJiaMobileGodRuntime) : IProgramAwakeningOverrideQuery
{
    public bool Granted(int owner, string skill, string? binding = null) => getCharacterGuoJiaMobileGodRuntime().HasShenGuoJiaAwakeningCondition(owner, skill, binding);
    public bool Issued(ProgramSkillFrame frame) => getCharacterGuoJiaMobileGodRuntime().HasShenGuoJiaAwakeningReceipt(frame);
}
