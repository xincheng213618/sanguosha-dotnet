#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNianRevivalRuntime))]
internal sealed partial class CoreGameEngineNianRevivalEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNianRevivalRuntime> _resolve;
    private CoreGameEngineNianRevivalEntries(Func<EngineOwners.CoreGameEngineNianRevivalRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNianRevivalEntries Create(Func<EngineOwners.CoreGameEngineNianRevivalRuntime> resolve) => new(resolve);
}
