#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HeJinCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHeJinOlEntries> getCharacterHeJinOlRuntime) : IHeJinProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouzhuHandDemandAndAssault ? getCharacterHeJinOlRuntime().BeginHeJinMouzhu(f) : getCharacterHeJinOlRuntime().BeginHeJinYanhuo(f);
}
