namespace CardGame.Core;

/// <summary>
/// Installs the mode's rule and policy profile independently of its setup algorithm.
/// An exact profile replaces only the wildcard profile for the selected mode.
/// </summary>
public interface IGameModeProfileFeatureFactory : IGameModeFeatureFactory { }
