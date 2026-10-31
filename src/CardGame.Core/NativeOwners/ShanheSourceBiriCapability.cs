#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceBiriCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceBiriRuntime>> getCoreGameEngineShanheSourceBiriRuntime) : IShanheSourceBiriHost
{
    public SkillProgramStepOutcome PreventFireRecoverDraw(ProgramSkillFrame frame) => getCoreGameEngineShanheSourceBiriRuntime().Value.BeginShanheSourceBiri(frame);
}
