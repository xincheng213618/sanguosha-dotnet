#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AHuiNanCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterAHuiNanOlRuntime>> getCharacterAHuiNanOlRuntime) : IAHuiNanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterAHuiNanOlRuntime().Value.BeginAHuiNan(f);
}
