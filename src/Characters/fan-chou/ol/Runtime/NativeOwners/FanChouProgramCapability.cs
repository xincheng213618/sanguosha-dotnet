#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FanChouProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterFanChouOlRuntime>> getCharacterFanChouOlRuntime) : IFanChouProgramHost
{
    public SkillProgramStepOutcome BeginFanChou(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterFanChouOlRuntime().Value.BeginFanChou(f, e);
}
