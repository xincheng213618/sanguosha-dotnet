#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardUseDebitsRuntime))]
internal sealed partial class CoreGameEngineCardUseDebitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardUseDebitsRuntime> _resolve;
    private CoreGameEngineCardUseDebitsEntries(Func<EngineOwners.CoreGameEngineCardUseDebitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUseDebitsEntries Create(Func<EngineOwners.CoreGameEngineCardUseDebitsRuntime> resolve) => new(resolve);
}
