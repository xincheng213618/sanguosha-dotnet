#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AwakeningOverrideQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoJiaMobileGodEntries> getCharacterGuoJiaMobileGodRuntime) : IProgramAwakeningOverrideQuery
{
    public bool Granted(int owner, string skill, string? binding = null) => getCharacterGuoJiaMobileGodRuntime().HasShenGuoJiaAwakeningCondition(owner, skill, binding);
    public bool Issued(ProgramSkillFrame frame) => getCharacterGuoJiaMobileGodRuntime().HasShenGuoJiaAwakeningReceipt(frame);
}
