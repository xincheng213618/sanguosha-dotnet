#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterChenDaoMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterChenDaoMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenDaoMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsMobileShiWanglieUnrespondable(long id) => this._targetFactory().IsMobileShiWanglieUnrespondable(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoBoundaryMobileRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterMaChaoBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterMaChaoBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryMaChaoUnrespondable(long frameId) => this._targetFactory().IsBoundaryMaChaoUnrespondable(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCoreResponseUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCoreResponseUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _targetFactory = targetFactory;
    public bool IsCompletedResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCompletedResponseKind kind) => this._targetFactory().IsCompletedResponseUse(action, kind);
    public bool IsCompletedResponseParentConsistent(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().IsCompletedResponseParentConsistent(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterLvKuangLvXiangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterLvKuangLvXiangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime> _targetFactory = targetFactory;
    public bool IsQigongUnrespondable(long id) => this._targetFactory().IsQigongUnrespondable(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouZhaJiangUnrespondable(long useId) => this._targetFactory().IsMouZhaJiangUnrespondable(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterPanZhangMaZhongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterPanZhangMaZhongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryAnjianUnrespondable(long frameId) => this._targetFactory().IsBoundaryAnjianUnrespondable(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanMobileShiRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterWeiYanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterWeiYanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsMobileShiZhuangshiUnrespondable(long id) => this._targetFactory().IsMobileShiZhuangshiUnrespondable(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterPangDeBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterPangDeBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangDeBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryPangDeUnrespondable(long frameId) => this._targetFactory().IsBoundaryPangDeUnrespondable(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCoreNonBasicSlashProfileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCoreNonBasicSlashProfileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> _targetFactory = targetFactory;
    public bool HasIssuedNonBasicSlashDistance(long? useId) => this._targetFactory().HasIssuedNonBasicSlashDistance(useId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().HasCardPolicy(owner, kind, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterHuangZhongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterHuangZhongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryHuangZhongUnrespondable(long frameId) => this._targetFactory().IsBoundaryHuangZhongUnrespondable(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangChongMobileShiRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterHuangChongMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangChongMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterHuangChongMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangChongMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsShiHcSiliSlashUnrespondable(long frameId) => this._targetFactory().IsShiHcSiliSlashUnrespondable(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCoreActualPlayPhaseCardUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCoreActualPlayPhaseCardUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> _targetFactory = targetFactory;
    public bool TracksActualPlayPhaseCardUses { get => this._targetFactory().TracksActualPlayPhaseCardUses; }

    public int GetActualPlayPhaseUseCount(int actorSeat) => this._targetFactory().GetActualPlayPhaseUseCount(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterZhangYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterZhangYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _targetFactory = targetFactory;
    public bool IsZhangYanLangdaoUnrespondable(long id) => this._targetFactory().IsZhangYanLangdaoUnrespondable(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime))]
internal sealed class CoreIssuedPlayUsePoliciesCharacterZhangFeiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreIssuedPlayUsePoliciesCharacterZhangFeiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsBoundaryZhangFeiLiyongUnrespondable(long frameId) => this._targetFactory().IsBoundaryZhangFeiLiyongUnrespondable(frameId);
}
