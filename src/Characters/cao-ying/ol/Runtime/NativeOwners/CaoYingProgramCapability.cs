#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CaoYingProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoYingOlRuntime>> getCharacterCaoYingOlRuntime) : ICaoYingProgramHost
{
    public SkillProgramStepOutcome BeginCaoYing(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterCaoYingOlRuntime().Value.BeginCaoYing(f, e);
}
