#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePindianRevealWindowsRuntime))]
internal sealed partial class CoreGameEnginePindianRevealWindowsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> _resolve;
    private CoreGameEnginePindianRevealWindowsEntries(Func<EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianRevealWindowsEntries Create(Func<EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> resolve) => new(resolve);
}
