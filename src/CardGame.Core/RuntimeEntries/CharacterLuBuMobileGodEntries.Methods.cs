#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuBuMobileGodEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MobileLuBuWuqianIssuedEvent> ActiveMobileWuqian(int actor) => _resolve().ActiveMobileWuqian(actor);
    internal bool AllowsMobileLbDamageGain(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().AllowsMobileLbDamageGain(g);
    internal void AssertMobileLbUses() => _resolve().AssertMobileLbUses();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileLb(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMobileLb(supplied, op);
    internal void ContinueMobileLb(long id) => _resolve().ContinueMobileLb(id);
    internal bool HasMobileWuqianArmorDisabled(int targetSeat) => _resolve().HasMobileWuqianArmorDisabled(targetSeat);
    internal bool MobileLbHasDamageHand(global::CardGame.Core.CharacterState owner) => _resolve().MobileLbHasDamageHand(owner);
    internal global::CardGame.Core.CardConversionSource? MobileWumouSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => _resolve().MobileWumouSource(actor, card);
    internal global::System.Collections.Generic.IReadOnlyList<int> MobileWumouTemplateTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction template) => _resolve().MobileWumouTemplateTargets(actor, card, template);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> MobileWuqianRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().MobileWuqianRuleContributions(player, query);
    internal void ObserveMobileWuqianUses(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMobileWuqianUses(payload);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> ProjectMobileWumouActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> allActions, int? selectedPhysicalCardId) => _resolve().ProjectMobileWumouActions(actor, allActions, selectedPhysicalCardId);
    internal bool ResumeMobileLb(long id) => _resolve().ResumeMobileLb(id);
    internal bool RetainMobileLb(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainMobileLb(f);
    internal bool ReturnMobileLbMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileLbMovement(f);
    internal bool TryResolveMobileWumouUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().TryResolveMobileWumouUse(actor, action);
    internal void ValidateMobileLbUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.MobileLuBuUseReceipt r) => _resolve().ValidateMobileLbUse(use, r);
    internal bool HasMobileWuqianCapability { get => _resolve().HasMobileWuqianCapability; }
}
