#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LvQianProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries> getCharacterLvQianOlRuntime) : ILvQianProgramHost
{
    public SkillProgramStepOutcome ExecuteLvQian(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterLvQianOlRuntime().ExecuteLvQian(f, op);
}
