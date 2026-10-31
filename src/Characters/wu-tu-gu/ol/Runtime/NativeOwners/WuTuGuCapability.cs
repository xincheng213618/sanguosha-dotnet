#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuTuGuCapability(Func<RuntimeEntries.CharacterWuTuGuOlEntries> getCharacterWuTuGuOlRuntime) : IWuTuGuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuTuGuOlRuntime().BeginWuTuGu(f, op);
}
