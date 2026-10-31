#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoCaoMobileGodRuntime))]
internal sealed partial class CharacterCaoCaoMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterCaoCaoMobileGodRuntime> _resolve;
    private CharacterCaoCaoMobileGodEntries(Func<EngineOwners.CharacterCaoCaoMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoCaoMobileGodEntries Create(Func<EngineOwners.CharacterCaoCaoMobileGodRuntime> resolve) => new(resolve);
}
