#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreShenwuExpandedProgramsEntries
{
    private readonly Func<EngineOwners.CoreShenwuExpandedProgramsRuntime> _resolve;
    private CoreShenwuExpandedProgramsEntries(Func<EngineOwners.CoreShenwuExpandedProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreShenwuExpandedProgramsEntries Create(Func<EngineOwners.CoreShenwuExpandedProgramsRuntime> resolve) => new(resolve);
}
