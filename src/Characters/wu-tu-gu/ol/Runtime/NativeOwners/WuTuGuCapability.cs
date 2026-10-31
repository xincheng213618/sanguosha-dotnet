#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuTuGuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWuTuGuOlEntries> getCharacterWuTuGuOlRuntime) : IWuTuGuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuTuGuOlRuntime().BeginWuTuGu(f, op);
}
