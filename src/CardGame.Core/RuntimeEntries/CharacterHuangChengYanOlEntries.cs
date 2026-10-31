#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangChengYanOlEntries
{
    private readonly Func<EngineOwners.CharacterHuangChengYanOlRuntime> _resolve;
    private CharacterHuangChengYanOlEntries(Func<EngineOwners.CharacterHuangChengYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangChengYanOlEntries Create(Func<EngineOwners.CharacterHuangChengYanOlRuntime> resolve) => new(resolve);
}
