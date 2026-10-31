#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.CardsMovedWindowRuntime))]
internal sealed class CoreCardMovementProgramsCardsMovedWindowAdapter(global::System.Func<global::CardGame.Core.CardsMovedWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCardsMovedWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.CardsMovedWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardsMovedWindowRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.CardMovementBatchCoordinator))]
internal sealed class CoreCardMovementProgramsCardMovementBatchCoordinatorAdapter(global::System.Func<global::CardGame.Core.CardMovementBatchCoordinator> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCardMovementBatchCoordinatorOperations
{
    private readonly global::System.Func<global::CardGame.Core.CardMovementBatchCoordinator> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementBatchCoordinator Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramWindowCursorRuntime))]
internal sealed class CoreCardMovementProgramsProgramWindowCursorAdapter(global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsProgramWindowCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramWindowCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramWindowCursorRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime))]
internal sealed class CoreCardMovementProgramsCoreGainGiftProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCoreGainGiftProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> _targetFactory = targetFactory;
    public bool TracksRedOwnedLoss { get => this._targetFactory().TracksRedOwnedLoss; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramShanheSourceZhaoranRuntime))]
internal sealed class CoreCardMovementProgramsCoreProgramShanheSourceZhaoranAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceZhaoranRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCoreProgramShanheSourceZhaoranOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheSourceZhaoranRuntime> _targetFactory = targetFactory;
    public bool HasShanheSourceZhaoranRules { get => this._targetFactory().HasShanheSourceZhaoranRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFaZhengMouMobileRuntime))]
internal sealed class CoreCardMovementProgramsCharacterFaZhengMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFaZhengMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCharacterFaZhengMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFaZhengMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouFaZhengCapability { get => this._targetFactory().HasMouFaZhengCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime))]
internal sealed class CoreCardMovementProgramsCharacterZhouFeiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCharacterZhouFeiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> _targetFactory = targetFactory;
    public bool HasLeZhouFeiLingkongCapability { get => this._targetFactory().HasLeZhouFeiLingkongCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime))]
internal sealed class CoreCardMovementProgramsCharacterZhaoZhongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCharacterZhaoZhongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> _targetFactory = targetFactory;
    public bool HasZhaoZhongCapability { get => this._targetFactory().HasZhaoZhongCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSimaYiOlGod2016Runtime))]
internal sealed class CoreCardMovementProgramsCharacterSimaYiOlGod2016Adapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlGod2016Runtime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCharacterSimaYiOlGod2016Operations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSimaYiOlGod2016Runtime> _targetFactory = targetFactory;
    public bool HasOl2016SimaRenjie { get => this._targetFactory().HasOl2016SimaRenjie; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime))]
internal sealed class CoreCardMovementProgramsCoreActualHandGainProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCoreActualHandGainProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualHandGainProgramsRuntime> _targetFactory = targetFactory;
    public bool HasActualHandGainCapability { get => this._targetFactory().HasActualHandGainCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime))]
internal sealed class CoreCardMovementProgramsCharacterQiaoRuiAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCharacterQiaoRuiAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryQiaoRuiCapability { get => this._targetFactory().HasAnniversaryQiaoRuiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime))]
internal sealed class CoreCardMovementProgramsCoreJudgedRankSlashPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCoreJudgedRankSlashPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasOtherActualBasicDiscardCapability { get => this._targetFactory().HasOtherActualBasicDiscardCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime))]
internal sealed class CoreCardMovementProgramsCharacterHuCheErAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCharacterHuCheErAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> _targetFactory = targetFactory;
    public bool HasAnniversaryHuCheErRules { get => this._targetFactory().HasAnniversaryHuCheErRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheChanrangEdictRuntime))]
internal sealed class CoreCardMovementProgramsCoreShanheChanrangEdictAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheChanrangEdictRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCoreShanheChanrangEdictOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheChanrangEdictRuntime> _targetFactory = targetFactory;
    public bool HasShanheChanrangCapability { get => this._targetFactory().HasShanheChanrangCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreCardMovementProgramsCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardMovementProgramsCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public bool TryAdvanceWenhePoisonLoss(global::CardGame.Core.CardsMovedTriggerWindowFrame supplied) => this._targetFactory().TryAdvanceWenhePoisonLoss(supplied);
}
