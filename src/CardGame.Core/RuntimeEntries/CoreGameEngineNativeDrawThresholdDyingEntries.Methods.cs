#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDrawThresholdDyingEntries
{
    internal bool IsOriginalDyingSuspendedByNativeDrawThresholdDamage(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByNativeDrawThresholdDamage(dying);
    internal bool TryDescribeNativeDrawThresholdDyingDamage(global::CardGame.Core.ProgramSkillFrame frame, int target, int amount, global::CardGame.Core.ProgramParticipantReference? source, global::CardGame.Core.DamageNature? nature, bool sourceLess, int? explicitSource, scoped out global::CardGame.Core.NativeDrawThresholdDyingOrigin? origin) => _resolve().TryDescribeNativeDrawThresholdDyingDamage(frame, target, amount, source, nature, sourceLess, explicitSource, out origin);
}
