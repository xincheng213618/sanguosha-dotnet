#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageCounterpartAcquisitionEntries
{
    internal bool CanOfferDamageCounterpartAcquisition(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDamageCounterpartAcquisition(candidate, trigger, context);
    internal bool CanOfferDamageCounterpartAcquisition2(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CanOfferDamageCounterpartAcquisition(owner, trigger, attack);
    internal bool HasDamageCounterpartAcquisition(int owner, string skillId, string stateId, int counterpart) => _resolve().HasDamageCounterpartAcquisition(owner, skillId, stateId, counterpart);
    internal global::CardGame.Core.SkillProgramStepOutcome RecordDamageCounterpartAcquisition(global::CardGame.Core.ProgramSkillFrame supplied, string stateId, string skillId) => _resolve().RecordDamageCounterpartAcquisition(supplied, stateId, skillId);
}
