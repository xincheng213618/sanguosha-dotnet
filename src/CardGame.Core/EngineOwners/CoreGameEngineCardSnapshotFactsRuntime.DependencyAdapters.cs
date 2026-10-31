#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.CardSkillMarkProjector))]
internal sealed class CoreCardSnapshotFactsCardSkillMarkProjectorAdapter(global::System.Func<global::CardGame.Core.CardSkillMarkProjector> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardSnapshotFactsCardSkillMarkProjectorOperations
{
    private readonly global::System.Func<global::CardGame.Core.CardSkillMarkProjector> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSkillMarkProjector Get() => this._targetFactory();
}
