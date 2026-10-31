#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OrdinaryGuYongCapability(Func<RuntimeEntries.CharacterGuYongOlEntries> getCharacterGuYongOlRuntime) : IOrdinaryGuYongHost
{
    public SkillProgramStepOutcome Bingyi(ProgramSkillFrame f) => getCharacterGuYongOlRuntime().BeginOrdinaryBingyi(f);
}
