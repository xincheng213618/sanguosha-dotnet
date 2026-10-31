#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlcoholSlashSuppressionsEntries
{
    internal bool CanRunAlcoholSlashSuppression(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanRunAlcoholSlashSuppression(trigger, context, ownerSeat);
    internal void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor) => _resolve().CaptureProgramAlcoholConsumption(cardUseFrameId, actor);
    internal void ExpireCurrentTurnOwnSkillSuppressions(int turnNumber, int turnOwnerSeat) => _resolve().ExpireCurrentTurnOwnSkillSuppressions(turnNumber, turnOwnerSeat);
    internal bool IsCurrentTurnOwnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsCurrentTurnOwnSkillGrantQualified(owner, grant);
    internal void SuppressOwnSkillAfterAlcoholSlashDamage(global::CardGame.Core.ProgramSkillFrame frame, string skillId) => _resolve().SuppressOwnSkillAfterAlcoholSlashDamage(frame, skillId);
    internal bool TryAlcoholSlashSuppressionUse(global::CardGame.Core.ProgramSkillWindowContext context, int ownerSeat, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().TryAlcoholSlashSuppressionUse(context, ownerSeat, out use);
}
