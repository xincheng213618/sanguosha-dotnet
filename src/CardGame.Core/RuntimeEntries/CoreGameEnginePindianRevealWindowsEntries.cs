#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime))]
internal sealed partial class CoreGameEnginePindianRevealWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> _resolve;
    private CoreGameEnginePindianRevealWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianRevealWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRevealWindowsRuntime> resolve) => new(resolve);
}
