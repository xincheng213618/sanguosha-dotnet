#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime))]
internal sealed partial class CoreGameEngineMouLuMengEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> _resolve;
    private CoreGameEngineMouLuMengEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMouLuMengEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime> resolve) => new(resolve);
}
