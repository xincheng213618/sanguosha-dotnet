#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZuoCiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries> getCharacterZuoCiHappyGodRuntime) : IHappyZuoCiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZuoCiHappyGodRuntime().BeginHappyZuoCi(f, op);
}
