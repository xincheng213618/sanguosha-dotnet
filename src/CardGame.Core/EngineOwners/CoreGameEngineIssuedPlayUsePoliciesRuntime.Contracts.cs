#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreIssuedPlayUsePoliciesCharacterChenDaoMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileShiWanglieUnrespondable(long id);
}

internal interface ICoreIssuedPlayUsePoliciesCharacterMaChaoBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryMaChaoUnrespondable(long frameId);
}

internal interface ICoreIssuedPlayUsePoliciesCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCompletedResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCompletedResponseKind kind);
    bool IsCompletedResponseParentConsistent(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreIssuedPlayUsePoliciesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreIssuedPlayUsePoliciesCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQigongUnrespondable(long id);
}

internal interface ICoreIssuedPlayUsePoliciesCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhaJiangUnrespondable(long useId);
}

internal interface ICoreIssuedPlayUsePoliciesCharacterPanZhangMaZhongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryAnjianUnrespondable(long frameId);
}

internal interface ICoreIssuedPlayUsePoliciesCharacterWeiYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMobileShiZhuangshiUnrespondable(long id);
}

internal interface ICoreIssuedPlayUsePoliciesCharacterPangDeBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryPangDeUnrespondable(long frameId);
}

internal interface ICoreIssuedPlayUsePoliciesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreIssuedPlayUsePoliciesCoreNonBasicSlashProfileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedNonBasicSlashDistance(long? useId);
}

internal interface ICoreIssuedPlayUsePoliciesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreIssuedPlayUsePoliciesCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreIssuedPlayUsePoliciesCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreIssuedPlayUsePoliciesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreIssuedPlayUsePoliciesCharacterHuangZhongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryHuangZhongUnrespondable(long frameId);
}

internal interface ICoreIssuedPlayUsePoliciesCharacterHuangChongMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiHcSiliSlashUnrespondable(long frameId);
}

internal interface ICoreIssuedPlayUsePoliciesCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksActualPlayPhaseCardUses { get; }

    int GetActualPlayPhaseUseCount(int actorSeat);
}

internal interface ICoreIssuedPlayUsePoliciesCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhangYanLangdaoUnrespondable(long id);
}

internal interface ICoreIssuedPlayUsePoliciesCharacterZhangFeiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBoundaryZhangFeiLiyongUnrespondable(long frameId);
}
