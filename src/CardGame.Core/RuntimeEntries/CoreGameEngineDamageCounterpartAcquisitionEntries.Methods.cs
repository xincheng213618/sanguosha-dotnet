#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageCounterpartAcquisitionEntries
{
    internal bool CanOfferDamageCounterpartAcquisition(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferDamageCounterpartAcquisition(candidate, trigger, context);
    internal bool CanOfferDamageCounterpartAcquisition2(int owner, SkillProgramTrigger trigger, GameEngine.IDamageAttempt attack) => _resolve().CanOfferDamageCounterpartAcquisition(owner, trigger, attack);
    internal bool HasDamageCounterpartAcquisition(int owner, string skillId, string stateId, int counterpart) => _resolve().HasDamageCounterpartAcquisition(owner, skillId, stateId, counterpart);
    internal SkillProgramStepOutcome RecordDamageCounterpartAcquisition(ProgramSkillFrame supplied, string stateId, string skillId) => _resolve().RecordDamageCounterpartAcquisition(supplied, stateId, skillId);
}
