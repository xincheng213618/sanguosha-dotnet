#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWenyiDamageHealthRuntime))]
internal sealed partial class CoreGameEngineWenyiDamageHealthEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> _resolve;
    private CoreGameEngineWenyiDamageHealthEntries(Func<EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenyiDamageHealthEntries Create(Func<EngineOwners.CoreGameEngineWenyiDamageHealthRuntime> resolve) => new(resolve);
}
