#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePassiveDamageCapsRuntime))]
internal sealed partial class CoreGameEnginePassiveDamageCapsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> _resolve;
    private CoreGameEnginePassiveDamageCapsEntries(Func<EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePassiveDamageCapsEntries Create(Func<EngineOwners.CoreGameEnginePassiveDamageCapsRuntime> resolve) => new(resolve);
}
