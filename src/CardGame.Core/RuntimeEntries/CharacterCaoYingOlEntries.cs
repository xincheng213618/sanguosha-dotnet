#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoYingOlRuntime))]
internal sealed partial class CharacterCaoYingOlEntries
{
    private readonly Func<EngineOwners.CharacterCaoYingOlRuntime> _resolve;
    private CharacterCaoYingOlEntries(Func<EngineOwners.CharacterCaoYingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoYingOlEntries Create(Func<EngineOwners.CharacterCaoYingOlRuntime> resolve) => new(resolve);
}
