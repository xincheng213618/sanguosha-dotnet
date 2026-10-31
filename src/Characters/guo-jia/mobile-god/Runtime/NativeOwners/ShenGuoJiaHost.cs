#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenGuoJiaHost(Func<RuntimeEntries.CharacterGuoJiaMobileGodEntries> getCharacterGuoJiaMobileGodRuntime) : IShenGuoJiaHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterGuoJiaMobileGodRuntime().BeginShenGuoJia(frame, operation);
}
