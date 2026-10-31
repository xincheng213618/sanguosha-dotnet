#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class YuanJiCapability(Func<RuntimeEntries.CharacterYuanJiOlEntries> getCharacterYuanJiOlRuntime) : IYuanJiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterYuanJiOlRuntime().ResolveYuanJi(f, op);
}
