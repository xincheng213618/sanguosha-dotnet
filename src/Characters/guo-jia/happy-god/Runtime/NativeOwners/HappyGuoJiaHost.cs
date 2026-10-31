#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyGuoJiaHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoJiaHappyGodEntries> getCharacterGuoJiaHappyGodRuntime) : IHappyGuoJiaHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterGuoJiaHappyGodRuntime().BeginHappyGuoJia(f, op);
}
