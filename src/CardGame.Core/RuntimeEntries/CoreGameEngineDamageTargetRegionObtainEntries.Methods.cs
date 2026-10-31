#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetRegionObtainEntries
{
    internal bool DamageTargetDuelBaseMatches(ProgramDamageTargetObtain receipt, ProgramDamageTargetDuelOrigin origin) => _resolve().DamageTargetDuelBaseMatches(receipt, origin);
    internal IReadOnlyList<PromptChoice> DamageTargetRegionObtainChoices(ProgramSkillFrame frame) => _resolve().DamageTargetRegionObtainChoices(frame);
    internal bool IsValidDamageTargetRegionObtain(ProgramSkillFrame frame, ProgramDamageTargetObtain draft) => _resolve().IsValidDamageTargetRegionObtain(frame, draft);
    internal void IssueDamageTargetRegionObtain(ProgramSkillFrame frame, PromptChoice choice) => _resolve().IssueDamageTargetRegionObtain(frame, choice);
    internal int PrepareDamageTargetDuelDamage(long useId, int targetSeat) => _resolve().PrepareDamageTargetDuelDamage(useId, targetSeat);
    internal PromptChoice SelectAiDamageTargetRegion(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiDamageTargetRegion(decision, frame);
    internal bool UsesDamageTargetRegionObtain(ProgramSkillFrame frame) => _resolve().UsesDamageTargetRegionObtain(frame);
}
