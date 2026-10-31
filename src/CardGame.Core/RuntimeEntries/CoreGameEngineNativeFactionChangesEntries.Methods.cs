#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeFactionChangesEntries
{
    internal void ApplyCaoTengFactionChange(global::CardGame.Core.ProgramSkillFrame frame, string faction) => _resolve().ApplyCaoTengFactionChange(frame, faction);
    internal global::System.Collections.Generic.IReadOnlyList<string> AvailableEffectiveFactionIds() => _resolve().AvailableEffectiveFactionIds();
    internal string? CaptureNativeAvatarFactionBeforeDeclaration(global::CardGame.Core.CharacterState player) => _resolve().CaptureNativeAvatarFactionBeforeDeclaration(player);
    internal global::CardGame.Core.GameEngine.NativeFactionAvatar? CaptureNativeFactionAvatar(global::CardGame.Core.CharacterState player) => _resolve().CaptureNativeFactionAvatar(player);
    internal long CaptureNativeFactionQualificationStamp(long underlying) => _resolve().CaptureNativeFactionQualificationStamp(underlying);
    internal void ChangeNativeEffectiveFaction(global::CardGame.Core.ProgramSkillFrame frame, string faction) => _resolve().ChangeNativeEffectiveFaction(frame, faction);
    internal void CompleteNativeAvatarFactionDeclaration(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CharacterState player, string? previous) => _resolve().CompleteNativeAvatarFactionDeclaration(frame, player, previous);
    internal bool HasNativeEffectiveFactionDeclaration(global::CardGame.Core.CharacterState player) => _resolve().HasNativeEffectiveFactionDeclaration(player);
    internal void SynchronizeNativeEffectiveFactions() => _resolve().SynchronizeNativeEffectiveFactions();
    internal bool TracksNativeEffectiveFactionChanges { get => _resolve().TracksNativeEffectiveFactionChanges; }
}
