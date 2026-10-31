#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhouYuHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhouYuHappyGodRuntime>> getCharacterZhouYuHappyGodRuntime) : IHappyZhouYuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhouYuHappyGodRuntime().Value.BeginHappyZy(f);
}
