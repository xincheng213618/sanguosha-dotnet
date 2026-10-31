#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyLuSuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuSuHappyGodEntries> getCharacterLuSuHappyGodRuntime) : IHappyLuSuHost
{
    public SkillProgramStepOutcome BeginHappyDingzhou(ProgramSkillFrame frame) => getCharacterLuSuHappyGodRuntime().BeginHappyDingzhou(frame);
}
