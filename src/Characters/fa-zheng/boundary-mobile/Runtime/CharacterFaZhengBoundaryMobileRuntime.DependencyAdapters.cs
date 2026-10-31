#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime))]
internal sealed class CharacterFaZhengBoundaryMobileCorePrivateOfferReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterFaZhengBoundaryMobileCorePrivateOfferReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().GetPausedPrivateOfferEffect(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJiangWeiBoundaryMobileRuntime))]
internal sealed class CharacterFaZhengBoundaryMobileCharacterJiangWeiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterFaZhengBoundaryMobileCharacterJiangWeiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int[]? MobileJiangWeiAssistedTargets(global::CardGame.Core.ProgramSkillFrame f, int actor) => this._targetFactory().MobileJiangWeiAssistedTargets(f, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime))]
internal sealed class CharacterFaZhengBoundaryMobileCoreAssistedPhysicalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterFaZhengBoundaryMobileCoreAssistedPhysicalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime> _targetFactory = targetFactory;
    public int[] AssistedPhysicalSlashTargets(int actorSeat) => this._targetFactory().AssistedPhysicalSlashTargets(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CharacterFaZhengBoundaryMobileCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterFaZhengBoundaryMobileCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public bool IsWithinAttackRange(int actorSeat, int targetSeat) => this._targetFactory().IsWithinAttackRange(actorSeat, targetSeat);
}
