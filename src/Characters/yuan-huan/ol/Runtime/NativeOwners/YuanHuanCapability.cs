#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class YuanHuanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYuanHuanOlEntries> getCharacterYuanHuanOlRuntime) : IYuanHuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYuanHuanOlRuntime().BeginYuanHuan(f, op);
}
