#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HuoJunProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuoJunOlRuntime>> getCharacterHuoJunOlRuntime) : IHuoJunProgramHost
{
    public SkillProgramStepOutcome RestoreSlotAndUseMatchingEquipment(ProgramSkillFrame f) => getCharacterHuoJunOlRuntime().Value.BeginHuoJunFenrui(f);
}
