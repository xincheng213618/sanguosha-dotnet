#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhouYuHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouYuHappyGodEntries> getCharacterZhouYuHappyGodRuntime) : IHappyZhouYuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhouYuHappyGodRuntime().BeginHappyZy(f);
}
