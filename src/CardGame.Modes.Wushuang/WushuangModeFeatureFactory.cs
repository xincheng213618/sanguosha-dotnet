using CardGame.Core;

namespace CardGame.Modes.Wushuang;

/// <summary>Explicit factory installed by the single-duel content package.</summary>
public sealed class WushuangModeFeatureFactory : IGameModeFeatureFactory
{
    public string Id => "wushuang:mode-runtime";
    public string ModeId => WushuangMode.ModeId;
    public IGameModeFeature Create(ModeFeatureCreationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return new WushuangModeFeature(context.Seed, context.ContentRegistry, context.GetHost<IWushuangModeHost>());
    }
}
