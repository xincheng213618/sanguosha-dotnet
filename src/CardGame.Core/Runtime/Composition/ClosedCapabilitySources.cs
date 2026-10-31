#nullable enable
namespace CardGame.Core.RuntimeAccess;
internal interface IClosedHandCardViewQueriesSource : IProgramRuntimeOperations
{
    IHandCardViewQueries Get();
}

internal sealed class ClosedHandCardViewQueriesSource(Func<IHandCardViewQueries> resolve) : IClosedHandCardViewQueriesSource
{
    public IHandCardViewQueries Get() => resolve();
}

internal interface IClosedMouYangshiRangeRuntimeSource : IProgramRuntimeOperations
{
    MouYangshiRangeRuntime Get();
}

[Core.ProgramCapability(typeof(MouYangshiRangeRuntime))]
internal sealed class ClosedMouYangshiRangeRuntimeSource(Func<MouYangshiRangeRuntime> resolve) : IClosedMouYangshiRangeRuntimeSource
{
    public MouYangshiRangeRuntime Get() => resolve();
}

internal interface IClosedNativeGivenCardQueriesSource : IProgramRuntimeOperations
{
    INativeGivenCardQueries Get();
}

internal sealed class ClosedNativeGivenCardQueriesSource(Func<INativeGivenCardQueries> resolve) : IClosedNativeGivenCardQueriesSource
{
    public INativeGivenCardQueries Get() => resolve();
}

internal interface IClosedOwnedCardMoveQueriesSource : IProgramRuntimeOperations
{
    IOwnedCardMoveQueries Get();
}

internal sealed class ClosedOwnedCardMoveQueriesSource(Func<IOwnedCardMoveQueries> resolve) : IClosedOwnedCardMoveQueriesSource
{
    public IOwnedCardMoveQueries Get() => resolve();
}

internal interface IClosedProgramOperationDrawsSource : IProgramRuntimeOperations
{
    IProgramOperationDraws Get();
}

internal sealed class ClosedProgramOperationDrawsSource(Func<IProgramOperationDraws> resolve) : IClosedProgramOperationDrawsSource
{
    public IProgramOperationDraws Get() => resolve();
}

internal interface IClosedProgramPhaseFrameQueriesSource : IProgramRuntimeOperations
{
    IProgramPhaseFrameQueries Get();
}

internal sealed class ClosedProgramPhaseFrameQueriesSource(Func<IProgramPhaseFrameQueries> resolve) : IClosedProgramPhaseFrameQueriesSource
{
    public IProgramPhaseFrameQueries Get() => resolve();
}

internal interface IClosedSaodiSeatingRuntimeSource : IProgramRuntimeOperations
{
    SaodiSeatingRuntime Get();
}

[Core.ProgramCapability(typeof(SaodiSeatingRuntime))]
internal sealed class ClosedSaodiSeatingRuntimeSource(Func<SaodiSeatingRuntime> resolve) : IClosedSaodiSeatingRuntimeSource
{
    public SaodiSeatingRuntime Get() => resolve();
}

internal interface IClosedSelectionParticipantQueriesSource : IProgramRuntimeOperations
{
    ISelectionParticipantQueries Get();
}

internal sealed class ClosedSelectionParticipantQueriesSource(Func<ISelectionParticipantQueries> resolve) : IClosedSelectionParticipantQueriesSource
{
    public ISelectionParticipantQueries Get() => resolve();
}

internal interface IClosedShuangjiaMarkRuntimeSource : IProgramRuntimeOperations
{
    ShuangjiaMarkRuntime Get();
}

[Core.ProgramCapability(typeof(ShuangjiaMarkRuntime))]
internal sealed class ClosedShuangjiaMarkRuntimeSource(Func<ShuangjiaMarkRuntime> resolve) : IClosedShuangjiaMarkRuntimeSource
{
    public ShuangjiaMarkRuntime Get() => resolve();
}

internal interface IClosedZhuitaoDistanceRuntimeSource : IProgramRuntimeOperations
{
    ZhuitaoDistanceRuntime Get();
}

[Core.ProgramCapability(typeof(ZhuitaoDistanceRuntime))]
internal sealed class ClosedZhuitaoDistanceRuntimeSource(Func<ZhuitaoDistanceRuntime> resolve) : IClosedZhuitaoDistanceRuntimeSource
{
    public ZhuitaoDistanceRuntime Get() => resolve();
}

internal static class ClosedCapabilityComposition
{
    internal static IEnumerable<ProgramNativeCapabilityBinding> Append(IEnumerable<ProgramNativeCapabilityBinding>? existing, ProgramRuntimeModuleCatalog modules, Func<IHandCardViewQueries> resolve0, Func<INativeGivenCardQueries> resolve1, Func<IOwnedCardMoveQueries> resolve2, Func<IProgramOperationDraws> resolve3, Func<IProgramPhaseFrameQueries> resolve4, Func<ISelectionParticipantQueries> resolve5)
    {
        foreach (var binding in existing ?? [])
            yield return binding;
        if (Enumerable.Any(modules.Registrations, registration => Enumerable.Contains(registration.Dependencies, typeof(IClosedHandCardViewQueriesSource))))
            yield return new(typeof(IClosedHandCardViewQueriesSource), new ClosedHandCardViewQueriesSource(resolve0));
        if (Enumerable.Any(modules.Registrations, registration => Enumerable.Contains(registration.Dependencies, typeof(IClosedNativeGivenCardQueriesSource))))
            yield return new(typeof(IClosedNativeGivenCardQueriesSource), new ClosedNativeGivenCardQueriesSource(resolve1));
        if (Enumerable.Any(modules.Registrations, registration => Enumerable.Contains(registration.Dependencies, typeof(IClosedOwnedCardMoveQueriesSource))))
            yield return new(typeof(IClosedOwnedCardMoveQueriesSource), new ClosedOwnedCardMoveQueriesSource(resolve2));
        if (Enumerable.Any(modules.Registrations, registration => Enumerable.Contains(registration.Dependencies, typeof(IClosedProgramOperationDrawsSource))))
            yield return new(typeof(IClosedProgramOperationDrawsSource), new ClosedProgramOperationDrawsSource(resolve3));
        if (Enumerable.Any(modules.Registrations, registration => Enumerable.Contains(registration.Dependencies, typeof(IClosedProgramPhaseFrameQueriesSource))))
            yield return new(typeof(IClosedProgramPhaseFrameQueriesSource), new ClosedProgramPhaseFrameQueriesSource(resolve4));
        if (Enumerable.Any(modules.Registrations, registration => Enumerable.Contains(registration.Dependencies, typeof(IClosedSelectionParticipantQueriesSource))))
            yield return new(typeof(IClosedSelectionParticipantQueriesSource), new ClosedSelectionParticipantQueriesSource(resolve5));
    }
}
