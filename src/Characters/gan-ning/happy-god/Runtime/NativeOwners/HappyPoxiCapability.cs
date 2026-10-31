#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HappyPoxiCapability(Func<RuntimeEntries.CharacterGanNingHappyGodEntries> getCharacterGanNingHappyGodRuntime) : IHappyPoxiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, string bind) => getCharacterGanNingHappyGodRuntime().BeginHappyPoxi(frame, bind);
}
