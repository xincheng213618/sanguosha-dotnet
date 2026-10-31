#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheXiashengRuntime))]
internal sealed partial class CoreGameEngineShanheXiashengEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheXiashengRuntime> _resolve;
    private CoreGameEngineShanheXiashengEntries(Func<EngineOwners.CoreGameEngineShanheXiashengRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheXiashengEntries Create(Func<EngineOwners.CoreGameEngineShanheXiashengRuntime> resolve) => new(resolve);
}
