#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSayingTieredSuppliesEntries
{
    internal void ResolveSayingTieredSlash(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveSayingTieredSlash(selected);
    internal bool SayingEquipmentSlotPaymentRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SayingEquipmentSlotPaymentRidesOn(f, child);
    internal bool SayingEquipmentSlotUseStillLegal(global::CardGame.Core.CharacterState actor, global::CardGame.Core.EquipmentSlotNamedUseDraft draft) => _resolve().SayingEquipmentSlotUseStillLegal(actor, draft);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SayingTieredProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SayingTieredProgramSlashChoices(f);
}
