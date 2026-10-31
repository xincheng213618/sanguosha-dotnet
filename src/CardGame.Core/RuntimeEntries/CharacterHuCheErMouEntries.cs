#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime))]
internal sealed partial class CharacterHuCheErMouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> _resolve;
    private CharacterHuCheErMouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuCheErMouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> resolve) => new(resolve);
}
