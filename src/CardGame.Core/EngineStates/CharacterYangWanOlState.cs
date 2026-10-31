#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterYangWanOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterYangWanOlZhuihuanArmChoicesAccess
{
    // Private 追还 arm choices: arm frame id → secretly chosen seat. The map is
    // runtime trusted state; replay rebuilds it from the accepted choice
    // commands exactly like the 闭境 marked-card map.
    private global::System.Collections.Generic.Dictionary<long, int> _zhuihuanArmChoices = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<long, int> ReferenceToZhuihuanArmChoices => ref _zhuihuanArmChoices;

    ref readonly global::System.Collections.Generic.Dictionary<long, int> global::CardGame.Core.EngineStates.Contracts.ICharacterYangWanOlZhuihuanArmChoicesAccess._zhuihuanArmChoices => ref _zhuihuanArmChoices;
}
