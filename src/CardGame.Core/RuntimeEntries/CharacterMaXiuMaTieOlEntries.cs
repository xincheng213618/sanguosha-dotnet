#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed partial class CharacterMaXiuMaTieOlEntries
{
    private readonly Func<EngineOwners.CharacterMaXiuMaTieOlRuntime> _resolve;
    private CharacterMaXiuMaTieOlEntries(Func<EngineOwners.CharacterMaXiuMaTieOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaXiuMaTieOlEntries Create(Func<EngineOwners.CharacterMaXiuMaTieOlRuntime> resolve) => new(resolve);
}
