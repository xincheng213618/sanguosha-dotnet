#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyLuSuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLuSuHappyGodRuntime>> getCharacterLuSuHappyGodRuntime) : IHappyLuSuHost
{
    public SkillProgramStepOutcome BeginHappyDingzhou(ProgramSkillFrame frame) => getCharacterLuSuHappyGodRuntime().Value.BeginHappyDingzhou(frame);
}
