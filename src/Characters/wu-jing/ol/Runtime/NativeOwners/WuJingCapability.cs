#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuJingCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWuJingOlEntries> getCharacterWuJingOlRuntime) : IWuJingHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuJingOlRuntime().BeginWuJing(f, op);
}
