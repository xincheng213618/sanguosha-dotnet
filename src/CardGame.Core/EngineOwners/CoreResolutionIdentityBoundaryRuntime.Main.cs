#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreResolutionIdentityBoundaryRuntime
{
    internal long _resolutionSequence { get => _resolutionSchedulerOperations._resolutionScheduler.Identities.Value; set => _resolutionSchedulerOperations._resolutionScheduler.Identities.Value = value; }
}
