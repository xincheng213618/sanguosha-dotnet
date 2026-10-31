#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime))]
internal sealed partial class CharacterDongChengMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> _resolve;
    private CharacterDongChengMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongChengMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongChengMouRuntime> resolve) => new(resolve);
}
