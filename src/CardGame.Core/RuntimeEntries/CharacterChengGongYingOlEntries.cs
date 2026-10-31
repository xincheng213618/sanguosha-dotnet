#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime))]
internal sealed partial class CharacterChengGongYingOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> _resolve;
    private CharacterChengGongYingOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengGongYingOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> resolve) => new(resolve);
}
