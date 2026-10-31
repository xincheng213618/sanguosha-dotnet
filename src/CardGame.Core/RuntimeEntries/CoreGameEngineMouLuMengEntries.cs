#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineMouLuMengRuntime))]
internal sealed partial class CoreGameEngineMouLuMengEntries
{
    private readonly Func<EngineOwners.CoreGameEngineMouLuMengRuntime> _resolve;
    private CoreGameEngineMouLuMengEntries(Func<EngineOwners.CoreGameEngineMouLuMengRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMouLuMengEntries Create(Func<EngineOwners.CoreGameEngineMouLuMengRuntime> resolve) => new(resolve);
}
