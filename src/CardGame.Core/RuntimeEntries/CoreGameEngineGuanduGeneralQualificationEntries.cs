#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralQualificationRuntime))]
internal sealed partial class CoreGameEngineGuanduGeneralQualificationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralQualificationRuntime> _resolve;
    private CoreGameEngineGuanduGeneralQualificationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralQualificationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralQualificationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralQualificationRuntime> resolve) => new(resolve);
}
