#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class YuanJiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYuanJiOlEntries> getCharacterYuanJiOlRuntime) : IYuanJiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYuanJiOlRuntime().ResolveYuanJi(f, op);
}
