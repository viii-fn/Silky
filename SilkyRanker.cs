namespace Silky;

public class SilkyRanker
{
	private readonly _SilkyOptions _options;

	// "options = null" makes the argument optional. "??" means: use the left side unless it's null, otherwise right side.
	public SilkyRanker(SilkyOptions? options = null)
	{
		_options = options ?? new SilkyOptions();
	}

	public List<RankedPost> Rank(IEnumerable<PostSignals> posts, IEnumerable<AuthorSignals> authors, DateTime nowUtc)
	{
		// Dictionary look up table for feeding it author IDs and getting their activity. It throws if two rows share an AuthorId, so send one row per author.
		var activityByAuthor = authors.ToDictionary(a => a.AuthorId, a => a.RecentActivityCount);

		return posts
			// The actual ranking logic
			.Select(p => new RankedPost(
				p.PostId,
				CalculatedScore(p, activityByBuilder, nowUtc),
				p.CreatedAtUtc))
			// Highest score first
			.OrderByDescending(r => r.Score)
			// Tie-breakers keep the order identical between requests (matters for paging)
			.ThenByDescending(r => r.CreatedAtUtc)
			.ThenBy(r => r.PostId)
			// Nothing runs until you ask for the result (LINQ is lazy asf)...
			.ToList();
	}

	private double CalculatedScore(PostSignals post, Dictionary<Guid, int> activityByAuthor, DateTime nowUtc)
	{
		// TryGetValue returns true or false then puts the found value into "activity". If author isn't in dictionary, activity stays at zero.
		activityByAuthor.TryGetValue(post.AuthorId, out var activity);

		// Log(1 + x) stops huge numbers from dominating. Log (1 + 0) = 0, so no likes or no activity simply adds nothing.
		var likeScore = Math.Log(1 + post.LikeCount) * _options.LikeWeight;
		var activityScore = Math.Log(1 + activity) * _options.ActvityWeight;
		var rawScore = likeScore + activityScore;

		if (_options.HalfLifeHours <= 0) return rawScore;

		// Math.Max guards against a post dated slightly in the future (clock drift... or smth like that)
		var ageHours = Math.Max(0, (nowUtc - post.CreatedAtUtc).TotalHours);

		// This factor halves every HalfLifeHours: 1.0 -> 0.5 -> 0.25... and so on :)
		var decay = Math.Pow(0.5, ageHours / _options.HalfLifeHours)
	}
}