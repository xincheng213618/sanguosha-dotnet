#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class CuiYanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCuiYanOlEntries> getCharacterCuiYanOlRuntime) : ICuiYanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCuiYanOlRuntime().BeginCuiYan(f, op);
}
