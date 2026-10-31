#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreWushuangModeProjectionRuntime))]
internal sealed class CoreWushuangCombatProgramsCoreWushuangModeProjectionAdapter(Func<CoreWushuangModeProjectionRuntime> targetFactory) : ICoreWushuangCombatProgramsCoreWushuangModeProjectionOperations
{
    private readonly Func<CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public bool IsWushuangMode { get => this._targetFactory().IsWushuangMode; }
}
