#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LuYuShengCapability(Func<RuntimeEntries.CharacterLuYuShengOlEntries> getCharacterLuYuShengOlRuntime) : ILuYuShengHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLuYuShengOlRuntime().BeginLuYuSheng(f, op);
}
