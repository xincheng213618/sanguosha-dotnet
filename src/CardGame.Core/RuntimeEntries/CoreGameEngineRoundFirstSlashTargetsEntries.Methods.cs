#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundFirstSlashTargetsEntries
{
    internal IEnumerable<RuleQueryContribution> RoundFirstSlashTargetContributions(CharacterState owner, CardKind kind, long? declaredUseFrameId = default) => _resolve().RoundFirstSlashTargetContributions(owner, kind, declaredUseFrameId);
}
