#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyLuSuCapability(Func<RuntimeEntries.CharacterLuSuHappyGodEntries> getCharacterLuSuHappyGodRuntime) : IHappyLuSuHost
{
    public SkillProgramStepOutcome BeginHappyDingzhou(ProgramSkillFrame frame) => getCharacterLuSuHappyGodRuntime().BeginHappyDingzhou(frame);
}
