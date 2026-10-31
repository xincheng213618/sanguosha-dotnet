#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterHuanShuJunOlCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICharacterHuanShuJunOlCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(CoreCommandSessionQueriesRuntime))]
internal sealed class CharacterHuanShuJunOlCoreCommandSessionQueriesAdapter(Func<CoreCommandSessionQueriesRuntime> targetFactory) : ICharacterHuanShuJunOlCoreCommandSessionQueriesOperations
{
    private readonly Func<CoreCommandSessionQueriesRuntime> _targetFactory = targetFactory;
    public long Revision { get => this._targetFactory().Revision; }
}
