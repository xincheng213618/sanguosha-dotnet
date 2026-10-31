#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoCaoMobileGodRuntime))]
internal sealed partial class CharacterCaoCaoMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoMobileGodRuntime> _resolve;
    private CharacterCaoCaoMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoCaoMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoCaoMobileGodRuntime> resolve) => new(resolve);
}
