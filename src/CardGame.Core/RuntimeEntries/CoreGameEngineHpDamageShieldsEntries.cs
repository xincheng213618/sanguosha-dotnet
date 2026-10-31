#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHpDamageShieldsRuntime))]
internal sealed partial class CoreGameEngineHpDamageShieldsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHpDamageShieldsRuntime> _resolve;
    private CoreGameEngineHpDamageShieldsEntries(Func<EngineOwners.CoreGameEngineHpDamageShieldsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpDamageShieldsEntries Create(Func<EngineOwners.CoreGameEngineHpDamageShieldsRuntime> resolve) => new(resolve);
}
