#nullable enable
namespace CardGame.Core.RuntimeAccess;
internal interface IClosedHandCardViewQueriesSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IHandCardViewQueries Get();
}

internal sealed class ClosedHandCardViewQueriesSource(global::System.Func<global::CardGame.Core.IHandCardViewQueries> resolve) : IClosedHandCardViewQueriesSource
{
    public global::CardGame.Core.IHandCardViewQueries Get() => resolve();
}

internal interface IClosedMouYangshiRangeRuntimeSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouYangshiRangeRuntime Get();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouYangshiRangeRuntime))]
internal sealed class ClosedMouYangshiRangeRuntimeSource(global::System.Func<global::CardGame.Core.MouYangshiRangeRuntime> resolve) : IClosedMouYangshiRangeRuntimeSource
{
    public global::CardGame.Core.MouYangshiRangeRuntime Get() => resolve();
}

internal interface IClosedNativeGivenCardQueriesSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.INativeGivenCardQueries Get();
}

internal sealed class ClosedNativeGivenCardQueriesSource(global::System.Func<global::CardGame.Core.INativeGivenCardQueries> resolve) : IClosedNativeGivenCardQueriesSource
{
    public global::CardGame.Core.INativeGivenCardQueries Get() => resolve();
}

internal interface IClosedOwnedCardMoveQueriesSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IOwnedCardMoveQueries Get();
}

internal sealed class ClosedOwnedCardMoveQueriesSource(global::System.Func<global::CardGame.Core.IOwnedCardMoveQueries> resolve) : IClosedOwnedCardMoveQueriesSource
{
    public global::CardGame.Core.IOwnedCardMoveQueries Get() => resolve();
}

internal interface IClosedProgramOperationDrawsSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IProgramOperationDraws Get();
}

internal sealed class ClosedProgramOperationDrawsSource(global::System.Func<global::CardGame.Core.IProgramOperationDraws> resolve) : IClosedProgramOperationDrawsSource
{
    public global::CardGame.Core.IProgramOperationDraws Get() => resolve();
}

internal interface IClosedProgramPhaseFrameQueriesSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IProgramPhaseFrameQueries Get();
}

internal sealed class ClosedProgramPhaseFrameQueriesSource(global::System.Func<global::CardGame.Core.IProgramPhaseFrameQueries> resolve) : IClosedProgramPhaseFrameQueriesSource
{
    public global::CardGame.Core.IProgramPhaseFrameQueries Get() => resolve();
}

internal interface IClosedSaodiSeatingRuntimeSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SaodiSeatingRuntime Get();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.SaodiSeatingRuntime))]
internal sealed class ClosedSaodiSeatingRuntimeSource(global::System.Func<global::CardGame.Core.SaodiSeatingRuntime> resolve) : IClosedSaodiSeatingRuntimeSource
{
    public global::CardGame.Core.SaodiSeatingRuntime Get() => resolve();
}

internal interface IClosedSelectionParticipantQueriesSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ISelectionParticipantQueries Get();
}

internal sealed class ClosedSelectionParticipantQueriesSource(global::System.Func<global::CardGame.Core.ISelectionParticipantQueries> resolve) : IClosedSelectionParticipantQueriesSource
{
    public global::CardGame.Core.ISelectionParticipantQueries Get() => resolve();
}

internal interface IClosedShuangjiaMarkRuntimeSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ShuangjiaMarkRuntime Get();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ShuangjiaMarkRuntime))]
internal sealed class ClosedShuangjiaMarkRuntimeSource(global::System.Func<global::CardGame.Core.ShuangjiaMarkRuntime> resolve) : IClosedShuangjiaMarkRuntimeSource
{
    public global::CardGame.Core.ShuangjiaMarkRuntime Get() => resolve();
}

internal interface IClosedZhuitaoDistanceRuntimeSource : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ZhuitaoDistanceRuntime Get();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ZhuitaoDistanceRuntime))]
internal sealed class ClosedZhuitaoDistanceRuntimeSource(global::System.Func<global::CardGame.Core.ZhuitaoDistanceRuntime> resolve) : IClosedZhuitaoDistanceRuntimeSource
{
    public global::CardGame.Core.ZhuitaoDistanceRuntime Get() => resolve();
}

internal static class ClosedCapabilityComposition
{
    internal static global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramNativeCapabilityBinding> Append(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramNativeCapabilityBinding>? existing, global::CardGame.Core.ProgramRuntimeModuleCatalog modules, global::System.Func<global::CardGame.Core.IHandCardViewQueries> resolve0, global::System.Func<global::CardGame.Core.INativeGivenCardQueries> resolve1, global::System.Func<global::CardGame.Core.IOwnedCardMoveQueries> resolve2, global::System.Func<global::CardGame.Core.IProgramOperationDraws> resolve3, global::System.Func<global::CardGame.Core.IProgramPhaseFrameQueries> resolve4, global::System.Func<global::CardGame.Core.ISelectionParticipantQueries> resolve5)
    {
        foreach (var binding in existing ?? [])
            yield return binding;
        if (global::System.Linq.Enumerable.Any(modules.Registrations, registration => global::System.Linq.Enumerable.Contains(registration.Dependencies, typeof(IClosedHandCardViewQueriesSource))))
            yield return new(typeof(IClosedHandCardViewQueriesSource), new ClosedHandCardViewQueriesSource(resolve0));
        if (global::System.Linq.Enumerable.Any(modules.Registrations, registration => global::System.Linq.Enumerable.Contains(registration.Dependencies, typeof(IClosedNativeGivenCardQueriesSource))))
            yield return new(typeof(IClosedNativeGivenCardQueriesSource), new ClosedNativeGivenCardQueriesSource(resolve1));
        if (global::System.Linq.Enumerable.Any(modules.Registrations, registration => global::System.Linq.Enumerable.Contains(registration.Dependencies, typeof(IClosedOwnedCardMoveQueriesSource))))
            yield return new(typeof(IClosedOwnedCardMoveQueriesSource), new ClosedOwnedCardMoveQueriesSource(resolve2));
        if (global::System.Linq.Enumerable.Any(modules.Registrations, registration => global::System.Linq.Enumerable.Contains(registration.Dependencies, typeof(IClosedProgramOperationDrawsSource))))
            yield return new(typeof(IClosedProgramOperationDrawsSource), new ClosedProgramOperationDrawsSource(resolve3));
        if (global::System.Linq.Enumerable.Any(modules.Registrations, registration => global::System.Linq.Enumerable.Contains(registration.Dependencies, typeof(IClosedProgramPhaseFrameQueriesSource))))
            yield return new(typeof(IClosedProgramPhaseFrameQueriesSource), new ClosedProgramPhaseFrameQueriesSource(resolve4));
        if (global::System.Linq.Enumerable.Any(modules.Registrations, registration => global::System.Linq.Enumerable.Contains(registration.Dependencies, typeof(IClosedSelectionParticipantQueriesSource))))
            yield return new(typeof(IClosedSelectionParticipantQueriesSource), new ClosedSelectionParticipantQueriesSource(resolve5));
    }
}
