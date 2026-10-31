#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetRegionObtainEntries
{
    internal bool DamageTargetDuelBaseMatches(global::CardGame.Core.ProgramDamageTargetObtain receipt, global::CardGame.Core.ProgramDamageTargetDuelOrigin origin) => _resolve().DamageTargetDuelBaseMatches(receipt, origin);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageTargetRegionObtainChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DamageTargetRegionObtainChoices(frame);
    internal bool IsExactDamageTargetRegionMovementChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardsMovedTriggerWindowFrame first) => _resolve().IsExactDamageTargetRegionMovementChild(frame, first);
    internal bool IsValidDamageTargetRegionObtain(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramDamageTargetObtain draft) => _resolve().IsValidDamageTargetRegionObtain(frame, draft);
    internal void IssueDamageTargetRegionObtain(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.PromptChoice choice) => _resolve().IssueDamageTargetRegionObtain(frame, choice);
    internal global::CardGame.Core.ProgramSkillFrame? PaidDamageTargetRegionObtainObserverRoot(long windowId) => _resolve().PaidDamageTargetRegionObtainObserverRoot(windowId);
    internal int PrepareDamageTargetDuelDamage(long useId, int targetSeat) => _resolve().PrepareDamageTargetDuelDamage(useId, targetSeat);
    internal global::CardGame.Core.PromptChoice SelectAiDamageTargetRegion(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiDamageTargetRegion(decision, frame);
    internal bool UsesDamageTargetRegionObtain(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().UsesDamageTargetRegionObtain(frame);
}
