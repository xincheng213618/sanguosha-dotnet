#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeDrawThresholdDyingRuntime
{
    internal bool TryDescribeNativeDrawThresholdDyingDamage(ProgramSkillFrame frame, int target, int amount, ProgramParticipantReference? source, DamageNature? nature, bool sourceLess, int? explicitSource, out NativeDrawThresholdDyingOrigin? origin) => _nativeDrawThresholdOperations.Get().TryDescribeNativeDrawThresholdDyingDamage(frame, target, amount, source, nature, sourceLess, explicitSource, out origin);
    internal bool IsOriginalDyingSuspendedByNativeDrawThresholdDamage(DyingFrame dying) => _nativeDrawThresholdOperations.Get().IsOriginalDyingSuspendedByNativeDrawThresholdDamage(dying);
}
