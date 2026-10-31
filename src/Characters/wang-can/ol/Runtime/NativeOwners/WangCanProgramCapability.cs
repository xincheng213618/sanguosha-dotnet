#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WangCanProgramCapability(Func<RuntimeEntries.CharacterWangCanOlEntries> getCharacterWangCanOlRuntime) : IWangCanProgramHost
{
    public SkillProgramStepOutcome ExecuteWangCan(SkillProgramEffectOp op, ProgramSkillFrame f) => getCharacterWangCanOlRuntime().ExecuteWangCan(op, f);
}
