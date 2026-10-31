#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WangCanProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangCanOlEntries> getCharacterWangCanOlRuntime) : IWangCanProgramHost
{
    public SkillProgramStepOutcome ExecuteWangCan(SkillProgramEffectOp op, ProgramSkillFrame f) => getCharacterWangCanOlRuntime().ExecuteWangCan(op, f);
}
