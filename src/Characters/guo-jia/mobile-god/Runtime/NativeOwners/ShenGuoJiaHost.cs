#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShenGuoJiaHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoJiaMobileGodEntries> getCharacterGuoJiaMobileGodRuntime) : IShenGuoJiaHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterGuoJiaMobileGodRuntime().BeginShenGuoJia(frame, operation);
}
