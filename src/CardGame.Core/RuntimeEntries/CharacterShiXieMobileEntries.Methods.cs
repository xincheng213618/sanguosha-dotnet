#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterShiXieMobileEntries
{
    internal bool CanOfferMobileShiXie(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferMobileShiXie(candidate, t, c);
    internal global::CardGame.Core.SkillProgramStepOutcome ChangeMobileShiXieDistance(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ChangeMobileShiXieDistance(supplied, op);
    internal void IssueMobileShiXieDistance(global::CardGame.Core.ProgramSkillFrame f, int amount) => _resolve().IssueMobileShiXieDistance(f, amount);
    internal bool KeepMobileShiXieIssuedCourtesy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMobileShiXieIssuedCourtesy(f);
    internal bool MobileShiXieCourtesyIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileShiXieCourtesyIssued(f);
    internal int MobileShiXieCourtesyRecipient(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileShiXieCourtesyRecipient(f);
    internal bool MobileShiXieCourtesySelected(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileShiXieCourtesySelected(f);
    internal bool MobileShiXieEndingParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileShiXieEndingParent(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> MobileShiXieIncomingDistance(int target) => _resolve().MobileShiXieIncomingDistance(target);
    internal void ReturnMobileShiXieCourtesyDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome drawn) => _resolve().ReturnMobileShiXieCourtesyDraw(supplied, drawn);
    internal bool SettleMobileShiXieCourtesy(long id) => _resolve().SettleMobileShiXieCourtesy(id);
}
