#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangChengYanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangChengYanOlRuntime> _resolve;
    private CharacterHuangChengYanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangChengYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangChengYanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangChengYanOlRuntime> resolve) => new(resolve);
}
