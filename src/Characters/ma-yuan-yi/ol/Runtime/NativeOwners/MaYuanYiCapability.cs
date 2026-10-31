#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MaYuanYiCapability(Func<RuntimeEntries.CharacterMaYuanYiOlEntries> getCharacterMaYuanYiOlRuntime) : IMaYuanYiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaYuanYiOlRuntime().BeginMaYuanYi(f, op);
}
