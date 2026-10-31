#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDrawThresholdDyingEntries
{
    internal bool IsOriginalDyingSuspendedByNativeDrawThresholdDamage(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByNativeDrawThresholdDamage(dying);
    internal bool TryDescribeNativeDrawThresholdDyingDamage(ProgramSkillFrame frame, int target, int amount, ProgramParticipantReference? source, DamageNature? nature, bool sourceLess, int? explicitSource, scoped out NativeDrawThresholdDyingOrigin? origin) => _resolve().TryDescribeNativeDrawThresholdDyingDamage(frame, target, amount, source, nature, sourceLess, explicitSource, out origin);
}
