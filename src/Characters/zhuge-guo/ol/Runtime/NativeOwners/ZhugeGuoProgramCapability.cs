#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhugeGuoProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhugeGuoOlRuntime>> getCharacterZhugeGuoOlRuntime) : IZhugeGuoProgramHost
{
    public SkillProgramStepOutcome BeginZhugeGuo(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterZhugeGuoOlRuntime().Value.BeginZhugeGuo(f, e);
}
