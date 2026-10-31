#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterQuHuangOlCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICharacterQuHuangOlCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(QuHuangPhaseEndLedger))]
internal sealed class CharacterQuHuangOlQuHuangPhaseEndLedgerAdapter(Func<QuHuangPhaseEndLedger> targetFactory) : ICharacterQuHuangOlQuHuangPhaseEndLedgerOperations
{
    private readonly Func<QuHuangPhaseEndLedger> _targetFactory = targetFactory;
    public QuHuangPhaseEndLedger Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(QuHuangRuntime))]
internal sealed class CharacterQuHuangOlQuHuangAdapter(Func<QuHuangRuntime> targetFactory) : ICharacterQuHuangOlQuHuangOperations
{
    private readonly Func<QuHuangRuntime> _targetFactory = targetFactory;
    public QuHuangRuntime Get() => this._targetFactory();
}
