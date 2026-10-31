#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterShiXieMobileEntries
{
    internal bool CanOfferMobileShiXie(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => _resolve().CanOfferMobileShiXie(candidate, t, c);
    internal SkillProgramStepOutcome ChangeMobileShiXieDistance(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ChangeMobileShiXieDistance(supplied, op);
    internal void IssueMobileShiXieDistance(ProgramSkillFrame f, int amount) => _resolve().IssueMobileShiXieDistance(f, amount);
    internal bool KeepMobileShiXieIssuedCourtesy(ProgramSkillFrame f) => _resolve().KeepMobileShiXieIssuedCourtesy(f);
    internal bool MobileShiXieCourtesyIssued(ProgramSkillFrame f) => _resolve().MobileShiXieCourtesyIssued(f);
    internal int MobileShiXieCourtesyRecipient(ProgramSkillFrame f) => _resolve().MobileShiXieCourtesyRecipient(f);
    internal bool MobileShiXieCourtesySelected(ProgramSkillFrame f) => _resolve().MobileShiXieCourtesySelected(f);
    internal bool MobileShiXieEndingParent(ProgramSkillFrame f) => _resolve().MobileShiXieEndingParent(f);
    internal IEnumerable<RuleQueryContribution> MobileShiXieIncomingDistance(int target) => _resolve().MobileShiXieIncomingDistance(target);
    internal void ReturnMobileShiXieCourtesyDraw(ProgramSkillFrame supplied, NativeDrawOutcome drawn) => _resolve().ReturnMobileShiXieCourtesyDraw(supplied, drawn);
    internal bool SettleMobileShiXieCourtesy(long id) => _resolve().SettleMobileShiXieCourtesy(id);
}
