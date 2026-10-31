#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMuluEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheMuluRuntime> _resolve;
    private CoreGameEngineShanheMuluEntries(Func<EngineOwners.CoreGameEngineShanheMuluRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheMuluEntries Create(Func<EngineOwners.CoreGameEngineShanheMuluRuntime> resolve) => new(resolve);
}
