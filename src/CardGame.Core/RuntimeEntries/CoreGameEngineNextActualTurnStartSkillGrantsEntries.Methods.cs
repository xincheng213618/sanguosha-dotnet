#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextActualTurnStartSkillGrantsEntries
{
    internal void DisableNextActualTurnStartSkillGrants(TurnStartedEvent started) => _resolve().DisableNextActualTurnStartSkillGrants(started);
    internal void ExpireNextActualTurnStartSkillGrants(TurnStartedEvent started) => _resolve().ExpireNextActualTurnStartSkillGrants(started);
    internal bool HasNextActualTurnStartSkillGrantExpired(SkillGrant grant) => _resolve().HasNextActualTurnStartSkillGrantExpired(grant);
    internal (CharacterState Recipient, SkillGrant Grant)[] NextActualTurnStartSkillGrantsDue(TurnStartedEvent started) => _resolve().NextActualTurnStartSkillGrantsDue(started);
}
