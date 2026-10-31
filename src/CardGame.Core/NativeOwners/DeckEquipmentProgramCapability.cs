#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DeckEquipmentProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries> getCoreGameEngineDeckEquipmentProgramsRuntime) : IDeckEquipmentProgramHost
{
    public SkillProgramStepOutcome UseRandomDeckEquipment(ProgramSkillFrame f, int seat, string bind) => getCoreGameEngineDeckEquipmentProgramsRuntime().UseProgramRandomDeckEquipment(f, seat, bind);
    public void GrantRandomSkillAndSuitShield(ProgramSkillFrame f, int seat, IReadOnlyList<string> skills, Suit suit) => getCoreGameEngineDeckEquipmentProgramsRuntime().GrantProgramRandomSkillAndSuitShield(f, seat, skills, suit);
}
