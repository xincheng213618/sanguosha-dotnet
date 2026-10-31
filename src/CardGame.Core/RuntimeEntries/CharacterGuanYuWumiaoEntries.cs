#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed partial class CharacterGuanYuWumiaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _resolve;
    private CharacterGuanYuWumiaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuanYuWumiaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> resolve) => new(resolve);
}
