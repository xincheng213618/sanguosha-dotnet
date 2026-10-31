#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyZhouYuHost(Func<RuntimeEntries.CharacterZhouYuHappyGodEntries> getCharacterZhouYuHappyGodRuntime) : IHappyZhouYuHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterZhouYuHappyGodRuntime().BeginHappyZy(f);
}
