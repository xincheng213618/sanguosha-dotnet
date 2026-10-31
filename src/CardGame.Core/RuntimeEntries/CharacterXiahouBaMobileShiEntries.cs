#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiahouBaMobileShiRuntime))]
internal sealed partial class CharacterXiahouBaMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterXiahouBaMobileShiRuntime> _resolve;
    private CharacterXiahouBaMobileShiEntries(Func<EngineOwners.CharacterXiahouBaMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouBaMobileShiEntries Create(Func<EngineOwners.CharacterXiahouBaMobileShiRuntime> resolve) => new(resolve);
}
