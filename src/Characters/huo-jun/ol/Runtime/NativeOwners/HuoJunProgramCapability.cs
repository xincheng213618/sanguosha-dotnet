#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HuoJunProgramCapability(Func<RuntimeEntries.CharacterHuoJunOlEntries> getCharacterHuoJunOlRuntime) : IHuoJunProgramHost
{
    public SkillProgramStepOutcome RestoreSlotAndUseMatchingEquipment(ProgramSkillFrame f) => getCharacterHuoJunOlRuntime().BeginHuoJunFenrui(f);
}
