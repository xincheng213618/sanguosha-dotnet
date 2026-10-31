#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LvQianProgramCapability(Func<RuntimeEntries.CharacterLvQianOlEntries> getCharacterLvQianOlRuntime) : ILvQianProgramHost
{
    public SkillProgramStepOutcome ExecuteLvQian(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLvQianOlRuntime().ExecuteLvQian(f, op);
}
