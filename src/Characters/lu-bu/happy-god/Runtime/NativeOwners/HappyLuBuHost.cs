#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyLuBuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuBuHappyGodEntries> getCharacterLuBuHappyGodRuntime) : IHappyLuBuHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuBuHappyGodRuntime().BeginHappyLuBu(f, op);
}
