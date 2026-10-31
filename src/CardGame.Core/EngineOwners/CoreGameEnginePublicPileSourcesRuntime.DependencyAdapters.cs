#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime))]
internal sealed class CorePublicPileSourcesCoreRicePileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCoreRicePileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRicePileRuntime> _targetFactory = targetFactory;
    public bool RiceCapability { get => this._targetFactory().RiceCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime))]
internal sealed class CorePublicPileSourcesCharacterLiFengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterLiFengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> _targetFactory = targetFactory;
    public bool HasLiFengCapability { get => this._targetFactory().HasLiFengCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CorePublicPileSourcesCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool HasGuanHaiCapability { get => this._targetFactory().HasGuanHaiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileRuntime))]
internal sealed class CorePublicPileSourcesCharacterGanNingMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterGanNingMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouGanNingMobile { get => this._targetFactory().HasMouGanNingMobile; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouDunMouMobileRuntime))]
internal sealed class CorePublicPileSourcesCharacterXiahouDunMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterXiahouDunMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouXiahouDunCapability { get => this._targetFactory().HasMouXiahouDunCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime))]
internal sealed class CorePublicPileSourcesCharacterGongsunZanMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterGongsunZanMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouGszMobile { get => this._targetFactory().HasMouGszMobile; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime))]
internal sealed class CorePublicPileSourcesCoreSnapshotValueProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCoreSnapshotValueProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card) => this._targetFactory().ToSnapshot(card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime))]
internal sealed class CorePublicPileSourcesCharacterGuoHuaiMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterGuoHuaiMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouGuoHuaiMobile { get => this._targetFactory().HasMouGuoHuaiMobile; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime))]
internal sealed class CorePublicPileSourcesCharacterDengAiMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterDengAiMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime> _targetFactory = targetFactory;
    public bool HasMouDengAiOl { get => this._targetFactory().HasMouDengAiOl; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime))]
internal sealed class CorePublicPileSourcesCharacterSunShangxiangMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterSunShangxiangMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShangxiangMouMobileRuntime> _targetFactory = targetFactory;
    public bool HasMouSunShangxiangMobile { get => this._targetFactory().HasMouSunShangxiangMobile; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime))]
internal sealed class CorePublicPileSourcesCharacterZhenJiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterZhenJiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhenJiHappyGodRuntime> _targetFactory = targetFactory;
    public bool HasHappyZhenJi { get => this._targetFactory().HasHappyZhenJi; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CorePublicPileSourcesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXianOlRuntime))]
internal sealed class CorePublicPileSourcesCharacterCaoXianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePublicPileSourcesCharacterCaoXianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXianOlRuntime> _targetFactory = targetFactory;
    public bool HasCaoXian { get => this._targetFactory().HasCaoXian; }
}
