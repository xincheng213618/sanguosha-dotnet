#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePassiveDamageCapsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICorePassiveDamageCapsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICorePassiveDamageCapsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardAttackState GetCardAttackState(long id);
}

internal interface ICorePassiveDamageCapsCoreTeamActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.CardUseEffectSource Source, int Cap)? JiangeDamageCap(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICorePassiveDamageCapsCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.AttackAttemptState GetProgramAttackState(long ownerFrameId);
}

internal interface ICorePassiveDamageCapsCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}
