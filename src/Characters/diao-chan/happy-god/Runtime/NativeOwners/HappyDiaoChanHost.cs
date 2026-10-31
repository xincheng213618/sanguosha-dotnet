#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyDiaoChanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDiaoChanHappyGodEntries> getCharacterDiaoChanHappyGodRuntime) : IHappyDiaoChanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterDiaoChanHappyGodRuntime().BeginHappyDc(f, op);
}
