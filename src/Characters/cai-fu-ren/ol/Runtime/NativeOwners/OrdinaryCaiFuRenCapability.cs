#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OrdinaryCaiFuRenCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentDonationProgramsEntries> getCoreGameEngineEquipmentDonationProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaiFuRenOlEntries> getCharacterCaiFuRenOlRuntime) : IOrdinaryCaiFuRenHost
{
    public SkillProgramStepOutcome Xianzhou(ProgramSkillFrame f, string stateId) => getCoreGameEngineEquipmentDonationProgramsRuntime().RunAllEquipmentDonation(f, stateId);
    public SkillProgramStepOutcome Qieting(ProgramSkillFrame f) => getCharacterCaiFuRenOlRuntime().BeginOrdinaryQieting(f);
}
