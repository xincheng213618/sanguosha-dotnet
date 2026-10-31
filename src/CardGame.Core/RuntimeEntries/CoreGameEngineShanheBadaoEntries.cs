#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheBadaoRuntime))]
internal sealed partial class CoreGameEngineShanheBadaoEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheBadaoRuntime> _resolve;
    private CoreGameEngineShanheBadaoEntries(Func<EngineOwners.CoreGameEngineShanheBadaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheBadaoEntries Create(Func<EngineOwners.CoreGameEngineShanheBadaoRuntime> resolve) => new(resolve);
}
