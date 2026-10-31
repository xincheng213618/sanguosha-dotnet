#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime))]
internal sealed class CoreResponseUseCompletionCharacterZouShiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterZouShiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZouShiLeRuntime> _targetFactory = targetFactory;
    public bool HasLeZouShiCapability { get => this._targetFactory().HasLeZouShiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime))]
internal sealed class CoreResponseUseCompletionCoreWholeActualTurnUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreWholeActualTurnUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> _targetFactory = targetFactory;
    public bool TracksWholeActualTurnUseOrdinal { get => this._targetFactory().TracksWholeActualTurnUseOrdinal; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreResponseUseCompletionCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void ContinueFinishedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().ContinueFinishedCardResponse(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreResponseUseCompletionCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreResponseUseCompletionCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void ContinueNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending) => this._targetFactory().ContinueNullificationWindow(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreResponseUseCompletionCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime))]
internal sealed class CoreResponseUseCompletionCharacterCaoXiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterCaoXiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> _targetFactory = targetFactory;
    public void CompleteCaoXiNativeCounterspellBeforeUseCompleted(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().CompleteCaoXiNativeCounterspellBeforeUseCompleted(action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime))]
internal sealed class CoreResponseUseCompletionCharacterLuSuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterLuSuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsShiLsAutonomousResponse(global::CardGame.Core.CardActionContext a) => this._targetFactory().IsShiLsAutonomousResponse(a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime))]
internal sealed class CoreResponseUseCompletionCharacterMaMiDiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterMaMiDiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaMiDiOlRuntime> _targetFactory = targetFactory;
    public bool HasMaMiDiCapability { get => this._targetFactory().HasMaMiDiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreResponseUseCompletionCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardResponseCompleted(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation originalContinuation) => this._targetFactory().TryBeginCardResponseCompleted(attack, action, originalContinuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime))]
internal sealed class CoreResponseUseCompletionCharacterLiFengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterLiFengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> _targetFactory = targetFactory;
    public bool HasJiyunResponseCompletion(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasJiyunResponseCompletion(action, continuation);
    public bool HasLiFengCapability { get => this._targetFactory().HasLiFengCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime))]
internal sealed class CoreResponseUseCompletionCoreDelayedNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreDelayedNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedNullificationRuntime> _targetFactory = targetFactory;
    public bool HasNullificationEffectOwner(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().HasNullificationEffectOwner(window);
    public long? NullificationParentActionId(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().NullificationParentActionId(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreResponseUseCompletionCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool HasSiruiCapability { get => this._targetFactory().HasSiruiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed class CoreResponseUseCompletionCoreTieredRoundZeroResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreTieredRoundZeroResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame ClearTieredRoundCounterspellResponse(global::CardGame.Core.NullificationWindowFrame input) => this._targetFactory().ClearTieredRoundCounterspellResponse(input);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreResponseUseCompletionCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreResponseUseCompletionCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreResponseUseCompletionCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime))]
internal sealed class CoreResponseUseCompletionCharacterHuangGaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterHuangGaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _targetFactory = targetFactory;
    public void PrepareMouZhaJiangResponse(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardKind kind) => this._targetFactory().PrepareMouZhaJiangResponse(actor, ids, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime))]
internal sealed class CoreResponseUseCompletionCharacterWuAnGuoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterWuAnGuoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuAnGuoOlRuntime> _targetFactory = targetFactory;
    public bool HasWuAnGuoCapability { get => this._targetFactory().HasWuAnGuoCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime))]
internal sealed class CoreResponseUseCompletionCharacterLiuChengMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterLiuChengMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuChengMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouLiuChengSuspendedCounterspell(global::CardGame.Core.NullificationWindowFrame window) => this._targetFactory().IsMouLiuChengSuspendedCounterspell(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime))]
internal sealed class CoreResponseUseCompletionCharacterCuiYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterCuiYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> _targetFactory = targetFactory;
    public bool HasCuiYanCapability { get => this._targetFactory().HasCuiYanCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime))]
internal sealed class CoreResponseUseCompletionCoreCurrentTurnUseKindsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCoreCurrentTurnUseKindsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> _targetFactory = targetFactory;
    public bool TracksCurrentTurnUseKinds { get => this._targetFactory().TracksCurrentTurnUseKinds; }
    public bool TracksActualTurnUseOrdinal { get => this._targetFactory().TracksActualTurnUseOrdinal; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime))]
internal sealed class CoreResponseUseCompletionCharacterMaChaoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseUseCompletionCharacterMaChaoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool HasHengwuCapability { get => this._targetFactory().HasHengwuCapability; }
}
