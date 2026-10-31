#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OrdinaryGuYongCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGuYongOlRuntime>> getCharacterGuYongOlRuntime) : IOrdinaryGuYongHost
{
    public SkillProgramStepOutcome Bingyi(ProgramSkillFrame f) => getCharacterGuYongOlRuntime().Value.BeginOrdinaryBingyi(f);
}
