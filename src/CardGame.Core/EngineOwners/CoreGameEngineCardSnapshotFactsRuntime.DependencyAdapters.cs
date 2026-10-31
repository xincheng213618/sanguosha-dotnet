#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CardSkillMarkProjector))]
internal sealed class CoreCardSnapshotFactsCardSkillMarkProjectorAdapter(Func<CardSkillMarkProjector> targetFactory) : ICoreCardSnapshotFactsCardSkillMarkProjectorOperations
{
    private readonly Func<CardSkillMarkProjector> _targetFactory = targetFactory;
    public CardSkillMarkProjector Get() => this._targetFactory();
}
