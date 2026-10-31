#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OrdinaryCaiFuRenCapability(Func<RuntimeEntries.CoreGameEngineEquipmentDonationProgramsEntries> getCoreGameEngineEquipmentDonationProgramsRuntime, Func<RuntimeEntries.CharacterCaiFuRenOlEntries> getCharacterCaiFuRenOlRuntime) : IOrdinaryCaiFuRenHost
{
    public SkillProgramStepOutcome Xianzhou(ProgramSkillFrame f, string stateId) => getCoreGameEngineEquipmentDonationProgramsRuntime().RunAllEquipmentDonation(f, stateId);
    public SkillProgramStepOutcome Qieting(ProgramSkillFrame f) => getCharacterCaiFuRenOlRuntime().BeginOrdinaryQieting(f);
}
