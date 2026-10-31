#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterFaZhengBoundaryMobileCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterFaZhengBoundaryMobileCharacterJiangWeiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[]? MobileJiangWeiAssistedTargets(global::CardGame.Core.ProgramSkillFrame f, int actor);
}

internal interface ICharacterFaZhengBoundaryMobileCoreAssistedPhysicalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] AssistedPhysicalSlashTargets(int actorSeat);
}

internal interface ICharacterFaZhengBoundaryMobileContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterFaZhengBoundaryMobileCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}
