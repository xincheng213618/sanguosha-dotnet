#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlcoholSlashSuppressionsEntries
{
    internal bool CanRunAlcoholSlashSuppression(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanRunAlcoholSlashSuppression(trigger, context, ownerSeat);
    internal void CaptureProgramAlcoholConsumption(long cardUseFrameId, CharacterState actor) => _resolve().CaptureProgramAlcoholConsumption(cardUseFrameId, actor);
    internal void ExpireCurrentTurnOwnSkillSuppressions(int turnNumber, int turnOwnerSeat) => _resolve().ExpireCurrentTurnOwnSkillSuppressions(turnNumber, turnOwnerSeat);
    internal bool IsCurrentTurnOwnSkillGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsCurrentTurnOwnSkillGrantQualified(owner, grant);
    internal void SuppressOwnSkillAfterAlcoholSlashDamage(ProgramSkillFrame frame, string skillId) => _resolve().SuppressOwnSkillAfterAlcoholSlashDamage(frame, skillId);
    internal bool TryAlcoholSlashSuppressionUse(ProgramSkillWindowContext context, int ownerSeat, scoped out CardUseFrame use) => _resolve().TryAlcoholSlashSuppressionUse(context, ownerSeat, out use);
}
