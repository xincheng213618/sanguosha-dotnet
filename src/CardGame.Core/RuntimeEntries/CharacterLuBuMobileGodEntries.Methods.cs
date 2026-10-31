#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuBuMobileGodEntries
{
    internal IEnumerable<MobileLuBuWuqianIssuedEvent> ActiveMobileWuqian(int actor) => _resolve().ActiveMobileWuqian(actor);
    internal bool AllowsMobileLbDamageGain(ProgramTriggerLegalityContext g) => _resolve().AllowsMobileLbDamageGain(g);
    internal void AssertMobileLbUses() => _resolve().AssertMobileLbUses();
    internal SkillProgramStepOutcome BeginMobileLb(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMobileLb(supplied, op);
    internal void ContinueMobileLb(long id) => _resolve().ContinueMobileLb(id);
    internal bool HasMobileWuqianArmorDisabled(int targetSeat) => _resolve().HasMobileWuqianArmorDisabled(targetSeat);
    internal bool MobileLbHasDamageHand(CharacterState owner) => _resolve().MobileLbHasDamageHand(owner);
    internal CardConversionSource? MobileWumouSource(CharacterState actor, Card card) => _resolve().MobileWumouSource(actor, card);
    internal IReadOnlyList<int> MobileWumouTemplateTargets(CharacterState actor, Card card, LegalAction template) => _resolve().MobileWumouTemplateTargets(actor, card, template);
    internal IEnumerable<RuleQueryContribution> MobileWuqianRuleContributions(CharacterState player, SkillRuleQuery query) => _resolve().MobileWuqianRuleContributions(player, query);
    internal void ObserveMobileWuqianUses(IGameEvent payload) => _resolve().ObserveMobileWuqianUses(payload);
    internal IReadOnlyList<LegalAction> ProjectMobileWumouActions(CharacterState actor, IReadOnlyList<LegalAction> allActions, int? selectedPhysicalCardId) => _resolve().ProjectMobileWumouActions(actor, allActions, selectedPhysicalCardId);
    internal bool ResumeMobileLb(long id) => _resolve().ResumeMobileLb(id);
    internal bool RetainMobileLb(ProgramSkillFrame f) => _resolve().RetainMobileLb(f);
    internal bool ReturnMobileLbMovement(ProgramSkillFrame f) => _resolve().ReturnMobileLbMovement(f);
    internal bool TryResolveMobileWumouUse(CharacterState actor, LegalAction action) => _resolve().TryResolveMobileWumouUse(actor, action);
    internal void ValidateMobileLbUse(CardUseFrame use, MobileLuBuUseReceipt r) => _resolve().ValidateMobileLbUse(use, r);
    internal bool HasMobileWuqianCapability { get => _resolve().HasMobileWuqianCapability; }
}
