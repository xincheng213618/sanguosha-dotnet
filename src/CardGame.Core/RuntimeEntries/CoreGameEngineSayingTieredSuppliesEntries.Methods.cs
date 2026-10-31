#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSayingTieredSuppliesEntries
{
    internal void ResolveSayingTieredSlash(PromptChoice selected) => _resolve().ResolveSayingTieredSlash(selected);
    internal bool SayingEquipmentSlotPaymentRidesOn(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SayingEquipmentSlotPaymentRidesOn(f, child);
    internal bool SayingEquipmentSlotUseStillLegal(CharacterState actor, EquipmentSlotNamedUseDraft draft) => _resolve().SayingEquipmentSlotUseStillLegal(actor, draft);
    internal IReadOnlyList<PromptChoice> SayingTieredProgramSlashChoices(ProgramSkillFrame f) => _resolve().SayingTieredProgramSlashChoices(f);
}
