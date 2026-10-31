#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextActualTurnStartSkillGrantsEntries
{
    internal void DisableNextActualTurnStartSkillGrants(global::CardGame.Core.TurnStartedEvent started) => _resolve().DisableNextActualTurnStartSkillGrants(started);
    internal void ExpireNextActualTurnStartSkillGrants(global::CardGame.Core.TurnStartedEvent started) => _resolve().ExpireNextActualTurnStartSkillGrants(started);
    internal bool HasNextActualTurnStartSkillGrantExpired(global::CardGame.Core.SkillGrant grant) => _resolve().HasNextActualTurnStartSkillGrantExpired(grant);
    internal (global::CardGame.Core.CharacterState Recipient, global::CardGame.Core.SkillGrant Grant)[] NextActualTurnStartSkillGrantsDue(global::CardGame.Core.TurnStartedEvent started) => _resolve().NextActualTurnStartSkillGrantsDue(started);
}
