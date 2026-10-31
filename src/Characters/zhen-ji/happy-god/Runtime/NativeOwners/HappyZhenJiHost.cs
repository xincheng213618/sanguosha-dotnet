#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhenJiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhenJiHappyGodEntries> getCharacterZhenJiHappyGodRuntime) : IHappyZhenJiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhenJiHappyGodRuntime().BeginHappyZj(f, op);
}
