namespace Silky;

// Configure to your liking :)
public class SilkyOptions
{
	// How much each post's likes matter;
	public double LikeWeight { get; set; } = 1.0;

	// How much the author's recent activity matters
	public double ActivityWeight { get; set; } = 0.5;

	// After this many hours, a posts score is halved. 0 or less = no decay or half life lol.
	public double HalfLifeHours { get; set; } = 24;
}