#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class YanPuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime) : IYanPuHost
{
    public SkillProgramStepOutcome ExecuteYanPu(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYanPuOlRuntime().BeginYanPu(f, op);
}
