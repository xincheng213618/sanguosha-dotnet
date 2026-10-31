#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangShiPingOlRuntime))]
internal sealed partial class CharacterZhangShiPingOlEntries
{
    private readonly Func<EngineOwners.CharacterZhangShiPingOlRuntime> _resolve;
    private CharacterZhangShiPingOlEntries(Func<EngineOwners.CharacterZhangShiPingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangShiPingOlEntries Create(Func<EngineOwners.CharacterZhangShiPingOlRuntime> resolve) => new(resolve);
}
