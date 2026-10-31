#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime))]
internal sealed partial class CharacterXiahouBaMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime> _resolve;
    private CharacterXiahouBaMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouBaMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouBaMobileShiRuntime> resolve) => new(resolve);
}
