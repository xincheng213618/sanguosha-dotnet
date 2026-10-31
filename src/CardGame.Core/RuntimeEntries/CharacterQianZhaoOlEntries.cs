#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed partial class CharacterQianZhaoOlEntries
{
    private readonly Func<EngineOwners.CharacterQianZhaoOlRuntime> _resolve;
    private CharacterQianZhaoOlEntries(Func<EngineOwners.CharacterQianZhaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQianZhaoOlEntries Create(Func<EngineOwners.CharacterQianZhaoOlRuntime> resolve) => new(resolve);
}
