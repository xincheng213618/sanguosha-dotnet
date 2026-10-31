#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterCaoCaoOlMagicCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICharacterCaoCaoOlMagicCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(MoCaoCaoFuzaiRuntime))]
internal sealed class CharacterCaoCaoOlMagicMoCaoCaoFuzaiAdapter(Func<MoCaoCaoFuzaiRuntime> targetFactory) : ICharacterCaoCaoOlMagicMoCaoCaoFuzaiOperations
{
    private readonly Func<MoCaoCaoFuzaiRuntime> _targetFactory = targetFactory;
    public MoCaoCaoFuzaiRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterCaoCaoOlMagicCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterCaoCaoOlMagicCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(MoCaoCaoBachaoRuntime))]
internal sealed class CharacterCaoCaoOlMagicMoCaoCaoBachaoAdapter(Func<MoCaoCaoBachaoRuntime> targetFactory) : ICharacterCaoCaoOlMagicMoCaoCaoBachaoOperations
{
    private readonly Func<MoCaoCaoBachaoRuntime> _targetFactory = targetFactory;
    public MoCaoCaoBachaoRuntime Get() => this._targetFactory();
}
