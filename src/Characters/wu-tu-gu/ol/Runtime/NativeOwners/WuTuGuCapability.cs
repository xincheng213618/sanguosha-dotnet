#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WuTuGuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterWuTuGuOlRuntime>> getCharacterWuTuGuOlRuntime) : IWuTuGuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterWuTuGuOlRuntime().Value.BeginWuTuGu(f, op);
}
