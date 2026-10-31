#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class LiYiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiYiOlEntries> getCharacterLiYiOlRuntime) : ILiYiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLiYiOlRuntime().BeginChanshuang(f, op);
}
