#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHuangChengYanOlState : Contracts.ICharacterHuangChengYanOlJiezhenConversionsAccess
{
    // 解阵 conversions survive across turns until the owner's next turn start or
    // the converted character's 八卦阵 judgment; replay rebuilds the map while the
    // accepted commands re-execute the converting op.
    private Dictionary<int, GameEngine.JiezhenConversionState> _jiezhenConversions = [];
    internal ref readonly Dictionary<int, GameEngine.JiezhenConversionState> ReferenceToJiezhenConversions => ref _jiezhenConversions;

    ref readonly Dictionary<int, GameEngine.JiezhenConversionState> Contracts.ICharacterHuangChengYanOlJiezhenConversionsAccess._jiezhenConversions => ref _jiezhenConversions;
}
