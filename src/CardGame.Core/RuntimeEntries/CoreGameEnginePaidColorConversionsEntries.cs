#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePaidColorConversionsRuntime))]
internal sealed partial class CoreGameEnginePaidColorConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePaidColorConversionsRuntime> _resolve;
    private CoreGameEnginePaidColorConversionsEntries(Func<EngineOwners.CoreGameEnginePaidColorConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidColorConversionsEntries Create(Func<EngineOwners.CoreGameEnginePaidColorConversionsRuntime> resolve) => new(resolve);
}
