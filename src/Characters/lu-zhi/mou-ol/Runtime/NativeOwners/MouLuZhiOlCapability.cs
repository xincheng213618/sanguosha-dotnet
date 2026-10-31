#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouLuZhiOlCapability(Func<RuntimeEntries.CharacterLuZhiMouOlEntries> getCharacterLuZhiMouOlRuntime) : IMouLuZhiOlHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuZhiMouOlRuntime().BeginMouLz(f, op);
}
