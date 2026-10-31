#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreShenwuExpandedProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuExpandedProgramsRuntime> _resolve;
    private CoreShenwuExpandedProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuExpandedProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreShenwuExpandedProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreShenwuExpandedProgramsRuntime> resolve) => new(resolve);
}
