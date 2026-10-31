#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHuangChengYanOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterHuangChengYanOlJiezhenConversionsAccess
{
    // 解阵 conversions survive across turns until the owner's next turn start or
    // the converted character's 八卦阵 judgment; replay rebuilds the map while the
    // accepted commands re-execute the converting op.
    private global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.JiezhenConversionState> _jiezhenConversions = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.JiezhenConversionState> ReferenceToJiezhenConversions => ref _jiezhenConversions;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.JiezhenConversionState> global::CardGame.Core.EngineStates.Contracts.ICharacterHuangChengYanOlJiezhenConversionsAccess._jiezhenConversions => ref _jiezhenConversions;
}
