#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OrdinaryGuYongCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuYongOlEntries> getCharacterGuYongOlRuntime) : IOrdinaryGuYongHost
{
    public SkillProgramStepOutcome Bingyi(ProgramSkillFrame f) => getCharacterGuYongOlRuntime().BeginOrdinaryBingyi(f);
}
