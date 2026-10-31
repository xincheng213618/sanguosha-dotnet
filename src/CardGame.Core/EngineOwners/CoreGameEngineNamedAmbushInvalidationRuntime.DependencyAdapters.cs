#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreNativeDamageApplicationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreNativeDamageApplicationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageApplicationRuntime> _targetFactory = targetFactory;
    public bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().ApplyAttackDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
    public void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update) => this._targetFactory().UpdateLifecycleCardUse(frameId, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> BuildNullificationCandidateSeats(int startAfterSeat) => this._targetFactory().BuildNullificationCandidateSeats(startAfterSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreNamedAmbushAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreNamedAmbushOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedAmbushRuntime> _targetFactory = targetFactory;
    public bool TryGetAmbushUse(global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.NamedAmbushUseIdentity identity, out global::CardGame.Core.CardActionContext action) => this._targetFactory().TryGetAmbushUse(context, out identity, out action);
    public bool HasNamedAmbushCapability { get => this._targetFactory().HasNamedAmbushCapability; }

    public global::CardGame.Core.NamedAmbushDeposit[] LiveAmbushDeposits(int owner, string? skill = default, string? instance = default) => this._targetFactory().LiveAmbushDeposits(owner, skill, instance);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreDrawFundedDistinctBasicResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreDrawFundedDistinctBasicResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> _targetFactory = targetFactory;
    public void ClearDrawFundedDistinctBasicDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ClearDrawFundedDistinctBasicDodgeResponse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime))]
internal sealed class CoreNamedAmbushInvalidationCharacterCaoXiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCharacterCaoXiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> _targetFactory = targetFactory;
    public void CaptureCaoXiNativeCounterspellOffset(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action, bool invalidated) => this._targetFactory().CaptureCaoXiNativeCounterspellOffset(pending, action, invalidated);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreRoundOnceCardlessNativeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreRoundOnceCardlessNativeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeResponsesRuntime> _targetFactory = targetFactory;
    public void ClearRoundOnceCardlessDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ClearRoundOnceCardlessDodgeResponse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public void ClearTieredRoundDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ClearTieredRoundDodgeResponse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime))]
internal sealed class CoreNamedAmbushInvalidationCharacterLiJueOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCharacterLiJueOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> _targetFactory = targetFactory;
    public void CaptureLiJueCompletedNullificationUse(long windowId, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureLiJueCompletedNullificationUse(windowId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime))]
internal sealed class CoreNamedAmbushInvalidationCharacterHuangZhongAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCharacterHuangZhongAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryHzUseIneffective(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsAnniversaryHzUseIneffective(use);
    public bool ContinueAnniversaryHzIneffectiveDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().ContinueAnniversaryHzIneffectiveDodge(attack, action, continuation);
    public bool HasAnniversaryHuangZhongCapability { get => this._targetFactory().HasAnniversaryHuangZhongCapability; }

    public bool AnniversaryHzCounterspellInvalidated(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action) => this._targetFactory().AnniversaryHzCounterspellInvalidated(pending, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreNamedAmbushInvalidationCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyWeiYanUseIneffective(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsHappyWeiYanUseIneffective(use);
    public bool ContinueHappyWeiYanIneffectiveDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().ContinueHappyWeiYanIneffectiveDodge(attack, action, continuation);
    public bool HasHappyWeiYanEzhiCapability { get => this._targetFactory().HasHappyWeiYanEzhiCapability; }

    public bool HappyWeiYanCounterspellInvalidated(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action) => this._targetFactory().HappyWeiYanCounterspellInvalidated(pending, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreNativeNullificationFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreNativeNullificationFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame ReplaceNullificationWindowFrame(global::CardGame.Core.NullificationWindowFrame next) => this._targetFactory().ReplaceNullificationWindowFrame(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime))]
internal sealed class CoreNamedAmbushInvalidationCharacterChengYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCharacterChengYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> _targetFactory = targetFactory;
    public bool IsMouCyUseIneffective(global::CardGame.Core.CardUseFrame use) => this._targetFactory().IsMouCyUseIneffective(use);
    public bool ContinueMouCyIneffectiveDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().ContinueMouCyIneffectiveDodge(attack, action, continuation);
    public bool HasMouChengYuAnniversary { get => this._targetFactory().HasMouChengYuAnniversary; }

    public bool MouCyCounterspellInvalidated(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action) => this._targetFactory().MouCyCounterspellInvalidated(pending, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreCompletedCategoryUseProofAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreCompletedCategoryUseProofOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedCategoryUseProofRuntime> _targetFactory = targetFactory;
    public bool HasExactAcceptedActualHandGainUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext completed) => this._targetFactory().HasExactAcceptedActualHandGainUse(use, completed);
    public bool RebuildAcceptedActualHandGainUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext accepted, global::CardGame.Core.CardActionContext completed, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int acceptedIndex) => this._targetFactory().RebuildAcceptedActualHandGainUse(use, accepted, completed, history, acceptedIndex);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreChainedStateBasicReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreChainedStateBasicReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicReturnsRuntime> _targetFactory = targetFactory;
    public void ClearChainedStateBasicDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ClearChainedStateBasicDodgeResponse(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreNamedAmbushInvalidationCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNamedAmbushInvalidationCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}
